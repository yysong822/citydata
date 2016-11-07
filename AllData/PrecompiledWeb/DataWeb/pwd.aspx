<%@ page title="" language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="pwd, App_Web_pwd.aspx.cdcab7d2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<table width="100%" border="0" cellspacing="2" cellpadding="2">
  <tr>
    <td colspan="2" height="15"></td>
  </tr>
  <tr>
    <td align="center" colspan="2" style="font-size:large; font-style: italic;">
        请输入旧密码和新密码</td>
  </tr>
  <tr>
    <td colspan="2" height="15">
        <asp:CompareValidator ID="CompareValidator1" runat="server" 
            ControlToCompare="tbNewPwd" ControlToValidate="tbCfmPwd" 
            ErrorMessage="* 新密码不相同！" ValidationGroup="pwd"></asp:CompareValidator>
    </td>
  </tr>
  <tr>
    <td width="40%"><div align="right" style="text-align: right">旧密码：</div></td>
    <td style="text-align: left">
        <asp:TextBox ID="tbOldPwd" runat="server" 
            Font-Size="12pt" Width="150px" TextMode="Password" ValidationGroup="pwd"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
            ControlToValidate="tbOldPwd" ErrorMessage="*" ValidationGroup="pwd"></asp:RequiredFieldValidator>
    </td>
  </tr>
  <tr>
    <td><div align="right" style="text-align: right">新密码：</div></td>
    <td style="text-align: left">
        <asp:TextBox ID="tbNewPwd" runat="server" Font-Size="12pt" 
            TextMode="Password" Width="150px" ValidationGroup="pwd"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
            ControlToValidate="tbNewPwd" ErrorMessage="*" ValidationGroup="pwd"></asp:RequiredFieldValidator>
    </td>
  </tr>
  <tr>
    <td><div align="right" style="text-align: right">确认密码：</div></td>
    <td style="text-align: left">
        <asp:TextBox ID="tbCfmPwd" runat="server" Font-Size="12pt" 
            TextMode="Password" Width="150px" ValidationGroup="pwd"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
            ControlToValidate="tbCfmPwd" ErrorMessage="*" ValidationGroup="pwd"></asp:RequiredFieldValidator>
    </td>
  </tr>
    <tr>
        <td>&nbsp;</td>
        <td style="text-align: left">
            <asp:Button ID="btnModPwd" runat="server" Font-Size="12px" 
                OnClick="btnModPwd_Click" Text="修  改" ValidationGroup="pwd" />
        </td>
    </tr>
</table>

</asp:Content>

