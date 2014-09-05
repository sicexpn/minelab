<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Gis.aspx.cs" Inherits="LUOBO.SingleShop.UI.Gis" %>
<%@ Register src="header.ascx" tagname="header" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title>next-wifi 首页</title>
<meta name="viewport" content="initial-scale=1.0, user-scalable=no" /> 
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

<script type="text/javascript" src="../UITemplet/js/Common.js"></script>
<script type="text/javascript" src="../UITemplet/js/jquery-1.10.2.min.js"></script>
<script type="text/javascript" src="../UITemplet/js/jquery-ui-1.10.4.min.js"></script>
<script type="text/javascript" src="http://api.map.baidu.com/api?v=2.0&ak=10de0613056269b007c32743d586d9d7"></script>
<script type="text/javascript" src="../UITemplet/js/TextIconOverlay.js"></script>
<script type="text/javascript" src="../UITemplet/js/MarkerClusterer.js"></script>
<script type="text/javascript" src="../UITemplet/js/respond.js" ></script>
<script type="text/javascript" src="../UITemplet/js/jquery.sidebar.js" ></script>
<script type="text/javascript" src="../UITemplet/js/bootstrap.js"></script>
<script type="text/javascript" src="../UITemplet/js/bootstrap-switch.js"></script>
<script type="text/javascript" src="../UITemplet/echarts/js/esl.js"></script>
<script type="text/javascript" src="../UITemplet/echarts/js/codemirror.js"></script>
<script type="text/javascript" src="../UITemplet/echarts/js/javascript.js"></script>

<link href="../UITemplet/css/bootstrap.css" media="all" rel="stylesheet" type="text/css" />
<link href="../UITemplet/css/font-awesome.css" media="all" rel="stylesheet" type="text/css" />
<link href="../UITemplet/css/bootstrap-switch.css" media="all" rel="stylesheet" type="text/css" />
<link href="../UITemplet/css/index.css" media="all" rel="stylesheet" type="text/css" />
<link href="../UITemplet/css/sidebar.css" media="all" rel="stylesheet" type="text/css" />
<link href="../UITemplet/echarts/css/monokai.css" rel="stylesheet" type="text/css" />
<link href="../UITemplet/echarts/css/codemirror.css" rel="stylesheet" type="text/css" />
<link href="../UITemplet/js/artDialog/skins/blue.css" rel="stylesheet" type="text/css" />
<style>
.badgeDefault {
  display: inline-block;
  min-width: 10px;
  padding: 3px 7px;
  font-size: 12px;
  font-weight: bold;
  line-height: 1;
  color: #ffffff;
  text-align: center;
  white-space: nowrap;
  vertical-align: baseline;
  border-radius: 10px;
}

</style>

