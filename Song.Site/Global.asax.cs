﻿using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
//using System.Data;
using System.Timers;
using WeiSha.Common;

using Song.ServiceInterfaces;
using Song.Entities;
using WeiSha.WebControl;
using System.Web.Mvc;
using System.Web.Routing;
using System.Threading;

namespace Song.Site
{
    public class Global : System.Web.HttpApplication
    {
        /// <summary>
        /// 在应用程序被实例化或第一次被调用时，该事件被触发。对于所有的HttpApplication 对象实例，它都会被调用。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Init(object sender, EventArgs e)
        {
            
        }
        /// <summary>
        /// 在HttpApplication 类的第一个实例被创建时，该事件被触发。它允许你创建可以由所有HttpApplication 实例访问的对象。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Start(object sender, EventArgs e)
        {
            //创建路由
            RegisterRoutes(RouteTable.Routes);

            //生成所有机构的二维码
            new Thread(new ThreadStart(() =>
            {
                Business.Do<IOrganization>().OrganBuildQrCode();
            })).Start();

            #region 章节事件
            //章节
            Business.Do<IOutline>().Save += delegate(object s, EventArgs ev)
           {
               if (s == null)
               {
                   WeiSha.Common.Cache<Song.Entities.Outline>.Data.Clear();
                   Business.Do<IOutline>().OutlineBuildCache();
               }
               else
               {
                   if (!(s is Outline)) return;
                   Outline ol = (Outline)s;
                   if (ol == null) return;
                   Song.Entities.Outline old = Business.Do<IOutline>().OutlineSingle(ol.Ol_ID);
                   ol.Ol_QuesCount = Business.Do<IOutline>().QuesOfCount(ol.Ol_ID, -1, true, true);
                   WeiSha.Common.Cache<Song.Entities.Outline>.Data.Update(old, ol);
               }
           };
            Business.Do<IOutline>().OnSave(null, EventArgs.Empty);
            #endregion

            #region  试题的事件
            //当试题内容变更时的事件
            Business.Do<IQuestions>().Save += delegate(object s, EventArgs ev)
            {
                if (s == null)
                {
                    //取所有试题进缓存
                    new Thread(new ThreadStart(() =>
                    {
                        Song.Entities.Questions[] ques = Business.Do<IQuestions>().QuesCount(-1, null, -1);
                        Song.ServiceImpls.QuestionsMethod.QuestionsCache.Singleton.Delete("all");
                        Song.ServiceImpls.QuestionsMethod.QuestionsCache.Singleton.Add(ques, int.MaxValue, "all");
                    })).Start();
                }
                else
                {
                    //单个试题的缓存刷新
                    if (!(s is Questions)) return;
                    Questions ques = (Questions)s;
                    if (ques == null) return;
                    Song.ServiceImpls.QuestionsMethod.QuestionsCache.Singleton.UpdateSingle(ques);
                }

            };
            Business.Do<IQuestions>().Add += delegate(object s, EventArgs ev)
            {
                Business.Do<IQuestions>().OnSave(s, ev);               
            };
            Business.Do<IQuestions>().OnSave(null, EventArgs.Empty);
            #endregion

            //当账号内容变更时，刷新当前登录对象
            Business.Do<IAccounts>().Save += delegate(object s, EventArgs ev)
            {
                if (!(s is Accounts)) return;
                Accounts acc = (Accounts)s;
                if (acc == null) return;
                int currid = Extend.LoginState.Accounts.CurrentUserId;
                if (currid != acc.Ac_ID) return;
                Extend.LoginState.Accounts.Refresh(currid);                
            };

        }    

        private void RegisterRoutes(RouteCollection routes){
            //伪静态页面，自动转到ashx动态页（ashx又自动取了/tempates/中的模板用于展示）
            routes.MapPageRoute(
                "web",
                "{term}.htm",
                "~/{term}.ashx"
            );
            //自定义页面（.cs为custom缩写，不是csharp哟），系统自动取/tempates/中的模板用于展示
            routes.MapPageRoute(
                "custom",
                "{term}.cshtm",
                "~/TemplatePage.ashx",
                false
             );
            //************************************
            //同上，只是此为手机端的ashx动态页
            routes.MapPageRoute(
                "mobile",
                "mobile/{term}.htm",
                "~/mobile/{term}.ashx"
                );
            //同上，此为手机端自定义页面
            routes.MapPageRoute(
                "mcustom",
                "mobile/{term}.cshtm",
                "~/TemplatePage.ashx"
                );
            //视频播放的防下载处理
            routes.MapPageRoute(
                "video",
                "v/{term}.aspx",
                "~/VideoUrl.aspx"
                );
        }


        protected void Application_End(object sender, EventArgs e)
        {

        }
        protected void Session_Start(object sender, EventArgs e)
        {

        }
        protected void Session_End(object sender, EventArgs e)
        {

        }

        
    }
}