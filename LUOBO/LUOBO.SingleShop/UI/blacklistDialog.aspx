<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="blacklistDialog.aspx.cs" Inherits="LUOBO.SingleShop.UI.blacklist" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>预警信息</title>
    <link href="../UITemplet/css/bootstrap.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/css/font-awesome.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/css/index.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/css/style.css" media="all" rel="stylesheet" type="text/css" />


    <script src="../UITemplet/js/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../UITemplet/js/Common.js"></script>
    <script language="javascript" type="text/javascript">
        var token = GetQueryString("token");

        var addkeyword = GetQueryString("addkeyword");

        $(document).ready(function () {
            GetBlackList();
            $("#add-item").val(addkeyword);
        });

        function GetBlackList() {
            $.ajax({
                type: 'post',
                url: 'AjaxComm.aspx',
                data: 'type=GetAlertKeyWord&token=' + token,
                dataType: 'json',
                error: function (msg) {
                    alert("服务器错误");
                },
                success: function (obj) {
                    if (obj.ResultCode == 0) {
                        $("#warnlist").empty();
                        for (var i = 0; i < obj.ResultOBJ.length; i++) {
                            $("#warnlist").append("<tr><td>" + obj.ResultOBJ[i].ID + "</td><td>" + obj.ResultOBJ[i].KEYWORD + "</td><td>" + "<span class='badge pull-right' style='cursor:pointer;' onclick='javascript:removeitem(" + obj.ResultOBJ[i].ID + ");'>删除</span>" + "</td></tr>");
                        }
                    }
                }
            });
        }

        function AddBlackList() {
            var addItem = $("#add-item").val();
            if (addItem == '') {
                alert("请填入关键词");
                return;
            }
            $.ajax({
                type: 'post',
                url: 'AjaxComm.aspx',
                data: 'type=AddAlertKeyWord&token=' + token +'&param="' + addItem + '"',
                dataType: 'json',
                error: function (msg) {
                    alert("服务器错误");
                },
                success: function (obj) {
                    if (obj.ResultCode == 0) {
                        alert(obj.ResultMsg);
                        GetBlackList();
                    }
                }
            });
        }

        function removeitem(delitem) {
            $.ajax({
                type: 'post',
                url: 'AjaxComm.aspx',
                data: 'type=DelAlertKeyWord&token=' + token + '&param=' + delitem,
                dataType: 'json',
                error: function (msg) {
                    alert("服务器错误");
                },
                success: function (obj) {
                    if (obj.ResultCode == 0) {
                        alert(obj.ResultMsg);
                        GetBlackList();
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
                    <div class="panel-heading">
                                    <span class="label label-primary" style="font-size:14px">敏感词列表：</span>
                <span>
                    <input id="add-item" type="text" class="input-sm" value="" placeholder="添加敏感词.."/>
                    <a class="btn btn-sm btn-success" href="#" onclick="AddBlackList();"><i class="icon-twitter"></i>添加</a>
                </span>
                <span class="label label-success pull-right" style="font-size:14px;cursor:pointer;" onclick="GetBlackList();">刷新</span>
                    </div>
                        <table class="table table-bordered table-striped" width="95%">
                            <tr>
                                <td>ID</td>
                                <td>关键词</td>
                                <td>操作</td>
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

