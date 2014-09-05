<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="state.aspx.cs" Inherits="LUOBO.SingleShop.UI.state" %>
<%@ Register src="header.ascx" tagname="header" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
 <head runat="server">
    <title>next-wifi 状态</title>
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
                ap_sort_total: "networktotal",
                ap_sort_rate: "networkrate",
                ap_sort_olpeople: "onlinepeoplenum"
            },
            user_sort_faild = {
                user_sort_linecounts: "LineCounts",
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
            HighLightMenu("状态");
            bindClick();
            getAPList();
        });

        function bindClick() {
            // 搜素按钮
            $('#search_btn').click(function () {
                if (!is_ap_loading) {
                    is_ap_loading = true;
                    cur_ap_sort = { faild: "", sort_idx: 0 };
                    all_user_sort = {};
                    var h_i = $('table[name="dTable"]').find("th i").removeClass().addClass("fa fa-sort");
                    getAPList();
                } else {
                    $(this).grumble({
                        text: '正在查询<br />请稍候',
                        angle: 90,
                        distance: 30,
                        type: 'alt-',
                        hideAfter: 500
                    });
                }
            });

            // 浮动Div
            $(window).scroll(function () {
                setFlowDivPostion();
            });

            // ap右键菜单
            $("tr[name='ap']").WinContextMenu({ contextMenuID: '#apContextMenu', action:
                function (e, target) {
                    switch (e.id) {
                        case "detail":
                            window.open("/ui/statisticalAP.aspx?token=" + token + "&apid=" + $(target).find('input[name="apid"]').val());
                            break;
                        case "update_ad":
                            alert("设备" + $(target).find('input[name="apid"]').val() + "更新广告");
                            break;
                        case "reboot":
                            alert("设备" + $(target).find('input[name="apid"]').val() + "重启");
                            break;
                        case "update_rom":
                            alert("设备" + $(target).find('input[name="apid"]').val() + "更新rom");
                            break;
                        case "others":
                            alert("设备" + $(target).find('input[name="apid"]').val() + "其他");
                            break;
                    }
                }
            });
            // user右键菜单
            $("tr[name='user']").WinContextMenu({ contextMenuID: '#userContextMenu', action:
                function (e, target) {
                    switch (e.id) {
                        case "more":
                            window.open("/ui/userstate.aspx?token=" + token + "&mac=" + $(target).find('input[name="mac"]').val());
                            break;
                        case "black_list":

                            break;
                        case "white_list":

                            break;

                        case "others":

                            break;
                    }
                }
            });

            // 绑定AP排序事件
            $('table[name="dTable"]').find("th").click(function () {
                if (this.id == null || this.id == "")
                    return false;
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

                    getAPList();
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


            // 绑定User排序事件
            $(document).on("click", 'table[name="uTable"] th', function () {
                if (this.getAttribute("name") == null || this.getAttribute("name") == "")
                    return false;
                if (!is_user_loading) {
                    is_user_loading = true;
                    
                    var h_i = $(expand_tr).next().find('table[name="uTable"]').find("th i").removeClass().addClass("fa fa-sort");
                    if (all_user_sort[expand_tr.id].faild == user_sort_faild[this.getAttribute("name")]) {
                        if (all_user_sort[expand_tr.id].sort_idx == 2)
                            all_user_sort[expand_tr.id].sort_idx = 0;
                        else
                            all_user_sort[expand_tr.id].sort_idx++;
                    } else {
                        all_user_sort[expand_tr.id].faild = user_sort_faild[this.getAttribute("name")];
                        all_user_sort[expand_tr.id].sort_idx = 1;
                    }
                    $(this).find("i").removeClass().addClass("fa fa-sort" + (all_user_sort[expand_tr.id].sort_idx != 0 ? "-" : "") + SORT[all_user_sort[expand_tr.id].sort_idx]);
                    getUserListForSort($(expand_tr).find("[name='apid']").val(), $(expand_tr).find("[name='apid']").attr("mac"));   
                    
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

        function getAPList() {
            $("#DbdyList").empty
            $("#DbdyList").html("<tr><td colspan='9'>正在加载...</td></tr>");
            $.ajax({
                type: 'post',
                url: 'AjaxComm.aspx',
                data: 'type=GetAPListForState&token=' + token + '&param={"NAME":"' + $('#search_txt').val() + '","COLUMN":"' + cur_ap_sort.faild + '","ORDERBY":"' + SORT[cur_ap_sort.sort_idx] + '"}',
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
                        if (obj.ResultOBJ.APList.length > 0) {
                            var item;
                            obj.ResultOBJ.ap_bad_num = 0;
                            for (var i = 0; i < obj.ResultOBJ.APList.length; i++) {
                                item = obj.ResultOBJ.APList[i];
                                if (!getState(item.LASTHB, item.HBINTERVAL)) {
                                    result = "<tr id='apTr" + item.ID + "' name='ap' class='warning' style='cursor:pointer;'" + item.ADDRESS + "\n'>";
                                    obj.ResultOBJ.ap_bad_num++;
                                }
                                else {
                                    result = "<tr id='apTr" + item.ID + "' name='ap' class='success' style='cursor:pointer'" + item.ADDRESS + "\n'>";
                                }

                                result += "<td style='width:180px;'><input name='apid' type='hidden' value='" + item.ID + "' mac='" + item.MAC + "' /><i class='fa fa-chevron-circle-right'></i>  " + item.ALIAS + "</td>";
                                result += "<td>" + GetTimeCompany(item.POWERTIME) + "</td>";
                                result += "<td>" + (item.MEMFREE == null ? "未知" : item.MEMFREE) + "</td>";
                                result += "<td>" + getUsedTraffic(item.NETWORKTOTAL) + "</td>";
                                result += "<td>" + (item.NETWORKRATE / 1024).toFixed(2) + "kb/s</td>";
                                result += "<td>" + item.ONLINEPEOPLENUM + "</td>";
                                result += "</tr>";

                                result += "<tr id='mTr" + item.ID + "'><td style='padding:0px' colspan='7'><div name='mDiv' style='margin:15px'></div></td></tr>";
                                $("#DbdyList").append(result);
                            }

                            dTableBindEvent();
                            $("tr[id^='mTr']").hide();

                            $('#head_avg_sc').html("平均开机时长：0");
                            $('#head_avg_rc').html("平均访问人次：" + obj.ResultOBJ.AvgVisitNum);
                            $('#head_ap_bad').html("设备故障：" + obj.ResultOBJ.ap_bad_num);
                            $('#head_ap_total').html("设备总量：" + obj.ResultOBJ.APList.length);
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

        function getState(_lasthb, _interval) {
            if (jsonToDate(_lasthb) > new Date(new Date() - (_interval * 1000 * 2)))
                return true;
            return false;
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
                data: 'type=GetUserForState&token=' + token + '&param={"APMAC":"' + _mac + '","COLUMN":"' + all_user_sort["apTr" + apid].faild + '","ORDERBY":"' + SORT[all_user_sort["apTr" + apid].sort_idx] + '"}',
                dataType: 'json',
                error: function (msg) {
                    is_user_loading = false;
                    //alert("网络错误");
                    //window.location.href = "login.aspx";
                },
                success: function (obj) {
                    is_user_loading = false;
                    if (obj.ResultCode == "0") {
                        if (obj.ResultOBJ.UserList.length > 0) {
                            var item;
                            for (var i = 0; i < obj.ResultOBJ.UserList.length; i++) {
                                item = obj.ResultOBJ.UserList[i];
                                result = "<tr name='user' style='cursor:pointer'>";
                                result += "<td>" + item.MAC.substring(0, 12) + "**-**<input name='mac' type='hidden' value='" + item.MAC + "' /><span class='pull-right badge'>" + (item.LineCounts > 1 ? item.LineCounts : "") + "</span></td>";
                                result += "<td>" + dateFormat(item.ConnectTime, "yyyy-MM-dd hh:mm") + "</td>";
                                result += "<td>" + (item.OnLineTime / 60).toFixed(2) + "分钟</td>";
                                result += "<td>" + item.OnLineCounts + "</td>";
                                result += "<td>" + getUsedTraffic(item.UsedTraffic) + "</td>";
                                result += "<td>" + getOlineType(item.OnLineType) + "</td>";
                                result += "</tr>";
                                $(uTable).find("tbody").append(result);
                            }
                            $(uTable).show();
                            $("#mTr" + _did).find("div[name='mDiv']").html(uTable);
                            $("#mTr" + _did).find("div[name='mDiv']").prepend("<div name='uTitle' style='padding-bottom:15px;padding-left:10px;'><span id='sUserTitle" + _did + "' class='label label-success' style='font-size:14px'>客户信息</span><span class='pull-right label label-success' style='font-size:14px;margin-right:10px'>平均访问时长：" + obj.ResultOBJ.AvgVisitTime + "s</span><span class='pull-right label label-success' style='font-size:14px;margin-right:10px'>平均访问次数：" + obj.ResultOBJ.AvgVisitNum + "</span><span class='pull-right label label-success' style='font-size:14px;margin-right:10px'>认证用户数：" + obj.ResultOBJ.RZUserNum + "</span><span class='pull-right label label-success' style='font-size:14px;margin-right:10px'>在线用户总数：" + obj.ResultOBJ.OnlinePeopleNum + "</span></div>");

                            testScroll(_did, _mac);

                            $("#sUserTitle" + _did).html("客户信息（共" + obj.ResultOBJ.AllPeopleCount + "位）<input id='uTrCount" + _did + "' type='hidden' value='" + obj.ResultOBJ.AllPeopleCount + "' /><input id='uTrPage" + _did + "' type='hidden' value='1' />");
                        } else {
                            $("#mTr" + _did).find("div[name='mDiv']").html("没有用户数据");
                        }
                    }
                    else {
                        alert(obj.ResultMsg);
                    }
                }
            });
        }

        function getUserListForSort(_did, _mac) {
            apid = _did;


            var uBody = $("#mTr" + _did).find("table[name='uTable'] tbody");
            $(uBody).html('加载中...');

            $.ajax({
                type: 'post',
                url: 'AjaxComm.aspx',
                data: 'type=GetUserForState&token=' + token + '&param={"APMAC":"' + _mac + '","COLUMN":"' + all_user_sort["apTr" + apid].faild + '","ORDERBY":"' + SORT[all_user_sort["apTr" + apid].sort_idx] + '"}',
                dataType: 'json',
                error: function (msg) {
                    is_user_loading = false;
                    //alert("网络错误");
                    //window.location.href = "login.aspx";
                },
                success: function (obj) {
                    is_user_loading = false;
                    $(uBody).html("");
                    if (obj.ResultCode == "0") {
                        if (obj.ResultOBJ.UserList.length > 0) {
                            var item;
                            for (var i = 0; i < obj.ResultOBJ.UserList.length; i++) {
                                item = obj.ResultOBJ.UserList[i];
                                result = "<tr name='user' style='cursor:pointer'>";
                                result += "<td>" + item.MAC.substring(0, 12) + "**-**<input name='mac' type='hidden' value='" + item.MAC + "' /><span class='pull-right badge'>" + (item.LineCounts > 1 ? item.LineCounts : "") + "</span></td>";
                                result += "<td>" + dateFormat(item.ConnectTime, "yyyy-MM-dd hh:mm") + "</td>";
                                result += "<td>" + (item.OnLineTime / 60).toFixed(2) + "分钟</td>";
                                result += "<td>" + item.OnLineCounts + "</td>";
                                result += "<td>" + getUsedTraffic(item.UsedTraffic) + "</td>";
                                result += "<td>" + getOlineType(item.OnLineType) + "</td>";
                                result += "</tr>";
                                uBody.append(result);
                            }
                            //$(uTable).show();
                            //$("#mTr" + _did).find("div[name='mDiv']").html(uTable);
                            //$("#mTr" + _did).find("div[name='mDiv']").prepend("<div style='padding-bottom:15px;padding-left:10px;'><span id='sUserTitle" + _did + "' class='label label-success' style='font-size:14px'>客户信息</span></div>");

                            testScroll(_did, _mac);

                            //$("#sUserTitle" + _did).html("客户信息（共" + obj.ResultOBJ.AllPeopleCount + "位）<input id='uTrCount" + _did + "' type='hidden' value='" + obj.ResultOBJ.AllPeopleCount + "' /><input id='uTrPage" + _did + "' type='hidden' value='1' />");
                        } else {
                            $("#mTr" + _did).find("div[name='mDiv']").html("没有用户数据");
                        }
                    }
                    else {
                        alert(obj.ResultMsg);
                    }
                }
            });
        }


        function getOlineType(_id) {
            if (_id.indexOf("-1,") > -1)
                _id = _id.replaceAll('-1,', '');
            if (_id.indexOf(",-1") > -1)
                _id = _id.replaceAll(',-1', '');
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
  </script>
  <script type="text/javascript">
      var is_user_loading = false;
      function testScroll(_did, _mac) {
          $(window).scroll(function () {
              if (parseInt($("#uTrCount" + _did).val()) <= $("#mTr" + _did + " table tbody tr").length)
                  return;
              if (is_user_loading)
                  return;
              if ($("#mTr" + _did + " table").height() > 0) {
                  if ($(document).scrollTop() > ($("#mTr" + _did + " table").offset().top + $("#mTr" + _did + " table").height() - $(window).height())) {
                      $("#mTr" + _did + " [name='mDiv']").append('<div id="divLoding" class="progress progress-striped active" style="width: 33.33333333333333%; margin-left: 33.33333333333333%;"><div class="progress-bar"  role="progressbar" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100" style="width: 100%"><span class="sr-only"></span></div></div>');
                      is_user_loading = true;
                      GetNextUserList(_did, _mac);
                  }
              }
          });
      }

      function GetNextUserList(_did, _mac) {
          var result = "";
          $("#uTrPage" + _did).val(parseInt($("#uTrPage" + _did).val()) + 1);
          $.ajax({
              type: 'post',
              url: 'AjaxComm.aspx',
              data: 'type=GetUserForState&token=' + token + '&param={"APMAC":"' + _mac + '","COLUMN":"' + all_user_sort["apTr" + _did].faild + '","ORDERBY":"' + SORT[all_user_sort["apTr" + _did].sort_idx] + '","CurrentPage":' + $("#uTrPage" + _did).val() + '}',
              dataType: 'json',
              error: function (msg) {
                  //alert("网络错误");
                  //window.location.href = "login.aspx";
              },
              success: function (obj) {
                  if (obj.ResultCode == "0") {
                      if (obj.ResultOBJ.UserList.length > 0) {
                          var item;
                          for (var i = 0; i < obj.ResultOBJ.UserList.length; i++) {
                              item = obj.ResultOBJ.UserList[i];
                              result += "<tr name='user' style='cursor:pointer'>";
                              result += "<td>" + item.MAC.substring(0, 12) + "**-**<input name='mac' type='hidden' value='" + item.MAC + "' /><span class='pull-right badge'>" + (item.LineCounts > 1 ? item.LineCounts : "") + "</span></td>";
                              result += "<td>" + dateFormat(item.ConnectTime, "yyyy-MM-dd hh:mm") + "</td>";
                              result += "<td>" + (item.OnLineTime / 60).toFixed(2) + "分钟</td>";
                              result += "<td>" + item.OnLineCounts + "</td>";
                              result += "<td>" + getUsedTraffic(item.UsedTraffic) + "</td>";
                              result += "<td>" + getOlineType(item.OnLineType) + "</td>";
                              result += "</tr>";
                          }

                          $("#divLoding").remove();
                          $("#mTr" + _did + " table tbody").append(result);
                          is_user_loading = false;
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
<uc1:header runat="server" />

<div class="container-fluid">
<div class="row-fluid">
    <div class="div-body col-md-12">
        <div class="panel panle_ssid">
            <div class="panel-heading"><!-- Split button -->
                <span id="sAPTitle" class="label label-success" style="font-size:14px">设备状态</span>
                <span>
                    <input id="search_txt" type="search" class="input-sm" placeholder="查询关键字.."/>
                    <a id="search_btn" class="btn btn-sm btn-success" href="javascript:void(0);"><i class="icon-twitter"></i>查询</a>
                </span>
                <span id="head_avg_sc" class="pull-right label label-success" style="font-size:14px;margin-right:10px">平均开机时长：</span>
                <span id="head_avg_rc" class="pull-right label label-success" style="font-size:14px;margin-right:10px">平均访问人次：</span>
                <span id="head_ap_bad" class="pull-right label label-warning" style="font-size:14px;margin-right:10px">设备故障：</span>
                <span id="head_ap_total" class="pull-right label label-success" style="font-size:14px;margin-right:10px">设备总量：</span>
            </div>
            <div class="widget-content padded clearfix">
                <table class="hor-minimalist-a table table-bordered table-hover" name="dTable" >
                <thead>
                    <tr>
                    <th scope="col" style="width:180px">设备名称</th>
                    <th id="ap_sort_powertime" class="btn-default" scope="col" style="cursor:pointer;">开机时长 <i class="fa fa-sort"></i></th>
                    <th id="ap_sort_memfree" class="btn-default" scope="col" style="cursor:pointer;">内存负载 <i class="fa fa-sort"></i></th>
                    <th id="ap_sort_total" class="btn-default" scope="col" style="cursor:pointer;">总流量 <i class="fa fa-sort"></i></th>
                    <th id="ap_sort_rate" class="btn-default" scope="col" style="cursor:pointer;">实时速率 <i class="fa fa-sort"></i></th>
                    <th id="ap_sort_olpeople" class="btn-default" scope="col" style="cursor:pointer;">在线人数 <i class="fa fa-sort"></i></th>
                    <%--<th class='hidden-xs' scope="col">状态</th>--%>
                    <%--<th class='hidden-xs'">操作</th>--%>
                    </tr>
                </thead>
                <tbody id="DbdyList" >
                </tbody>
                </table>
            </div>
        </div>
    </div>
</div>
</div>

<table style="display:none" class="hor-minimalist-a table table-bordered table-hover" id="uTable" name="uTable">
    <thead>
        <tr>
        <th name="user_sort_linecounts" class="btn-default" scope="col" style="width:170px;cursor:pointer;">客户MAC <i class="fa fa-sort"></i></th>
        <th name="user_sort_connecttime" class="btn-default" scope="col" style="cursor:pointer;">连接时间 <i class="fa fa-sort"></i></th>
        <th name="user_sort_onlinetime" class="btn-default" scope="col" style="cursor:pointer;">上网时长 <i class="fa fa-sort"></i></th>
        <th name="user_sort_onlinecounts" class="btn-default" scope="col" style="cursor:pointer;">上网次数 <i class="fa fa-sort"></i></th>
        <th name="user_sort_usedtraffic" class="btn-default" scope="col" style="cursor:pointer;">使用流量 <i class="fa fa-sort"></i></th>
        <th name="user_sort_onlinetype" class="btn-default" scope="col" style="cursor:pointer;">认证方式 <i class="fa fa-sort"></i></th>
        </tr>
    </thead>
    <tbody>
    </tbody>
</table>

<div id="apContextMenu" class="WincontextMenu" style="top:100px; left:230px; display:none;">
  <li><a id="detail" href="#"><img src="../UITemplet/css/contextmenu/icons/Copy.png"><span>查询详细状态</span></a></li>
  <div class="m-split"></div>
  <li><a id="update_ad" href="#" class="cmDisable"><img src="../UITemplet/css/contextmenu/icons/New.png"><span>更改广告设定</span></a></li>
  <li><a id="reboot" href="#" class="cmDisable"><img src="../UITemplet/css/contextmenu/icons/Save.png"><span>重启</span></a></li>
  <%--<li><a id="update_rom" href="#"><img src="../UITemplet/css/contextmenu/icons/table.png"><span>更新Rom</span></a></li>--%>
  <div class="m-split"></div>
  <li><a id="others" href="#" class="cmDisable"><img src="../UITemplet/css/contextmenu/icons/Pinion.png"><span>其他</span></a></li>
</div>

<div id="userContextMenu" class="WincontextMenu" style="top:100px; left:230px; display:none;">
  <li><a id="more" href="#"><img src="../UITemplet/css/contextmenu/icons/Save.png"><span>更多信息</span></a></li>
  <div class="m-split"></div>
  <li><a id="black_list" href="#" class="cmDisable"><img src="../UITemplet/css/contextmenu/icons/Copy.png"><span>加入黑名单</span></a></li>
  <li><a id="white_list" href="#" class="cmDisable"><img src="../UITemplet/css/contextmenu/icons/New.png"><span>加入白名单</span></a></li>
  <div class="m-split"></div>
  <li><a id="others" href="#" class="cmDisable"><img src="../UITemplet/css/contextmenu/icons/table.png"><span>其他</span></a></li>
</div>

<div id="flow_div" style="position:absolute;width:100%;display:none;padding:0px 10px 0px 10px;">
    <table class="hor-minimalist-a table table-bordered table-hover" style="-webkit-box-shadow: 0px 0px 10px #000000;box-shadow: 0px 0px 10px #000000;float:left;"></table>
</div>

<div id="flow_to_top" style="position:fixed; bottom:50px; right:40px; display:none;"><a href="javascript:gotoTop();" class="btn"><i class="fa fa-arrow-up fa-3x"></i></a></div>
</body>
</html>
