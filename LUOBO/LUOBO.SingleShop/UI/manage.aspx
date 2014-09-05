<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="manage.aspx.cs" Inherits="LUOBO.SingleShop.UI.manage" %>
<%@ Register src="header.ascx" tagname="header" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../UITemplet/css/bootstrap.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/css/font-awesome.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/css/index.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/css/style.css" media="all" rel="stylesheet" type="text/css" />
    <script src="../UITemplet/js/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../UITemplet/js/Common.js"></script>
    <script type="text/javascript" src="../UITemplet/js/PageViewJS.js"></script>
    <script language="javascript" type="text/javascript">
        var token = GetQueryString("token"); //读取token
        var myAdPubList;
        var AD_TEMPLET;
        var org_id;
        var tmpsubpage = "";

        function SetUrlRefresh(url) {
            if (url.indexOf("?") > 0)
                return url + "&t=" + (new Date().getTime());
            else
                return url + "?t=" + (new Date().getTime());
        }

        $(document).ready(function () {
            HighLightMenu("控制");
            $("#UserToken").val(token);
            getAdList(1);

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

        function InitPage() {
            $("#ad_id").val(-1);
            $("#PreviwPage").attr("src", "");
            $("#ad_publist_div").empty();
            InitRightView(0);
            GetTempletList();
            GetHangyeList();
        }

        function InitRightView(d) {
            if (d == 1) {
                $("#ad_message").hide();
                $("#ad-pub").hide();
                $("#ad-pubview").hide();
                $("#ad-edit").show();
            } else if (d == 2) {
                $("#ad_message").hide();
                $("#ad-edit").hide();
                $("#ad-pubview").hide();
                $("#ad-pub").show();
            } else if (d == 3) {
                $("#ad-edit").hide();
                $("#ad-pubview").hide();
                $("#ad-pub").hide();
                $("#ad_message").show();
            } else {
                $("#ad_message").hide();
                $("#ad-pub").hide();
                $("#ad-edit").hide();
                $("#ad-pubview").show();
            }
        }

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

    </script>

    <script language="javascript" type="text/javascript">
        function getAdList(curPage) {
            InitPage();
            var pagesize = 10;
            $.ajax({
                type: 'post',
                url: 'AjaxComm.aspx',
                data: 'type=GetADPubList&token=' + token + '&param={"keystr":"","adaudit":"-1","pagenum":"' + curPage + '","pagesize":"' + pagesize + '"}',
                dataType: 'json',
                error: function (msg) {
                    //alert("服务器错误");
                },
                success: function (obj) {
                    if (obj.ResultCode == 0) {
                        $("#div_ADList").empty();
                        var item;
                        myAdPubList = obj.ResultOBJ.ADList;
                        for (var i = 0; i < obj.ResultOBJ.ADList.length; i++) {
                            item = obj.ResultOBJ.ADList[i];
                            $("#div_ADList").append("<a herf='#ad-view' onclick='javascript:ShowAD(" + item.ADInfo.ORG_ID + "," + item.ADInfo.AD_ID + ",\"" + item.ADInfo.AD_HomePage + "\");'  class='go-ad-view list-group-item ssid-item'><span class='badge pull-right'>" + item.PubCount + "</span>" + item.ADInfo.AD_Title + getStaStr(item.ADInfo.AD_Stat) + "</a>");
                        }
                        var allPage = obj.ResultOBJ.AllCount % pagesize == 0 ? obj.ResultOBJ.AllCount / pagesize : parseInt(obj.ResultOBJ.AllCount / pagesize) + 1;
                        ShowPage({ CurrentPage: curPage, MaxPageSize: allPage, PageShowSize: 5, IsUpDown: true, ShowElement: document.getElementById("divADPage"), PageEvents: function (page) { getAdList(page); } });
                        $(".ssid-item").click(function () {
                            $(".ssid-item").removeClass("active");
                            $(this).addClass("active");
                        });
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

        function ShowAD(ORG_ID, AD_ID, AD_HomePage) {
            org_id = ORG_ID;
            $("#ad_id").val(AD_ID);
            InitRightView(0);
            $("#PreviwPage").attr("src", SetUrlRefresh("/ADUserFile/" + ORG_ID + "/" + AD_ID + "/" + AD_HomePage));
            GetPubDeviceList(AD_ID);
        }

        function GetPubDeviceList(adid) {
            $("#ad_publist_div").empty();
            var result = "";
            $.ajax({
                type: 'post',
                url: 'AjaxComm.aspx',
                data: 'type=APManage/GetDeviceListByAdid&token=' + token + "&ID=" + adid,
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
                                result += "<a class='list-group-item'><span class='label label-primary'> " + item.APNAME + "</span></a>";
                                if (item.SSIDList.length > 0) {
                                    for (var j = 0; j < item.SSIDList.length; j++) {
                                        item_ssid = item.SSIDList[j];
                                        result += "<a herf='#' class='go-ad-view list-group-item ssid-item'>" + item_ssid.NAME + "</a>";
                                    }
                                }
                            }
                        } else {
                            result = "<a herf='#' class='go-ad-view list-group-item ssid-item'>暂无发布设备</a>";
                        }
                        $("#ad_publist_div").append(result);
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

    <script language="javascript" type="text/javascript">
        function myadedit() {
            var adid = $("#ad_id").val();
            if (adid >= 0) {
                InitRightView(1);
                GetAdInfo(adid);
            } else {
                alert("请先选择要修改的广告！");
            }
        }

        function SetActiveAdedit(t) {
            if (t) {
                $("#btn_ad_save").prop("disabled", false);
            } else {
                $("#btn_ad_save").prop("disabled", true);
            }
        }

        function GetAdInfo(adid) {
            if (adid > 0) {
                SetActiveAdedit(false);
                $("#ad_title").val("");
                $("#ad_ssid").val("");
                $("#homepage").val("");
                $("#pubcount").val(0);
                $("#ad_pubpath").val("");
                $("#modeldiv").empty();
                $("#templetfiles").empty();
                $("#templetcontent").empty();
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
                            $("#UserToken").val(token);
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

        function addNewAD() {
            getNewAD();
            InitRightView(1);
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
            //getAdList(1);
            $("#PreviwPage").attr("src", SetUrlRefresh("/ADUserFile/" + org_id + "/" + adid + "/" + tmpsubpage));
            $("#ad_id").val(adid);
            //GetAdInfo(adid);
            $("#ad_message_div").empty();
            InitRightView(3);
            $("#ad_message_div").append("保存成功。<input type='button' class='go-ad-view btn btn-primary' onclick='javascript:myadedit()' value='重新修改' /><input type='button' class='go-ad-view btn btn-primary' onclick='javascript:myadpub()' value='发布' />");
        }

        function saveerr(msg) {
            alert(msg);
            var adid = $("#ad_id").val();
            //getAdList(1);
            GetAdInfo(adid);
        }
        
    </script>

    <script language="javascript" type="text/javascript">

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
    </script>

    <script language="javascript" type="text/javascript">
        function myadpub() {
            if ($("#ad_id").val() >= 0) {
                var p = 0;
                var id = $("#ad_id").val();
                for (var i = 0; i < myAdPubList.length; i++) {
                    if (myAdPubList[i].ADInfo.AD_ID == id) {
                        p = myAdPubList[i].PubCount;
                    }
                }
                $("#span_pubcount").empty();
                $("#span_pubcount").append(p);
                InitRightView(2);
            } else {
                alert("请先选择要发布的广告！");
            }
        }

        function setPubType(t) {
//            alert($("#span_pubcount").text());
//            alert($("#ad_id").val() + "||" + $("#UserToken").val());
            switch (t) {
                case 0:
                    if ($("#ad_id").val() == "" || $("#ad_id").val() < 1) {
                        alert("no ad");
                        return;
                    }
                    postaudit($("#ad_id").val(), 0, "", 0, 0);
                    break;
                case 1:
                    alert("b");
                    break;
                case 2:
                    alert("b");
                    break;
                case 3: ;
                    var p = $("#span_pubcount").text();
                    if (p > 0) {
                        if ($("#ad_id").val() == "" || $("#ad_id").val() < 1) {
                            alert("no ad");
                            return;
                        }
                        postaudit($("#ad_id").val(), 3, "", 0, 0);
                    }else{
                        alert("该广告暂无发布设备，请用其它方式发布！");
                        return;
                    }
                    break;
                default:
                    alert("no public type");
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
//                        alert("广告提交审核，请等待审核结果！");
//                        GetAdInfo(adid);
                        $("#ad_message_div").empty();
                        InitRightView(3);
                        $("#ad_message_div").append("广告提交审核，请等待审核结果！<input type='button' class='go-ad-view btn btn-primary' onclick='javascript:GetPubDeviceList(" + adid + ");' value='返回' />");
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
</head>
<body>
    <uc1:header ID="header1" runat="server" />

    <div class="container-fluid">
        <div  class="row-fluid">
            <div class="div-body  col-md-4" id="ad-list" name="ad-list">
                <div class="panel panle_right">
                    <div class="panel-heading">我的广告
                    </div>
                    <div>
                        <table class="hor-minimalist-top table table-striped" width="95%">
                            <tr>
                                <td>广告名称 <a href="javascript:addNewAD();">【新广告】</a></td>
                                <td>状态</td>
                                <td>发布点数</td>
                            </tr>
                        </table>
                    </div>
                    <div class="list-group" id="div_ADList" name="tbDeviceList">
                        <%--<a herf='javascript:ShowAD();'  class='go-ad-view list-group-item ssid-item'><span class='badge pull-right'>2</span>广告名 <span class='label label-info pull-right'>待审核</span></a>
                        <a herf='javascript:ShowAD();'  class='go-ad-view list-group-item ssid-item'><span class='badge pull-right'>2</span>广告名 <span class='label label-info pull-right'>待审核</span></a>
                        <a herf='javascript:ShowAD();'  class='go-ad-view list-group-item ssid-item'><span class='badge pull-right'>2</span>广告名 <span class='label label-info pull-right'>待审核</span></a>--%>
                    </div>
                    <div id="divADPage" class="hor-minimalist-page"></div>
                </div>
            </div>
            <div class="div-body col-md-4" id="ad-view" name="ad-view">
                <div class="panel panle_right">
                    <div class="panel-heading">广告预览 <a style="float:right" class='goNext visible-xs' href='#ad-edit'><i class='fa fa-fw fa-pencil'></i>参数</a>
                    </div>
                    <div class="btn-group btn-group-xs pull-right">
                        <button type="button" class="btn btn-warning" onclick="javascript:myadedit()">修改</button>
                        <button type="button" class="btn btn-warning" onclick="javascript:myadpub()">发布</button>
                    </div>
                    <div class="widget-content padded clearfix">
                        <iframe id="PreviwPage" name="PreviwPage" style="float:right; width:100%; height:600px;"></iframe>
                    </div>
                </div>
            </div>
            <div class="div-body col-md-4" id="ad-edit" name="ad-edit">
                <div class="panel panle_right">
                    <div class="panel-heading">广告编辑<a style="float:right" class='goNext visible-xs' href='#ad-view'><i class='fa fa-fw fa-eye'></i>预览</a>
                    </div>
                    <div class="widget-content padded clearfix" id="ad_edit_contentdiv">
                        <form id="form_adedit" action="ADSave.aspx" target="upload" method="post" enctype="multipart/form-data" onsubmit="return SaveAD();">
                            <input type="hidden" id="ad_id" name="ad_id" value="-1" />
                            <input type="hidden" id="homepage" name="homepage" value="" />
                            <input type="hidden" name="pubcount" value="0" />
                            <input type="hidden" id="ad_pubpath" name="ad_pubpath" value="" />
                            <input type="hidden" id="temppage" name="temppage" value="" />
                            <input type="hidden" id="UserToken" name="UserToken" value="" />
                            <%--<input type="hidden" id="ad_ssid" name="ad_ssid" value="" />--%>
                            <table class="hor-minimalist-top table table-striped">
                            <tr>
                                <td width="100px">广告名称：</td>
                                <td colspan="3"><input id="ad_title" name="ad_title" type="text"class="form-control" /></td>
                            </tr>
                            <tr>
                                <td>SSID显示：</td>
                                <td colspan="3">
                                <input id="ad_ssid" name="ad_ssid" type="text" class="form-control"/>
                                </td>
                            </tr>
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
                                <%--<input type="button" id="btn_ad_savepost"  class="go-ad-view btn btn-primary" onclick="javascript:saveadpost();" value="提交审核"/>--%>
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
            <div class="div-body col-md-4" id="ad_message" name="ad_message">
                <div class="panel panle_right">
                    <div class="panel-heading">发布情况<a style="float:right" class='goNext visible-xs' href='#ad-view'><i class='fa fa-fw fa-eye'></i>预览</a>
                    </div>
                    <div class="list-group" id="ad_message_div">
                        
                    </div>
                    <div id="ad_pub_div">
                        adsffffffffff
                    </div>
                </div>
            </div>
            <div class="div-body col-md-4" id="ad-pubview" name="ad-pubview">
                <div class="panel panle_right">
                    <div class="panel-heading">发布情况<a style="float:right" class='goNext visible-xs' href='#ad-view'><i class='fa fa-fw fa-eye'></i>预览</a>
                    </div>
                    <div class="list-group" id="ad_publist_div">
                        <%--<a class='list-group-item'><span class='label label-primary'>AP1</span></a>
                        <a class='go-ad-view list-group-item ssid-item'>SSID1</a>
                        <a class='list-group-item'><span class='label label-primary'>AP2</span></a>
                        <a class='go-ad-view list-group-item ssid-item'>SSID2</a>--%>
                    </div>
                </div>
            </div>
            <div class="div-body col-md-4" id="ad-pub" name="ad-pub">
                <div class="panel panle_right">
                    <div class="panel-heading">广告发布<a style="float:right" class='goNext visible-xs' href='#ad-view'><i class='fa fa-fw fa-eye'></i>预览</a>
                    </div>
                    <div class="list-group">
                        <a class='go-ad-view list-group-item pub-item' onclick="setPubType(3);">替换已发布该广告的所有设备（<span id="span_pubcount"></span>）</a>
                        <a class='go-ad-view list-group-item pub-item' onclick="setPubType(0);">只审核不发布</a>
                        <a class='go-ad-view list-group-item pub-item' onclick="setPubType(1);">发布到指定设备</a>
                        <a class='go-ad-view list-group-item pub-item' onclick="setPubType(2);">发布到发布方案</a>
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

</body>
</html>