<script type="text/javascript">
    var map;
    //var glb_point;
    /* Icon图标 */
    var personIcon, wifiOnIcon, wifiOffIcon,warningIcon;
    // 统计对话框
    var statis_dialog;
    // 点位标记对象集合
    var markers = [];
    var warning_markers = [];
    // 聚合对象
    var markerClusterer = null;
    var APList = [];
    // interval ID
    var intervalid;
    // 获取AP信息的间隔时间 单位s
    var refreshInterval = 30;
    var displayMode = "apMode";
    //读取token
    var token = GetQueryString("token");
    var curLat;
    var curLon;
    var iconStates = { on: true, off: true };

    $(document).ready(function () {
        HighLightMenu("首页");
        $("[name='my_checkbox']").bootstrapSwitch();
        initialize();
        addSwitchEvent();
        $("ul#demo_menu").sidebar({
            position: "right",
            callback: {
                item: {
                    enter: function () {
                        $(this).find("a").animate({ color: "black" }, 250);
                    },
                    leave: function () {
                        $(this).find("a").animate({ color: "white" }, 250);
                    }
                }
            }
        });
    });

    function initialize() {
        setBigData();
        $.ajax({
            type: 'post',
            url: 'AjaxComm.aspx',
            data: 'type=GetAPListForGIS&token=' + token,
            dataType: 'json',
            error: function (msg) {
                //alert("服务器错误");
                initMap();
            },
            success: function (obj) {
                if (obj.ResultCode == 0) {
                    APList = obj.ResultOBJ;
                    initMap();

                } else {
                    alert(obj.ResultMsg);
                    if (obj.ResultCode == -100) {
                        window.location.href = "login.aspx";
                    }
                }
            }
        });
    }

    function setBigData() {
        // 设置总访问量、日均访问量、平均访问时长、当前用户数
        $.ajax({
            type: 'post',
            url: 'AjaxComm.aspx',
            data: 'type=GetPeopleCount&token=' + token,
            dataType: 'json',
            error: function (msg) {
                //alert("服务器错误");
            },
            success: function (obj) {
                if (obj.ResultCode == 0) {
                    $('#total_visit').html(obj.ResultOBJ.AllVisitCounts);
                    $('#avg_visit').html(obj.ResultOBJ.DayAvageVisitCounts);
                    $('#avg_length').html(Math.round(obj.ResultOBJ.AvageVisitTime / 60) + " <span style='font-size:14px;line-height:1.4em'>分钟</span>");
                    $('#cur_user').html(obj.ResultOBJ.OnlinePeopleNum);
                } else {
                    alert(obj.ResultMsg);
                }
            }
        });
    }

    function setDeviceInfo(_APList) {
        var offlAP = _APList.length;
        for (var i = 0; i < _APList.length; i++) {
            if (_APList[i].ISLIVE)
                offlAP--;
        }
        $('#total_ap').html(_APList.length);
        $('#bad_ap').html(offlAP);
        $('#bad_ap').css("color", "red");
    }

    function initMap() {
        map = new BMap.Map('map', { enableMapClick: false });

        if (APList.length == 0) {
            getLocation();   
            return;
        }

        var points = [];
        var point;
        for (var i = 0; i < APList.length; i++) {
            point = new BMap.Point(APList[i].LAT, APList[i].LON);
            points.push(point);
        }
        var portView = map.getViewport(points);

        map.centerAndZoom(portView.center, portView.zoom);
        map.setMapType(BMAP_HYBRID_MAP);
        map.enableScrollWheelZoom();
        map.addControl(new BMap.NavigationControl());
        map.addControl(new BMap.ScaleControl({ offset: new BMap.Size(15, 100) }));
        map.addControl(new BMap.OverviewMapControl());
        map.addControl(new BMap.MapTypeControl({ offset: new BMap.Size(70, 5) }));

        personIcon = new BMap.Icon("../UITemplet/img/person.gif", new BMap.Size(24, 24), { offset: new BMap.Size(12, 24) });
        wifiOnIcon = new BMap.Icon("../UITemplet/img/wifi_on.gif", new BMap.Size(30, 30), { offset: new BMap.Size(15, 30) });
        wifiOffIcon = new BMap.Icon("../UITemplet/img/wifi_off.gif", new BMap.Size(30, 30), { offset: new BMap.Size(15, 30) });
        warningIcon = new BMap.Icon("../UITemplet/img/warning_wifi.gif", new BMap.Size(80, 80), { offset: new BMap.Size(40, 40) });
        addEvent();
        refreshTime = refreshInterval;
        intervalid = window.setInterval("updateRereshTime()", 1000);

        initStatistic();
        updateWiFiView(APList, displayMode);
    }

    function getLocation() {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(function (position) {
                var coords = position.coords;
                map.centerAndZoom(new BMap.Point(coords.longitude,coords.latitude), 15);
                map.setMapType(BMAP_HYBRID_MAP);
                map.enableScrollWheelZoom();
                map.addControl(new BMap.NavigationControl());
                map.addControl(new BMap.ScaleControl({ offset: new BMap.Size(15, 100) }));
                map.addControl(new BMap.OverviewMapControl());
                map.addControl(new BMap.MapTypeControl({ offset: new BMap.Size(70, 5) }));

                personIcon = new BMap.Icon("../UITemplet/img/person.gif", new BMap.Size(24, 24), { offset: new BMap.Size(12, 24) });
                wifiOnIcon = new BMap.Icon("../UITemplet/img/wifi_on.gif", new BMap.Size(30, 30), { offset: new BMap.Size(15, 30) });
                wifiOffIcon = new BMap.Icon("../UITemplet/img/wifi_off.gif", new BMap.Size(30, 30), { offset: new BMap.Size(15, 30) });
                warningIcon = new BMap.Icon("../UITemplet/img/warning_wifi.gif", new BMap.Size(80, 80), { offset: new BMap.Size(40, 40) });
                addEvent();
                refreshTime = refreshInterval;
                intervalid = window.setInterval("updateRereshTime()", 1000);
                initStatistic();
                updateWiFiView(APList, displayMode);
            },
            function (error) {
                switch (error.code) {
                    case 1:
                        alert("位置服务被拒绝。");
                        break;
                    case 2:
                        alert("暂时获取不到位置信息。");
                        break;
                    case 3:
                        alert("获取信息超时。");
                        break;
                    default:
                        alert("未知错误。");
                        break;
                }
                return false;
            });
        } else {
            alert("你的浏览器不支持HTML5来获取地理位置信息。");
        }
    }

    //_type: 0 wifi开  1 wifi关  2 在线人数
    //_markerType:5000 告警图 
    function createMarker(_point, _type, _text, _zIndex) {
        var marker;

        switch (_type) {
            case 0:
                marker = new BMap.Marker(_point, { icon: wifiOnIcon });
                break;
            case 1:
                marker = new BMap.Marker(_point, { icon: wifiOffIcon });
                break;
            case 2:
                marker = new BMapLib.TextIconOverlay(_point, _text);
                break;
            case 3:
                marker = new BMap.Marker(_point, { icon: warningIcon, enableMassClear: false });
                break;
        }
        marker.setZIndex && marker.setZIndex(_zIndex);
        return marker;
    }

    function getAPList() {
        $.ajax({
            type: 'post',
            url: 'AjaxComm.aspx',
            data: 'type=GetAPListForGIS&token=' + token,
            dataType: 'json',
            error: function (msg) {
                //alert("服务器错误");
            },
            success: function (obj) {
                if (obj.ResultCode == 0) {
                    APList = obj.ResultOBJ;
                    updateWiFiView(APList, displayMode);
                } else {
                    //alert(obj.ResultMsg);
                    if (obj.ResultCode == -100) {
                        //window.location.href = "login.aspx";
                    }
                }
            }
        });
    }

    function updateWiFiView(_datas, _displayMode) {
        setDeviceInfo(_datas);
        markers.length = 0;
        map.clearOverlays();
        var point;
        var mkr;
        if (_displayMode == "apMode") {
            if (markerClusterer != null) {
                markerClusterer.clearMarkers();
                markerClusterer.clearEvent();
                markerClusterer = null;
            }
            for (var i = 0; i < _datas.length; i++) {

                point = new BMap.Point(_datas[i].LAT, _datas[i].LON);
                if (_datas[i].ISLIVE) {
                    mkr = createMarker(point, 0, 0, 9999);
                    if (!iconStates.on)
                        mkr.hide();
                    
                }
                else {
                    mkr = createMarker(point, 1, 0, 9999);
                    if (!iconStates.off)
                        mkr.hide();
                    
                }
                map.addOverlay(mkr);
                mkr.tag = _datas[i];
                //mkr.setAnimation(BMAP_ANIMATION_DROP);
                mkr.addEventListener("click", function () {
                    //TODO 统计
                    statis_dialog.iframe.src = 'statistical2.aspx?token=' + token + "&apid=" + this.tag.ID;
                    //$('#statistic_chk').bootstrapSwitch("toggleState");
                    $('#statistic_chk').bootstrapSwitch('state', true, true);
                    statis_dialog.show();
                    statis_dialog._showFrame();
                    
                });

                mkr.addEventListener("mouseout", function () {
                    this.closeInfoWindow();
                });
                mkr.addEventListener("mouseover", function () {
                    var opts = {
                        width: 240,
                        height: 110,
                        enableMessage: false,
                        title: "<b>" + this.tag.ALIAS + "</b>"
                    }
                    this.mInfoWindow = this.mInfoWindow || new BMap.InfoWindow("<div style='font-size:9px'>"
                            + (this.tag.ISLIVE ? "" : ("<span style='color:red'>设备在" + dateFormat(this.tag.LASTHB, "yyyy-MM-dd hh:mm") + "发生故障，请重启</span><br />"))
                            + "地址：" + (this.tag.ADDRESS ? this.tag.ADDRESS : "未知")
                            + "<br />开机时间：" + dateFormat(this.tag.POWERDATETIME, "yyyy-MM-dd hh:mm")
                            + "<br />待机时长：" + Math.round(this.tag.FREETIME / 60) + "分钟"
                            + "<br />网络流量：" + Math.round(this.tag.NETWORKTOTAL / 1024 / 1024) + "mb"
                            + "<br />网络速率：" + Math.round(this.tag.NETWORKRATE / 1024) + "kb/s</div>", opts);
                    this.openInfoWindow(this.mInfoWindow, map.getCenter());
                });

            }
        }
        else if (_displayMode == "personMode") {
            for (var i = 0; i < _datas.length; i++) {
                point = new BMap.Point(_datas[i].LAT, _datas[i].LON);
                if ((_datas[i].ISLIVE && !iconStates.on) || (!_datas[i].ISLIVE && !iconStates.off)) {
                    continue;
                }
                markers.push(createMarker(point, 2, _datas[i].ONLINEPEOPLENUM, 9999));
            }
            markerClusterer = markerClusterer || new BMapLib.MarkerClusterer(map);
            markerClusterer.clearMarkers();
            markerClusterer.addMarkers(markers);
            markerClusterer.setMaxZoom(15);
        }
    }

