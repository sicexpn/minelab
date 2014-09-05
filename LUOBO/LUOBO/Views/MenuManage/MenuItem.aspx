<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MenuItem.aspx</title>
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/Common.js" type="text/javascript"></script>
    <script src="../../Scripts/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <link href="../../Content/lhbStyle/stylesheets/bootstrap.min.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../../Content/lhbStyle/stylesheets/font-awesome.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../../Content/lhbStyle/stylesheets/se7en-font.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../../Scripts/jquery-ui.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        html, body
        {
            width: 100%;
        }
        
        html
        {
            height: 100%;
        }
        
        body
        {
            color: #666666;
            background: #e8e8e8;
            padding-top: 20px;
            min-height: 100%;
        }
        
        /*----------------------------------------------------------------------------------- */
        /*  Widget-Container */
        /*----------------------------------------------------------------------------------- */
        .widget-container
        {
            min-height: 320px;
            background: white;
            box-shadow: 0 1px 2px rgba(0, 0, 0, 0.1);
        }
        .widget-container.fluid-height
        {
            height: auto;
            min-height: 0;
        }
        .widget-container.small
        {
            min-height: 120px;
            height: 200px;
        }
        .widget-container.gallery
        {
            min-height: 400px;
        }
        .widget-container .heading
        {
            background: rgba(255, 255, 255, 0.94);
            height: 50px;
            padding: 15px 15px;
            color: #007aff;
            font-size: 15px;
            width: 100%;
            font-weight: 400;
            margin: 0;
        }
        .widget-container .heading [class^="icon-"], .widget-container .heading [class*="icon-"]
        {
            margin-right: 10px;
            font-size: 14px;
        }
        .widget-container .heading [class^="icon-"].pull-right, .widget-container .heading [class*="icon-"].pull-right
        {
            margin-right: 0px;
            margin-left: 15px;
            opacity: 0.35;
            font-size: 1.1em;
        }
        .widget-container .heading [class^="icon-"]:hover, .widget-container .heading [class*="icon-"]:hover
        {
            cursor: pointer;
            opacity: 1;
        }
        .widget-container .tabs
        {
            background: whitesmoke;
            border-bottom: 1px solid #dddddd;
        }
        .widget-container .widget-content
        {
            width: 100%;
        }
        .widget-container.scrollable
        {
            position: relative;
            height: 400px;
            padding-top: 50px;
        }
        .widget-container.scrollable.chat-home
        {
            height: 427px;
        }
        .widget-container.scrollable .heading
        {
            position: absolute;
            top: 0;
            left: 0;
            z-index: 10;
        }
        .widget-container.scrollable .shadow
        {
            box-shadow: 0 2px 2px -1px rgba(0, 0, 0, 0.1);
        }
        .widget-container.scrollable .widget-content
        {
            height: 100%;
            position: relative;
            overflow-y: auto;
            -webkit-overflow-scrolling: touch;
        }
        
        .padded
        {
            padding: 15px;
        }
        
        .divMain
        {
            padding: 9px 14px;
            margin-bottom: 14px;
            border: 1px solid #ddd;
            border-radius: 4px;
            margin: auto;
        }
    </style>
    <script type="text/javascript">
        var MenuType = ["普通菜单", "右键菜单"];
        var MenuItem = { M_ID: -1, APP_ID: -1, M_NAME: "", M_PID: -1, M_LEVEL: 1, M_ORDER: 1, M_TYPE: 0, M_URL: "", M_REMARK: "", M_ICON: "", M_ICONTYPE: 0, M_ISON: false };
        var MenuListComplete = false;
        var MenuTypeComplete = false;
        var MenuICONTypeComplete = false;

        $(document).ready(function () {
            if (GetQueryString("id") != null)
                GetMenuByID();

            SetBodyHeight();
            GetUserMenuList();
            GetDicByMenuType();
            GetDicByMenuICONType();

            $("#btnSave").click(function () { SaveMenu(); });
            $("#btnCancel").click(function () { window.location.href = "/MenuManage/MenuList"; });

            window.onresize = function () {
                SetBodyHeight();
            };
        });

        function GetQueryString(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
            r = decodeURIComponent(window.location.search).substr(1).match(reg);

            if (r != null) {
                return unescape(r[2]);
            }
            else {
                r = window.location.search.substr(1).match(reg);
                if (r != null) {
                    return unescape(r[2]);
                }
                else {
                    return null;
                }
            }
        }

        function SetBodyHeight() {
            $("div[class='widget-container clearfix']").attr("style", "height:" + (document.documentElement.clientHeight - 40) + "px; overflow:auto;");
        }

        function GetMenuItem() {
            MenuItem.M_NAME = $("#txtMenuName").val().Trim();
            MenuItem.M_PID = $("#slcMenuList").val().split(',')[0];
            MenuItem.M_LEVEL = $("#slcMenuList").val().split(',')[1];
            MenuItem.M_ORDER = $("#txtMenuOrder").val().Trim();
            MenuItem.M_TYPE = $("#slcMenuType").val();
            MenuItem.M_URL = $("#txtMenuURL").val().Trim();
            MenuItem.M_REMARK = $("#txtMenuRemark").val().Trim();
            MenuItem.M_ICON = $("#txtMenuICON").val().Trim();
            MenuItem.M_ICONTYPE = $("#slcMenuICONType").val();
            MenuItem.M_ISON = $("input[name='rdIsOn']:checked").val() == "1" ? true : false;
        }

        function SetMenuItem() {
            if (MenuListComplete && MenuTypeComplete && MenuICONTypeComplete) {
                $("#txtMenuName").val(MenuItem.M_NAME);
                $("#slcMenuList").val(MenuItem.M_PID + "," + MenuItem.M_LEVEL);
                $("#txtMenuOrder").val(MenuItem.M_ORDER);
                $("#slcMenuType").val(MenuItem.M_TYPE);
                $("#txtMenuURL").val(MenuItem.M_URL);
                $("#txtMenuRemark").val(MenuItem.M_REMARK);
                $("#txtMenuICON").val(MenuItem.M_ICON);
                $("#slcMenuICONType").val(MenuItem.M_ICONTYPE);
                if (MenuItem.M_ISON)
                    $("#rdON").prop("checked", true);
                else
                    $("#rdOFF").prop("checked", true);
            }
            else {
                setTimeout(SetMenuItem, 100);
            }
        }

        function InitMenuTree(list) {
            var result = "";
            var item;
            for (var i = 0; i < list.length; i++) {
                var spase = "";
                item = list[i];
                for (var j = 0; j < item.M_LEVEL - 1; j++) {
                    spase += "　";
                }
                result += "<option value='" + item.M_ID + "," + (item.M_LEVEL + 1) + "'>" + spase + item.M_NAME + "</option>";
                result += InitMenuTree(item.SubMenuList);
            }
            return result;
        }

        function GetUserMenuList() {
            $.ajax({
                type: 'post', //可选get
                url: 'GetUserMenuList', //这里是接收数据的PHP程序
                data: 'type=none',
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    $("#slcMenuList").empty();
                    if (obj.ResultOBJ.length > 0) {
                        $("#slcMenuList").append("<option value='-1,1'>无</option>");
                        $("#slcMenuList").append(InitMenuTree(obj.ResultOBJ));
                    }
                    MenuListComplete = true;
                }
            });
        }

        function GetDicByMenuType() {
            $.ajax({
                type: 'post', //可选get
                url: 'GetDicByMenuType', //这里是接收数据的PHP程序
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    $("#slcMenuType").empty();
                    if (obj.ResultOBJ.length > 0) {
                        var result = "";
                        for (var i = 0; i < obj.ResultOBJ.length; i++)
                            result += "<option value='" + obj.ResultOBJ[i].VALUE + "'>" + obj.ResultOBJ[i].NAME + "</option>";
                        $("#slcMenuType").append(result);
                    }
                    MenuTypeComplete = true;
                }
            });
        }

        function GetDicByMenuICONType() {
            $.ajax({
                type: 'post', //可选get
                url: 'GetDicByMenuICONType', //这里是接收数据的PHP程序
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    $("#slcMenuICONType").empty();
                    if (obj.ResultOBJ.length > 0) {
                        var result = "";
                        for (var i = 0; i < obj.ResultOBJ.length; i++)
                            result += "<option value='" + obj.ResultOBJ[i].VALUE + "'>" + obj.ResultOBJ[i].NAME + "</option>";
                        $("#slcMenuICONType").append(result);
                    }
                    MenuICONTypeComplete = true;
                }
            });
        }

        function ValidationInfo() {
            if ($("#txtMenuName").val().Trim() == "") {
                alert("请输入菜单名称");
                return false;
            }
            if ($("#txtMenuOrder").val().Trim() == "") {
                alert("请输入菜单排序");
                return false;
            } else if (isNaN($("#txtMenuOrder").val().Trim())) {
                alert("菜单排序必须为数字");
                return false;
            }
            if ($("#txtMenuURL").val().Trim() == "") {
                alert("请输入菜单路径");
                return false;
            }
            if ($("#slcMenuICONType").val() != "0") {
                if ($("#txtMenuICON").val().Trim() == "") {
                    alert("请输入菜单图标");
                    return false;
                }
            }
            return true;
        }

        function GetMenuByID() {
            $.ajax({
                type: 'post', //可选get
                url: 'GetMenuByID', //这里是接收数据的PHP程序
                data: 'mid=' + GetQueryString("id"),
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    if (obj.ResultCode == 0) {
                        if (obj.ResultOBJ != null) {
                            MenuItem = obj.ResultOBJ;
                            SetMenuItem();
                        }
                        else {
                            alert(obj.ResultMsg);
                        }
                    }
                    else {
                        alert(obj.ResultMsg);
                    }
                }
            });
        }

        function SaveMenu() {
            if (!ValidationInfo())
                return;
            GetMenuItem();

            $.ajax({
                type: 'post', //可选get
                url: 'SaveMenu', //这里是接收数据的PHP程序
                data: MenuItem,
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    if (obj.ResultCode == 0) {
                        alert(obj.ResultMsg);
                        window.location.href = "/MenuManage/MenuList";
                    }
                    else {
                        alert(obj.ResultMsg);
                    }
                }
            });
        }
    </script>
