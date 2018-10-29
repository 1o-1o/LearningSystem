﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using VTemplate.Engine;
using System.Data;

namespace Song.Site.Mobile
{
    /// <summary>
    /// 试题练习的章节列表展示
    /// </summary>
    public class QuesOutlines : BasePage
    {
        //课程ID，章节id
        protected int couid = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;
        //是否选学的当前课程
        bool isStudy = false;      
        protected override void InitPageTemplate(HttpContext context)
        {
            if (!Extend.LoginState.Accounts.IsLogin)
                this.Response.Redirect("login.ashx");
            
            //当前选中的课程
            Song.Entities.Course course = Extend.LoginState.Accounts.Course();            
            if (course != null)
            {
                //是否免费，或是限时免费
                if (course.Cou_IsLimitFree)
                {
                    DateTime freeEnd = course.Cou_FreeEnd.AddDays(1).Date;
                    if (!(course.Cou_FreeStart <= DateTime.Now && freeEnd >= DateTime.Now))
                        course.Cou_IsLimitFree = false;
                }
                //是否学习当前课程
                isStudy = Business.Do<ICourse>().StudyIsCourse(this.Account.Ac_ID, course.Cou_ID);
                ////是否可以学习,如果是免费或已经选修便可以学习，否则当前课程允许试用且当前章节是免费的，也可以学习
                //bool canStudy = isStudy || course.Cou_IsFree || course.Cou_IsLimitFree ? true : course.Cou_IsTry;
                //this.Document.Variables.SetValue("canStudy", canStudy);
                this.Document.SetValue("currCourse", course);
                couid = course.Cou_ID;                            
                //当前课程下的章节
                Song.Entities.Outline[] outlines = Business.Do<IOutline>().OutlineAll(couid, true);
                foreach (Song.Entities.Outline c in outlines)
                {
                    c.Ol_Intro = Extend.Html.ClearHTML(c.Ol_Intro);
                    //计算每个章节下的试题数
                    if (c.Ol_QuesCount <= 0)
                    {
                        c.Ol_QuesCount = Business.Do<IOutline>().QuesOfCount(c.Ol_ID, -1, true, true);
                        Business.Do<IOutline>().OutlineSave(c);
                    }
                }
                this.Document.SetValue("outlines", outlines);
                //树形章节输出
                this.Document.Variables.SetValue("dtOutlines", Business.Do<IOutline>().OutlineTree(outlines));  
            }
            this.Document.SetValue("couid", couid);  
            //课程资源、课程视频资源的所在的路径
            this.Document.SetValue("path", Upload.Get["Course"].Virtual);
            this.Document.SetValue("vpath", Upload.Get["CourseVideo"].Virtual);
            //试题练习记录
            Song.Entities.LogForStudentQuestions log = Business.Do<ILogs>().QuestionSingle(this.Account.Ac_ID, couid, 0);
            this.Document.SetValue("log", log);
            //是否拥有子级
            this.Document.RegisterGlobalFunction(this.isChildren);
            this.Document.RegisterGlobalFunction(this.getChildren);            
        }
        /// <summary>
        /// 是否拥有子级
        /// </summary>
        /// <returns>0为没有子级，其它有子级</returns>
        protected object isChildren(object[] id)
        {
            int pid = 0;
            if (id.Length > 0 && id[0] is int)
                pid = Convert.ToInt32(id[0]);
            bool isChilid = Business.Do<IOutline>().OutlineIsChildren(couid, pid, true);
            return isChilid ? 0 : 1;
        }
        /// <summary>
        /// 获取当前章节的子级章节
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected object getChildren(object[] id)
        {
            int pid = 0;
            if (id.Length > 0 && id[0] is int)
                pid = Convert.ToInt32(id[0]);
            return Business.Do<IOutline>().OutlineChildren(couid, pid, true, 0);
        }       
    }
}