//    function initSymbol() {
//        var innerhtml1 = '<div style="float:left;font-size:9px"><img alt="" src="../UITemplet/img/wifi_on.gif" style="height:20px"/><span>&nbsp;&nbsp;&nbsp;在线</span></div><div style="float:left;margin-top:4px;font-size:9px"><img alt="" src="../UITemplet/img/wifi_off.gif" style="height:20px" /><span>&nbsp;&nbsp;&nbsp;离线</span></div>';
//        symbol_dialog = art.dialog({
//            padding: '0px',
//            title: '图例',
//            content: innerhtml1,
//            opacity: 0.2,
//            width: 80,
//            left: '0%',
//            height: 65,
//            top: '100%',
//            fixed: true,
//            close: function () {
//                this.hide();
//                return false;
//            }
//        });
//    }

//    function initDeviceInfo() {
//        deviceInfo_dialog = art.dialog({
//            padding: '0px',
//            title: '小记',
//            opacity: 0.2,
//            content:"<br/><br/><br/>",
//            width: 130,
//            left: '0%',
//            height:65,
//            top: '100%',
//            fixed: true,
//            close: function () {
//                this.hide();
//                return false;
//            }
//        });
//    }

    function initStatistic() {
        statis_dialog = art.dialog.open('statistical2.aspx?token=' + token, { title: '统计数据',
            padding: '0px',
            opacity: 0.2,
            top: '100%',
            left: '0%',
            width: '400px',
            height: '220px',
            fixed: true,
            resize: true,
            isPP: true,
            max: true,
            map: map,
            close: function () {
                this.hide();
                $('#statistic_chk').bootstrapSwitch("toggleState");
                return false;
            }
        });
    }

