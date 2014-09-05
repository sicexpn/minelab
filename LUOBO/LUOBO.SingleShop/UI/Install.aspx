<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Install.aspx.cs" Inherits="LUOBO.SingleShop.UI.install" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=480" />
    <title>next-wifi 初始安装</title>
    <link media="all" rel="stylesheet" href="../UITemplet/css/bootstrap-3.2.min.css" />
    <link media="all" rel="stylesheet" href="../UITemplet/css/font-awesome.css" />
    <link media="all" rel="stylesheet" href="../UITemplet/css/index.css" />
    <link media="all" rel="stylesheet" href="../UITemplet/css/jquery-ui.min.css" />
    <link media="all" rel="stylesheet" href="../UITemplet/css/bootstrap-switch.css" />

    <script type="text/javascript" src="../UITemplet/js/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="../UITemplet/js/jquery-ui-1.10.4.min.js"></script>
    <script type="text/javascript" src="../UITemplet/js/bootstrap-3.2.min.js"></script>
    <script type="text/javascript" src="../UITemplet/js/bootstrap-switch.js"></script>
    <script type="text/javascript" src="http://api.map.baidu.com/api?v=2.0&ak=10de0613056269b007c32743d586d9d7"></script>
    <script type="text/javascript" src="../UITemplet/js/Common.js"></script>
    <script type="text/javascript" src="../UITemplet/js/javascript-wifi.js"></script>
    <script type="text/javascript">
        var all_info = {},
            token = GetQueryString("token"),
            chainList = [],
            singleList = [],
            my_otype = [0,0];

        var dialog_add_chain,
            dialog_add_single,
            dialog_ssid_pwd;

        var isForbidScroll = true, // 
            isOpening = false, //正在打开AP开关
            isLoading = false; //正在读取SSID数量

        var map, marker;
        var curStep = "#setup_step1";
        $(document).ready(function () {
            GetInstallUser();
            initMap();
            getLocation();
            bindEvent();
            initDialog();
            all_info.AP_MAC = GetQueryString("mac");
            GetHangyeList();
            GetAreaList();
            GetChainList();
            GetSingleList();
            GetOrgType();

            $('div[name="step2_more"]').hide();
            $("#step4_ssid_ison_chk").bootstrapSwitch();
        });

        function bindEvent() {
            // 机构类型Radio切换事件
            $('input[name="org_type"]').click(function () {
                // 选择连锁时
                if ($(this).val() == "2") {
                    $('#step2_fullname_single').hide();
                    $('#step2_fullname_chain').show();
                    $('#step2_add_chain').show();
                    $('#step2_add_single').hide();
                    $('#step2_fullname_chain').val(-1);
                    $('div[name="step2_more"]').hide();


                } //选择单店时
                else if ($(this).val() == "3") {
                    $('#step2_fullname_single').show();
                    $('#step2_fullname_chain').hide();
                    $('#step2_add_chain').hide();
                    $('#step2_add_single').show();
                    $('#step2_ismanage_div').hide();
                    $('#step2_apalias_div').hide();
                    $('#step2_fullname_single').val(-1);
                    $('div[name="step2_more"]').hide();
                }
            });

            // 单店Select切换事件
            $('#step2_fullname_single').change(function () {
                var mthis = $(this);
                var curItem;
                for (var i = 0; i < singleList.length; i++) {
                    if (singleList[i].NAME == $(this).find('option:selected').text()) {
                        curItem = singleList[i];
                        break;
                    }
                }
                $('#step2_nickname').val("");
                if (mthis.val() > 0) {
                    $('#step2_ismanage_div').hide();
                    $('#step2_apalias_div').show();
                    $('div[name="step2_more"]').hide();
                    $('#step2_nickname').val(curItem.DESCRIPTION);
                    $('#step3_ad_single').show();
                    $('#step3_ad_chain').hide();
                }
                else if (mthis.val() == "-2") {
                    $('#step2_ismanage_div').hide();
                    $('#step2_apalias_div').show();
                    $('div[name="step2_more"]').show();
                    $('#step3_ad_single').hide();
                    $('#step3_ad_chain').hide();
                }
                else {
                    $('#step2_ismanage_div').hide();
                    $('#step2_apalias_div').hide();
                    $('div[name="step2_more"]').hide();
                    $('#step3_ad_single').hide();
                    $('#step3_ad_chain').hide();
                }
            });

            //连锁Select切换事件
            $('#step2_fullname_chain').change(function () {
                var mthis = $(this);
                var curItem;
                for (var i = 0; i < chainList.length; i++) {
                    if (chainList[i].NAME == $(this).find('option:selected').text()) {
                        curItem = chainList[i];
                        break;
                    }
                }
                $('#step2_nickname').val("");
                if (mthis.val() > 0) {
                    $('#step2_ismanage_div').show();
                    $('#step2_apalias_div').show();
                    if ($('input[name="org_ismanage"]:checked').val() == "true") {
                        $('div[name="step2_more"]').show();
                        $('#step3_ad_chain').hide();
                    }
                    else if ($('input[name="org_ismanage"]:checked').val() == "false") {
                        $('div[name="step2_more"]').hide();
                        $('#step3_ad_chain').show();
                    } else {
                        $('div[name="step2_more"]').hide();
                        $('#step3_ad_chain').hide();
                    }
                    $('#step2_nickname').val(curItem.DESCRIPTION);

                    $('#step3_ad_single').hide();
                    
                }
                else if (mthis.val() == "-2") {
                    $('#step2_ismanage_div').hide();
                    $('#step2_apalias_div').show();
                    $('div[name="step2_more"]').show();

                    $('#step3_ad_single').hide();
                    $('#step3_ad_chain').hide();
                }
                else {
                    $('#step2_ismanage_div').hide();
                    $('#step2_apalias_div').hide();
                    $('div[name="step2_more"]').hide();

                    $('#step3_ad_single').hide();
                    $('#step3_ad_chain').hide();
                }
            });

            // 新增单店点击事件
            $('#step2_add_single').click(function () {
                dialog_add_single.dialog("open");
            });

            // 新增连锁店点击事件
            $('#step2_add_chain').click(function () {
                dialog_add_chain.dialog("open");
            });

            // 是否统一管理Radio事件
            $('input[name="org_ismanage"]').click(function () {
                if ($(this).val() == "true") {
                    $('div[name="step2_more"]').show();
                    $('#step3_ad_chain').hide();
                } else if ($(this).val() == "false") {
                    if ($("#step2_fullname_chain").val() == "-2") {
                        $('div[name="step2_more"]').show();
                    } else {
                        $('div[name="step2_more"]').hide();
                        $('#step3_ad_chain').show();
                    }
                }
            });

            // 定时开关Checkbox点击事件
            $('#step4_time_chk').click(function () {
                if ($(this).is(':checked')) {
                    $('#step4_ssid_stime').removeAttr('disabled');
                    $('#step4_ssid_etime').removeAttr('disabled');
                } else {
                    $('#step4_ssid_stime').attr('disabled', '');
                    $('#step4_ssid_etime').attr('disabled', '');
                }
            });

            // 拨号上网Checkbox点击事件
            $('#step4_ol_chk').click(function () {
                if ($(this).is(':checked')) {
                    $('#step4_ssid_ol_account').removeAttr('disabled');
                    $('#step4_ssid_ol_pwd').removeAttr('disabled');
                } else {
                    $('#step4_ssid_ol_account').attr('disabled', '');
                    $('#step4_ssid_ol_pwd').attr('disabled', '');
                }
            });

            // 设备开关Button点击事件
            $("#step3_ap_power_btn").click(function () {
                if (isOpening) { }
                else {
                    isOpening = true;

                    $(this).toggleClass("hover");
                    if ($(this).hasClass("hover")) {
                        $('#step_next').html("下一步");
                    } else {
                        $('#step_next').html("提交");
                    }

                    if (isLoading) {
                        $(".load").fadeTo(100, 0, slideUp);
                    }
                    else {
                        $(".load").fadeTo(400, 1, slideDown);
                    }
                }
                function slideDown() {
                    isLoading = true; isOpening = false;
                    GetSSIDNum();
                }

                function slideUp() {
                    isLoading = false; isOpening = false;
                    $('#step3_ssid_list').slideToggle(100);
                    $(".load img").show();
                }
            });

            // 广告语的默认Radio事件
            $('input[name="ad_rdo"]').click(function () {
                var _this = $(this);
                switch (_this.val()) {
                    case "custom":
                        $('#container_wifi').slideDown(400);
                        $('#ap_list_div').hide();
                        if ($('#step3_ap_power_btn').hasClass('hover')) {
                            $('#step_next').html("下一步");
                        } else {
                            $('#step_next').html("提交");
                        }
                        break;
                    case "chain":
                        $('#container_wifi').slideUp(200);
                        $('#step_next').html("提交");
                        $('#ap_list_div').show();
                        break;
                    case "single":
                        $('#container_wifi').slideUp(200);
                        $('#ap_list_div').show();
                        $('#step_next').html("提交");
                        break;
                    case "agent":
                        $('#container_wifi').slideUp(200);
                        $('#ap_list_div').hide();
                        $('#step_next').html("提交");
                        break;
                }
            });

            $('#step2_admin').change(function () {
                var _this = $(this);

                _this.next().removeClass('glyphicon-ok glyphicon-remove').addClass('glyphicon-refresh fa-spin');

                $.ajax({
                    type: 'post',
                    url: 'AjaxComm.aspx',
                    data: 'type=HasAccount&token=' + token + '&param="' + _this.val() + '@next-wifi.com"',
                    dataType: 'json',
                    error: function (msg) {
                        _this.parent().parent().parent().removeClass('has-error has-success fa-spin').addClass('has-error');
                        _this.next().removeClass('glyphicon-ok glyphicon-remove glyphicon-refresh fa-spin').addClass('glyphicon-remove');
                    },
                    success: function (obj) {
                        if (obj.ResultCode == 0) {
                            if (obj.ResultOBJ) {
                                _this.parent().parent().parent().removeClass('has-error has-success fa-spin').addClass('has-error');
                                _this.next().removeClass('glyphicon-ok glyphicon-remove glyphicon-refresh fa-spin').addClass('glyphicon-remove');
                                _this.parent().parent().parent().find('.notice').html("帐号重复，请您换一个试试");
                            }
                            else {
                                if (_this.val().length < 2) {
                                    _this.parent().parent().parent().removeClass('has-error has-success fa-spin').addClass('has-error');
                                    _this.next().removeClass('glyphicon-ok glyphicon-remove glyphicon-refresh fa-spin').addClass('glyphicon-remove');
                                    _this.parent().parent().parent().find('.notice').html("帐号长度要求2-16位，请修改");
                                    //alert("帐号长度要求2-16位，请修改");
                                } else {
                                    _this.parent().parent().parent().removeClass('has-error has-success fa-spin').addClass('has-success');
                                    _this.next().removeClass('glyphicon-ok glyphicon-remove glyphicon-refresh fa-spin').addClass('glyphicon-ok');
                                    _this.parent().parent().parent().find('.notice').html("");
                                }
                            }
                        }
                        else {
                            _this.parent().parent().removeClass('has-error has-succes fa-spins').addClass('has-error');
                            _this.next().removeClass('glyphicon-ok glyphicon-remove glyphicon-refresh fa-spin').addClass('glyphicon-remove');
                        }
                    }
                });
            });

            $('#step2_apalias').blur(function () {
                var _this = $(this);
                _this.next().removeClass('glyphicon-ok glyphicon-remove').addClass('glyphicon-refresh fa-spin');
                if (_this.val().trim() == "") {
                    _this.parent().parent().removeClass('has-error has-success').addClass('has-error');
                    _this.next().removeClass('glyphicon-ok glyphicon-remove glyphicon-refresh fa-spin').addClass('glyphicon-remove');
                    _this.parent().parent().find('.notice').html("请您填写设备别名，用于以后管理设备");
                } else {
                    _this.parent().parent().removeClass('has-error has-success').addClass('has-success');
                    _this.next().removeClass('glyphicon-ok glyphicon-remove glyphicon-refresh fa-spin').addClass('glyphicon-ok');
                    _this.parent().parent().find('.notice').html("");
                }
            });

            $('#step2_apalias').change(function () {
                var _this = $(this);
                _this.next().removeClass('glyphicon-ok glyphicon-remove').addClass('glyphicon-refresh fa-spin');
                if (_this.val().trim() == "") {
                    _this.parent().parent().removeClass('has-error has-success').addClass('has-error');
                    _this.next().removeClass('glyphicon-ok glyphicon-remove glyphicon-refresh fa-spin').addClass('glyphicon-remove');
                    _this.parent().parent().find('.notice').html("请您填写设备别名，用于以后管理设备");
                } else {
                    _this.parent().parent().removeClass('has-error has-success').addClass('has-success');
                    _this.next().removeClass('glyphicon-ok glyphicon-remove glyphicon-refresh fa-spin').addClass('glyphicon-ok');
                    _this.parent().parent().find('.notice').html("");
                }
            });
            $('#step2_nickname').change(function () {
                var _this = $(this);
                _this.next().removeClass('glyphicon-ok glyphicon-remove').addClass('glyphicon-refresh fa-spin');
                if (_this.val().trim() == "") {
                    _this.parent().parent().removeClass('has-error has-success').addClass('has-error');
                    _this.next().removeClass('glyphicon-ok glyphicon-remove glyphicon-refresh fa-spin').addClass('glyphicon-remove');
                    _this.parent().parent().find('.notice').html("请您填写商户简称，用于生成广告名称");
                } else {
                    _this.parent().parent().removeClass('has-error has-success').addClass('has-success');
                    _this.next().removeClass('glyphicon-ok glyphicon-remove glyphicon-refresh fa-spin').addClass('glyphicon-ok');
                    _this.parent().parent().find('.notice').html("");
                }
            });
            $('#step2_industry').change(function () {
                var _this = $(this);
                if (_this.val().trim() == "-1") {
                    _this.parent().removeClass('has-error has-success').addClass('has-error');
                    _this.parent().parent().find('.notice').html("请您选择行业，用于自动与周围商圈交换广告");
                } else {
                    _this.parent().removeClass('has-error has-success').addClass('has-success');
                    _this.parent().parent().find('.notice').html("");
                }
            });
            $('#step2_area').change(function () {
                var _this = $(this);
                if (_this.val().trim() == "-1") {
                    _this.parent().removeClass('has-error has-success').addClass('has-error');
                    _this.parent().parent().find('.notice').html("请您选择营业面积，用于调整WiFi发射功率覆盖面积");
                } else {
                    _this.parent().removeClass('has-error has-success').addClass('has-success');
                    _this.parent().parent().find('.notice').html("");
                }
            });
            $('#step2_contact').change(function () {
                var _this = $(this);
                _this.next().removeClass('glyphicon-ok glyphicon-remove').addClass('glyphicon-refresh fa-spin');
                if (_this.val().trim() == "") {
                    _this.parent().parent().removeClass('has-error has-success').addClass('has-error');
                    _this.next().removeClass('glyphicon-ok glyphicon-remove glyphicon-refresh fa-spin').addClass('glyphicon-remove');
                    _this.parent().parent().find('.notice').html("请您填写联系人，用于售后服务联系");
                } else {
                    _this.parent().parent().removeClass('has-error has-success').addClass('has-success');
                    _this.next().removeClass('glyphicon-ok glyphicon-remove glyphicon-refresh fa-spin').addClass('glyphicon-ok');
                    _this.parent().parent().find('.notice').html("");
                }
            });

            $('#step2_phone').change(function () {
                var _this = $(this);
                _this.next().removeClass('glyphicon-ok glyphicon-remove').addClass('glyphicon-refresh fa-spin');
                if (_this.val().trim() != "" && isPhone(_this.val().trim())) {
                    _this.parent().parent().removeClass('has-error has-success').addClass('has-success');
                    _this.next().removeClass('glyphicon-ok glyphicon-remove glyphicon-refresh fa-spin').addClass('glyphicon-ok');
                    _this.parent().parent().find('.notice').html("");
                } else {
                    _this.parent().parent().removeClass('has-error has-success').addClass('has-error');
                    _this.next().removeClass('glyphicon-ok glyphicon-remove glyphicon-refresh fa-spin').addClass('glyphicon-remove');
                    _this.parent().parent().find('.notice').html("请您填写联系电话，11位手机，用于售后服务联系");
                }
            });
            $('#step2_qq').change(function () {
                var _this = $(this);
                _this.next().removeClass('glyphicon-ok glyphicon-remove').addClass('glyphicon-refresh fa-spin');
                if (_this.val().trim() != "" && isQQ(_this.val().trim())) {
                    _this.parent().parent().removeClass('has-error has-success').addClass('has-success');
                    _this.next().removeClass('glyphicon-ok glyphicon-remove glyphicon-refresh fa-spin').addClass('glyphicon-ok');
                    _this.parent().parent().find('.notice').html("");
                } else {
                    _this.parent().parent().removeClass('has-error has-success').addClass('has-error');
                    _this.next().removeClass('glyphicon-ok glyphicon-remove glyphicon-refresh fa-spin').addClass('glyphicon-remove');
                    _this.parent().parent().find('.notice').html("请您填写联系QQ，5-12位，用于售后服务联系");
                }
            });
            $('#step2_weibo').change(function () {
                var _this = $(this);
                _this.next().removeClass('glyphicon-ok glyphicon-remove').addClass('glyphicon-refresh fa-spin');
                if (_this.val().trim() == "") {
                    _this.parent().parent().removeClass('has-error has-success').addClass('has-error');
                    _this.next().removeClass('glyphicon-ok glyphicon-remove glyphicon-refresh fa-spin').addClass('glyphicon-remove');
                    _this.parent().parent().find('.notice').html("请您填写企业微博，用于售后服务联系");
                } else {
                    _this.parent().parent().removeClass('has-error has-success').addClass('has-success');
                    _this.next().removeClass('glyphicon-ok glyphicon-remove glyphicon-refresh fa-spin').addClass('glyphicon-ok');
                    _this.parent().parent().find('.notice').html("");
                }
            });
            $('#step2_weixin').change(function () {
                var _this = $(this);
                _this.next().removeClass('glyphicon-ok glyphicon-remove').addClass('glyphicon-refresh fa-spin');
                if (_this.val().trim() == "") {
                    _this.parent().parent().removeClass('has-error has-success').addClass('has-error');
                    _this.next().removeClass('glyphicon-ok glyphicon-remove glyphicon-refresh fa-spin').addClass('glyphicon-remove');
                    _this.parent().parent().find('.notice').html("请您填写企业微信号，用于售后服务联系");
                } else {
                    _this.parent().parent().removeClass('has-error has-success').addClass('has-success');
                    _this.next().removeClass('glyphicon-ok glyphicon-remove glyphicon-refresh fa-spin').addClass('glyphicon-ok');
                    _this.parent().parent().find('.notice').html("");
                }
            });
            $('#step2_pwd').change(function () {
                var _this = $(this);
                _this.next().removeClass('glyphicon-ok glyphicon-remove').addClass('glyphicon-refresh fa-spin');
                if (_this.val().trim() == "") {
                    _this.parent().parent().removeClass('has-error has-success').addClass('has-error');
                    _this.next().removeClass('glyphicon-ok glyphicon-remove glyphicon-refresh fa-spin').addClass('glyphicon-remove');
                    _this.parent().parent().find('.notice').html("请您填写密码，用于帮助您进行WiFi管理");
                } else {
                    _this.parent().parent().removeClass('has-error has-success').addClass('has-success');
                    _this.next().removeClass('glyphicon-ok glyphicon-remove glyphicon-refresh fa-spin').addClass('glyphicon-ok');
                    _this.parent().parent().find('.notice').html("");
                }
            });
        }

        function initDialog() {
            dialog_add_chain = $("#dialog_add_chain").dialog({
                autoOpen: false,
                height: 210,
                width: 300,
                modal: true,
                buttons: {
                    "确定": function () {
                        var _name = $('#dialog_chain_fullname').val().trim();

                        if (_name == "") {
                            alert("请填写商户全称");
                            return false;
                        }
                        for (var i = 0; i < chainList.length; i++) {
                            if (chainList[i].NAME == _name) {
                                $('#dialog_chain_notice').html("已经存在了同名的连锁店");
                                return false;
                            }
                        }
                        for (var i = 0; i < singleList.length; i++) {
                            if (singleList[i].NAME == _name) {
                                $('#dialog_chain_notice').html("已经存在了同名的单店");
                                return false;
                            }
                        }

                        var new_chain = $("#step2_fullname_chain option[value='-2']");
                        if (new_chain.length == 0) {
                            $("#step2_fullname_chain").append("<option value='-2'>" + _name + "</option>");
                        } else {
                            new_chain.html(_name);
                        }
                        $("#step2_fullname_chain").val(-2);
                        $('div[name="step2_more"]').show();
                        $('#step2_apalias_div').show();
                        $('#step2_ismanage_div').hide();
                        $('#step3_ad_single').hide();
                        $('#step3_ad_chain').hide();
                        $('#step2_nickname').val("");
                        $(this).dialog("close");
                    },
                    "取消": function () {
                        $(this).dialog("close");
                    }
                }
            });

            dialog_add_single = $("#dialog_add_single").dialog({
                autoOpen: false,
                height: 210,
                width: 300,
                modal: true,
                buttons: {
                    "确定": function () {
                        var _name = $('#dialog_single_fullname').val().trim();

                        if (_name == "") {
                            alert("请填写商户全称");
                            return false;
                        }

                        for (var i = 0; i < singleList.length; i++) {
                            if (singleList[i].NAME == _name) {
                                $('#dialog_single_notice').html("已经存在了同名的单店");
                                return false;
                            }
                        }
                        for (var i = 0; i < chainList.length; i++) {
                            if (chainList[i].NAME == _name) {
                                $('#dialog_single_notice').html("已经存在了同名的连锁店");
                                return false;
                            }
                        }

                        var new_single = $("#step2_fullname_single option[value='-2']");
                        if (new_single.length == 0) {
                            $("#step2_fullname_single").append("<option value='-2'>" + _name + "</option>");
                        } else {
                            new_single.html(_name);
                        }
                        $("#step2_fullname_single").val(-2);
                        $('div[name="step2_more"]').show();
                        $('#step2_apalias_div').show();
                        $('#step2_ismanage_div').hide();
                        $('#step3_ad_single').hide();
                        $('#step3_ad_chain').hide();
                        $('#step2_nickname').val("");
                        $(this).dialog("close");
                    },
                    "取消": function () {
                        $(this).dialog("close");
                    }
                }
            });

            dialog_ssid_pwd = $("#dialog_ssid_pwd").dialog({
                autoOpen: false,
                height: 200,
                width: 300,
                modal: true,
                buttons: {
                    "确定": function () {
                        var pwd = $('#step3_ssid_pwd').val();
                        if (pwd != "") {
                            if (pwd.length < 8 || pwd.length > 16) {
                                alert("密码长度必须在8位至16位之间");
                                return;
                            }
                            if (!isNumberOrLetter(pwd)) {
                                alert("密码只能包含数字和字母");
                                return;
                            }
                        }
                        var index = dialog_ssid_pwd.curIndex;
                        var _thisLi = $('li[name="ssid_item_' + index + '"]');
                        _thisLi.find('input').attr('pwd', pwd);
                        _thisLi.find('i').removeClass('fa-unlock').addClass('fa-lock');
                        _thisLi.find('.signal').addClass('hover');
                        $(this).dialog("close");
                    },
                    "取消": function () {
                        $(this).dialog("close");
                    }
                }
            });
        }

        //判断是否是数字或字母 
        function isNumberOrLetter(s) {
            var regu = "^[0-9a-zA-Z]+$";
            var re = new RegExp(regu);
            if (re.test(s)) {
                return true;
            } else {
                return false;
            }
        }

        function isNumber(s) {
            var regu = "^[0-9]+$";
            var re = new RegExp(regu);
            if (re.test(s))
                return true;
            else
                return false;
        }

        function isPhone(s) {
            var regu = "(^[0-9]{3,4}\-[0-9]{7,8}$)|(^[0-9]{7,8}$)|(^\([0-9]{3,4}\)[0-9]{3,8}$)|(^0{0,1}13[0-9]{9}$)";
            var re = new RegExp(regu);
            if (re.test(s))
                return true;
            else
                return false;
        }

        function isQQ(s) {
            var regu = "^[0-9]\\d{4,11}$";
            var re = new RegExp(regu);
            if (re.test(s))
                return true;
            else
                return false;
        }

        function initMap(_lng, _lat) {
            map = new BMap.Map('map', { enableMapClick: false });
            map.addControl(new BMap.NavigationControl());
            map.centerAndZoom(new BMap.Point(_lng, _lat), 15);
            map.enableScrollWheelZoom();
            map.addEventListener("click", function (e) {
                marker.setPosition(e.point);
                getGeoInfo(e.point.lng, e.point.lat);
            });

            marker = new BMap.Marker(new BMap.Point(_lng, _lat), { enableDragging: true, raiseOnDrag: true });
            marker.addEventListener("dragend", function (e) {
                getGeoInfo(e.point.lng, e.point.lat);
            });

            map.addOverlay(marker);
        }

        function getLocation() {
            $.ajax({
                url: 'http://api.map.baidu.com/location/ip?ak=10de0613056269b007c32743d586d9d7&coor=bd09ll',
                dataType: 'jsonp',
                success: function (obj) {
                    if (obj.status == 0) {
                        initMap(obj.content.point.x, obj.content.point.y);
                        var urlStr = "http://api.map.baidu.com/geocoder/v2/?output=json&ak=10de0613056269b007c32743d586d9d7&location=" + obj.content.point.y + "," + obj.content.point.x;
                        $.ajax({
                            url: urlStr,
                            dataType: 'jsonp',
                            success: function (obj) {
                                if (obj.status == 0) {
                                    //$('#step1_address_span').html(obj.result.formatted_address);
                                    $('#step1_address_span').html(obj.result.addressComponent.city + obj.result.addressComponent.district + obj.result.addressComponent.street);
                                }
                                else {
                                }
                            }
                        });
                    }
                    else {
                        $('#step1_address_span').html("未能获取到您的地理位置");
                    }
                }
            });

            //if (navigator.geolocation) {
            //  navigator.geolocation.getCurrentPosition(function (position) {
            //    var coords = position.coords;
            //    marker = new BMap.Marker(new BMap.Point(coords.longitude, coords.latitude), { enableDragging: true, raiseOnDrag: true });
            //    marker.addEventListener("dragend", function (e) {
            //      getGeoInfo(e.point.lng, e.point.lat);
            //    });
            //
            //    map.addOverlay(marker);
            //
            //    map.centerAndZoom(new BMap.Point(coords.longitude, coords.latitude), 15);
            //    map.enableScrollWheelZoom();
            //    map.addEventListener("click", function (e) {
            //      marker.setPosition(e.point);
            //      getGeoInfo(e.point.lng, e.point.lat);
            //    });
            //
            //    var urlStr = "http://api.map.baidu.com/geocoder/v2/?output=json&ak=10de0613056269b007c32743d586d9d7&location=" + coords.latitude + "," + coords.longitude;
            //    $.ajax({
            //
            //      url: urlStr,
            //      dataType: 'jsonp',
            //      success: function (obj) {
            //        if (obj.status == 0) {
            //          //$('#step1_address_span').html(obj.result.formatted_address);
            //          $('#step1_address_span').html(obj.result.addressComponent.city + obj.result.addressComponent.district + obj.result.addressComponent.street);
            //        }
            //        else {
            //        }
            //      }
            //    });
            //  },
            //      function (error) {
            //        switch (error.code) {
            //          case 1:
            //            alert("位置服务被拒绝。");
            //            break;
            //          case 2:
            //            alert("暂时获取不到位置信息。");
            //            break;
            //          case 3:
            //            alert("获取信息超时。");
            //            break;
            //          default:
            //            alert("未知错误。");
            //            break;
            //        }
            //        return false;
            //      });
            //} else {
            //  alert("你的浏览器不支持HTML5来获取地理位置信息。");
            //}
        }

        function getGeoInfo(_lon, _lat) {
            $('#step1_address_input').val("Loading");
            var urlStr = "http://api.map.baidu.com/geocoder/v2/?output=json&ak=10de0613056269b007c32743d586d9d7&location=" + _lat + "," + _lon;
            $.ajax({
                url: urlStr,
                dataType: 'jsonp',
                success: function (obj) {
                    if (obj.status == 0) {
                        $('#step1_address_input').val(obj.result.formatted_address);
                        //$('#step1_address_input').html(obj.result.addressComponent.city + obj.result.addressComponent.district + obj.result.addressComponent.street);
                    }
                    else {
                    }
                }
            });
        }

        function gotoStep(_dom_id, _this) {
            var tmp;
            if (_dom_id == "nav")
                tmp = "#setup_step1";
            else
                tmp = _dom_id;
            if (tmp > curStep) {
                switch (curStep) {
                    case "#setup_step1":
                        if (!checkstep1()) {
                            return;
                        }
                        all_info.AP_ADDRESS = $('#step1_address_input').val();
                        all_info.AP_LON = marker.getPosition().lat;
                        all_info.AP_LAT = marker.getPosition().lng;
                        //alert("保存1步");
                        $('#setup_step1').slideUp(300);
                        $('#setup_step2').slideDown(300);
                        break;
                    case "#setup_step2":
                        if (!checkstep2())
                            return;

                        all_info.ORG_TYPE = $('input[name="org_type"]:checked').val();
                        if (all_info.ORG_TYPE == "2") {
                            if ($('input[name="org_ismanage"]:checked').val() == "true") {
                                all_info.ORG_ISMANAGE = true;
                                all_info.ORG_PID = $('#step2_fullname_chain').val();
                                all_info.ORG_ID = -1;
                                all_info.ORG_FULLNAME = $('#step2_apalias').val();
                                all_info.AP_ALIAS = $('#step2_apalias').val();
                                all_info.ORG_SIMPLENAME = $('#step2_nickname').val();
                                all_info.ORG_INDUSTRY = $('#step2_industry').val();
                                all_info.ORG_AREA = $('#step2_area').val();
                                all_info.ORG_CONTACTER = $('#step2_contact').val();
                                all_info.ORG_CONTACT = $('#step2_phone').val();
                                all_info.ORG_QQ = $('#step2_qq').val();
                                all_info.ORG_WEIBO = $('#step2_weibo').val();
                                all_info.ORG_WEIXIN = $('#step2_weixin').val();
                                all_info.USER_ACCOUNT = $('#step2_admin').val();
                                all_info.USER_PWD = $('#step2_pwd').val();
                            }
                            else {
                                all_info.ORG_ISMANAGE = false;
                                if ($('#step2_fullname_chain').val() == "-2") {
                                    all_info.ORG_ID = -1;
                                    all_info.ORG_FULLNAME = $('#step2_fullname_chain').find('option:selected').html();
                                    all_info.AP_ALIAS = $('#step2_apalias').val();
                                    all_info.ORG_SIMPLENAME = $('#step2_nickname').val();
                                    all_info.ORG_INDUSTRY = $('#step2_industry').val();
                                    all_info.ORG_AREA = $('#step2_area').val();
                                    all_info.ORG_CONTACTER = $('#step2_contact').val();
                                    all_info.ORG_CONTACT = $('#step2_phone').val();
                                    all_info.ORG_QQ = $('#step2_qq').val();
                                    all_info.ORG_WEIBO = $('#step2_weibo').val();
                                    all_info.ORG_WEIXIN = $('#step2_weixin').val();
                                    all_info.USER_ACCOUNT = $('#step2_admin').val();
                                    all_info.USER_PWD = $('#step2_pwd').val();
                                } else {
                                    all_info.ORG_ID = $('#step2_fullname_chain').val();
                                    all_info.AP_ALIAS = $('#step2_apalias').val()
                                    all_info.ORG_SIMPLENAME = $('#step2_nickname').val();
                                    $('#step3_ad_chain').show();
                                    GetAPList(all_info.ORG_ID);
                                }
                            }
                        }
                        else if (all_info.ORG_TYPE == "3") {
                            $('#step3_ad_chain').hide();
                            if ($('#step2_fullname_single').val() == "-2") {
                                all_info.ORG_ID = -1;
                                all_info.ORG_FULLNAME = $('#step2_fullname_single').find('option:selected').html();
                                all_info.AP_ALIAS = $('#step2_apalias').val();
                                all_info.ORG_SIMPLENAME = $('#step2_nickname').val();
                                all_info.ORG_INDUSTRY = $('#step2_industry').val();
                                all_info.ORG_AREA = $('#step2_area').val();
                                all_info.ORG_CONTACTER = $('#step2_contact').val();
                                all_info.ORG_CONTACT = $('#step2_phone').val();
                                all_info.ORG_QQ = $('#step2_qq').val();
                                all_info.ORG_WEIBO = $('#step2_weibo').val();
                                all_info.ORG_WEIXIN = $('#step2_weixin').val();
                                all_info.USER_ACCOUNT = $('#step2_admin').val();
                                all_info.USER_PWD = $('#step2_pwd').val();
                            }
                            else if ($('#step2_fullname_single').val() > 0) {
                                all_info.ORG_ID = $('#step2_fullname_single').val();
                                all_info.ORG_SIMPLENAME = $('#step2_nickname').val();
                                all_info.AP_ALIAS = $('#step2_apalias').val();
                                GetAPList(all_info.ORG_ID);
                            }
                        }
                        $('#setup_step2').slideUp(300);
                        $('#setup_step3').slideDown(300);
                        $('input[name="ad_rdo"][value="custom"]').prop("checked", true);
                        $('#container_wifi').show();
                        $('#ap_list_div').hide();
                        if ($('#step3_ap_power_btn').hasClass('hover')) {
                            $('#step3_ap_power_btn').click();
                        }
                        break;
                    case "#setup_step3":
                        if (!checkstep3()) {
                            return;
                        }
                        all_info.DEFAULT = $('input[name="ad_rdo"]:checked').val() == undefined ? -1 : $('input[name="ad_rdo"]:checked').val();
                        if (all_info.DEFAULT != "custom" && all_info.DEFAULT != "agent")
                            all_info.DEFAULT = $('#ap_list_slt').val();

                        all_info.SSIDLIST = [];
                        if ($('#step3_ap_power_btn').hasClass('hover')) {
                            var obj_ssid;
                            $('#step3_ssid_list li').each(function () {
                                if ($(this).find('.button').hasClass('hover')) {
                                    obj_ssid = new Object();
                                    obj_ssid.ID = -1;
                                    obj_ssid.NAME = $(this).find('input').val();
                                    obj_ssid.ISON = true;
                                    obj_ssid.ISPWD = $(this).find('.signal').hasClass('hover');
                                    obj_ssid.PWD = $(this).find('input').attr('pwd') == undefined ? "" : $(this).find('input').attr('pwd');
                                    all_info.SSIDLIST.push(obj_ssid);
                                }
                            });
                        }

                        if ($(_this).html() == "下一步") {
                            $('#setup_step3').slideUp(300);
                            $('#setup_step4').slideDown(300);
                        }
                        break;
                    case "#setup_step4":
                        if (!checkstep4()) {
                            return;
                        }
                        for (var i = 0; i < all_info.SSIDLIST.length; i++) {
                            all_info.SSIDLIST[i].MAXLINKCOUNT = $('#step4_ssid_peoplenum').val().trim();
                            all_info.SSIDLIST[i].MAXUS = $('#step4_ssid_up').val().trim();
                            all_info.SSIDLIST[i].MAXDS = $('#step4_ssid_down').val().trim();
                        }
                        break;
                }
            }
            else {
                $(tmp).slideDown(300);
                $(curStep).slideUp(300);
            }

            if ($(_this).html() == "提交") {
                PostAllInfo(_this);

            }
            else {
                curStep = tmp;
            }
        }

        function CreateSSIDList(_ssidnum) {
            $('#step3_ssid_list').html("");
            var str = "<ul>";

            for (var i = 0; i < _ssidnum; i++) {
                str += "<li name='ssid_item_" + i + "'><div class='signal'></div><span>";
                str += "<input type='text' class='input-sm' value='【" + all_info.ORG_SIMPLENAME + "】' " + (i == 0 ? "" : "disabled") + ">";
                str += "<button type='button' class='btn btn-default' tag='" + i + "' " + (i == 0 ? "" : "disabled") + "><span class='btn-icon'><i class='fa fa-fw fa-unlock'></i></span></button></span>";
                str += "<div class='button  " + (i == 0 ? "hover" : "") + "' tag='" + i + "'></div>";
                if (_ssidnum != 1 && i < _ssidnum - 1) {
                    str += "<div class='line'></div>";
                }
                str += "</li>";
            }
            str += "</ul>";
            $('#step3_ssid_list').html(str);
            $('#step3_ssid_list').slideToggle(100);
            bindSSIDListPowerClick();
            bindSSIDSecurityClick();
        }

        function bindSSIDListPowerClick() {
            $('#step3_ssid_list').find('.button').click(function () {
                var _this = $(this);
                var index = _this.attr("tag");
                var _thisLi = $('li[name="ssid_item_' + index + '"]');

                if (_this.hasClass('hover')) {
                    _thisLi.find('button').attr("disabled", "");
                    _thisLi.find('input').attr("disabled", "");
                } else {
                    _thisLi.find('button').removeAttr("disabled");
                    _thisLi.find('input').removeAttr("disabled");
                }
                $(this).toggleClass("hover");
            });
        }

        function bindSSIDSecurityClick() {
            $('#step3_ssid_list').find('button').click(function () {
                var _this = $(this);
                var index = _this.attr("tag");
                var _thisLi = $('li[name="ssid_item_' + index + '"]');
                if (_this.find('i').hasClass('fa-unlock')) {
                    dialog_ssid_pwd.curIndex = index;
                    dialog_ssid_pwd.dialog("open");
                } else {
                    _this.find('i').removeClass('fa-lock').addClass('fa-unlock');
                    _thisLi.find('.signal').removeClass('hover');
                    _thisLi.find('input').removeAttr('pwd');
                }
            });
        }

        function checkstep1() {
            if ($('#step1_address_input').val().trim() == "") {
                alert("请输入门店所在地址");
                return false;
            }
            if (marker == null) {
                alert("请在地图上选择点位");
                return false;
            }
            return true;
        }

        function checkstep2() {
            if($('#step2_fullname_single').is(':visible')){
                if ($('#step2_fullname_single').val() == "-1") {
                    alert("请选择商户全称");
                    $('#step2_fullname_single').focus();
                    return false;
                }
            }
            if($('#step2_fullname_chain').is(':visible')){
                if ($('#step2_fullname_chain').val() == "-1") {
                    alert("请选择商户全称");
                    $('#step2_fullname_chain').focus();
                    return false;
                }
            }
            if ($('input[name="org_ismanage"]').is(':visible')) {
                if ($('input[name="org_ismanage"]:checked').val() == undefined) {
                    alert("请选择是独立管理还是统一管理");
                    return false;
                }
            }
            if ($('#step2_apalias').is(':visible')) {
                if (!$('#step2_apalias').parent().parent().hasClass('has-success')) {
                    $('#step2_apalias').focus();
                    return false;
                }
            }
            if ($('#step2_nickname').is(':visible')) {
                if (!$('#step2_nickname').parent().parent().hasClass('has-success')) {
                    $('#step2_nickname').focus();
                    return false;
                }
            }
            if ($('#step2_industry').is(':visible')) {
                if (!$('#step2_industry').parent().hasClass('has-success')) {
                    $('#step2_industry').focus();
                    return false;
                }
            }
            if ($('#step2_area').is(':visible')) {
                if (!$('#step2_area').parent().hasClass('has-success')) {
                    $('#step2_area').focus();
                    return false;
                }
            }
            if ($('#step2_contact').is(':visible')) {
                if (!$('#step2_contact').parent().parent().hasClass('has-success')) {
                    $('#step2_contact').focus();
                    return false;
                }
            }

            if ($('#step2_phone').is(':visible')) {
                if (!$('#step2_phone').parent().parent().hasClass('has-success')) {
                    $('#step2_phone').focus();
                    return false;
                }
            }

            if ($('#step2_qq').is(':visible')) {
                if (!$('#step2_qq').parent().parent().hasClass('has-success')) {
                    $('#step2_qq').focus();
                    return false;
                }
            }
            if ($('#step2_weibo').is(':visible')) {
                if (!$('#step2_weibo').parent().parent().hasClass('has-success')) {
                    $('#step2_weibo').focus();
                    return false;
                }
            }
            if ($('#step2_weixin').is(':visible')) {
                if (!$('#step2_weixin').parent().parent().hasClass('has-success')) {
                    $('#step2_weixin').focus();
                    return false;
                }
            }
            if ($('#step2_admin').is(':visible')) {
                if (!$('#step2_admin').parent().parent().parent().hasClass('has-success')) {
                    $('#step2_admin').focus();
                    return false;
                }
            }
            if ($('#step2_pwd').is(':visible')) {
                if (!$('#step2_pwd').parent().parent().hasClass('has-success')) {
                    $('#step2_pwd').focus();
                    return false;
                }
            }
            return true;

//            if ($('input[name="org_type"]:checked').val() == "2") {
//                // 没有选择连锁商户全称
//                if ($('#step2_fullname_chain').val() == "-1") {
//                    alert("请选择商户全称");
//                    $('#step2_fullname_chain').focus();
//                    return false;
//                } // 新增的连锁店检查
//                else if ($('#step2_fullname_chain').val() == "-2") {
//                    if ($('#step2_apalias').val().trim() == "") {
//                        alert("请填写设备别名");
//                        $('#step2_apalias').focus();
//                        return false;
//                    }
//                    if ($('#step2_nickname').val().trim() == "") {
//                        alert("请填写商户简称");
//                        $('#step2_nickname').focus();
//                        return false;
//                    }
//                    if ($('#step2_industry').val() < 0) {
//                        alert("请选择行业");
//                        $('#step2_industry').focus();
//                        return false;
//                    }
//                    if ($('#step2_area').val() < 0) {
//                        alert("请选择营业面积");
//                        $('#step2_area').focus();
//                        return false;
//                    }
//                    if ($('#step2_contact').val().trim() == "") {
//                        alert("请填写联系人");
//                        $('#step2_contact').focus();
//                        return false;
//                    }
//                    if ($('#step2_phone').val().trim() == "") {
//                        alert("请填写联系电话");
//                        $('#step2_phone').focus();
//                        return false;
//                    }
//                    if ($('#step2_qq').val().trim() == "") {
//                        alert("请填写联系QQ");
//                        $('#step2_qq').focus();
//                        return false;
//                    }
//                    if ($('#step2_weibo').val().trim() == "") {
//                        alert("请填写企业微博号");
//                        $('#step2_weibo').focus();
//                        return false;
//                    }
//                    if ($('#step2_weixin').val().trim() == "") {
//                        alert("请填写企业微信号");
//                        $('#step2_weixin').focus();
//                        return false;
//                    }
//                    if ($('#step2_admin').val().trim() == "") {
//                        alert("请填写管理帐号");
//                        $('#step2_admin').focus();
//                        return false;
//                    }
//                    if (!$('#step2_admin').parent().parent().hasClass('has-success')) {
//                        alert("管理帐号有误，长度6-32位请重新输入");
//                        $('#step2_admin').focus();
//                        return false;
//                    }
//                    if ($('#step2_pwd').val().trim() == "") {
//                        alert("请填写管理密码");
//                        $('#step2_pwd').focus();
//                        return false;
//                    }
//                    return true;
//                }

//                if ($('input[name="org_ismanage"]:checked').val() == undefined) {
//                    alert("请选择是独立管理还是统一管理");
//                    return false;
//                }
//                else if ($('input[name="org_ismanage"]:checked').val() == "false") {
//                    if ($('#step2_apalias').val().trim() == "") {
//                        alert("请填写门店名称");
//                        $('#step2_apalias').focus();
//                        return false;
//                    }
//                } else if ($('input[name="org_ismanage"]:checked').val() == "true") {
//                    if ($('#step2_nickname').val().trim() == "") {
//                        alert("请填写商户简称");
//                        $('#step2_nickname').focus();
//                        return false;
//                    }
//                    if ($('#step2_industry').val() < 0) {
//                        alert("请选择行业");
//                        $('#step2_industry').focus();
//                        return false;
//                    }
//                    if ($('#step2_area').val() < 0) {
//                        alert("请选择营业面积");
//                        $('#step2_area').focus();
//                        return false;
//                    }
//                    if ($('#step2_contact').val().trim() == "") {
//                        alert("请填写联系人");
//                        $('#step2_contact').focus();
//                        return false;
//                    }
//                    if ($('#step2_phone').val().trim() == "") {
//                        alert("请填写联系电话");
//                        $('#step2_phone').focus();
//                        return false;
//                    }
//                    if ($('#step2_qq').val().trim() == "") {
//                        alert("请填写联系QQ");
//                        $('#step2_qq').focus();
//                        return false;
//                    }
//                    if ($('#step2_weibo').val().trim() == "") {
//                        alert("请填写企业微博号");
//                        $('#step2_weibo').focus();
//                        return false;
//                    }
//                    if ($('#step2_weixin').val().trim() == "") {
//                        alert("请填写企业微信号");
//                        $('#step2_weixin').focus();
//                        return false;
//                    }
//                    if ($('#step2_admin').val().trim() == "") {
//                        alert("请填写管理帐号");
//                        $('#step2_admin').focus();
//                        return false;
//                    }
//                    if (!$('#step2_admin').parent().parent().hasClass('has-success')) {
//                        alert("管理帐号有误，长度6-32位请重新输入");
//                        $('#step2_admin').focus();
//                        return false;
//                    }
//                    if ($('#step2_pwd').val().trim() == "") {
//                        alert("请填写管理密码");
//                        $('#step2_pwd').focus();
//                        return false;
//                    }
//                }
//            }
//            else if ($('input[name="org_type"]:checked').val() == "3") {
//                if ($('#step2_fullname_single').val() == "-1") {
//                    alert("请选择商户全称");
//                    $('#step2_fullname_single').focus();
//                    return false;
//                }
//                else if ($('#step2_fullname_single').val() == "-2") {

//                    if ($('#step2_apalias').val().trim() == "") {
//                        alert("请填写商户简称");
//                        $('#step2_apalias').focus();
//                        return false;
//                    }

//                    if ($('#step2_nickname').val().trim() == "") {
//                        alert("请填写商户简称");
//                        $('#step2_nickname').focus();
//                        return false;
//                    }
//                    if ($('#step2_industry').val() < 0) {
//                        alert("请选择行业");
//                        $('#step2_industry').focus();
//                        return false;
//                    }
//                    if ($('#step2_area').val() < 0) {
//                        alert("请选择营业面积");
//                        $('#step2_area').focus();
//                        return false;
//                    }
//                    if ($('#step2_contact').val().trim() == "") {
//                        alert("请填写联系人");
//                        $('#step2_contact').focus();
//                        return false;
//                    }
//                    if ($('#step2_phone').val().trim() == "" || !isPhone($('#step2_phone').val().trim())) {
//                        alert("请填写联系电话，手机应11位数字，固定电话以区号开头");
//                        $('#step2_phone').focus();
//                        return false;
//                    }
//                    if ($('#step2_qq').val().trim() == "" || !isQQ($('#step2_qq').val().trim())) {
//                        alert("请填写联系QQ，QQ号为5-13位数字");
//                        $('#step2_qq').focus();
//                        return false;
//                    }
//                    if ($('#step2_weibo').val().trim() == "") {
//                        alert("请填写企业微博号");
//                        $('#step2_weibo').focus();
//                        return false;
//                    }
//                    if ($('#step2_weixin').val().trim() == "") {
//                        alert("请填写企业微信号");
//                        $('#step2_weixin').focus();
//                        return false;
//                    }
//                    if ($('#step2_admin').val().trim() == "") {
//                        alert("请填写管理帐号");
//                        $('#step2_admin').focus();
//                        return false;
//                    }
//                    if (!$('#step2_admin').parent().parent().hasClass('has-success')) {
//                        alert("管理帐号有误，长度6-32位请重新输入");
//                        $('#step2_admin').focus();
//                        return false;
//                    }
//                    if ($('#step2_pwd').val().trim() == "") {
//                        alert("请填写管理密码");
//                        $('#step2_pwd').focus();
//                        return false;
//                    }
//                }
//                else {
//                    if ($('#step2_apalias').val().trim() == "") {
//                        alert("请填写设备别称");
//                        $('#step2_apalias').focus();
//                        return false;
//                    }
//                }
//            }
//            return true;
        }

        function checkstep3() {
            if ($('input[name="ad_rdo"]:visible').length > 0) {
                if ($('input[name="ad_rdo"]:checked').val() == undefined) {
                    alert("请选择默认设置");
                    return false;
                }

                if ($('input[name="ad_rdo"]:checked').val() == "single" || $('input[name="ad_rdo"]:checked').val() == "chain") {
                    if ($('#ap_list_slt').val() < 0) {
                        alert("请选择默认设置");
                        return false;
                    }
                }
            }
            return true;
        }

        function checkstep4() {
            if ($('#step4_ssid_peoplenum').val().trim() == "" || !isNumber($('#step4_ssid_peoplenum').val()) || $('#step4_ssid_peoplenum').val() < 0) {
                alert('请输入并发上网人数并且必须是正整数');
                $('#step3_ssid_peoplenum').focus();
                return false;
            }
            if ($('#step4_ssid_up').val().trim() == "" || !isNumber($('#step4_ssid_up').val()) || $('#step4_ssid_up').val() < 0) {
                alert('请输入最大上行带宽并且必须是正整数');
                $('#step3_ssid_up').focus();
                return false;
            }
            if ($('#step4_ssid_down').val().trim() == "" || !isNumber($('#step4_ssid_down').val()) || $('#step4_ssid_down').val() < 0) {
                alert('请输入最大下行带宽并且必须是正整数');
                $('#step4_ssid_down').focus();
                return false;
            }
            return true;
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
                        $("#step2_industry").empty();
                        $("#step2_industry").append("<option value='-1'>请选择</option>")
                        for (var i = 0; i < obj.ResultOBJ.length; i++) {
                            $("#step2_industry").append("<option value='" + obj.ResultOBJ[i].VALUE + "'>" + obj.ResultOBJ[i].NAME + "</option>");
                        }
                    } else if (obj.ResultCode == -100) {
                        alert(obj.ResultMsg);
                        window.location.href = "LoginInstall.aspx?mac=" + all_info.AP_MAC;
                    }
                }
            });
        }

        function GetAreaList() {
            $.ajax({
                type: 'post',
                url: 'AjaxComm.aspx',
                data: 'type=GetDicByMianJi&token=' + token,
                dataType: 'json',
                error: function (msg) {
                    //alert("服务器错误");
                },
                success: function (obj) {
                    if (obj.ResultCode == 0) {
                        $("#step2_area").empty();
                        $("#step2_area").append("<option value='-1'>请选择</option>")
                        for (var i = 0; i < obj.ResultOBJ.length; i++) {
                            $("#step2_area").append("<option value='" + obj.ResultOBJ[i].VALUE + "'>" + obj.ResultOBJ[i].NAME + "</option>");
                        }
                    } else {
                        //alert(obj.ResultMsg);
                    }
                }
            });
        }

        function GetChainList() {
            $.ajax({
                type: 'post',
                url: 'AjaxComm.aspx',
                data: 'type=GetChainList&token=' + token,
                dataType: 'json',
                error: function (msg) {
                    //alert("服务器错误");
                },
                success: function (obj) {
                    if (obj.ResultCode == 0) {
                        chainList = obj.ResultOBJ;
                        $("#step2_fullname_chain").empty();
                        if (obj.ResultOBJ.length == 0) {
                            $("#step2_fullname_chain").append("<option value='-1'>请添加连锁店</option>")
                        } else {
                            $("#step2_fullname_chain").append("<option value='-1'>请选择</option>")
                        }
                        for (var i = 0; i < obj.ResultOBJ.length; i++) {
                            $("#step2_fullname_chain").append("<option value='" + obj.ResultOBJ[i].ID + "'>" + obj.ResultOBJ[i].NAME + "</option>");
                        }
                    } else {
                        //alert(obj.ResultMsg);
                    }
                }
            });
        }

        function GetSingleList() {
            $.ajax({
                type: 'post',
                url: 'AjaxComm.aspx',
                data: 'type=GetSingleList&token=' + token,
                dataType: 'json',
                error: function (msg) {
                    //alert("服务器错误");
                },
                success: function (obj) {
                    if (obj.ResultCode == 0) {
                        singleList = obj.ResultOBJ;
                        $("#step2_fullname_single").empty();
                        if (obj.ResultOBJ.length == 0) {
                            $("#step2_fullname_single").append("<option value='-1'>请添加单店</option>")
                        } else {
                            $("#step2_fullname_single").append("<option value='-1'>请选择</option>")
                        }

                        for (var i = 0; i < obj.ResultOBJ.length; i++) {
                            $("#step2_fullname_single").append("<option value='" + obj.ResultOBJ[i].ID + "'>" + obj.ResultOBJ[i].NAME + "</option>");
                        }
                    } else {
                        //alert(obj.ResultMsg);
                    }
                }
            });
        }

        function GetSSIDNum() {
            $.ajax({
                type: 'post',
                url: 'AjaxComm.aspx',
                data: 'type=APManage/GetSSIDNumByMAC&token=' + token + '&param="' + all_info.AP_MAC + '"',
                dataType: 'json',
                error: function (msg) {
                    //alert("服务器错误");
                    $(".load img").hide();
                    $('#step3_ssid_list').slideToggle(100);
                },
                success: function (obj) {
                    $(".load img").hide();
                    if (obj.ResultCode == 0) {
                        if (isLoading) {
                            if (obj.ResultOBJ < 0) {
                                $('#step3_ssid_list').slideToggle(100);
                                $('#step3_ssid_list').html("网络错误，请尝试重新开启设备");
                            } else {
                                CreateSSIDList(obj.ResultOBJ);
                            }
                        }
                    } else {
                        $('#step3_ssid_list').slideToggle(100);
                        $('#step3_ssid_list').html("");
                        alert(obj.ResultMsg);
                    }
                }
            });
        }

        function GetOrgType() {
            $.ajax({
                type: 'post',
                url: 'AjaxComm.aspx',
                data: 'type=GetOrgType&token=' + token,
                dataType: 'json',
                error: function (msg) {
                    //alert("服务器错误");
                    my_otype[0] = -1;
                },
                success: function (obj) {
                    if (obj.ResultCode == 0) {
                        var r = obj.ResultOBJ.split(',');
                        my_otype[0] = r[0];
                        my_otype[1] = r[1];
                        if (r[0] == 1 && r[1] == 1) {
                            $('#step3_ad_agent').show();
                        } else {
                            $('#step3_ad_agent').hide();
                        }
                    } else {
                        my_otype[0] = -1;
                    }
                }
            });
        }

        function GetInstallUser() {
            var username = decodeURIComponent(GetQueryString("username"));
            $("span[name='username']").html(username);
        }

        function GetAPList(_oid) {
            $.ajax({
                type: 'post',
                url: 'AjaxComm.aspx',
                data: 'type=APManage/GetAPListByOID&token=' + token + '&param=' + _oid,
                dataType: 'json',
                error: function (msg) {

                },
                success: function (obj) {
                    if (obj.ResultCode == 0) {
                        $("#ap_list_slt").empty();
                        $("#ap_list_slt").append("<option value='-1'>请选择</option>")
                        for (var i = 0; i < obj.ResultOBJ.length; i++) {
                            $("#ap_list_slt").append("<option value='" + obj.ResultOBJ[i].ID + "'>" + obj.ResultOBJ[i].ALIAS + "</option>");
                        }
                    }
                }
            });
        }

        function PostAllInfo(_btn) {
            $(_btn).html("提交中");
            $(_btn).attr("disabled", "");
            $.ajax({
                type: 'post',
                url: 'AjaxComm.aspx',
                data: 'type=Install&token=' + token + '&param=' + JSON.stringify(all_info),
                dataType: 'json',
                error: function (msg) {
                    $(_btn).html("提交");
                    $(_btn).removeAttr("disabled");
                },
                success: function (obj) {
                    if (obj.ResultCode == 0) {
                        alert("该设备已经安装完毕，如需修改广告，请您登录到萝卜云平台(http://ad.next-wifi.com/ui/login.aspx进行相关操作)");
                        $(_btn).html("提交完成");
                    } else {
                        $(_btn).html("提交");
                        $(_btn).removeAttr("disabled");
                        alert(obj.ResultMsg);
                    }

                }
            });
        }
    </script>

  <style>
    html
    {
        <%-- overflow:hidden;--%>
    }
    .setup-step
    {
        font-size:1.2em;
    }
    .setup-step form
    {
        font-size:1.1em;
    }
    .form-group
    {
        margin-bottom:8px;
    }
    .notice
    {
        color:Red;
        font-weight:bold;
        font-size:12px;
    }
    .step-title
    {
        font-size:1.5em;
    }
  </style>

  <style>
    .clear{
	    clear:both;
    }

    .container_wifi *{
	    margin:0;
	    padding:0;
    }

    .outline,.list{
	    width:auto;
	    margin:20px auto;
	    height:auto;
	    border:2px solid #A1A7AE;
	    background:#FFF;
	    border-radius:20px; 
    }

    .outline span{
	    float:left;
	    margin:auto 20px;
	    font-family:"黑体";
	    font-weight:600;
	    height:48px;
	    line-height:48px;
	    letter-spacing:2px;
    }

    .button{
	    float:right;
	    background:url(/UITemplet/img/wifi/button.png);
	    background-position:-51px 0;
	    width:78px;
	    height:28px;
	    margin:10px 5px;
	    cursor:pointer;
	    -webkit-transition:0.3s ease-in-out;
	    -moz-transition:0.3s ease-in-out;
	    border-radius:32px;
    }

    .button.hover{
	    background-position:0 0;
    }

    .buttonIE{
	    float:right;
	    background:url(/UITemplet/img/wifi/msie.png);
	    background-position:-78px 0;
	    width:78px;
	    height:28px;
	    margin:15px 5px;
	    cursor:pointer;
    }

    .load{
	    width:auto;
	    height:30px;
	    margin:0 auto;
	    display:none;
    }

    .load span{
	    float:left;
	    margin:0 20px;
	    font-family:"微软雅黑";
	    line-height:30px;
	    letter-spacing:2px;
	    color:#4C566C;
	    text-shadow:0 2px #E7EAED;
    }

    .load img{
	    float:left;
	    margin:0 10px;
    }

    .list{
	    margin:20px auto;	    
	    height:auto;
	    display:none;
    }


    .list span{
	    float:left;
	    font:bold 32px "微软雅黑";
	    line-height:34px;
	    height:48px;
	    letter-spacing:2px;
    }
    .list span input{
	    font-size: 14px;
      height: 37px;
      margin-top: 5px;
      border: 1px solid #ccc;
      border-radius: 4px 0px 0px 4px;
      border-radius
      -webkit-box-shadow: inset 0 1px 1px rgba(0,0,0,.075);
      box-shadow: inset 0 1px 1px rgba(0,0,0,.075);
      -webkit-transition: border-color ease-in-out .15s,-webkit-box-shadow ease-in-out .15s;
      -o-transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;
      transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;
    }
    .list span button{
	    font-size: 17px;
      height: 37px;
      margin-top: 7px;     
      margin-left: -1px;
      padding: 3px 5px;
      border-radius: 0px 4px 4px 0px;
      -webkit-box-shadow: inset 0 1px 1px rgba(0,0,0,.075);
      box-shadow: inset 0 1px 1px rgba(0,0,0,.075);
      -webkit-transition: border-color ease-in-out .15s,-webkit-box-shadow ease-in-out .15s;
      -o-transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;
      transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;
    }


    .list ul{
	    list-style:none;
	    margin:0 auto;
	    padding:0;
    }

    .list li{
	    position:relative;
	    height:48px;
    }
    
    .line{
	    position:relative;
	    top:48px;
	    height:2px;
	    background:#A1A7AE;
    }

    .direct{
	    display:block;
	    float:right;
	    margin:20px;
	    border-radius:30px;
    }

    .signal{
	    background:url(/UITemplet/img/wifi/signal.png);
	    display:block;
	    float:left;
	    margin:4px 10px;
	    width:40px;
	    height:40px;
    }
    .signal.hover
    {
     background:url(/UITemplet/img/wifi/signal_lock.png);   
    }
  </style>
