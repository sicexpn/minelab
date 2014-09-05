<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="userState.aspx.cs" Inherits="LUOBO.SingleShop.UI.userState" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>next-wifi 用户状态</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" charset="utf-8"/>

    <link href="../UITemplet/css/bootstrap.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/css/font-awesome.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/css/index.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/css/style.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/css/AeroWindow-Contextmenu.css" rel="stylesheet">
    <link href="../UITemplet/css/grumble.min.css" media="all" rel="stylesheet" type="text/css" />
    <script src="../UITemplet/js/Common.js" type="text/javascript"></script>
    <script src="../UITemplet/js/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../UITemplet/js/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../UITemplet/js/jquery.grumble.min.js" type="text/javascript"></script>
    <script src="../UITemplet/js/bootstrap.js" type="text/javascript"></script>
    <script src="../UITemplet/js/login.js" type="text/javascript"></script>
    <script src="../UITemplet/js/respond.js" type="text/javascript"></script>
    <script src="../UITemplet/js/AeroWindow-Contextmenu.js" type="text/javascript"></script>
    <script src="../UITemplet/js/useragents.js" type="text/javascript"></script>

<script type="text/javascript">
    
    Browsers = new UserAgents(navigator.userAgent);
    var txt = "browserName: " + (Browsers.browser.name ? Browsers.browser.name : '') + '<br />' +
			  "browserChannel: " + (Browsers.browser.channel ? Browsers.browser.channel : '') + '<br />' +
			  "browserVersion: " + (Browsers.browser.version ? Browsers.browser.version.toString() : '') + '<br />' +
			  "browserVersionType: " + (Browsers.browser.version ? Browsers.browser.version.type : '') + '<br />' +
			  "browserVersionMajor: " + (Browsers.browser.version ? Browsers.browser.version.major : '') + '<br />' +
			  "browserVersionMinor: " + (Browsers.browser.version ? Browsers.browser.version.minor : '') + '<br />' +
			  "browserVersionOriginal: " + (Browsers.browser.version ? Browsers.browser.version.original : '') + '<br />' +
			  "browserMode: " + (Browsers.browser.mode ? Browsers.browser.mode : '') + '<br />' +
			  "engineName: " + (Browsers.engine.name ? Browsers.engine.name : '') + '<br />' +
			  "engineVersion: " + (Browsers.engine.version ? Browsers.engine.version.toString() : '') + '<br />' +
			  "osName: " + (Browsers.os.name ? Browsers.os.name : '') + '<br />' +
			  "osVersion: " + (Browsers.os.version ? Browsers.os.version.toString() : '') + '<br />' +
			  "deviceManufacturer: " + (Browsers.device.manufacturer ? Browsers.device.manufacturer : '') + '<br />' +
			  "deviceModel: " + (Browsers.device.model ? Browsers.device.model : '') + '<br />' +
			  "deviceType: " + (Browsers.device.type ? Browsers.device.type : '') + '<br />' +
			  "useragent: " + navigator.userAgent + '<br />' +
			  "humanReadable: " + Browsers.toString();
</script>

