<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="security3.aspx.cs" Inherits="LUOBO.SingleShop.UI.security3" %>
<%@ Register src="header.ascx" tagname="header" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
 <head id="Head1" runat="server">
    <title>next-wifi 安全</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" charset="utf-8"/>

<script type="text/javascript" src="../UITemplet/js/Common.js"></script>
<script type="text/javascript" src="../UITemplet/js/jquery-1.10.2.min.js"></script>
<script type="text/javascript" src="../UITemplet/js/jquery-ui-1.10.4.min.js"></script>
<script type="text/javascript" src="../UITemplet/js/respond.js" ></script>
<script type="text/javascript" src="../UITemplet/js/jquery.sidebar.js" ></script>
<script type="text/javascript" src="../UITemplet/js/bootstrap.js"></script>
<script type="text/javascript" src="../UITemplet/js/bootstrap-switch.js"></script>
<script type="text/javascript" src="../UITemplet/echarts/js/esl.js"></script>
<script type="text/javascript" src="../UITemplet/echarts/js/codemirror.js"></script>
<script type="text/javascript" src="../UITemplet/echarts/js/javascript.js"></script>
<script type="text/javascript" src="../UITemplet/js/AeroWindow-Contextmenu.js"></script>
<%--<script type="text/javascript" src="../UITemplet/js/jquery.textSearch-1.0.js"></script>--%>
<script type="text/javascript" src="../UITemplet/js/jquery.grumble.min.js"></script>

<link href="../UITemplet/css/bootstrap.css" media="all" rel="stylesheet" type="text/css" />
<link href="../UITemplet/css/font-awesome.css" media="all" rel="stylesheet" type="text/css" />
<link href="../UITemplet/css/bootstrap-switch.css" media="all" rel="stylesheet" type="text/css" />
<link href="../UITemplet/css/index.css" media="all" rel="stylesheet" type="text/css" />
<link href="../UITemplet/css/sidebar.css" media="all" rel="stylesheet" type="text/css" />
<link href="../UITemplet/echarts/css/monokai.css" rel="stylesheet" type="text/css" />
<link href="../UITemplet/echarts/css/codemirror.css" rel="stylesheet" type="text/css" />
<link href="../UITemplet/js/artDialog/skins/blue.css" rel="stylesheet" type="text/css" />
<link href="../UITemplet/css/AeroWindow-Contextmenu.css" rel="stylesheet">
<link href="../UITemplet/css/grumble.min.css" media="all" rel="stylesheet" type="text/css" />

<style>
table.zonglan tr td
{
    vertical-align:middle;
    text-align:center;
    height:15px;
    padding:5px;
}
table.zonglan tr td input
{
    vertical-align:middle;
    float:center;
    margin-bottom:4px;
}
table.zonglan tr td .data
{
    vertical-align:middle;
    text-align:center;
}
table.zonglan tr td.data span
{
    cursor:pointer;
}
    
table.zonglan tr td span.mlabel
{
   font-size:12px;
   color:Gray;
   font-weight:normal;
}
</style>
<script type="text/javascript">
    function ChangeDateFormat(time) {
        if (time != null) {
            var date = new Date(parseInt(time.replace("/Date(", "").replace(")/", ""), 10));
            var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
            var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
            var hour = date.getHours() < 10 ? "0" + date.getHours() : date.getHours();
            var minute = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();
            var second = date.getSeconds() < 10 ? "0" + date.getSeconds() : date.getSeconds(); 
            return month + "-" + currentDate + " " +hour + ":" + minute + ":" + second;
        }
        return "";
    }
        
</script>
  
