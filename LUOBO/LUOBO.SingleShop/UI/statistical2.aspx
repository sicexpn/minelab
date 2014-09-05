<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="statistical2.aspx.cs" Inherits="LUOBO.SingleShop.UI.statistical2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>统计</title>
    <script src="../UITemplet/js/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../UITemplet/echarts/js/esl.js" type="text/javascript"></script>
    <script src="../UITemplet/echarts/js/codemirror.js" type="text/javascript"></script>
    <script src="../UITemplet/echarts/js/javascript.js" type="text/javascript"></script>
    <script src="../UITemplet/js/Common.js" type="text/javascript"></script>

    <link href="../UITemplet/echarts/css/monokai.css" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/echarts/css/codemirror.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        $().ready(function () {
            if (GetQueryString("apid") != null)
                $("#btnAll").show();

            $("#btnAll").click(function () { window.location.href = window.location.pathname + "?token=" + GetQueryString("token"); });

            InitPeopleChart(GetQueryString("token"), (GetQueryString("apid") == null ? "-99" : GetQueryString("apid")));
        });
    </script>
</head>
<body>
    <div id="divPeople">
        <div>
            <div style="float:left;"></div>
            <div style="float:right;">
                <input id="btnAll" style="width:40px; height:20px; display:none" type="button" value="全部" />
                <input id="btnRealTime" style="width:40px; height:20px" type="button" value="实时" onclick="StatisticalPeopleRealTime()" />
                <input id="btnWeek" style="width:55px; height:20px" type="button" value="近一周" onclick="StatisticalPeopleWeek()" />
                <input id="btnMonth" style="width:55px; height:20px" type="button" value="近一月" onclick="StatisticalPeopleMonth()" />
                <input id="btnYear" style="width:55px; height:20px" type="button" value="近一年" onclick="StatisticalPeopleYear()" />
            </div>
        </div>
        <div id="divPeopleBody" md="main" style="width:100%; min-height: 180px; max-height:400px"></div>
        <span md="wrong-message" style="color:red"></span>  
    </div>
    <script src="../UITemplet/js/SingleShopMainChart.js" type="text/javascript"></script>
    <script src="../UITemplet/echarts/js/echartsExample.js" type="text/javascript"></script>
</body>
</html>
