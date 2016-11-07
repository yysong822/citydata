<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="station, App_Web_station.aspx.cdcab7d2" title="站点管理" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            text-align: right;
        }
        .style2
        {
            text-align: left;
        }
        
        .style5
        {
            width: 250px;
            text-align: right;
        }
        .style6
        {
            width: 40px;
            text-align: right;
        }
        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <table style="width: 100%;">
        <tr>
            <td>
                <asp:Panel ID="plView" runat="server">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <table style="width: 100%;">
                                <tr>
                                    <td class="style6">频道：</td>
                                    <td class="style2">
                                        <asp:DropDownList ID="ddlUserChannel" runat="server" Width="100px" Height="20px"
                                            AutoPostBack="True" onselectedindexchanged="ddlUserChannel_SelectedIndexChanged">
                                            <asp:ListItem Value="-1">-- 请选择频道 --</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td class="style6">栏目：</td>
                                    <td class="style2">
                                        <asp:DropDownList ID="ddlUserCP" runat="server" Width="160px" Height="20px"
                                            AutoPostBack="True" onselectedindexchanged="ddlUserCP_SelectedIndexChanged">
                                            <asp:ListItem Value="-1">-- 请选择栏目 --</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td class="style1">站点号：</td>
                                    <td>
                                        <asp:TextBox ID="tbStaID" runat="server" Width="72px"></asp:TextBox>
                                    </td>
                                    <td class="style1">站点名称：</td>
                                    <td class="style2">
                                        <asp:TextBox ID="tbStaName" runat="server" Width="80px"></asp:TextBox>
                                    </td>
                                    <td class="style2">
                                        <asp:Button ID="btnQuery" runat="server" Text="查询" 
                                            onclick="btnQuery_Click" />
                                    </td>
                                </tr>
                                    <tr id="headDynRow" runat="server">
                                        <td class="style6">
                                            &nbsp;</td>
                                        <td class="style2">
                                            &nbsp;</td>
                                        <td class="style6">
                                            &nbsp;</td>
                                        <td class="style2">
                                            &nbsp;</td>
                                        <td class="style1">
                                            &nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                        <td class="style1">
                                            &nbsp;</td>
                                        <td class="style2">
                                            &nbsp;</td>
                                        <td class="style2">
                                            <asp:Button ID="btnExport" runat="server" Text="导出" 
                                                onclick="btnExport_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView ID="gvStationInfo" runat="server" Width="100%" 
                                    AutoGenerateColumns="False" DataKeyNames="StationTableID" AllowPaging="True" 
                                    PageSize="30" CellPadding="4" GridLines="None"  OnRowDataBound="gvStationInfo_RowDataBound" 
                                    OnRowEditing="gvStationInfo_RowEditing" OnPageIndexChanging="gvStationInfo_PageIndexChanging" 
                                    OnRowUpdating="gvStationInfo_RowUpdating" OnRowCancelingEdit="gvStationInfo_RowCancelingEdit" 
                                    OnRowDeleting="gvStationInfo_RowDeleting">
                                    
                                    <Columns>
                                        <asp:CommandField ShowEditButton="True" CancelText="取消" EditText="编辑" 
                                            UpdateText="更新" />
                                        <asp:BoundField DataField="StationTableID" HeaderText="StationTableID" 
                                            ReadOnly="True" SortExpression="StationTableID" Visible="False" />
                                        <asp:BoundField DataField="StationID" HeaderText="站点编号" 
                                            SortExpression="StationID"  />
                                        <asp:BoundField DataField="StationName" HeaderText="站点名称" 
                                            SortExpression="StationName"  />
                                        <asp:CommandField DeleteText="删除" ShowDeleteButton="True" />
                                    </Columns>
                                    
                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <EditRowStyle BackColor="#999999" />
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    
                                </asp:GridView>
                                
                                <asp:Label ID="lblMessage" runat="server" Font-Bold="False" Font-Italic="True" 
                                    Font-Size="Large"></asp:Label>
                                
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:LinkButton ID="lbtnShowAdd" runat="server" Font-Bold="False" 
                                    Font-Italic="True" Font-Overline="False" Font-Size="Large" 
                                    Font-Underline="True" onclick="lbtnShowAdd_Click">点击此处添加站点</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="plAdd" runat="server">
                    <table style="width:100%;">
                        <tr>
                            <td class="style5"></td>
                            <td style="font-size:large; font-style: italic; text-align: left;">
                                请输入站点信息
                            </td>
                        </tr>
                        <tr>
                            <td class="style5"></td>
                            <td class="style2">
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                    ErrorMessage="* 站点编号应为数字且最多5位！" ValidationExpression="^\d{1,5}$" 
                                    ControlToValidate="tbStaID_Add"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="style5">
                                所属频道：
                            </td>
                            <td class="style2">
                                <asp:DropDownList ID="ddlUserChannel_Add" runat="server" Width="164px" Height="20px"
                                    AutoPostBack="True" 
                                    onselectedindexchanged="ddlUserChannel_Add_SelectedIndexChanged">
                                    <asp:ListItem Value="-1" Selected="True">-- 请选择频道 --</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                    ControlToValidate="ddlUserChannel_Add" ErrorMessage="*" 
                                    ValidationGroup="StaAdd" InitialValue="-1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="style5">
                                所属栏目：
                            </td>
                            <td class="style2">
                                <asp:DropDownList ID="ddlUserCP_Add" runat="server" Width="164px" Height="20px">
                                    <asp:ListItem Value="-1" Selected="True">-- 请选择栏目 --</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                                    ControlToValidate="ddlUserCP_Add" ErrorMessage="*" 
                                    ValidationGroup="StaAdd" InitialValue="-1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="style5">
                                站点编号：
                            </td>
                            <td class="style2">
                                <asp:TextBox ID="tbStaID_Add" runat="server" Width="160px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                    ControlToValidate="tbStaID_Add" ErrorMessage="*" 
                                    ValidationGroup="StaAdd"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="style5">
                                站点名称：
                            </td>
                            <td class="style2">
                                <asp:TextBox ID="tbStaName_Add" runat="server" Width="160px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                    ControlToValidate="tbStaName_Add" ErrorMessage="*" 
                                    ValidationGroup="StaAdd"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="style5">&nbsp;</td>
                            <td class="style2">备注：如果新添加的站点已存在，将仅关联该站点和栏目。<br />
                                一个栏目可以多次关联同一站点，解除关联请转到<a href="setting2.aspx">栏目配置</a>。
                            </td>
                        </tr>
                        <tr>
                            <td class="style5">
                                
                            </td>
                            <td class="style2">
                                <asp:Button ID="btnAddOper" runat="server" Text="添加" 
                                    onclick="btnAddOper_Click" ValidationGroup="StaAdd" />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnAddCancel" runat="server" Text="取消" 
                                    onclick="btnAddCancel_Click" ValidationGroup="StaAddCel" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                
            </td>
        </tr>
    </table>
    
</asp:Content>




