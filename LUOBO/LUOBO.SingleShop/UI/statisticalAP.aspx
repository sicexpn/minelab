<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="statisticalAP.aspx.cs" Inherits="LUOBO.SingleShop.UI.statisticalAP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>next-wifi 设备详情</title>
    <meta name="viewport" content="initial-scale=1.0, user-scalable=no" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <script type="text/javascript" src="../UITemplet/js/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="../UITemplet/js/jquery-ui-1.10.4.min.js"></script>
    <script type="text/javascript" src="../UITemplet/js/Common.js"></script>
    <script type="text/javascript" src="../UITemplet/js/MarkerClusterer.js"></script>
    <script type="text/javascript" src="http://m.next-wifi.com/Scripts/My97DatePicker/WdatePicker.js"></script>
    <script type="text/javascript">
        function Clone(myObj) {
            if (typeof (myObj) != 'object') return myObj;
            if (myObj == null) return myObj;
            var myNewObj = new Object();
            for (var i in myObj)
                myNewObj[i] = Clone(myObj[i]);
            return myNewObj;
        }
    </script>
    <script src="../UITemplet/js/bootstrap.js" type="text/javascript"></script>
    <script type="text/javascript" src="../UITemplet/js/jquery.sidebar.js"></script>
    <script type="text/javascript" src="../UITemplet/echarts/js/esl.js"></script>
    <script type="text/javascript" src="../UITemplet/echarts/js/codemirror.js"></script>
    <script type="text/javascript" src="../UITemplet/echarts/js/javascript.js"></script>
    <style type="text/css">
        .bodyMain
        {
            background: #FFF;
            border-radius: 4px;
            margin: 0 3px 5px 3px;
            padding: 5px;
            box-shadow: 0 1px 1px rgba(0, 0, 0, 0.15);
        }
        .bodyMain-height
        {
            height: 380px;
        }
        .tbClass
        {
            width: 100%;
        }
        .tbClass tr td
        {
            padding: 4px;
            font-size: 12px;
            line-height: 1.9;
            vertical-align: top;
            border-top: 1px solid #dddddd;
        }
    </style>
    <link href="../UITemplet/css/bootstrap.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/css/font-awesome.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/css/index.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/css/sidebar.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/echarts/css/monokai.css" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/echarts/css/codemirror.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        String.prototype.format = function (args) {
            var result = this;
            if (arguments.length > 0) {
                if (arguments.length == 1 && typeof (args) == "object") {
                    for (var key in args) {
                        if (args[key] != undefined) {
                            var reg = new RegExp("({" + key + "})", "g");
                            result = result.replace(reg, args[key]);
                        }
                    }
                }
                else {
                    for (var i = 0; i < arguments.length; i++) {
                        if (arguments[i] != undefined) {
                            var reg = new RegExp("({)" + i + "(})", "g");
                            result = result.replace(reg, arguments[i]);
                        }
                    }
                }
            }
            return result;
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

        //计算天数差的函数，通用  
        function DateDiff(sDate1, sDate2) {//sDate1和sDate2是2006-12-18格式  
            var aDate, oDate1, oDate2, iDays
            aDate = sDate1.split("-")
            oDate1 = new Date(aDate[1] + '-' + aDate[2] + '-' + aDate[0])//转换为12-18-2006格式  
            aDate = sDate2.split("-")
            oDate2 = new Date(aDate[1] + '-' + aDate[2] + '-' + aDate[0])
            iDays = parseInt(Math.abs(oDate1 - oDate2) / 1000 / 60 / 60 / 24)//把相差的毫秒数转换为天数  
            return iDays
        }
    </script>
    <script type="text/javascript">
        var timeTicket;
        var token = GetQueryString("token");
        var apid = GetQueryString("apid");
        var echarts;
        var options = [];
        var completeNum;
        var startDate = "";
        var endDate = "";

        $().ready(function () {
            $("#startDate").val(new Date(new Date() - (3600000 * 24 * 6)).format("yyyy-MM-dd"));
            $("#endDate").val(new Date().format("yyyy-MM-dd"));
            $("#tbAPInfo td:even").attr("style", "background-color:#f9f9f9");
            showLoadings();
            $.ajax({
                type: 'post',
                url: 'AjaxComm.aspx',
                data: 'type=GetAPListForState&token=' + token,
                dataType: 'json',
                error: function (msg) {
                },
                success: function (obj) {
                    if (obj.ResultCode == "0") {
                        if (obj.ResultOBJ.APList.length > 0) {
                            var item;
                            for (var i = 0; i < obj.ResultOBJ.APList.length; i++) {
                                item = obj.ResultOBJ.APList[i];
                                result = "<option value='" + item.ID + "'>" + item.ALIAS + "</option>";
                                $("#slcAP").append(result);
                            }
                            $("#slcAP option").each(function () {
                                if ($(this).val() == apid)
                                    $(this).attr("selected", true);
                            });

                            completeNum = 0;
                            InitAPInfo();
                            InitPeople();
                            InitSSIDPeople("Week");
                            InitSSIDFlow("Week");
                            InitSSIDTime("Week");
                            InitADPeople("Week");
                            InitAuthenticationPeople("Week");
                            completeAll();
                            timeTicket = setInterval(StatisticalOnline, 5000);
                        }
                    }
                    else {
                        alert(obj.ResultMsg);
                    }
                }
            });

            $("#btnSubmit").click(function () {
                if ($("#startDate").val() == "") {
                    alert("请选择起始时间!");
                    return;
                }
                if ($("#endDate").val() == "") {
                    alert("请选择结束时间!");
                    return;
                }

                showLoadings([1, 2, 3, 4]);
                completeNum = 1;
                startDate = $("#startDate").val();
                endDate = $("#endDate").val();
                InitSSIDPeople("Date");
                InitSSIDFlow("Date");
                InitSSIDTime("Date");
                InitADPeople("Date");
                InitAuthenticationPeople("Date");
                completeAll([1, 2, 3, 4, 5]);
            });

            $("#slcAP").change(function () {
                showLoadings();
                $("#startDate").val("");
                $("#endDate").val("");
                completeNum = 0;
                InitAPInfo();
                InitPeople();
                InitSSIDPeople("Week");
                InitSSIDFlow("Week");
                InitSSIDTime("Week");
                InitADPeople("Week");
                InitAuthenticationPeople("Week");
                completeAll();
                timeTicket = setInterval(StatisticalOnline, 5000);
            });
        });

        function InitAPInfo() {
            $.ajax({
                type: 'post',
                url: 'AjaxComm.aspx',
                data: 'type=APManage/GetAPInfoByID&token=' + token + '&param=' + ($("#slcAP").val() == "-99" ? "0" : $("#slcAP").val()),
                dataType: 'json',
                error: function (msg) {
                },
                success: function (obj) {
                    if (obj.ResultCode == "0") {
                        if (obj.ResultOBJ != null) {
                            $("#tbAPInfo td:eq(1)").html(obj.ResultOBJ.ALIAS);
                            $("#tbAPInfo td:eq(3)").html(obj.ResultOBJ.MAC);
                            $("#tbAPInfo td:eq(5)").html(obj.ResultOBJ.MODEL);
                            $("#tbAPInfo td:eq(7)").html(obj.ResultOBJ.MANUFACTURER);
                            $("#tbAPInfo td:eq(9)").html(obj.ResultOBJ.PURCHASER);
                            $("#tbAPInfo td:eq(11)").html(obj.ResultOBJ.SERIAL);
                            $("#tbAPInfo td:eq(13)").html(obj.ResultOBJ.DESCRIPTION);
                            $("#tbAPInfo td:eq(15)").html(obj.ResultOBJ.ADDRESS);
                            $("#tbAPInfo td:eq(17)").html(obj.ResultOBJ.DEVICESTATE);
                            $("#tbAPInfo td:eq(19)").html(dateFormat(obj.ResultOBJ.ConnectTime, "yyyy-MM-dd hh:mm:ss"));
                            $("#tbAPInfo td:eq(21)").html(dateFormat(obj.ResultOBJ.POWERDATETIME, "yyyy-MM-dd hh:mm:ss"));
                            $("#tbAPInfo td:eq(23)").html(dateFormat(obj.ResultOBJ.LASTHB, "yyyy-MM-dd hh:mm:ss"));
                            $("#tbAPInfo td:eq(25)").html(GetTimeCompany(obj.ResultOBJ.HISTORYTIME));
                            $("#tbAPInfo td:eq(27)").html(GetTimeCompany(obj.ResultOBJ.POWERTIME));
                            $("#tbAPInfo td:eq(29)").html(GetTimeCompany(obj.ResultOBJ.FREETIME));
                            $("#tbAPInfo td:eq(31)").html(obj.ResultOBJ.MAXSSIDNUM);
                            $("#tbAPInfo td:eq(33)").html(obj.ResultOBJ.SUPPORT3G ? "是" : "否");
                            $("#tbAPInfo td:eq(35)").html(obj.ResultOBJ.ONLINEPEOPLENUM);
                            $("#tbAPInfo td:eq(37)").html(obj.ResultOBJ.CPU);
                            $("#tbAPInfo td:eq(39)").html(obj.ResultOBJ.MEMFREE);
                            $("#tbAPInfo td:eq(41)").html(GetTimeCompany(obj.ResultOBJ.HBINTERVAL));
                            $("#tbAPInfo td:eq(43)").html(getUsedTraffic(obj.ResultOBJ.NETWORKRATE) + "/s");
                            $("#tbAPInfo td:eq(45)").html(getUsedTraffic(obj.ResultOBJ.NETWORKTOTAL));
                            return;
                        }
                    }
                    alert(obj.ResultMsg);
                }
            });
        }

        function InitPeople() {
            clearInterval(timeTicket);
            optionOnline.xAxis[0].data = (function () {
                var now = new Date();
                var res = [];
                var len = lineNum;
                while (len--) {
                    res.unshift(now.toTimeString().substr(0, 8));
                    now = new Date(now - 5000);
                }
                return res;
            })();
            optionOnline.series[0].data = (function () {
                var res = [];
                var len = lineNum;
                while (len--) {
                    res.push(0);
                }
                return res;
            })();
            optionOnline.series[1].data = (function () {
                var res = [];
                var len = lineNum;
                while (len--) {
                    res.push(0);
                }
                return res;
            })();
            completeNum++;
        }

        function InitSSIDPeople(mode) {
            var param = '{' + ($("#slcAP").val() == "-99" ? "" : '"ID":"' + $("#slcAP").val() + '",') + '"Type":"AP","Mode":"' + mode + '","StartTime":"' + startDate + '","EndTime":"' + endDate + '"}';
            $("#divSSIDPeopleTableBody tbody").html("<tr><td colspan='4'>正在加载...</td></tr>");
            $.ajax({
                type: 'post', //可选get
                url: 'AjaxComm.aspx', //这里是接收数据的PHP程序
                data: 'type=GetSSIDPeopleStatistical&token=' + token + '&param=' + param,
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    var tbStr = "";
                    optionSSIDPeople.xAxis[0].data = new Array();
                    optionSSIDPeople.series[0].data = new Array();
                    optionSSIDPeople.series[1].data = new Array();
                    optionSSIDPeople.series[2].data = new Array();
                    for (var i = 0; i < obj.ResultOBJ.length; i++) {
                        optionSSIDPeople.xAxis[0].data.push(obj.ResultOBJ[i].NAME);
                        optionSSIDPeople.series[0].data.push(obj.ResultOBJ[i].NUM[1]);
                        optionSSIDPeople.series[1].data.push(obj.ResultOBJ[i].NUM[2]);
                        optionSSIDPeople.series[2].data.push(obj.ResultOBJ[i].NUM[0]);

                        tbStr += "<tr><td>" + obj.ResultOBJ[i].NAME + "</td><td>" + obj.ResultOBJ[i].NUM[1] + "</td><td>" + obj.ResultOBJ[i].NUM[2] + "</td><td>" + obj.ResultOBJ[i].NUM[0] + "</td></tr>";
                    }
                    $("#divSSIDPeopleTableBody tbody").html(tbStr);
                    completeNum++;
                }
            });
        }

        function InitSSIDFlow(mode) {
            var param = '{' + ($("#slcAP").val() == "-99" ? "" : '"ID":"' + $("#slcAP").val() + '",') + '"Type":"AP","Mode":"' + mode + '","StartTime":"' + startDate + '","EndTime":"' + endDate + '"}';
            $("#divSSIDFlowTableBody tbody").html("<tr><td colspan='3'>正在加载...</td></tr>");
            $.ajax({
                type: 'post', //可选get
                url: 'AjaxComm.aspx', //这里是接收数据的PHP程序
                data: 'type=GetSSIDTrafficStatistical&token=' + token + '&param=' + param,
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    var tbStr = "";
                    optionSSIDFlow.xAxis[0].data = new Array();
                    optionSSIDFlow.series[0].data = new Array();
                    optionSSIDFlow.series[1].data = new Array();
                    for (var i = 0; i < obj.ResultOBJ.length; i++) {
                        optionSSIDFlow.xAxis[0].data.push(obj.ResultOBJ[i].NAME);
                        optionSSIDFlow.series[0].data.push((obj.ResultOBJ[i].NUM[0] / 1024 / 1024).toFixed(2));
                        optionSSIDFlow.series[1].data.push((obj.ResultOBJ[i].NUM[1] / 1024 / 1024).toFixed(2));

                        tbStr += "<tr><td>" + obj.ResultOBJ[i].NAME + "</td><td>" + (obj.ResultOBJ[i].NUM[0] / 1024 / 1024).toFixed(2) + "MB</td><td>" + (obj.ResultOBJ[i].NUM[1] / 1024 / 1024).toFixed(2) + "MB</td></tr>";
                    }
                    $("#divSSIDFlowTableBody tbody").html(tbStr);
                    completeNum++;
                }
            });
        }

        function InitSSIDTime(mode) {
            var param = '{' + ($("#slcAP").val() == "-99" ? "" : '"ID":"' + $("#slcAP").val() + '",') + '"Type":"AP","Mode":"' + mode + '","StartTime":"' + startDate + '","EndTime":"' + endDate + '"}';
            $("#divSSIDTimeTableBody tbody").html("<tr><td colspan='3'>正在加载...</td></tr>");
            $.ajax({
                type: 'post', //可选get
                url: 'AjaxComm.aspx', //这里是接收数据的PHP程序
                data: 'type=GetSSIDUseTimeStatistical&token=' + token + '&param=' + param,
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    var tbStr = "";
                    optionSSIDTime.xAxis[0].data = new Array();
                    optionSSIDTime.series[0].data = new Array();
                    optionSSIDTime.series[1].data = new Array();
                    for (var i = 0; i < obj.ResultOBJ.length; i++) {
                        optionSSIDTime.xAxis[0].data.push(obj.ResultOBJ[i].NAME);
                        optionSSIDTime.series[0].data.push((obj.ResultOBJ[i].NUM[0] / 60).toFixed(2));
                        optionSSIDTime.series[1].data.push((obj.ResultOBJ[i].NUM[1] / 60).toFixed(2));

                        tbStr += "<tr><td>" + obj.ResultOBJ[i].NAME + "</td><td>" + (obj.ResultOBJ[i].NUM[0] / 60).toFixed(2) + "分钟</td><td>" + (obj.ResultOBJ[i].NUM[1] / 60).toFixed(2) + "分钟</td></tr>";
                    }
                    $("#divSSIDTimeTableBody tbody").html(tbStr);
                    completeNum++;
                }
            });
        }

        function InitADPeople(mode) {
            var param = '{' + ($("#slcAP").val() == "-99" ? "" : '"ID":"' + $("#slcAP").val() + '",') + '"Type":"AP","Mode":"' + mode + '","StartTime":"' + startDate + '","EndTime":"' + endDate + '"}';
            $("#divADPeopleTableBody tbody").html("<tr><td colspan='3'>正在加载...</td></tr>");
            $.ajax({
                type: 'post', //可选get
                url: 'AjaxComm.aspx', //这里是接收数据的PHP程序
                data: 'type=GetAPOfADStatistical&token=' + token + '&param=' + param,
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    var tbStr = "";
                    optionADPeople.xAxis[0].data = new Array();
                    optionADPeople.series[0].data = new Array();
                    optionADPeople.series[1].data = new Array();
                    for (var i = 0; i < obj.ResultOBJ.length; i++) {
                        optionADPeople.xAxis[0].data.push(obj.ResultOBJ[i].NAME);
                        optionADPeople.series[0].data.push(obj.ResultOBJ[i].NUM[1]);
                        optionADPeople.series[1].data.push(obj.ResultOBJ[i].NUM[0]);

                        tbStr += "<tr><td>" + obj.ResultOBJ[i].NAME + "</td><td>" + obj.ResultOBJ[i].NUM[1] + "</td><td>" + obj.ResultOBJ[i].NUM[0] + "</td></tr>";
                    }
                    $("#divADPeopleTableBody tbody").html(tbStr);
                    completeNum++;
                }
            });
        }

        function InitAuthenticationPeople(mode) {
            var param = '{' + ($("#slcAP").val() == "-99" ? "" : '"ID":"' + $("#slcAP").val() + '",') + '"Type":"AP","Mode":"' + mode + '","StartTime":"' + startDate + '","EndTime":"' + endDate + '"}';
            $("#divAuthenticationPeopleTableBody tbody").html("<tr><td colspan='3'>正在加载...</td></tr>");
            $.ajax({
                type: 'post', //可选get
                url: 'AjaxComm.aspx', //这里是接收数据的PHP程序
                data: 'type=GetAuthenticationPeopleStatistical&token=' + token + '&param=' + param,
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    var tbStr = "";
                    optionAuthenticationPeople.legend.data = new Array();
                    optionAuthenticationPeople.series[0].data = new Array();
                    for (var i = 0; i < obj.ResultOBJ.length; i++) {
                        optionAuthenticationPeople.legend.data.push(getOlineType(obj.ResultOBJ[i].NAME));
                        optionAuthenticationPeople.series[0].data.push({ value: obj.ResultOBJ[i].NUM, name: getOlineType(obj.ResultOBJ[i].NAME) });

                        tbStr += "<tr><td>" + getOlineType(obj.ResultOBJ[i].NAME) + "</td><td>" + obj.ResultOBJ[i].NUM + "</td></tr>";
                    }
                    $("#divAuthenticationPeopleTableBody tbody").html(tbStr);
                    completeNum++;
                }
            });
        }

        function getOlineType(_id) {
            var mid = Number(_id);
            var tmp;
            if (isNaN(mid)) {
                tmp = "多种认证";
            } else {
                switch (mid) {
                    case -1:
                        tmp = "未认证";
                        break;
                    case 0:
                        tmp = "一键登录";
                        break;
                    case 1:
                        tmp = "QQ登陆";
                        break;
                    case 2:
                        tmp = "微博登陆";
                        break;
                    case 3:
                        tmp = "微信登陆";
                        break;
                    default:
                        tmp = "未知";
                        break;
                }
            }
            return tmp;
        }

        function requireCallback(ec) {
            echarts = ec;
            if (myChart.length > 0) {
                for (var i = 0, l = myChart.length; i < l; i++) {
                    myChart[i].dispose && myChart[i].dispose();
                }
            }
            myChart = [];
            for (var i = 0, l = domMain.length; i < l; i++) {
                myChart[i] = echarts.init(domMain[i]);
            }

            if (options.length == 0) {
                initOption();
                //$("[md='wrong-message']").html("提示信息显示部分");
            }

            window.onresize = function () {
                for (var i = 0, l = myChart.length; i < l; i++) {
                    myChart[i].resize && myChart[i].resize();
                }
            };
        }

        function initOption() {
            options.push(optionOnline);
            options.push(optionSSIDPeople);
            options.push(optionSSIDFlow);
            options.push(optionSSIDTime);
            options.push(optionADPeople);
            options.push(optionAuthenticationPeople);
        }

        function completeAll(obj) {
            if (completeNum == $("[md='main']").length)
                refreshAll(obj);
            else
                setTimeout(function () { completeAll(obj) }, 10);
        }

        function StatisticalWeek() {
            $("#startDate").val(new Date(new Date() - (3600000 * 24 * 6)).format("yyyy-MM-dd"));
            $("#endDate").val(new Date().format("yyyy-MM-dd"));
            showLoadings([1, 2, 3, 4]);
            completeNum = 1;
            InitSSIDPeople("Week");
            InitSSIDFlow("Week");
            InitSSIDTime("Week");
            InitADPeople("Week");
            InitAuthenticationPeople("Week");
            completeAll([1, 2, 3, 4, 5]);
        }

        function StatisticalMonth() {
            $("#startDate").val(new Date(new Date() - (3600000 * 24 * 29)).format("yyyy-MM-dd"));
            $("#endDate").val(new Date().format("yyyy-MM-dd"));
            showLoadings([1, 2, 3, 4]);
            completeNum = 1;
            InitSSIDPeople("Month");
            InitSSIDFlow("Month");
            InitSSIDTime("Month");
            InitADPeople("Month");
            InitAuthenticationPeople("Month");
            completeAll([1, 2, 3, 4, 5]);
        }

        function StatisticalYear() {
            $("#startDate").val(new Date(new Date() - (3600000 * 24 * 30 * 11)).format("yyyy-MM-dd"));
            $("#endDate").val(new Date().format("yyyy-MM-dd"));
            showLoadings([1, 2, 3, 4]);
            completeNum = 1;
            InitSSIDPeople("Year");
            InitSSIDFlow("Year");
            InitSSIDTime("Year");
            InitADPeople("Year");
            InitAuthenticationPeople("Year");
            completeAll([1, 2, 3, 4, 5]);
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
            <div class="div-body col-md-12">
                <div class="bodyMain">
                    设备：<select id="slcAP"></select>
                </div>
            </div>
        </div>
        <div class="row-fluid">
            <div class="div-body col-md-6">
                <div class="bodyMain" style="height: auto">
                    <table id="tbAPInfo" class="tbClass hor-minimalist-top table-bordered">
                        <tr>
                            <td>设备名称</td>
                            <td></td>
                            <td>MAC地址</td>
                            <td></td>
                            <td>型号</td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>制造商</td>
                            <td></td>
                            <td>购买人</td>
                            <td></td>
                            <td>产品编号</td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>简要说明</td>
                            <td></td>
                            <td>地址</td>
                            <td></td>
                            <td>设备状态</td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>注册日期</td>
                            <td></td>
                            <td>启动时间</td>
                            <td></td>
                            <td>最后心跳时间</td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>历史时长</td>
                            <td></td>
                            <td>开机时长</td>
                            <td></td>
                            <td>待机时长</td>
                            <td></td>
                        </tr>


                        <tr>
                            <td>最大SSID数</td>
                            <td></td>
                            <td>支持3G</td>
                            <td></td>
                            <td>在线人数</td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>cpu占用率%</td>
                            <td></td>
                            <td>空闲内存</td>
                            <td></td>
                            <td>心跳上报间隔</td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>网络速率</td>
                            <td></td>
                            <td>开机后总流量</td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="div-body col-md-6">
                <div id="divOnline" class="bodyMain" style="height: 259px">
                    <div id="divOnlineBody" md="main" style="height: 95%"></div>
                    <span md="wrong-message" style="color: red"></span>
                    <script type="text/javascript">
                        var lineNum = 50;
                        optionOnline = {
                            title: {
                                text: '在线人数统计',
                                x: 'center'
                            },
                            tooltip: {
                                trigger: 'axis'
                            },
                            legend: {
                                orient: 'vertical',
                                x: 'right',
                                y: 'center',
                                data: ['在线人数', '在线人次']
                            },
                            grid: {
                                y: 30,
                                x2: 100,
                                y2: 40
                            },
                            xAxis: [
                                {
                                    type: 'category',
                                    boundaryGap: true,
                                    axisLabel: {
                                        interval: 'auto',
                                        rotate: 0
                                    },
                                    data: (function () {
                                        var now = new Date();
                                        var res = [];
                                        var len = lineNum;
                                        while (len--) {
                                            res.unshift(now.toTimeString().substr(0, 8));
                                            now = new Date(now - 60000);
                                        }
                                        return res;
                                    })()
                                }
                            ],
                            yAxis: [
                                {
                                    type: 'value',
                                    scale: true,
                                    name: '人数/人次',
                                    boundaryGap: [0.2, 0.2]
                                }
                            ],
                            series: [
                                {
                                    name: '在线人数',
                                    type: 'line',
                                    smooth: true,
                                    data: (function () {
                                        var res = [];
                                        var len = lineNum;
                                        while (len--) {
                                            res.push(0);
                                        }
                                        return res;
                                    })()
                                },
                                {
                                    name: '在线人次',
                                    type: 'line',
                                    smooth: true,
                                    data: (function () {
                                        var res = [];
                                        var len = lineNum;
                                        while (len--) {
                                            res.push(0);
                                        }
                                        return res;
                                    })()
                                }
                            ]
                        };
                            
                        function StatisticalOnline() {
                            $.ajax({
                                type: 'post', //可选get
                                url: 'AjaxComm.aspx', //这里是接收数据的PHP程序
                                data: 'type=StatisticalOnline&token=' + token + '&param={' + ($("#slcAP").val() == "-99" ? "" : '"ID":"' + $("#slcAP").val() + '",') + '"Type":"AP","Mode":"RealTime"}',
                                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                                success: function (obj) {
                                    myChart[0].addData(obj);
                                }
                            });
                        }

                    </script>
                </div>
            </div>
        </div>
        <div class="row-fluid">
            <div class="div-body col-md-12">
                <div class="bodyMain" style="min-height:34px">
                    <div class="div-body col-md-3">
                        <input id="btnWeek" style="width: 70px;" type="button" value="近一周" onclick="StatisticalWeek()" />
                        <input id="btnMonth" style="width: 70px;" type="button" value="近一月" onclick="StatisticalMonth()" />
                        <input id="btnYear" style="width: 70px;" type="button" value="近一年" onclick="StatisticalYear()" />
                    </div>
                    <div class="div-body col-md-9">
                        时间区间：
                        <input id="startDate" type="text" style="width:100px" class="Wdate" onFocus="WdatePicker({isShowWeek:true})"/>
                        -
                        <input id="endDate" type="text" style="width:100px" class="Wdate" onFocus="WdatePicker({isShowWeek:true})"/>
                        <input id="btnSubmit" style="width: 70px;" type="button" value="统计" onclick="" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row-fluid">
            <div class="div-body col-md-4">
                <div id="divSSIDPeople" class="bodyMain bodyMain-height">
                    <div id="divSSIDPeopleBody" md="main" style="height: 46%"></div>
                    <span md="wrong-message" style="color: red"></span>
                    <script type="text/javascript">
                        optionSSIDPeople = {
                            title: {
                                text: 'SSID人数统计',
                                x: 'center'
                            },
                            grid: {
                                x2: 100,
                                y2: 30,
                                x: 50,
                                y: 30
                            },
                            tooltip: {
                                trigger: 'axis',
                                axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                                    type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
                                }
                            },
                            legend: {
                                data: ['认证人数', '未认证人数', '连接人数'],
                                orient : 'vertical',
                                x : 'right',
                                y : 'center'
                            },
                            xAxis: [
                                {
                                    type: 'category',
                                    data: ['周一', '周二', '周三', '周四', '周五', '周六', '周日']
                                }
                            ],
                            yAxis: [
                                {
                                    type: 'value',
                                    splitArea: { show: true }
                                }
                            ],
                            series: [
                                {
                                    name: '认证人数',
                                    type: 'bar',
                                    stack: '人数',
                                    data: [220, 182, 191, 234, 290, 330, 310]
                                },
                                {
                                    name: '未认证人数',
                                    type: 'bar',
                                    stack: '人数',
                                    data: [150, 232, 201, 154, 190, 330, 410]
                                },
                                {
                                    name: '连接人数',
                                    type: 'bar',
                                    data: [862, 1018, 964, 1026, 1679, 1600, 1570]
                                }
                            ]
                        };
                    </script>

                    <div id="divSSIDPeopleTableBody" style="width: 100%; height: 50%">
                        <table class="tbClass hor-minimalist-top table-bordered table-striped">
                            <thead>
                                <tr>
                                    <td>名称</td>
                                    <td>认证人数</td>
                                    <td>未认证人数</td>
                                    <td>连接人数</td>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            <div class="div-body col-md-4">
                <div id="divSSIDFlow" class="bodyMain bodyMain-height">
                    <div id="divSSIDFlowBody" md="main" style="height: 46%"></div>
                    <span md="wrong-message" style="color: red"></span>
                    <script type="text/javascript">
                        var optionSSIDFlow = {
                            title: {
                                text: 'SSID流量统计',
                                x: 'center'
                            },
                            grid: {
                                x2: 100,
                                y2: 30,
                                x: 50,
                                y: 30
                            },
                            tooltip: {
                                trigger: 'axis',
                                axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                                    type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
                                },
                                formatter: '{b}<br/>{a0}:{c0}MB<br/>{a1}:{c1}MB'
                            },
                            legend: {
                                data: ['使用流量', '平均流量'],
                                orient: 'vertical',
                                x: 'right',
                                y: 'center'
                            },
                            xAxis: [
                                {
                                    type: 'category',
                                    data: ['周一', '周二', '周三', '周四', '周五', '周六', '周日']
                                }
                            ],
                            yAxis: [
                                {
                                    type: 'value',
                                    name: '单位:MB',
                                    splitArea: { show: true }
                                }
                            ],
                            series: [
                                {
                                    name: '使用流量',
                                    type: 'bar',
                                    data: [220, 182, 191, 234, 290, 330, 310]
                                },
                                {
                                    name: '平均流量',
                                    type: 'bar',
                                    data: [150, 232, 201, 154, 190, 330, 410]
                                }
                            ]
                        };
                    </script>

                    <div id="divSSIDFlowTableBody" style="width: 100%; height: 50%">
                        <table class="tbClass hor-minimalist-top table-bordered table-striped">
                            <thead>
                                <tr>
                                    <td>名称</td>
                                    <td>使用流量</td>
                                    <td>平均流量</td>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            <div class="div-body col-md-4">
                <div id="divSSIDTime" class="bodyMain bodyMain-height">
                    <div id="divSSIDTimeBody" md="main" style="height: 46%"></div>
                    <span md="wrong-message" style="color: red"></span>
                    <script type="text/javascript">
                        var optionSSIDTime = {
                            title: {
                                text: 'SSID时长统计',
                                x: 'center'
                            },
                            grid: {
                                x2: 115,
                                y2: 30,
                                x: 50,
                                y: 30
                            },
                            tooltip: {
                                trigger: 'axis',
                                axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                                    type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
                                },
                                formatter: '{b}<br/>{a0}:{c0}分钟<br/>{a1}:{c1}分钟'
                            },
                            legend: {
                                data: ['总连接时长', '平均连接时长'],
                                orient: 'vertical',
                                x: 'right',
                                y: 'center'
                            },
                            xAxis: [
                                {
                                    type: 'category',
                                    data: ['周一', '周二', '周三', '周四', '周五', '周六', '周日']
                                }
                            ],
                            yAxis: [
                                {
                                    type: 'value',
                                    name: '单位:分钟',
                                    splitArea: { show: true }
                                }
                            ],
                            series: [
                                {
                                    name: '总连接时长',
                                    type: 'bar',
                                    data: [220, 182, 191, 234, 290, 330, 310]
                                },
                                {
                                    name: '平均连接时长',
                                    type: 'bar',
                                    data: [150, 232, 201, 154, 190, 330, 410]
                                }
                            ]
                        };
                    </script>

                    <div id="divSSIDTimeTableBody" style="width: 100%; height: 50%">
                        <table class="tbClass hor-minimalist-top table-bordered table-striped">
                            <thead>
                                <tr>
                                    <td>名称</td>
                                    <td>总连接时长</td>
                                    <td>平均连接时长</td>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
        <div class="row-fluid">
            <div class="div-body col-md-4">
                <div id="divADPeople" class="bodyMain bodyMain-height">
                    <div id="divADPeopleBody" md="main" style="height: 46%"></div>
                    <span md="wrong-message" style="color: red"></span>
                    <script type="text/javascript">
                        optionADPeople = {
                            title: {
                                text: '广告人数统计',
                                x: 'center'
                            },
                            grid: {
                                x2: 100,
                                y2: 30,
                                x: 50,
                                y: 30
                            },
                            tooltip: {
                                trigger: 'axis',
                                axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                                    type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
                                }
                            },
                            legend: {
                                data: ['访问人数', '访问人次'],
                                orient: 'vertical',
                                x: 'right',
                                y: 'center'
                            },
                            xAxis: [
                                {
                                    type: 'category',
                                    data: ['周一', '周二', '周三', '周四', '周五', '周六', '周日']
                                }
                            ],
                            yAxis: [
                                {
                                    type: 'value',
                                    splitArea: { show: true }
                                }
                            ],
                            series: [
                                {
                                    name: '访问人数',
                                    type: 'bar',
                                    data: [220, 182, 191, 234, 290, 330, 310]
                                },
                                {
                                    name: '访问人次',
                                    type: 'bar',
                                    data: [150, 232, 201, 154, 190, 330, 410]
                                }
                            ]
                        };
                    </script>

                    <div id="divADPeopleTableBody" style="width: 100%; height: 50%">
                        <table class="tbClass hor-minimalist-top table-bordered table-striped">
                            <thead>
                                <tr>
                                    <td>名称</td>
                                    <td>访问人数</td>
                                    <td>访问人次</td>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            <div class="div-body col-md-4">
                <div id="divAuthenticationPeople" class="bodyMain bodyMain-height">
                    <div id="divAuthenticationPeopleBody" md="main" style="height: 46%"></div>
                    <span md="wrong-message" style="color: red"></span>
                    <script type="text/javascript">
                        optionAuthenticationPeople = {
                            title: {
                                text: '认证类型人数统计',
                                x: 'right'
                            },
                            tooltip: {
                                trigger: 'item',
                                formatter: "{a} <br/>{b} : {c} ({d}%)"
                            },
                            legend: {
                                orient: 'vertical',
                                x: 'right',
                                y: 'center',
                                data: ['未认证用户数', '认证用户数']
                            },
                            series: [
                                {
                                    name: '有效用户统计',
                                    type: 'pie',
                                    radius: '50%',
                                    startAngle: 90,
                                    minAngle: 0,
                                    center: ['35%', 90],
                                    data: [
                                        { value: 335, name: '未认证用户数' },
                                        { value: 1048, name: '认证用户数' }
                                    ]
                                }
                            ]
                        };
                    </script>

                    <div id="divAuthenticationPeopleTableBody" style="width: 100%; height: 50%">
                        <table class="tbClass hor-minimalist-top table-bordered table-striped">
                            <thead>
                                <tr>
                                    <td>认证类型</td>
                                    <td>认证人数</td>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="../UITemplet/echarts/js/echartsExample.js" type="text/javascript"></script>
</body>
</html>
