<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="login, App_Web_login.aspx.cdcab7d2" title="用户登录" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<asp:Panel ID="plLogin" runat="server" Visible="true">
<table width="100%" border="0" cellspacing="2" cellpadding="2">
  <tr>
    <td colspan="2" height="15"></td>
  </tr>
  <tr>
    <td align="center" colspan="2" style="font-size:large; font-style: italic;">
        请输入用户名和密码</td>
  </tr>
  <tr>
    <td colspan="2" height="15"></td>
  </tr>
  <tr>
    <td width="40%"><div align="right" style="text-align: right">用户名：</div></td>
    <td style="text-align: left">
        <asp:TextBox ID="tbUserID" runat="server" 
            Font-Size="12pt" Width="150px" ValidationGroup="login"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
            ControlToValidate="tbUserID" ErrorMessage="*" ValidationGroup="login"></asp:RequiredFieldValidator>
    </td>
  </tr>
  <tr>
    <td><div align="right" style="text-align: right">密　码：</div></td>
    <td style="text-align: left">
        <asp:TextBox ID="tbUserPwd" runat="server" Font-Size="12pt" 
            TextMode="Password" Width="150px" ValidationGroup="login"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
            ControlToValidate="tbUserPwd" ErrorMessage="*" ValidationGroup="login"></asp:RequiredFieldValidator>
    </td>
  </tr>
    <tr>
        <td>&nbsp;</td>
        <td style="text-align: left">
            <asp:Button ID="btnLogin" runat="server" Font-Size="12px" 
                OnClick="btnLogin_Click" Text="登  录" ValidationGroup="login" />
        </td>
    </tr>
</table>
</asp:Panel>

<asp:Panel ID="plLoginDone" runat="server" Visible="false">
<table width="100%" border="0" cellspacing="2" cellpadding="2">
  <tr>
    <td height="25"></td>
  </tr>
  <tr>
    <td align="center">
        <asp:Label ID="lblMessage" runat="server" Font-Italic="True" Font-Size="Large"></asp:Label>
    </td>
  </tr>
    <tr>
        <td align="center">
            <asp:LinkButton ID="btnLogout" runat="server" Font-Italic="True" 
            Font-Size="Large" onclick="btnLogout_Click" ForeColor="Blue" 
                Font-Underline="True">点击此处退出登录</asp:LinkButton>
        </td>
    </tr>
  </table>
</asp:Panel>

</asp:Content>

