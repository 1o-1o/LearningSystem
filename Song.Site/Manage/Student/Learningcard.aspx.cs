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
using WeiSha.WebControl;
using System.IO;

namespace Song.Site.Manage.Student
{
    public partial class Learningcard : Extend.CustomPage
    {
        Song.Entities.Organization org;
        //��ǰѧԱ��ѧϰ���������Լ�ʹ����
        protected int cardcount, usecount;
        protected void Page_Load(object sender, EventArgs e)
        {

            org = Business.Do<IOrganization>().OrganCurrent();
            if (!this.IsPostBack)
            {
                //��ǰѧԱ��ѧϰ������
                int accid = Extend.LoginState.Accounts.CurrentUserId;
                if (accid > 0)
                {
                    cardcount = Business.Do<ILearningCard>().AccountCardOfCount(accid);                    
                    usecount = cardcount - Business.Do<ILearningCard>().AccountCardOfCount(accid, 0);                    
                    //ѧԱ��ѧϰ��
                    Song.Entities.LearningCard[] cards = Business.Do<ILearningCard>().AccountCards(accid);
                    
                } 
            }
        }
        private void init()
        {


        }
    }
}
