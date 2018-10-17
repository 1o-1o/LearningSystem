﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeiSha.Common;
using Song.ServiceInterfaces;
using Song.Entities;
using WeiSha.WebControl;
using System.Data;

namespace Song.Site.Manage.Course
{
    public partial class Courses_Outlines : Extend.CustomPage
    {
        //课程id
        protected int couid = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;
        //当前章节id
        protected int olid = WeiSha.Common.Request.QueryString["olid"].Int32 ?? 0;
        protected string UID
        {
            get { return this.getUID(); }
        }
        //上传资料的所有路径
        private string _uppath = "Course";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                //隐藏确定按钮
                EnterButton btnEnter = (EnterButton)Master.FindControl("btnEnter");
                btnEnter.Visible = false;
                BindData(null, null);                
            }
        }
        /// <summary>
        /// 绑定章节列表
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            Song.Entities.Outline[] outline = Business.Do<IOutline>().OutlineAll(couid, null);
            DataTable dt = WeiSha.WebControl.Tree.ObjectArrayToDataTable.To(outline);
            WeiSha.WebControl.Tree.DataTableTree tree = new WeiSha.WebControl.Tree.DataTableTree();
            tree.IdKeyName = "OL_ID";
            tree.ParentIdKeyName = "OL_PID";
            tree.TaxKeyName = "Ol_Tax";
            tree.Root = 0;
            dt = tree.BuilderTree(dt);
            GridView1.DataSource = dt;
            GridView1.DataKeyNames = new string[] { "Ol_ID" };
            GridView1.DataBind();
        }
        #region 行内事件        
        /// <summary>
        /// 单个删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDel_Click(object sender, ImageClickEventArgs e)
        {
            WeiSha.WebControl.RowDelete img = (WeiSha.WebControl.RowDelete)sender;
            int index = ((GridViewRow)(img.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            Business.Do<IOutline>().OutlineDelete(id);
            BindData(null, null);            
        }
        /// <summary>
        /// 上移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbUp_Click(object sender, EventArgs e)
        {
            GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
            int id = Convert.ToInt32(this.GridView1.DataKeys[gr.RowIndex].Value);
            if (Business.Do<IOutline>().OutlineUp(couid, id))            
                BindData(null, null);            
        }
        /// <summary>
        /// 下移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbDown_Click(object sender, EventArgs e)
        {
            GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
            int id = Convert.ToInt32(this.GridView1.DataKeys[gr.RowIndex].Value);
            if (Business.Do<IOutline>().OutlineDown(couid, id))          
                BindData(null, null);            
        }
        /// <summary>
        /// 进入编辑行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnToEditor_Click(object sender, EventArgs e)
        {
            GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
            int index = gr.RowIndex;
            GridView1.EditIndex = index;
                BindData(null, null);
        }
        /// <summary>
        /// 退出编辑状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEditBack_Click(object sender, EventArgs e)
        {
            GridView1.EditIndex = -1;
            BindData(null, null);
        }
        /// <summary>
        /// 编辑当前数据项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEditEnter_Click(object sender, EventArgs e)
        {
        }
        #endregion
    }
}