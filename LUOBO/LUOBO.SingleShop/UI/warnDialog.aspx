<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="warnDialog.aspx.cs" Inherits="LUOBO.SingleShop.UI.warnDialog" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>预警信息</title>
    <link href="../UITemplet/css/bootstrap.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/css/font-awesome.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/css/index.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/css/style.css" media="all" rel="stylesheet" type="text/css" />
    <script src="../UITemplet/js/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../UITemplet/js/Common.js"></script>
    <script language="javascript" type="text/javascript">
        var token = GetQueryString("token");

        $(document).ready(function () {
            GetWarnList();
        });

        function GetWarnList() {
            $.ajax({
                type: 'post',
                url: 'AjaxComm.aspx',
                data: 'type=GetAlertListNotHandle&token=' + token,
                dataType: 'json',
                error: function (msg) {
                    alert("服务器错误");
                },
                success: function (obj) {
                    if (obj.ResultCode == 0) {
                        $("#warnlist").empty();
                        for (var i = 0; i < obj.ResultOBJ.length; i++) {
                            $("#warnlist").append("<tr><td>" + obj.ResultOBJ[i].APNAME + "</td><td>" + obj.ResultOBJ[i].G_SSID + "</td><td>" + obj.ResultOBJ[i].KEYWORD + "</td></tr>");
                        }
                    }
                }
            });
        }
    </script>
</head>
<body>
    <div class="container-fluid">
        <div  class="row-fluid">
            <div class="div-body  col-md-12" id="warn-list" name="warn-list">
                <div class="panel">
                    <div class="panel-heading">发现SSID预警信息：
                    </div>
                        <table class="table table-bordered table-striped" width="95%">
                            <tr>
                                <td>发射器</td>
                                <td>危险SSID</td>
                                <td>关键词</td>
                            </tr>
                            <tbody id="warnlist">
                            </tbody>
                        </table>
                </div>
            </div>
       </div>  
    </div>
</body>
</html>