</head>
<body>
    <div class="container-fluid">
        <div class="row-fluid">
            <div class="div-body col-md-12">
                <div class="widget-container clearfix">
                    <div class="widget-content padded clearfix">
                        <div name="divBody" class="divMain" style="width: 400px;">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-sm-4 control-label">父级菜单</label>
                                    <div class="col-sm-8">
                                        <select id="slcMenuList" class="form-control"></select>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-4 control-label">菜单名称</label>
                                    <div class="col-sm-8">
                                        <input id="txtMenuName" type="text" class="form-control" placeholder="菜单名称" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-4 control-label">菜单路径</label>
                                    <div class="col-sm-8">
                                        <input id="txtMenuURL" type="text" class="form-control" placeholder="菜单路径" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-4 control-label">菜单类型</label>
                                    <div class="col-sm-8">
                                        <select id="slcMenuType" class="form-control"></select>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-4 control-label">菜单排序</label>
                                    <div class="col-sm-8">
                                        <input id="txtMenuOrder" type="text" class="form-control" value="1" placeholder="菜单排序" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-4 control-label">菜单图标类型</label>
                                    <div class="col-sm-8">
                                        <select id="slcMenuICONType" class="form-control"></select>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-4 control-label">菜单图标</label>
                                    <div class="col-sm-8">
                                        <input id="txtMenuICON" type="text" class="form-control" placeholder="菜单图标" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-4 control-label">备注</label>
                                    <div class="col-sm-8">
                                        <textarea id="txtMenuRemark" class="form-control" rows="5" placeholder="备注"></textarea>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-4 control-label">是否启用</label>
                                    <div class="col-sm-8">
                                        <label class="radio-inline"><input id="rdON" name="rdIsOn" type="radio" value="1" checked /> 是</label>
                                        <label class="radio-inline"><input id="rdOFF" name="rdIsOn" type="radio" value="0" /> 否</label>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-offset-4 col-sm-8">
                                        <button id="btnSave" type="button" class="btn btn-success btn-sm">保存</button>
                                        <button id="btnCancel" type="button" class="btn btn-default btn-sm">取消</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
