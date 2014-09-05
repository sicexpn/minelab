<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="control.aspx.cs" Inherits="LUOBO.SingleShop.UI.control" ValidateRequest="false"%>
<%@ Register src="header.ascx" tagname="header" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
    <head>
    <title>next-wifi 控制</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" charset="utf-8"/>
    <link href="../UITemplet/css/bootstrap.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/css/font-awesome.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/css/index.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/css/style.css" media="all" rel="stylesheet" type="text/css" />
    <script src="../UITemplet/js/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../UITemplet/js/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script type="text/javascript" src="../UITemplet/js/Common.js"></script>
    <script src="../UITemplet/js/bootstrap.js" type="text/javascript"></script>
    <script src="../UITemplet/js/login.js" type="text/javascript"></script>
    <script src="../UITemplet/js/respond.js" type="text/javascript"></script>
    <link rel="stylesheet" href="../UITemplet/kindeditor/themes/default/default.css" />
	<link rel="stylesheet" href="../UITemplet/kindeditor/plugins/code/prettify.css" />
	<script charset="utf-8" src="../UITemplet/kindeditor/kindeditor.js"></script>
	<script charset="utf-8" src="../UITemplet/kindeditor/lang/zh_CN.js"></script>
	<script charset="utf-8" src="../UITemplet/kindeditor/plugins/code/prettify.js"></script>
    <style>
        .changeable
        {
            position:absolute; 
         }
    </style>
    <script type="text/javascript" language="javascript">

        function SetUrlRefresh(url) {
            if (url.indexOf("?") > 0)
                return url + "&t=" + (new Date().getTime());
            else
                return url + "?t=" + (new Date().getTime());
        }

        function trim(str) {
            if (str == null)
                return "";
            else
                return str.replace(/(^\s*)|(\s*$)/g, "");
        }

        jQuery.fn.anchorGoWhere = function (options) {
            var obj = jQuery(this);
            var defaults = { target: 0, timer: 500 };
            var o = jQuery.extend(defaults, options);
            obj.each(function (i) {
                jQuery(obj[i]).click(function () {
                    switch (o.target) {
                        case 1:
                            var _rel = jQuery(this).attr("href").substr(1);
                            var _targetTop = jQuery("#" + _rel).offset().top;
                            jQuery("html,body").animate({ scrollTop: _targetTop }, o.timer);
                            break;
                        case 2:
                            var _rel = jQuery(this).attr("href").substr(1);
                            var _targetLeft = jQuery("#" + _rel).offset().left;
                            jQuery("html,body").animate({ scrollLeft: _targetLeft }, o.timer);
                            break;
                        case 3:
                            var _rel_c = jQuery(this).attr("class").substr(3,7);
                            var _targetTop = jQuery("#" + _rel_c).offset().top;
                            jQuery("html,body").animate({ scrollTop: _targetTop }, o.timer);
                            break;
                    }
                    return false;
                });
            });
        };
    </script>

    <script>
        $(document).ready(function () {
            HighLightMenu("控制");
            //折叠窗口
            var icons = {
                header: "arrow-e",
                activeHeader: "arrow-s"
            };
            $("#accordion").accordion({
                icons: icons
            });

            //wifi列表样式
            $(".ssid-item").click(function () {
                $(".ssid-item").removeClass("active");
                $(this).addClass("active");
            });
            //移动版跳转窗口
            $(".goTop").anchorGoWhere({ target: 1 });
            $(".goDown").anchorGoWhere({ target: 1 });
            $(".goNext").anchorGoWhere({ target: 1 });
            $(".goFront").anchorGoWhere({ target: 1 });
            $(".goVertical").anchorGoWhere({ target: 2 });
            $(".go-ad-view").anchorGoWhere({ target: 3 });
            $(".go-ad-edit").anchorGoWhere({ target: 3 });

            //切换窗口
            var windowWidth = $(document).width();

            $("#list-view-btn").click(function () {
                if (thisSSID == null) {
                    alert("请选择要编辑的SSID");
                    return;
                }
                windowWidth = $(document).width();
                if ($("#ad-edit").hasClass("hidden")) {
                    GetAdInfo(thisSSID.ADID);
                    $("#ad-list").animate({ marginLeft: (-windowWidth / 2) }, 1000);
                    $("#ad-edit").animate({ marginLeft: (windowWidth / 2) }, 1000);
                    $("#ad-view").animate({ marginLeft: "0px" }, 1000);
                    $("#ad-edit").removeClass("hidden");
                    setTimeout(function () {
                        $("#ad-list").addClass("hidden");
                    }, 1000);
                    $("#list-view-btn").html("<i class='fa fa-fw fa-list'></i>返回列表<p class='disabled hidden-xs pull-left' style='font-size:28px; color:#d9534f'><i class='fa fa-fw fa-arrow-circle-left fa-dongtai'></i></p>");
                }
                else {
                    $("#ad-list").animate({ marginLeft: "0px" }, 1000);
                    $("#ad-edit").animate({ marginLeft: windowWidth }, 1000);
                    $("#ad-view").animate({ marginLeft: (windowWidth / 2) }, 1000);
                    $("#ad-list").removeClass("hidden");
                    $("#list-view-btn").html("<i class='fa fa-fw fa-pencil'></i>编辑<p class='disabled hidden-xs pull-right' style='font-size:28px; color:#d9534f'><i class='fa fa-fw fa-arrow-circle-right fa-dongtai'></i></p>");
                    setTimeout(function () {
                        $("#ad-edit").addClass("hidden");
                    }, 1000);
                    $("#PreviwPage").attr("src", SetUrlRefresh(thisSSID.PATH + thisSSID.PORTAL));
                }
            });
            $("#ad-view").css("margin-left", windowWidth / 2);
            $("#ad-edit").css("margin-left", windowWidth);
            $("#ad-edit").addClass("hidden");
        });

        var resizeTimeout;
        var tempWindowWidth;
        window.onresize = function () {
            clearTimeout(resizeTimeout);
            resizeTimeout = setTimeout(function () {
                tempWindowWidth = $(document.body).width();
                if ($("#ad-edit").hasClass("hidden")) {
                    $("#ad-list").animate({ marginLeft: "0px" }, "slow");
                    $("#ad-edit").animate({ marginLeft: tempWindowWidth }, "slow");
                    $("#ad-view").animate({ marginLeft: (tempWindowWidth / 2) }, "slow");
                }
                else {
                    $("#ad-list").animate({ marginLeft: (-tempWindowWidth / 2) }, "slow");
                    $("#ad-edit").animate({ marginLeft: (tempWindowWidth / 2) }, "slow");
                    $("#ad-view").animate({ marginLeft: "0px" }, "slow");
                }
            }, 250);
        }
    </script>
    
    <script type="text/javascript">
        var DeviceList = null;
        var thisSSID = null;
        var token = GetQueryString("token"); //读取token
        var tmpsubpage = "";
        var thisSSID_ID = 0;
        var preview_ssid = "";
        var preview_ad = "";
        var thisorg_id;

        $(function () {
            $("#token").val(token);
            $("#UserToken").val(token);
            GetOrgApList();
            GetTempletList();
            //GetHangyeList();
            GetAdList();

            $("#SaveSSID_btn").prop("disabled", true);
            SetActiveAdedit(false);

            $("#dialog-form").dialog({
                autoOpen: false,
                height: 500,
                width: 600,
                modal: true,
                buttons: {
                    "确定": function () {
                        var bValid = true;
                        if (bValid) {
                            var picval = $('input:radio[name="picdialog_pic"]:checked').val();
                            $("#" + selectpicdiv).val(picval);
                            $(this).dialog("close");
                        }
                    },
                    "取消": function () {
                        $(this).dialog("close");
                    },
                    "刷新": function () {
                        getserverpics();
                    }
                },
                close: function () {

                }
            });
        });

        function GetTempletList() {
            $.ajax({
                type: 'post',
                url: 'AjaxComm.aspx',
                data: 'type=GetTempletList&token=' + token,
                dataType: 'json',
                error: function (msg) {
                    //alert("服务器错误");
                },
                success: function (obj) {
                    if (obj.ResultCode == 0) {
                        $("#ad_model").empty();
                        for (var i = 0; i < obj.ResultOBJ.length; i++) {
                            $("#ad_model").append("<option value='" + obj.ResultOBJ[i].SADT_ID + "'>" + obj.ResultOBJ[i].SADT_NAME + "</option>");
                        }
                    } else {
                        alert(obj.ResultMsg);
                        if (obj.ResultCode == -100) {
                            window.location.href = "login.aspx";
                        }
                    }
                }
            });
        }

        function GetHangyeList() {
            $.ajax({
                type: 'post',
                url: 'AjaxComm.aspx',
                data: 'type=GetDicByHangYe&token=' + token,
                dataType: 'json',
                error: function (msg) {
                    //alert("服务器错误");
                },
                success: function (obj) {
                    if (obj.ResultCode == 0) {
                        $("#ad_type").empty();
                        for (var i = 0; i < obj.ResultOBJ.length; i++) {
                            $("#ad_type").append("<option value='" + obj.ResultOBJ[i].VALUE + "'>" + obj.ResultOBJ[i].NAME + "</option>");
                        }
                    } else {
                        alert(obj.ResultMsg);
                        if (obj.ResultCode == -100) {
                            window.location.href = "login.aspx";
                        }
                    }
                }
            });
        }

        function GetAdList() {
            $("#org_ad_list").empty();
            $("#org_ad_list").append("<option value='0'>添加新广告</option>");
            $.ajax({
                type: 'post',
                url: 'AjaxComm.aspx',
                data: 'type=GetADList&token=' + token,
                dataType: 'json',
                error: function (XMLHttpRequest, textStatus, errorThrown) {
//                    alert(XMLHttpRequest.status);
//                    alert(XMLHttpRequest.readyState);
//                    alert(textStatus);
//                    alert(errorThrown);
                },
                success: function (obj) {
                    if (obj.ResultCode == 0) {
                        for (var i = 0; i < obj.ResultOBJ.length; i++) {
                            $("#org_ad_list").append("<option value='" + obj.ResultOBJ[i].AD_ID + "'>" + obj.ResultOBJ[i].AD_Title + "</option>");
                        }
                        $("#org_ad_list").val($("#ad_id").val());
                    } else {
                        alert(obj.ResultMsg);
                        if (obj.ResultCode == -100) {
                            window.location.href = "login.aspx";
                        }
                    }
                }
            });
        }

        function GetOrgApList(_dom) {
          if (_dom != undefined && $('#text_org_ap').is(":hidden")) {
            $('#text_org_ap').show('slide');
            return;
          }
            $("#select_org_ap_list").empty();
            $.ajax({
                type: 'post',
                url: 'AjaxComm.aspx',
                data: 'type=GetAPListForState&token=' + token + '&param={"NAME":"' + $("#text_org_ap").val() + '","COLUMN":"","ORDERBY":""}',
                dataType: 'json',
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //                    alert(XMLHttpRequest.status);
                    //alert(XMLHttpRequest.readyState);
                    //                    alert(textStatus);
                    //                    alert(errorThrown);
                },
                success: function (obj) {
                    if (obj.ResultCode == 0) {
                        for (var i = 0; i < obj.ResultOBJ.APList.length; i++) {
                            $("#select_org_ap_list").append("<option value='" + obj.ResultOBJ.APList[i].ID + "' ds='" + obj.ResultOBJ.APList[i].DEVICESTATE + "'>" + obj.ResultOBJ.APList[i].ALIAS + "</option>");
                        }
                        if (obj.ResultOBJ.APList.length == 1) {
                            $("#select_org_ap_list").hide();
                        }
                        ChangeOrgAP();
                    } else {
                        alert(obj.ResultMsg);
                        if (obj.ResultCode == -100) {
                            window.location.href = "login.aspx";
                        }
                    }
                }
            });
        }

        function ChangeOrgAP() {
            DeviceList = null;
            thisSSID = null;
            $("#PreviwPage").attr("src", "");
            var tmpd = $("#select_org_ap_list option:selected").val();
            var tmpn = $("#select_org_ap_list option:selected").text();
            var ds = $("#select_org_ap_list option:selected").attr('ds');
            GetSSIDListFromAP(tmpd, tmpn, ds);
        }

        function GetSSIDListFromAP(apid, apname, devicestate) {
          $("#tbDeviceList").empty();
          $("#tbDeviceList").append("<a class='list-group-item'><span class='label label-primary'> " + apname + "</span><span class='pull-right' onclick='javascript:addssid(\"" + apid + "\");'><abbr title='添加广告'><i class='fa fa-plus pull-right'></i></abbr></span></a>");
          var result = "";
          $.ajax({
              type: 'post',
              url: 'AjaxComm.aspx',
              data: 'type=SSIDManage/GetSSIDListByAPID&token=' + token + '&param=' + apid,
              dataType: 'json',
              error: function (XMLHttpRequest, textStatus, errorThrown) {
                  //                    alert(XMLHttpRequest.status);
                  //                    alert(XMLHttpRequest.readyState);
                  //                    alert(textStatus);
                  alert(errorThrown);
              },
              success: function (obj) {
                  $("#tbdAPCTList").empty();
                  $("#divPage").empty();
                  if (obj.ResultCode == "0") {
                      if (obj.ResultOBJ.length > 0) {
                          DeviceList = obj.ResultOBJ;
                          var item;
                          var item_ssid;
                          for (var i = 0; i < obj.ResultOBJ.length; i++) {
                              item_ssid = obj.ResultOBJ[i];
                              thisorg_id = item_ssid.OID;
                              result += "<a herf='#ad-view' onclick='javascript:PreViewSSID(" + i + ")'  class='go-ad-view list-group-item ssid-item'><span onclick='javascript:removessid(\"" + item_ssid.ID + "\");'> <abbr title='删除'><i class='fa fa-fw fa-trash-o pull-right'></i></abbr> </span> <span onclick='javascript:modifyssidname(" + item_ssid.ID + ",\"" + item_ssid.NAME + "\",\"" + item_ssid.PWD + "\");'  style='cursor:pointer;'><abbr title='设置'><i class='fa fa-fw fa-gear pull-right'></i></abbr></span>"
                              result += getStaStr(item_ssid.STATE, item_ssid.NAME);
                              result += getDownStr(devicestate, item_ssid.DOWNPATH, item_ssid.PATH);
                              result += "</a>";
                          }
                          $("#tbDeviceList").append(result);
                          $(".go-ad-view").anchorGoWhere({ target: 3 });
                          //wifi列表样式
                          $(".ssid-item").click(function () {
                              $(".ssid-item").removeClass("active");
                              $(this).addClass("active");
                          });

                      }
                  } else {
                      alert(obj.ResultMsg);
                      if (obj.ResultCode == -100) {
                          window.location.href = "login.aspx";
                      }
                  }
              }
          });
        }

        function getDownStr(_ds, _downpath, _adpath) {
            var str = "";
            if (_ds == 2) {
                if (_downpath != null) {
                    str = "<span onclick='javascript:showDown(this, \"" + _downpath + "\")'><abbr title='下载广告'><i class='fa fa-fw fa-cloud-download pull-right'></i></abbr></span>";
                }
                else {
                    str = "<span onclick='javascript:showDown(this, null, \"" + _adpath + "\")'><abbr title='下载广告'><i class='fa fa-fw fa-cloud-download pull-right'></i></abbr></span>";
                }
            }
            return str;
        }

        function showDown(_dom, _url, _adpath) {
            if (_url != null || $(_dom).attr('isDown')) {
                $("#dialog_download").find('a').attr('href', _url != null ? _url : $(_dom).attr('isDown'));
            } else {
                PostTarAD(_dom, _adpath);
            }
            $("#dialog_download").dialog("open");
        }

        function PostTarAD(_dom, _path) {
            $('#dialog_download').find('a').html('正在打包，请稍候<i class="fa fa-spinner fa-spin"></i>');
            $.ajax({
                type: 'post',
                url: 'AjaxComm.aspx',
                data: 'type=ADManage/TarAD&token=' + token + '&param="' + _path + '"',
                dataType: 'json',
                error: function (msg) {
                    $('#dialog_download').find('a').html('打包失败，请重试');
                },
                success: function (obj) {
                    if (obj.ResultCode == "0") {
                        $(_dom).attr('isDown', obj.ResultOBJ);
                        $('#dialog_download').find('a').html('点击下载');
                        $('#dialog_download').find('a').attr('href', obj.ResultOBJ);
                    } else {
                        $('#dialog_download').find('a').html('打包失败，请重试');
                        if (obj.ResultCode == -100) {
                            window.location.href = "login.aspx";
                        }
                    }
                }
            });
        }

        function getStaStr(type, name) {
            var str = "";
            switch (type) {
                case 0:
                    str = "<span class='fa-dongtai'>" + name + "</span><span class='label label-info pull-right fa-dongtai'>待审核</span>";
                    break;
                case 1:
                    str = "<span class='fa-dongtai'>" + name + "</span><span class='label label-info pull-right fa-dongtai'>审核中</span>";
                    break;
                case 2:
                    str = name + "<span><abbr title='已通过审核'><i class='fa fa-fw fa-check pull-right'></i></abbr></span>";
                    break;
                case 3:
                    str = name + "<span class='label label-danger pull-right'>拒绝申请</span>";
                    break;
                case 4:
                    str = name + "<span class='label label-warning pull-right'>撤销申请</span>";
                    break;
            }
            return str;
        }

        function PreViewSSID(t) {
            thisSSID = DeviceList[t];
            $("#PreviwPage").attr("src", SetUrlRefresh(thisSSID.PATH + thisSSID.PORTAL));
        }

        function changemanagetype(t) {
            if (t == 0) {
                this.location.href = "control.aspx?token=" + token;
            }
            if (t == 1) {
                this.location.href = "manage.aspx?token=" + token;
            }
        }
    </script>
    
    <script type="text/javascript">
