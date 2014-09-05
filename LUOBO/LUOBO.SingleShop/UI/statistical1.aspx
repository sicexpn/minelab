<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="statistical1.aspx.cs" Inherits="LUOBO.SingleShop.UI.statistical1" %>
<%@ Register src="header.ascx" tagname="header" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>next-wifi 统计</title>
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
            height: 380px;
            border-radius: 4px;
            margin: 0 3px 5px 3px;
            padding: 5px;
            box-shadow: 0 1px 1px rgba(0, 0, 0, 0.15);
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

        $().ready(function () {
            $("#startDate").val(new Date(new Date() - (3600000 * 24 * 6)).format("yyyy-MM-dd"));
            $("#endDate").val(new Date().format("yyyy-MM-dd"));
            HighLightMenu("统计");
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
                        }
                    }
                    else {
                        alert(obj.ResultMsg);
                    }
                }
            });

            $("#btnSubmit").click(function () {
                showLoadings();
                if ($("#startDate").val() == "") {
                    alert("请选择起始时间!");
                    return;
                }
                if ($("#endDate").val() == "") {
                    alert("请选择结束时间!");
                    return;
                }

                completeNum = 0;
                //在线人数统计------------------------------------------------------------------------
                clearInterval(timeTicket);
                $.ajax({
                    type: 'post', //可选get
                    url: 'AjaxComm.aspx', //这里是接收数据的PHP程序
                    data: 'type=StatisticalOnline&token=' + token + '&param={' + ($("#slcAP").val() == "-99" ? "" : '"ID":"' + $("#slcAP").val() + '",') + '"Type":"AP","Mode":"Date","StartTime":"' + $("#startDate").val() + '","EndTime":"' + $("#endDate").val() + '"}',
                    dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                    success: function (obj) {
                        optionOnline.xAxis[0].data = (function () {
                            var now = new Date($("#endDate").val());
                            var res = [];
                            var len = DateDiff($("#startDate").val(), $("#endDate").val()) + 1;
                            while (len--) {
                                res.unshift(now.toLocaleDateString().replace(/^\D*/, ''));
                                now = new Date(now - (3600000 * 24));
                            }
                            return res;
                        })();
                        optionOnline.series[0].data = obj[0];
                        optionOnline.series[1].data = obj[1];
                        optionOnline.xAxis[0].axisLabel.interval = 'auto';
                        optionOnline.xAxis[0].axisLabel.rotate = 0;

                        completeNum++;
                    }
                });

                //WIFI连接使用统计------------------------------------------------------------------------
                $.ajax({
                    type: 'post', //可选get
                    url: 'AjaxComm.aspx', //这里是接收数据的PHP程序
                    data: 'type=StatisticalPeople&token=' + token + '&param={' + ($("#slcAP").val() == "-99" ? "" : '"ID":"' + $("#slcAP").val() + '",') + '"Type":"AP","Mode":"Date","StartTime":"' + $("#startDate").val() + '","EndTime":"' + $("#endDate").val() + '"}',
                    dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                    success: function (obj) {
                        var val1 = 0, val2 = 0;
                        for (var i = 0; i < obj[1].length; i++)
                            val1 += obj[1][i];
                        for (var i = 0; i < obj[0].length; i++)
                            val2 += obj[0][i];
                        optionPeople.series[0].data = [
                                { value: val1, name: '未认证用户数' },
                                { value: val2, name: '认证用户数' }
                            ];
                        optionPeopleLine.xAxis[0].data = (function () {
                            var now = new Date($("#endDate").val());
                            var res = [];
                            var len = DateDiff($("#startDate").val(), $("#endDate").val()) + 1;
                            while (len--) {
                                res.unshift(now.toLocaleDateString().replace(/^\D*/, ''));
                                now = new Date(now - (3600000 * 24));
                            }
                            return res;
                        })();
                        optionPeopleLine.series[0].data = obj[2];
                        optionPeopleLine.series[1].data = obj[0];
                        completeNum += 2;
                    }
                });

                //SSID访问量统计------------------------------------------------------------------------
                $("#divSSIDTableBody tbody").html("<tr><td colspan='3'>正在加载...</td></tr>");
                $.ajax({
                    type: 'post', //可选get
                    url: 'AjaxComm.aspx', //这里是接收数据的PHP程序
                    data: 'type=StatisticalSSID&token=' + token + '&param={' + ($("#slcAP").val() == "-99" ? "" : '"ID":"' + $("#slcAP").val() + '",') + '"Type":"AP","Mode":"Date","StartTime":"' + $("#startDate").val() + '","EndTime":"' + $("#endDate").val() + '"}',
                    dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                    success: function (obj) {
                        var tbStr = "";
                        //                        optionSSID.legend.data = (function () {
                        //                            var res = [];
                        //                            for (var i = 0; i < obj.length; i++) {
                        //                                res.push(obj[i].NAME);
                        //                            }
                        //                            return res;
                        //                        })();
                        optionSSID.series[0].data = (function () {
                            var res = [];
                            for (var i = 0; i < obj.length; i++) {
                                tbStr += "<tr><td>" + obj[i].ID + "</td><td>" + obj[i].NAME + "</td><td>" + obj[i].NUM + "</td></tr>";
                                res.push({ value: obj[i].NUM, name: obj[i].NAME });
                            }
                            return res;
                        })();
                        $("#divSSIDTableBody tbody").html(tbStr);

                        completeNum++;
                    }
                });

                //广告访问量统计------------------------------------------------------------------------
                $("#divADTableBody tbody").html("<tr><td colspan='3'>正在加载...</td></tr>");
                $.ajax({
                    type: 'post', //可选get
                    url: 'AjaxComm.aspx', //这里是接收数据的PHP程序
                    data: 'type=StatisticalAD&token=' + token + '&param={' + ($("#slcAP").val() == "-99" ? "" : '"ID":"' + $("#slcAP").val() + '",') + '"Type":"AP","Mode":"Date","StartTime":"' + $("#startDate").val() + '","EndTime":"' + $("#endDate").val() + '"}',
                    dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                    success: function (obj) {
                        var tbStr = "";
                        //                        optionAD.legend.data = (function () {
                        //                            var res = [];
                        //                            for (var i = 0; i < obj.length; i++) {
                        //                                res.push(obj[i].NAME);
                        //                            }
                        //                            return res;
                        //                        })();
                        optionAD.series[0].data = (function () {
                            var res = [];
                            for (var i = 0; i < obj.length; i++) {
                                tbStr += "<tr><td>" + obj[i].ID + "</td><td>" + obj[i].NAME + "</td><td>" + obj[i].NUM + "</td></tr>";
                                res.push({ value: obj[i].NUM, name: obj[i].NAME });
                            }
                            return res;
                        })();
                        $("#divADTableBody tbody").html(tbStr);

                        completeNum++;
                    }
                });
                completeAll($("[md='main']").length);
            });
        });

        var echarts;
        var options = [];
        var completeNum;
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

            callbackFun();

            window.onresize = function () {
                for (var i = 0, l = myChart.length; i < l; i++) {
                    myChart[i].resize && myChart[i].resize();
                }
            };
        }

        function callbackFun() {
            completeNum = 0;
            //在线人数统计------------------------------------------------------------------------
            $.ajax({
                type: 'post', //可选get
                url: 'AjaxComm.aspx', //这里是接收数据的PHP程序
                data: 'type=StatisticalOnline&token=' + token + '&param={' + ($("#slcAP").val() == "-99" ? "" : '"ID":"' + $("#slcAP").val() + '",') + '"Type":"AP","Mode":"Week"}',
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    optionOnline.xAxis[0].data = (function () {
                        var now = new Date();
                        var res = [];
                        var len = 7;
                        while (len--) {
                            res.unshift(now.toLocaleDateString().substr(5));
                            now = new Date(now - (3600000 * 24));
                        }
                        return res;
                    })();
                    optionOnline.series[0].data = obj[0];
                    optionOnline.series[1].data = obj[1];
                    optionOnline.xAxis[0].axisLabel.interval = 'auto';
                    optionOnline.xAxis[0].axisLabel.rotate = 0;
                    completeNum++;
                }
            });
            //WIFI连接使用统计------------------------------------------------------------------------
            $.ajax({
                type: 'post', //可选get
                url: 'AjaxComm.aspx', //这里是接收数据的PHP程序
                data: 'type=StatisticalPeople&token=' + token + '&param={' + ($("#slcAP").val() == "-99" ? "" : '"ID":"' + $("#slcAP").val() + '",') + '"Type":"AP","Mode":"Week"}',
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    if (obj.ResultCode != null) {
                        alert(obj.ResultMsg);
                        if (obj.ResultCode == -100) {
                            window.location.href = "login.aspx";
                        }
                        return;
                    }

                    var val1 = 0, val2 = 0;
                    for (var i = 0; i < obj[1].length; i++)
                        val1 += obj[1][i];
                    for (var i = 0; i < obj[0].length; i++)
                        val2 += obj[0][i];
                    optionPeople.series[0].data = [
                                { value: val1, name: '未认证用户数' },
                                { value: val2, name: '认证用户数' }
                            ];
                    optionPeopleLine.xAxis[0].data = (function () {
                        var now = new Date();
                        var res = [];
                        var len = 7;
                        while (len--) {
                            res.unshift(now.toLocaleDateString().replace(/^\D*/, ''));
                            now = new Date(now - (3600000 * 24));
                        }
                        return res;
                    })();
                    optionPeopleLine.series[0].data = obj[2];
                    optionPeopleLine.series[1].data = obj[0];
                    completeNum += 2;
                }
            });

            //SSID访问量统计------------------------------------------------------------------------
            $("#divSSIDTableBody tbody").html("<tr><td colspan='3'>正在加载...</td></tr>");
            $.ajax({
                type: 'post', //可选get
                url: 'AjaxComm.aspx', //这里是接收数据的PHP程序
                data: 'type=StatisticalSSID&token=' + token + '&param={' + ($("#slcAP").val() == "-99" ? "" : '"ID":"' + $("#slcAP").val() + '",') + '"Type":"AP","Mode":"Week"}',
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    var tbStr = "";
                    //                    optionSSID.legend.data = (function () {
                    //                        var res = [];
                    //                        for (var i = 0; i < obj.length; i++) {
                    //                            res.push(obj[i].NAME);
                    //                        }
                    //                        return res;
                    //                    })();
                    optionSSID.series[0].data = (function () {
                        var res = [];
                        for (var i = 0; i < obj.length; i++) {
                            tbStr += "<tr><td>" + obj[i].ID + "</td><td>" + obj[i].NAME + "</td><td>" + obj[i].NUM + "</td></tr>";
                            res.push({ value: obj[i].NUM, name: obj[i].NAME });
                        }
                        return res;
                    })();
                    $("#divSSIDTableBody tbody").html(tbStr);

                    completeNum++;
                }
            });

            //广告访问量统计------------------------------------------------------------------------
            $("#divADTableBody tbody").html("<tr><td colspan='3'>正在加载...</td></tr>");
            $.ajax({
                type: 'post', //可选get
                url: 'AjaxComm.aspx', //这里是接收数据的PHP程序
                data: 'type=StatisticalAD&token=' + token + '&param={' + ($("#slcAP").val() == "-99" ? "" : '"ID":"' + $("#slcAP").val() + '",') + '"Type":"AP","Mode":"Week"}',
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    var tbStr = "";
                    //                    optionAD.legend.data = (function () {
                    //                        var res = [];
                    //                        for (var i = 0; i < obj.length; i++) {
                    //                            res.push(obj[i].NAME);
                    //                        }
                    //                        return res;
                    //                    })();
                    optionAD.series[0].data = (function () {
                        var res = [];
                        for (var i = 0; i < obj.length; i++) {
                            tbStr += "<tr><td>" + obj[i].ID + "</td><td>" + obj[i].NAME + "</td><td>" + obj[i].NUM + "</td></tr>";
                            res.push({ value: obj[i].NUM, name: obj[i].NAME });
                        }
                        return res;
                    })();
                    $("#divADTableBody tbody").html(tbStr);

                    completeNum++;
                }
            });
            completeAll($("[md='main']").length);
        }

        function initOption() {
            options.push(optionOnline);
            options.push(optionPeople);
            options.push(optionPeopleLine);
            options.push(optionSSID);
            options.push(optionAD);
        }

        function StatisticalRealTime() {
            optionOnline.xAxis[0].axisLabel.interval = 'auto';
            optionOnline.xAxis[0].axisLabel.rotate = 0;
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

            clearInterval(timeTicket);
            refresh(true, 0);
            timeTicket = setInterval(StatisticalOnline, 5000);
        }

        function StatisticalWeek() {
            $("#startDate").val(new Date(new Date() - (3600000 * 24 * 6)).format("yyyy-MM-dd"));
            $("#endDate").val(new Date().format("yyyy-MM-dd"));

            showLoadings();
            completeNum = 0;
            //在线人数统计------------------------------------------------------------------------
            clearInterval(timeTicket);
            $.ajax({
                type: 'post', //可选get
                url: 'AjaxComm.aspx', //这里是接收数据的PHP程序
                data: 'type=StatisticalOnline&token=' + token + '&param={' + ($("#slcAP").val() == "-99" ? "" : '"ID":"' + $("#slcAP").val() + '",') + '"Type":"AP","Mode":"Week"}',
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    optionOnline.xAxis[0].data = (function () {
                        var now = new Date();
                        var res = [];
                        var len = 7;
                        while (len--) {
                            res.unshift(now.toLocaleDateString().substr(5));
                            now = new Date(now - (3600000 * 24));
                        }
                        return res;
                    })();
                    optionOnline.series[0].data = obj[0];
                    optionOnline.series[1].data = obj[1];
                    optionOnline.xAxis[0].axisLabel.interval = 'auto';
                    optionOnline.xAxis[0].axisLabel.rotate = 0;
                    completeNum++;
                }
            });

            //WIFI连接使用统计------------------------------------------------------------------------
            $.ajax({
                type: 'post', //可选get
                url: 'AjaxComm.aspx', //这里是接收数据的PHP程序
                data: 'type=StatisticalPeople&token=' + token + '&param={' + ($("#slcAP").val() == "-99" ? "" : '"ID":"' + $("#slcAP").val() + '",') + '"Type":"AP","Mode":"Week"}',
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    var val1 = 0, val2 = 0;
                    for (var i = 0; i < obj[1].length; i++)
                        val1 += obj[1][i];
                    for (var i = 0; i < obj[0].length; i++)
                        val2 += obj[0][i];
                    optionPeople.series[0].data = [
                                { value: val1, name: '未认证用户数' },
                                { value: val2, name: '认证用户数' }
                            ];
                    optionPeopleLine.xAxis[0].data = (function () {
                        var now = new Date();
                        var res = [];
                        var len = 7;
                        while (len--) {
                            res.unshift(now.toLocaleDateString().replace(/^\D*/, ''));
                            now = new Date(now - (3600000 * 24));
                        }
                        return res;
                    })();
                    optionPeopleLine.series[0].data = obj[2];
                    optionPeopleLine.series[1].data = obj[0];
                    completeNum += 2;
                }
            });

            //SSID访问量统计------------------------------------------------------------------------
            $("#divSSIDTableBody tbody").html("<tr><td colspan='3'>正在加载...</td></tr>");
            $.ajax({
                type: 'post', //可选get
                url: 'AjaxComm.aspx', //这里是接收数据的PHP程序
                data: 'type=StatisticalSSID&token=' + token + '&param={' + ($("#slcAP").val() == "-99" ? "" : '"ID":"' + $("#slcAP").val() + '",') + '"Type":"AP","Mode":"Week"}',
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    var tbStr = "";
                    //                    optionSSID.legend.data = (function () {
                    //                        var res = [];
                    //                        for (var i = 0; i < obj.length; i++) {
                    //                            res.push(obj[i].NAME);
                    //                        }
                    //                        return res;
                    //                    })();
                    optionSSID.series[0].data = (function () {
                        var res = [];
                        for (var i = 0; i < obj.length; i++) {
                            tbStr += "<tr><td>" + obj[i].ID + "</td><td>" + obj[i].NAME + "</td><td>" + obj[i].NUM + "</td></tr>";
                            res.push({ value: obj[i].NUM, name: obj[i].NAME });
                        }
                        return res;
                    })();
                    $("#divSSIDTableBody tbody").html(tbStr);

                    completeNum++;
                }
            });

            //广告访问量统计------------------------------------------------------------------------
            $("#divADTableBody tbody").html("<tr><td colspan='3'>正在加载...</td></tr>");
            $.ajax({
                type: 'post', //可选get
                url: 'AjaxComm.aspx', //这里是接收数据的PHP程序
                data: 'type=StatisticalAD&token=' + token + '&param={' + ($("#slcAP").val() == "-99" ? "" : '"ID":"' + $("#slcAP").val() + '",') + '"Type":"AP","Mode":"Week"}',
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    var tbStr = "";
                    //                    optionAD.legend.data = (function () {
                    //                        var res = [];
                    //                        for (var i = 0; i < obj.length; i++) {
                    //                            res.push(obj[i].NAME);
                    //                        }
                    //                        return res;
                    //                    })();
                    optionAD.series[0].data = (function () {
                        var res = [];
                        for (var i = 0; i < obj.length; i++) {
                            tbStr += "<tr><td>" + obj[i].ID + "</td><td>" + obj[i].NAME + "</td><td>" + obj[i].NUM + "</td></tr>";
                            res.push({ value: obj[i].NUM, name: obj[i].NAME });
                        }
                        return res;
                    })();
                    $("#divADTableBody tbody").html(tbStr);

                    completeNum++;
                }
            });
            completeAll($("[md='main']").length);
        }

        function StatisticalMonth() {
            $("#startDate").val(new Date(new Date() - (3600000 * 24 * 29)).format("yyyy-MM-dd"));
            $("#endDate").val(new Date().format("yyyy-MM-dd"));

            showLoadings();
            completeNum = 0;
            //在线人数统计------------------------------------------------------------------------
            clearInterval(timeTicket);
            $.ajax({
                type: 'post', //可选get
                url: 'AjaxComm.aspx', //这里是接收数据的PHP程序
                data: 'type=StatisticalOnline&token=' + token + '&param={' + ($("#slcAP").val() == "-99" ? "" : '"ID":"' + $("#slcAP").val() + '",') + '"Type":"AP","Mode":"Month"}',
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    optionOnline.xAxis[0].data = (function () {
                        var now = new Date();
                        var res = [];
                        var len = 30;
                        while (len--) {
                            res.unshift(now.toLocaleDateString().substr(5));
                            now = new Date(now - (3600000 * 24));
                        }
                        return res;
                    })();
                    optionOnline.series[0].data = obj[0];
                    optionOnline.series[1].data = obj[1];
                    optionOnline.xAxis[0].axisLabel.interval = 0;
                    optionOnline.xAxis[0].axisLabel.rotate = -30;

                    completeNum++;
                }
            });

            //WIFI连接使用统计------------------------------------------------------------------------
            $.ajax({
                type: 'post', //可选get
                url: 'AjaxComm.aspx', //这里是接收数据的PHP程序
                data: 'type=StatisticalPeople&token=' + token + '&param={' + ($("#slcAP").val() == "-99" ? "" : '"ID":"' + $("#slcAP").val() + '",') + '"Type":"AP","Mode":"Month"}',
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    var val1 = 0, val2 = 0;
                    for (var i = 0; i < obj[1].length; i++)
                        val1 += obj[1][i];
                    for (var i = 0; i < obj[0].length; i++)
                        val2 += obj[0][i];
                    optionPeople.series[0].data = [
                                { value: val1, name: '未认证用户数' },
                                { value: val2, name: '认证用户数' }
                            ];
                    optionPeopleLine.xAxis[0].data = (function () {
                        var now = new Date();
                        var res = [];
                        var len = 30;
                        while (len--) {
                            res.unshift(now.toLocaleDateString().replace(/^\D*/, ''));
                            now = new Date(now - (3600000 * 24));
                        }
                        return res;
                    })();
                    optionPeopleLine.series[0].data = obj[2];
                    optionPeopleLine.series[1].data = obj[0];
                    completeNum += 2;
                }
            });

            //SSID访问量统计------------------------------------------------------------------------
            $("#divSSIDTableBody tbody").html("<tr><td colspan='3'>正在加载...</td></tr>");
            $.ajax({
                type: 'post', //可选get
                url: 'AjaxComm.aspx', //这里是接收数据的PHP程序
                data: 'type=StatisticalSSID&token=' + token + '&param={' + ($("#slcAP").val() == "-99" ? "" : '"ID":"' + $("#slcAP").val() + '",') + '"Type":"AP","Mode":"Month"}',
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    var tbStr = "";
                    //                    optionSSID.legend.data = (function () {
                    //                        var res = [];
                    //                        for (var i = 0; i < obj.length; i++) {
                    //                            res.push(obj[i].NAME);
                    //                        }
                    //                        return res;
                    //                    })();
                    optionSSID.series[0].data = (function () {
                        var res = [];
                        for (var i = 0; i < obj.length; i++) {
                            tbStr += "<tr><td>" + obj[i].ID + "</td><td>" + obj[i].NAME + "</td><td>" + obj[i].NUM + "</td></tr>";
                            res.push({ value: obj[i].NUM, name: obj[i].NAME });
                        }
                        return res;
                    })();
                    $("#divSSIDTableBody tbody").html(tbStr);

                    completeNum++;
                }
            });

            //广告访问量统计------------------------------------------------------------------------
            $("#divADTableBody tbody").html("<tr><td colspan='3'>正在加载...</td></tr>");
            $.ajax({
                type: 'post', //可选get
                url: 'AjaxComm.aspx', //这里是接收数据的PHP程序
                data: 'type=StatisticalAD&token=' + token + '&param={' + ($("#slcAP").val() == "-99" ? "" : '"ID":"' + $("#slcAP").val() + '",') + '"Type":"AP","Mode":"Month"}',
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    var tbStr = "";
                    //                    optionAD.legend.data = (function () {
                    //                        var res = [];
                    //                        for (var i = 0; i < obj.length; i++) {
                    //                            res.push(obj[i].NAME);
                    //                        }
                    //                        return res;
                    //                    })();
                    optionAD.series[0].data = (function () {
                        var res = [];
                        for (var i = 0; i < obj.length; i++) {
                            tbStr += "<tr><td>" + obj[i].ID + "</td><td>" + obj[i].NAME + "</td><td>" + obj[i].NUM + "</td></tr>";
                            res.push({ value: obj[i].NUM, name: obj[i].NAME });
                        }
                        return res;
                    })();
                    $("#divADTableBody tbody").html(tbStr);

                    completeNum++;
                }
            });
            completeAll($("[md='main']").length);
        }

        function completeAll(_length) {
            if (completeNum == _length) {
                if (options.length == 0) {
                    initOption();
                    //$("[md='wrong-message']").html("提示信息显示部分");
                }
                refreshAll();
            }
            else
                setTimeout(function () { completeAll(_length); }, 10);
        }
    </script>
