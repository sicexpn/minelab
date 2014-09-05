<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="statistical.aspx.cs" Inherits="LUOBO.SingleShop.UI.statistical" %>
<%@ Register src="header.ascx" tagname="header" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>统计</title>
    <script type="text/javascript" src="../UITemplet/js/Common.js"></script>
    <script src="../UITemplet/js/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../UITemplet/echarts/js/esl.js" type="text/javascript"></script>
    <script src="../UITemplet/echarts/js/codemirror.js" type="text/javascript"></script>
    <script src="../UITemplet/echarts/js/javascript.js" type="text/javascript"></script>
    <script src="../UITemplet/js/bootstrap.js" type="text/javascript"></script>
    <script src="../UITemplet/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../UITemplet/js/jquery.searchSelectBox.js" type="text/javascript"></script>
    
    <link href="../UITemplet/css/bootstrap.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/css/font-awesome.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/css/index.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/css/sidebar.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/echarts/css/monokai.css" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/echarts/css/codemirror.css" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/css/jquery.searchSelectBox.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .bodyMain
        {
            background: #FFF;
            height: 250px;
            border-radius: 4px;
            margin: 0 3px 5px 3px;
            padding: 5px;
            box-shadow: 0 1px 1px rgba(0, 0, 0, 0.15);
        }
    </style>

    <script type="text/javascript">
        var token = GetQueryString("token");
        var echarts;
        var options = [];
        var completeNum;

        function SeriesItem() {
            var obj = new Object();

            obj.name = '';
            obj.type = 'bar';
            obj.data = new Array();

            return obj;
        }

        $().ready(function () {

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
                            $('#txtAPList').searchSelectBox(obj.ResultOBJ.APList, { key: "ID", value: "ALIAS" });
//                            for (var i = 0; i < obj.ResultOBJ.APList.length; i++) {
//                                item = obj.ResultOBJ.APList[i];
//                                result = "<option value='" + item.ID + "'>" + item.ALIAS + "</option>";
//                                $("#slcAP").append(result);
//                                //                                result1 = "<li><label for='" + item.ID + "'>" + item.ALIAS + "</label></li>";
//                                //                                $("#ulAPList").append(result1);
//                            }
                            //$("#slcAP option").each(function () {
                            //    if ($(this).val() == apid)
                            //        $(this).attr("selected", true);
                            //});

                            completeNum = 0;
                            InitPeople();
                            InitModel();
                            InitAD();
                            completeAll();
                        }
                    }
                    else {
                        alert(obj.ResultMsg);
                    }
                }
            });

            $("#btnSubmit").click(function () {
                showLoadings();
                completeNum = 0;
                InitPeople();
                InitModel();
                InitAD();
                completeAll();
            });

            $("#txtAPList").change(function () {
                showLoadings();
                completeNum = 0;
                InitPeople();
                InitModel();
                InitAD();
                completeAll();
            });
        });

        function InitPeople() {
            $.ajax({
                type: 'post', //可选get
                url: 'AjaxComm.aspx', //这里是接收数据的PHP程序
                data: 'type=SelectTowHourIntervalPeopleCount&token=' + token + '&param=' + $("#txtAPList").prop("key"),
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    optionPeople.xAxis[0].data = (function () {
                        var now = new Date();
                        var res = [];

                        for (var i = 0; i <= now.getHours(); i += 2)
                            res.push(i);

                        return res;
                    })();

                    optionPeople.series[0].data = new Array();
                    for (var i = 0; i < obj.ResultOBJ.length; i++) {
                        optionPeople.series[0].data.push(obj.ResultOBJ[i]);
                    }
                    completeNum++;
                }
            });
        }

        function InitModel() {
            $.ajax({
                type: 'post', //可选get
                url: 'AjaxComm.aspx', //这里是接收数据的PHP程序
                data: 'type=SelectTowHourIntervalModelCount&token=' + token + '&param=' + $("#txtAPList").prop("key"),
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    optionModel.xAxis[0].data = (function () {
                        var now = new Date();
                        var res = [];

                        for (var i = 0; i <= now.getHours(); i += 2)
                            res.push(i);

                        return res;
                    })(); ;

                    optionModel.legend.data = new Array();
                    optionModel.series = new Array();
                    if (obj.ResultOBJ.length > 0) {
                        for (var i = 0; i < obj.ResultOBJ.length; i++) {
                            optionModel.legend.data.push(obj.ResultOBJ[i].NAME);
                            var item = new SeriesItem();
                            item.name = obj.ResultOBJ[i].NAME;
                            item.data = obj.ResultOBJ[i].NUM;
                            optionModel.series.push(item);
                        }
                    }
                    else {
                        optionModel.legend.data.push("");
                        var item = new SeriesItem();
                        for (var i = 0; i < optionModel.xAxis[0].data.length; i++)
                            item.data.push(0);
                        optionModel.series.push(item);
                    }
                    completeNum++;
                }
            });
        }

        function InitAD() {
            $.ajax({
                type: 'post', //可选get
                url: 'AjaxComm.aspx', //这里是接收数据的PHP程序
                data: 'type=SelectTowHourIntervalSSIDCount&token=' + token + '&param=' + $("#txtAPList").prop("key"),
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    optionAD.xAxis[0].data = (function () {
                        var now = new Date();
                        var res = [];

                        for (var i = 0; i <= now.getHours(); i += 2)
                            res.push(i);

                        return res;
                    })(); ;

                    optionAD.legend.data = new Array();
                    optionAD.series = new Array();
                    if (obj.ResultOBJ.length > 0) {
                        for (var i = 0; i < obj.ResultOBJ.length; i++) {
                            optionAD.legend.data.push(obj.ResultOBJ[i].NAME);
                            var item = new SeriesItem();
                            item.name = obj.ResultOBJ[i].NAME;
                            item.data = obj.ResultOBJ[i].NUM;
                            optionAD.series.push(item);
                        }
                    }
                    else {
                        optionAD.legend.data.push("");
                        var item = new SeriesItem();
                        for (var i = 0; i < optionAD.xAxis[0].data.length; i++)
                            item.data.push(0);
                        optionAD.series.push(item);
                    }
                    completeNum++;
                }
            });
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
            options.push(optionModel);
            options.push(optionAD);
            options.push(optionPeople);
        }

        function completeAll(obj) {
            if (completeNum == $("[md='main']").length)
                refreshAll(obj);
            else
                setTimeout(function () { completeAll(obj) }, 10);
        }
    </script>


    <script type="text/javascript">
