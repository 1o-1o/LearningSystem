using System;
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
using System.Xml.Serialization;
using System.IO;


namespace Song.Site.Manage.Course
{
    public partial class Outline_Edit : Extend.CustomPage
    {
        //�½�ID
        private int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        //�γ�ID
        private int couid = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                InitBind();
                fill();
            }
            this.Form.DefaultButton = this.btnEnter.UniqueID;
        }
        /// <summary>
        /// ����ĳ�ʼ��
        /// </summary>
        private void InitBind()
        {
            Song.Entities.Outline[] outline = Business.Do<IOutline>().OutlineAll(couid, null);
            DataTable dt = WeiSha.WebControl.Tree.ObjectArrayToDataTable.To(outline);
            WeiSha.WebControl.Tree.DataTableTree tree = new WeiSha.WebControl.Tree.DataTableTree();
            tree.IdKeyName = "Ol_ID";
            tree.ParentIdKeyName = "Ol_PID";
            tree.TaxKeyName = "Ol_Tax";
            tree.Root = 0;
            dt = tree.BuilderTree(dt);
            ddlOutline.DataSource = dt;
            this.ddlOutline.DataTextField = "Ol_Name";
            this.ddlOutline.DataValueField = "Ol_ID";
            ddlOutline.DataBind();
            this.ddlOutline.Items.Insert(0, new ListItem("-- �����½� --", "0"));
        }

        private void fill()
        {
            Song.Entities.Outline mm;
            if (id > 0)
            {
                mm = Business.Do<IOutline>().OutlineSingle(id);
                //�Ƿ���ʾ
                cbIsUse.Checked = mm.Ol_IsUse;
                cbIsFree.Checked = mm.Ol_IsFree;  //�Ƿ����
                cbIsFinish.Checked = mm.Ol_IsFinish;    //�Ƿ����
                //�ϼ��½�
                ListItem li = ddlOutline.Items.FindByValue(mm.Ol_PID.ToString());
                if (li != null)
                {
                    ddlOutline.SelectedIndex = -1;
                    li.Selected = true;
                }
                //Ψһ��ʶ
                ViewState["UID"] = mm.Ol_UID;              
            }
            else
            {
                //���������
                mm = new Song.Entities.Outline();
                ViewState["UID"] = WeiSha.Common.Request.UniqueID();
            }
            //����
            Ol_Name.Text = mm.Ol_Name;
            //���
            Ol_Intro.Text = mm.Ol_Intro; 
        }
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            Song.Entities.Outline ol = id < 1 ? new Song.Entities.Outline() : Business.Do<IOutline>().OutlineSingle(id);
            if (ol == null) return;
            //����
            ol.Ol_Name = Ol_Name.Text.Trim();
            //���
            ol.Ol_Intro = Ol_Intro.Text;
            //�ϼ��½�
            int pid = 0;
            int.TryParse(ddlOutline.SelectedValue, out pid);
            ol.Ol_PID = pid;

            ol.Ol_IsUse = cbIsUse.Checked;  //�Ƿ�����
            ol.Ol_IsFree = cbIsFree.Checked;  //���
            ol.Ol_IsFinish = cbIsFinish.Checked;    //���
            //�����γ�
            ol.Cou_ID = couid;
            Song.Entities.Course cou = Business.Do<ICourse>().CourseSingle(couid);
            if (cou != null) ol.Sbj_ID = cou.Sbj_ID;
            //ȫ��ΨһID
            ol.Ol_UID = getUID();
            try
            {
                if (id < 1)
                {
                    //����
                    Business.Do<IOutline>().OutlineAdd(ol);
                }
                else
                {
                    Business.Do<IOutline>().OutlineSave(ol);
                }
                Master.AlertCloseAndRefresh("�������");
            }
            catch
            {
                throw;
            }
        }
    }
}