//        SSID修改
        $(function () {
            $("#dialog_editssid").dialog({
                autoOpen: false,
                height: 200,
                width: 300,
                modal: true,
                buttons: {
                    "确定": function () {
                        var ssidname = $("#input_ssid_newname").val();
                        var ssidid = $("#input_ssid_id").val();
                        var ssidpass = $("#input_ssid_password").val();
                        var ispass = "false";
                        if (ssidid > 0) {
                            if (ssidname == "") {
                                alert("请输入SSID名称！");
                                return;
                            }
                            if (ssidpass != "") {
                                //if (ssidpass.length < 8 || ssidpass.length > 16) {
                                //    alert("密码长度必须在8位至16位之间");
                                //    return;
                                //}
                                //if (!isNumberOrLetter(ssidpass)) {
                                //    alert("密码只能包含数字和字母");
                                //    return;
                                //}
                                ispass = "true";
                            }
                            $.ajax({
                                type: 'post',
                                url: 'AjaxComm.aspx',
                                data: 'type=SSIDManage/SaveSSID&param={"MAXFLOW":"0","VONLINETIME":"0","ID":"' + ssidid + '","NAME":"' + ssidname + '","ISPWD":"' + ispass + '","PWD":"' + ssidpass + '"}&token=' + token,
                                dataType: 'json',
                                error: function (msg) {
                                    //alert("服务器错误");
                                },
                                success: function (obj) {
                                    if (obj.ResultCode == 0) {
                                        if ($('#select_org_ap_list option:selected').attr('ds') == 2) {
                                            alert("保存成功，请等待审核通过后，观察到设备SSID已变更后再断开网络");
                                        }
                                        else {
                                            alert(obj.ResultMsg);
                                        }
                                        ChangeOrgAP();
                                    }
                                    else {
                                        alert(obj.ResultMsg);
                                        if (obj.ResultCode == -100) {
                                            window.location.href = "login.aspx";
                                        }
                                    }
                                }
                            });
                            $(this).dialog("close");
                        }
                    },
                    "取消": function () {
                        $(this).dialog("close");
                    }
                },
                close: function () {

                }
            });

            $("#dialog_download").dialog({
                autoOpen: false,
                height: 200,
                width: 300,
                modal: true,
                buttons: {
                    "关闭": function () {
                        $(this).dialog("close");
                    }
                }
            });
            $("#dialog_download").find('a').click(function () {
                if ($(this).html() == "点击下载")
                    $("#dialog_download").dialog('close');
            });
        });

        function isNumberOrLetter(s) {//判断是否是数字或字母 
            var regu = "^[0-9a-zA-Z]+$";
            var re = new RegExp(regu);
            if (re.test(s)) {
                return true;
            } else {
                return false;
            }
        }



        function modifyssidname(ssidid, ssidname, ssidpass) {
            $("#input_ssid_newname").val(ssidname);
            $("#input_ssid_id").val(ssidid);
            $("#input_ssid_password").val(ssidpass);
            $("#dialog_editssid").dialog("open");
        }

        function removessid(ssid_id) {
            if (!confirm("确认要删除？")) {
                return false;
            }
            $.ajax({
                type: 'post',
                url: 'AjaxComm.aspx',
                data: 'type=SSIDManage/DisableSSID&token=' + token + '&param=' + ssid_id,
                dataType: 'json',
                error: function (msg) {
                    //alert("服务器错误" + msg);
                },
                success: function (obj) {
                    if (obj.ResultCode == "0") {
                        alert(obj.ResultMsg);
                        ChangeOrgAP();
                    } else {
                        alert(obj.ResultMsg);
                        if (obj.ResultCode == -100) {
                            window.location.href = "login.aspx";
                        }
                    }
                }
            });
        }

        function addssid(ap_id) {
            $.ajax({
                type: 'post',
                url: 'AjaxComm.aspx',
                data: 'type=SSIDManage/AddSSID&token=' + token + '&param=' + ap_id,
                dataType: 'json',
                error: function (msg) {
                    //alert("服务器错误");
                },
                success: function (obj) {
                    if (obj.ResultCode == "0") {
                        alert(obj.ResultMsg);
                        ChangeOrgAP();
                    } else {
                        alert(obj.ResultMsg);
                        if (obj.ResultCode == -100) {
                            window.location.href = "login.aspx";
                        }
                    }
                }
            });
        }
    </script>
    
    <script type="text/javascript">

        function GetAdInfo(adid) {
            $("#org_ad_list").val(adid);
            $("#ad_id").val(adid);
            SetActiveAdedit(false);
            if (adid > 0) {
                $.ajax({
                    type: 'post',
                    url: 'AjaxComm.aspx',
                    data: 'type=GetADInfo&token=' + token + '&ad_id=' + adid,
                    dataType: 'json',
                    error: function (msg) {
                        //alert("服务器错误");
                    },
                    success: function (obj) {
                        if (obj.ResultCode == 0) {
                            $("#ad_model").prop("disabled", true);
                            $("#ad_model").val(obj.ResultOBJ.AD_Model);
                            $("#ad_id").val(adid);
                            $("#ad_title").val(obj.ResultOBJ.AD_Title);
                            $("#ad_ssid").val(obj.ResultOBJ.AD_SSID);
                            $("#ad_type").val(obj.ResultOBJ.AD_Type);
                            $("#homepage").val(obj.ResultOBJ.AD_HomePage);
                            $("#pubcount").val(obj.ResultOBJ.AD_Release_Count);
                            $("#ad_pubpath").val(obj.ResultOBJ.AD_PUBPATH);
                            $("#modeldiv").empty();
                            $("#modeldiv").append("<input type='hidden' name='ad_model' value='" + obj.ResultOBJ.AD_Model + "' />");
                            if (obj.ResultOBJ.AD_Stat != 1) {
                                SetActiveAdedit(true);
                            }
                            getadfiles();
                        } else {
                            alert(obj.ResultMsg);
                            if (obj.ResultCode == -100) {
                                window.location.href = "login.aspx";
                            }
                        }
                    }
                });
            } else {
                getNewAD();
            }
        }

        function getadfiles() {
            $.ajax({
                type: 'post',
                url: 'AjaxComm.aspx',
                data: 'type=GetADFiles&token=' + token + '&ad_id=' + $("#ad_id").val(),
                dataType: 'json',
                error: function (msg) {
                    //alert("服务器错误");
                },
                success: function (obj) {
                    if (obj.ResultCode == "0") {
                        $("#templetfiles").empty();
                        AD_TEMPLET = obj.ResultOBJ;
                        FillPages(obj.ResultOBJ);
                    } else {
                        alert(obj.ResultMsg);
                        if (obj.ResultCode == -100) {
                            window.location.href = "login.aspx";
                        }
                    }
                }
            });
        }

        function getcontent(page) {
            
            AD_TEMPPAGE = null;
            var item;
            for (var i = 0; i < AD_TEMPLET.length; i++) {
                item = AD_TEMPLET[i];
                if (item.File_Name == page) {
                    $("#temppage").val(page);
                    $("#PreviwPage").attr("src", SetUrlRefresh(item.File_Url));
                    AD_TEMPPAGE = item;
                    $("#templetcontent").empty();
                    var templethtml = "";
                    var tmpvalue = "";
                    for (var j = 0; j < item.File_Templet.length; j++) {
                        tmpvalue = "";
                        for (var k = 0; k < item.File_Content.length; k++) {
                            if (item.File_Content[k].TKey == ("Templet_" + j)) {
                                tmpvalue = item.File_Content[k].TValue;
                                break;
                            }
                        }
                        if (item.File_Templet[j].Unit_Type == "txt" || item.File_Templet[j].Unit_Type == "parameter") {
                            $("#templetcontent").append("<tr><td>" + item.File_Templet[j].Unit_Name + " ：</td><td colspan=2>" + getcombox(item.File_Templet[j].Unit_ValueDict, "Templet_" + j, tmpvalue) + "</td></tr>");
                        } else if (item.File_Templet[j].Unit_Type == "pic") {
                            $("#templetcontent").append("<tr><td>" + item.File_Templet[j].Unit_Name + " ：</td><td> " + tmpvalue + " </div><div id='Templet_" + j + "_div'> <input class='pull-left' id='Templet_" + j + "' name='Templet_" + j + "' type='file' /> <a class='pull-right' href='javascript:ChangePicSelect(\"Templet_" + j + "_div\",1,\"Templet_" + j + "\")'>我的图库</a></div></td></tr>");
                        } else if (item.File_Templet[j].Unit_Type == "richtxt") {
                            $("#templetcontent").append("<tr><td>" + item.File_Templet[j].Unit_Name + " ：</td><td colspan=2><textarea id='Templet_" + j + "' name='Templet_" + j + "' cols='100' rows='12' style='width:500px;height:300px;visibility:hidden;'></textarea></td></tr>");
                            kindeditcontrol("Templet_" + j, $("#UserToken").val(), $("#ad_id").val(), j, tmpvalue);
                        } else if (item.File_Templet[j].Unit_Type == "sysparam") {
                            tmpvalue = getsysparam(item.File_Templet[j].Unit_ValueDict);
                            $("#templetcontent").append("<input type='hidden' id='Templet_" + j + "' name='Templet_" + j + "' value='" + tmpvalue + "' />");
                        }
                        if (item.File_Templet[j].Unit_Link == "true" && (item.File_Templet[j].Unit_Type == "txt" || item.File_Templet[j].Unit_Type == "pic")) {
                            tmpvalue = "";
                            for (var k = 0; k < item.File_Content.length; k++) {
                                if (item.File_Content[k].TKey == ("Templet_" + j + "_link")) {
                                    tmpvalue = item.File_Content[k].TValue;
                                    break;
                                }
                            }
                            $("#templetcontent").append("<tr><td>连接地址：</td><td colspan=2><input class='form-control' id='Templet_" + j + "_link' name='Templet_" + j + "_link' type='text' value='" + tmpvalue + "' /></td></tr>");
                        }
                    }
                }
            }
        }

        function getsysparam(paramkey) {
            var str = "";
            switch (paramkey) {
                case "orgid":
                    str = thisorg_id;
                    break;
            }
            return str;
        }

        function getcombox(valuedict, myname, myvalue) {
            var result = "";
            if (trim(valuedict).indexOf(":") > 0) {
                result = "<select id='" + myname + "' name='" + myname + "' class='form-control'>";
                var tmpkey, tmpvalue, tmpsel;
                var words = valuedict.split("||")
                for (var i = 0; i < words.length; ++i) {
                    tmpkey = "";
                    tmpvalue = "";
                    tmpsel = "";
                    if (words[i].indexOf(":") > 0) {
                        tmpkey = words[i].substring(0, words[i].indexOf(":"));
                        tmpvalue = words[i].substring(words[i].indexOf(":") + 1);
                        if (myvalue == tmpvalue) {
                            tmpsel = " selected='selected'";
                        }
                        result += "<option value='" + tmpvalue + "' " + tmpsel + ">" + tmpkey + "</option>";
                    }
                }
                result += "</select>";
            } else {
                result = "<input class='form-control' id='" + myname + "' name='" + myname + "' type='text' value='" + myvalue + "' />";
            }
            return result;
        }

        function kindeditcontrol(area, token, adid, sid, myvalue) {
            var editor1 = KindEditor.create('#' + area, {
                cssPath: '/UITemplet/kindeditor/plugins/code/prettify.css',
                uploadJson: 'upload.ashx?token=' + token + "&adid=" + adid + "&sid=" + sid,
                allowFileManager: false,
                items: ['source', 'fullscreen', '|', 'undo', 'redo', 'cut', 'copy', 'paste',
                        'plainpaste', 'wordpaste', '|', 'justifyleft', 'justifycenter', 'justifyright',
                        'justifyfull', 'insertorderedlist', 'insertunorderedlist', 'indent', 'outdent', 'subscript',
                        'superscript', '|', 'selectall', '/',
                        'title', 'fontname', 'fontsize', '|', 'forecolor', 'hilitecolor', 'bold',
                        'italic', 'underline', 'strikethrough', 'removeformat', '|', 'image',
                        'advtable', 'hr', 'emoticons', 'link', 'unlink'],
                afterBlur: function () { this.sync(); } 

            });
            //prettyPrint();
            editor1.html(myvalue);
        }

        function getNewAD() {
            $("#ad_model").prop("disabled", false);
            $("#ad_id").val(0);
            $("#ad_title").val("");
            $("#ad_ssid").val("");
            $("#homepage").val("");
            $("#pubcount").val(0);
            $("#ad_pubpath").val("");
            $("#modeldiv").empty();
            $("#templetfiles").empty();
            $("#templetcontent").empty();
            $("#PreviwPage").attr("src", "");
            SetActiveAdedit(true);
            $("#btn_ad_savepost").prop("disabled", true);
            GetTempletFiles();
        }

        function SetActiveAdedit(t){
            if (t) {
                $("#btn_ad_save").prop("disabled", false);
                $("#btn_ad_savepost").prop("disabled", false);
                $("#ad_list").prop("disabled", false);
            } else {
                $("#btn_ad_save").prop("disabled", true);
                $("#btn_ad_savepost").prop("disabled", true);
                $("#ad_list").prop("disabled", true);
            }
        }

        function GetTempletFiles() {
            $.ajax({
                type: 'post',
                url: 'AjaxComm.aspx',
                data: 'type=GetTempletFiles&token=' + token + '&temp_ID=' + $("#ad_model option:selected").val(),
                dataType: 'json',
                error: function (msg) {
                    //alert("服务器错误");
                },
                success: function (obj) {
                    if (obj.ResultCode == "0") {
                        $("#templetfiles").empty();
                        AD_TEMPLET = obj.ResultOBJ;
                        FillPages(obj.ResultOBJ);
                    } else {
                        alert(obj.ResultMsg);
                        if (obj.ResultCode == -100) {
                            window.location.href = "login.aspx";
                        }
                    }
                }
            });
        }

        function FillPages(ResultOBJ) {
            var item;
            var f;
            for (var i = 0; i < ResultOBJ.length; i++) {
                item = ResultOBJ[i];
                f = item.File_Name;
                fn = item.File_Note;
                if (item.isPortal) {
                    $("#homepage").val(f);
                    if (tmpsubpage == "") {
                        getcontent(f);
                    } else {
                        getcontent(tmpsubpage);
                        tmpsubpage = "";
                    }
                    fn = fn + "*";
                }
                $("#templetfiles").append("<label class='btn btn-default " + (i == 0 ? ' active' : '') + "' onclick='javascript:getcontent(\"" + item.File_Name + "\");'><input type='radio' name='options' id='option1' >" + fn + "</label>");
            }
        }

        function ChangePicSelect(mydiv, p, n) {
            $("#" + mydiv).empty();
            if (p == 0) {
                $("#" + mydiv).append("<input class='pull-left' id='" + n + "' name='" + n + "' type='file' /> <a class='pull-right' href='javascript:ChangePicSelect(\"" + mydiv + "\",1,\"" + n + "\")'>我的图库</a>");
            } else {
                $("#" + mydiv).append("<input id='" + n + "' name='" + n + "' type='text'/> <input  type='button' onclick='javascript:SelectServerPic(\"" + n + "\")' value='选择' /> <a class='pull-right' href='javascript:ChangePicSelect(\"" + mydiv + "\",0,\"" + n + "\")'>上传图片</a>");
            }
        }

        var ServerPics = null;
        var selectpicdiv;
        function SelectServerPic(picdiv) {
            selectpicdiv = picdiv;
            if (ServerPics == null) {
                getserverpics();
            }
            else {
                showpicsdialog();
            }
        }
        function getserverpics() {
            $.ajax({
                type: 'post',
                url: 'AjaxComm.aspx',
                data: 'type=GetServerPics&token=' + token,
                dataType: 'json',
                error: function (msg) {
                    //alert("服务器错误");
                },
                success: function (obj) {
                    if (obj.ResultCode == "0") {
                        ServerPics = obj.ResultOBJ;
                        showpicsdialog();
                    } else {
                        alert(obj.ResultMsg);
                        if (obj.ResultCode == -100) {
                            window.location.href = "login.aspx";
                        }
                    }
                }
            });
        }
        function showpicsdialog() {
            $("#dl_serverpics").empty();
            for (var i = 0; i < ServerPics.length; i++) {
                $("#dl_serverpics").append("<dd class ='pic-list'><img  src='" + ServerPics[i].FileURL + "'  /><br /><input type='radio' id='picdialog_pic' name='picdialog_pic' value='" + ServerPics[i].FileName + "' /></dd>");

            }
            $("#dialog-form").dialog("open");
        }

        function ChangeAd() {
            var tmpadid = $("#org_ad_list option:selected").val();
            GetAdInfo(tmpadid);
        }

    </script>

    <script>
        function submitsavaad() {
            $("#form_adedit").submit();
        }
        function SaveAD() {
            if (AD_TEMPLET.length < 1) {
                alert("no");
                return false;
            }
            if (AD_TEMPPAGE == null) {
                alert("no");
                return false;
            }
            if ($("#ad_title").val() == "") {
                alert("请输入广告名称");
                return false;
            }
            tmpsubpage = $("#temppage").val();
            SetActiveAdedit(false);
            return true;
        }

        function savesuss(adid) {
            GetAdList();
            GetAdInfo(adid);
        }

        function saveerr(msg) {
            alert(msg);
            var adid = $("#ad_id").val();
            GetAdList();
            GetAdInfo(adid);
        }

        function saveadpost() {
            var adid = $("#ad_id").val();
            if (adid > 0) {
                SetActiveAdedit(false);
                postaudit(adid, 1, thisSSID.ID, 0, 0);
            } else {
                alert("请先保存广告信息");
            }
        }

        function postaudit(adid, type, ids, savecase, iscopy) {
            $.ajax({
                type: 'post',
                url: 'AjaxComm.aspx',
                data: 'type=PostADAudit&token=' + token + '&param={"ad_id":"' + adid + '","pub_type":"' + type + '","ids":"' + ids + '","ascase":"' + savecase + '","isCopyName":"' + iscopy + '"}',
                dataType: 'json',
                error: function (msg) {
                    //alert("服务器错误");
                },
                success: function (obj) {
                    if (obj.ResultCode == "0") {
                        alert("广告提交审核，请等待审核结果！");
                        GetAdInfo(adid);
                    } else {
                        alert(obj.ResultMsg);
                        if (obj.ResultCode == -100) {
                            window.location.href = "login.aspx";
                        }
                    }
                }
            });
        }

        function exituser() {
            $.ajax({
                type: 'post',
                url: 'AjaxComm.aspx',
                data: 'type=Logout&token=' + token,
                dataType: 'json',
                error: function (msg) {
                    //alert("网络错误");
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
    </head>
    <body>
    <uc1:header ID="header1" runat="server" />
    <div class="container-fluid">
        <div  class="row-fluid">
            <div class="div-body  col-md-6 col-sm-6 changeable" id="ad-list" name="ad-list">
                <div class="panel panle_ssid panel-default">
                    <div class="panel-heading" style="font-size:18px">
                        <span class="label" style="color:#d9534f;font-size:38px"><em>①</em></span>广告语设置
                        <span>
                            <div class="pull-right">
 <select id="select_org_ap_list" name="select_org_ap_list"  style="width:130px;font-size:12px;height:28px" onchange="ChangeOrgAP()"></select>
                                <input id="text_org_ap" type="search" class="input-sm " placeholder="查询关键字.." value="" style=" display:none;"/>
                                <a id="search_btn" class="btn btn-success" href="#" onclick="GetOrgApList(this)"><i class="fa fa-search"></i></a>
                            </div>
                        </span>
                    </div>
                    <%--<div class="btn-group btn-group-xs pull-right">
                        <button type="button" class="btn btn-success" onclick="javascript:changemanagetype(0)">按设备查看</button>
                        <button type="button" class="btn btn-warning" onclick="javascript:changemanagetype(1)">按广告查看</button>
                    </div>--%>
                    <div class="list-group" id="tbDeviceList" name="tbDeviceList" style="float:right; width:100%; height:622px; overflow-y:auto">
                    </div>
                </div>
            </div>
            <div class="div-body col-md-6 col-sm-6 changeable" id="ad-view" name="ad-view">
                <div class="panel panle_right panel-default">
                    <div class="panel-heading" style="font-size:18px">
                        <span class="label" style="color:#d9534f;font-size:38px"><em>②</em></span>广告页预览<a style="float:right;font-size:20px;line-height: 28px;"  href='#' id="list-view-btn"><i class='fa fa-fw fa-pencil'></i>编辑<p class='disabled hidden-xs pull-right' style='font-size:28px; color:#d9534f'><i class="fa fa-fw fa-arrow-circle-right fa-dongtai"></i></p></a><a style='float:right' class='goNext visible-xs' href='#ad-edit'><i class='fa fa-fw fa-pencil'></i>参数</a>
                    </div>
                    <div class="widget-content padded clearfix">
                        <iframe id="PreviwPage" name="PreviwPage" style="float:right; width:100%; height:600px;"></iframe>
                    </div>
                </div>
            </div>
            <div class="div-body col-md-6 col-sm-6 changeable" id="ad-edit" name="ad-edit">
                <div class="panel panle_right panel-default">
                    <div class="panel-heading" style="font-size:18px">
                        <span class="label" style="color:#d9534f;font-size:38px"><em>③</em></span>广告页编辑<a style="float:right" class='goNext visible-xs' href='#ad-view'><i class='fa fa-fw fa-eye'></i>预览</a>
                    </div>
                    <div class="widget-content padded clearfix">
                        <div>
                          <div class="col-md-10">
                            <select class="form-control" id="org_ad_list" name="org_ad_list" onchange="ChangeAd()"></select>
                          </div>
                          <div class="col-md-2">
                            <input type="button" id="btn_addnewad" class="go-ad-view btn btn-primary" onclick="javascript:GetAdInfo(0)" value="新建广告" />
                          </div>
                            <form id="form_adedit" action="ADSave.aspx" target="upload" method="post" enctype="multipart/form-data" onsubmit="return SaveAD();">
                                <input type="hidden" id="ad_id" name="ad_id" value="0" />
                                <input type="hidden" id="homepage" name="homepage" value="" />
                                <input type="hidden" name="pubcount" value="0" />
                                <input type="hidden" id="ad_pubpath" name="ad_pubpath" value="" />
                                <input type="hidden" id="temppage" name="temppage" value="" />
                                <input type="hidden" id="UserToken" name="UserToken" value="" />
                                <input type="hidden" id="ad_ssid" name="ad_ssid" value="" />
                                <input type="hidden" id="ad_type" name="ad_type" value=0>
                                <table class="hor-minimalist-top table table-striped">
                                    <tr>
                                        <td width="100px">广告名称：</td>
                                        <td colspan="3"><input id="ad_title" name="ad_title" type="text"class="form-control" /></td>
                                    </tr>
                                    <tr>
                                        <td>应用模版：</td>
                                        <td>
                                            <select id="ad_model" name="ad_model" class="form-control" onchange="GetTempletFiles()"></select><div id="modeldiv"/>
                                        </td>
                                        <td></td>
                                        <td></td>
<%--                                        <td width="60px">行业：</td>
                                        <td>
                                            <select id="ad_type" name="ad_type" class="form-control"></select>
                                        </td>--%>
                                    </tr>
                                    <tr style="background-color:#eee">
                                        <td valign="top" colspan="4">
                                            <div id="templetfiles" class="btn-group" data-toggle="buttons"></div>
                                            <table id="templetcontent" style="background-color:#eee" class="hor-minimalist-top table table-bordered table-striped" width="95%">
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" class="text-center">
                                            <input type="button" id="btn_ad_save" class="go-ad-view btn btn-primary" onclick="javascript:submitsavaad()" value="保存并预览" />&nbsp;
                                            <input type="button" id="btn_ad_savepost"  class="go-ad-view btn btn-primary" onclick="javascript:saveadpost();" value="提交审核"/>
                                        </td>
                                    </tr>
                                </table>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
    <iframe id="upload" name="upload" style="width:0px; height:0px; border:0px;" ></iframe>
    <div id="dialog-form" title="我的图库：">
        <fieldset>
            <dl id="dl_serverpics">
      
            </dl>
        </fieldset>
    </div>
    <div id="dialog_editssid" title="SSID设置：">
        <fieldset>
            <div>SSID显示名称：<input type="text" id="input_ssid_newname" name="input_ssid_newname" value=""/><input type="hidden" id="input_ssid_id" name="input_ssid_id" value="0"/></div>
            <div>SSID登陆密码：<input type="text" id="input_ssid_password" name="input_ssid_password" value=""  class="form-control" disabled=disabled/></div>
        </fieldset>
    </div>

    <div id="dialog_download" title="广告下载：">
        <fieldset>
            <div><a target="_blank">开始下载</a></div><br />
            <p>请将下载的压缩包中的内容直接解压到U盘根目录，请勿改动目录结构</p>
        </fieldset>
    </div>
</body>
</html>