</head>
<body>
    <uc1:header ID="header1" runat="server" />

    <div class="container-fluid">
      <div class="row-fluid">
      <div class="div-body col-md-12">
        <div id="divOnline" class="bodyMain" style="height: 250px">
            <div>
                设备：<select id="slcAP"><option value="-99" selected>全部</option></select>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <input id="btnRealTime" style="width: 50px;" type="button" value="实时" onclick="StatisticalRealTime()" />
                <input id="btnWeek" style="width: 70px;" type="button" value="近一周" onclick="StatisticalWeek()" />
                <input id="btnMonth" style="width: 70px;" type="button" value="近一月" onclick="StatisticalMonth()" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                时间区间：
                <input id="startDate" type="text" class="Wdate" onFocus="WdatePicker({isShowWeek:true})"/>
                -
                <input id="endDate" type="text" class="Wdate" onFocus="WdatePicker({isShowWeek:true})"/>
                <input id="btnSubmit" style="width: 70px;" type="button" value="统计" onclick="" />
            </div>
            <div id="divOnlineBody" md="main" style="height: 88%"></div>
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

                clearInterval(timeTicket);
                var timeTicket;
                //timeTicket = setInterval(StatisticalOnline, 5000);
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
      <div class="div-body col-md-4">
        <div id="divPeople" class="bodyMain">
            <div id="divPeopleBody" md="main" style="height: 46%"></div>
            <span md="wrong-message" style="color: red"></span>
            <script type="text/javascript">
                optionPeople = {
                    title: {
                        text: 'WIFI连接使用量统计',
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

            <div id="divPeopleLineBody" md="main" style="height: 50%"></div>
            <script type="text/javascript">
                optionPeopleLine = {
                    tooltip: {
                        trigger: 'axis'
                    },
                    legend: {
                        orient: 'vertical',
                        x: 'right',
                        y: 'center',
                        data: ['总用户数', '认证用户数']
                    },
                    grid: {
                        x: 40,
                        y: 20,
                        x2: 100,
                        y2: 40
                    },
                    xAxis: [
                        {
                            type: 'category',
                            boundaryGap: false,
                            data: (function () {
                                var now = new Date();
                                var res = [];
                                var len = 10;
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
                            name: '人数',
                            boundaryGap: [0.2, 0.2]
                        }
                    ],
                    series: [
                        {
                            name: '总用户数',
                            type: 'line',
                            smooth: true,
                            data: (function () {
                                var res = [];
                                var len = lineNum;
                                while (len--) {
                                    res.push(20);
                                }
                                return res;
                            })()
                        },
                        {
                            name: '认证用户数',
                            type: 'line',
                            smooth: true,
                            data: (function () {
                                var res = [];
                                var len = lineNum;
                                while (len--) {
                                    res.push(10);
                                }
                                return res;
                            })()
                        }
                    ]
                };
            </script>
        </div>
      </div>
      <div class="div-body col-md-4">
        <div id="divSSID" class="bodyMain">
            <div id="divSSIDBody" md="main" style="height: 46%;"></div>
            <span md="wrong-message" style="color: red"></span>
            <script type="text/javascript">
                optionSSID = {
                    title: {
                        text: 'SSID访问量统计',
                        x: 'right'
                    },
                    tooltip: {
                        trigger: 'item',
                        formatter: "{a} <br/>{b} : {c} ({d}%)"
                    },
                    //                    legend: {
                    //                        orient: 'vertical',
                    //                        x: 'right',
                    //                        y: 'center',
                    //                        data: ['SSID1', 'SSID2', 'SSID3', 'SSID4', 'SSID5']
                    //                    },
                    series: [
                        {
                            name: 'SSID访问量统计',
                            type: 'pie',
                            radius: '50%',
                            startAngle: 90,
                            minAngle: 0,
                            center: ['50%', 90],
                            data: [
                                { value: 315, name: 'SSID1' },
                                { value: 210, name: 'SSID2' },
                                { value: 234, name: 'SSID3' },
                                { value: 155, name: 'SSID4' },
                                { value: 284, name: 'SSID5' }
                            ]
                        }
                    ]
                };
            </script>

            <div id="divSSIDTableBody" style="width: 100%; height: 50%">
                <table class="tbClass hor-minimalist-top table-bordered table-striped">
                    <thead>
                        <tr>
                            <td>序号</td>
                            <td>名称</td>
                            <td>数量</td>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
            </div>
        </div>
      </div>
      <div class="div-body col-md-4">
        <div id="divAD" class="bodyMain">
            <div id="divADBody" md="main" style="height: 46%;"></div>
            <span md="wrong-message" style="color: red"></span>
            <script type="text/javascript">
                optionAD = {
                    title: {
                        text: '广告页访问量统计',
                        x: 'right'
                    },
                    tooltip: {
                        trigger: 'item',
                        formatter: "{a} <br/>{b} : {c} ({d}%)"
                    },
                    //                    legend: {
                    //                        orient: 'vertical',
                    //                        x: 'right',
                    //                        y: 'center',
                    //                        data: ['广告1', '广告2', '广告3', '广告4', '广告5']
                    //                    },
                    series: [
                        {
                            name: '广告页访问量统计',
                            type: 'pie',
                            radius: '50%',
                            startAngle: 180,
                            minAngle: 0,
                            center: ['50%', 90],
                            data: [
                                { value: 335, name: '广告1' },
                                { value: 310, name: '广告2' },
                                { value: 234, name: '广告3' },
                                { value: 135, name: '广告4' },
                                { value: 1048, name: '广告5' }
                            ]
                        }
                    ]
                };
            </script>

            <div id="divADTableBody" style="width: 100%; height: 50%">
                <table class="tbClass hor-minimalist-top table-bordered table-striped">
                    <thead>
                        <tr>
                            <td>序号</td>
                            <td>名称</td>
                            <td>数量</td>
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