//        $(function () {
//            //定义方法
//            var $ui = $('#divDropDown');
//            //定义参数将#searchform放进去		
//            
//            $('#txtAPList').bind('focus', function () {
//                //定义遍历获得.searchtext当点击鼠标时添加一个点击事件方法
//                $ui.find('.dropdownlist').width($(this).width() + 12);
//                $ui.find('.dropdownlist').slideDown(500);
//            });
//            //定义遍历获得.arrowDown后添加一个类arrowUp后在删除掉arrowDown类andSelf()可以同事添加两个不同的类，通过遍历获得.dropdown类 以滑动方式显示隐藏

//            $('#txtAPList').bind('blur', function () {
//                //定义遍历获得.searchtext当点击鼠标时添加一个点击事件方法
//                $ui.find('.dropdownlist').slideUp(500);
//            });

//            //            $ui.find(".dropdownlist").bind('mouseleave', function () {
//            //                //定义获得当鼠标点击时添加一个改变元素的背景色方法
//            //                $ui.find('.dropdownlist').slideUp(500);
//            //                //定义遍历添加arrowDown类在删除掉arrowUp将两个方法方法在一起在通过遍历获得.dropdown类调用滑动展开方法
//            //            });

//            /** 选择所有的复选框 **/
//            $ui.find('.dropdownlist').find('label[for="all"]').prev().bind('click', function () {
//                //定义遍历获得.dropdown在通过遍历获得label[for="all"]前一个同胞元素点击鼠标时添加一个点击事件
//                $(this).parent().siblings().find(':checkbox').attr('checked', this.checked).attr('disabled', this.checked);
//                //当前查找每个段落的带有查找每个元素的所有类名通过遍历在获得:checkbox获得当前的checked 属性的当前的checked在获得
//            });
//        });
	</script>