</head>
<body>
  <nav class="nav navbar-default" role="navigation">
    <div class="navbar-header" id="top-bar">
      <a class="navbar-brand" id="logo" href="#"><span class="label label-default">萝卜wifi云平台 V1.68</span></a>
      <div class="navbar-brand" style="font-size:12px;">欢迎您，<span name="username" style="color:Red;"></span></div>
      <div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
        <ul class="nav navbar-nav navbar-right">
          <li class="dropdown"><a href="#" class="dropdown-toggle" data-toggle="dropdown"><i
            class="fa fa-list-ul"></i><b class="caret"></b></a>
            <ul class="dropdown-menu">
              <li><a><i class="fa fa-user"></i><span name="username"></span></a></li>
              <li><a id="flset"><i class="fa fa-cogs"></i>参数设置</a></li>
              <li><a href="javascript:flagdisplay('aboutUs')"><i class="fa fa-group"></i>关于我们</a></li>
              <li><a href="javascript:flagdisplay('xgpass');"><i class="fa fa-key"></i>修改密码</a></li>
              <li><a href="javascript:exituser();"><i class="fa fa-sign-out"></i>退出</a></li>
            </ul>
          </li>
        </ul>
      </div>
    </div>
  </nav>
  <div class="container-fluid">
    <div class="row row-body">

