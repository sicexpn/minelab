<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Default</title>
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
    <link rel="stylesheet" href="../../Scripts/jquery-ui.css">
    <script type="text/javascript">
        var jgName = "";
        var userName = "";
        var currentPage = 0;
        var size = 10;
        var allPage = 0;
        $(function () {
            $("#btnSearch").click(function () { FindUserList(1); });
            $("#btnDisable").click(function () { DisableUser(); });
            $("#btnAdd").click(function () { window.location.href = "User?type=0&id=0"; });

            FindUserList(1);
        });

        function FindUserList(curPage) {
            currentPage = curPage;
            jgName = $("#txtJG").val();
            userName = $("#txtRY").val();
            var result = "";
            $.ajax({
                type: 'post', //可选get
                url: 'FindUserList', //这里是接收数据的PHP程序
                data: 'jgName=' + jgName + '&userName=' + userName + '&size=' + size + '&curPage=' + curPage + "&userType=-99", //传给PHP的数据，多个参数用&连接
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    $("#tbdUserList").empty();
                    $("#divPage").empty();
                    if (obj.UserList.length > 0) {
                        var item;
                        for (var i = 0; i < obj.UserList.length; i++) {
                            item = obj.UserList[i];
                            result = "<tr>";
                            result += "<td><input name='ckUser' type='checkbox' value='" + item.ID + "' /></td>";
                            result += "<td>" + item.USERNAME + "</td>";
                            result += "<td>" + item.ACCOUNT + "</td>";
                            result += "<td>" + item.CONTACT + "</td>";
                            result += "<td>" + (item.ONAME == null ? "" : item.ONAME) + "</td>";
                            result += "<td>" + item.DNAME + "</td>";
                            result += "<td><a href='User?type=1&id=" + item.ID + "'>修改</a> <a href='Authorize?id=" + item.ID + "'>授权</a></td>";
                            result += "</tr>";
                            $("#tbdUserList").append(result);
                        }
                        allPage = obj.AllCount % size == 0 ? obj.AllCount / size : parseInt(obj.AllCount / size) + 1;
                        ShowPage({ CurrentPage: curPage, MaxPageSize: allPage, PageShowSize: 5, IsUpDown: true, ShowElement: document.getElementById("divPage"), PageEvents: function (page) { FindUserList(page); } });
                    }
                }
            });
        }

        function DisableUser() {
            var ids = "";
            $("input[name='ckUser']:checked").each(function () {
                if (ids != "")
                    ids += ",";
                ids += $(this).val();
            });

            if (ids == "") {
                alert("没有选择删除的数据!");
                return;
            }
            $.ajax({
                type: 'post', //可选get
                url: 'DisableUser', //这里是接收数据的PHP程序
                data: 'ids=' + ids, //传给PHP的数据，多个参数用&连接
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    if (obj) {
                        alert("停用成功!");
                        FindUserList(currentPage);
                    }
                    else
                        alert("停用失败!");
                }
            });
        }
    </script>
</head>
<body>
    <div class="container-fluid">
        <div class="row-fluid">
            <div class="div-body col-md-12">
                <div class="widget-container  clearfix">
                    <div class="heading"><i class="fa fa-fw fa-list-ul"></i>用户管理</div>
                    <div class="widget-content padded clearfix">
                        <div class="top-search">
                            机构名称：<input id="txtJG" type="text" style="width: 50" />
                            人员名称：<input id="txtRY" type="text" style="width: 50" />
                            <input id="btnSearch" type="button" value="查询" />
                            <input id="btnAdd" type="button" value="新增" />
                            <input id="btnDisable" type="button" value="停用" />
                        </div>
                        <br />
                        <table class="hor-minimalist-a  table table-bordered table-striped">
                            <thead>
                                <tr>
                                    <th scope="col">选择</th>
                                    <th scope="col">人员名称</th>
                                    <th scope="col">帐号</th>
                                    <th scope="col">联系方式</th>
                                    <th scope="col">所属机构</th>
                                    <th scope="col">用户类别</th>
                                    <th scope="col">操作</th>
                                </tr>
                            </thead>
                            <tbody id="tbdUserList">
                            </tbody>
                        </table>
                        <div id="divPage" class="hor-minimalist-page"></div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
