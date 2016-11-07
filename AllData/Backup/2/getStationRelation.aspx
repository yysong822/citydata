<%@ Page Language="C#" AutoEventWireup="true" CodeFile="getStationRelation.aspx.cs" Inherits="getStationRelation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>获得城市列表</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    获得城市列表
    <br />
    <asp:Button ID="bt_getStation" runat="server" Text="Button" 
            onclick="bt_getStation_Click" />
    </div>
    </form>
</body>
</html>
