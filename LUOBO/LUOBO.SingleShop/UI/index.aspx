<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="LUOBO.SingleShop.UI.index" ValidateRequest="false"%>
<%@ Register src="header.ascx" tagname="header" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
    <head id="Head1" runat="server">
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
    <script>
        function SetUrlRefresh(url) {
            if (url.indexOf("?") > 0)
                return url + "&t=" + (new Date().getTime());
            else
                return url + "?t=" + (new Date().getTime());
        }
        
    </script>
    <script type="text/javascript" language="javascript">
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
                            var _rel_c = jQuery(this).attr("class").substr(3, 7);
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
              //wifi密码框控制
              var PWD = $('#PWD');
              var ISPWD = document.getElementById("ISPWD");
              PWD.attr("disabled", true);

              $('#ISPWD').click(function () {
                  if (ISPWD.checked == true) { //可编辑 
                      ISPWD.value = true;
                      PWD.attr("disabled", false);
                      PWD.focus();

                  } else {
                      PWD.attr("disabled", true);
                      ISPWD.value = false;
                  }
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
          });
  </script>
    


    <script type="text/javascript">
        var DeviceList;
        var token = GetQueryString("token"); //读取token
        var tmpsubpage = "";
        var thisSSID_ID = 0;
        var preview_ssid = "";
        var preview_ad = "";

        $(function () {
            $("#token").val(token);
            $("#UserToken").val(token);
            GetDeviceList(token);
            GetTempletList();
            GetHangyeList();
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
            $("#ad_list").empty();
            $("#ad_list").append("<option value='0'>添加新广告</option>");
            $.ajax({
                type: 'post',
                url: 'AjaxComm.aspx',
                data: 'type=GetADList&token=' + token,
                dataType: 'json',
                error: function (msg) {
                    //alert("服务器错误");
                },
                success: function (obj) {
                    if (obj.ResultCode == 0) {
                        for (var i = 0; i < obj.ResultOBJ.length; i++) {
                            $("#ad_list").append("<option value='" + obj.ResultOBJ[i].AD_ID + "'>" + obj.ResultOBJ[i].AD_Title + "</option>");
                        }
                        $("#ad_list").val($("#ad_id").val());
                    } else {
                        alert(obj.ResultMsg);
                        if (obj.ResultCode == -100) {
                            window.location.href = "login.aspx";
                        }
                    }
                }
            });
        }

        function GetDeviceList(token) {
            currentToken = token;
            var result = "";
            $.ajax({
                type: 'post',
                url: 'AjaxComm.aspx',
                data: 'type=APManage/GetDeviceList&token=' + currentToken,
                dataType: 'json',
                error: function (msg) {
                    //alert("服务器错误");
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
                                item = obj.ResultOBJ[i];
                                result += "<a class='list-group-item'><span class='label label-primary'> " + item.APNAME + "</span><span class='label label-primary pull-right' onclick='javascript:addssid(\"" + item.APID + "\");'>添加广告</span></a>";
                                if (item.SSIDList.length > 0) {
                                    for (var j = 0; j < item.SSIDList.length; j++) {
                                        item_ssid = item.SSIDList[j];
                                        result += "<a herf='#ad-view' onclick='javascript:EditSSID(" + i + "," + j + ")'  class='go-ad-view list-group-item ssid-item'>" + "<span class='badge pull-right' onclick='javascript:removessid(\"" + item_ssid.ID + "\");'>删除</span>" + item_ssid.NAME + getStaStr(item_ssid.STATE) + "</a>";
                                    }
                                }
                            }
                            $("#tbDeviceList").empty();
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

        function getStaStr(type) {
            var str = "";
            switch (type) {
                case 0:
                    str = "<span class='label label-info pull-right'>待审核</span>";
                    break;
                case 1:
                    str = "<span class='label label-info pull-right'>审核中</span>";
                    break;
                case 2:
                    str = "<span class='label label-success pull-right'>通过审核</span>";
                    break;
                case 3:
                    str = "<span class='label label-danger pull-right'>拒绝申请</span>";
                    break;
                case 4:
                    str = "<span class='label label-warning pull-right'>撤销申请</span>";
                    break;
            }
            return str;
        }

        function loadpreviewpage(k) {
            if (k == 0) {
                $("#PreviwPage").attr("src", preview_ssid);
            } else {
                $("#PreviwPage").attr("src", preview_ad);
            }
        }

        function EditSSID(ap, ssid) {
            var item_ssid = DeviceList[ap].SSIDList[ssid];
            thisSSID_ID = item_ssid.ID;
            $("#ID").val(item_ssid.ID);
            preview_ssid = item_ssid.PATH + item_ssid.PORTAL;
            loadpreviewpage(0);

            if (item_ssid.NAME != "") {
                $("#NAME").val(item_ssid.NAME);
            }
            else { $("#NAME").val("未命名"); }
            $("#MAXFLOW").val(item_ssid.MAXFLOW);
            $("#VONLINETIME").val(item_ssid.VONLINETIME);

            //wifi密码框控制
            var PWD = $('#PWD');
            var ISPWD = document.getElementById("ISPWD");
            if (item_ssid.ISPWD) {
                ISPWD.value = true;
                ISPWD.checked = true;
                PWD.attr("disabled", false);
                PWD.val(item_ssid.PWD);
            }
            else {
                ISPWD.value = false;
                ISPWD.checked = false;
                PWD.attr("disabled", true);
                PWD.val(item_ssid.PWD);
            }
            if (item_ssid.STATE == 0 || item_ssid.STATE == 1) {
                $("#SaveSSID_btn").prop("disabled", true);
            } else {
                $("#SaveSSID_btn").prop("disabled", false);
            }
            GetAdInfo(item_ssid.ADID);
        }

        function SaveSSID() {
            var SaveSSID_btn = $("#SaveSSID_btn");

            var MAXFLOW = $("#MAXFLOW").val();
            var VONLINETIME = $("#VONLINETIME").val();
            var NAME = $("#NAME").val();
            var ID = $("#ID").val();
            var ISPWD = $("#ISPWD").val();
            var PWD = $("#PWD").val();

            function isNumber(s) {//判断是否是数字
                var regu = "^[0-9]+$";
                var re = new RegExp(regu);
                if (s.search(re) != -1) {
                    return true;
                } else {
                    return false;
                }
            }

            function isNumberOrLetter(s) {//判断是否是数字或字母 
                var regu = "^[0-9a-zA-Z]+$";
                var re = new RegExp(regu);
                if (re.test(s)) {
                    return true;
                } else {
                    return false;
                }
            }

            function chk_ssid_input() {

                if (NAME == "") {
                    alert("请输入广告名称");
                    return false;
                }
                if (MAXFLOW.length > 3) {
                    alert("流量限制必须小于999mb,不做限制请留空或填0");
                    return false;
                }
                if (!isNumber(MAXFLOW)) {
                    alert("流量限制只能包含数字");
                    return false;
                }
                if (VONLINETIME.length > 4) {
                    alert("访客上网时长限制必须小于9999秒,不做限制请留空或填0");
                    return false;
                }
                if (!isNumber(VONLINETIME)) {
                    alert("访客上网时长只能包含数字");
                    return false;
                }

                if (ISPWD == "true") {
                    if (PWD == "") {
                        alert("请输入上网密码，或关闭密码访问选项");
                        return false;
                    }
                    if (PWD.length < 8 || PWD.length > 16) {
                        alert("密码长度必须在8位至16位之间");
                        return false;
                    }
                    if (!isNumberOrLetter(PWD)) {
                        alert("密码只能包含数字和字母");
                        return false;
                    }
                }
                return true;
            }

            if (chk_ssid_input()) {

                SaveSSID_btn.attr("disabled", true);
                SaveSSID_btn.val("提交中，请耐心等待");

                currentToken = $("#token").val()
                $.ajax({
                    type: 'post', //可选get
                    url: 'AjaxComm.aspx', //这里是接收数据的PHP程序
                    data: 'type=SSIDManage/SaveSSID&param={"MAXFLOW":"' + MAXFLOW + '","VONLINETIME":"' + VONLINETIME + '","ID":"' + ID + '","NAME":"' + NAME + '","ISPWD":"' + ISPWD + '","PWD":"' + PWD + '"}&token=' + currentToken,
                    dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                    error: function (msg) {
                        //alert("服务器错误");
                    },
                    success: function (obj) {
                        SaveSSID_btn.attr("disabled", false);
                        SaveSSID_btn.val("保存并提交审核");
                        if (obj.ResultCode == 1) {
                            alert(obj.ResultMsg);
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

        }

        function GetAdInfo(adid) {
            $("#ad_list").val(adid);
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
                    //document.getElementById("PreviwPage").src = item.File_Url;
                    //$("#PreviwPage").attr("src", SetUrlRefresh(item.File_Url));
                    preview_ad = SetUrlRefresh(item.File_Url);
                    loadpreviewpage(1);
                    AD_TEMPPAGE = item;
                    $("#templetcontent").empty();
                    var templethtml = "";
                    var tmpvalue = "";
                    for (var j = 0; j < item.File_Templet.length; j++) {
                        //                        if (item.File_Templet[j].Unit_Type == "txt") {
                        //                            tmpvalue = "";
                        //                            for (var k = 0; k < item.File_Content.length; k++) {
                        //                                if (item.File_Content[k].TKey == ("Templet_" + j)) {
                        //                                    tmpvalue = item.File_Content[k].TValue;
                        //                                    break;
                        //                                }
                        //                            }
                        //                            templethtml += "<tr><td><div >" + item.File_Templet[j].Unit_Name + " ：</td><td colspan=2><input class='form-control' id='Templet_" + j + "' name='Templet_" + j + "' type='text' value='" + tmpvalue + "' /></div></td></tr>";
                        //                        }
                        //                        if (item.File_Templet[j].Unit_Type == "pic") {
                        //                            tmpvalue = "";
                        //                            for (var k = 0; k < item.File_Content.length; k++) {
                        //                                if (item.File_Content[k].TKey == ("Templet_" + j)) {
                        //                                    tmpvalue = item.File_Content[k].TValue;
                        //                                    break;
                        //                                }
                        //                            }
                        //                            templethtml += "<tr><td><div >" + item.File_Templet[j].Unit_Name + " ：</td><td> " + tmpvalue + " </div><div id='Templet_" + j + "_div'> <input class='pull-left' id='Templet_" + j + "' name='Templet_" + j + "' type='file' /> <a class='pull-right' href='javascript:ChangePicSelect(\"Templet_" + j + "_div\",1,\"Templet_" + j + "\")'>选择已上传图片</a></div></td></tr>";
                        //                        }
                        tmpvalue = "";
                        for (var k = 0; k < item.File_Content.length; k++) {
                            if (item.File_Content[k].TKey == ("Templet_" + j)) {
                                tmpvalue = item.File_Content[k].TValue;
                                break;
                            }
                        }
                        if (item.File_Templet[j].Unit_Type == "txt" || item.File_Templet[j].Unit_Type == "parameter") {
                            templethtml += "<tr><td><div >" + item.File_Templet[j].Unit_Name + " ：</td><td colspan=2><input class='form-control' id='Templet_" + j + "' name='Templet_" + j + "' type='text' value='" + tmpvalue + "' /></div></td></tr>";
                        } else if (item.File_Templet[j].Unit_Type == "pic") {
                            templethtml += "<tr><td><div >" + item.File_Templet[j].Unit_Name + " ：</td><td> " + tmpvalue + " </div><div id='Templet_" + j + "_div'> <input class='pull-left' id='Templet_" + j + "' name='Templet_" + j + "' type='file' /> <a class='pull-right' href='javascript:ChangePicSelect(\"Templet_" + j + "_div\",1,\"Templet_" + j + "\")'>选择已上传图片</a></div></td></tr>";
                        }
                        if (item.File_Templet[j].Unit_Link == "true") {
                            tmpvalue = "";
                            for (var k = 0; k < item.File_Content.length; k++) {
                                if (item.File_Content[k].TKey == ("Templet_" + j + "_link")) {
                                    tmpvalue = item.File_Content[k].TValue;
                                    break;
                                }
                            }
                            templethtml += "<div><td>连接地址：</td><td colspan=2><input class='form-control' id='Templet_" + j + "_link' name='Templet_" + j + "_link' type='text' value='" + tmpvalue + "' /></div>";
                        }
                    }
                    $("#templetcontent").append(templethtml);
                }
            }
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

        function SetActiveAdedit(t) {
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
                $("#templetfiles").append("<label class='btn btn-primary " + (i == 0 ? ' active' : '') + "' onclick='javascript:getcontent(\"" + item.File_Name + "\");'><input type='radio' name='options' id='option1' >" + fn + "</label>");
            }
        }

        function ChangePicSelect(mydiv, p, n) {
            $("#" + mydiv).empty();
            if (p == 0) {
                $("#" + mydiv).append("<input class='pull-left' id='" + n + "' name='" + n + "' type='file' /> <a class='pull-right' href='javascript:ChangePicSelect(\"" + mydiv + "\",1,\"" + n + "\")'>选择已上传图片</a>");
            } else {
                $("#" + mydiv).append("<input id='" + n + "' name='" + n + "' type='text'/> <input  type='button' onclick='javascript:SelectServerPic(\"" + n + "\")' value='选择' /> <a class='pull-right' href='javascript:ChangePicSelect(\"" + mydiv + "\",0,\"" + n + "\")'>选择本地文件上传</a>");
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
            var tmpadid = $("#ad_list option:selected").val();
            GetAdInfo(tmpadid);
        }

    </script>
    <script>
        function removessid(ssid_id) {
            if (!confirm("确认要删除？")) {
                return false;
            }
            //alert(ssid_id);
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
                        GetDeviceList(token);
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
            //alert(ap_id);
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
                        GetDeviceList(token);
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
            var copyadname = 0;
            var adid = $("#ad_id").val();
            if (adid > 0) {
                //                if ($("#iscopyssidname").is(':checked')) {
                //                    copyadname = 1;
                //                }
                //GetAdInfo(adid);
                SetActiveAdedit(false);
                postaudit(adid, 1, thisSSID_ID, 0, copyadname);
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
    </head>
    <body>
    <uc1:header ID="header1" runat="server" />

    <div class="container-fluid">
      <div  class="row-fluid">
        <div class="div-body  col-md-4" id="ad-list" name="ad-list">
    <div class="panel panle_ssid panel-default">
      <div class="panel-heading" style="font-size:18px"><!-- Split button -->
          <span class="label" style="color:#d9534f;font-size:38px"><em>1</em></span>发射器管理<p class="disabled hidden-xs" style="float:right; font-size:28px; color:#d9534f"><font face=Symbol>&#190;</font></p>
            </div>
            <div class="btn-group btn-group-xs pull-right">
  <button type="button" class="btn btn-success" onclick="javascript:loadpreviewpage(0)">按设备查看</button>
  <button type="button" class="btn btn-warning" onclick="javascript:loadpreviewpage(1)">按广告查看</button>
</div>
<div class="list-group" id="tbDeviceList" name="tbDeviceList" style="float:right; width:100%; height:641px; overflow-y:auto">
</div>
              <div id="divPage" class="hor-minimalist-page"></div>
              </div>
        </div>
        <div class="div-body col-md-4" id="ad-view" name="ad-view">
    <div class="panel panle_right panel-default">
      <div class="panel-heading" style="font-size:18px"><!-- Split button -->
      <p class="disabled hidden-xs" style="float:left; font-size:28px; color:#d9534f"><font face=Symbol>&#174;</font></p>
                <span class="label" style="color:#d9534f;font-size:38px"><em>2</em></span>广告预览<p class="disabled hidden-xs" style="float:right; font-size:28px; color:#d9534f"><font face=Symbol>&#190;</font></p><a style="float:right" class='goNext visible-xs' href='#ad-edit'><i class='fa fa-fw fa-pencil'></i>参数</a></div>
            <div class="btn-group btn-group-xs pull-right">
  <button type="button" class="btn btn-success" onclick="javascript:loadpreviewpage(0)">使用中广告</button>
  <button type="button" class="btn btn-warning" onclick="javascript:loadpreviewpage(1)">编辑中广告</button>
</div>
            <div class="widget-content padded clearfix">
              <iframe id="PreviwPage" name="PreviwPage" style="float:right; width:100%; height:600px;"></iframe>
            </div>
          </div>
        </div>
        <div class="div-body col-md-4" id="ad-edit" name="ad-edit">
    <div class="panel panle_right panel-default">
      <div class="panel-heading" style="font-size:18px"><!-- Split button -->
      <p class="disabled hidden-xs" style="float:left; font-size:28px; color:#d9534f"><font face=Symbol>&#174;</font></p>
            <span class="label" style="color:#d9534f;font-size:38px"><em>3</em></span>广告编辑<a style="float:right" class='goNext visible-xs' href='#ad-view'><i class='fa fa-fw fa-eye'></i>预览</a></div>
            <div id="accordion"><h4><span class="label">参数设置</span></h4>
            <div class="widget-content padded clearfix">
              <div>
                <form id="formss" action="AjaxComm.aspx"  target="AjaxComm.aspx" method="post" enctype="multipart/form-data" onsubmit="return false;">
                  <input type="hidden" id="type" name="type" value="SSIDManage/SaveSSID" />
                  <input type="hidden" id="ID" name="ID" value="" />
                  <input type="hidden" id="token" name="token" value="" />
                  <table class="table table-striped .table-condensed">
                    <tr>
                      <td width="150px">SSID显示名称：</td>
                        <td>
                        <input id="NAME" name="NAME" class="form-control" type="text" placeholder="请输入广告语"/></td>
                    </tr>
                    <tr>
                    <td>最大流量(mb)：</td>
                        <td>
                        <input id="MAXFLOW" name="MAXFLOW" class="form-control" type="text" placeholder="不做限制请留空"/></td>
                    </tr>
                    <tr>
                    <td>访客上网时长(秒)：</td>
                        <td>
                        <input id="VONLINETIME" name="VONLINETIME" class="form-control" type="text" placeholder="不做限制请留空"/></td>
                    </tr>
                      <tr>
                        <td><input type="checkbox" value="" id="ISPWD" name="ISPWD" /> 上网密码：</td>
                        <td><input id="PWD" name="PWD" type="text" class="form-control" placeholder="如启用,请输入8-16位数字或字母"/></td>
                    </tr>
                    <tr>
                      <td  colspan="2" class="text-center">
                        <input type="button" id="SaveSSID_btn" name="SaveSSID_btn"  class="go-ad-view btn btn-primary" onclick="javascript:SaveSSID()" value="保存并提交审核" /></td>
                    </tr>
                    
                  </table>
                </form>
              </div>
            </div>
            <h4><span class="label">内容管理</span></h4>
            <div class="widget-content padded clearfix">
              <div>
              <select class="form-control" id="ad_list" name="ad_list" onchange="ChangeAd()"></select>
               <form id="form_adedit" action="ADSave.aspx" target="upload" method="post" enctype="multipart/form-data" onsubmit="return SaveAD();">
                    <input type="hidden" id="ad_id" name="ad_id" value="0" />
                    <input type="hidden" id="homepage" name="homepage" value="" />
                    <input type="hidden" name="pubcount" value="0" />
                    <input type="hidden" id="ad_pubpath" name="ad_pubpath" value="" />
                    <input type="hidden" id="temppage" name="temppage" value="" />
                    <input type="hidden" id="UserToken" name="UserToken" value="" />
                    <input type="hidden" id="ad_ssid" name="ad_ssid" value="" />
                  <table class="hor-minimalist-top table table-striped">
                    <tr>
                        <td width="100px">广告名称：</td>
                        <td colspan="3"><input id="ad_title" name="ad_title" type="text"class="form-control" /></td>
                    </tr>
                    <%--<tr>
                      <td>SSID：</td>
                        <td colspan="3">
                        <input id="ad_ssid" name="ad_ssid" type="text" class="form-control"/>
                        </td>
                    </tr>--%>
                    <tr>
                        <td>应用模版：</td>
                        <td>
                            <select id="ad_model" name="ad_model" class="form-control" onchange="GetTempletFiles()"></select><div id="modeldiv"/>
                        </td>
                        <td width="60px">行业：</td>
                        <td>
                            <select id="ad_type" name="ad_type" class="form-control"></select>
                        </td>
                    </tr>
                    <tr style="background-color:#eee">
                        <td valign="top" colspan="4">
                            <div id="templetfiles" class="btn-group" data-toggle="buttons"></div>
                            <table id="templetcontent" class="hor-minimalist-top table table-striped" width="95%">
                            </table>
                        </td>
                    </tr>
                    <tr>
                      <td colspan="4" class="text-center">
                        <input type="button" id="btn_ad_save" class="go-ad-view btn btn-primary" onclick="javascript:submitsavaad()" value="保存并预览" />&nbsp;
                        <input type="button" id="btn_ad_savepost"  class="go-ad-view btn btn-primary" onclick="javascript:saveadpost();" value="提交审核"/>
                        <%--<label class="checkbox-inline">
  <input type="checkbox" id="iscopyssidname" name="iscopyssidname"/> 更新SSID名称
</label>--%>
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
    </div>

    <iframe id="upload" name="upload" style="width:0px; height:0px; border:0px;" ></iframe>
    <div id="dialog-form" title="请选择图片：">
      <fieldset>
      <dl id="dl_serverpics">
      
      </dl>
      </fieldset>
    </div>
    <script>
        document.write("<a href='manage.aspx?token=" + token + "'>.</a>");
    </script>
</body>
</html>