</head>
<body>
    <uc1:header ID="header1" runat="server" />
    <div class="container-fluid">
<%--        <div class="row-fluid">
            <div class="div-body col-md-12">
                <div id="divDropDown" class="bodyMain" style="height:auto">
                    <div class="input-group" style="width:700px">
                      <input id="txtAPList" type="text" class="form-control" />
                      <span class="input-group-addon dropdown-caret-down" style="padding:0px 6px"></span>
                    </div>
                    <ul id="ulAPList" class="dropdownlist clearfix" style="display:none; z-index:100000000; overflow:auto; max-height:200px">
			        </ul>
                </div>
            </div>
        </div>--%>
        <div class="row-fluid">
            <div class="div-body col-md-12">
                <div class="bodyMain" style="height:40px">
                    <span style="float:left">设备：</span>
                    <input id="txtAPList" type="text" />
                    <%--<select id="slcAP"><option value="-99">全部</option></select>--%>
                    <input id="btnSubmit" type="button" value="刷新" />
                </div>
            </div>
        </div>
      <div class="row-fluid">
        <div class="div-body col-md-6">
            <div id="divModel" class="bodyMain">
                <div id="divModelBody" md="main" style="height: 100%"></div>
                <span md="wrong-message" style="color: red"></span>
                <script type="text/javascript">
                    optionModel = {
                        title: {
                            text: '机型访问人次统计',
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
            </div>
        </div>
        <div class="div-body col-md-6">
            <div id="divAD" class="bodyMain bodyMain-height">
                <div id="divADBody" md="main" style="height: 100%"></div>
                <span md="wrong-message" style="color: red"></span>
                <script type="text/javascript">
                    optionAD = {
                        title: {
                            text: 'SSID访问人次统计',
                            x: 'center'
                        },
                        grid: {
                            x2: 50,
                            y2: 30,
                            x: 50,
                            y: 70
                        },
                        tooltip: {
                            trigger: 'axis',
                            axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                                type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
                            }
                        },
                        legend: {
                            data: ['访问人数', '访问人次'],
                            orient: 'horizontal',
                            x: 'center',
                            y: 40
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
            </div>
        </div>
      </div>
      <div class="row-fluid">
        <div class="div-body col-md-6">
            <div id="divPeople" class="bodyMain">
                <div id="divPeopleBody" md="main" style="height: 100%"></div>
                <span md="wrong-message" style="color: red"></span>
                <script type="text/javascript">
                    optionPeople = {
                        title: {
                            text: '访问人次统计',
                            x: 'center'
                        },
                        tooltip: {
                            trigger: 'axis'
                        },
                        legend: {
                            orient: 'vertical',
                            x: 'right',
                            y: 'center',
                            data: ['访问人次']
                        },
                        grid: {
                            x2: 100,
                            y2: 30,
                            x: 50,
                            y: 30
                        },
                        xAxis: [
                            {
                                type: 'category',
                                boundaryGap: false,
                                data: ['周一', '周二', '周三', '周四', '周五', '周六', '周日']
                            }
                        ],
                        yAxis: [
                            {
                                type: 'value',
                                scale: true,
                                boundaryGap: [0.2, 0.2]
                            }
                        ],
                        series: [
                            {
                                name: '访问人次',
                                type: 'line',
                                smooth: true,
                                data: [0]
                            }
                        ]
                    };
                </script>
            </div>
        </div>
      </div>
    </div>
    <script src="../UITemplet/echarts/js/echartsExample.js" type="text/javascript"></script>
</body>
</html>