<script type="text/javascript">
    var cur_ap_sort = { faild: "Similarity", sort_idx: 1 };
    var ap_sort_faild = {
        ap_sort_SSID: "G_SSID",
        ap_sort_Similarity: "Similarity",
        ap_sort_Strength: "G_STRONG",
        ap_sort_Channel: "CHANNEL",
        ap_sort_Time: "G_TIME",
        ap_sort_firstTime:"FIRSTTIME"
    };
    var all_user_sort = {};
    var user_sort_faild = {
        user_sort_connecttime: "ConnectTime",
        user_sort_onlinetime: "OnLineTime",
        user_sort_onlinecounts: "OnLineCounts",
        user_sort_usedtraffic: "UsedTraffic",
        user_sort_onlinetype: "OnLineType"
    };
    var SORT = ["", "desc", "asc"];
    var filtermod = 0;
    var is_ap_loading = true;


    var expand_tr,
        expand_tr_next,
        is_show_flow = false,
        is_bottom_flow = false;

    var token = GetQueryString("token"); //读取token
    var url_oid = GetQueryString("oid");
    url_oid = url_oid == null ? 0 : url_oid;

    var APList = [];
    var keywords = null;
    // interval ID
    var intervalid;
    // 获取AP信息的间隔时间 单位s
    var refreshInterval = 60;
    var displayMode = "apMode";
    //读取token
    var curLat;
    var curLon;
    var iconStates = { on: true, off: true };
    var SSIDList = [];

    $(document).ready(function () {
        InitSignalCharts();
        initRealSwitch();
        HighLightMenu("安全");
        getAPList(url_oid);
        getAPNearCount(url_oid);
        bindClick();
        whitelistDialog();
        blacklistDialog();
        alertSetting();
        getOrgList();
        openRefresh(true);
        $("[name='my_checkbox']").bootstrapSwitch();
        addSwitchEvent();
        $('#warning_alert').find("[name='close']").click(function () { $('#warning_alert').fadeOut(); });

        $("ul#demo_menu2").sidebar({
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
//            $(window).scroll(function () {
//                setFlowDivPostion();
//            });

        // ap右键菜单
        $("tr[name='ap']").WinContextMenu({ contextMenuID: '#apContextMenu', action:
            function (e, target) {
                switch (e.id) {
                    case "menu_add_keyword":
                        dialog_blacklist.iframe.src = 'blacklistDialog.aspx?token=' + token + '&addkeyword=' + encodeURI($(target).find("td[name='tdSSID']").text().trim());
                        dialog_blacklist.show();
                        dialog_blacklist._showFrame();
                        break;
                    case "menu_add_whitelist":
                        dialog_whitelist.iframe.src = 'whitelistDialog.aspx?token=' + token + '&addmac=' + $(target).find("td[name='tdMAC']").text().trim();
                        dialog_whitelist.show();
                        dialog_whitelist._showFrame();
                        break;
                    case "others":
                        alert("设备" + $(target).find('input[name="apid"]').val() + "重启");
                        break;
                }
            }
        });

        // 内部右键菜单
        $("tr[name='user']").WinContextMenu({ contextMenuID: '#apContextMenu', action:
            function (e, target) {
                switch (e.id) {
                    case "menu_add_keyword":
                        dialog_blacklist.iframe.src = 'blacklistDialog.aspx?token=' + token + '&addkeyword=' + encodeURI($(target).find("td[name='tdSSID']").text().trim());
                        dialog_blacklist.show();
                        dialog_blacklist._showFrame();
                        break;
                    case "menu_add_whitelist":
                        dialog_whitelist.iframe.src = 'whitelistDialog.aspx?token=' + token + '&addmac=' + $(target).find("td[name='tdMAC']").text().trim();
                        dialog_whitelist.show();
                        dialog_whitelist._showFrame();
                        break;
                    case "others":
                        alert("设备" + $(target).find('input[name="apid"]').val() + "重启");
                        break;


                    //case "detail":
                    //    $(target).find('input[name="apid"]').val();
                    //    break;
                    //case "update_ad":
                    //    alert("设备" + $(target).find('input[name="apid"]').val() + "更新广告");
                    //    break;
                    //case "reboot":
                    //    alert("设备" + $(target).find('input[name="apid"]').val() + "重启");
                    //    break;
                    //case "update_rom":
                    //    alert("设备" + $(target).find('input[name="apid"]').val() + "更新rom");
                    //    break;
                    //case "others":
                    //    alert("设备" + $(target).find('input[name="apid"]').val() + "其他");
                    //    break;
                }
            }
        });

        // 绑定AP排序事件
        $('table[name="dTable"]').find("th").click(function () {
            if (this.id == null || this.id == "")
                return true;
            if (!is_ap_loading) {
                is_ap_loading = true;
                var h_i = $('table[name="dTable"]').find("th i").removeClass().addClass("fa fa-sort");
                if (cur_ap_sort.faild == ap_sort_faild[this.id]) {
                    if (cur_ap_sort.sort_idx == 2)
                        cur_ap_sort.sort_idx = 0;
                    else
                        cur_ap_sort.sort_idx++;
                } else {
                    cur_ap_sort.faild = ap_sort_faild[this.id];
                    cur_ap_sort.sort_idx = 1;
                }
                $(this).find("i").removeClass().addClass("fa fa-sort" + (cur_ap_sort.sort_idx != 0 ? "-" : "") + SORT[cur_ap_sort.sort_idx]);

                getAPList($('#org_slt').children('option:selected').val());
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

    function setFilterMod(mod) {
        var cur_oid = $('#org_slt').children('option:selected').val();
        filtermod = mod;
        getAPList(cur_oid);
    }

    function FilterSSIDList(mod) {
      var arr = new Array();
      if (typeof (mod) == "object") {
        if ($(mod).attr("mod") == "all")
          $("input[name='optionsCheckbox']").prop("checked", $("input[mod='all']").prop("checked"));
        else
          $("input[mod='all']").prop("checked", false);
        arr = GetCheckboxVal();
      }
      else {
        $("input[name='optionsCheckbox']").each(function () {
          if ($(this).val() == mod)
            $(this).prop("checked", true);
          else
            $(this).prop("checked", false);
        });
        arr.push(mod);
      }
      InitSSIDList(arr);
      if ($('#org_slt').children('option:selected').val() != 0)
        $('[name="table_ap_hidden"]').hide();
      else
        $('[name="table_ap_hidden"]').show();
    }

    function GetCheckboxVal() {
        var result = new Array();
        $("input[name='optionsCheckbox']:checked").each(function () { result.push($(this).val()); });
        return result;
    }

    function isCon(arr, val) {
        for (var j = 0; j < val.split(',').length; j++) {
            for (var i = 0; i < arr.length; i++) {
                if (arr[i] == val.split(',')[j])
                    return true;
            }
        }
        return false;
    }

    function getAPList(_oid) {
        
        $("#DbdyList").empty();
        $("#DbdyList").html("<tr><td colspan='9'>正在加载...</td></tr>");
        if (filtermod == null || filtermod=='') filtermod = "0";

        $.ajax({
            type: 'post',
            url: 'AjaxComm.aspx',
            data: 'type=GetFilterSSIDInfoByAPID&token=' + token + '&page=1&size=80&param={"filtermod":"' + filtermod + '","ordercul":"' + cur_ap_sort.faild + '","sortstr":"' + SORT[cur_ap_sort.sort_idx] + '","apid":"' + _oid + '","isRealTime":' + $('#realSwitch').bootstrapSwitch('state') + '}',
            dataType: 'json',
            error: function (msg) {
                is_ap_loading = false;
                //alert("网络错误");
                //window.location.href = "login.aspx";
            },
            success: function (obj) {
                is_ap_loading = false;
                if (obj.ResultCode == "0") {
                    SSIDList = obj.ResultOBJ.ssidList
                    InitSSIDList(GetCheckboxVal());

                    if (_oid != 0)
                        $('[name="table_ap_hidden"]').hide();
                    else
                        $('[name="table_ap_hidden"]').show();
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

    function InitSSIDList(filterSSID) {
        $("#DbdyList").empty();
        if (SSIDList.length > 0) {
            $('#scan_time').html(dateFormat(SSIDList[0].G_TIME, "yyyy-MM-dd hh:mm:ss"));
            var item;
            var tmp_ssid = "";
            var tmp_keyword = "";
            for (var i = 0; i < SSIDList.length; i++) {
                item = SSIDList[i];
                if (!$("input[mod='all']").prop("checked"))
                    if (!isCon(filterSSID, item.LevelFlag))
                        continue;
                if (filtermod == 0) {
                    if (item.LevelFlag.split(',')[0] == 1)
                        result = "<tr id='apTr" + i + "' name='ap' class='danger' style='cursor:pointer'" + item.ADDRESS + "\n'>";
                    else if (item.LevelFlag.split(',')[0] == 2)
                        result = "<tr id='apTr" + i + "' name='ap' class='success' style='cursor:pointer'" + item.ADDRESS + "\n'>";
                    else if (item.LevelFlag.split(',')[0] == 3)
                        result = "<tr id='apTr" + i + "' name='ap' class='info' style='cursor:pointer'" + item.ADDRESS + "\n'>";
                    else if (item.LevelFlag.split(',')[0] == 4)
                        result = "<tr id='apTr" + i + "' name='ap' style='cursor:pointer;background:#eee'" + item.ADDRESS + "\n'>";
                    else if (item.LevelFlag.split(',')[0] == 5)
                        result = "<tr id='apTr" + i + "' name='ap' class='warning' style='cursor:pointer'" + item.ADDRESS + "\n'>";
                    else
                        result = "<tr id='apTr" + i + "' name='ap' style='cursor:pointer'" + item.ADDRESS + "\n'>";
                } else {
                    result = "<tr id='apTr" + i + "' name='ap' class='" + getTrColor(filtermod) + "' style='cursor:pointer'" + item.ADDRESS + "\n'>";
                }
                //result += "<td style='width:180px;'><input name='apid' type='hidden' value='" + i + "' mac='" + item.G_SSID + "' /><i class='fa fa-chevron-circle-right'></i>  " + item.CapturerName + "</td>";
                result += "<td name='table_ap_hidden'>" + item.CapturerName + "</td>";
                result += "<td name='tdSSID'><input name='apid' type='hidden' value='" + i + "' apid='"+item.APID+"' mac='" + item.G_SSID + "' />";

                if (item.G_SSID == "") {
                    result += "(隐藏)";
                } else {
                    tmp_ssid = item.G_SSID;
                    if (item.KEYWORD != null) {
                        var t = item.KEYWORD.split(',');
                        for (var j = 0; j < t.length; j++) {
                            var re = new RegExp(t[j], "i");
                            tmp_ssid = tmp_ssid.replace(re, "<span style='color:red'>" + re.exec(tmp_ssid) + "</span>");
                        }
                    }
                    result += tmp_ssid;
                }

                result += "<span name='group_count' class='pull-right badge'>" + (item.GroupCount > 1 ? item.GroupCount : "") + "</span></td>";

                result += "<td name='tdMAC'>" + item.G_MAC + "</td>";
                result += "<td>" + item.G_STRONG + " dbm</td>";
                result += "<td>" + item.CHANNEL + "</td>";
                result += "<td>" + ChangeDateFormat(item.FirstTime) + "</td>";
                result += "<td>" + ChangeDateFormat(item.G_TIME) + "</td>";
                result += "<td>" + (item.Similarity * 100).toFixed(2) + "%</td>";
                result += "</tr>";
                result += "<tr id='mTr" + i + "'><td style='padding:0px' colspan='7'><div name='mDiv' style='margin:15px'></div></td></tr>";
                $("#DbdyList").append(result);

            }
            if ($("#DbdyList").html() == "")
                $("#DbdyList").html("<tr><td colspan='9'>没有相关的SSID...</td></tr>");
            else
                dTableBindEvent();
            
            $("tr[id^='mTr']").hide();

        } else {
            $("#DbdyList").html("<tr><td colspan='9'>没有相关的SSID...</td></tr>");
            $('#scan_time').html(new Date().format('yyyy-MM-dd hh:mm:ss'));
        }
    }


    function getTrColor(_filterMode) {
        var tmp = "";
        switch (_filterMode) {
            case 1:
                tmp = "danger";
                break;
            case 2:
                tmp = "success";
                break;
            case 3:
                tmp = "info";
                break;
            case 4:
                tmp = "";
                break;
            case 5:
                tmp = "warning";
                break;
        }
        return tmp;
    }

    function dTableBindEvent() {
        var tbList = $("[name='dTable']");
        tbList.each(function () {
            var self = this;

            $("tr[name='ap']", $(self)).click(function () {
                var trThis = this;

                if ($(trThis).find("span[name='group_count']").html() == "")
                    return true;

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
                    //setFlowDivPostion();

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

//        function getKeywordList(_dom,_type) {
//            $.ajax({
//                type: 'post',
//                url: 'AjaxComm.aspx',
//                data: 'type=GetAlertKeyWord&token=' + token,
//                dataType: 'json',
//                error: function (msg) {
//                    alert("服务器错误");
//                },
//                success: function (obj) {
//                    if (obj.ResultCode == 0) {
//                        keywords = "";
//                        for (var i = 0; i < obj.ResultOBJ.length; i++) {
//                            keywords += obj.ResultOBJ[i].KEYWORD + " ";
//                        }
//                        setKeywordHighlight(_dom, _type);
//                    }
//                }
//            });
//        }
//        function setKeywordHighlight(_dom, _type) {
//            if (keywords == null) {
//                getKeywordList(_dom, _type);
//            } else {
//                if (_type == "ap") {
//                    $(_dom).textSearch(keywords, { callback: function () { dTableBindEvent(); } });
//                }
//                else {
//                    $(_dom).textSearch(keywords);
//                }
//            }
//        }

    function getUserList(_did, _mac) {
        
        if ($("#mTr" + _did).find("div[name='mDiv']").html() != "")
            return;
        var apid = $("#apTr" + _did).find('[name="apid"]').attr('apid');
        $("#mTr" + _did).find("div[name='mDiv']").html("加载中...");
        var uTable = $("#uTable").clone();
        $(uTable).removeAttr("id");
        $.ajax({
            type: 'post',
            url: 'AjaxComm.aspx',
            data: 'type=GetSameSSIDInfo&token=' + token + '&param={"Name":"' + _mac + '","ID":"' + apid + '"}',
            dataType: 'json',
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                is_user_loading = false;
                //                    alert(XMLHttpRequest.status);
                //                    alert(XMLHttpRequest.readyState);
                //                    alert(textStatus);
                //                    alert(errorThrown);
                //window.location.href = "login.aspx";
            },
            success: function (obj) {
                is_user_loading = false;
                if (obj.ResultCode == "0") {
                    //                            alert(obj.ResultOBJ.length);
                    if (obj.ResultOBJ.length > 0) {
                        var item;
                        var tmp_ssid = "";
                        for (var i = 0; i < obj.ResultOBJ.length; i++) {
                            item = obj.ResultOBJ[i];
                            result = "<tr name='user'>";
                            result += "<td name='tdSSID'><input name='apid' type='hidden' value='" + i + "' apid='" + apid + "' ssid='" + item.G_SSID + "' />";
                            if (item.G_SSID == "") {
                                result += "(隐藏)";
                            } else {
                                tmp_ssid = item.G_SSID;
                                if (item.KEYWORD != null) {
                                    var t = item.KEYWORD.split(',');
                                    for (var j = 0; j < t.length; j++) {
                                        var re = new RegExp(t[j], "i");
                                        tmp_ssid = tmp_ssid.replace(re, "<span style='color:red'>" + re.exec(tmp_ssid) + "</span>");
                                    }
                                }
                                result += tmp_ssid;
                            }
                            result += "</td>";
                            result += "<td name='tdMAC'>" + item.G_MAC + "</td>";
                            result += "<td>" + item.G_STRONG + " dbm</td>";
                            result += "<td>" + (item.KEYWORD == null ? "" : item.KEYWORD) + "</td>";
                            result += "<td>" + ChangeDateFormat(item.G_TIME) + "</td>";
                            result += "</tr>";
                            $(uTable).find("tbody").append(result);
                        }
                        $(uTable).show();
                        $("#mTr" + _did).find("div[name='mDiv']").html(uTable);
                        //setKeywordHighlight($("#mTr" + _did).find("div[name='mDiv']"), "user");

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

    function openRefresh(flag) {
        if (flag) {
            refreshTime = 60;
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
        $('#progressbar-sec').width((refreshInterval - refreshTime) / refreshInterval * 100 + "%");
        $('#progressbar-sec').find("span[name='sec']").html("(" + refreshTime + "秒)");

        if (refreshTime <= 0) {
            getAPList($('#org_slt').children('option:selected').val());
            getAPNearCount($('#org_slt').children('option:selected').val())
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
            updateWiFiView(APList, displayMode);
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
            updateWiFiView(APList, displayMode);
        });
    }

    function whitelistDialog() {
        dialog_whitelist = art.dialog.open('whitelistDialog.aspx?token=' + token, { title: '白名单',
            padding: '0px',
            opacity: 0.2,
            top: '25%',
            left: '25%',
            width: '500px',
            height: '300px',
            fixed: true,
            resize: false,
            show: false,
            close: function () {
                this.hide();
                return false;
            }
        });
    }

    function blacklistDialog() {
        dialog_blacklist = art.dialog.open('blacklistDialog.aspx?token=' + token, { title: '可疑关键词',
            padding: '0px',
            opacity: 0.2,
            top: '25%',
            left: '25%',
            width: '500px',
            height: '300px',
            fixed: true,
            resize: false,
            show: false,
            close: function () {
                this.hide();
                return false;
            }
        });
    }

    function alertSetting() {
        dialog_alertsetting = art.dialog.open('alarmsite.aspx?token=' + token, { title: '告警设置',
            padding: '0px',
            opacity: 0.2,
            top: '25%',
            left: '25%',
            width: '280px',
            height: '390px',
            fixed: true,
            resize: true,
            resize: false,
            show: false,
            close: function () {
                this.hide();
                return false;
            }
        });
    }

    function showBlacklist() {
        dialog_blacklist.show();
    }

    function showWhitelist() {
        dialog_whitelist.show();
    }

    function showAlertSetting() {
        dialog_alertsetting.show();
    }

    function getOrgList() {
        $('#org_slt').html("");
        $('#org_slt').append("<option value='0'>全部</option>");
        $.ajax({
            type: 'post',
            url: 'AjaxComm.aspx',
            data: 'type=GetAPListForState&token=' + token,
            dataType: 'json',
            error: function (msg) {
                //alert("网络错误");
                //window.location.href = "login.aspx";
            },
            success: function (obj) {
                if (obj.ResultCode == "0") {

                    for (var i = 0; i < obj.ResultOBJ.APList.length; i++)
                        $('#org_slt').append("<option value='" + obj.ResultOBJ.APList[i].ID + "'>" + obj.ResultOBJ.APList[i].ALIAS + "</option>");
                    if (url_oid != 0)
                        $('#org_slt').val(url_oid);

                    $('#org_slt').change(function () {
                        getAPList($(this).children('option:selected').val());
                        getAPNearCount($(this).children('option:selected').val());
                    });
                    $('#progressbar-sec').find("span[name='apcount']").html("正在为您监测<span style='font-weight:bold; color:red;'>" + obj.ResultOBJ.APList.length + "</span>个营业厅");
                    //if (obj.ResultOBJ.length > 1)
                    //    $('#org_div').fadeIn();
                }
                else {
                    if (obj.ResultCode == -100) {
                        window.location.href = "login.aspx";
                    }
                }
            }
        });
    }

    function getAPNearCount(_oid) {
        //if(_oid == 0)
        //    $('#label_all').html("全部可疑");
        //else
        //    $('#label_all').html("全部");
        //
        //$('#label_warning').html("可疑");
        //$('#label_believe').html("可信");
        //$('#label_chinese').html("中文");
        //$('#label_new').html("新增");
        //$('#label_rival').html("同业");
        $('span[name="zl_sy"]').html("...");
        $('span[name="zl_ky"]').html("...");
        $('span[name="zl_kx"]').html("...");
        $('span[name="zl_zw"]').html("...");
        $('span[name="zl_xz"]').html("...");
        $('span[name="zl_ty"]').html("...");
        $.ajax({
            type: 'post',
            url: 'AjaxComm.aspx',
            data: 'type=GetAPNearCountByAPID&token=' + token + '&param={"apid":"' + _oid + '","isRealTime":' + $('#realSwitch').bootstrapSwitch('state') + '}',
            dataType: 'json',
            error: function (msg) {
                //alert("网络错误");
                //window.location.href = "login.aspx";
            },
            success: function (obj) {
                if (obj.ResultCode == "0") {
                    if (_oid == 0)
                        $('#label_all').html("全部可疑(" + obj.ResultOBJ[0] + ")");
                    else
                        $('#label_all').html("全部 (" + obj.ResultOBJ[0] + ")");
                    $('#label_warning').html("可疑 (" + obj.ResultOBJ[1] + ")");
                    $('#label_believe').html("可信 (" + obj.ResultOBJ[2] + ")");
                    $('#label_chinese').html("中文 (" + obj.ResultOBJ[3] + ")");
                    $('#label_new').html("新增 (" + obj.ResultOBJ[4] + ")");
                    $('#label_rival').html("同业 (" + obj.ResultOBJ[5] + ")");

                    $('span[name="zl_sy"]').html(obj.ResultOBJ[0]);
                    $('span[name="zl_ky"]').html(obj.ResultOBJ[1]);
                    $('span[name="zl_kx"]').html(obj.ResultOBJ[2]);
                    $('span[name="zl_zw"]').html(obj.ResultOBJ[3]);
                    $('span[name="zl_xz"]').html(obj.ResultOBJ[4]);
                    $('span[name="zl_ty"]').html(obj.ResultOBJ[5]);

                    $('#tc_all').html(0);
                    $('#tc_ds').html(0);
                    $('#tc_zw').html(0);
                    $('#tc_ky').html(0);
                    $('#tc_kx').html(0);
                    updateTcData($('#tc_all'), obj.ResultOBJ[0]);
                    updateTcData($('#tc_ky'), obj.ResultOBJ[1]);
                    updateTcData($('#tc_kx'), obj.ResultOBJ[2]);
                    updateTcData($('#tc_zw'), obj.ResultOBJ[3]);
                    updateTcData($('#tc_ds'), obj.ResultOBJ[5]);
                }
                else {
                    if (obj.ResultCode == -100) {
                        window.location.href = "login.aspx";
                    }
                }
            }
        });
    }

    function getScanAPNearCount(_oid) {
        $.ajax({
            type: 'post',
            url: 'AjaxComm.aspx',
            data: 'type=GetAPNearCountByAPID&token=' + token + '&param={"apid":"' + _oid + '","isRealTime":true}',
            dataType: 'json',
            error: function (msg) {
                //alert("网络错误");
                //window.location.href = "login.aspx";
            },
            success: function (obj) {
                if (obj.ResultCode == "0") {
                    $('#tc_all').html(0);
                    $('#tc_ds').html(0);
                    $('#tc_zw').html(0);
                    $('#tc_ky').html(0);
                    $('#tc_kx').html(0);
                    updateTcData($('#tc_all'), obj.ResultOBJ[0]);
                    updateTcData($('#tc_ky'), obj.ResultOBJ[1]);
                    updateTcData($('#tc_kx'), obj.ResultOBJ[2]);
                    updateTcData($('#tc_zw'), obj.ResultOBJ[3]);
                    updateTcData($('#tc_ds'), obj.ResultOBJ[5]);
                }
                else {
                    if (obj.ResultCode == -100) {
                        window.location.href = "login.aspx";
                    }
                }
            }
        });
    }

    function updateTcData(_dom, _data) {
        var tmp = parseInt($(_dom).html());
        $('#progressbar-radar').html();
        $('#progressbar-radar').width(Math.round(tmp / _data * 100) + "%");
        $('#progressbar-radar').find("span").html(Math.round(tmp / _data * 100) + "%");
        if (tmp < _data) {
            $(_dom).html(++tmp);
            setTimeout(function () { updateTcData(_dom, _data) }, 10);
        } else {
            _dom.attr("id") == "tc_all" && $('#progressbar-radar-header').hide();
        }
    }

    function setWarningInfo(_warningInfo) {
        
        if (_warningInfo.length > 0) {
            var content = "<strong style='font-size:16px;'>危险！ 还有<span name='count'>" + _warningInfo.length + "</span>条未处理</strong><table style='margin-top:10px;'>";
            for (var i = 0; i < _warningInfo.length; i++) {
                content += "<tr name='info_" + _warningInfo[i].LOG_ID + "' style='height:23px'><td>在<strong><span style='color:red'>" + _warningInfo[i].APNAME + "</span></strong></td><td>附近发现名为</td><td><strong><span style='color:red;'>" + _warningInfo[i].G_SSID + "</span></strong></td><td>的可疑SSID，请问是<a href='javascript:void(0);' onclick='notice(" + _warningInfo[i].LOG_ID + ",\"" + _warningInfo[i].APNAME + "\")' class='alert-link'>通知</a>还是加入<a href='javascript:void(0);' onclick='addWhite(" + _warningInfo[i].LOG_ID + ",\"" + _warningInfo[i].G_SSID + "\")'  class='alert-link'>白名单</a>？</td></tr>";
            }
            content += "</table>";
            $('#warning_alert').find("[name='content']").html(content);
            $('#warning_alert').show("normal");
        }
    }

    function notice(_log_id ,_oname) {
        if (confirm("确定通知到【" + _oname + "】的联系人吗？")) {
            $.ajax({
                type: 'post',
                url: 'AjaxComm.aspx',
                data: 'type=ProcessForNotice&token=' + token + '&param=' + _log_id,
                dataType: 'json',
                error: function (msg) {
                    //alert("网络错误");
                    //window.location.href = "login.aspx";
                },
                success: function (obj) {
                    if (obj.ResultCode == "0") {
                        $('#warning_alert').find("[name='info_" + _log_id + "']").remove();
                        var count = parseFloat($('#warning_alert').find("[name='count']").html()) - 1;
                        if (count == 0)
                            $('#warning_alert').fadeOut();
                        else
                            $('#warning_alert').find("[name='count']").html(count);
                        alert("处理成功");
                    } else if (obj.ResultCode == -100) {
                        window.location.href = "login.aspx";
                    } else {
                        alert(obj.ResultMsg);
                    }
                }
            });
        }
    }

    function addWhite(_log_id, _ssid) {
        if (confirm("确定将【" + _ssid + "】加入白名单吗？")) {
            $.ajax({
                type: 'post',
                url: 'AjaxComm.aspx',
                data: 'type=ProcessForWhiteList&token=' + token + '&param=' + _log_id,
                dataType: 'json',
                error: function (msg) {
                    //alert("网络错误");
                    //window.location.href = "login.aspx";
                },
                success: function (obj) {
                    if (obj.ResultCode == "0") {

                        $('#warning_alert').find("[name='info_" + _log_id + "']").remove();
                        var count = parseFloat($('#warning_alert').find("[name='count']").html()) - 1;
                        if (count == 0)
                            $('#warning_alert').fadeOut();
                        else
                            $('#warning_alert').find("[name='count']").html(count);
                        alert("处理成功");
                    } else if (obj.ResultCode == -100) {
                        window.location.href = "login.aspx";
                    } else {
                        alert(obj.ResultMsg);
                    }
                }
            });
        }
    }

    function initRealSwitch() {
        $('#realSwitch').bootstrapSwitch();
        $('#realSwitch').on('switchChange.bootstrapSwitch', function (event, state) {
            getAPList($('#org_slt').children('option:selected').val());
            getAPNearCount($('#org_slt').children('option:selected').val());
            InitSignalCharts();
        })
    }

    
    function RefreshAPList() {
        var oid = $('#org_slt').children('option:selected').val();
        getAPList(oid);
        getAPNearCount(oid);
    }
</script>
<script type="text/javascript">
    var echarts;
    var options = [];
    var dataStyle = {
        normal: {
            label: {
                show: true,
                position: 'inside',
                formatter: '{a}'
            }
        }
    };
    var dataStyle1 = {
        normal: {
            label: {
                show: true,
                position: 'inside',
                formatter: '{a}'
            }
        }
    };

    var dialog_blacklist;
    var dialog_whitelist;
    var dialog_alertsetting;

    function InitSignalCharts() {
        $.ajax({
            type: 'post', //可选get
            url: 'AjaxComm.aspx', //这里是接收数据的PHP程序
            data: 'type=GetWarnGraph&token=' + token +"&param="+$('#realSwitch').bootstrapSwitch("state"),
            dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
            success: function (obj) {
                var tbStr = "";
                if (obj.ResultOBJ.length > 0) {
                    optionSignalDistribution.series = new Array();
                    optionSignalChannel.series = new Array();
                }
                var item = null;
                for (var i = 0; i < obj.ResultOBJ.length; i++) {
                    var objItem = obj.ResultOBJ[i];
                    item = new SeriesItem();
                    item.name = objItem.G_SSID;
                    item.itemStyle = dataStyle1;
                    item.type = "scatter";
                    item.data.push(GetPoint(objItem.G_STRONG));
                    optionSignalDistribution.series.push(item);
                    optionSignalDistribution.series.push(new SeriesMarkLine());

                    item = new SeriesItem();
                    item.name = objItem.G_SSID;
                    item.type = "scatter";
                    item.data.push([objItem.CHANNEL, objItem.G_STRONG]);
                    optionSignalChannel.series.push(item);
                }

                refreshAll();
            }
        });
    }

    function GetPoint(strong) {
        var point = [0, 0, 0];
        var rad = Math.random();
        point[0] = Math.cos(Math.PI * rad * 2) * Math.abs(strong);
        point[1] = Math.sin(Math.PI * rad * 2) * Math.abs(strong);
        point[2] = strong;
        return point;
    }

    function SeriesItem() {
        var result = new Object();
        result.name = "";
        result.type = "";
        result.itemStyle = dataStyle;
        result.data = new Array();
        return result;
    }

    function SeriesMarkLine() {
        var result = new Object();
        result.name = "";
        result.type = "scatter";
        result.data = [[-100, -100], [100, 100]];
        result.itemStyle = {
            normal: {
                color: "rgba(0,0,0,0)"
            }
        };
        result.markLine = {
            itemStyle: {
                normal: {
                    lineStyle: {
                        width: 1,
                        color: "rgba(255,255,255,1)"
                }
            }
        },
        data: [
                [{ xAxis: -80, yAxis: 0 }, { xAxis: 80, yAxis: 0}],
                [{ xAxis: 0, yAxis: -80 }, { xAxis: 0, yAxis: 80}]
            ]
    };
        return result;
    }

    function initOption() {
        options.push(optionSignalDistribution);
        options.push(optionSignalChannel);
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
</script>
</head>

<body>
<uc1:header ID="Header1" runat="server" />

<div class="container-fluid">
<div class="row-fluid">
    <div class="div-body col-md-8">
        <div class="panel panle_ssid" style="float:right; width:100%; height:980px; overflow-y:auto">
        <div class="progress progress-striped active" style="margin:0px;height:18px">
          <div id="progressbar-sec" class="progress-bar progress-bar-success" role="progressbar" aria-valuemin="0" aria-valuemax="100" style="width: 0%">
                <span name="apcount"></span><span name="sec">50</span>
          </div>
        </div>
            
            <div class="row-fluid">
                <div class="div-body col-md-12">
                    <div id="warning_alert" class="alert alert-danger alert-dismissable" style="margin:0px; display:none;">
                      <button name="close" type="button" class="close">&times;</button>
                      <div name="content">
                      </div>
                    </div>
                </div>
            </div>

            <div class="row-fluid">
                <div class="div-body col-md-12">
                    <div style="margin:10px 10px;">
                        <span class="label label-primary" style="font-size:14px;">总览</span>
                        <span style="margin-left:15px;"><input id="realSwitch" type="checkbox" data-size="mini" data-on-text="实时" data-off-text="历史" data-on-color="success" data-off-color="danger" checked/></span>
                        <table class="hor-minimalist-a table table-bordered table-condensed zonglan" style="margin-top:10px;">
                        <tr>
                            <td colspan="3" style="text-align:left;">
                                <span class="label" style="color:Gray">扫描范围：</span>
                                <select id="org_slt" style="height:25px;width:200px; padding:3px 6px"></select>
                            </td>
                            <td colspan="3" style="text-align:right">扫描时间：<span id="scan_time">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span></td>
                        </tr>
                        <%--<tr class="danger">
                            <td><span class="label mlabel">危险的SSID</span></td><td class="data"><span name="zl_ky" class="label label-danger" onclick="setFilterMod(1);">...</span> <input type="radio" name="optionsRadios" value="warning" onclick="setFilterMod(1);" /></td></tr>
                        <tr class="success">
                            <td><span class="label mlabel">信任的SSID</span></td><td class="data"><span name="zl_kx" class="label label-success" onclick="setFilterMod(2);">...</span> <input type="radio" name="optionsRadios" value="option2" onclick="setFilterMod(2);" /></td></tr>
                        <tr class="info">
                            <td><span class="label mlabel">中文的SSID</span></td><td class="data">   <span name="zl_zw" class="label label-info" onclick="setFilterMod(3);">...</span> <input type="radio" name="optionsRadios" value="option3" onclick="setFilterMod(3);" /></td></tr>
                        <tr class="warning">
                            <td><span class="label mlabel">同业的SSID</span></td><td class="data"> <span name="zl_ty" class="label label-warning" onclick="setFilterMod(5);">...</span> <input type="radio" name="optionsRadios" value="option3" onclick="setFilterMod(5);" /></td></tr>
                        <tr>
                            <td><span class="label mlabel">新增的SSID</span></td><td class="data"><span name="zl_xz" class="label mlabel" onclick="setFilterMod(4);">...</span><input type="radio" name="optionsRadios" onclick="setFilterMod(4);" /></td></tr>
                        <tr>
                            <td><span class="label mlabel">所有的SSID</span></td><td class="data"><span name="zl_sy" class="label mlabel" onclick="setFilterMod(0);">...</span><input type="radio" name="optionsRadios" onclick="setFilterMod(0);" /></td></tr>--%>
                        <%--<tr>
                            <td style="background:#eee"><input type="radio" name="optionsRadios" value="warning" onclick="setFilterMod(0);" /><br /><span class="label mlabel">全部的SSID</span></td>
                            <td class="danger"><input type="radio" name="optionsRadios" value="warning" onclick="setFilterMod(1);" /><br /><span class="label mlabel">可疑的SSID</span></td>
                            <td class="success"><input type="radio" name="optionsRadios" value="warning" onclick="setFilterMod(2);" /><br /><span class="label mlabel">信任的SSID</span></td>
                            <td class="info"><input type="radio" name="optionsRadios" value="warning" onclick="setFilterMod(3);" /><br /><span class="label mlabel">中文的SSID</span></td>
                            <td class="warning"><input type="radio" name="optionsRadios" value="warning" onclick="setFilterMod(5);" /><br /><span class="label mlabel">同业的SSID</span></td>
                            <td><input type="radio" name="optionsRadios" value="warning" onclick="setFilterMod(4);" /><br /><span class="label mlabel">新增的SSID</span></td>
                        </tr>--%>
                        <tr>
                            <td><input type="checkbox" name="optionsCheckbox" mod="all" value="0" onclick="FilterSSIDList(this);" checked /><br /><span class="label mlabel">全部的SSID</span></td>
                            <td class="danger"><input type="checkbox" name="optionsCheckbox" value="1" onclick="FilterSSIDList(this);" checked /><br /><span class="label mlabel">可疑的SSID</span></td>
                            <td class="success"><input type="checkbox" name="optionsCheckbox" value="2" onclick="FilterSSIDList(this);" checked /><br /><span class="label mlabel">信任的SSID</span></td>
                            <td class="info"><input type="checkbox" name="optionsCheckbox" value="3" onclick="FilterSSIDList(this);" checked /><br /><span class="label mlabel">中文的SSID</span></td>
                            <td class="warning"><input type="checkbox" name="optionsCheckbox" value="5" onclick="FilterSSIDList(this);" checked /><br /><span class="label mlabel">同业的SSID</span></td>
                            <td style="background:#eee"><input type="checkbox" name="optionsCheckbox" value="4" onclick="FilterSSIDList(this);" checked /><br /><span class="label mlabel">新增的SSID</span></td>
                        </tr>
                        <tr>
                            <td class="data"><span name="zl_sy" class="label mlabel" onclick="FilterSSIDList(0);">...</span></td>
                            <td class="data danger"><span name="zl_ky" class="label label-danger" onclick="FilterSSIDList(1);">...</span></td>
                            <td class="data success"><span name="zl_kx" class="label label-success" onclick="FilterSSIDList(2);">...</span></td>
                            <td class="data info"><span name="zl_zw" class="label label-info" onclick="FilterSSIDList(3);">...</span></td>
                            <td class="data warning"><span name="zl_ty" class="label label-warning" onclick="FilterSSIDList(5);">...</span></td>
                            <td class="data" style="background:#eee"><span name="zl_xz" class="label mlabel" style=" background:#ddd" onclick="FilterSSIDList(4);">...</span></td>
                        </tr>
                        <%--<tr>
                            <td class="danger"><span class="label mlabel">危险的SSID</span></td><td class="data danger"><span name="zl_ky" class="label label-danger" onclick="setFilterMod(1);">...</span> <input type="radio" name="optionsRadios" value="warning" onclick="setFilterMod(1);" /></td>
                            <td class="success"><span class="label mlabel">信任的SSID</span></td><td class="data success"><span name="zl_kx" class="label label-success" onclick="setFilterMod(2);">...</span> <input type="radio" name="optionsRadios" value="option2" onclick="setFilterMod(2);" /></td></tr>
                        <tr class="info">
                            <td class="info"><span class="label mlabel">中文的SSID</span></td><td class="data info">   <span name="zl_zw" class="label label-info" onclick="setFilterMod(3);">...</span> <input type="radio" name="optionsRadios" value="option3" onclick="setFilterMod(3);" /></td>
                            <td class="warning"><span class="label mlabel">同业的SSID</span></td><td class="data warning"> <span name="zl_ty" class="label label-warning" onclick="setFilterMod(5);">...</span> <input type="radio" name="optionsRadios" value="option3" onclick="setFilterMod(5);" /></td></tr>
                        <tr>
                            <td><span class="label mlabel">新增的SSID</span></td><td class="data"><span name="zl_xz" class="label mlabel" onclick="setFilterMod(4);">...</span><input type="radio" name="optionsRadios" onclick="setFilterMod(4);" /></td>
                            <td><span class="label mlabel">所有的SSID</span></td><td class="data"><span name="zl_sy" class="label mlabel" onclick="setFilterMod(0);">...</span><input type="radio" name="optionsRadios" onclick="setFilterMod(0);" /></td></tr>--%>
                        </table>
                    </div>
                </div>
            </div>
            
            <div class="panel-heading"><!-- Split button -->
                
                <span class="label label-primary" style="font-size:14px">分营业厅</span>
                <span class="label label-success" style="font-size:14px;cursor:pointer" onclick="RefreshAPList();">刷新</span>
                
                

                
                <%--<label class="checkbox-inline pull-right">
                  <input type="radio" name="optionsRadios" id="optionsRadios4" value="option3" onclick="setFilterMod(4);" /><span id="label_new">新增</span>
                </label>
                <label class="checkbox-inline pull-right">
                  <input type="radio" name="optionsRadios" id="optionsRadios5" value="option3" onclick="setFilterMod(5);" /><span id="label_rival" class="label label-danger">同业</span>
                </label>
                <label class="checkbox-inline pull-right">
                  <input type="radio" name="optionsRadios" id="optionsRadios3" value="option3" onclick="setFilterMod(3);" /><span id="label_chinese" class="label label-info">中文</span> 
                </label>
                <label class="checkbox-inline pull-right">
                  <input type="radio" name="optionsRadios" id="optionsRadios2" value="option2" onclick="setFilterMod(2);" /><span id="label_believe" class="label label-success">可信</span>
                </label>
                <label class="checkbox-inline pull-right">
                <input type="radio" name="optionsRadios" id="optionsRadios1" value="warning" onclick="setFilterMod(1);" /><span id="label_warning" class="label label-warning">可疑</span>
                </label>
                <label class="checkbox-inline pull-right">
                  <input type="radio" name="optionsRadios" id="Radio1" value="option3" checked onclick="setFilterMod(0);" /><span id="label_all">全部</span>
                </label>--%>

                <%--<span>

                    <input id="search_txt" type="search" class="input-sm" placeholder="查询关键字.."/>
                    <a id="search_btn" class="btn btn-sm btn-success" href="javascript:void(0);"><i class="icon-twitter"></i>查询</a>

                </span>--%>
            </div>
            <div class="widget-content padded clearfix">
                <table class="hor-minimalist-a table table-bordered table-hover" name="dTable" >
                <thead>
                    <tr>
                    <%--<th scope="col" style="width:180px">Capturer</th>--%>
                    <th name="table_ap_hidden" scope="col" style="cursor:pointer;" class="btn-default" >营业厅</th>
                    <th scope="col" style="cursor:pointer;" class="btn-default" id="ap_sort_SSID">SSID <i class="fa fa-sort"></i></th>
                    <th scope="col">BSSID</th>
                    <th scope="col" style="cursor:pointer;" class="btn-default" id="ap_sort_Strength">信号强度 <i class="fa fa-sort-desc"></i></th>
                    <th scope="col" style="cursor:pointer;" class="btn-default" id="ap_sort_Channel">信道 <i class="fa fa-sort"></i></th>
                    <th scope="col" style="cursor:pointer;" class="btn-default" id="ap_sort_firstTime">首次发现时间 <i class="fa fa-sort"></i></th>
                    <th scope="col" style="cursor:pointer;" class="btn-default" id="ap_sort_Time">最后发现时间 <i class="fa fa-sort"></i></th>
                    <th scope="col" style="cursor:pointer;" class="btn-default" id="ap_sort_Similarity">相似度 <i class="fa fa-sort"></i></th>
                    <%--<th scope="col">关键字词</th>--%>
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
                <span id="Span3" class="label label-primary" style="font-size:14px">安全探测</span>
            </div>
            <div class="widget-content padded clearfix">
                <div style="height: 220">
                    <div style="float:left;">
                        <img src="../UITemplet/img/radar.gif" height="150" width="150" style="margin:10px 0px 0px 10px"/>
                        <div id="progressbar-radar-header" class="progress progress-striped active" style="margin:20px 10px 20px 20px;">
                          <div id="progressbar-radar" class="progress-bar progress-bar-info" role="progressbar" aria-valuemin="0" aria-valuemax="100" style="width: 0%">
                          </div>
                        </div>
                        <span></span>
                        <%--<span id="pc_radar" style="color: white;font-size: 20px;position: relative;left: -75px;top: 5px;font-weight: bold;"></span>--%>
                    </div>
                        
                    <div style="float:left;margin:20px 0px 40px 20px">
                        <table style="text-align:left;color:Green;" >
                            <tr><th colspan="2" style="font-size:14px;letter-spacing: 8px;">周边SSID</th></tr>
                            <tr><td id="tc_all" colspan="2" style="font-size:3.4em;">Loading</td></tr>

                            <tr style="height:25px"><td style="width:120px;">可疑的SSID</td><td id="tc_ky" style=" cursor:pointer;" onclick="FilterSSIDList(1);">...</td></tr>
                            <tr style="height:25px"><td style="width:120px;">安全的SSID</td><td id="tc_kx" style=" cursor:pointer;" onclick="FilterSSIDList(2);">...</td></tr>
                            <tr style="height:25px"><td style="">同业观察</td><td id="tc_ds" style=" cursor:pointer;" onclick="FilterSSIDList(5);" >...</td></tr>
                            <tr style="height:25px"><td style="">中文的SSID</td><td id="tc_zw" style=" cursor:pointer;" onclick="FilterSSIDList(3);">...</td></tr>
                        </table>
                    </div>

                </div>
            </div>
        </div>
        </div>
        <div class="row-fluid"></div>
        </div>
        <div class="row-fluid">
        <div class="div-body col-md-12">
        <div class="panel panle_ssid">
            <div class="panel-heading"><!-- Split button -->
                <span id="Span1" class="label label-primary" style="font-size:14px">信号分布</span>
            </div>
            <div class="widget-content padded clearfix">
                <div id="divSignalDistributionBody" md="main" style="height: 300px"></div>
                <span md="wrong-message" style="color: red"></span>
                <script type="text/javascript">
                    optionSignalDistribution = {
                        tooltip: {
                            trigger: 'item',
                            formatter: function (value) {
                                if (value[2][0] && value[2][0])
                                    return value[0] + '<br/>' + '信号强度: ' + value[2][2] + 'dBm';
                                else
                                    return "中心线";
                            }
                        },
                        backgroundColor: "rgba(46,139,87,1)",
                        grid: {
                            x2: 30,
                            y2: 30,
                            x: 30,
                            y: 30
                        },
                        xAxis: [
                            {
                                type: 'value',
                                power: 1,
                                axisLine: false,
                                splitLine: false,
                                axisLabel: {
                                    show: false
                                },
                                min: -80,
                                max: 80,
                                //scale: true,
                                data: (function () {
                                    var res = [];
                                    var len = 13;
                                    while (1 < len--)
                                        res.unshift(len);
                                    return res;
                                })()
                            }
                        ],
                        yAxis: [
                            {
                                type: 'value',
                                power: 1,
                                precision: 2,
                                min: -80,
                                max: 80,
                                //scale: true,
                                axisLabel: {
                                    show: false,
                                    formatter: '{value} dBm'
                                },
                                axisLine: false,
                                splitLine: false,
                                splitArea: { show: false }
                            }
                        ],
                        series: [
                            {
                                name: '',
                                type: 'scatter',
                                itemStyle: dataStyle,
                                data: [[0, 0]]
                            }
                        ]
                    };
                </script>
            </div>
        </div>
        </div>
        <div class="row-fluid"></div>
        </div>
        <div class="row-fluid">
            <div class="div-body col-md-12">
            <div class="panel panle_ssid">
                <div class="panel-heading"><!-- Split button -->
                    <span id="Span2" class="label label-primary" style="font-size:14px">信号频道</span>
                </div>
                <div class="widget-content padded clearfix">
                   <div id="divSignalChannelBody" md="main" style="height: 300px"></div>
                    <span md="wrong-message" style="color: red"></span>
                    <script type="text/javascript">
                    optionSignalChannel = {
                        tooltip: {
                            trigger: 'item',
                            formatter: function (value) {
                                return value[0] + '<br/>' + value[2][0] + '信道 : ' + value[2][1] + 'dBm';
                            }
                        },
                        grid: {
                            x2: 60,
                            y2: 30,
                            x: 75,
                            y: 30
                        },
                        xAxis: [
                            {
                                type: 'value',
                                power: 1,
                                scale: true
                            }
                        ],
                        yAxis: [
                            {
                                type: 'value',
                                power: 1,
                                precision: 2,
                                scale: true,
                                axisLabel: {
                                    formatter: '{value} dBm'
                                },
                                splitArea: { show: true }
                            }
                        ],
                        series: [
                            {
                                name: '',
                                type: 'scatter',
                                itemStyle: dataStyle,
                                data: [[0, 0]]
                            }
                        ]
                    };
                </script>
                </div>
            </div>
            </div>
            <div class="row-fluid"></div>
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
 <%--onclick="javascript:displayChangeClick(); onclick="javascript:openRefresh();"  --%>
 <ul id="demo_menu2" class="nav nav-pills nav-stacked">
    <li ><a id="auto_refresh_btn" href="#" >自动刷新<span id="refreshTime" class="badge"></span><span class="pull-right"><input id="refresh_chk" name="my_checkbox" type="checkbox" data-size="imini" data-on-text="开" data-off-text="关" data-on-color="success" data-off-color="danger" checked/></span></a></li>
        <li class="divider"></li>
<li ><a id="a1" href="#" onclick="showWhitelist();" >白名单</a></li>
        <li class="divider"></li>
    <li ><a id="a2" href="#" onclick="showBlacklist();">可疑关键词管理</a></li>
        <li class="divider"></li>
    <li ><a id="a3" href="#" onclick="showAlertSetting();">报警设置</a></li>
        <li class="divider"></li>
</ul>

<div id="apContextMenu" class="WincontextMenu" style="top:100px; left:230px; display:none;">
  <li><a id="menu_add_keyword" href="#"><img src="../UITemplet/css/contextmenu/icons/Copy.png"><span>添加可疑关键字</span></a></li>
  <div class="m-split"></div>
  <li><a id="menu_add_whitelist" href="#"><img src="../UITemplet/css/contextmenu/icons/New.png"><span>加入白名单</span></a></li>
  <div class="m-split"></div>
  <li><a id="others" href="#" class="cmDisable"><img src="../UITemplet/css/contextmenu/icons/Pinion.png"><span>其他</span></a></li>
</div>
<script src="../UITemplet/echarts/js/echartsExample.js" type="text/javascript"></script>
</body>
</html>
