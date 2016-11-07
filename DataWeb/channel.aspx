<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="channel.aspx.cs" Inherits="channel" Title="频道管理" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 116px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


        <table style="width:100%;">
        <tr>
        <td colspan = "2">
            <asp:Label ID="Label1" runat="server" Text="频道管理" Font-Bold="True" Font-Size="Large"></asp:Label>
        </td>
        </tr>
            <tr>
                <td style="text-align:left; width:250px">
                    
                    <asp:TreeView ID="trChannel" runat="server"  OnSelectedNodeChanged = "trChannel_SelectedNodeChanged">
                        <ParentNodeStyle Font-Bold="true" VerticalPadding="20px" />
                        <HoverNodeStyle Font-Underline="True" ForeColor="#6666AA" BackColor="#D4D4D4"/>
                        <SelectedNodeStyle BackColor="#B5B5B5" Font-Underline="False" 
                            HorizontalPadding="0px" VerticalPadding="0px" />
                        <NodeStyle Font-Names="Tahoma" Font-Size="8pt" ForeColor="Black" 
                            HorizontalPadding="2px" NodeSpacing="0px" VerticalPadding="2px" />
                    </asp:TreeView>
                    
                    </td>
                <td style="text-align:center; width:350px" valign = "top">
                    <table style="width:100%;">
                    <tr>
                            <td colspan="3" style="height :50px" >
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lbTip" runat="server" Text="选择为："></asp:Label>
                                <asp:Label ID="lbSelected" runat="server" Text="空！"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:right;">
                                <asp:Label ID="lbExistChannel" runat="server" Text="现有频道："></asp:Label>
                            </td>
                            <td style="text-align:left;" class="style1">
                                <asp:TextBox ID="tbExistChannel" runat="server"></asp:TextBox>
                            </td>
                            <td style="text-align:left;">
                                <asp:Button ID="bModifyName" runat="server" Text="改名" 
                                    onclick="bModifyName_Click" Enabled="False" />
                                <asp:Button ID="bDelete" runat="server" Text="删除" onclick="bDelete_Click" OnClientClick = "if(confirm('您确定要删除此频道吗?')){return true;}else{return false;}"
                                    Enabled="False" />
                            </td>
                        </tr>
                        
                        <tr>
                            <td style="text-align:right;">
                                <asp:Label ID="lbAddChannel" runat="server" Text="新增频道："></asp:Label>
                            </td>
                            <td style="text-align:left;" class="style1">
                                <asp:TextBox ID="tbAddChannel" runat="server"></asp:TextBox>
                            </td>
                            <td style="text-align:left;"">
                                <asp:Button ID="bAddChannel" runat="server" Text="增加" 
                                    onclick="bAddChannel_Click" Enabled="False" />
                            </td>
                        </tr>
                        
                    </table>
                </td>
            </tr>
        </table>
        
        
</asp:Content>

