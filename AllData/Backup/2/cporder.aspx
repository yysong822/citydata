<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="cporder.aspx.cs" Inherits="cporder" Title="顺序配置" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style2
        {
            width: 266px;
        }
    </style>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


    <table style="width:100%;">
        <tr>
            <td>
                <asp:Label ID="lbTitle" runat="server" Font-Bold="True" Font-Size="16px" 
                    Text="栏目或频道顺序配置"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <table style="width:400px;">
                    <tr>
                        <td style="text-align:left; width:250px">
                            <table style="width:100%;">
                                
                                <tr>
                                    <td class="style2" rowspan="2" align = "right">
                                        <asp:ListBox ID="lboxChannel" runat="server" OnSelectedIndexChanged="lboxChannel_SelectedIndexChanged" 
                                            AutoPostBack="True"  style="overflow:scroll;" Height="230px" 
                                            Width="150px"></asp:ListBox>
                                    </td>
                                    <td align = "left" valign = "bottom">
                                        <asp:Button ID="bCUp" runat="server" Text="上移" onclick="bCUp_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align = "left" valign = "top" >
                                        <asp:Button ID="bCDown" runat="server" Text="下移" onclick="bCDown_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Button ID="bCOreder" runat="server" Text="确定" onclick="bCOreder_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="width:100px;">
                        </td>
                        <td >
                            <table style="width:100%;">
                                
                                <tr>
                                    <td class="style2" rowspan="2" align = "right">
                                        <asp:ListBox ID="lboxProgram" runat="server" style="overflow:scroll;" Height="230px" 
                                            Width="300px"></asp:ListBox>
                                    </td>
                                    <td align = "left" valign = "bottom">
                                        <asp:Button ID="bPUp" runat="server" Text="上移" onclick="bPUp_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align = "left" valign = "top" >
                                        <asp:Button ID="bPDown" runat="server" Text="下移" onclick="bPDown_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Button ID="bPOrder" runat="server" onclick="bPOrder_Click" Text="确定" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
    </table>


</asp:Content>

