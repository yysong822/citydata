<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="user, App_Web_user.aspx.cdcab7d2" title="用户管理" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 250px;
            text-align: right;
        }
        .style2
        {
            text-align: left;
        }
        
        .style4
        {
            width: 68px;
            text-align: right;
        }
        .style5
        {
            width: 40px;
            text-align: left;
        }
                
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table style="width:100%;">
        <tr>
            <td>
                <asp:Panel ID="plView" runat="server">
                    <table style="width:100%;">
                        <tr>
                            <td>
                                <table style="width:100%;">
                                    <tr>
                                        <td class="style4">
                                            工号ID：
                                        </td>
                                        <td class="style2">
                                            <asp:TextBox ID="tbUserID_Query" runat="server" Width="80px"></asp:TextBox>
                                        </td>
                                        <td class="style4">
                                            用户名称：
                                        </td>
                                        <td class="style2">
                                            <asp:TextBox ID="tbUserName_Query" runat="server" Width="100px"></asp:TextBox>
                                        </td>
                                        <td class="style5">
                                            角色：
                                        </td>
                                        <td class="style2">
                                            <asp:DropDownList ID="ddlUserRole_Query" runat="server" Width="60px" Height="20px">
                                            
                                                <asp:ListItem Selected="True" Value="-1">所有</asp:ListItem>
                                                <asp:ListItem Value="1">制片人</asp:ListItem>
                                                <asp:ListItem Value="0">管理员</asp:ListItem>
                                            
                                            </asp:DropDownList>
                                        </td>
                                        <td class="style5">
                                            状态：
                                        </td>
                                        <td class="style2">
                                            <asp:DropDownList ID="ddlUserState_Query" runat="server" Width="60px" Height="20px">
                                            
                                                <asp:ListItem Selected="True" Value="-1">所有</asp:ListItem>
                                                <asp:ListItem Value="0">启用</asp:ListItem>
                                                <asp:ListItem Value="1">禁用</asp:ListItem>
                                            
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnQuery" runat="server" Text="查询" onclick="btnQuery_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="False" PageSize="30" 
                                    DataKeyNames="UserID" AllowPaging="True" GridLines="None" Width="100%" 
                                    CellPadding="4" onrowcommand="gvUsers_RowCommand" 
                                    onrowdeleting="gvUsers_RowDeleting" onrowediting="gvUsers_RowEditing" 
                                    onrowupdating="gvUsers_RowUpdating" 
                                    onpageindexchanging="gvUsers_PageIndexChanging" 
                                    onrowdatabound="gvUsers_RowDataBound" 
                                    onrowcancelingedit="gvUsers_RowCancelingEdit" >
                                    
                                    <Columns>
                                        <asp:CommandField ShowEditButton="True" CancelText="取消" DeleteText="删除" 
                                            EditText="编辑" UpdateText="更新" ValidationGroup="UserDetailEdit" />
                                        <asp:BoundField DataField="UserID" HeaderText="工号ID" ReadOnly="True" 
                                            SortExpression="UserID" />
                                        <asp:TemplateField HeaderText="用户名称" SortExpression="UserName">
                                            <ItemTemplate>
                                                <asp:Label ID="lbUserName_Item" runat="server" Text='<%# Bind("UserName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="tbUserName_Edit" runat="server" Text='<%# Bind("UserName") %>' 
                                                    Width="100px" ValidationGroup="UserDetailEdit"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                                    ControlToValidate="tbUserName_Edit" ErrorMessage="*" 
                                                    ValidationGroup="UserDetailEdit"></asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Password" HeaderText="" SortExpression="Password" Visible="False" />
                                        <asp:TemplateField HeaderText="用户角色" SortExpression="UserRole">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" 
                                                    Text='<%# Eval("UserRole").ToString() == "0" ? "管理员" : "制片人" %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlUserRole_Edit" runat="server" 
                                                    SelectedValue='<%# Bind("UserRole") %>' Width="80px" Height="20px">
                                                    <asp:ListItem Value="1">制片人</asp:ListItem>
                                                    <asp:ListItem Value="0">管理员</asp:ListItem>
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="用户状态" SortExpression="UserState">
                                            <ItemTemplate>
                                                <asp:Label ID="Label2" runat="server" 
                                                    Text='<%# Eval("UserState").ToString() == "0" ? "启用" : "禁用" %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlUserState_Edit" runat="server" 
                                                    SelectedValue='<%# Bind("UserState") %>' Width="80px" Height="20px">
                                                    <asp:ListItem Value="0">启用</asp:ListItem>
                                                    <asp:ListItem Value="1">禁用</asp:ListItem>
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="UserComm" HeaderText="用户备注" 
                                            SortExpression="UserComm" Visible="False" />
                                        <asp:TemplateField ShowHeader="False" HeaderText="密码">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnResetPwd" runat="server" CausesValidation="False" 
                                                    CommandArgument='<%# Eval("UserID") %>' CommandName="Reset" Text="重置"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:CommandField ShowDeleteButton="True" DeleteText="删除" />
                                    </Columns>
                                    
                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                    <PagerStyle BackColor="#284775" ForeColor="White" />
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
                                    Font-Underline="True" onclick="lbtnShowAdd_Click">点击此处添加用户</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td></td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="plAdd" runat="server">
                    <table style="width:100%;">
                        <tr>
                            <td class="style1"></td>
                            <td style="font-size:large; font-style: italic; text-align: left;">
                                请输入用户信息
                            </td>
                        </tr>
                        <tr>
                            <td class="style1"></td>
                            <td class="style2">
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                    ControlToValidate="tbUserID_Add" ErrorMessage="* 工号ID必须为6位数字！" 
                                    ValidationGroup="UserAdd" ValidationExpression="^\d{6}$"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                工号ID：
                            </td>
                            <td class="style2">
                                <asp:TextBox ID="tbUserID_Add" runat="server" Width="160px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                    ControlToValidate="tbUserID_Add" ErrorMessage="*" 
                                    ValidationGroup="UserAdd"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                用户名称：
                            </td>
                            <td class="style2">
                                <asp:TextBox ID="tbUserName_Add" runat="server" Width="160px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                    ControlToValidate="tbUserName_Add" ErrorMessage="*" 
                                    ValidationGroup="UserAdd"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                用户角色：
                            </td>
                            <td class="style2">
                                <asp:RadioButtonList ID="rblUserRole_Add" runat="server" 
                                    RepeatDirection="Horizontal">
                                
                                    <asp:ListItem Selected="True" Value="1">制片人</asp:ListItem>
                                    <asp:ListItem Value="0">管理员</asp:ListItem>
                                
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                用户状态：
                            </td>
                            <td class="style2">
                                <asp:RadioButtonList ID="rblUserState_Add" runat="server" 
                                    RepeatDirection="Horizontal">
                                
                                    <asp:ListItem Selected="True" Value="0">启用√</asp:ListItem>
                                    <asp:ListItem Value="1">禁用×</asp:ListItem>
                                
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                用户备注：<br />
                                （可选）
                            </td>
                            <td class="style2">
                                <asp:TextBox ID="tbUserComm_Add" runat="server" TextMode="MultiLine" Height="77px" 
                                    Width="160px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                
                            </td>
                            <td class="style2">
                                备注：用户初始密码为6个连续的数字零。
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                
                            </td>
                            <td class="style2">
                                <asp:Button ID="btnAddOper" runat="server" Text="添加" 
                                    onclick="btnAddOper_Click" ValidationGroup="UserAdd" />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnAddCancel" runat="server" Text="取消" 
                                    onclick="btnAddCancel_Click" ValidationGroup="UserAddCel" />
                            </td>
                        </tr>
                    </table>
                    
                </asp:Panel>
            </td>
        </tr>
    </table>
    
</asp:Content>

