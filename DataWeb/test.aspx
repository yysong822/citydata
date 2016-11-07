<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="test.aspx.cs" Inherits="test" Title="无标题页" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
<asp:GridView ID="GridView1" runat="server" 
        OnPageIndexChanging = "GridView1_PageIndexChanging" >
</asp:GridView>
    <asp:TreeView ID="TreeView1" runat="server" ImageSet="Arrows">
        <ParentNodeStyle Font-Bold="False" />
        <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
        <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" 
            HorizontalPadding="0px" VerticalPadding="0px" />
        <Nodes>
            <asp:TreeNode Text="父节点1" Value="父节点1">
                <asp:TreeNode Text="子节点1" Value="子节点1"></asp:TreeNode>
                <asp:TreeNode Text="子节点2" Value="子节点2"></asp:TreeNode>
            </asp:TreeNode>
            <asp:TreeNode Text="父节点2" Value="父节点2">
                <asp:TreeNode Text="子节点1" Value="子节点1"></asp:TreeNode>
            </asp:TreeNode>
        </Nodes>
        <NodeStyle Font-Names="Tahoma" Font-Size="10pt" ForeColor="Black" 
            HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />
    </asp:TreeView>
</asp:Content>

