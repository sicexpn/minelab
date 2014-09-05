<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>MenuAllocation</title>
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
        var UID = <%=ViewData["uid"]%>;
        var UserList;

        $(document).ready(function () {
            SetBodyHeight();
            GetUserMenuList();
            GetUserListByOID();
            if (UID == 0)
                GetOrgList();

            $("#cbMenu").click(function () { $("input[id^='cbMenu_']").prop("checked", $(this).prop("checked")); });
            $("#cbUser").click(function () { $("input[id^='cbUser_']").prop("checked", $(this).prop("checked")); });
            $("#btnAllocation").click(function () { SetUserMenuPermissions(); });
            $("#slcOrgList").change(function () { FilterUserList(); });

            window.onresize = function () {
                SetBodyHeight();
            };
        });

        function SetBodyHeight() {
            $("div[class='widget-container clearfix']").attr("style", "height:" + (document.documentElement.clientHeight - 40) + "px;");
            $("div[name='divBody']").attr("style", "height:" + ($("div[class='widget-content padded clearfix']").height() - 40) + "px; overflow:auto;");
            if (UID == 0)
                $("#divUserList").attr("style", "height:" + ($("div[class='widget-content padded clearfix']").height() - 74) + "px; overflow:auto;");
        }

        function InitMenuTree(list) {
            var result = "";
            var item;
            for (var i = 0; i < list.length; i++) {
                var spase = "";
                item = list[i];
                result += "<tr>";
                result += "<td><input id='cbMenu_" + item.M_ID + "' type='checkbox' value='" + item.M_ID + "' /></td>";
                for (var j = 0; j < item.M_LEVEL - 1; j++) {
                    spase += "　";
                }
                result += "<td>" + spase + item.M_NAME + "</td>";
                result += "<td>" + MenuType[item.M_TYPE] + "</td>";
                result += "<td>" + item.M_REMARK + "</td>";
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
                data: 'type=none',
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    $("#tbdMenuList").empty();
                    $("#divPage").empty();
                    if (obj.ResultOBJ.length > 0) {
                        $("#tbdMenuList").append(InitMenuTree(obj.ResultOBJ));
                    }
                    else {
                        $("#tbdMenuList").append("<tr><td colspan='8'>无菜单信息!</td></tr>");
                    }
                }
            });
        }

        function GetUserListByOID() {
            $.ajax({
                type: 'post', //可选get
                url: 'GetUserListByOID', //这里是接收数据的PHP程序
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    $("#ulUserList").empty();
                    if (obj.ResultOBJ.length > 0) {
                        UserList = obj.ResultOBJ;
                        var result = "";
                        for (var i = 0; i < obj.ResultOBJ.length; i++) {
                            result += "<li><div class='checkbox'><label><input id='cbUser_" + obj.ResultOBJ[i].ID + "' type='checkbox' value='" + obj.ResultOBJ[i].ID + "'> " + obj.ResultOBJ[i].USERNAME + "</label></div></li>";
                        }
                        $("#ulUserList").append(result);
                    }
                }
            });
        }

        function GetOrgList() {
            $.ajax({
                type: 'post', //可选get
                url: 'GetOrgList', //这里是接收数据的PHP程序
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    $("#slcOrgList").show();
                    $("#slcOrgList").empty();
                    var result = "<option value='-1'>全部</option>";
                    if (obj.ResultOBJ.length > 0) {
                        for (var i = 0; i < obj.ResultOBJ.length; i++) {
                            var item = obj.ResultOBJ[i];
                            var spase = "";
                            for (var j = 0; j < item.PIDHELP.split(',').length - 1; j++) {
                                spase += "　";
                            }
                            result += "<option value='" + item.ID + "'>" + spase + item.NAME + "</option>";
                        }
                        $("#slcOrgList").append(result);
                    }
                }
            });
        }

        function SetUserMenuPermissions() {
            var menuids = "";
            var userids = "";
            $("input[id^='cbMenu_']:checked").each(function () {
                if (menuids != "")
                    menuids += ",";
                menuids += $(this).val();
            });
            $("input[id^='cbUser_']:checked").each(function () {
                if (userids != "")
                    userids += ",";
                userids += $(this).val();
            });
            if (menuids == "") {
                alert("请选择分配的菜单!");
                return;
            }
            if (userids == "") {
                alert("请选择分配的用户!");
                return;
            }
            $.ajax({
                type: 'post', //可选get
                url: 'SetUserMenuPermissions', //这里是接收数据的PHP程序
                data: 'menuids=' + menuids + '&userids=' + userids,
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    if (obj.ResultCode == 0) {
                        alert("更新成功!");
                        $("#cbMenu").prop("checked", false);
                        $("input[id^='cbMenu_']").prop("checked", false);
                        $("#cbUser").prop("checked", false);
                        $("input[id^='cbUser_']").prop("checked", false);
                    } else {
                        alert(obj.ResultMsg);
                    }
                }
            });
        }

        function FilterUserList() {
            if (UserList.length > 0) {
                $("#ulUserList").empty();
                var result = "";
                for (var i = 0; i < UserList.length; i++) {
                    if ($("#slcOrgList").val() == -1)
                        result += "<li><div class='checkbox'><label><input id='cbUser_" + UserList[i].ID + "' type='checkbox' value='" + UserList[i].ID + "'> " + UserList[i].USERNAME + "</label></div></li>";
                    else if (UserList[i].OID == $("#slcOrgList").val())
                        result += "<li><div class='checkbox'><label><input id='cbUser_" + UserList[i].ID + "' type='checkbox' value='" + UserList[i].ID + "'> " + UserList[i].USERNAME + "</label></div></li>";
                }
                $("#ulUserList").append(result);
            }
        }
    </script>

    <style type="text/css">
        .divMain
        {
            padding: 9px 14px;
            margin-bottom: 14px;
            border: 1px solid #ddd;
            border-radius: 4px;
        }
    </style>
</head>
<body>
    <div class="container-fluid">
        <div class="row-fluid">
            <div class="div-body col-md-9">
                <div class="widget-container clearfix">
                    <div class="widget-content padded clearfix" style="height:100%">
                        <div class="checkbox"><label><input id="cbMenu" type="checkbox" /> 全选</label></div>
                        <input id="btnAllocation" style="margin-top: -30px; float: right;" type="button" value="分配权限" />
                        <div name="divBody" style="height:95%; overflow:auto;">
                            <table class="hor-minimalist-a table table-bordered table-striped">
                                <thead>
                                    <tr>
                                        <th scope="col" style="width: 50px;">选择</th>
                                        <th scope="col">菜单名称</th>
                                        <th scope="col">菜单类型</th>
                                        <th scope="col">备注</th>
                                    </tr>
                                </thead>
                                <tbody id="tbdMenuList">
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
            <div class="div-body col-md-3">
                <div class="widget-container clearfix">
                    <div class="widget-content padded clearfix" style="height:100%">
                        <div><select id="slcOrgList" class="form-control" style="display:none;"></select></div>
                        <div class="checkbox"><label><input id="cbUser" type="checkbox" /> 全选</label></div>
                        <div id="divUserList" name="divBody" class="divMain" style="height:95%; overflow:auto;">
                            <ul id="ulUserList" class="list-unstyled">
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
