<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="report.aspx.cs" Inherits="LUOBO.SingleShop.UI.report" %>
<%@ Register src="header.ascx" tagname="header" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>next-wifi 报表</title>
    <meta name="viewport" content="initial-scale=1.0, user-scalable=no" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <script type="text/javascript" src="../UITemplet/js/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="../UITemplet/js/jquery-ui-1.10.4.min.js"></script>
    <script type="text/javascript" src="../UITemplet/js/Common.js"></script>
    <script type="text/javascript" src="../UITemplet/js/MarkerClusterer.js"></script>
    <script type="text/javascript" src="http://m.next-wifi.com/Scripts/My97DatePicker/WdatePicker.js"></script>
    <script src="../UITemplet/js/bootstrap.js" type="text/javascript"></script>
    <script type="text/javascript" src="../UITemplet/js/jquery.sidebar.js"></script>
    <script type="text/javascript" src="../UITemplet/echarts/js/esl.js"></script>
    <script type="text/javascript" src="../UITemplet/echarts/js/codemirror.js"></script>
    <script type="text/javascript" src="../UITemplet/echarts/js/javascript.js"></script>

    <link href="../UITemplet/css/bootstrap.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/css/font-awesome.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/css/index.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/css/sidebar.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/css/jquery-ui.min.css" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/echarts/css/monokai.css" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/echarts/css/codemirror.css" rel="stylesheet" type="text/css" />
    
    <style type="text/css">
        .bodyMain
        {
            background: #FFF;
            height: 380px;
            border-radius: 4px;
            margin: 0 3px 5px 3px;
            padding: 5px;
            box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.15);
        }
        .tbClass
        {
            width: 100%;
            margin-bottom: 20px;
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
    <script type="text/javascript">
        function Clone(myObj) {
            if (typeof (myObj) != 'object') return myObj;
            if (myObj == null) return myObj;
            var myNewObj = new Object();
            for (var i in myObj)
                myNewObj[i] = Clone(myObj[i]);
            return myNewObj;
        }

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

        function GetCurrMonthDay() {
            var date = new Date();
            date.setMonth(date.getMonth() + 1);
            date = new Date(date.format('yyyy-MM') + '-1');
            date.setDate(date.getDate() - 1);
            return date.getDate();
        }

        //by函数接受一个成员名字符串做为参数
        //并返回一个可以用来对包含该成员的对象数组进行排序的比较函数
        var by = function (name) {
            return function (o, p) {
                var a, b;
                if (typeof o === "object" && typeof p === "object" && o && p) {
                    a = o[name];
                    b = p[name];
                    if (a === b) {
                        return 0;
                    }
                    if (typeof a === typeof b) {
                        return a < b ? -1 : 1;
                    }
                    return typeof a < typeof b ? -1 : 1;
                }
                else {
                    throw ("error");
                }
            }
        }
    </script>
    <script type="text/javascript">
        var token = GetQueryString("token");
        var echarts;
        var options = [];
        var completeNum = 0;
        myChart = {};
        var effectOption = { text: '载入中...', textStyle: { fontSize: 20 }, effect: 'bubble' };
        
        function requireCallback(ec) {
            echarts = ec;
            for (chart in myChart) {
                myChart[chart].dispose && myChart[chart].dispose();
            }
            myChart = {};

            for (var i = 0, l = domMain.length; i < l; i++) {
                myChart[$(domMain[i]).attr("id")] = echarts.init(domMain[i]);
            }
            
            initReady();

            window.onresize = function () {
                for (key in myChart) {
                    myChart[key].resize && myChart[key].resize();
                }
            };
        }
    </script>

    <script type="text/javascript">
        var param = '';
        var today = new Date();
        today.setMonth(today.getMonth() - 1);
        $(document).ready(function () {
            HighLightMenu("报表");
            $("#startDate").val(today.format('yyyy-MM'));
            $("#btnRefresh").click(function () { initReady(); });

            var currPID = 0;
            for (var i = 0; i < Public_Menu.length; i++) {
                if (Public_Menu[i].M_NAME == '报表') {
                    currPID = Public_Menu[i].M_ID;
                    break;
                }
            }
            var strUL = "";
            for (var i = 0; i < Public_Menu.length; i++) {
                if (Public_Menu[i].M_PID == currPID && Public_Menu[i].M_LEVEL == 2) {
                    strUL += '<li><a href="' + Public_Menu[i].M_URL + '">' + Public_Menu[i].M_NAME + '</a></li>';
                }
            }
            $("#tabs ul:eq(0)").append(strUL);

            $('#tabs').tabs({ activate: function (event, ui) {
                switch ($(ui.newTab).text()) {
                    case "总体情况":
                        myChart["divTotalDownBody"].resize();
                        myChart["divTotalDownBody"].setOption(totalDownOption, true);
                        myChart["divTotalDiffOSBody"].resize();
                        myChart["divTotalDiffOSBody"].setOption(totalDiffOSOption, true);
                        break;
                    case "APP下载分析":
                        myChart["divAppDownYingYeTingBody"].resize();
                        myChart["divAppDownYingYeTingBody"].setOption(appDownYingYeTingOption, true);
                        appDownDiffTimeOption.legend.x = document.getElementById('divAppDownDiffTimeBody').offsetWidth / 2 + 5;
                        myChart["divAppDownDiffTimeBody"].resize();
                        myChart["divAppDownDiffTimeBody"].setOption(appDownDiffTimeOption, true);
                        myChart["divAppDownDiffOSBody"].resize();
                        myChart["divAppDownDiffOSBody"].setOption(appDownDiffOSOption, true);
                        break;
                    case "业务推广分析":
                        myChart["divPromotionYingYeTingBody"].resize();
                        myChart["divPromotionYingYeTingBody"].setOption(promotionYingYeTingOption, true);
                        promotionDiffTimeOption.legend.x = document.getElementById('divPromotionDiffTimeBody').offsetWidth / 2 + 5;
                        myChart["divPromotionDiffTimeBody"].resize();
                        myChart["divPromotionDiffTimeBody"].setOption(promotionDiffTimeOption, true);
                        myChart["divPromotionDiffOSBody"].resize();
                        myChart["divPromotionDiffOSBody"].setOption(promotionDiffOSOption, true);
                        myChart["divPromotionDiffADBody"].resize();
                        myChart["divPromotionDiffADBody"].setOption(promotionDiffADOption, true);
                        break;
                    case "营业厅分析":
                        myChart["divYingYeTingConnNumBody"].resize();
                        myChart["divYingYeTingConnNumBody"].setOption(yingYeTingConnNumOption, true);
                        myChart["divYingYeTingConnNumSumBody"].resize();
                        myChart["divYingYeTingConnNumSumBody"].setOption(yingYeTingConnNumSumOption, true);
                        myChart["divYingYeTingDiffOSBody"].resize();
                        myChart["divYingYeTingDiffOSBody"].setOption(yingYeTingDiffOSOption, true);
                        myChart["divYingYeTingDiffOSSumBody"].resize();
                        myChart["divYingYeTingDiffOSSumBody"].setOption(yingYeTingDiffOSSumOption, true);
                        break;
                    case "用户行为与构成分析":
                        myChart["divUserBehaviorBody"].resize();
                        myChart["divUserBehaviorBody"].setOption(userBehaviorOption, true);
                        break;
                    case "安全分析":
                        myChart["divAnQuanBody"].resize();
                        myChart["divAnQuanBody"].setOption(anQuanOption, true);
                        myChart["divAnQuanSumBody"].resize();
                        myChart["divAnQuanSumBody"].setOption(anQuanSumOption, true);
                        break;
                }
            }
            });
        });

        function initReady() {
            param = '{"StartTime": "' + $("#startDate").val() + '-01"}';
            GetUserBehaviorTableAndChart();
            GetAppDownloadTableAndChart();
            GetTotal();
            GetPromotionTableAndChart();
            GetYingYeTingTableAndChart();
            GetAnQuanTableAndChart();
        }
        
        function GetTotal() {
            //var param = '{"StartTime":"2014-07-01","EndTime":"2014-07-31"}';
            $('#totalTbody').empty();
            myChart["divTotalDownBody"].showLoading(effectOption);
            myChart["divTotalDiffOSBody"].showLoading(effectOption);

            $.ajax({
                type: 'post',
                url: 'AjaxComm.aspx',
                data: 'type=StatisticalMonth/Total&token=' + token + '&param=' + param,
                dataType: 'json',
                error: function (msg) {

                },
                success: function (obj) {
                    if (obj.ResultCode == "0") {
                        var ios = 0;
                        var fios = 0;
                        for (var j = 0; j < obj.ResultOBJ.BUTCZXTXZB.length; j++) {
                            if (obj.ResultOBJ.BUTCZXTXZB[j].NAME == "iPhone" || obj.ResultOBJ.BUTCZXTXZB[j].NAME == "iPad")
                                ios += obj.ResultOBJ.BUTCZXTXZB[j].NUM;
                            else
                                fios += obj.ResultOBJ.BUTCZXTXZB[j].NUM;
                        }

                        totalDownOption.series[0].data = [];
                        totalDiffOSOption.series[0].data = [];
                        // ------总体情况Table------
                        var str = "<tr>"
                            + "<td><strong>总人数</strong></td><td>" + obj.ResultOBJ.ZONGRS + "</td>"
                            + "<td><strong>广告点击数</strong></td><td>" + obj.ResultOBJ.GUANGGDJRC + "</td>"
                            + "<td><strong>连接人群下载比</strong></td><td>" + (obj.ResultOBJ.LIANJRQXZB * 100).toFixed(2) + "%</td>"
                            + "</tr><tr>"
                            + "<td><strong>总上网人次</strong></td><td>" + obj.ResultOBJ.ZONGSWRC + "</td>"
                            + "<td><strong>下载人次</strong></td><td>" + obj.ResultOBJ.XIAZRC + "</td>"
                            + "<td><strong>不同操作系统下载比</strong></td><td>" + (ios * 100.0 / (ios + fios)).toFixed(2) + "%/" + (fios * 100.0 / (ios + fios)).toFixed(2) + "%（IOS/非IOS）</td>"
                            + "</tr><tr>"
                            + "<td><strong>平均上网次数</strong></td><td>" + obj.ResultOBJ.PINGJSWCS.toFixed(2) + "</td>"
                            + "<td><strong>平均上网时长</strong></td><td>" + obj.ResultOBJ.PINGJSWSC + "分钟</td>"
                            + "<td><strong>平均营业厅下载比</strong></td><td>" + obj.ResultOBJ.PINGJYYTXZB.toFixed(2) + "%</td>"
                            + "</tr>";

                        // ------总体情况图表------
                        totalDownOption.series[0].data.push({ name: '广告点击人数', value: obj.ResultOBJ.ZONGRS - obj.ResultOBJ.XIAZRS });
                        totalDownOption.series[0].data.push({ name: '下载点击人数', value: obj.ResultOBJ.XIAZRS });
                        totalDiffOSOption.series[0].data.push({ name: 'IOS', value: ios });
                        totalDiffOSOption.series[0].data.push({ name: '非IOS', value: fios });

                        myChart["divTotalDownBody"].setOption(totalDownOption, true);
                        myChart["divTotalDownBody"].hideLoading();
                        myChart["divTotalDiffOSBody"].setOption(totalDiffOSOption, true);
                        myChart["divTotalDiffOSBody"].hideLoading();
                        $('#totalTbody').html(str);
                    }
                    else {
                        alert(obj.ResultMsg);
                    }
                }
            });
        }

        function GetUserBehaviorTableAndChart() {
            //var param = '{"StartTime":"2014-07-01","EndTime":"2014-07-31"}';
            myChart["divUserBehaviorBody"].showLoading(effectOption);
            $('#userBehaviorTable').empty();

            $.ajax({
                type: 'post',
                url: 'AjaxComm.aspx',
                data: 'type=StatisticalMonth/UserBehavior&token=' + token + '&param=' + param,
                dataType: 'json',
                error: function (msg) {

                },
                success: function (obj) {
                    if (obj.ResultCode == "0") {
                        $('#userBehaviorTable').append("<thead><tr><th>手机终端</th><th>数量</th><th>比例</th><th>平均数量（每日）</th><th>不同SSID</th><th>点击次数</th><th>平均点击次数（每日）</th></tr></thead>");
                        var item;
                        var str = "";
                        // ------统计图------
                        userBehaviorOption.xAxis[0].data = [];
                        userBehaviorOption.legend.data = [];
                        userBehaviorOption.series = [];
                        var pie = {
                            name: '用户手机终端分布', type: 'pie',
                            tooltip: {
                                trigger: 'item',
                                formatter: '{a} <br/>{b} : {c}人 ({d}%)'
                            },
                            center: ['70%', '30%'],
                            radius: [0, '30%'],
                            startAngle: 30,
                            minAngle: 10,
                            itemStyle: {
                                normal: { labelLine: { length: 20} }
                            },
                            data: []
                        }
                        var bars = {};
                        var lines = {
                            name: '终端数量',
                            type: 'line',
                            yAxisIndex: 1,
                            data: []
                        };
                        var ssidLength = 0;

                        // ------用户行为分析Table------
                        for (var i = 0; i < obj.ResultOBJ.length; i++) {
                            item = obj.ResultOBJ[i];
                            ssidLength = item.SSID.length;
                            str += "<tbody><tr><td rowspan='" + ssidLength + "'>" + item.SHOUJZD + "</td><td rowspan='" + ssidLength + "'>" + item.SHUL + "</td><td rowspan='" + ssidLength + "'>" + item.BIL.toFixed(2) + "%</td><td rowspan='" + ssidLength + "'>" + (item.SHUL / GetCurrMonthDay()).toFixed(2) + "</td>";
                            if (item.SSID.length > 0) {
                                str += "<td>" + item.SSID[0].NAME + "</td>"
                                    + "<td>" + item.SSID[0].NUM + "</td>"
                                    + "<td>" + (item.SSID[0].NUM / GetCurrMonthDay()).toFixed(2) + "</td></tr>";

                                for (var j = 1; j < ssidLength; j++) {
                                    str += "<tr><td>" + item.SSID[j].NAME + "</td><td>" + item.SSID[j].NUM + "</td><td>" + (item.SSID[j].NUM / GetCurrMonthDay()).toFixed(2) + "</td></tr>";
                                }

                            } else {
                                str += "</tr>";
                            }

                            // 填充Option
                            for (var j = 0; j < ssidLength; j++) {
                                if (bars[item.SSID[j].NAME] == undefined) {
                                    bars[item.SSID[j].NAME] = { name: item.SSID[j].NAME, type: "bar", data: [item.SSID[j].NUM] };
                                } else {
                                    bars[item.SSID[j].NAME].data.push(item.SSID[j].NUM);
                                }
                            }
                            userBehaviorOption.xAxis[0].data.push(item.SHOUJZD);
                            userBehaviorOption.legend.data.push(item.SHOUJZD);
                            pie.data.push({ name: item.SHOUJZD, value: item.SHUL });

                            lines.data.push(item.SHUL);
                            str += "</tbody>";
                        }
                        $('#userBehaviorTable').append(str);

                        for (var j = 0; j < obj.ResultOBJ[0].SSID.length; j++) {
                            userBehaviorOption.legend.data.push(item.SSID[j].NAME);
                        }
                        userBehaviorOption.legend.data.push("终端数量");
                        userBehaviorOption.series.push(pie);
                        for (bar in bars) {
                            userBehaviorOption.series.push(bars[bar]);
                        }
                        userBehaviorOption.series.push(lines);
                        myChart["divUserBehaviorBody"].setOption(userBehaviorOption, true);
                        myChart["divUserBehaviorBody"].hideLoading();
                    }
                    else {
                        alert(obj.ResultMsg);
                    }
                }
            });
        }

        function GetAppDownloadTableAndChart() {
            myChart["divAppDownYingYeTingBody"].showLoading(effectOption);
            myChart["divAppDownDiffTimeBody"].showLoading(effectOption);
            myChart["divAppDownDiffOSBody"].showLoading(effectOption);

            //var param = '{"StartTime":"2014-07-01","EndTime":"2014-07-31"}';

            $('#appDownloadTable').empty();
            $('#appDownloadTable').append("<thead><tr><th>类型</th><th>名称</th><th>下载量（人次）</th><th>下载比例</th><th>平均下载量（每日）</th><th>总下载量（人次）</th></tr></thead>");
            $.ajax({
                type: 'post',
                url: 'AjaxComm.aspx',
                data: 'type=StatisticalMonth/AppDownload&token=' + token + '&param=' + param,
                dataType: 'json',
                error: function (msg) {

                },
                success: function (obj) {
                    if (obj.ResultCode == "0") {
                        var item;
                        var str = "";

                        var length = 0;
                        for (var i = 0; i < obj.ResultOBJ.length; i++) {
                            // ------App下载Table
                            item = obj.ResultOBJ[i];
                            item_length = item.VALUE.length;
                            str += "<tbody>";
                            if (item_length > 0) {
                                str += "<tr><td rowspan='" + item_length + "'>" + item.LEIX + "</td><td>" + item.VALUE[0].NAME + "</td><td>" + item.VALUE[0].NUM[0] + "</td><td>" + (item.VALUE[0].NUM[0] * 100 / item.VALUE[0].NUM[1]).toFixed(2) + "%</td><td>" + (item.VALUE[0].NUM[0] / GetCurrMonthDay()).toFixed(2) + "</td><td rowspan='" + item_length + "'>" + item.VALUE[0].NUM[1] + "</td></tr>";
                                for (var j = 1; j < item_length; j++) {
                                    str += "<tr><td>" + item.VALUE[j].NAME + "</td><td>" + item.VALUE[j].NUM[0] + "</td><td>" + (item.VALUE[j].NUM[0] * 100 / item.VALUE[j].NUM[1]).toFixed(2) + "%</td><td>" + (item.VALUE[j].NUM[0] / GetCurrMonthDay()).toFixed(2) + "</td></tr>";
                                }
                            }
                            str += "</tbody>";

                            // ------ App下载三个饼图生成
                            switch (item.LEIX) {
                                case "不同营业厅":
                                    appDownYingYeTingOption.legend.data = [];
                                    appDownYingYeTingOption.series[0].data = [];
                                    for (var j = 0; j < item_length; j++) {
                                        appDownYingYeTingOption.legend.data.push(item.VALUE[j].NAME);
                                        appDownYingYeTingOption.series[0].data.push({ name: item.VALUE[j].NAME, value: item.VALUE[j].NUM[0] });
                                    }
                                    break;
                                case "不同终端":
                                    appDownDiffOSOption.legend.data = [];
                                    appDownDiffOSOption.series[0].data = [];
                                    var itemStyle = { normal: { labelLine: { length: 5}} };

                                    for (var j = 0; j < item_length; j++) {
                                        appDownDiffOSOption.legend.data.push(item.VALUE[j].NAME);
                                        appDownDiffOSOption.series[0].data.push({ name: item.VALUE[j].NAME, value: item.VALUE[j].NUM[0], itemStyle: itemStyle });
                                    }
                                    break;
                                case "不同时间段":
                                    var rStep = 25;
                                    item.VALUE.sort(function (a, b) { return b.NUM[0] - a.NUM[0]; });
                                    appDownDiffTimeOption.legend.data = [];
                                    appDownDiffTimeOption.series = [];
                                    for (var j = 0; j < item_length; j++) {

                                        appDownDiffTimeOption.legend.data.push((item.VALUE[j].NUM[0] * 100.0 / item.VALUE[j].NUM[1]).toFixed(1) + '%在' + item.VALUE[j].NAME);
                                        appDownDiffTimeOption.series.push({
                                            name: item.VALUE[j].NAME,
                                            type: 'pie',
                                            clockWise: false,
                                            radius: [120 - j * rStep, 120 - rStep * (j + 1)],
                                            itemStyle: diffTimeDataStyle,
                                            data: [
                                                {
                                                    name: (item.VALUE[j].NUM[0] * 100.0 / item.VALUE[j].NUM[1]).toFixed(1) + '%在' + item.VALUE[j].NAME,
                                                    value: item.VALUE[j].NUM[0]
                                                },
                                                {
                                                    name: 'invisible',
                                                    value: item.VALUE[j].NUM[1] - item.VALUE[j].NUM[0],
                                                    itemStyle: diffTimePlaceHolderStyle
                                                }
                                            ]
                                        });
                                    }
                                    break;
                            }
                        }
                        $('#appDownloadTable').append(str);

                        myChart["divAppDownYingYeTingBody"].setOption(appDownYingYeTingOption, true);
                        myChart["divAppDownYingYeTingBody"].hideLoading();
                        myChart["divAppDownDiffTimeBody"].setOption(appDownDiffTimeOption, true);
                        myChart["divAppDownDiffTimeBody"].hideLoading();
                        myChart["divAppDownDiffOSBody"].setOption(appDownDiffOSOption, true);
                        myChart["divAppDownDiffOSBody"].hideLoading();
                    }
                    else {
                        alert(obj.ResultMsg);
                    }
                }
            });
        }

        function GetPromotionTableAndChart() {
            myChart["divPromotionYingYeTingBody"].showLoading(effectOption);
            myChart["divPromotionDiffTimeBody"].showLoading(effectOption);
            myChart["divPromotionDiffOSBody"].showLoading(effectOption);
            myChart["divPromotionDiffADBody"].showLoading(effectOption);

           // var param = '{"StartTime":"2014-07-01","EndTime":"2014-07-31"}';

            $('#promotionTable').empty();
            $('#promotionTable').append("<thead><tr><th>类型</th><th>名称</th><th>广告访问量（人次）</th><th>访问比例</th><th>平均广告访问量（每次）</th><th>总广告访问量（人次）</th></tr></thead>");
            $.ajax({
                type: 'post',
                url: 'AjaxComm.aspx',
                data: 'type=StatisticalMonth/Promotion&token=' + token + '&param=' + param,
                dataType: 'json',
                error: function (msg) {

                },
                success: function (obj) {
                    if (obj.ResultCode == "0") {
                        var item;
                        var str = "";

                        var length = 0;
                        for (var i = 0; i < obj.ResultOBJ.length; i++) {
                            // ------广告推广分析Table
                            item = obj.ResultOBJ[i];
                            item_length = item.VALUE.length;
                            str += "<tbody>";
                            if (item_length > 0) {
                                str += "<tr><td rowspan='" + item_length + "'>" + item.LEIX + "</td><td>" + item.VALUE[0].NAME + "</td><td>" + item.VALUE[0].NUM[0] + "</td><td>" + (item.VALUE[0].NUM[0] * 100 / item.VALUE[0].NUM[1]).toFixed(2) + "%</td><td>" + (item.VALUE[0].NUM[0] / GetCurrMonthDay()).toFixed(2) + "</td><td rowspan='" + item_length + "'>" + item.VALUE[0].NUM[1] + "</td></tr>";
                                for (var j = 1; j < item_length; j++) {
                                    str += "<tr><td>" + item.VALUE[j].NAME + "</td><td>" + item.VALUE[j].NUM[0] + "</td><td>" + (item.VALUE[j].NUM[0] * 100 / item.VALUE[j].NUM[1]).toFixed(2) + "%</td><td>" + (item.VALUE[j].NUM[0] / GetCurrMonthDay()).toFixed(2) + "</td></tr>";
                                }
                            }
                            str += "</tbody>";

                            // ------ 广告推广分析 四个饼图生成
                            switch (item.LEIX) {
                                case "不同营业厅":
                                    promotionYingYeTingOption.legend.data = [];
                                    promotionYingYeTingOption.series[0].data = [];
                                    for (var j = 0; j < item_length; j++) {
                                        promotionYingYeTingOption.legend.data.push(item.VALUE[j].NAME);
                                        promotionYingYeTingOption.series[0].data.push({ name: item.VALUE[j].NAME, value: item.VALUE[j].NUM[0] });
                                    }
                                    break;
                                case "不同终端":
                                    promotionDiffOSOption.legend.data = [];
                                    promotionDiffOSOption.series[0].data = [];
                                    var itemStyle = { normal: { labelLine: { length: 0}} };

                                    for (var j = 0; j < item_length; j++) {
                                        promotionDiffOSOption.legend.data.push(item.VALUE[j].NAME);
                                        promotionDiffOSOption.series[0].data.push({ name: item.VALUE[j].NAME, value: item.VALUE[j].NUM[0], itemStyle: itemStyle });
                                    }
                                    break;
                                case "不同时间段":
                                    var rStep = 30;
                                    item.VALUE.sort(function (a, b) { return b.NUM[0] - a.NUM[0]; });
                                    promotionDiffTimeOption.legend.data = [];
                                    promotionDiffTimeOption.series = [];
                                    for (var j = 0; j < item_length; j++) {

                                        promotionDiffTimeOption.legend.data.push((item.VALUE[j].NUM[0] * 100.0 / item.VALUE[j].NUM[1]).toFixed(1) + '%在' + item.VALUE[j].NAME);
                                        promotionDiffTimeOption.series.push({
                                            name: item.VALUE[j].NAME,
                                            type: 'pie',
                                            clockWise: false,
                                            radius: [130 - j * rStep, 130 - rStep * (j + 1)],
                                            itemStyle: diffTimeDataStyle,
                                            data: [
                                                {
                                                    name: (item.VALUE[j].NUM[0] * 100.0 / item.VALUE[j].NUM[1]).toFixed(1) + '%在' + item.VALUE[j].NAME,
                                                    value: item.VALUE[j].NUM[0]
                                                },
                                                {
                                                    name: 'invisible',
                                                    value: item.VALUE[j].NUM[1] - item.VALUE[j].NUM[0],
                                                    itemStyle: diffTimePlaceHolderStyle
                                                }
                                            ]
                                        });
                                    }
                                    break;
                                case "不同广告":
                                    promotionDiffADOption.legend.data = [];
                                    promotionDiffADOption.series[0].data = [];
                                    var itemStyle = {
                                        normal: {
                                            label: { show: true },
                                            labelLine: {
                                                show: true,
                                                length: 10
                                            }
                                        },
                                        emphasis: {
                                            label: { show: true },
                                            labelLine: {
                                                show: true,
                                                length: 10
                                            }
                                        }
                                    };

                                    var sum = 0;
                                    for (var j = 0; j < item_length; j++)
                                        sum += item.VALUE[j].NUM[0];
                                    for (var j = 0; j < item_length; j++) {
                                        promotionDiffADOption.legend.data.push(item.VALUE[j].NAME);
                                        if (item.VALUE[j].NUM[0] * 100.0 / sum < 10.0) {
                                            itemStyle.normal.label.show = false;
                                            itemStyle.normal.labelLine.show = false;

                                        }
                                        else {
                                            itemStyle.normal.label.show = true;
                                            itemStyle.normal.labelLine.show = true;
                                        }
                                        promotionDiffADOption.series[0].data.push({ name: item.VALUE[j].NAME, value: item.VALUE[j].NUM[0], itemStyle: $.extend(true, {}, itemStyle) });
                                    }
                                    break;
                            }
                        }
                        $('#promotionTable').append(str);

                        myChart["divPromotionYingYeTingBody"].setOption(promotionYingYeTingOption, true);
                        myChart["divPromotionYingYeTingBody"].hideLoading();
                        myChart["divPromotionDiffTimeBody"].setOption(promotionDiffTimeOption, true);
                        myChart["divPromotionDiffTimeBody"].hideLoading();
                        myChart["divPromotionDiffOSBody"].setOption(promotionDiffOSOption, true);
                        myChart["divPromotionDiffOSBody"].hideLoading();
                        myChart["divPromotionDiffADBody"].setOption(promotionDiffADOption, true);
                        myChart["divPromotionDiffADBody"].hideLoading();
                    }
                    else {
                        alert(obj.ResultMsg);
                    }
                }
            });
        }

        function GetYingYeTingTableAndChart() {
            myChart["divYingYeTingConnNumBody"].showLoading(effectOption);
            myChart["divYingYeTingConnNumSumBody"].showLoading(effectOption);
            myChart["divYingYeTingDiffOSBody"].showLoading(effectOption);
            myChart["divYingYeTingDiffOSSumBody"].showLoading(effectOption);
            //var param = '{"StartTime":"2014-07-01","EndTime":"2014-07-31"}';
            //
            $('#yingYeTingTable tbody').empty();
            $('#yingYeTingTable thead').empty();

            $.ajax({
                type: 'post',
                url: 'AjaxComm.aspx',
                data: 'type=StatisticalMonth/YingYeTing&token=' + token + '&param=' + param,
                dataType: 'json',
                error: function (msg) {

                },
                success: function (obj) {
                    if (obj.ResultCode == "0") {
                        var item;
                        var str = "";
                        var seriesItem = {
                            name: '',
                            barCategoryGap: '50%',
                            type: 'bar',
                            stack: '',
                            data: []
                        };


                        yingYeTingConnNumOption.xAxis[0].data = [];
                        yingYeTingConnNumOption.series[0].data = [];
                        yingYeTingConnNumOption.series[1].data = [];
                        yingYeTingConnNumSumOption.series[0].data = [];
                        yingYeTingDiffOSOption.xAxis[0].data = [];
                        yingYeTingDiffOSOption.legend.data = [];
                        yingYeTingDiffOSOption.series = [];
                        yingYeTingDiffOSSumOption.series[0].data = [];

                        var length = 0;
                        for (var i = 0; i < obj.ResultOBJ.length; i++) {
                            // ------营业厅分析Table
                            item = obj.ResultOBJ[i];

                            if (item.MINGC == null)
                                continue;

                            if (i == 0) {
                                $('#yingYeTingTable thead').append("<tr><th style='vertical-align:middle' rowspan='2'>营业厅</th><th style='vertical-align:middle' rowspan='2'>人次</th><th style='vertical-align:middle' rowspan='2'>人数</th><th style='vertical-align:middle' rowspan='2'>平均访问次数</th><th style='vertical-align:middle' rowspan='2'>平均访问时长</th><th style='vertical-align:middle' rowspan='2'>下载次数</th><th style='vertical-align:middle' rowspan='2'>广告点击数</th><th style='vertical-align:middle' rowspan='2'>连接人群下载比例</th><th  style='text-align:center' colspan='" + item.BUTCZXTXZB.length + "'>不同操作系统下载比例</th></tr>");
                                var tmpThead = "<tr>";
                                for (var j = 0; j < item.BUTCZXTXZB.length; j++) {
                                    tmpThead += "<th style='font-size: 13px;font-weight: normal;'>" + item.BUTCZXTXZB[j].NAME + "</th>";

                                    seriesItem.name = item.BUTCZXTXZB[j].NAME;
                                    seriesItem.stack = "非IOS";
                                    if (item.BUTCZXTXZB[j].NAME == "iPhone" || item.BUTCZXTXZB[j].NAME == "iPad")
                                        seriesItem.stack = "IOS";
                                    yingYeTingDiffOSOption.series.push($.extend(true, {}, seriesItem));
                                    yingYeTingDiffOSOption.legend.data.push(item.BUTCZXTXZB[j].NAME);
                                }
                                tmpThead += "</tr>";
                                $('#yingYeTingTable thead').append(tmpThead);
                            }

                            var tmpTbody = "<tr><td>" + item.MINGC + "</td><td>" + item.RENC + "</td><td>" + item.RENS + "</td><td>"
                                 + item.PINGJFWCS.toFixed(2) + "</td><td>" + item.PINGJFWSC.toFixed(2) + "</td><td>"
                                 + item.XIAZCS + "</td><td>" + item.GUANGGDJCS + "</td><td>" + (item.XIAZRS * 100.0 / item.RENC).toFixed(2) + "%</td>";
                            var ios = 0;
                            var fios = 0;
                            var sum = 0;
                            for (var j = 0; j < item.BUTCZXTXZB.length; j++)
                                sum += item.BUTCZXTXZB[j].NUM;
                            for (var j = 0; j < item.BUTCZXTXZB.length; j++) {
                                tmpTbody += "<td>" + (item.BUTCZXTXZB[j].NUM * 100.0 / sum).toFixed(2) + "%</td>";
                                if (item.BUTCZXTXZB[j].NAME == "iPhone" || item.BUTCZXTXZB[j].NAME == "iPad")
                                    ios += item.BUTCZXTXZB[j].NUM;
                                else
                                    fios += item.BUTCZXTXZB[j].NUM;
                                yingYeTingDiffOSOption.series[j].data.push(item.BUTCZXTXZB[j].NUM);
                            }
                            tmpTbody += "</tr>";
                            $('#yingYeTingTable tbody').append(tmpTbody);

                            // ------营业厅分析图表
                            if (item.MINGC != "合计") {
                                yingYeTingConnNumOption.xAxis[0].data.push(item.MINGC);
                                yingYeTingConnNumOption.series[0].data.push(item.XIAZRS); //下载
                                yingYeTingConnNumOption.series[1].data.push(item.RENC - item.XIAZRS); //非下载

                                yingYeTingDiffOSOption.xAxis[0].data.push(item.MINGC);
                                yingYeTingDiffOSOption.series[0].data.push(ios); //IOS
                                yingYeTingDiffOSOption.series[1].data.push(fios); //非IOS
                            }
                            else {
                                yingYeTingConnNumSumOption.series[0].data.push({ value: item.XIAZRS, name: '下载' });
                                yingYeTingConnNumSumOption.series[0].data.push({ value: item.RENC - item.XIAZRS, name: '非下载' });
                                yingYeTingDiffOSSumOption.series[0].data.push({ value: ios, name: 'IOS' });
                                yingYeTingDiffOSSumOption.series[0].data.push({ value: fios, name: '非IOS' });
                            }
                        }
                        $('#promotionTable').append(str);

                        myChart["divYingYeTingConnNumBody"].setOption(yingYeTingConnNumOption, true);
                        myChart["divYingYeTingConnNumBody"].hideLoading();
                        myChart["divYingYeTingConnNumSumBody"].setOption(yingYeTingConnNumSumOption, true);
                        myChart["divYingYeTingConnNumSumBody"].hideLoading();
                        myChart["divYingYeTingDiffOSBody"].setOption(yingYeTingDiffOSOption, true);
                        myChart["divYingYeTingDiffOSBody"].hideLoading();
                        myChart["divYingYeTingDiffOSSumBody"].setOption(yingYeTingDiffOSSumOption, true);
                        myChart["divYingYeTingDiffOSSumBody"].hideLoading();
                    }
                    else {
                        alert(obj.ResultMsg);
                    }
                }
            });
        }

        function GetAnQuanTableAndChart() {
            myChart["divAnQuanBody"].showLoading(effectOption);
            myChart["divAnQuanSumBody"].showLoading(effectOption);

            $('#anQuanTable').empty();
            $('#anQuanTable').append("<thead><tr><th>营业厅</th><th>类型</th><th>数量</th><th>比例</th><th>平均数量（每日）</th><th>总数</th></tr></thead>");
            $.ajax({
                type: 'post',
                url: 'AjaxComm.aspx',
                data: 'type=StatisticalMonth/AnQuan&token=' + token + '&param=' + param,
                dataType: 'json',
                error: function (msg) {

                },
                success: function (obj) {
                    if (obj.ResultCode == "0") {
                        var KEY = 0, XINR = 0, ZHONGW = 0, TONGY = 0, XINZ = 0, ZONGS = 0;
                        var seriesItem = {
                            name: '',
                            barCategoryGap: '50%',
                            type: 'bar',
                            stack: '安全分析',
                            data: []
                        };

                        anQuanOption.xAxis[0].data = [];
                        anQuanOption.series = [];
                        anQuanSumOption.series[0].data = [];

                        // ------安全分析Table
                        var str = "<tbody>";
                        for (var i = 0; i < obj.ResultOBJ.length; i++) {
                            str += "<tr><td rowspan='5'>" + obj.ResultOBJ[i].ALIAS + "</td><td>可疑</td><td>" + obj.ResultOBJ[i].KEY + "</td><td>" + (obj.ResultOBJ[i].KEY * 100.0 / obj.ResultOBJ[i].ZONGS).toFixed(2) + "%</td><td>" + (obj.ResultOBJ[i].KEY / GetCurrMonthDay()).toFixed(2) + "</td><td rowspan='5'>" + obj.ResultOBJ[i].ZONGS + "</td></tr>";
                            str += "<tr><td>信任</td><td>" + obj.ResultOBJ[i].XINR + "</td><td>" + (obj.ResultOBJ[i].XINR * 100.0 / obj.ResultOBJ[i].ZONGS).toFixed(2) + "%</td><td>" + (obj.ResultOBJ[i].XINR / GetCurrMonthDay()).toFixed(2) + "</td></tr>";
                            str += "<tr><td>中文</td><td>" + obj.ResultOBJ[i].ZHONGW + "</td><td>" + (obj.ResultOBJ[i].ZHONGW * 100.0 / obj.ResultOBJ[i].ZONGS).toFixed(2) + "%</td><td>" + (obj.ResultOBJ[i].ZHONGW / GetCurrMonthDay()).toFixed(2) + "</td></tr>";
                            str += "<tr><td>同业</td><td>" + obj.ResultOBJ[i].TONGY + "</td><td>" + (obj.ResultOBJ[i].TONGY * 100.0 / obj.ResultOBJ[i].ZONGS).toFixed(2) + "%</td><td>" + (obj.ResultOBJ[i].TONGY / GetCurrMonthDay()).toFixed(2) + "</td></tr>";
                            str += "<tr><td>新增</td><td>" + obj.ResultOBJ[i].XINZ + "</td><td>" + (obj.ResultOBJ[i].XINZ * 100.0 / obj.ResultOBJ[i].ZONGS).toFixed(2) + "%</td><td>" + (obj.ResultOBJ[i].XINZ / GetCurrMonthDay()).toFixed(2) + "</td></tr>";

                            KEY += obj.ResultOBJ[i].KEY;
                            XINR += obj.ResultOBJ[i].XINR;
                            ZHONGW += obj.ResultOBJ[i].ZHONGW;
                            TONGY += obj.ResultOBJ[i].TONGY;
                            XINZ += obj.ResultOBJ[i].XINZ;
                        }
                        ZONGS = KEY + XINR + ZHONGW + TONGY + XINZ;
                        str += "<tr><td rowspan='5'>合计</td><td>可疑</td><td>" + KEY + "</td><td>" + (KEY * 100.0 / ZONGS).toFixed(2) + "%</td><td>" + (KEY / GetCurrMonthDay()).toFixed(2) + "</td><td rowspan='5'>" + ZONGS + "</td></tr>";
                        str += "<tr><td>信任</td><td>" + XINR + "</td><td>" + (XINR * 100.0 / ZONGS).toFixed(2) + "%</td><td>" + (XINR / GetCurrMonthDay()).toFixed(2) + "</td></tr>";
                        str += "<tr><td>中文</td><td>" + ZHONGW + "</td><td>" + (ZHONGW * 100.0 / ZONGS).toFixed(2) + "%</td><td>" + (ZHONGW / GetCurrMonthDay()).toFixed(2) + "</td></tr>";
                        str += "<tr><td>同业</td><td>" + TONGY + "</td><td>" + (TONGY * 100.0 / ZONGS).toFixed(2) + "%</td><td>" + (TONGY / GetCurrMonthDay()).toFixed(2) + "</td></tr>";
                        str += "<tr><td>新增</td><td>" + XINZ + "</td><td>" + (XINZ * 100.0 / ZONGS).toFixed(2) + "%</td><td>" + (XINZ / GetCurrMonthDay()).toFixed(2) + "</td></tr>";
                        str += "</tbody>";
                        // ------安全分析图表
                        seriesItem.name = '可疑';
                        anQuanOption.series.push($.extend(true, {}, seriesItem));
                        seriesItem.name = '信任';
                        anQuanOption.series.push($.extend(true, {}, seriesItem));
                        seriesItem.name = '中文';
                        anQuanOption.series.push($.extend(true, {}, seriesItem));
                        seriesItem.name = '同业';
                        anQuanOption.series.push($.extend(true, {}, seriesItem));
                        seriesItem.name = '新增';
                        anQuanOption.series.push($.extend(true, {}, seriesItem));

                        for (var i = 0; i < obj.ResultOBJ.length; i++) {
                            anQuanOption.xAxis[0].data.push(obj.ResultOBJ[i].ALIAS);
                            anQuanOption.series[0].data.push(obj.ResultOBJ[i].KEY);
                            anQuanOption.series[1].data.push(obj.ResultOBJ[i].XINR);
                            anQuanOption.series[2].data.push(obj.ResultOBJ[i].ZHONGW);
                            anQuanOption.series[3].data.push(obj.ResultOBJ[i].TONGY);
                            anQuanOption.series[4].data.push(obj.ResultOBJ[i].XINZ);
                        }
                        anQuanSumOption.series[0].data.push({ value: KEY, name: '可疑' });
                        anQuanSumOption.series[0].data.push({ value: XINR, name: '信任' });
                        anQuanSumOption.series[0].data.push({ value: ZHONGW, name: '中文' });
                        anQuanSumOption.series[0].data.push({ value: TONGY, name: '同业' });
                        anQuanSumOption.series[0].data.push({ value: XINZ, name: '新增' });
                        $('#anQuanTable').append(str);

                        myChart["divAnQuanBody"].setOption(anQuanOption, true);
                        myChart["divAnQuanBody"].hideLoading();
                        myChart["divAnQuanSumBody"].setOption(anQuanSumOption, true);
                        myChart["divAnQuanSumBody"].hideLoading();
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
    <uc1:header ID="header1" runat="server" />

    <div style="margin-right: 15px; margin-left: 15px;">
        <div class="row">
            <div class="div-body col-md-12">
                <div class="bodyMain" style="height: 36px;">
                    <div style="float:right; margin-right:20px;">
                        <input id="startDate" type="text" class="Wdate" onFocus="WdatePicker({dateFmt:'yyyy-MM',maxDate: today.format('yyyy-MM')})"/>
                        <button id="btnRefresh" type="button" class="btn btn-default btn-sm">刷新</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div style="margin-right: 11px; margin-left: 11px;">
        <div id="tabs">
            <ul>
                <%--<li><a href="#tabs-1">总体情况</a></li>
                <li><a href="#tabs-2">APP下载分析</a></li>
                <li><a href="#tabs-3">业务推广分析</a></li>
                <li><a href="#tabs-4">营业厅分析</a></li>
                <li><a href="#tabs-5">用户行为与构成分析</a></li>
                <li><a href="#tabs-6">安全分析</a></li>--%>
            </ul>
            <div id="tabs-1" class="row" style="display:none">
                <div class="div-body col-md-12">
                    <div class="panel panel-default">
                        <%--<div class="panel-heading lead"><strong>总体情况</strong></div>--%>
                        <div class="panel-body">
                            <div class="row">
                                <div class="div-body col-md-6">
                                    <div class="bodyMain">
                                        <div id="divTotalDownBody" md="main" style="height:380px;"></div>
                                        <span md="wrong-message" style="color: red"></span>
                                        <script type="text/javascript">
                                            var totalDownOption = {
                                                title: {
                                                    text: '连接人群下载比',
                                                    x: 'center',
                                                    textStyle: {
                                                        color: 'rgba(30,144,255,0.8)',
                                                        fontFamily: '微软雅黑',
                                                        fontSize: 25,
                                                        fontWeight: 'bolder'
                                                    }
                                                },
                                                tooltip: {
                                                    trigger: 'item',
                                                    formatter: "{a} <br/>{b} : {c} ({d}%)"
                                                },
                                                legend: {
                                                    y: 340,
                                                    data: ['广告点击人数', '下载点击人数']
                                                },
                                                series: [
                                                    {
                                                        name: '下载人数分布',
                                                        type: 'pie',
                                                        radius: '60%',
                                                        center: ['50%', '50%'],
                                                        data: []
                                                    }
                                                ]
                                            };
                                        </script>
                                    </div>
                                </div>
                                <div class="div-body col-md-6">
                                    <div class="bodyMain">
                                        <div id="divTotalDiffOSBody" md="main" style="height:380px;"></div>
                                        <span md="wrong-message" style="color: red"></span>
                                        <script type="text/javascript">
                                            var totalDiffOSOption = {
                                                title: {
                                                    text: '不同操作系统下载比',
                                                    x: 'center',
                                                    textStyle: {
                                                        color: 'rgba(30,144,255,0.8)',
                                                        fontFamily: '微软雅黑',
                                                        fontSize: 25,
                                                        fontWeight: 'bolder'
                                                    }
                                                },
                                                tooltip: {
                                                    trigger: 'item',
                                                    formatter: "{a} <br/>{b} : {c} ({d}%)"
                                                },
                                                legend: {
                                                    y: 340,
                                                    data: ['IOS', '非IOS']
                                                },
                                                series: [
                                                    {
                                                        name: '不同操作系统分布',
                                                        type: 'pie',
                                                        radius: '60%',
                                                        center: ['50%', '50%'],
                                                        data: []
                                                    }
                                                ]
                                            };
                                        </script>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <table class="table">
                            <tbody id="totalTbody">
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            <div class="clearfix"></div>
            <div id="tabs-2" class="row" style="display:none">
                <div class="div-body col-md-12">
                    <div class="panel panel-default">
                        <%--<div class="panel-heading lead"><strong>APP下载分析</strong></div>--%>
                        <div class="panel-body">
                            <div class="row">
                                <div class="div-body col-md-4">
                                    <div class="bodyMain">
                                        <!--App下载分析 -- 营业厅饼图-->
                                        <div id="divAppDownYingYeTingBody" md="main" style="height:380px;"></div>
                                        <span md="wrong-message" style="color: red"></span>
                                        <script type="text/javascript">
                                            var appDownYingYeTingOption = {
                                                title: {
                                                    text: '不同营业厅',
                                                    x: 'center',
                                                    textStyle: {
                                                        color: 'rgba(30,144,255,0.8)',
                                                        fontFamily: '微软雅黑',
                                                        fontSize: 25,
                                                        fontWeight: 'bolder'
                                                    }
                                                },
                                                tooltip: {
                                                    trigger: 'item',
                                                    formatter: "{a} <br/>{b} : {c}次 ({d}%)"
                                                },
                                                legend: {
                                                    orient: 'vertical',
                                                    x: 'left',
                                                    y: 'center',
                                                    data: []
                                                },
                                                calculable: true,
                                                series: [
                                                    {
                                                        name: '下载量',
                                                        type: 'pie',
                                                        radius: ['40%', '60%'],
                                                        center: ['65%', '55%'],
                                                        itemStyle : {
                                                            normal : {label : {show : false }, labelLine : {show : false}},
                                                            emphasis : {
                                                                label : {
                                                                    show : true,
                                                                    position : 'center',
                                                                    textStyle : {fontSize : '12',fontWeight : 'bold'}
                                                                }
                                                            }
                                                        },
                                                        data: []
                                                    }
                                                ]
                                            };
                                        </script>
                                    </div>
                                </div>
                                <div class="div-body col-md-4">
                                    <div class="bodyMain">
                                        <!--App下载分析 -- 不同时段饼图-->
                                        <div id="divAppDownDiffTimeBody" md="main" style="height:380px;"></div>
                                        <span md="wrong-message" style="color: red"></span>
                                        <script type="text/javascript">
                                            var diffTimeDataStyle = { normal: { label: { show: false }, labelLine: { show: false}} };
                                            var diffTimePlaceHolderStyle = {
                                                normal: {
                                                    color: 'rgba(0,0,0,0)',
                                                    label: { show: false },
                                                    labelLine: { show: false }
                                                },
                                                emphasis: {
                                                    color: 'rgba(0,0,0,0)'
                                                }
                                            };
                                            var appDownDiffTimeOption = {
                                                title: {
                                                    text: '不同时段',
                                                    x: 'center',
                                                    textStyle: {
                                                        color: 'rgba(30,144,255,0.8)',
                                                        fontFamily: '微软雅黑',
                                                        fontSize: 25,
                                                        fontWeight: 'bolder'
                                                    }
                                                },
                                                tooltip: {
                                                    show: 'item',
                                                    formatter: "{a} <br/>{b} : {c}次 ({d}%)"
                                                },
                                                legend: {
                                                    orient: 'vertical',
                                                    x: document.getElementById('divAppDownDiffTimeBody').offsetWidth / 2,
                                                    y: 75,
                                                    itemGap: 12,
                                                    data: []
                                                },
                                                series: []
                                            };
                                        </script>
                                    </div>
                                </div>
                                <div class="div-body col-md-4">
                                    <div class="bodyMain">
                                        <!--App下载分析 -- 不同终端饼图-->
                                        <div id="divAppDownDiffOSBody" md="main" style="height:380px;"></div>
                                        <span md="wrong-message" style="color: red"></span>
                                        <script type="text/javascript">
                                            var appDownDiffOSOption = {
                                                title: {
                                                    text: '不同终端',
                                                    x: 'center',
                                                    textStyle: {
                                                        color: 'rgba(30,144,255,0.8)',
                                                        fontFamily: '微软雅黑',
                                                        fontSize: 25,
                                                        fontWeight: 'bolder'
                                                    }
                                                },
                                                tooltip: {
                                                    trigger: 'item',
                                                    formatter: "{a} <br/>{b} : {c}次 ({d}%)"
                                                },
                                                legend: {
                                                    orient: 'vertical',
                                                    x: 'left',
                                                    y: 'center',
                                                    data: []
                                                },
                                                calculable: true,
                                                series: [
                                                    {
                                                        name: '下载量',
                                                        type: 'pie',
                                                        radius: ['20%', '45%'],
                                                        center: ['65%', '55%'],
                                                        roseType: 'area',
                                                        data: []
                                                    }
                                                ]
                                            };
                    
                                        </script>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <table class="table" id="appDownloadTable"></table>
                    </div>
                </div>
            </div>
            <div class="clearfix"></div>
            <div id="tabs-3" class="row" style="display:none">
                <div class="div-body col-md-12">
                    <div class="panel panel-default">
                        <%--<div class="panel-heading lead"><strong>业务推广分析</strong></div>--%>
                        <div class="panel-body">
                            <div class="row">
                                <div class="div-body col-md-6">
                                    <div class="bodyMain">
                                        <!--广告推广分析 -- 营业厅饼图-->
                                        <div id="divPromotionYingYeTingBody" md="main" style="height:380px;"></div>
                                        <span md="wrong-message" style="color: red"></span>
                                        <script type="text/javascript">
                                            var promotionYingYeTingOption = {
                                                title: {
                                                    text: '不同营业厅',
                                                    x: 'center',
                                                    textStyle: {
                                                        color: 'rgba(30,144,255,0.8)',
                                                        fontFamily: '微软雅黑',
                                                        fontSize: 25,
                                                        fontWeight: 'bolder'
                                                    }
                                                },
                                                tooltip: {
                                                    trigger: 'item',
                                                    formatter: "{a} <br/>{b} : {c}次 ({d}%)"
                                                },
                                                legend: {
                                                    orient: 'vertical',
                                                    x: 'left',
                                                    y: 'center',
                                                    data: []
                                                },
                                                calculable: true,
                                                series: [
                                                    {
                                                        name: '访问量',
                                                        type: 'pie',
                                                        radius: ['40%', '60%'],
                                                        center: ['50%', '50%'],
                                                        itemStyle: {
                                                            normal: { label: { show: false }, labelLine: { show: false} },
                                                            emphasis: {
                                                                label: {
                                                                    show: true,
                                                                    position: 'center',
                                                                    textStyle: { fontSize: '15', fontWeight: 'bold' }
                                                                }
                                                            }
                                                        },
                                                        data: []
                                                    }
                                                ]
                                            };
                                        </script>
                                    </div>
                                </div>
                                <div class="div-body col-md-6">
                                    <div class="bodyMain">
                                        <!--广告推广分析 -- 不同时段饼图-->
                                        <div id="divPromotionDiffTimeBody" md="main" style="height:380px;"></div>
                                        <span md="wrong-message" style="color: red"></span>
                                        <script type="text/javascript">
                                            var promotionDiffTimeOption = {
                                                title: {
                                                    text: '不同时段',
                                                    x: 'center',
                                                    textStyle: {
                                                        color: 'rgba(30,144,255,0.8)',
                                                        fontFamily: '微软雅黑',
                                                        fontSize: 25,
                                                        fontWeight: 'bolder'
                                                    }
                                                },
                                                tooltip: {
                                                    show: 'item',
                                                    formatter: "{a} <br/>{b} : {c}次 ({d}%)"
                                                },
                                                legend: {
                                                    orient: 'vertical',
                                                    x: document.getElementById('divPromotionDiffTimeBody').offsetWidth / 2,
                                                    y: 70,
                                                    itemGap: 17,
                                                    data: []
                                                },
                                                series: []
                                            };
                                        </script>
                                    </div>
                                </div>
                            </div>
                            <div class="row" style="margin-top:10px">
                                <div class="div-body col-md-6">
                                    <div class="bodyMain">
                                        <!--广告推广分析 -- 不同终端饼图-->
                                        <div id="divPromotionDiffOSBody" md="main" style="height:380px;"></div>
                                        <span md="wrong-message" style="color: red"></span>
                                        <script type="text/javascript">
                                            var promotionDiffOSOption = {
                                                title: {
                                                    text: '不同终端',
                                                    x: 'center',
                                                    textStyle: {
                                                        color: 'rgba(30,144,255,0.8)',
                                                        fontFamily: '微软雅黑',
                                                        fontSize: 25,
                                                        fontWeight: 'bolder'
                                                    }
                                                },
                                                tooltip: {
                                                    trigger: 'item',
                                                    formatter: "{a} <br/>{b} : {c}次 ({d}%)"
                                                },
                                                legend: {
                                                    orient: 'vertical',
                                                    x: 'left',
                                                    y: 'center',
                                                    data: []
                                                },
                                                calculable: true,
                                                series: [
                                                    {
                                                        name: '访问量',
                                                        type: 'pie',
                                                        radius: ['20%', '60%'],
                                                        center: ['50%', '55%'],
                                                        startAngle:10,
                                                        roseType: 'area',
                                                        data: []
                                                    }
                                                ]
                                            };
                    
                                        </script>
                                    </div>
                                </div>
                                <div class="div-body col-md-6">
                                    <div class="bodyMain">
                                    <!--广告推广分析 -- 不同广告饼图-->
                                        <div id="divPromotionDiffADBody" md="main" style="height:380px;"></div>
                                        <span md="wrong-message" style="color: red"></span>
                                        <script type="text/javascript">
                                            var promotionDiffADOption = {
                                                title: {
                                                    text: '不同广告',
                                                    x: 'center',
                                                    textStyle: {
                                                        color: 'rgba(30,144,255,0.8)',
                                                        fontFamily: '微软雅黑',
                                                        fontSize: 25,
                                                        fontWeight: 'bolder'
                                                    }
                                                },
                                                tooltip: {
                                                    trigger: 'item',
                                                    formatter: "{a} <br/>{b} : {c} ({d}%)"
                                                },
                                                legend: {
                                                    orient: 'vertical',
                                                    x: 'left',
                                                    y: 'center',
                                                    data: []
                                                },
                                                series: [
                                                    {
                                                        name: '广告页',
                                                        type: 'pie',
                                                        radius: '60%',
                                                        minAngle:5,
                                                        center: ['60%', '50%'],
                                                        data: []
                                                    }
                                                ]
                                            };
                                        </script>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <table class="table" id="promotionTable"></table>
                    </div>
                </div>
            </div>
            <div class="clearfix"></div>
            <div id="tabs-4" class="row" style="display:none">
                <div class="div-body col-md-12">
                    <div class="panel panel-default">
                        <%--<div class="panel-heading lead"><strong>营业厅分析</strong></div>--%>
                        <div class="panel-body">
                            <div class="row">
                                <div class="div-body col-md-8">
                                    <div class="bodyMain">
                                        <div id="divYingYeTingConnNumBody" md="main" style="height:380px;"></div>
                                        <span md="wrong-message" style="color: red"></span>
                                        <script type="text/javascript">
                                            var yingYeTingConnNumOption = {
                                                title: {
                                                    text: '连接人数下载比例',
                                                    x: 'center',
                                                    textStyle: {
                                                        color: 'rgba(30,144,255,0.8)',
                                                        fontFamily: '微软雅黑',
                                                        fontSize: 25,
                                                        fontWeight: 'bolder'
                                                    }
                                                },
                                                grid: {
                                                    y2: 80
                                                },
                                                tooltip: {
                                                    trigger: 'axis',
                                                    axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                                                        type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
                                                    },
                                                    formatter: function (param) {
                                                        return param[0][1] + '<br/>'
                                                           + param[0][0] + ' : ' + param[0][2] + '<br/>'
                                                           + param[1][0] + ' : ' + (param[1][2] + param[0][2]);
                                                    }
                                                },
                                                legend: {
                                                    x: 'center',
                                                    y: 340,
                                                    data: ['下载', '非下载']
                                                },
                                                xAxis: [
                                                    {
                                                        type: 'category',
                                                        splitLine: { show: false },
                                                        data: []
                                                    }
                                                ],
                                                yAxis: [
                                                    {
                                                        type: 'value',
                                                        boundaryGap: [0, 0.1],
                                                        splitArea: { show: true }
                                                    }
                                                ],
                                                series: [
                                                    {
                                                        name: '下载',
                                                        type: 'bar',
                                                        stack: 'sum',
                                                        barCategoryGap: '50%',
                                                        itemStyle: {
                                                            normal: {
                                                                color: 'tomato',
                                                                borderColor: 'tomato',
                                                                borderWidth: 6,
                                                                label: {
                                                                    show: true, position: 'inside'
                                                                }
                                                            }
                                                        },
                                                        data: []
                                                    },
                                                    {
                                                        name: '非下载',
                                                        type: 'bar',
                                                        stack: 'sum',
                                                        itemStyle: {
                                                            normal: {
                                                                color: '#fff',
                                                                borderColor: 'tomato',
                                                                borderWidth: 6,
                                                                label: {
                                                                    show: true,
                                                                    position: 'top',
                                                                    formatter: function (a, b, c) {
                                                                        for (var i = 0, l = yingYeTingDiffOSOption.xAxis[0].data.length; i < l; i++) {
                                                                            if (yingYeTingDiffOSOption.xAxis[0].data[i] == b) {
                                                                                return yingYeTingDiffOSOption.series[0].data[i] + c;
                                                                            }
                                                                        }
                                                                    },
                                                                    textStyle: {
                                                                        color: 'tomato'
                                                                    }
                                                                }
                                                            }
                                                        },
                                                        data: []
                                                    }
                                                ]
                                            };
                                        </script>
                                    </div>
                                </div>
                                <div class="div-body col-md-4">
                                    <div class="bodyMain">
                                        <div id="divYingYeTingConnNumSumBody" md="main" style="height:380px;"></div>
                                        <span md="wrong-message" style="color: red"></span>
                                        <script type="text/javascript">
                                            var yingYeTingConnNumSumOption = {
                                                title: {
                                                    text: '连接人数下载合计比例',
                                                    x: 'center',
                                                    textStyle: {
                                                        color: 'rgba(30,144,255,0.8)',
                                                        fontFamily: '微软雅黑',
                                                        fontSize: 25,
                                                        fontWeight: 'bolder'
                                                    }
                                                },
                                                tooltip: {
                                                    trigger: 'item',
                                                    formatter: "{a} <br/>{b} : {c} ({d}%)"
                                                },
                                                legend: {
                                                    x: 'center',
                                                    y: 340,
                                                    data: ['下载', '非下载']
                                                },
                                                series: [
                                                    {
                                                        name: '下载比',
                                                        type: 'pie',
                                                        radius: '55%',
                                                        center: ['50%', '50%'],
                                                        data: []
                                                    }
                                                ]
                                            };
                                        </script>
                                    </div>
                                </div>
                            </div>
                            <div class="row" style="margin-top: 10px;">
                                <div class="div-body col-md-8">
                                    <div class="bodyMain">
                                        <div id="divYingYeTingDiffOSBody" md="main" style="height:380px;"></div>
                                        <span md="wrong-message" style="color: red"></span>
                                        <script type="text/javascript">
                                            var yingYeTingDiffOSOption = {
                                                title: {
                                                    text: '不同操作系统下载比例',
                                                    x: 'center',
                                                    textStyle: {
                                                        color: 'rgba(30,144,255,0.8)',
                                                        fontFamily: '微软雅黑',
                                                        fontSize: 25,
                                                        fontWeight: 'bolder'
                                                    }
                                                },
                                                grid: {
                                                    y2: 80
                                                },
                                                tooltip: {
                                                    trigger: 'axis',
                                                    axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                                                        type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
                                                    },
                                                    formatter: function (param) {
                                                        var result = param[0][1] + '<br/>';
                                                        var strIOS = "";
                                                        var strFIOS = "";
                                                        for (var i = 0; i < param.length; i++) {
                                                            if (param[i][0] == "iPhone" || param[i][0] == "iPad")
                                                                strIOS += param[i][0] + ' : ' + param[i][2] + '<br/>';
                                                            else
                                                                strFIOS += param[i][0] + ' : ' + param[i][2] + '<br/>';
                                                        }
                                                        return result + "IOS<br />" + strIOS + "非IOS<br />" + strFIOS;

                                                    }
                                                },
                                                legend: {
                                                    x: 'center',
                                                    y: 340,
                                                    data: []
                                                },
                                                xAxis: [
                                                    {
                                                        type: 'category',
                                                        splitLine: { show: false },
                                                        data: []
                                                    }
                                                ],
                                                yAxis: [
                                                    {
                                                        type: 'value',
                                                        boundaryGap: [0, 0.1],
                                                        splitArea: { show: true }
                                                    }
                                                ],
                                                series: []
                                            };
                                        </script>
                                    </div>
                                </div>
                                <div class="div-body col-md-4">
                                    <div class="bodyMain">
                                        <div id="divYingYeTingDiffOSSumBody" md="main" style="height:380px;"></div>
                                        <span md="wrong-message" style="color: red"></span>
                                        <script type="text/javascript">
                                            var yingYeTingDiffOSSumOption = {
                                                title: {
                                                    text: '不同操作系统下载合计比例',
                                                    x: 'center',
                                                    textStyle: {
                                                        color: 'rgba(30,144,255,0.8)',
                                                        fontFamily: '微软雅黑',
                                                        fontSize: 25,
                                                        fontWeight: 'bolder'
                                                    }
                                                },
                                                tooltip: {
                                                    trigger: 'item',
                                                    formatter: "{a} <br/>{b} : {c} ({d}%)"
                                                },
                                                legend: {
                                                    x: 'center',
                                                    y: 340,
                                                    data: ['IOS', '非IOS']
                                                },
                                                series: [
                                                    {
                                                        name: '下载量',
                                                        type: 'pie',
                                                        radius: '55%',
                                                        center: ['50%', '50%'],
                                                        data: []
                                                    }
                                                ]
                                            };
                                        </script>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <table class="table" id="yingYeTingTable">
                            <thead></thead>
                            <tbody></tbody>
                        </table>
                    </div>
                </div>
            </div>
            <div class="clearfix"></div>
            <div id="tabs-5" class="row" style="display:none">
                <div class="div-body col-md-12">
                    <div class="panel panel-default">
                        <%--<div class="panel-heading lead"><strong>用户行为与构成分析</strong></div>--%>
                        <div class="panel-body">
                            <div class="row">
                                <div class="div-body col-md-12">
                                    <div class="bodyMain">
                                        <!-- 用户行为与构成 -->
                                        <div id="divUserBehaviorBody" md="main" style="height:380px;"></div>
                                        <span md="wrong-message" style="color: red"></span>
                                        <div class="clearfix"></div>
                                        <script type="text/javascript">
                                            var userBehaviorOption = {
                                                tooltip: {
                                                    trigger: 'axis'//,
                                                    //formatter: '{b} <br/>{a0} : {c0}点击<br/>{a1} : {c1}点击<br/>{a2} : {c2}'
                                                },
                                                grid: {
                                                    y2: 80
                                                },
                                                legend: {
                                                    x: 'center',
                                                    y: 340,
                                                    data: []
                                                },
                                                xAxis: [
                                                        {
                                                            type: 'category',
                                                            splitLine: { show: false },
                                                            data: []
                                                        }
                                                    ],
                                                yAxis: [
                                                        {
                                                            type: 'value',
                                                            position: 'left',
                                                            name: '点击次数'
                                                        },
                                                        {
                                                            type: 'value',
                                                            position: 'left',
                                                            name: '终端数量'
                                                        }
                                                        ],
                                               series: []
                                           };
                                        </script>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <table class="table" id="userBehaviorTable"></table>
                    </div>
                </div>
            </div>
            <div class="clearfix"></div>
            <div id="tabs-6" class="row" style="display:none">
                <div class="div-body col-md-12">
                    <div class="panel panel-default">
                        <%--<div class="panel-heading lead"><strong>安全分析</strong></div>--%>
                        <div class="panel-body">
                            <div class="row">
                                <div class="div-body col-md-8">
                                    <div class="bodyMain">
                                        <div id="divAnQuanBody" md="main" style="height:380px;"></div>
                                        <span md="wrong-message" style="color: red"></span>
                                        <script type="text/javascript">
                                            var anQuanOption = {
                                                title: {
                                                    text: '安全分析比例',
                                                    x: 'center',
                                                    textStyle: {
                                                        color: 'rgba(30,144,255,0.8)',
                                                        fontFamily: '微软雅黑',
                                                        fontSize: 25,
                                                        fontWeight: 'bolder'
                                                    }
                                                },
                                                grid: {
                                                    y2: 80
                                                },
                                                tooltip: {
                                                    trigger: 'axis',
                                                    axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                                                        type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
                                                    },
                                                    formatter: function (param) {
                                                        var result = param[0][1] + '<br/>';
                                                        for (var i = 0; i < param.length; i++)
                                                            result += param[i][0] + ' : ' + param[i][2] + '<br/>';
                                                        return result;
                                                    }
                                                },
                                                legend: {
                                                    x: 'center',
                                                    y: 340,
                                                    data: ['可疑', '信任', '中文', '同业', '新增']
                                                },
                                                xAxis: [
                                                    {
                                                        type: 'category',
                                                        splitLine: { show: false },
                                                        data: []
                                                    }
                                                ],
                                                yAxis: [
                                                    {
                                                        type: 'value',
                                                        boundaryGap: [0, 0.1],
                                                        splitArea: { show: true }
                                                    }
                                                ],
                                                series: []
                                            };
                                        </script>
                                    </div>
                                </div>
                                <div class="div-body col-md-4">
                                    <div class="bodyMain">
                                        <div id="divAnQuanSumBody" md="main" style="height:380px;"></div>
                                        <span md="wrong-message" style="color: red"></span>
                                        <script type="text/javascript">
                                            var anQuanSumOption = {
                                                title: {
                                                    text: '安全分析合计比例',
                                                    x: 'center',
                                                    textStyle: {
                                                        color: 'rgba(30,144,255,0.8)',
                                                        fontFamily: '微软雅黑',
                                                        fontSize: 25,
                                                        fontWeight: 'bolder'
                                                    }
                                                },
                                                tooltip: {
                                                    trigger: 'item',
                                                    formatter: "{a} <br/>{b} : {c} ({d}%)"
                                                },
                                                legend: {
                                                    x: 'center',
                                                    y: 340,
                                                    data: ['可疑', '信任', '中文', '同业', '新增']
                                                },
                                                series: [
                                                    {
                                                        name: '下载量',
                                                        type: 'pie',
                                                        radius: '55%',
                                                        center: ['50%', '50%'],
                                                        minAngle: 5,
                                                        data: []
                                                    }
                                                ]
                                            };
                                        </script>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <table class="table" id="anQuanTable"></table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="../UITemplet/echarts/js/echartsExample.js" type="text/javascript"></script>
</body>
</html>
