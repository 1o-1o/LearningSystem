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

namespace Song.Site.Manage.Teacher
{
    public partial class Teacher_Password : Extend.CustomPage
    {
        private int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {                
                fill();
            }   
            //������������ʾ���
            //this.trPw1.Visible = this.trPw2.Visible = id == 0;
        }
        private void fill()
        {            
            Song.Entities.Teacher ea;
            if (id == 0) return;
            ea = Business.Do<ITeacher>().TeacherSingle(id);          
            //Ա������
            this.lbName.Text = ea.Th_Name;           
            
        }

        /// <summary>
        /// ��֤�˺��Ƿ��Ѿ�����
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        protected void cusv_ServerValidate(object source, ServerValidateEventArgs args)
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            Song.Entities.Teacher th = Business.Do<ITeacher>().TeacherSingle(id);
            if (th == null) th = new Entities.Teacher();
            th.Org_ID = org.Org_ID;
            //�ж��Ƿ�ͨ����֤
            bool isAccess = Business.Do<ITeacher>().IsTeacherExist(org.Org_ID,th);
            args.IsValid = !isAccess;
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPw_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Extend.LoginState.Admin.IsAdmin) throw new Exception("�ǹ���Ա��Ȩ�˲���Ȩ�ޣ�");
                if (id == 0) throw new Exception("��ǰ��Ϣ�����ڣ�");
                Song.Entities.Teacher obj;
                obj = Business.Do<ITeacher>().TeacherSingle(id);
                Song.Entities.Accounts acc = Business.Do<IAccounts>().AccountsSingle(obj.Ac_ID);
                //Ա����¼���룬Ϊ��
                if (tbPw1.Text.Trim() != "")
                {                    
                    obj.Th_Pw = tbPw1.Text.Trim();
                    if (acc != null) acc.Ac_Pw = new WeiSha.Common.Param.Method.ConvertToAnyValue(obj.Th_Pw).MD5;    
                }
                obj.Th_Pw = new WeiSha.Common.Param.Method.ConvertToAnyValue(obj.Th_Pw).MD5;
                Business.Do<IAccounts>().AccountsSave(acc);
                Business.Do<ITeacher>().TeacherSave(obj);
                Master.AlertCloseAndRefresh("�����ɹ���");
            }
            catch (Exception ex)
            {
                Master.Alert(ex.Message);
            }
        }
       
    }
}
