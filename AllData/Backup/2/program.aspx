<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="program.aspx.cs" Inherits="program" Title="栏目管理" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 160px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<table style="width:100%;">
            <tr>
                <td colspan = "2">
                    <asp:Label ID="Label1" runat="server" Text="栏目管理" Font-Bold="True" Font-Size="Large"></asp:Label>
                </td>
            <tr>
                <td style="text-align:left; width:250px">
                    <asp:TreeView ID="trProgram" runat="server" OnSelectedNodeChanged = "trProgram_SelectedNodeChanged">
                        <ParentNodeStyle Font-Bold="true" VerticalPadding="20px" />
                        <HoverNodeStyle Font-Underline="True" ForeColor="#6666AA" BackColor="#D4D4D4"/>
                        <SelectedNodeStyle BackColor="#B5B5B5" Font-Underline="False" 
                            HorizontalPadding="0px" VerticalPadding="0px" />
                        <NodeStyle Font-Names="Tahoma" Font-Size="8pt" ForeColor="Black" 
                            HorizontalPadding="2px" NodeSpacing="0px" VerticalPadding="2px" />
                    </asp:TreeView>
                    </td>
                <td valign = "top">
                    <table style="text-align:left; width:400px">
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
                                <asp:Label ID="lbExistProgram" runat="server" Text="现有栏目："></asp:Label>
                            </td>
                            <td style="text-align:left;" class="style1">
                                <asp:TextBox ID="tbExistProgram" runat="server" Width="150px"></asp:TextBox>
                            </td>
                            <td style="text-align:left;">
                                <asp:Button ID="bModifyName" runat="server" Text="改名" 
                                    onclick="bModifyName_Click" />
                                <asp:Button ID="bDelete" runat="server" Text="删除" onclick="bDelete_Click"  OnClientClick = "if(confirm('您确定要删除此栏目吗?')){return true;}else{return false;}"/>
                            </td>
                        </tr>
                        
                        <tr>
                            <td style="text-align:right;">
                                <asp:Label ID="lbAddProgram" runat="server" Text="新增栏目："></asp:Label>
                            </td>
                            <td style="text-align:left;" class="style1">
                                <asp:TextBox ID="tbAddProgram" runat="server" Width="150px"></asp:TextBox>
                            </td>
                            <td style="text-align:left;">
                                <asp:Button ID="bAddProgram" runat="server" Text="增加" 
                                    onclick="bAddProgram_Click" />
                            </td>
                        </tr>
                        
                        <tr>
                            <td style="text-align:right;">
                                <asp:Label ID="lbMoveProgram" runat="server" Text="栏目转移："></asp:Label>
                            </td>
                            <td style="text-align:left;" class="style1">
                                <asp:DropDownList ID="ddlProgramList" runat="server" Width="157px">
                                </asp:DropDownList>
                            </td>
                            <td style="text-align:left;">
                                <asp:Button ID="bMove" runat="server" onclick="bMove_Click" Text="转到" />
                            </td>
                        </tr>
                        
                    </table>
                </td>
            </tr>
        </table>

</asp:Content>

