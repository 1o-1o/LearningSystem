﻿using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using WeiSha.Common;

using Song.ServiceInterfaces;
using Song.Entities;
using WeiSha.WebControl;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Song.Site.Manage.Card
{
    public partial class Learningcard_Code : Extend.CustomPage
    {
        //学习卡设置项的id
        protected int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        Song.Entities.Organization org = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            org = Business.Do<IOrganization>().OrganCurrent();
            if (!this.IsPostBack)
            {
                BindData(null, null);
            }
        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            //总记录数
            int count = 0;
            Song.Entities.LearningCard[] eas = null;
            eas = Business.Do<ILearningCard>().CardPager(org.Org_ID, id, null,null, Pager1.Size, Pager1.Index, out count);
            GridView1.DataSource = eas;
            GridView1.DataKeyNames = new string[] { "Lc_ID" };
            GridView1.DataBind();

            Pager1.RecordAmount = count;
        }
       
    }
}