<script type="text/ecmascript">
    var flow_user_div = null,
        is_show_flow = false;
    var _ADList = null;

    var cur_ad_sort = { faild: "", sort_idx: 0 },
            ad_sort_faild = {
                ap_sort_alias: "ALIAS",
                ap_sort_ssidname: "SSIDNAME",
                ap_sort_title: "TITLE",
                ap_sort_currenttime: "CURRENTTIME"                
            };

    var SORT = ["", "desc", "asc"];
    var is_ad_loading = true;
    var token = GetQueryString("token"); //读取token

    $(document).ready(function () {
        flow_user_div = $('#user_info_div').clone();
        $(flow_user_div).removeAttr("id");
        $(flow_user_div).css("position", "absolute");
        $(flow_user_div).css("width", $('#user_info_div').width() + 1);
        $(flow_user_div).css("left", $('#user_info_div').offset().left);
        $(flow_user_div).hide();
        $("body").append(flow_user_div);
        getADList();
        GetVisitOLInfo();
        GetCalingAuthenticationInfo();
        bindEvents();
        $("#slcDList").change(function () {
            $("#adBdyList").empty();
            $("#adBdyList").append(InitADList(_ADList));
        });
    });

    

    function bindEvents() {
        // 浮动用户状态信息
        $(window).scroll(function () {
            setFlowDivPostion();
        });

        $(window).resize(function () {
            $(flow_user_div).css("width", $('#user_info_div').width() + 1);
        });

        // 绑定访问广告排序事件
        $('table[name="adTable"]').find("th").click(function () {
            if (this.id == null || this.id == "")
                return false;
            if (!is_ad_loading) {
                is_ad_loading = true;
                var h_i = $('table[name="adTable"]').find("th i").removeClass().addClass("fa fa-sort");
                if (cur_ad_sort.faild == ad_sort_faild[this.id]) {
                    if (cur_ad_sort.sort_idx == 2)
                        cur_ad_sort.sort_idx = 0;
                    else
                        cur_ad_sort.sort_idx++;
                } else {
                    cur_ad_sort.faild = ad_sort_faild[this.id];
                    cur_ad_sort.sort_idx = 1;
                }
                $(this).find("i").removeClass().addClass("fa fa-sort" + (cur_ad_sort.sort_idx != 0 ? "-" : "") + SORT[cur_ad_sort.sort_idx]);

                getADList();
            } else {
                $(this).grumble({
                    text: '正在查询<br />请稍候',
                    angle: 90,
                    distance: 30,
                    type: 'alt-',
                    hideAfter: 750
                });
            }
        });
    }

    function getADList() {
        $("#adBdyList").empty
        $("#adBdyList").html("<tr><td colspan='9'>正在加载...</td></tr>");
        $.ajax({
            type: 'post',
            url: 'AjaxComm.aspx',
            data: 'type=GetAdInfoByCallingID&token=' + token + '&param={"NAME":"' + GetQueryString("mac") + '","COLUMN":"' + cur_ad_sort.faild + '","ORDERBY":"' + SORT[cur_ad_sort.sort_idx] + '"}',
            dataType: 'json',
            error: function (msg) {
                is_ad_loading = false;
            },
            success: function (obj) {
                is_ad_loading = false;
                if (obj.ResultCode == "0") {
                    $("#adBdyList").empty();
                    if (obj.ResultOBJ.length > 0) {
                        _ADList = obj.ResultOBJ;
                        $("#adBdyList").append(InitADList(obj.ResultOBJ));
                        var accdList = GetAccessDevice();
                        if (accdList.length > 1)
                            $("#slcDList").show();
                        for (var i = 0; i < accdList.length; i++)
                            $("#slcDList").append("<option value='" + accdList[i] + "'>" + accdList[i] + "</option>");

                        // 解析UA
                        Browsers = new UserAgents(obj.ResultOBJ[0].UserAgent);
                        var user_html = "<dl class='dl-horizontal' style='margin:0px'>";
                        user_html += "<dt style='width:80px;'>用户标识：</dt>";
                        user_html += "<dd style='margin-left:100px;'>" + GetQueryString("mac"); +"</dd>";
                        if (Browsers.device.model) {
                            user_html += "<dt style='width:80px;'>设备型号：</dt>";
                            user_html += "<dd style='margin-left:100px;'>" + Browsers.device.model + "</dd>";
                        }
                        if (Browsers.os.name) {
                            user_html += "<dt style='width:80px;'>操作系统：</dt>";
                            user_html += "<dd style='margin-left:100px;'>" + Browsers.os.name + " " + (Browsers.os.version ? Browsers.os.version.toString() : "") + "</dd>";
                        }
                        if (Browsers.browser.name) {
                            user_html += "<dt style='width:80px;'>浏览器：</dt>";
                            user_html += "<dd style='margin-left:100px;'>" + Browsers.browser.name + " " + (Browsers.browser.version ? Browsers.browser.version.major : '') + "</dd>";
                        }
                        user_html += "</dl>";
                        $('div[name="user_basic_div"]').next().html(user_html);

                    } else {
                        //没有任何设备的时候
                    }

                }
                else {
                    alert(obj.ResultMsg);
                }
            }
        });
    }

    function GetAccessDevice() {
        var list = new Array();
        var flag = true;
        for (var i = 0; i < _ADList.length; i++) {
            flag = true;
            for (var j = 0; j < list.length; j++) {
                if (list[j] == _ADList[i].ALIAS)
                    flag = false;
            }
            if (flag)
                list.push(_ADList[i].ALIAS);
        }
        return list;
    }

    function InitADList(objList) {
        var result = "";
        var item;
        for (var i = 0; i < objList.length; i++) {
            item = objList[i];
            if ($("#slcDList").val() != "全部")
                if ($("#slcDList").val() != item.ALIAS)
                    continue;
            result += "<tr>";
            result += "<td>" + (i + 1) + "</td>";
            result += "<td>" + item.ALIAS + "</td>";
            result += "<td>" + item.SSIDNAME + "</td>";
            result += "<td>" + item.Title + "</td>";
            result += "<td>" + dateFormat(item.CurrentTime, "yyyy-MM-dd hh:mm:ss") + "</td>";
            result += "</tr>";
        }
        return result;
    }

    function setFlowDivPostion() {
        if (flow_user_div != null && $(document).width() > 980) {
            if ($('#user_info_div').offset().top - $(document).scrollTop() < 0) {
                if (!is_show_flow) {
                    $(flow_user_div).show();
                    is_show_flow = true;
                }
                $(flow_user_div).css("top", $(document).scrollTop());
            } else {
                if (is_show_flow) {
                    $(flow_user_div).hide();
                    is_show_flow = false;
                }
            }
        } else {
            if (is_show_flow) {
                $(flow_user_div).hide();
                is_show_flow = false;
            }
        }
    }

    function GetVisitOLInfo() {
        $.ajax({
            type: 'post',
            url: 'AjaxComm.aspx',
            data: 'type=GetVisitOLInfoByCalingID&token=' + token + '&param="' + GetQueryString("mac")+'"',
            dataType: 'json',
            error: function (msg) {

            },
            success: function (obj) {
                if (obj.ResultCode == "0") {
                    var ol_html = "<dl class='dl-horizontal' style='margin:0px'>";
                    ol_html += "<dt style='width:80px;'>总时长：</dt>";
                    ol_html += "<dd style='margin-left:100px;'>" + (obj.ResultOBJ.OLZSC / 60).toFixed(2) + " 分钟</dd>";
                    ol_html += "<dt style='width:80px;'>总流量：</dt>";
                    ol_html += "<dd style='margin-left:100px;'>" + getUsedTraffic(obj.ResultOBJ.OLZLL) + "</dd>";
                    ol_html += "</dl>";
                    $('div[name="user_ol_div"]').next().html(ol_html);
                } else {
                    //没有任何设备的时候
                }
            }
        });
    }

    function GetCalingAuthenticationInfo() {
        $.ajax({
            type: 'post',
            url: 'AjaxComm.aspx',
            data: 'type=GetCalingAuthentication&token=' + token + '&param="' + GetQueryString("mac") + '"',
            dataType: 'json',
            error: function (msg) {

            },
            success: function (obj) {
                if (obj.ResultCode == "0") {
                    var ol_html = "<dl class='dl-horizontal' style='margin:0px'>";
                    ol_html += "<dt style='width:80px;'>认证类型：</dt>";
                    for (var i = 0; i < obj.ResultOBJ.length; i++)
                        ol_html += "<dd style='margin-left:100px;'>" + obj.ResultOBJ[i] + "</dd>";
                    ol_html += "</dl>";
                    $('div[name="user_check_div"]').next().html(ol_html);
                } else {
                    //没有任何设备的时候
                }
            }
        });
    }

    function getUsedTraffic(_traffic) {
        if (_traffic == null) {
            return "0 b";
        }
        else if (_traffic < 1024) {
            return _traffic + " b";
        } else if (_traffic < 1048576) {
            return eval(_traffic / 1024).toFixed(2) + " kb";
        } else {
            return eval(_traffic / 1024 / 1024).toFixed(2) + " mb";
        }
    }