<%--      <div class="col-md-4">
        <div class="panel setup-first">
          <div style="font-size:2em;margin-left:10px;">欢迎安装使用LuoBo WiFi</div>
        </div>
      </div>--%>

      <div class="col-md-12" style="padding:0px;">
        <!-- 第一步 -->
        <div class="row">
          <div class="col-md-12">
            <div class="panel panel-default setup-first" id="setup_step1">
              <div class="panel-heading">
                <div class="step-title">第一步：设定地理位置信息</div>
              </div>
              <div class="panel-body">
              <div class="setup-step">
                <div>您现在位于<span id="step1_address_span" style="color:Red">Loading</span>附近，请输入具体门牌号(可以在地图上直接点击您门店的位置)</div>
                <div class="form-inline" style="margin-top:10px;">
                  
                  <label class="sr-only" for="step1_address_input">地址</label>
                  <input type="text" class="form-control" style="width:80%;" id="step1_address_input" value="" placeholder="门店地址 / Address" />
                  
                  <button type="button" class="btn btn-default" onclick="javascript:gotoStep('#setup_step2')">下一步</button>
                </div>
                <div id="map" style="width:100%;height:500px;padding:10px;"></div>
              </div>
              </div>
              
            </div>
          </div>
        </div>
        <!-- 第二步 -->
        <div class="row">
          <div class="col-md-12">
            <div class="panel panel-default setup" id="setup_step2" style="display:none;">
              
                <div class="panel-heading">
                  <div class="step-title">第二步：设定使用者信息</div>
                </div>
                <div class="panel-body">
                  <div class="setup-step">
                    <div class="form-horizontal">
                      <div class="form-group">
                        <div class="col-sm-2"></div>
                        <div class="col-sm-4">
                        <label class="radio-inline">
                          <input type="radio" name="org_type" value="3" checked> 单个商户
                        </label>
                        <label class="radio-inline">
                          <input type="radio" name="org_type" value="2"> 连锁商户
                        </label>
                        </div>
                        <div class="col-sm-6">
                        </div>
                      </div>

                      <div class="form-group">
                        <label for="step2_fullname" class="col-sm-2 control-label">商户全称</label>
                        <div class="col-sm-4">
                          <%--<input type="text" class="form-control input-sm" id="step2_fullname_single">--%>
                          <select class="form-control input-sm" id="step2_fullname_single"><option>加载中</option></select>
                          <select class="form-control input-sm" id="step2_fullname_chain" style="display:none;"><option>加载中</option></select>
                        </div>
                        <div class="col-sm-6">
                          <button type="button" class="btn btn-default" id="step2_add_single">新增单店商户</button>
                          <button type="button" class="btn btn-default" id="step2_add_chain" style="display:none;">新增连锁商户</button>
                        </div>
                      </div>

                      <div class="form-group" id="step2_ismanage_div" style="display:none">
                        <div class="col-sm-2"></div>
                        <div class="col-sm-4">
                          <label class="radio-inline">
                            <input type="radio" name="org_ismanage" value="false"> 设备统一管理
                          </label>
                          <label class="radio-inline">
                            <input type="radio" name="org_ismanage" value="true"> 设备独立管理
                          </label>
                        </div>
                        <div class="col-sm-6">
                        </div>
                      </div>
                      
                      <div class="form-group has-feedback" id="step2_apalias_div" style="display:none">
                        <label for="step2_contact" class="col-sm-2 control-label">设备别称</label>
                        <div class="col-sm-4">
                          <input type="text" class="form-control input-sm" id="step2_apalias" placeholder="设备别称 / Alias" >
                          <span class="glyphicon form-control-feedback"></span>
                        </div>
                        <div class="col-sm-6">
                          <span class="notice">重要，请认真填写，用于在设备管理时进行区分</span>
                        </div>
                      </div>

                      <div class="form-group has-feedback" name="step2_more">
                        <label for="step2_nickname" class="col-sm-2 control-label">商户简称</label>
                        <div class="col-sm-4">
                          <input type="text" class="form-control input-sm" id="step2_nickname" placeholder="商户简称 / Abbreviation" >
                          <span class="glyphicon form-control-feedback"></span>
                        </div>
                        <div class="col-sm-6">
                          <span class="notice">重要，请认真填写，用于生成下一步的广告名称</span>
                        </div>
                      </div>
                      <div class="form-group" name="step2_more">
                        <label for="step2_industry" class="col-sm-2 control-label">所属行业</label>
                        <div class="col-sm-4">
                          <select class="form-control input-sm" id="step2_industry"><option value="-2">加载中</option></select>
                        </div>
                        <div class="col-sm-6">
                          <span class="notice">重要，请认真填写，用于自动与周围商圈交换广告</span>
                        </div>
                      </div>
                      <div class="form-group" name="step2_more">
                        <label for="step2_area" class="col-sm-2 control-label">营业面积</label>
                        <div class="col-sm-4">
                          <select class="form-control input-sm" id="step2_area"><option value="-2">加载中</option></select>
                        </div>
                        <div class="col-sm-6">
                          <span class="notice">重要，请认真填写，用于调整WiFi发射功率覆盖面积</span>
                        </div>
                      </div>
                      <div class="form-group has-feedback" name="step2_more">
                        <label for="step2_contact" class="col-sm-2 control-label">联系人</label>
                        <div class="col-sm-4">
                          <input type="text" class="form-control input-sm" id="step2_contact" placeholder="联系人 / Contact" >
                          <span class="glyphicon form-control-feedback"></span>
                        </div>
                        <div class="col-sm-6">
                          <span class="notice">重要，请认真填写，用于售后服务联系</span>
                        </div>
                      </div>
                      <div class="form-group has-feedback" name="step2_more">
                        <label for="step2_phone" class="col-sm-2 control-label">联系电话</label>
                        <div class="col-sm-4">
                          <input type="text" class="form-control input-sm" id="step2_phone" placeholder="联系电话 / Phone" >
                          <span class="glyphicon form-control-feedback"></span>
                        </div>
                        <div class="col-sm-6">
                          <span class="notice">重要，请认真填写，用于售后服务联系</span>
                        </div>
                      </div>
                      <div class="form-group has-feedback" name="step2_more">
                        <label for="step2_qq" class="col-sm-2 control-label">联系QQ</label>
                        <div class="col-sm-4">
                          <input type="text" class="form-control input-sm" id="step2_qq" placeholder="联系QQ / QQ" >
                          <span class="glyphicon form-control-feedback"></span>
                        </div>
                        <div class="col-sm-6">
                          <span class="notice">重要，请认真填写，用于售后服务联系</span>
                        </div>
                      </div>
                      <div class="form-group has-feedback" name="step2_more">
                        <label for="step2_weibo" class="col-sm-2 control-label">企业微博号</label>
                        <div class="col-sm-4">
                          <input type="text" class="form-control input-sm" id="step2_weibo" placeholder="企业微博号 / Sina Weibo" >
                          <span class="glyphicon form-control-feedback"></span>
                        </div>
                        <div class="col-sm-6">
                          <span class="notice">重要，请认真填写，用于帮助企业进行微博营销</span>
                        </div>
                      </div>
                      <div class="form-group has-feedback" name="step2_more">
                        <label for="step2_weixin" class="col-sm-2 control-label">企业微信号</label>
                        <div class="col-sm-4">
                          <input type="text" class="form-control input-sm" id="step2_weixin" placeholder="企业微信号 / Tencent Weixin" >
                          <span class="glyphicon form-control-feedback"></span>
                        </div>
                        <div class="col-sm-6" name="step2_more">
                          <span class="notice">重要，请认真填写，用于帮助企业进行微信营销</span>
                        </div>
                      </div>
                      <div class="form-group has-feedback" name="step2_more">
                        <label for="step2_admin" class="col-sm-2 control-label">管理员帐号</label>
                        <div class="col-sm-4">
                          <div class="input-group">
                          <input type="text" class="form-control input-sm" id="step2_admin" placeholder="管理员帐号 / Account" >
                          <span class="glyphicon form-control-feedback"></span>
                          <div class="input-group-addon">@next-wifi.com&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</div>
                          </div>
                        </div>
                        <div class="col-sm-6" name="step2_more">
                          <span class="notice">重要，请认真填写，用于帮助企业进行WiFi管理</span>
                        </div>
                      </div>
                      <div class="form-group has-feedback" name="step2_more">
                        <label for="step2_pwd" class="col-sm-2 control-label">管理员密码</label>
                        <div class="col-sm-4">                        
                          <input type="password" class="form-control input-sm" id="step2_pwd" placeholder="管理员密码 / Password" >
                          <span class="glyphicon form-control-feedback"></span>
                        </div>
                        <div class="col-sm-6">
                          <span class="notice">重要，请认真填写，用于帮助企业进行WiFi管理</span>
                        </div>
                      </div>
                      <div class="form-group">
                        <div class="col-sm-offset-2 col-sm-10">
                          <button type="button" class="btn btn-default" onclick="javascript:gotoStep('nav')">上一步</button>
                          <button type="button" class="btn btn-default" onclick="javascript:gotoStep('#setup_step3')">下一步</button>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
            </div>
          </div>
        </div>
        <!-- 第三步开始 -->
        <div class="row">
          <div class="col-md-12">
            <div class="panel panel-default setup" id="setup_step3" style="display:none;">
                <div class="panel-heading">
                  <div class="step-title">第三步：设定推广信息 &mdash; 广告服务</div>
                </div>
                <div class="panel-body">
                <div class="setup-step">
                  <div class="radio">
                    <label>
                      <input type="radio" name="ad_rdo" value="custom" checked>
                      使用自定义设置
                    </label>
                  </div>
                  <div class="radio" id="step3_ad_agent" style="display:none;">
                    <label>
                      <input type="radio" name="ad_rdo" value="agent">
                      使用代理商默认设置
                    </label>
                  </div>
                  <div class="radio" id="step3_ad_chain" style="display:none;">
                    <label>
                      <input type="radio" name="ad_rdo" value="chain" >
                      使用其他门店路由器设置
                    </label>
                  </div>
                  
                  <div class="radio" id="step3_ad_single" style="display:none;">
                    <label>
                      <input type="radio" name="ad_rdo" value="single">
                      使用门店内其他路由器设置
                    </label>
                  </div>

                  <div id="ap_list_div" style="display:none;">
                    <label for="step2_area" class="col-sm-2 control-label">设备列表</label>
                    <div class="col-sm-4">
                        <select class="form-control input-sm" id="ap_list_slt"><option value="-2">加载中</option></select>
                    </div>
                    <div class="clear"></div>
                  </div>

                  <div id="container_wifi" class="container_wifi">
                      <div class="outline">
                        <span>WiFi设备状态</span><div class="button" id="step3_ap_power_btn"></div>
                        <div class="clear"></div>
                      </div>
                      <div class="load"><span>读取SSID...</span><img src="/UITemplet/img/wifi/loading.gif" /></div>
                      <div class="list" id="step3_ssid_list"></div>
                  </div>

                  <div>
                      <button type="button" class="btn btn-default" onclick="javascript:gotoStep('#setup_step2')">上一步</button>
                      <button type="button" class="btn btn-default" id="step_next" onclick="javascript:gotoStep('#setup_step4',this)">提交</button>
                  </div>
                </div>
                </div>
            </div>
          </div>
        </div>
        <!-- 第四步开始 -->
        <div class="row">
          <div class="col-md-12">
            <div class="panel panel-default setup" id="setup_step4" style="display:none;">
                <div class="panel-heading">
                  <div class="step-title">第四步：设定推广信息 &mdash; 上网服务</div>
                </div>
                <div class="panel-body">
                <div class="setup-step">
                 <div class="form-horizontal">
                  <div class="form-group">
                    <label for="step4_ssid_peoplenum" class="col-sm-2 control-label">并发上网人数</label>
                    <div class="col-sm-4">
                      <div class="input-group">
                        <input type="number" class="form-control input-sm" id="step4_ssid_peoplenum">
                        <div class="input-group-addon">人</div>
                      </div>
                    </div>
                    <div class="col-sm-6">
                      <span class="notice"></span>
                    </div>
                  </div>
                  <div class="form-group">
                    <label for="step4_ssid_up" class="col-sm-2 control-label">默认上行带宽</label>
                    <div class="col-sm-4">
                      <div class="input-group">
                        <input type="number" class="form-control input-sm" id="step4_ssid_up">
                        <div class="input-group-addon">kbps</div>
                      </div>
                    </div>
                    <div class="col-sm-6">
                      <span class="notice"></span>
                    </div>
                  </div>
                  <div class="form-group">
                    <label for="step4_ssid_down" class="col-sm-2 control-label">默认下行带宽</label>
                    <div class="col-sm-4">
                      <div class="input-group">
                        <input type="number" class="form-control input-sm" id="step4_ssid_down">
                        <div class="input-group-addon">kbps</div>
                      </div>
                    </div>
                    <div class="col-sm-6">
                      <span class="notice"></span>
                    </div>
                  </div>
                  <div class="form-group">
                    <div class="col-sm-2">
                      <div class="checkbox disabled" style="text-align: right;">
                        <label style="font-weight:bold;">
                          <input type="checkbox" id="step4_time_chk" disabled>
                          定时开关
                        </label>
                      </div>
                    </div>
                    <div class="col-sm-4">
                      <div class="input-group">
                        <input type="text" class="form-control input-sm" id="step4_ssid_stime" disabled>
                        <div class="input-group-addon">到</div>
                        <input type="text" class="form-control input-sm" id="step4_ssid_etime" disabled>
                      </div>
                    </div>
                    <div class="col-sm-6">
                      <span class="notice"></span>
                    </div>
                  </div>

                  <div class="form-group">
                    <div class="col-sm-2">
                      <div class="checkbox disabled" style="text-align: right;">
                        <label style="font-weight:bold;">
                          <input type="checkbox" id="step4_ol_chk" disabled>
                          拨号上网
                        </label>
                      </div>
                    </div>
                    <div class="col-sm-4">
                      <div class="input-group">
                        <div class="input-group-addon" style="padding:0px 2px 0px 10px">帐号</div>
                        <input type="text" class="form-control input-sm" id="step4_ssid_ol_account" disabled>
                        <div class="input-group-addon" style="padding:0px 2px 0px 10px">密码</div>
                        <input type="password" class="form-control input-sm" id="step4_ssid_ol_pwd" disabled>
                      </div>
                    </div>
                    <div class="col-sm-6">
                      <span class="notice"></span>
                    </div>
                  </div>

                  <div class="form-group">
                    <div class="col-sm-offset-2 col-sm-10">
                      <button type="button" class="btn btn-default" onclick="javascript:gotoStep('#setup_step3')">上一步</button>
                      <button type="button" class="btn btn-default" onclick="javascript:gotoStep('#setup_step5',this)">提交</button>
                    </div>
                  </div>
                </div>
                </div>
                </div>
            </div>
          </div>
        </div>
        <!-- 第四步结束 -->
      </div>
    </div>
  </div>

  <div id="dialog_add_chain" title="新增连锁店">
    <fieldset>
      <div>
        <%--<label for="name">商户全称：</label>
        <input type="text" class="form-control" id="dialog_chain_fullname" value="">--%>
        <div class="form-group has-feedback">
          <label for="dialog_chain_fullname" class="control-label">商户全称</label>
          <div>
            <input type="text" class="form-control input-sm" id="dialog_chain_fullname" placeholder="商户全称 / Name" >
            <span class="glyphicon form-control-feedback"></span>
          </div>
          <span class="notice" id="dialog_chain_notice"></span>
        </div>
      </div>
    </fieldset>
  </div>
  <div id="dialog_add_single" title="新增单店">
    <fieldset>
      <%--<div><label for="name">商户全称：</label><input type="text" class="form-control" id="dialog_single_fullname" value=""></div>--%>
      <div>
        <div class="form-group has-feedback">
          <label for="dialog_single_fullname" class="control-label">商户全称</label>
          <div>
            <input type="text" class="form-control input-sm" id="dialog_single_fullname" placeholder="商户全称 / Name" >
            <span class="glyphicon form-control-feedback"></span>
            <span class="notice" id="dialog_single_notice"></span>
          </div>
        </div>
      </div>
    </fieldset>
  </div>
  <div id="dialog_ssid_pwd" title="设置密码">
    <fieldset>
      <div><label for="name">WiFi密码：</label><input type="text" class="form-control" id="step3_ssid_pwd" value=""></div>
    </fieldset>
  </div>
</body>
</html>