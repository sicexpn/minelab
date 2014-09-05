<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CacheManage.aspx.cs" Inherits="LUOBO.SingleShop.UI.CacheManage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:GridView ID="gvCacheList" runat="server" AutoGenerateColumns="False" 
            onrowcommand="gvCacheList_RowCommand">
            <Columns>
                <asp:BoundField HeaderText="缓存名称" DataField="Name" />
                <asp:ButtonField HeaderText="操作" Text="清除缓存" />
            </Columns>
        </asp:GridView>
    </div>
    </form>
</body>
</html>
