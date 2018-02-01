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
    public partial class Students_Coupon : Extend.CustomPage
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
            Song.Entities.Accounts ea;
            if (id == 0) return;
            ea = Business.Do<IAccounts>().AccountsSingle(id);
            //Ա������
            this.lbName.Text = ea.Ac_Name;
            //�˻����
            lbMoney.Text = ea.Ac_Money.ToString();

        }

        /// <summary>
        /// ��ֵ��۷�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddMoney_Click(object sender, EventArgs e)
        {            
            if (!Extend.LoginState.Admin.IsAdmin) throw new Exception("�ǹ���Ա��Ȩ�˲���Ȩ�ޣ�");
            Song.Entities.Accounts st = Business.Do<IAccounts>().AccountsSingle(id);
            if (st == null) throw new Exception("��ǰ��Ϣ�����ڣ�");
            //��������
            int type = 2;
            int.TryParse(rblOpera.SelectedItem.Value, out type);
            //�������
            int money = 0;
            int.TryParse(tbMoney.Text, out money);
            //��������
            Song.Entities.MoneyAccount ma = new MoneyAccount();
            ma.Ma_Monery = money;
            ma.Ma_Remark = tbRemark.Text.Trim();
            ma.Ac_ID = st.Ac_ID;
            ma.Ma_Source = "����Ա����";
            //��ֵ��ʽ������Ա��ֵ
            ma.Ma_From = 1;
            ma.Ma_IsSuccess = true;
            //������
            Song.Entities.EmpAccount emp=Extend.LoginState.Admin.CurrentUser;
            try
            {
                string mobi = !string.IsNullOrWhiteSpace(emp.Acc_MobileTel) && emp.Acc_AccName != emp.Acc_MobileTel ? emp.Acc_MobileTel : "";
                //����ǳ�ֵ
                if (type == 2)
                {                   
                    ma.Ma_Info = string.Format("����Ա{0}��{1}{2}��������ֵ{3}Ԫ", emp.Acc_Name, emp.Acc_AccName, mobi, money);
                    Business.Do<IAccounts>().MoneyIncome(ma);
                }
                //�����ת��
                if (type == 1)
                {
                    ma.Ma_Info = string.Format("����Ա{0}��{1}{2}���۳���{3}Ԫ", emp.Acc_Name, emp.Acc_AccName, mobi, money);
                    Business.Do<IAccounts>().MoneyPay(ma);
                }
                Master.AlertCloseAndRefresh("�����ɹ���");
            }
            catch (Exception ex)
            {
                this.Alert(ex.Message);
            }
        }       

    }
}
