<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Default</title>
    <script src="../../Scripts/Common.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.10.2.js"></script>
    <script src="../../Content/lhbStyle/javascripts/bootstrap.min.js" type="text/javascript"></script>
    <script src="../../Content/lhbStyle/javascripts/jquery.mousewheel.js" type="text/javascript"></script>
    <script src="../../Content/lhbStyle/javascripts/jquery.dataTables.min.js" type="text/javascript"></script>

    <script src="../../Scripts/jquery-ui.js"></script>
    <script src="../../Scripts/jquery.ztree.core-3.5.min.js" type="text/javascript"></script>

    <link href="../../Content/lhbStyle/stylesheets/bootstrap.min.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../../Content/lhbStyle/stylesheets/font-awesome.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../../Content/lhbStyle/stylesheets/se7en-font.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../../Content/lhbStyle/stylesheets/style.css" media="all" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../../Scripts/jquery-ui.css" />
    <link href="../../Content/zTreeStyle/zTreeStyle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">

        var cname = "";
        var AppID = 0;
        var currentPage = 0;
        var size = 10;
        var allPage = 0;
        $(function () {
            $("#btnSearch").click(function () { FindAppcompetencList(1); });
            $("#btnDisable").click(function () { DisableUser(); });
            $("#btnAdd").click(function () { window.location.href = "AppCompetenc?type=0&id=0"; });

            FindAppcompetencList(1);
            FindAPPList();
        });

        function FindAppcompetencList(curPage) {
            currentPage = curPage;
            cname = $("#txtName").val();
            var result = "";
            $.ajax({
                type: 'post', //可选get
                url: 'FindAppcompetencList', //这里是接收数据的PHP程序
                data: 'size=' + size + '&curPage=' + curPage + '&name=' + cname + '&appID=' + AppID, //传给PHP的数据，多个参数用&连接
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    $("#tbdAppcompetencList").empty();
                    $("#divPage").empty();
                    if (obj.AppcompetencList.length > 0) {
                        var item;
                        for (var i = 0; i < obj.AppcompetencList.length; i++) {
                            item = obj.AppcompetencList[i];
                            result = "<tr>";
                            result += "<td><input name='ckAppcompetenc' type='checkbox' value='" + item.ID + "' /></td>";
                            result += "<td>" + item.APPLICATIONNAME + "</td>";
                            result += "<td>" + item.NAME + "</td>";
                            result += "<td>" + item.CONTROLLER + "</td>";
                            result += "<td>" + item.ACTION + "</td>";
                            result += "<td><a href='AppCompetenc?type=1&id=" + item.ID + "'>修改</a>";
                            result += "</tr>";
                            $("#tbdAppcompetencList").append(result);
                        }
                        allPage = obj.AllCount % size == 0 ? obj.AllCount / size : parseInt(obj.AllCount / size) + 1;
                        ShowPage({ CurrentPage: curPage, MaxPageSize: allPage, PageShowSize: 5, IsUpDown: true, ShowElement: document.getElementById("divPage"), PageEvents: function (page) { FindAppcompetencList(page); } });
                    }
                }
            });
        }

        //-------下拉列表树---------
        var setting = {
            view: {
                dblClickExpand: false,
                selectedMulti: false
            },
            data: {
                simpleData: {
                    enable: true
                }
            },
            callback: {
                onClick: onClick
            }
        };

        var zNodes = new Array();
        var tid = "";
        function FindAPPList() {
            $.ajax({
                type: 'post', //可选get
                url: 'FindAPPList', //这里是接收数据的PHP程序
                //data: 'jgID=' + POID,
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    zNodes.length = 0;
                    var data;
                    data = new Object();
                    data.id = 0;
                    data.name = "全部";
                    data.pId = -1;
                    zNodes.push(data);

                    if (obj.length > 0) {
                        for (var i = 0; i < obj.length; i++) {
                            data = new Object();
                            data.id = obj[i].ID;
                            data.name = obj[i].APPLICATIONNAME;
                            data.pId = -1;
                            zNodes.push(data);
                        }
                    }

                    $.fn.zTree.init($("#treeDemo"), setting, zNodes);
                    var zTreeObj = $.fn.zTree.getZTreeObj("treeDemo");
                    var zNodeObj = zTreeObj.getNodes()[0];
                    zTreeObj.selectNode(zNodeObj, false);
                    onClick(null, null, zNodeObj);
                }
            });
        }

        function onClick(e, treeId, treeNode) {
            var zTree = $.fn.zTree.getZTreeObj("treeDemo"),
			nodes = zTree.getSelectedNodes(),
			v = "";
            nodes.sort(function compare(a, b) { return a.id - b.id; });
            for (var i = 0, l = nodes.length; i < l; i++) {
                v += nodes[i].name + ",";
                AppID = nodes[i].id;
            }
            if (v.length > 0) v = v.substring(0, v.length - 1);
            var cityObj = $("#citySel");
            cityObj.attr("value", v);
        }

        function showMenu() {
            var cityObj = $("#citySel");
            var cityOffset = $("#citySel").offset();
            $("#menuContent").css({ left: cityOffset.left + "px", top: cityOffset.top + cityObj.outerHeight() + "px" }).slideDown("fast");

            $("body").bind("mousedown", onBodyDown);
        }
        function hideMenu() {
            $("#menuContent").fadeOut("fast");
            $("body").unbind("mousedown", onBodyDown);
        }
        function onBodyDown(event) {
            if (!(event.target.id == "menuBtn" || event.target.id == "menuContent" || $(event.target).parents("#menuContent").length > 0)) {
                hideMenu();
            }
        }
        //-------下拉列表树---------
    </script>
</head>
<body>
    <div class="container-fluid">
        <div class="row-fluid">
            <div class="div-body col-md-12">
                <div class="widget-container  clearfix">
                    <div class="heading"><i class="fa fa-fw fa-list-ul"></i>应用权限管理</div>
                    <div class="widget-content padded clearfix">
                        <div class="top-search">
                            权限名称：<input id="txtName" type="text" style="width: 50" />
                            引用：<input id="citySel" type="text" readonly value="" style="width:120px;"/>
                            <a id="menuBtn" href="#" onclick="showMenu(); return false;">选择</a>
                            <input id="btnSearch" type="button" value="查询" />
                            <input id="btnAdd" type="button" value="新增" />
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
                                    <th scope="col">操作</th>
                                </tr>
                            </thead>
                            <tbody id="tbdAppcompetencList">
                            </tbody>
                        </table>
                        <div id="divPage" class="hor-minimalist-page"></div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="menuContent" class="menuContent" style="display:none; position: absolute;">
	    <ul id="treeDemo" class="ztree" style="margin-top:0; width:160px;"></ul>
    </div>
</body>
</html>