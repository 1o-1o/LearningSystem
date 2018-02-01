<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    Codebehind="Navigation.aspx.cs" Inherits="Song.Site.Manage.Admin.Navigation" Title="�ޱ���ҳ" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="header">
        <uc1:toolsBar ID="ToolsBar1" runat="server" WinPath="Navigation_Edit.aspx" GvName="GridView1"
            WinWidth="800" WinHeight="600" IsWinOpen="true" OnDelete="DeleteEvent" />
        <div class="searchBox">
            <asp:RadioButtonList ID="rblType" runat="server" AutoPostBack="True" 
                onselectedindexchanged="rblType_SelectedIndexChanged" 
                RepeatDirection="Horizontal" RepeatLayout="Flow">
                <asp:ListItem Selected="True" Value="main">�������˵�</asp:ListItem>
                <asp:ListItem Value="foot">�ײ�����</asp:ListItem>
            </asp:RadioButtonList>
        </div>
    </div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
        ShowSelectBox="True">
        <EmptyDataTemplate>
            ��ǰ����û�д����ֻ��˵����˵���
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="����">
                <itemtemplate>
<cc1:RowDelete id="btnDel" onclick="btnDel_Click" runat="server"></cc1:RowDelete> 
<cc1:RowEdit id="btnEdit" runat="server" ></cc1:RowEdit> 
</itemtemplate>
                <itemstyle cssclass="center" width="44px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="�ƶ�">
                <itemstyle cssclass="center" />
                <headerstyle width="80px" />
                <itemtemplate>
<asp:LinkButton id="lbUp" onclick="lbUp_Click" runat="server"  Enabled='<%# Container.DataItemIndex!=0 %>'>����</asp:LinkButton> 
<asp:LinkButton id="lbDown" onclick="lbDown_Click" runat="server"  Enabled='<%# Container.DataItemIndex+1< ((System.Data.DataTable)GridView1.DataSource).Rows.Count %>'>����</asp:LinkButton>

</itemtemplate>
            </asp:TemplateField>
            <%--<asp:TemplateField HeaderText="ID">
                <itemstyle cssclass="center" width="40px" />
                <itemtemplate>
              
<%# Eval("Nav_ID")%>
</itemtemplate>
            </asp:TemplateField>--%>
            <asp:TemplateField HeaderText="����">
                <itemstyle cssclass="center" width="40px" />
                <itemtemplate>
              
<%# Eval("Nav_Tax")%>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="��������">
                <itemstyle cssclass="left"  />
                <itemtemplate>
               <span class="treeIco"><%# Eval("Tree")%></span> 
               <img src="<%# Eval("Nav_Logo")%>" style="height:12px;width:12px;" />
<span style="color:<%# Eval("Nav_Color")%>;<%# Convert.ToBoolean(Eval("Nav_IsBold")) ? "font-weight:bold;" : ""%>font-family:<%# Eval("Nav_Font","{0}")%>" title="<%# Eval("Nav_Title")%>"><%# Eval("Nav_Name")%></span>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="����">
                <itemstyle cssclass="left"/>
                <itemtemplate>
              
<%# Eval("Nav_Url")%>
</itemtemplate>
            </asp:TemplateField>
            
            
            <asp:TemplateField HeaderText="��ʾ">
                <itemstyle cssclass="center" width="60px" />
                <itemtemplate>
<cc1:StateButton id="sbShow" onclick="sbShow_Click" runat="server" TrueText="��ʾ" FalseText="����" State='<%# Eval("Nav_IsShow","{0}")=="True"%>'></cc1:StateButton> 
</itemtemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
</asp:Content>
