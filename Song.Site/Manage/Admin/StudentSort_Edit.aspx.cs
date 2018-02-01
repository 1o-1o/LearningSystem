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



namespace Song.Site.Manage.Admin
{
    public partial class Sort_Edit : Extend.CustomPage
    {
        private int id = WeiSha.Common.Request.QueryString["id"].Decrypt().Int32 ?? 0;
        //Ա���ϴ����ϵ�����·��
        //private string _uppath = "Student";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                fill();
            }
            this.Form.DefaultButton = this.btnEnter.UniqueID;
        }
        private void fill()
        {
            Song.Entities.StudentSort th = id == 0 ? new Song.Entities.StudentSort() : Business.Do<IStudent>().SortSingle(id);
            if (th == null) return;
            if (id != 0) this.EntityBind(th);
        }
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org == null) throw new WeiSha.Common.ExceptionForAlert("��ǰ����������");
            //
            Song.Entities.StudentSort th = id == 0 ? new Song.Entities.StudentSort() : Business.Do<IStudent>().SortSingle(id);
            th = this.EntityFill(th) as Song.Entities.StudentSort;
            th.Org_ID = org.Org_ID;
            th.Org_Name = org.Org_Name;
            //�ж��Ƿ�����
            if (Business.Do<IStudent>().SortIsExist(th))
            {
                Master.Alert("��ǰѧ�������Ѿ����ڣ�");
            }
            else
            {
                try
                {
                    if (id == 0)
                    {
                        Business.Do<IStudent>().SortAdd(th);
                    }
                    else
                    {
                        Business.Do<IStudent>().SortSave(th);
                    }

                    Master.AlertCloseAndRefresh("�����ɹ���");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