</script>
    
</head>

<body>
    
<nav class="nav navbar-default" role="navigation">
  <!-- Brand and toggle get grouped for better mobile display -->
  <div class="navbar-header" id="top-bar" >
    <a class="navbar-brand" id="logo" href="#"><span class="label label-default"> 萝卜wifi云平台 V1.68</span></a>
     
    <div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
    <ul class="nav navbar-nav navbar-right">
      <li class="dropdown">
        <a href="#" class="dropdown-toggle" data-toggle="dropdown"><i class="fa fa-list-ul"></i><b class="caret"></b></a>
        <ul class="dropdown-menu">
          <li><a><i class="fa fa-user"></i> Login.LName</a></li>
          
          <li><a id="flset" ><i class="fa fa-cogs"></i> 参数设置</a></li>
          <li><a href="javascript:flagdisplay('aboutUs')"><i class="fa fa-group"></i> 关于我们</a></li>
          <li><a href="javascript:flagdisplay('xgpass');"><i class="fa fa-key"></i> 修改密码</a></li>
          <li><a href="javascript:exituser();"><i class="fa fa-sign-out"></i> 退出</a></li>
        </ul>
      </li>
    </ul>
  </div>
  </div>
</nav>

<div class="container-fluid">
    <div class="row-fluid">
        <div class="col-md-3">
            <div id="user_info_div" class="panel panel-default">
            <div name="user_basic_div" class="panel-heading"><span class="label label-info" style="font-size:12px;">用户基本信息</span></div>
            <div class="panel-body">
                <small>Loading</small>
            </div>
            
            <div name="user_ol_div" class="panel-heading"><span class="label label-info" style="font-size:12px;">用户上网信息</span></div>
            <div class="panel-body">
                <small>Loading</small>
            </div>
                
            <div name="user_check_div" class="panel-heading"><span class="label label-info" style="font-size:12px;">用户上网认证类型</span></div>
            <div class="panel-body">
                <small>Loading</small>
            </div>

            <div name="user_contact_div" class="panel-heading"><span class="label label-info" style="font-size:12px;">用户联系方式</span></div>
            <div class="panel-body">
                <small>暂无</small>
                 <%--<div class="title">
                    新浪微博号
                </div>
                <div id="user_contact_weibo" class="text">
                    abcabc
                </div>
                <div class="title">
                    微信号
                </div>
                <div id="user_contact_weixin" class="text">
                    abcabc
                </div>
                <div class="title">
                    手机号
                </div>
                <div id="user_contact_cell"class="text">
                    123123123
                </div>--%>
            </div>
            </div>
        </div>

        <div class="col-md-9">
        <div class="panel panle_ssid">
            <div class="panel-heading">
                <span id="adTitle" class="label label-success" style="font-size:14px">访问广告信息</span>
                <select id="slcDList" class="form-control" style="width:200px; float:right; margin-top: -10px; display:none;"><option value='全部'>全部</option></select>
                <%--<span>
                    <input id="search_txt" type="search" class="input-sm" placeholder="查询关键字.."/>
                    <a id="search_btn" class="btn btn-sm btn-success" href="javascript:void(0);"><i class="icon-twitter"></i>查询</a>
                </span>
                <span id="head_avg_sc" class="pull-right label label-success" style="font-size:14px;margin-right:10px">平均开机时长：</span>
                <span id="head_avg_rc" class="pull-right label label-success" style="font-size:14px;margin-right:10px">平均访问人次：</span>
                <span id="head_ap_bad" class="pull-right label label-warning" style="font-size:14px;margin-right:10px">设备故障：</span>
                <span id="head_ap_total" class="pull-right label label-success" style="font-size:14px;margin-right:10px">设备总量：</span>--%>
            </div>
            <div class="widget-content padded clearfix">
                <table class="hor-minimalist-a table table-bordered table-hover" name="adTable" >
                <thead>
                    <tr>
                        <th scope="col">序号</th>
                        <th id="ap_sort_alias" scope="col" class="btn-default" style="cursor:pointer;">访问设备 <i class="fa fa-sort"></i></th>
                        <th id="ap_sort_ssidname" scope="col" class="btn-default" style="cursor:pointer;">访问SSID <i class="fa fa-sort"></i></th>
                        <th id="ap_sort_title" scope="col" class="btn-default" style="cursor:pointer;">访问页面 <i class="fa fa-sort"></i></th>
                        <th id="ap_sort_currenttime" scope="col" class="btn-default" style="cursor:pointer;">访问时间 <i class="fa fa-sort"></i></th>
                    </tr>
                </thead>
                <tbody id="adBdyList" >
                </tbody>
                </table>
            </div>
        </div>
    </div>
    </div>
</div>
</body>
</html>
