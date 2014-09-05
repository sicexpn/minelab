<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Authorize</title>
    <script src="../../Scripts/Common.js" type="text/javascript"></script>
    <script src="../../Content/lhbStyle/javascripts/bootstrap.min.js" type="text/javascript"></script>
    <script src="../../Content/lhbStyle/javascripts/jquery.mousewheel.js" type="text/javascript"></script>
    <script src="../../Content/lhbStyle/javascripts/jquery.dataTables.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.10.2.js"></script>
    <script src="../../Scripts/jquery-ui.js"></script>

    <link href="../../Content/lhbStyle/stylesheets/bootstrap.min.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../../Content/lhbStyle/stylesheets/font-awesome.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../../Content/lhbStyle/stylesheets/se7en-font.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../../Content/lhbStyle/stylesheets/style.css" media="all" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../../Scripts/jquery-ui.css" />
    <script type="text/javascript">
        var uid = 0;
        var userAppCompetence = "";
        var ckAppcompetencIDs = "";
        $(function () {
            uid = parseInt($("#hdUID").val());
            $("#btnAll").click(function () {
                if ($("input[name='ckAppcompetenc']").length != $("input[name='ckAppcompetenc']:checked").length)
                    $("input[name='ckAppcompetenc']").prop("checked", true);
                else
                    $("input[name='ckAppcompetenc']").prop("checked", false);

                GetSelectAC();
            });
            $("#btnAdd").click(function () { AddUserAppCompetence(); });

            GetUserAppCompetence();
        });

        function GetUserAppCompetence() {
            $.ajax({
                type: 'post', //可选get
                url: 'GetUserAppCompetence', //这里是接收数据的PHP程序
                data: 'uid=' + uid, //传给PHP的数据，多个参数用&连接
                dataType: 'text', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    userAppCompetence = obj;
                    FindAppcompetencList();
                }
            });
        }

        function FindAppcompetencList() {
            var result = "";
            $.ajax({
                type: 'post', //可选get
                url: 'FindAppcompetencList', //这里是接收数据的PHP程序
                //data: 'uid=0', //传给PHP的数据，多个参数用&连接
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    $("#tbdAppCompetencList").empty();
                    if (obj.length > 0) {
                        var item;
                        for (var i = 0; i < obj.length; i++) {
                            item = obj[i];
                            result = "<tr>";
                            if (userAppCompetence.indexOf("$" + item.ID + "$") == -1) {
                                item.isSelect = false;
                                result += "<td><input name='ckAppcompetenc' type='checkbox' value='" + item.ID + "' /></td>";
                            }
                            else {
                                item.isSelect = true;
                                result += "<td><input name='ckAppcompetenc' type='checkbox' value='" + item.ID + "' checked='checked' /></td>";
                            }
                            result += "<td>" + item.APPLICATIONNAME + "</td>";
                            result += "<td>" + item.NAME + "</td>";
                            result += "<td>" + item.CONTROLLER + "</td>";
                            result += "<td>" + item.ACTION + "</td>";
                            result += "</tr>";
                            $("#tbdAppCompetencList").append(result);
                        }

                        $("input[name='ckAppcompetenc']").click(function () { GetSelectAC(); });
                        GetSelectAC();
                    }
                }
            });
        }

        function GetSelectAC() {
            ckAppcompetencIDs = "";
            $("input[name='ckAppcompetenc']:checked").each(function () {
                if (ckAppcompetencIDs != "")
                    ckAppcompetencIDs += ",";
                ckAppcompetencIDs += $(this).val();
            });
        }

        function AddUserAppCompetence() {
            $.ajax({
                type: 'post', //可选get
                url: 'AddUserAppCompetence', //这里是接收数据的PHP程序
                data: 'appcids=' + ckAppcompetencIDs + "&uid=" + uid, //传给PHP的数据，多个参数用&连接
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    alert(obj.ResultMsg);
                }
            });
        }
    </script>
</head>
<body>
    <input id="hdUID" type="hidden" value="<%=ViewData["uid"]%>" />
    <div class="container-fluid">
        <div class="row-fluid">
            <div class="div-body col-md-12">
                <div class="widget-container  clearfix">
                    <div class="heading"><i class="fa fa-fw fa-list-ul"></i>用户授权</div>
                    <div class="widget-content padded clearfix">
                        <div class="top-search">
                            <input id="btnAll" type="button" value="全选" />
                            <input id="btnAdd" type="button" value="授权" />
                        </div>
                        <br />
                        <table class="hor-minimalist-a  table table-bordered table-striped">
                            <thead>
                                <tr>
                                    <th scope="col">选择</th>
                                    <th scope="col">应用名称</th>
                                    <th scope="col">权限名称</th>
                                    <th scope="col">Controller名称</th>
                                    <th scope="col">Action名称</th>
                                </tr>
                            </thead>
                            <tbody id="tbdAppCompetencList">
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
