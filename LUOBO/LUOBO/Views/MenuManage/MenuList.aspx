<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>MenuList</title>
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/Common.js" type="text/javascript"></script>
    <script src="../../Scripts/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <link href="../../Content/lhbStyle/stylesheets/bootstrap.min.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../../Content/lhbStyle/stylesheets/font-awesome.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../../Content/lhbStyle/stylesheets/se7en-font.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../../Content/lhbStyle/stylesheets/style.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../../Scripts/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/zTreeStyle/zTreeStyle.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        var MenuType = ["普通菜单", "右键菜单"];
        var MenuIConType = ["无图标", "样式图标", "文件图标"];
        var MenuNo = 0;

        $(function () {
            $("#btnAdd").click(function () { window.location.href = "/MenuManage/MenuItem"; });
            $("#btnAllocation").click(function () { window.location.href = "/MenuManage/MenuAllocation"; });

            GetUserMenuList();
        });

        function GetMenuICON(type, icon) {
            var result = "";

            switch (type) {
                case 0:
                    result = "";
                    break;
                case 1:
                    result = "<i class='" + icon + "'></i>";
                    break;
                case 2:
                    result = icon;
                    break;
            }

            return result;
        }

        function InitMenuTree(list) {
            var result = "";
            var item;
            for (var i = 0; i < list.length; i++) {
                var spase = "";
                MenuNo++;
                item = list[i];
                result += "<tr>";
                result += "<td>" + MenuNo + "</td>";
                for (var j = 0; j < item.M_LEVEL - 1; j++) {
                    spase += "　";
                }
                result += "<td>" + spase + item.M_NAME + "</td>";
                result += "<td>" + item.M_URL + "</td>";
                result += "<td>" + MenuType[item.M_TYPE] + "</td>";
                result += "<td style='text-align:center'>" + GetMenuICON(item.M_ICONTYPE, item.M_ICON) + "</td>";
                result += "<td>" + MenuIConType[item.M_ICONTYPE] + "</td>";
                result += "<td>" + item.M_REMARK + "</td>";
                result += "<td><a href='MenuItem?id=" + item.M_ID + "'>编辑</a> <a id='a_" + item.M_ID + "' href='javascript:SetMenuIsOn(" + item.M_ID + "," + (!item.M_ISON) + ")'>" + (item.M_ISON ? "禁用" : "启用") + "</a></td>";
                result += "</tr>";
                result += InitMenuTree(item.SubMenuList);
            }
            return result;
        }

        function GetUserMenuList() {
            $("#tbdMenuList").append("<tr><td colspan='8'>正在加载...</td></tr>");
            $.ajax({
                type: 'post', //可选get
                url: 'GetUserMenuList', //这里是接收数据的PHP程序
                data: 'type=all',
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    $("#tbdMenuList").empty();
                    $("#divPage").empty();
                    if (obj.ResultOBJ.length > 0) {
                        MenuNo = 0;
                        $("#tbdMenuList").append(InitMenuTree(obj.ResultOBJ));
                    }
                    else {
                        $("#tbdMenuList").append("<tr><td colspan='8'>无菜单信息!</td></tr>");
                    }
                }
            });
        }

        function SetMenuIsOn(id, ison) {
            $.ajax({
                type: 'post', //可选get
                url: 'SetMenuIsOn', //这里是接收数据的PHP程序
                data: 'id=' + id + '&ison=' + ison,
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (data) {
                    if (data.ResultCode == 0) {
                        $("#a_" + id).html(ison ? "禁用" : "启用");
                        $("#a_" + id).attr("href", "javascript:SetMenuIsOn(" + id + "," + (!ison) + ")");
                        alert("更新成功!");
                    }
                    else
                        alert(data.ResultMsg);
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
                    <div class="widget-content padded clearfix">
                        <div>
                            <input id="btnAdd" type="button" value="添加菜单" />
                            <input id="btnAllocation" type="button" value="分配权限" />
                        </div>
                        <br />
                        <table class="hor-minimalist-a  table table-bordered table-striped">
                            <thead>
                                <tr>
                                    <th scope="col">序号</th>
                                    <th scope="col">菜单名称</th>
                                    <th scope="col">页面路径</th>
                                    <th scope="col">菜单类型</th>
                                    <th scope="col">菜单图标</th>
                                    <th scope="col">菜单图标类型</th>
                                    <th scope="col">备注</th>
                                    <th scope="col">操作</th>
                                </tr>
                            </thead>
                            <tbody id="tbdMenuList">
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
