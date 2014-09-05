<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User</title>
    <script src="../../Scripts/Common.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.10.2.js"></script>
    <script src="../../Scripts/jquery-ui.js"></script>
    <script src="../../Scripts/jquery.ztree.core-3.5.min.js" type="text/javascript"></script>
    <script src="../../Scripts/ajaxfileupload.js" type="text/javascript"></script>

    <link href="../../Content/Site.css" rel="stylesheet" type="text/css" />
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
    <link href="../../Content/zTreeStyle/zTreeStyle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        var OID = 0;
        var POID = 0;
        $(function () {
            OID = $("#hdOID").val();
            POID = $("#hdPOID").val();

            $("input[name='regCount']").click(function () { $("tbody[name='tbdBody']").hide(); $("#tbd" + $(this).val()).show(); });

            //            $.ajax({
            //                type: 'post', //可选get
            //                url: 'GetUserTypeList', //这里是接收数据的PHP程序
            //                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
            //                success: function (obj) {
            //                    for (var i = 0; i < obj.length; i++) {
            //                        $("#slcUT").append("<option value='" + obj[i].VALUE + "'>" + obj[i].NAME + "</option>");
            //                    }

            //                    if ($("#hdType").val() == "0")
            //                        $("#slcUT").val("2");
            //                    else
            //                        $("#slcUT").val($("#hdState").val());
            //                }
            //            });

            if ($("#hdType").val() != "0")
                $("#trMode").hide();

            $("#btnSubmit").click(function () {
                if ($("#hdType").val() == "0") {
                    if ($("input[name='regCount']:checked").val() == "One")
                        AddUser();
                    else
                        ImportUser();
                }
                else
                    UpdateUser();
            });

            FindJGList();
        });

        var id = 0;
        var username = "";
        var account = "";
        var pwd = "";
        var contact = "";
        var usertype = 2;
        function AddUser() {
            if (!GetUserInfo())
                return;
            $.ajax({
                type: 'post', //可选get
                url: 'AddUser', //这里是接收数据的PHP程序
                data: 'USERNAME=' + username + '&ACCOUNT=' + account + '&PWD=' + pwd + '&CONTACT=' + contact + '&USERTYPE=' + usertype + '&OID=' + OID, //传给PHP的数据，多个参数用&连接
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    alert(obj.ResultMsg);
                }
            });
        }

        function UpdateUser() {
            id = $("#hdID").val();
            usertype = $("#hdState").val();
            if (!GetUserInfo())
                return;
            $.ajax({
                type: 'post', //可选get
                url: 'UpdateUser', //这里是接收数据的PHP程序
                data: "ID=" + id + '&USERNAME=' + username + '&ACCOUNT=' + account + '&PWD=' + pwd + '&CONTACT=' + contact + '&USERTYPE=' + usertype + '&OID=' + OID, //传给PHP的数据，多个参数用&连接
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    if (obj)
                        alert("修改成功!");
                    else
                        alert("修改失败!");
                }
            });
        }

        function ImportUser() {
            $.ajaxFileUpload({
                url: 'ImportUser',
                secureuri: false,
                fileElementId: 'btnImport',
                dataType: 'json',
                data: { OID: OID },
                success: function (obj, status) {
                    alert(obj.ResultMsg);
                },
                error: function (data, status, e) {
                    alert("提交失败");
                }
            });
        }

        function GetUserInfo() {
            //名称判断
            if ($("#txtUserName").val().Trim() == "") {
                alert("请输入名称!");
                return false;
            }
            else
                username = $("#txtUserName").val().Trim();
            //帐号判断
            if ($("#txtAccount").val().Trim() == "") {
                alert("请输入帐号!");
                return false;
            }
            else
                account = $("#txtAccount").val().Trim();
            //密码判断
            if ($("#hdType").val() == "0") {
                if ($("#txtPWD").val().Trim() == "") {
                    alert("请输入密码!");
                    return false;
                }
                else if ($("#txtPWD").val().Trim() != $("#txtRPWD").val().Trim()) {
                    alert("两次输入密码不一致!");
                    return false;
                }
                else
                    pwd = $("#txtPWD").val().Trim();
            }
            else {
                if ($("#txtPWD").val().Trim() != $("#txtRPWD").val().Trim()) {
                    alert("两次输入密码不一致!");
                    return false;
                }
                else
                    pwd = $("#txtPWD").val().Trim();
            }
            //联系方式判断
            if ($("#txtContact").val().Trim() == "") {
                alert("请输入联系方式!");
                return false;
            }
            else
                contact = $("#txtContact").val().Trim();
            //类别判断
//            if ($("#slcUT").val() == "-99") {
//                alert("请选择类别!");
//                return false;
//            }
//            else
//                usertype = $("#slcUT").val().Trim();
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
        function FindJGList() {
            $.ajax({
                type: 'post', //可选get
                url: '/APManage/FindJGList', //这里是接收数据的PHP程序
                data: 'jgID=' + POID,
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    zNodes.length = 0;
                    if (obj.length > 0) {
                        var item;
                        var data;
                        for (var i = 0; i < obj.length; i++) {
                            if (obj[i].ID == OID) {
                                POIDS = obj[i].PIDHELP;
                                break;
                            }
                        }

                        for (var i = 0; i < obj.length; i++) {
                            data = new Object();

                            if (POIDS.indexOf("$" + obj[i].ID + "$") > -1)
                                data.open = true;
                            else
                                data.open = false;
                            data.id = obj[i].ID;
                            data.pId = obj[i].PID;
                            data.name = obj[i].NAME;
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
                OID = nodes[i].id;
            }
            if (v.length > 0) v = v.substring(0, v.length - 1);
            var cityObj = $("#citySel");
            cityObj.attr("value", v);
        }

        function onNodeCreated(e, treeId, treeNode) {
            if (treeNode.id == OID)
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
                        <i class="fa fa-fw fa-list-ul"></i>新增用户
                    </div>
                    <div class="widget-content padded clearfix">
                        <input id="hdType" type="hidden" value="<%=ViewData["type"]%>" />
                        <input id="hdOID" type="hidden" value="<%=Convert.ToInt32(ViewData["type"]) == 1 ? ((LUOBO.Entity.SYS_USER)ViewData["user"]).OID.ToString() : Request.Cookies["LUOBO"].Values["oid"]%>" />
                        <input id="hdPOID" type="hidden" value="<%=Request.Cookies["LUOBO"]["OID"] %>" />
                        <input id="hdID" type="hidden" value="<%=((LUOBO.Entity.SYS_USER)ViewData["user"]).ID%>" />
                        <input id="hdState" type="hidden" value="<%=Convert.ToInt32(ViewData["type"]) == 1 ? ((LUOBO.Entity.SYS_USER)ViewData["user"]).USERTYPE : 2%>" />
                        <table class="table table-bordered table-striped">
                            <tr>
                                <td style="width: 120px; text-align: right">机构：</td>
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
                                        <input id="txtUserName" type="text" value="<%=((LUOBO.Entity.SYS_USER)ViewData["user"]).USERNAME%>" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">帐号：</td>
                                    <td>
                                        <input id="txtAccount" type="text" value="<%=((LUOBO.Entity.SYS_USER)ViewData["user"]).ACCOUNT%>" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">密码：</td>
                                    <td>
                                        <input id="txtPWD" type="password" /></td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">确认密码：</td>
                                    <td>
                                        <input id="txtRPWD" type="password" /></td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">联系方式：</td>
                                    <td>
                                        <input id="txtContact" type="text" value="<%=((LUOBO.Entity.SYS_USER)ViewData["user"]).CONTACT%>" />
                                    </td>
                                </tr>
                                <%--<tr>
                                    <td style="text-align: right">类别：</td>
                                    <td>
                                        <select id="slcUT" style="width: 100px">
                                            <option value="-99">全部</option>
                                        </select>
                                    </td>
                                </tr>--%>
                            </tbody>
                            <tbody id="tbdMost" name="tbdBody" style="display:none">
                                <tr>
                                    <td style="width: 120px; text-align: right">导入文件：</td>
                                    <td>
                                        <input id="btnImport" type="file" name="fileToUpload" class="input"/>
                                    </td>
                                </tr>
                            </tbody>
                            <tr>
                                <td></td>
                                <td>
                                    <input id="btnSubmit" type="button" value="确定" /></td>
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
