﻿<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    CodeBehind="Students_Details.aspx.cs" Inherits="Song.Site.Manage.Admin.Students_Details"
    Title="学员信息详情" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="WebEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
 <asp:DropDownList ID="ddlEducation" runat="server">
                    <asp:ListItem Value="81">小学</asp:ListItem>
                    <asp:ListItem Value="71">初中</asp:ListItem>
                    <asp:ListItem Value="61">高中</asp:ListItem>
                    <asp:ListItem Value="41">中等职业教育</asp:ListItem>
                    <asp:ListItem Value="31">大学（专科）</asp:ListItem>
                    <asp:ListItem Value="21">大学（本科）</asp:ListItem>
                    <asp:ListItem Value="14">硕士</asp:ListItem>
                    <asp:ListItem Value="11">博士</asp:ListItem>
                    <asp:ListItem Value="90">其它</asp:ListItem>
                </asp:DropDownList>
    <asp:Repeater ID="rptAccounts" runat="server">
        <ItemTemplate>
         
                <table width="100%" class="first" border="1" cellspacing="0" cellpadding="0" style="page-break-after: always">
                    <tr>
                        <td class="right" width="100px">
                            姓名：
                        </td>
                        <td class="left" width="100px">
                            <%# Eval("Ac_name") %>
                        </td>
                        <td class="right" width="100px">
                            性别：
                        </td>
                        <td class="left">
                            <%# Eval("Ac_sex","{0}")=="0" ? "女" : "男" %>
                        </td>
                        
                        <td rowspan="5" valign="middle" class="photo">
                            <img src="<%# Eval("Ac_photo") %>" width="150px" height="200px" />
                        </td>
                    </tr>
                    <tr>
                    <td class="right">
                            年龄：
                        </td>
                        <td class="left">
                            <%# Convert.ToInt16(Eval("Ac_Age")) > 200 ? "未知" : Eval("Ac_Age")%>
                        </td> <td class="right">
                            出生年月：
                        </td>
                        <td class="left">
                            <%# Convert.ToDateTime(Eval("Ac_Birthday")).AddYears(200) < DateTime.Now ? "" : Convert.ToDateTime(Eval("Ac_Birthday")).ToString("yyyy年MM月")%>
                        </td>
                    </tr>
                    <tr>
                        <td class="right">
                            籍贯：
                        </td>
                        <td class="left">
                            <%# Eval("Ac_Native")%>
                        </td>
                        <td class="right">
                            民 族：
                        </td>
                        <td class="left">
                            <%# Eval("Ac_Nation")%>
                        </td>
                       
                    </tr>
                    <tr>
                        <td class="right">
                            学号：
                        </td>
                        <td class="left">
                            <%# Eval("Ac_CodeNumber")%>
                        </td>
                        <td class="right">
                            身份证：
                        </td>
                        <td class="left">
                            <%# Eval("Ac_IDCardNumber")%>
                        </td>
                    </tr>
                    <tr>
                        <td class="right">
                            学历：
                        </td>
                        <td class="left">
                            <%# getEdu(Eval("Ac_Education"))%>                           
                        </td>
                        <td class="right">
                            专业：
                        </td>
                        <td class="left">
                            <%# Eval("Ac_Major")%>
                        </td>
                    </tr>
                    <tr>
                        <td class="right">
                            毕业院校：
                        </td>
                        <td class="left" colspan="4">
                            <%# Eval("Ac_School")%>
                        </td>
                    </tr>
                 <tr>
                        <td class="right">
                            住址：
                        </td>
                        <td class="left" colspan="4">
                            <%# Eval("Ac_Address")%>
                        </td>
                    </tr>
                <tr>
                        <td class="right">
                            通讯地址：
                        </td>
                        <td class="left" colspan="4">
                            <%# Eval("Ac_AddrContact")%>
                        </td>
                    </tr>
                    <tr>
                        <td class="right">
                            邮编：
                        </td>
                        <td class="left">
                            <%# Eval("Ac_Zip")%>                           
                        </td>
                        <td class="right">
                            电话：
                        </td>
                        <td class="left" colspan="2">
                            <%# Eval("Ac_MobiTel1")%> &nbsp;  <%# Eval("Ac_MobiTel2", "{0}") == Eval("Ac_MobiTel1", "{0}") ? "" : Eval("Ac_MobiTel2", "{0}")%> &nbsp; <%# Eval("Ac_Tel")%> 
                        </td>
                    </tr>  
                      <tr>
                        <td class="right">
                            电子邮箱：
                        </td>
                        <td class="left">
                            <%# Eval("Ac_Email")%>                           
                        </td>
                        <td class="right">
                            即时通讯：
                        </td>
                        <td class="left" colspan="2">
                            <%# Eval("Ac_Weixin", "{0}") != "" ? "微信：" + Eval("Ac_Weixin", "{0}") : ""%>
                            &nbsp;
                            <%# Eval("Ac_Qq", "{0}") != "" ? "QQ：" + Eval("Ac_Qq", "{0}") : ""%>
                        </td>
                    </tr>
                     <tr>
                        <td class="right">
                            紧急联系人：
                        </td>
                        <td class="left">
                            <%# Eval("Ac_LinkMan")%>                           
                        </td>
                        <td class="right">
                            紧急电话：
                        </td>
                        <td class="left" colspan="2">
                             <%# Eval("Ac_LinkManPhone")%>      
                        </td>
                    </tr>
                    <tr>
                        <td class="center" colspan="5">
                          个人简介
                        </td>
                       
                        
                    </tr>
                    <tr>
                        <td colspan="5">
                            <div class="intro"><%# Eval("Ac_Intro")%></div>   
                        </td>
                       
                        
                    </tr>
                </table>
         
        </ItemTemplate>
    </asp:Repeater>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
</asp:Content>
