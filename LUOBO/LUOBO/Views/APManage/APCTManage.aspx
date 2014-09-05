<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AP配置模版管理</title>
    <script src="../../Scripts/Common.js" type="text/javascript"></script>
    <link href="../../Content/Site.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        

        var size = 10;
        var allPage = 0;
        var currentPage = 1;

        $(function () {
            FindAPCTList(1);

            $("#btnDel").click(function () { DelAPCT(); });
            $("#btnAdd").click(function () { AddAPCT(); });
//          $("#btnSearch").click(function () { FindAPCTList(1); });
        });

        function FindAPCTList(curPage) {
            $("#tbdAPCTList").html("<tr><td colspan='2'>正在加载...</td></tr>");
            currentPage = curPage;
            var result = "";
            $.ajax({
                type: 'post', //可选get
                url: 'FindAPCTList', //这里是接收数据的PHP程序
                data: 'size=' + size + '&curPage=' + curPage, //传给PHP的数据，多个参数用&连接
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    if (obj.ResultCode == 1) {
                        alert("操作失败,错误原因：" + obj.ResultMsg);
                    }
                    else {
                        $("#tbdAPCTList").empty();
                        $("#divPage").empty();
                        if (obj.ResultOBJ.APCTList.length > 0) {
                            var item;
                            for (var i = 0; i < obj.ResultOBJ.APCTList.length; i++) {
                                item = obj.ResultOBJ.APCTList[i];
                                result += "<tr>";
                                result += "<td><input name='ckAPCT' type='checkbox' value='" + item.ID + "' /></td>";
                                result += "<td>" + item.TNAME + "</td>";
                                result += "<td>" + item.DESCRIPTION + "</td>";
                                result += "<td>" + item.FIRMWARE + "</td>";
                                result += "<td>" + item.VERSION + "</td>";
                                result += "<td>" + new Date(+/\d+/.exec(item.UPDATETIME)).format("yyyy-MM-dd") + "</td>";
                                result += "<td><a href='APCTEdit?APCTID=" + item.ID + "'>编辑</a></td>";
                                result += "</tr>";
                            }
                            if (result != "")
                                $("#tbdAPCTList").append(result);
                            else
                                $("#tbdAPCTList").html("<tr><td colspan='2'>没有数据!</td></tr>");
                            allPage = obj.ResultOBJ.AllCount % size == 0 ? obj.ResultOBJ.AllCount / size : parseInt(obj.ResultOBJ.AllCount / size) + 1;
                            ShowPage({ CurrentPage: curPage, MaxPageSize: allPage, PageShowSize: 5, IsUpDown: true, ShowElement: document.getElementById("divPage"), PageEvents: function (page) { FindAPCTList(page); } });
                        }
                    }
                }
            });
        }

        function DelAPCT() {
            var ids = "";
            $("input[name='ckAPCT']:checked").each(function () {
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
                url: 'DelAPCT', //这里是接收数据的PHP程序
                data: 'ids=' + ids, //传给PHP的数据，多个参数用&连接
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    if (obj) {
                        FindAPCTList(currentPage);
                        alert("删除成功!");
                    }
                    else
                        alert("删除失败!");
                }
            });
        }

        function AddAPCT() {
            window.location.href = "APCTAdd";
        }

    </script>

</head>
<body>
    <div class="div-body">
        <div class="div-body-top">
            <table class="hor-minimalist-top">
                <tr>
                    <td>
                        <input id="btnAdd" value="新增" type="button" class="btn-normal"/>
                        <input id="btnDel" value="删除" type="button" class="btn-normal"/>
                    </td>
                </tr>
            </table>
        </div>
        <div class="div-body-bottom">
            <table class="hor-minimalist-a">
                <thead>
                    <tr>
                        <th scope="col">选择</th>
                        <th scope="col">模版名称</th>
                        <th scope="col">模版描述</th>
                        <th scope="col">适配固件</th>
                        <th scope="col">适配版本号</th>
                        <th scope="col">修改时间</th>
                        <th scope="col">操作</th>
                    </tr>
                </thead>
                <tbody id="tbdAPCTList">
                
                </tbody>
            </table>
        </div>
    </div>
    <div id="divPage" class="hor-minimalist-page"></div>
</body>
</html>
