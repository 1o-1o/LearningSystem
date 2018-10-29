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
    /// Notices 的摘要说明
    /// </summary>
    public class Course : BasePage
    {

        //课程章节
        Song.Entities.Outline[] outline;
        //是否购买当前课程
        bool isBuy = false;
        protected override void InitPageTemplate(HttpContext context)
        {
            //当前课程信息
            int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
            Song.Entities.Course cou = Business.Do<ICourse>().CourseSingle(id);
            if (cou == null) return;
            if ((WeiSha.Common.Request.Cookies["Course_" + cou.Cou_ID].Int32 ?? 0) == 0)
            {
                cou.Cou_ViewNum++;
                Business.Do<ICourse>().CourseSave(cou);
                context.Response.Cookies["Course_" + cou.Cou_ID].Value = cou.Cou_ID.ToString();
            }
            cou.Cou_Logo = Upload.Get["Course"].Virtual + cou.Cou_Logo;
            cou.Cou_LogoSmall = Upload.Get["Course"].Virtual + cou.Cou_LogoSmall;
            this.Document.Variables.SetValue("course", cou);
            //是否学习当前课程
            if (this.Account != null)
            {
                //是否购买
                isBuy = Business.Do<ICourse>().StudyIsCourse(this.Account.Ac_ID, cou.Cou_ID);
                this.Document.Variables.SetValue("isStudy", isBuy);
                //是否在试用中
                bool istry = Business.Do<ICourse>().IsTryout(cou.Cou_ID, this.Account.Ac_ID);
                this.Document.Variables.SetValue("isTry", istry);
            }
            //课程章节列表
            outline = Business.Do<IOutline>().OutlineAll(cou.Cou_ID, true);
            this.Document.Variables.SetValue("Outline", outline);
            //树形章节输出
            this.Document.Variables.SetValue("olTree", Business.Do<IOutline>().OutlineTree(outline));
            //课程公告
            Song.Entities.Guide[] guides = Business.Do<IGuide>().GuideCount(-1, cou.Cou_ID, -1, 20);
            this.Document.Variables.SetValue("guides", guides); 
            //当前课程的主讲老师
            Song.Entities.Teacher teacher = Business.Do<ITeacher>().TeacherSingle(cou.Th_ID);
            if (teacher != null)
            {
                teacher.Th_Photo = string.IsNullOrWhiteSpace(teacher.Th_Photo) ? null : Upload.Get["Teacher"].Virtual + teacher.Th_Photo;
                this.Document.Variables.SetValue("th", teacher);
                
            }
            //学习该课程的总人数，包括已经过期的
            int studyCount = Business.Do<ICourse>().CourseStudentSum(cou.Cou_ID, null);
            this.Document.Variables.SetValue("studyCount", studyCount);
        } 
    }
}