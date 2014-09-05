<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>AppCompetenc</title>
    <script src="../../Scripts/Common.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.10.2.js"></script>
    <script src="../../Scripts/jquery-ui.js"></script>
    <script src="../../Scripts/jquery.ztree.core-3.5.min.js" type="text/javascript"></script>
    <script src="../../Scripts/ajaxfileupload.js" type="text/javascript"></script>
    <script src="../../Content/lhbStyle/javascripts/bootstrap.min.js" type="text/javascript"></script>
    <script src="../../Content/lhbStyle/javascripts/jquery.mousewheel.js" type="text/javascript"></script>
    <script src="../../Content/lhbStyle/javascripts/jquery.dataTables.min.js" type="text/javascript"></script>

    <link href="../../Content/Site.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/lhbStyle/stylesheets/bootstrap.min.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../../Content/lhbStyle/stylesheets/font-awesome.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../../Content/lhbStyle/stylesheets/se7en-font.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../../Content/lhbStyle/stylesheets/style.css" media="all" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../../Scripts/jquery-ui.css" />
    <link href="../../Content/zTreeStyle/zTreeStyle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        var AppID = 0;
        var POID = 0;
        $(function () {
            AppID = parseInt($("#hdAppID").val());

            $("input[name='regCount']").click(function () { $("tbody[name='tbdBody']").hide(); $("#tbd" + $(this).val()).show(); });

            if ($("#hdType").val() != "0")
                $("#trMode").hide();

            $("#btnSubmit").click(function () {
                if ($("#hdType").val() == "0") {
                    if ($("input[name='regCount']:checked").val() == "One")
                        AddApp();
                    else
                        ImportApp();
                }
                else
                    UpdateApp();
            });

            FindAPPList();
        });

        var id = 0;
        var name = "";
        var controller = "";
        var action = "";
        function AddApp() {
            if (!GetAppInfo())
                return;
            $.ajax({
                type: 'post', //可选get
                url: 'AddAppCompetenc', //这里是接收数据的PHP程序
                data: 'APPID=' + AppID + '&NAME=' + name + '&CONTROLLER=' + controller + '&ACTION=' + action, //传给PHP的数据，多个参数用&连接
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    alert(obj.ResultMsg);
                }
            });
        }

        function UpdateApp() {
            id = $("#hdID").val();
            if (!GetAppInfo())
                return;
            $.ajax({
                type: 'post', //可选get
                url: 'UpdateAppCompetenc', //这里是接收数据的PHP程序
                data: "ID=" + id + '&APPID=' + AppID + '&NAME=' + name + '&CONTROLLER=' + controller + '&ACTION=' + action, //传给PHP的数据，多个参数用&连接
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    if (obj)
                        alert("修改成功!");
                    else
                        alert("修改失败!");
                }
            });
        }

        function ImportApp() {
            $.ajaxFileUpload({
                url: 'ImportAppCompetenc',
                secureuri: false,
                fileElementId: 'btnImport',
                dataType: 'json',
                data: { AppID: AppID },
                success: function (obj, status) {
                    alert(obj.ResultMsg);
                },
                error: function (data, status, e) {
                    alert("提交失败");
                }
            });
        }

        function GetAppInfo() {
            //应用判断
            if (AppID == 0) {
                alert("请选择应用!");
                return false;
            }
            //名称判断
            if ($("#txtNAME").val().Trim() == "") {
                alert("请输入名称!");
                return false;
            }
            else
                name = $("#txtNAME").val().Trim();
            //Controller名称判断
            if ($("#txtCONTROLLER").val().Trim() == "") {
                alert("请输入Controller名称!");
                return false;
            }
            else
                controller = $("#txtCONTROLLER").val().Trim();
            //Action名称判断
            if ($("#txtACTION").val().Trim() == "") {
                alert("请输入Action名称!");
                return false;
            }
            else
                action = $("#txtACTION").val().Trim();
            return true;
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
                onClick: onClick,
                onNodeCreated: onNodeCreated
            }
        };

        var zNodes = new Array();
        var POIDS = "";
        var tid = "";
        function FindAPPList() {
            $.ajax({
                type: 'post', //可选get
                url: 'FindAPPList', //这里是接收数据的PHP程序
                //data: 'jgID=' + POID,
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    zNodes.length = 0;
                    if (obj.length > 0) {
                        var data;
                        for (var i = 0; i < obj.length; i++) {
                            data = new Object();
                            data.id = obj[i].ID;
                            data.name = obj[i].APPLICATIONNAME;
                            data.pId = 0;
                            zNodes.push(data);
                        }
                    }

                    $.fn.zTree.init($("#treeDemo"), setting, zNodes);
                    var zTreeObj = $.fn.zTree.getZTreeObj("treeDemo");
                    var zNodeObj = zTreeObj.getNodeByTId(tid);
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

        function onNodeCreated(e, treeId, treeNode) {
            if (treeNode.id == AppID)
                tid = treeNode.tId;
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
                    <div class="heading">
                        <i class="fa fa-fw fa-list-ul"></i>新增应用权限
                    </div>
                    <div class="widget-content padded clearfix">
                        <input id="hdType" type="hidden" value="<%=ViewData["type"]%>" />
                        <input id="hdID" type="hidden" value="<%=((LUOBO.Entity.SYS_APPCOMPETENC)ViewData["appcompetenc"]).ID%>" />
                        <input id="hdAppID" type="hidden" value="<%=Convert.ToInt32(ViewData["type"]) == 1 ? ((LUOBO.Entity.SYS_APPCOMPETENC)ViewData["appcompetenc"]).APPID : 0%>" />
                        <table class="table table-bordered table-striped">
                            <tr>
                                <td style="width: 120px; text-align: right">应用：</td>
                                <td>
                                    <input id="citySel" type="text" readonly value="" style="width:120px;"/>
                                    <a id="menuBtn" href="#" onclick="showMenu(); return false;">选择</a>
                                </td>
                            </tr>
                            <tr id="trMode">
                                <td style="width: 120px; text-align: right">新增模式：</td>
                                <td>
                                    <label><input id="rdoOne" type="radio" name="regCount" value="One" checked="checked" />单条</label>
                                    <label><input id="rdoMost" type="radio" name="regCount" value="Most" />批量</label>
                                </td>
                            </tr>
                            <tbody id="tbdOne" name="tbdBody">
                                <tr>
                                    <td style="width: 120px; text-align: right">名称：</td>
                                    <td>
                                        <input id="txtNAME" type="text" value="<%=((LUOBO.Entity.SYS_APPCOMPETENC)ViewData["appcompetenc"]).NAME%>" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">Controller名称：</td>
                                    <td>
                                        <input id="txtCONTROLLER" type="text" value="<%=((LUOBO.Entity.SYS_APPCOMPETENC)ViewData["appcompetenc"]).CONTROLLER%>" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">Action名称：</td>
                                    <td>
                                        <input id="txtACTION" type="text" value="<%=((LUOBO.Entity.SYS_APPCOMPETENC)ViewData["appcompetenc"]).ACTION%>" />
                                    </td>
                                </tr>
                            </tbody>
                            <tbody id="tbdMost" name="tbdBody" style="display:none">
                                <tr>
                                    <td style="width: 120px; text-align: right">导入文件：</td>
                                    <td><input id="btnImport" type="file" name="fileToUpload" class="input"/></td>
                                </tr>
                            </tbody>
                            <tr>
                                <td></td>
                                <td><input id="btnSubmit" type="button" value="确定" /></td>
                            </tr>
                        </table>
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