//    function showSymbol() {
//        symbol_dialog.show();
//    }

    function showStatis() {
        statis_dialog.show();
    }

    function addEvent() {
        map.addEventListener("mousemove", function (type) {
            $('#txt_test').val(type.point.lat + " " + type.point.lng);
            glb_point = type.point;
        });
    }

    function openRefresh(flag) {
        if (flag) {
            refreshTime = 0;
            updateRereshTime();
            intervalid = window.setInterval("updateRereshTime()", 1000);
        }
        else {
            clearInterval(intervalid);
            $('#refreshTime').html("");
            //clearInterval(intervalid);
        }
    }

    function updateRereshTime() {
        $('#refreshTime').html(refreshTime + "s");
        if (refreshTime <= 0) {
            getAPList();
            refreshTime = refreshInterval;
        }
        else {
            refreshTime -= 1;
        }
    }

    function displayChangeClick(flag) {
        if (flag)
            displayMode = "apMode";
        else 
            displayMode = "personMode";
        updateWiFiView(APList, displayMode);
    }

    function addSwitchEvent() {
        $('input[name="my_checkbox"]').on('switchChange.bootstrapSwitch', function (event, state) {
            //            console.log(this); // DOM element
            //            console.log(event); // jQuery event
            //            console.log(state); // true | false
            switch ($(this).attr("id")) {
                case "refresh_chk":
                    openRefresh(state);
                    break;
                case "display_chk":
                    displayChangeClick(state);
                    break;
                case "statistic_chk":
                    if (state)
                        statis_dialog.show();
                    else
                        statis_dialog.hide();
                    break;
            }
        });

        $('#icon_display_on').click(function () {
            if (iconStates.on) {
                $(this).removeClass("badge");
                $(this).removeClass("btn-primary");
                iconStates.on = false;
            } else {
                $(this).addClass("badge");
                $(this).addClass("btn-primary");
                iconStates.on = true;
            }
            var overlays = map.getOverlays();
            for (var i = 0; i < overlays.length; i++) {
                if (overlays[i].tag && (overlays[i].tag.ISLIVE))
                    if (iconStates.on)
                        overlays[i].show();
                    else
                        overlays[i].hide();
            }
            //updateWiFiView(APList, displayMode);
        });

        $('#icon_display_off').click(function () {
            if (iconStates.off) {
                $(this).removeClass("badge");
                $(this).removeClass("btn-primary");
                iconStates.off = false;
            } else {
                $(this).addClass("badge");
                $(this).addClass("btn-primary");
                iconStates.off = true;
            }
            var overlays = map.getOverlays();
            for (var i = 0; i < overlays.length; i++) {
                
                if (overlays[i].tag && (!overlays[i].tag.ISLIVE))
                    if (iconStates.off)
                        overlays[i].show();
                    else
                        overlays[i].hide();
            }
            //updateWiFiView(APList, displayMode);
        });
    }

    function redirct(_name) {
        var url = "";
        switch (_name) {
            case "首页":
                url = "gis.aspx?token=" + token;
                break;
            case "控制":
                url = "index.aspx?token=" + token;
                break;
            case "统计":
                url = "statistical1.aspx?token=" + token;
                break;
            case "状态":
                url = "state.aspx?token=" + token;
                break;
        }
        window.location.href = url;
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

    function setWarningMarker(_macs) {
        for (var i = 0; i < warning_markers.length; i++) {
            map.removeOverlay(warning_markers[i]);
        }
        warning_markers.length = 0;

        var macs = _macs;
        var point;
        var marker;
        var count = 0;
        var _points = [];
        for (var i = 0; i < APList.length; i++) {
            if (macs.length == count)
                break;
            for (var j = 0; j < macs.length; j++) {
                if (APList[i].MAC == macs[j]) {
                    count++;
                    point = new BMap.Point(APList[i].LAT, APList[i].LON);
                    _points.push(point);
                    marker = createMarker(point, 3, 0, 5000);
                    warning_markers.push(marker);
                    map.addOverlay(marker);
                    break;
                }
            }
        }
        if (macs.length != 0) {
            var portView = map.getViewport(_points);
            map.centerAndZoom(portView.center, portView.zoom);
        }
    }
</script>
</head>
<body>
<uc1:header runat="server" />

<div class="container-fluid">
<div class="row-fluid">
          <div class="col-md-12">
            <div class="stats-container">
              <div class="col-md-2">
                <div class="number">
                  <i class="fa fa-star-half-o"></i>
                  <span id="total_ap"><small>Loading</small></span>
                </div>
                <div class="text">
                  总设备数
                </div>
              </div>
              <div class="col-md-2">
                <div class="number">
                  <i class="fa fa-power-off"></i>
                  <span id="bad_ap"><small>Loading</small></span>
                </div>
                <div class="text">
                  故障设备数
                </div>
              </div>
              <div class="col-md-2">
                <div class="number">
                  <i class="fa fa-location-arrow"></i>
                  <span id="total_visit"><small>Loading</small></span>
                </div>
                <div class="text">
                  总访问量
                </div>
              </div>
              <div class="col-md-2">
                <div class="number">
                  <i class="fa fa-retweet"></i>
                  <span id="avg_visit"><small>Loading</small></span>
                </div>
                <div class="text">
                  日均访问量
                </div>
              </div>
              <div class="col-md-2">
                <div class="number">
                  <i class="fa fa-flag"></i>
                  <span id="avg_length"><small>Loading</small></span>
                </div>
                <div class="text">
                  平均访问时长
                </div>
              </div>
              <div class="col-md-2">
                <div class="number">
                  <i class="fa fa-user"></i>
                  <span id="cur_user"><small>Loading</small></span>
                </div>
                <div class="text">
                  当前用户数
                </div>
              </div>
            </div>
          </div>
        </div>

      <div  class="row-fluid">
        <div class="div-body  col-md-12" id="ad-list" name="ad-list">
            <div class="panel panle_ssid">
                <div id="map" style="width:100%;height: 400px"></div>
            </div>
        </div>
    </div>
</div>
 <%--onclick="javascript:displayChangeClick(); onclick="javascript:openRefresh();"  --%>
<ul id="demo_menu" class="nav nav-pills nav-stacked">
    <li ><a href="#">设备筛选<span id="icon_display_on" class="badgeDefault btn-primary badge pull-right"><img alt="" src="../UITemplet/img/wifi_on.gif" style="height:20px"/></span><span id="icon_display_off" class="badgeDefault btn-primary badge pull-right"><img alt="" src="../UITemplet/img/wifi_off.gif" style="height:20px"/></span></a></li>
        <li class="divider"></li>
    <li ><a id="auto_refresh_btn" href="#" >自动刷新<span id="refreshTime" class="badge"></span><span class="pull-right"><input id="refresh_chk" name="my_checkbox" type="checkbox" data-size="imini" data-on-text="开" data-off-text="关" data-on-color="success" data-off-color="danger" checked/></span></a></li>
        <li class="divider"></li>
    <li ><a id="displayMode_btn" href="#">显示模式<span class="pull-right"><input id="display_chk" name="my_checkbox" type="checkbox" data-size="mini" data-on-text="设备" data-off-text="人数" data-on-color="success" data-off-color="danger" checked /></span></a></li>
        <li class="divider"></li>
    <li ><a href="#">统计<span class="pull-right"><input id="statistic_chk" name="my_checkbox" type="checkbox" data-size="mini" data-on-text="开" data-off-text="关" data-on-color="success" data-off-color="danger" checked/></span></a></li>
</ul>
<script src="../UITemplet/js/SingleShopMainChart.js" type="text/javascript"></script>
<script src="../UITemplet/echarts/js/echartsExample.js" type="text/javascript"></script>
</body>
</html>
