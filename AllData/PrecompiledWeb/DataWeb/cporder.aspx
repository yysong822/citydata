<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="cporder, App_Web_cporder.aspx.cdcab7d2" title="˳������" %>

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
                    Text="��Ŀ��Ƶ��˳������"></asp:Label>
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
                                        <asp:Button ID="bCUp" runat="server" Text="����" onclick="bCUp_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align = "left" valign = "top" >
                                        <asp:Button ID="bCDown" runat="server" Text="����" onclick="bCDown_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Button ID="bCOreder" runat="server" Text="ȷ��" onclick="bCOreder_Click" />
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
                                        <asp:Button ID="bPUp" runat="server" Text="����" onclick="bPUp_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align = "left" valign = "top" >
                                        <asp:Button ID="bPDown" runat="server" Text="����" onclick="bPDown_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Button ID="bPOrder" runat="server" onclick="bPOrder_Click" Text="ȷ��" />
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

