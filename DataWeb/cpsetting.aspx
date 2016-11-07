<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="cpsetting.aspx.cs" Inherits="cpsetting" Title="Ƶ������" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 276px;
            text-align: left;
            vertical-align: top;
        }
        .style2
        {
            font-size: large;
            font-weight: bold;
        }
        .style3
        {
            vertical-align: top;
        }
        .style4
        {
            width: 150px;
            text-align: left;
        }
        .style5
        {
            width: 60px;
            text-align: left;
        }
        .style6
        {
            text-align: left;
        }
        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<script type="text/javascript" language="javascript" >

    function moveItems(objfrom, objto) {
        var lbLeft = document.getElementById(objfrom);
        var lbRight = document.getElementById(objto);
        for (var i = 0; i < lbLeft.options.length; i++) {
            if (lbLeft.options[i].selected) {
                lbRight.appendChild(lbLeft.options[i]);
                i--;        // bug fix. �Ƴ�ѡ�����������
            }
        }
    }

    function userCPSetProc() {
        // ����ѡƵ��ID��������
        var lbUsedCP = document.getElementById("<%=lbUsedCP.ClientID%>");
        var hfUserCP = document.getElementById("<%=hfUserCP.ClientID%>");

        hfUserCP.value = "";
        
        var arr = new Array();
        for (var i = 0; i < lbUsedCP.options.length; i++) {
            //arr[i] = lbUsedCP.options[i].value + ";" + lbUsedCP.options[i].text;
            arr[i] = lbUsedCP.options[i].value;
        }

        hfUserCP.value = arr.join('|');
        
    }

    function cpUserSetProc() {
        // �������
        var ddlChannel = document.getElementById("<%=ddlChannel.ClientID%>");
        if (ddlChannel.options[ddlChannel.selectedIndex].value == "-1") {
            alert("����δѡ��Ƶ����");
            return false;
        }

        // ����ѡ�û�ID��������
        var lbUsedUser = document.getElementById("<%=lbUsedUser.ClientID%>");
        var hfCPUser = document.getElementById("<%=hfCPUser.ClientID%>");
        
        hfCPUser.value = "";
        
        var arr = new Array();
        for (var i = 0; i < lbUsedUser.options.length; i++) {
            //arr[i] = lbUsedUser.options[i].value + ";" + lbUsedUser.options[i].text;
            arr[i] = lbUsedUser.options[i].value;
        }
        
        hfCPUser.value = arr.join('|');
        
        return true;
    }

</script>

    <table style="width:100%;">
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <table style="width:100%;">
                    <tr>
                        <td class="style1">
                            <asp:TreeView ID="tvCPUsers" runat="server" 
                                onselectednodechanged="tvCPUsers_SelectedNodeChanged" >
                                <ParentNodeStyle Font-Bold="False" />
                                <HoverNodeStyle Font-Underline="True" ForeColor="#6666AA" BackColor="#D4D4D4"/>
                                <SelectedNodeStyle BackColor="#B5B5B5" Font-Underline="False" />
                                <RootNodeStyle Font-Bold="False" />
                                <NodeStyle Font-Names="Tahoma" Font-Size="8pt" ForeColor="Black" 
                                    HorizontalPadding="2px" NodeSpacing="0px" VerticalPadding="2px" />
                            </asp:TreeView>
                        </td>
                    
                        <td class="style3">
                            <table id="ctrlTab" runat="server">
                                <tr>
                                    <td class="style2" colspan="3">�û�Ƶ����Ŀ����</td>
                                </tr>
                                <tr>
                                    <td class="style6" colspan="3">
                                        ������ʾ��<br />
                                        1. չ��������νṹ�ɲ鿴ϵͳ���е��û�Ƶ����Ŀ����<br />
                                        2. ѡ��Ƶ���б����Ƶ�����ƿ����ù����Ƶ�����û�<br />
                                        3. ����û��������ø��û��ɹ����Ƶ����Ŀ</td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:Label ID="lbTips" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:Label ID="lbWarning" runat="server" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:Label ID="lbMessage" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <hr /></td>
                                </tr>
                                <tr>
                                    <td class="style4">
                                        ��ѡƵ���б�</td>
                                    <td>
                                        &nbsp;</td>
                                    <td class="style6">
                                        ��ѡƵ���б�</td>
                                </tr>
                                <tr>
                                    <td class="style4">
                                        <select id="lbUsableCP" name="D1" multiple="true" runat="server" style="height: 150px; width: 120px" >
                                        </select>
                                    </td>
                                    <td class="style5">
                                        <input id="turnRight" type="button" value=">>" 
                                            onclick='moveItems("<%=lbUsableCP.ClientID%>", "<%=lbUsedCP.ClientID%>");' />
                                        <br />
                                        <br />
                                        <input id="turnLeft" type="button" value="<<" 
                                            onclick='moveItems("<%=lbUsedCP.ClientID%>", "<%=lbUsableCP.ClientID%>");' />
                                    </td>
                                    <td class="style6">
                                        <select id="lbUsedCP" name="D2" multiple="true" runat="server" style="height: 150px; width: 120px" >
                                        </select>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:Button ID="btnUserCPSet" runat="server" onclick="btnUserCPSet_Click" 
                                            Text="����" onclientclick="userCPSetProc();" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style4">
                                        <asp:DropDownList ID="ddlChannel" runat="server" Width="120px" Height="20px" 
                                            onselectedindexchanged="ddlChannel_SelectedIndexChanged" 
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="style6" colspan="2">
                                        <asp:Label ID="lbMessage2" runat="server"></asp:Label>    
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style4">
                                        ��ѡ�û��б�</td>
                                    <td>
                                        &nbsp;</td>
                                    <td class="style6">
                                        ��ѡ�û��б�</td>
                                </tr>
                                <tr>
                                    <td class="style4">
                                        <select id="lbUsableUser" name="D3" multiple="true" runat="server" style="height: 150px; width: 120px" >
                                        </select>
                                    </td>
                                    <td class="style5">
                                        <input id="Button1" type="button" value=">>" 
                                            onclick='moveItems("<%=lbUsableUser.ClientID%>", "<%=lbUsedUser.ClientID%>");' />
                                        <br />
                                        <br />
                                        <input id="Button2" type="button" value="<<" 
                                            onclick='moveItems("<%=lbUsedUser.ClientID%>", "<%=lbUsableUser.ClientID%>");' />
                                    </td>
                                    <td class="style6">
                                        <select id="lbUsedUser" name="D4" multiple="true" runat="server" style="height: 150px; width: 120px" >
                                        </select>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:Button ID="btnCPUserSet" runat="server" onclick="btnCPUserSet_Click" 
                                            Text="����" onclientclick="return cpUserSetProc();" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <input id="hfUserCP" type="hidden" value="" runat="server" />
                                        <input id="hfCPUser" type="hidden" value="" runat="server" />
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
                <table style="width:100%;">
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>


</asp:Content>

