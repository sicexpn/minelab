<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="header.ascx.cs" Inherits="LUOBO.SingleShop.UI.header" %>
<script type="text/javascript" src="../UITemplet/js/artDialog/artDialog.source.js?skin=blue"></script>
<script type="text/javascript" src="../UITemplet/js/artDialog/iframeTools.source.js"></script>
<script>

        //function redirct(_name) {
        //    var url = "";
        //    switch (_name) {
        //        case "首页":
        //            url = "gis.aspx?token=" + token;
        //            break;
        //        case "控制":
        //            url = "control.aspx?token=" + token;
        //            break;
        //        case "统计":
        //            url = "statistical1.aspx?token=" + token;
        //            break;
        //        case "报表":
        //            url = "report.aspx?token=" + token;
        //            break;
        //        case "状态":
        //            url = "state.aspx?token=" + token;
        //            break;
        //        case "安全":
        //            url = "security3.aspx?token=" + token;
        //            break;
        //    }
        //    window.location.href = url;
        //}
        //菜单高亮
        function HighLightMenu(hlMenu) {
            $("#navbar-collapse-1 ul a").removeAttr("class");
            $("#navbar-collapse-1 ul a").each(function () {
                if ($(this).text() == hlMenu) {
                    $(this).addClass("current");
                }
            });
        }
        function exituser() {
            $.ajax({
                type: 'post',
                url: 'AjaxComm.aspx',
                data: 'type=Logout&token=' + token,
                dataType: 'json',
                error: function (msg) {
                    //alert("网络错误");
                    //window.location.href = "login.aspx";
                },
                success: function (obj) {
                    if (obj.ResultCode == 0) {
                        window.location.href = "login.aspx";
                    } else {
                        alert(obj.ResultMsg);
                    }
                }
            });
        }
        // 获取预警信息
        var warn_interval;
        warn_interval = window.setInterval("GetAlertCount()", 30000);

        function GetAlertCount() {
            var warn_nr = 0;
            $.ajax({
                type: 'post',
                url: 'AjaxComm.aspx',
                //data: 'type=GetAlertCount&token=' + token,
                data: 'type=GetAlertListNotHandle&token=' + token,
                dataType: 'json',
                error: function (msg) {
                    // alert("服务器错误");
                },
                success: function (obj) {
                    if (obj.ResultCode == 0) {
                        warn_nr = obj.ResultOBJ.length;
                        //调用地图告警方法
                        if (typeof setWarningMarker != 'undefined' && setWarningMarker instanceof Function) {
                            var macArr = new Array();
                            for (var i = 0; i < obj.ResultOBJ.length; i++) {
                                macArr.push(obj.ResultOBJ[i].AP_MAC);
                            }
                            setWarningMarker(unique(macArr));
                        }
                        //调用安全页告警方法
                        if (typeof setWarningInfo != 'undefined' && setWarningInfo instanceof Function)
                            setWarningInfo(obj.ResultOBJ);
                        //alert(warn_nr);
                        updateWarnView(warn_nr);
                    } else {
                        //alert(obj.ResultMsg);
                        if (obj.ResultCode == -100) {
                            //window.location.href = "login.aspx";
                        }
                    }
                }
            });
        }

        function updateWarnView(warn_nr) {
            if (warn_nr > 0) {
                $("#warn-nr").html(warn_nr);
                $("#warn-nr").addClass("fa-colorful");
                window.open('warnDialog.aspx?token=' + token, 'warning', 'width=400,height=400,top=2000,left=2000,toolbar=no,menubar=no,scrollbars=no,resizable=no,location=no,status=no');
            }
            else {
                $("#warn-nr").html = "";
                $("#warn-nr").removeClass("fa-colorful");
            }
        };
        var is_ext_setting_dialog = true;
        function ext_setting_dialog_show() {
            if (is_ext_setting_dialog) {
                is_ext_setting_dialog = false;
                ext_setting_dialog = art.dialog.open('ext_setting.aspx?token=' + token, { title: '上网设置',
                    padding: '0px',
                    opacity: 0.2,
                    top: '30%',
                    left: '30%',
                    width: '600px',
                    height: '500px',
                    fixed: true,
                    resize: true,
                    close: function () {
                        is_ext_setting_dialog = true;
                    }
                });
            }
        }
        </script>
    <nav class="nav navbar-default" role="navigation">
        <div class="navbar-header" id="top-bar" >
            <a class="navbar-brand" id="logo" href="#"><span class="label label-default"> 萝卜wifi云平台 V1.68</span></a>
            <div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
                <ul class="nav navbar-nav navbar-right">
                    <li class="dropdown">
                        <a href="#" class="dropdown-toggle" data-toggle="dropdown"><i class="fa fa-list-ul"></i><b class="caret"></b></a>
                        <ul class="dropdown-menu">
                            <li><a><i class="fa fa-user"></i> Login.LName</a></li>
                            <li><a id="flset" ><i class="fa fa-cogs"></i> 监控设置</a></li>
                            <li><a href="javascript:flagdisplay('aboutUs')"><i class="fa fa-group"></i> 关于我们</a></li>
                            <li><a href="javascript:flagdisplay('xgpass');"><i class="fa fa-key"></i> 修改密码</a></li>
                            <li><a href="javascript:exituser();"><i class="fa fa-sign-out"></i> 退出</a></li>
                        </ul>
                    </li>
                </ul>
            </div>
        </div>
        <div class="collapse navbar-collapse" id="navbar-collapse-1">
            <ul id="ulMenu" class="nav">
                <%--<li><a class="current" href="javascript:redirct('首页');"><span><i class="fa fa-home fa-2x"></i></span>首页</a></li>
                <li><a href="javascript:redirct('状态');"><span><i class="fa fa-fire fa-2x"></i></span>状态</a></li>
                <li><a href="javascript:redirct('控制');"><span><i class="fa fa-code-fork fa-2x"></i></span>控制</a></li>
                <li><a href="javascript:redirct('统计');"><span><i class="fa fa-edit fa-2x"></i></span>统计</a></li>
                <li><a href="javascript:redirct('报表');"><span><i class="fa fa-bar-chart-o fa-2x"></i></span>报表</a></li>
                <li><a href="javascript:redirct('安全');"><span><i id="warn-nr" class="fa fa-shield fa-2x"></i></span>安全</a></li>
                <li><a href="javascript:ext_setting_dialog_show();"><span><i class="fa fa-cog fa-2x"></i></span>设置</a></li>--%>
            </ul>
        </div>
    </nav>
    <script type="text/javascript">
        var Public_Menu;
        $.ajax({
            type: 'post',
            url: 'AjaxComm.aspx',
            data: 'type=GetMenuByToken&token=' + token,
            async: false,
            dataType: 'json',
            success: function (obj) {
                if (obj.ResultCode == 0) {
                    if (obj.ResultOBJ.length > 0) {
                        var flag = true;
                        
                        for (var i = 0; i < obj.ResultOBJ.length; i++)
                            if (window.location.pathname.toLowerCase() == obj.ResultOBJ[i].M_URL.toLowerCase())
                                flag = false;

                        if (flag)
                            window.location.href = "/UI/error.aspx?type=-99";

                        var strMenu = '';
                        for (var i = 0; i < obj.ResultOBJ.length; i++) {
                            if (obj.ResultOBJ[i].M_ICONTYPE == 1) {
                                if (obj.ResultOBJ[i].M_NAME == "设置") {
                                    strMenu += '<li><a' + (i == 0 ? ' class="current"' : '') + ' href="javascript:ext_setting_dialog_show();"><span><i class="' + obj.ResultOBJ[i].M_ICON + '"></i></span>' + obj.ResultOBJ[i].M_NAME + '</a></li>';
                                }
                                else {
                                    strMenu += '<li><a' + (i == 0 ? ' class="current"' : '') + ' href="' + obj.ResultOBJ[i].M_URL + '?token=' + token + '"><span><i class="' + obj.ResultOBJ[i].M_ICON + '"></i></span>' + obj.ResultOBJ[i].M_NAME + '</a></li>';
                                }
                            }
                        }
                        $("#ulMenu").append(strMenu);
                        Public_Menu = obj.ResultOBJ;
                    }
                    else {
                        alert(obj.ResultMsg);
                    }
                } else {
                    alert(obj.ResultMsg);
                }
            }
        });
    </script>