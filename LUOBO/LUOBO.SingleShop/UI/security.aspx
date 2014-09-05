<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="security.aspx.cs" Inherits="LUOBO.SingleShop.UI.state" %>
<%@ Register src="header.ascx" tagname="header" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
 <head runat="server">
    <title>next-wifi 安全</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" charset="utf-8"/>

    <link href="../UITemplet/css/bootstrap.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/css/font-awesome.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/css/index.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/css/style.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/css/AeroWindow-Contextmenu.css" rel="stylesheet">
    <link href="../UITemplet/css/grumble.min.css" media="all" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../UITemplet/js/Common.js"></script>
    <script src="../UITemplet/js/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../UITemplet/js/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../UITemplet/js/jquery.grumble.min.js" type="text/javascript"></script>
    <script src="../UITemplet/js/bootstrap.js" type="text/javascript"></script>
    <script src="../UITemplet/js/login.js" type="text/javascript"></script>
    <script src="../UITemplet/js/respond.js" type="text/javascript"></script>
    <script src="../UITemplet/js/AeroWindow-Contextmenu.js" type="text/javascript"></script>
    
    <script type="text/javascript">
        var cur_ap_sort = { faild: "", sort_idx: 0 },
            all_user_sort = {},
            ap_sort_faild = {
                ap_sort_powertime: "powertime",
                ap_sort_memfree: "memfree",
                ap_sort_Strength: "networktotal",
                ap_sort_Channel: "networkrate",
                ap_sort_olpeople: "onlinepeoplenum"
            },
            user_sort_faild = {
                user_sort_connecttime: "ConnectTime",
                user_sort_onlinetime: "OnLineTime",
                user_sort_onlinecounts: "OnLineCounts",
                user_sort_usedtraffic: "UsedTraffic",
                user_sort_onlinetype: "OnLineType"
            };
        var SORT = ["", "desc", "asc"];
        var is_ap_loading = true;

        var expand_tr,
            expand_tr_next,
            is_show_flow = false,
            is_bottom_flow = false;

        var token = GetQueryString("token"); //读取token

        $(document).ready(function () {
            HighLightMenu("安全");
            bindClick();
            getAPList();
        });

        function bindClick() {
            // 搜素按钮
//            $('#search_btn').click(function () {
//                if (!is_ap_loading) {
//                    is_ap_loading = true;
//                    cur_ap_sort = { faild: "", sort_idx: 0 };
//                    all_user_sort = {};
//                    var h_i = $('table[name="dTable"]').find("th i").removeClass().addClass("fa fa-sort");
//                    getAPList();
//                } else {
//                    $(this).grumble({
//                        text: '正在查询<br />请稍候',
//                        angle: 90,
//                        distance: 30,
//                        type: 'alt-',
//                        hideAfter: 500
//                    });
//                }
//            });

            // 浮动Div
            $(window).scroll(function () {
                setFlowDivPostion();
            });

        }

        function gotoTop() {
            if (expand_tr != null) {
                $("body,html").animate({
                    scrollTop: $(expand_tr).offset().top
                }, 800);
            }
        }

        function setFlowDivPostion() {
            if (expand_tr != null) {
                if ($(expand_tr).offset().top - $(document).scrollTop() < 0) {
                    if (!is_show_flow) {
                        $('#flow_div').show();
                        $('#flow_to_top').show();
                        is_show_flow = true;
                    }

                    if (expand_tr_next.length != 0) {
                        if ($(document).scrollTop() + $('#flow_div').height() < $(expand_tr_next).offset().top) {
                            $('#flow_div').show();

                            $('#flow_div').css("top", $(document).scrollTop());
                        } else if ($(document).scrollTop() < $(expand_tr_next).offset().top) {
                            $('#flow_div').show();

                            $('#flow_div').css("top", $(expand_tr_next).offset().top - $('#flow_div').height());
                            is_bottom_flow = false;
                        } else {
                            $('#flow_div').hide();
                        }
                    } else {
                        $('#flow_div').show();
                        $('#flow_div').css("top", $(document).scrollTop());
                    }
                } else {
                    if (is_show_flow) {
                        $('#flow_div').hide();
                        $('#flow_to_top').hide();
                        is_show_flow = false;
                    }
                }
            }
        }

        function getAPList() {
            $("#DbdyList").empty
            $("#DbdyList").html("<tr><td colspan='9'>正在加载...</td></tr>");
            $.ajax({
                type: 'post',
                url: 'AjaxComm.aspx',
                data: 'type=GetWarnInfo&token=' + token,
                dataType: 'json',
                error: function (msg) {
                    is_ap_loading = false;
                    //alert("网络错误");
                    //window.location.href = "login.aspx";
                },
                success: function (obj) {
                    is_ap_loading = false;
                    if (obj.ResultCode == "0") {
                        $("#DbdyList").empty();
                        if (obj.ResultOBJ.length > 0) {
                            var item;
                            for (var i = 0; i < obj.ResultOBJ.length; i++) {
                                item = obj.ResultOBJ[i];
                                if (item.KEYWORD == null) {
                                    result = "<tr id='apTr" + item.ID + "' name='ap' class='success' style='cursor:pointer'" + item.ADDRESS + "\n'>";
                                } else {
                                    result = "<tr id='apTr" + item.ID + "' name='ap' class='warning' style='cursor:pointer'" + item.ADDRESS + "\n'>";
                                }
                                result += "<td style='width:180px;'><input name='apid' type='hidden' value='" + item.LOG_ID + "' mac='" + item.G_MAC + "' /><i class='fa fa-chevron-circle-right'></i>  " + item.APNAME + "</td>";
                                result += "<td>" + item.G_SSID + "</td>";
                                result += "<td>" + item.G_MAC + "</td>";
                                result += "<td>" + item.G_STRONG + " kb/s</td>";
                                result += "<td>" + item.KEYWORD + "</td>";
                                result += "</tr>";
                                result += "<tr id='mTr" + item.LOG_ID + "'><td style='padding:0px' colspan='7'><div name='mDiv' style='margin:15px'></div></td></tr>";
                                $("#DbdyList").append(result);
                            }

                            dTableBindEvent();
                            $("tr[id^='mTr']").hide();

                        } else {
                            $("#DbdyList").html("<tr><td colspan='9'>没有任何数据...</td></tr>");
                        }

                    }
                    else {
                        alert(obj.ResultMsg);
                        if (obj.ResultCode == -100) {
                            window.location.href = "login.aspx";
                        }
                    }
                }
            });
        }


        function dTableBindEvent() {
            var tbList = $("[name='dTable']");
            tbList.each(function () {
                var self = this;

                $("tr[name='ap']", $(self)).click(function () {
                    var trThis = this;
                    var did = $(trThis).find("[name='apid']").val();
                    $("tr[id^='mTr']").each(function () {
                        if ($(this).attr("id") != "mTr" + did) {
                            var obj = $(this);
                            $(this).find("div[name='mDiv']").hide("normal", function () { $(obj).hide(); });
                        }
                    });

                    if ($(trThis).next().is(":hidden")) {
                        $(trThis).next().show("normal");
                        $(trThis).next().find("div[name='mDiv']").show("normal");
                        $("tr[name='ap']").find("i").removeClass().addClass("fa fa-chevron-circle-right");
                        $(trThis).find("i").removeClass().addClass("fa fa-chevron-circle-down");

                        expand_tr = trThis;
                        expand_tr_next = $(trThis).next().next();
                        $('#flow_div').find('table').html($(trThis).clone());
                        setFlowDivPostion();

                        all_user_sort[trThis.id] || (all_user_sort[trThis.id] = { faild: "", sort_idx: 0 });
                    }
                    else {
                        expand_tr = null;
                        expand_tr_next = null;
                        $(trThis).next().find("div[name='mDiv']").hide("normal", function () { $(trThis).next().hide(); });
                        $("tr[name='ap']").find("i").removeClass().addClass("fa fa-chevron-circle-right");
                    }

                    getUserList(did, $(trThis).find("[name='apid']").attr("mac"));
                });
            });
        }

        function getUserList(_did, _mac) {
            apid = _did;
            if ($("#mTr" + _did).find("div[name='mDiv']").html() != "")
                return;

            $("#mTr" + _did).find("div[name='mDiv']").html("加载中...");
            var uTable = $("#uTable").clone();
            $(uTable).removeAttr("id");
            $.ajax({
                type: 'post',
                url: 'AjaxComm.aspx',
                data: 'type=GetAlertListByMAC&token=' + token + '&mac=' + _mac,
                dataType: 'json',
                error: function (msg) {
                    is_user_loading = false;
                    //alert("网络错误");
                    //window.location.href = "login.aspx";
                },
                success: function (obj) {
                    is_user_loading = false;
                    if (obj.ResultCode == "0") {
                        if (obj.ResultOBJ.length > 0) {
                            var item;
                            for (var i = 0; i < obj.ResultOBJ.length; i++) {
                                item = obj.ResultOBJ[i];
                                result = "<tr name='user'>";
                                result += "<td>" + item.G_SSID + "</td>";
                                result += "<td>" + item.G_MAC + "</td>";
                                result += "<td>" + item.G_STRONG + " kb/s</td>";
                                result += "<td>" + item.KEYWORD + "</td>";
                                result += "<td>" + item.G_TIME_S + "</td>";
                                result += "</tr>";
                                $(uTable).find("tbody").append(result);
                            }
                            $(uTable).show();
                            $("#mTr" + _did).find("div[name='mDiv']").html(uTable);

                        } else {
                            $("#mTr" + _did).find("div[name='mDiv']").html("没有数据");
                        }
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
<uc1:header ID="Header1" runat="server" />

<div class="container-fluid">
<div class="row-fluid">
    <div class="div-body col-md-8">
        <div class="panel panle_ssid">
            <div class="panel-heading"><!-- Split button -->
                <span id="Span11" class="label label-primary" style="font-size:14px">探测结果</span>
                <span>
                    <%--<input id="search_txt" type="search" class="input-sm" placeholder="查询关键字.."/>
                    <a id="search_btn" class="btn btn-sm btn-success" href="javascript:void(0);"><i class="icon-twitter"></i>查询</a>--%>
                </span>
            </div>
            <div class="widget-content padded clearfix">
                <table class="hor-minimalist-a table table-bordered table-hover" name="dTable" >
                <thead>
                    <tr>
                    <th scope="col" style="width:180px">发射器</th>
                    <th scope="col">探测到SSID</th>
                    <th scope="col">MAC</th>
                    <th scope="col">信号强度</th>
                    <th scope="col">关键字词</th>
                    </tr>
                </thead>
                <tbody id="DbdyList" >
                </tbody>
                </table>
            </div>
        </div>
    </div>
    <div class="div-body col-md-4">
    <div class="row-fluid">
        <div class="div-body col-md-12">
        <div class="panel panle_ssid">
            <div class="panel-heading"><!-- Split button -->
                <span id="Span1" class="label label-danger" style="font-size:14px">黑名单</span>
                <span>
                    <input id="search1" type="search" class="input-sm" placeholder="查询关键字.."/>
                    <a id="A1" class="btn btn-sm btn-success" href="javascript:void(0);"><i class="icon-twitter"></i>查询</a>
                </span>
            </div>
            <div class="widget-content padded clearfix">
                <table class="hor-minimalist-a table table-bordered table-hover" name="dTable" >
                <thead>
                    <tr>
                    <th scope="col">关键词</th>
                    </tr>
                </thead>
                <tbody id="Tbody1" >
                </tbody>
                </table>
            </div>
        </div>
        </div>
    <div class="row-fluid">
        <div class="div-body col-md-12">
        <div class="panel panle_ssid">
            <div class="panel-heading"><!-- Split button -->
                <span id="Span6" class="label label-success" style="font-size:14px">白名单</span>
                <span>
                    <input id="search2" type="search" class="input-sm" placeholder="查询关键字.."/>
                    <a id="A2" class="btn btn-sm btn-success" href="javascript:void(0);"><i class="icon-twitter"></i>查询</a>
                </span>
            </div>
            <div class="widget-content padded clearfix">
                <table class="hor-minimalist-a table table-bordered table-hover" name="dTable" >
                <thead>
                    <tr>
                    <th scope="col">安全MAC</th>
                    </tr>
                </thead>
                <tbody id="Tbody2" >
                </tbody>
                </table>
            </div>
        </div>
    </div>
    </div>
        </div>
    </div>
    
</div>
</div>

<table style="display:none" class="hor-minimalist-a table table-bordered table-hover" id="uTable" name="uTable">
    <thead>
        <tr>
            <th scope="col">探测到SSID</th>
            <th scope="col">MAC</th>
            <th scope="col">信号强度</th>
            <th scope="col">关键字词</th>
            <th scope="col">时间</th>
        </tr>
    </thead>
    <tbody>
    </tbody>
</table>



<div id="flow_div" style="position:absolute;width:100%;display:none;padding:0px 10px 0px 10px;">
    <table class="hor-minimalist-a table table-bordered table-hover" style="-webkit-box-shadow: 0px 0px 10px #000000;box-shadow: 0px 0px 10px #000000;float:left;"></table>
</div>

<div id="flow_to_top" style="position:fixed; bottom:50px; right:40px; display:none;"><a href="javascript:gotoTop();" class="btn"><i class="fa fa-arrow-up fa-3x"></i></a></div>
</body>
</html>
