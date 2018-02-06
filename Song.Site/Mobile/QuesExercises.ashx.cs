﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using VTemplate.Engine;

namespace Song.Site.Mobile
{
    /// <summary>
    /// 试题练习
    /// </summary>
    public class QuesExercises : BasePage
    {
        //章节id
        protected int olid = WeiSha.Common.Request.QueryString["olid"].Int32 ?? 0;
        protected int couid = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;
        //试题的启始索引与当前取多少条记录
        protected int start = WeiSha.Common.Request.QueryString["start"].Int32 ?? 1;
        protected int size = WeiSha.Common.Request.QueryString["size"].Int32 ?? 100;
        //试题总记录数
        protected int sumCount = WeiSha.Common.Request.QueryString["sumcount"].Int32 ?? 0;
        //当前学员收藏的试题
        Song.Entities.Questions[] collectQues = null;
        protected override void InitPageTemplate(HttpContext context)
        {
            //索引启始
            this.Document.SetValue("start", start);
            //传递过来的试题总记录数
            //this.Document.SetValue("sumCount", sumCount);
            if (!Extend.LoginState.Accounts.IsLogin)
                this.Response.Redirect("login.ashx");
            //当前章节
            Song.Entities.Outline outline = Business.Do<IOutline>().OutlineSingle(olid);
            this.Document.SetValue("outline", outline);
            //当前课程
            Song.Entities.Course course = null;
            if (outline != null)
            {
                couid = outline.Cou_ID;
                course = Business.Do<ICourse>().CourseSingle(couid);
                this.Document.SetValue("course", course);
            }
            if (course == null && couid > 0) course = Business.Do<ICourse>().CourseSingle(couid);
            //是否购买该课程
            Song.Entities.Course couBuy = Business.Do<ICourse>().IsBuyCourse(couid, Extend.LoginState.Accounts.CurrentUser.Ac_ID, 1);
            bool isBuy = couBuy != null;
            //是否购买，如果免费也算已经购买
            this.Document.SetValue("isBuy", isBuy || course.Cou_IsFree);
            if (couBuy == null) couBuy = course;
            this.Document.SetValue("couBuy", couBuy);
            //试用的题数
            this.Document.SetValue("tryNum", course.Cou_TryNum);
            sumCount = isBuy || course.Cou_IsFree ? Business.Do<IQuestions>().QuesOfCount(-1, -1, couid, olid, -1, true) : course.Cou_TryNum;
            //this.Document.SetValue("sumCount", sumCount);
            int total = Business.Do<IQuestions>().QuesOfCount(-1, -1, couid, olid, -1, true);
            this.Document.SetValue("Total", total);  //试题的总数
            //题型
            this.Document.SetValue("quesType", WeiSha.Common.App.Get["QuesType"].Split(','));
            //试题列表
            string uid = string.Format("Questions：{0}-{1}-{2}-{3}", couid, olid, start, size);    //缓存的uid
            Song.Entities.Questions[] ques = Business.Do<IQuestions>().CacheQuestions(uid);
            if (ques == null)
            {
                ques = Business.Do<IQuestions>().QuesCount(-1, -1, couid, olid, -1, -1, true, start - 1, size);                
                for (int i = 0; i < ques.Length; i++)
                {
                    ques[i] = Extend.Questions.TranText(ques[i]);
                    ques[i].Qus_Title = ques[i].Qus_Title.Replace("&lt;", "<");
                    ques[i].Qus_Title = ques[i].Qus_Title.Replace("&gt;", ">");
                    ques[i].Qus_Title = Extend.Html.ClearHTML(ques[i].Qus_Title, "p", "div", "font");
                    ques[i].Qus_Title = ques[i].Qus_Title.Replace("\n", "<br/>");
                    if (!string.IsNullOrWhiteSpace(ques[i].Qus_Answer)) 
                        ques[i].Qus_Answer = ques[i].Qus_Answer.Replace("&nbsp;", " ");
                }
                uid = Business.Do<IQuestions>().CacheAdd(ques, 60*24, uid);
            }
            List<Song.Entities.Questions> list = new List<Entities.Questions>();
            if (isBuy || course.Cou_IsFree) list = ques.ToList<Song.Entities.Questions>();  //如果已经购买或免费；
            if (!(isBuy || course.Cou_IsFree))
            {
                for (int i = 0; i < sumCount; i++) list.Add(ques[i]);   //如果还在试用
            }
            this.Document.SetValue("uid", uid);
            this.Document.SetValue("ques", list);

            this.Document.RegisterGlobalFunction(this.AnswerItems);
            this.Document.RegisterGlobalFunction(this.GetAnswer);
            this.Document.RegisterGlobalFunction(this.GetOrder);
            this.Document.RegisterGlobalFunction(this.IsCollect);
        }
        /// <summary>
        /// 当前试题的选项，仅用于单选与多选
        /// </summary>
        /// <returns>0为没有子级，其它有子级</returns>
        protected object AnswerItems(object[] p)
        {
            Song.Entities.Questions qus = null;
            if (p.Length > 0)
                qus = (Song.Entities.Questions)p[0];
            //当前试题的答案
            Song.Entities.QuesAnswer[] ans = Business.Do<IQuestions>().QuestionsAnswer(qus, null);
            for (int i = 0; i < ans.Length; i++)
            {
                ans[i] = Extend.Questions.TranText(ans[i]);
                //ans[i].Ans_Context = ans[i].Ans_Context.Replace("<", "&lt;");
                //ans[i].Ans_Context = ans[i].Ans_Context.Replace(">", "&gt;");
            }
            return ans;
        }
        /// <summary>
        /// 试题的答案
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        protected object GetAnswer(object[] objs)
        {     
            //当前试题
            Song.Entities.Questions qus = (Song.Entities.Questions)objs[0];
            string ansStr = "";
            if (qus.Qus_Type == 1)
            {
                //当前试题的答案
                Song.Entities.QuesAnswer[] ans = Business.Do<IQuestions>().QuestionsAnswer(qus, null);
                for (int i = 0; i < ans.Length; i++)
                {
                    if (ans[i].Ans_IsCorrect)
                        ansStr += (char)(65 + i);
                }
            }
            if (qus.Qus_Type == 2)
            {
                Song.Entities.QuesAnswer[] ans = Business.Do<IQuestions>().QuestionsAnswer(qus, null);
                for (int i = 0; i < ans.Length; i++)
                {
                    if (ans[i].Ans_IsCorrect)
                        ansStr += (char)(65 + i) + "、";
                }
                ansStr = ansStr.Substring(0, ansStr.LastIndexOf("、"));
            }
            if (qus.Qus_Type == 3)
                ansStr = qus.Qus_IsCorrect ? "正确" : "错误";
            if (qus.Qus_Type == 4)
            {
                if (qus != null && !string.IsNullOrEmpty(qus.Qus_Answer))
                    ansStr = qus.Qus_Answer;
            }
            if (qus.Qus_Type == 5)
            {
                //当前试题的答案
                Song.Entities.QuesAnswer[] ans = Business.Do<IQuestions>().QuestionsAnswer(qus, null);
                for (int i = 0; i < ans.Length; i++)
                     ansStr += (char)(65 + i) + "、" + ans[i].Ans_Context + "<br/>";
            }
            return ansStr;
        }

        /// <summary>
        /// 试题是否被当前学员收藏
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        protected object IsCollect(object[] objs)
        {
            int qid = 0;
            if (objs.Length > 0)
                qid = Convert.ToInt32(objs[0]);
            //当前收藏            
            if (collectQues == null)
            {
                Song.Entities.Course currCourse = Extend.LoginState.Accounts.Course();
                if (Extend.LoginState.Accounts.IsLogin)
                {
                    Song.Entities.Accounts st = Extend.LoginState.Accounts.CurrentUser;
                    collectQues = Business.Do<IStudent>().CollectAll4Ques(st.Ac_ID, 0, currCourse.Cou_ID, 0);
                }
                else
                {
                    collectQues = Business.Do<IStudent>().CollectAll4Ques(0, 0, currCourse.Cou_ID, 0);
                }
            }
            if (collectQues != null)
            {
                foreach (Song.Entities.Questions q in collectQues)
                {
                    if (qid == q.Qus_ID) return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 获取序号
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected object GetOrder(object[] index)
        {
            int tax = 0;
            if (index.Length > 0)
                tax = Convert.ToInt32(index[0]);
            return (char)(tax - 1 + 65);
        }
    }
}