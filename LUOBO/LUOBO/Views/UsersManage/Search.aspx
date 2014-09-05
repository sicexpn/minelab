<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Default</title>
    <script src="../../Scripts/Common.js" type="text/javascript"></script>
    <script src="../../Scripts/Common.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.ztree.core-3.5.min.js" type="text/javascript"></script>
    <script src="../../Scripts/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <link href="../../Content/Site.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/zTreeStyle/zTreeStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/lhbStyle/stylesheets/bootstrap.min.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../../Content/lhbStyle/stylesheets/font-awesome.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../../Content/lhbStyle/stylesheets/se7en-font.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../../Content/lhbStyle/stylesheets/style.css" media="all" rel="stylesheet" type="text/css" />
    <script src="../../Content/lhbStyle/javascripts/bootstrap.min.js" type="text/javascript"></script>
    <script src="../../Content/lhbStyle/javascripts/jquery.mousewheel.js" type="text/javascript"></script>
    <script src="../../Content/lhbStyle/javascripts/jquery.dataTables.min.js" type="text/javascript"></script>
<!--<script src="../../Content/lhbStyle/javascripts/bootstrap-editable.min.js" type="text/javascript"></script>
    <script src="../../Content/lhbStyle/javascripts/xeditable-demo.js" type="text/javascript"></script>-->
    <link rel="stylesheet" href="../../Scripts/jquery-ui.css">
    <script src="../../Scripts/jquery-1.10.2.js"></script>
    <script src="../../Scripts/jquery-ui.js"></script>
    <script type="text/javascript">
        var jgName = "";
        var userName = "";
        var userType = -99;
        var size = 10;
        var allPage = 0;
        $(function () {
            $("#btnSearch").click(function () { FindUserList(1); });

            $.ajax({
                type: 'post', //可选get
                url: 'GetUserTypeList', //这里是接收数据的PHP程序
                //data: 'jgName=' + jgName + '&userName=' + userName + '&size=' + size + '&curPage=' + curPage + "&userType=" + userType, //传给PHP的数据，多个参数用&连接
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    for (var i = 0; i < obj.length; i++) {
                        $("#slcUT").append("<option value='" + obj[i].VALUE + "'>" + obj[i].NAME + "</option>");
                    }
                }
            });
        });

        function FindUserList(curPage) {
            jgName = $("#txtJG").val();
            userName = $("#txtRY").val();
            userType = $("#slcUT").val();
            var result = "";
            $.ajax({
                type: 'post', //可选get
                url: 'FindUserList', //这里是接收数据的PHP程序
                data: 'jgName=' + jgName + '&userName=' + userName + '&size=' + size + '&curPage=' + curPage + "&userType=" + userType, //传给PHP的数据，多个参数用&连接
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    $("#tbdUserList").empty();
                    $("#divPage").empty();
                    if (obj.UserList.length > 0) {
                        var item;
                        for (var i = 0; i < obj.UserList.length; i++) {
                            item = obj.UserList[i];
                            result = "<tr>";
                            result += "<td>" + item.USERNAME + "</td>";
                            result += "<td>" + item.ACCOUNT + "</td>";
                            result += "<td>" + item.CONTACT + "</td>";
                            result += "<td>" + (item.ONAME == null ? "" : item.ONAME) + "</td>";
                            result += "<td>" + item.DNAME + "</td>";
                            result += "</tr>";
                            $("#tbdUserList").append(result);
                        }
                        allPage = obj.AllCount % size == 0 ? obj.AllCount / size : parseInt(obj.AllCount / size) + 1;
                        ShowPage({ CurrentPage: curPage, MaxPageSize: allPage, PageShowSize: 5, IsUpDown: true, ShowElement: document.getElementById("divPage"), PageEvents: function (page) { FindUserList(page); } });
                    }
                }
            });
        }
    </script>
</head>
<body>

<div class="container-fluid">
      <div  class="row-fluid">
        <div class="div-body col-md-12">
        <div class="widget-container  clearfix">
            <div class="heading"><i class="fa fa-fw fa-list-ul"></i>用户管理

            </div>
            <div class="widget-content padded clearfix">
                        <div class="top-search">
              机构名称：<input id="txtJG" type="text" style="width:50" />
              人员名称：<input id="txtRY" type="text" style="width:50" />
                        <select id="slcUT">
                            <option value="-99" selected>全部</option>
                        </select>
              <input id="btnSearch" type="button" value="查询" />
              </div>
             <br></br>


              <table class="hor-minimalist-a  table table-bordered table-striped">
<thead>
                    <tr>
                        <th scope="col">人员名称</th>
                        <th scope="col">帐号</th>
                        <th scope="col">联系方式</th>
                        <th scope="col">所属机构</th>
                        <th scope="col">用户类别</th>
                    </tr>
                </thead>
                <tbody id="tbdUserList">
                </tbody>
            </table>
            <div id="div1" class="hor-minimalist-page"></div>
            </div>
            
            </div>
        </div>
        </div>
      

    </div>
    
</body>
</html>
