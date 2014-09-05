<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="error.aspx.cs" Inherits="LUOBO.SingleShop.UI.error" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>错误</title>
    <script src="../UITemplet/js/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../UITemplet/js/Common.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
    <script type="text/javascript">
        var type = parseInt(GetQueryString("type"));
        switch (type) {
            case -99:
                $("body").html("没有权限!");
                break;
        }
    </script>
</body>
</html>
