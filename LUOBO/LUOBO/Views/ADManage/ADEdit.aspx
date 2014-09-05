<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta charset="utf-8">
    <script src="../../Scripts/Common.js" type="text/javascript"></script>
    <link href="../../Content/lhbStyle/stylesheets/bootstrap.min.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../../Content/lhbStyle/stylesheets/font-awesome.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../../Content/lhbStyle/stylesheets/se7en-font.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../../Content/lhbStyle/stylesheets/style.css" media="all" rel="stylesheet" type="text/css" />
    <script src="../../Content/lhbStyle/javascripts/bootstrap.min.js" type="text/javascript"></script>
    <script src="../../Content/lhbStyle/javascripts/jquery.mousewheel.js" type="text/javascript"></script>
    <script src="../../Content/lhbStyle/javascripts/jquery.dataTables.min.js" type="text/javascript"></script>
<!--<script src="../../Content/lhbStyle/javascripts/bootstrap-editable.min.js" type="text/javascript"></script>
    <script src="../../Content/lhbStyle/javascripts/xeditable-demo.js" type="text/javascript"></script>-->
    <link rel="stylesheet" href="../../Scripts/jquery-ui.css">
    <script src="../../Scripts/jquery-1.10.2.js"></script>
    <script src="../../Scripts/jquery-ui.js"></script>
    <title>ADEdit</title>
    
    <script type="text/javascript">
        var AD_TEMPLET;
        var AD_TEMPPAGE = null;
        $(function () {
            $("#ad_model").change(function () { getfilelist(); });

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

            getmain();
        });


        function getmain() {
            if ($("#ad_id").val() > 0) {
                $("#ad_model").prop("disabled", true);
                $("#modeldiv").empty();
                getadinfo();
            } else {
                $("#ad_model").prop("disabled", false);
                $("#modeldiv").empty();
                getfilelist();
            }
        }

        function getadinfo() {
            $.ajax({
                type: 'post',
                url: 'GetADInfo',
                data: 'AD_ID=' + $("#ad_id").val(),
                dataType: 'json',
                success: function (obj) {
                    if (obj.ResultCode == "0") {
                        $("#ad_title").val(obj.ResultOBJ.AD_Title);
                        $("#ad_ssid").val(obj.ResultOBJ.AD_SSID);
                        $("#ad_type").val(obj.ResultOBJ.AD_Type);
                        $("#ad_model").val(obj.ResultOBJ.AD_Model);
                        $("#homepage").val(obj.ResultOBJ.AD_HomePage);
                        $("#pubcount").val(obj.ResultOBJ.AD_Release_Count);
                        $("#ad_pubpath").val(obj.ResultOBJ.AD_PUBPATH);
                        $("#modeldiv").append("<input type='hidden' name='ad_model' value='" + obj.ResultOBJ.AD_Model + "' />");
                        getadfiles();
                    } else {
                        alert(obj.ResultMsg);
                    }
                }
            });
        }

        function getadfiles() {
            $.ajax({
                type: 'post',
                url: 'GetADFiles',
                data: 'ad_id=' + $("#ad_id").val(),
                dataType: 'json',
                success: function (obj) {
                    if (obj.ResultCode == "0") {
                        $("#templetfiles").empty();
                        AD_TEMPLET = obj.ResultOBJ;
                        var item;
                        var f;
                        for (var i = 0; i < obj.ResultOBJ.length; i++) {
                            item = obj.ResultOBJ[i];
                            f = item.File_Name;
                            if (item.isPortal) {
                                $("#homepage").val(f);
                                if (tmpsubpage == "") {
                                    getcontent(f);
                                } else {
                                    getcontent(tmpsubpage);
                                    tmpsubpage = "";
                                }
                                f = f + "*";
                            }
                            $("#templetfiles").append("<label class='btn btn-primary " + (i == 0 ? ' active' : '') + "' onclick='javascript:getcontent(\"" + item.File_Name + "\");'><input type='radio' name='options' id='option1' >" + f + "</label>");
     
                           // $("#templetfiles").append("<a href='javascript:getcontent(\"" + item.File_Name + "\");'>" + f + "</a> -- ");
                        }
                    } else {
                        alert(obj.ResultMsg);
                    }
                }
            });
        }

        function getfilelist() {
            $.ajax({
                type: 'post',
                url: 'GetTempletFiles',
                data: 'ADT_ID=' + $("#ad_model option:selected").val(),
                dataType: 'json',
                success: function (obj) {
                    if (obj.ResultCode == "0") {
                        $("#templetfiles").empty();
                        AD_TEMPLET = obj.ResultOBJ;
                        var item;
                        var f;
                        for (var i = 0; i < obj.ResultOBJ.length; i++) {
                            item = obj.ResultOBJ[i];
                            f = item.File_Name;
                            if (item.isPortal) {
                                $("#homepage").val(f);
                                if (tmpsubpage == "") {
                                    getcontent(f);
                                } else {
                                    getcontent(tmpsubpage);
                                    tmpsubpage = "";
                                }
                                f = f + "*";
                            }
                            //$("#templetfiles").append("<a href='javascript:getcontent(\"" + item.File_Name + "\");'>" + f + "</a> -- ");
                            $("#templetfiles").append("<label class='btn btn-primary " + (i == 0 ? " active" : "") + "' onclick='javascript:getcontent(\"" + item.File_Name + "\");'><input type='radio' name='options' id='option1' >" + f + "</label>");
                      
                        }
                    } else {
                        alert(obj.ResultMsg);
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
                    $("#PreviwPage").attr("src", item.File_Url);
                    AD_TEMPPAGE = item;
                    $("#templetcontent").empty();
                    var templethtml = "";
                    var tmpvalue = "";
                    for (var j = 0; j < item.File_Templet.length; j++) {
                        if (item.File_Templet[j].Unit_Type == "txt") {
                            tmpvalue = "";
                            for (var k = 0; k < item.File_Content.length; k++) {
                                if (item.File_Content[k].TKey == ("Templet_" + j)) {
                                    tmpvalue = item.File_Content[k].TValue;
                                    break;
                                }
                            }
                            templethtml += "<tr><td><div >" + item.File_Templet[j].Unit_Name + " ：<input id='Templet_" + j + "' name='Templet_" + j + "' type='text' value='" + tmpvalue + "' /></div></td></tr>";
                  
                        }
                        if (item.File_Templet[j].Unit_Type == "pic") {
                            tmpvalue = "";
                            for (var k = 0; k < item.File_Content.length; k++) {
                                if (item.File_Content[k].TKey == ("Templet_" + j)) {
                                    tmpvalue = item.File_Content[k].TValue;
                                    break;
                                }
                            }
                            templethtml += "<tr><td><div >" + item.File_Templet[j].Unit_Name + " ： " + tmpvalue + " </div><div id='Templet_" + j + "_div'> <input id='Templet_" + j + "' name='Templet_" + j + "' type='file' /> <a href='javascript:ChangePicSelect(\"Templet_" + j + "_div\",1,\"Templet_" + j + "\")'>选择已上传图片</a></div></td></tr>";
                        }
                        if (item.File_Templet[j].Unit_Link == "true") {
                            tmpvalue = "";
                            for (var k = 0; k < item.File_Content.length; k++) {
                                if (item.File_Content[k].TKey == ("Templet_" + j + "_link")) {
                                    tmpvalue = item.File_Content[k].TValue;
                                    break;
                                }
                            }
                            templethtml += "<div>连接地址：<input id='Templet_" + j + "_link' name='Templet_" + j + "_link' type='text' value='" + tmpvalue + "' /></div>";
                        }
                    }
                    $("#templetcontent").append(templethtml);
                }
            }
        }

        function ChangePicSelect(mydiv,p,n) {
            $("#" + mydiv).empty();
            if (p == 0) {
                $("#" + mydiv).append("<input id='" + n + "' name='" + n + "' type='file' /> <a href='javascript:ChangePicSelect(\"" + mydiv + "\",1,\"" + n + "\")'>选择已上传图片</a>");
            } else {
                $("#" + mydiv).append("<input id='" + n + "' name='" + n + "' type='text'/> <input type='button' onclick='javascript:SelectServerPic(\"" + n + "\")' value='选择' /> <a href='javascript:ChangePicSelect(\"" + mydiv + "\",0,\"" + n + "\")'>选择本地文件上传</a>");
            }
        }
        var tmpsubpage = ""
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
            if ($("#ad_ssid").val() == "") {
                alert("请输入SSID显示名称");
                return false;
            }
            tmpsubpage = $("#temppage").val();
            return true;
        }

        function savesuss(adid) {
            $("#ad_id").val(adid);
            getmain();
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
                url: 'GetServerPics',
                dataType: 'json',
                success: function (obj) {
                    if (obj.ResultCode == "0") {
                        ServerPics = obj.ResultOBJ;
                        showpicsdialog();
                    } else {
                        alert(obj.ResultMsg);
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

        var pubtype = null;
        var tmpPubList = null;
        var pubList = null;
        var pubpagesize = 10;
        function PubToCase(page,c) {
            //getmain();
            tmpPubList = null;
            if (c == 0) {
                pubList = null;
            }
            pubtype = 2;
            $("#div_adpub").empty();
            $("#div_adpub").append("<table ><tr><td><table><thead><tr><th>选择</th><th>推广方案名称</th><th>点数</th><th>更新SSID</th><th>推广内容</th></tr></thead><tbody id='adpub_case_show'></tbody></table></td><td><input type='button' onclick='javascript:addpubs();' value='>>'/><br/><input type='button' onclick='javascript:removepubs();' value='<<'/></td><td><table><thead><tr><th>选择</th><th>推广方案名称</th><th>点数</th><th>更新SSID</th><th>推广内容</th></tr></thead><tbody id='adpub_case_select'></tbody></table></td></tr><tr><td><div id='adpub_pages'/></td><td></td><td>选中<span id='adpub_case_count'>0</span>个发布方案。<input type='button' onclick='javascript:subaudit();' value='提交发布'/></td></tr></table>");

            $.ajax({
                type: 'post',
                url: 'GetCaseFromPage',
                data: 'page=' + page + "&psize=" + pubpagesize,
                dataType: 'json',
                success: function (obj) {
                    if (obj.ResultCode == "0") {
                        tmpPubList = new Object(obj.ResultOBJ);
                        showpubselct();
                    } else {
                        alert(obj.ResultMsg);
                    }
                }
            });
        }
        function PubToSSID(page,c) {
            //getmain();
            tmpPubList = null;
            if (c == 0) {
                pubList = null;
            }
            pubtype = 1;
            $("#div_adpub").empty();
            $("#div_adpub").append("<table ><tr><td><table><thead><tr><th>选择</th><th>SSID</th><th>AP</th><th>机构</th><th>更新SSID</th><th>推广内容</th></tr></thead><tbody id='adpub_ssid_show'></tbody></table></td><td><input type='button' onclick='javascript:addpubs();' value='>>'/><br/><input type='button' onclick='javascript:removepubs();' value='<<'/></td><td><table><thead><tr><th>选择</th><th>SSID</th><th>AP</th><th>机构</th><th>更新SSID</th><th>推广内容</th></tr></thead><tbody id='adpub_ssid_select'></tbody></table></td></tr><tr><td><div id='adpub_pages'/></td><td></td><td>选中<span id='adpub_ssid_count'>0</span>个SSID。<input type='checkbox' id='cb_ascase' name='cb_ascase' value='1'/>存为发布方案<input type='button' onclick='javascript:subaudit();' value='提交发布'/></td></tr></table>");

//            $.ajax({
//                type: 'post',
//                url: 'GetSSIDFromPage',
//                data: 'page=' + page + "&psize=" + pubpagesize,
//                dataType: 'json',
//                success: function (obj) {
//                    tmpPubList = obj;
//                    showpubselct();
//                }
//            });
        }
        function showpubselct() {
            switch (pubtype) {
                case 2:
                    $("#adpub_case_show").empty();
                    $("#adpub_case_select").empty();
                    //$("#divPage").empty();
                    if (tmpPubList != null) {
                        var result;
                        var item;
                        var t = true;
                        for (var i = 0; i < tmpPubList.ACList.length; i++) {
                            item = tmpPubList.ACList[i];
                            t = true;
                            if (pubList != null) {
                                for (var j = 0; j < pubList.ACList.length; j++) {
                                    if (item.CASE.AC_ID == pubList.ACList[j].CASE.AC_ID) {
                                        t = false;
                                        break;
                                    }
                                }
                            }
                            var checkstr = "√";
                            if (t) {
                                var tmpcheckstr = "<input type='checkbox' id='cb_pubids' name='cb_pubids' value='" + i + "'/>";
                            }
                            result = "<tr>";
                            result += "<td>" + tmpcheckstr + "</td>";
                            result += "<td>" + item.CASE.AC_TITLE + "</td>";
                            result += "<td>" + item.SSID_Count + "</td>";
                            result += "<td>" + item.CASE.IS_COPYSSID.toString().replace("0", "否").replace("1", "是") + "</td>";
                            result += "<td>" + item.CASE.AD_TITLE + "</td>";
                            result += "</tr>";
                            $("#adpub_case_show").append(result);
                        }
                        //allPage = obj.AllCount % size == 0 ? obj.AllCount / size : parseInt(obj.AllCount / size) + 1;
                        //ShowPage({ CurrentPage: curPage, MaxPageSize: allPage, PageShowSize: 5, IsUpDown: true, ShowElement: document.getElementById("divPage"), PageEvents: function (page) { FindAPCTList(page); } });
                    }
                    if (pubList != null) {
                        var resultstr;
                        var item;
                        for (var i = 0; i < pubList.ACList.length; i++) {
                            item = pubList.ACList[i];
                            resultstr = "<tr>";
                            resultstr += "<td><input type='checkbox' id='cb_selectpubids' name='cb_selectpubids' value='" + i + "'/></td>";
                            resultstr += "<td>" + item.CASE.AC_TITLE + "</td>";
                            resultstr += "<td>" + item.SSID_Count + "</td>";
                            resultstr += "<td>" + item.CASE.IS_COPYSSID.toString().replace("0", "否").replace("1", "是") + "</td>";
                            resultstr += "<td>" + item.CASE.AD_TITLE + "</td>";
                            resultstr += "</tr>";
                            $("#adpub_case_select").append(resultstr);
                        }
                        $("#adpub_case_count").empty();
                        $("#adpub_case_count").append(pubList.AllCount);
                    }
                    break;
                case 1:
                    alert("111");
                    break;
            }
        }
        function addpubs() {
            if (pubList == null) {
                pubList = new Object();
                pubList.ACList = new Array();
                pubList.AllCount = 0;
            }
            $("input[name='cb_pubids']:checkbox").each(function () {
                if ($(this).is(':checked')) {
                    pubList.ACList.push(tmpPubList.ACList[$(this).val()]);
                    pubList.AllCount += 1;
                }
            });
            showpubselct();
        }
        function removepubs() {
            var i = 0;
            $("input[name='cb_selectpubids']:checkbox").each(function () {
                if ($(this).is(':checked')) {
                    pubList.ACList.splice($(this).val() - i, 1);
                    pubList.AllCount -= 1;
                    i += 1;
                }
            });
            showpubselct();
        }

        function subaudit() {
            if (pubList == null || pubList.ACList.length < 1) {
                alert("请选择发布SSID或者推广方案");
                return;
            }
            var savecase = 0;
            var ids = "";
            switch (pubtype) {
                case 1:
                    if ($("#cb_ascase").is(':checked')) {
                        savecase = 1;
                    }
                    break;
                case 2:
                    for (var i = 0; i < pubList.ACList.length; ++i) {
                        ids += "," + pubList.ACList[i].CASE.AC_ID;
                    }
                    ids = ids.substring(1);
                    break;
            }
            if (ids == "") {
                alert("no data");
                return;
            }
            alert(pubtype + "^^^^^^^^" + ids);
            var iscopy = 0;
            postaudit($("#ad_id").val(), pubtype, ids, savecase, iscopy);
        }
        function postaudit(adid, type, ids, savecase, iscopy) {
            $.ajax({
                type: 'post',
                url: 'PostAudit',
                data: 'AD_ID=' + adid + "&AuditType=" + type + "&IDS=" + ids + "&ascase=" + savecase + "&iscopy=" + iscopy,
                dataType: 'json',
                success: function (obj) {
                    if (obj.ResultCode == "0") {
                        alert("ok");
                    } else {
                        alert(obj.ResultMsg);
                    }
                }
            });
        }
        function postadonly() {
            if ($("#ad_id").val() == "" || $("#ad_id").val() < 1) {
                alert("no ad");
                return;
            }
            postaudit($("#ad_id").val(), 0,"");
        }
    </script>
    
</head>
<body>

    <div class="container-fluid">
      <div  class="row-fluid">
        
        
        <div class="div-body col-md-4" id="ad-edit" name="ad-edit">
          <div class="widget-container  clearfix">
            <div class="heading"><i class='fa fa-fw fa-pencil'></i>广告编辑<a style="float:right" class='goNext visible-xs' href='#ad-view'><i class='fa fa-fw fa-eye'></i>预览</a></div>
            <div class="widget-content padded clearfix">
              <div>
                <form id="formss" action="ADSave" target="upload" method="post" enctype="multipart/form-data" onsubmit="return SaveAD();">
                <input type="hidden" id="ad_id" name="ad_id" value="<%:ViewData["ad_id"]%>" />
                <input type="hidden" id="homepage" name="homepage" value="" />
                <input type="hidden" name="pubcount" value="0" />
                <input type="hidden" id="ad_pubpath" name="ad_pubpath" value="" />
                <input type="hidden" id="temppage" name="temppage" value="" />
                  <table class="hor-minimalist-top table table-bordered table-striped">
                    <tr>
                      <td>广告名称：
                        <input id="ad_title" name="ad_title" type="text" /></td>
                    </tr>
                    <tr>
                      <td>SSID显示名称：
                        <input id="ad_ssid" name="ad_ssid" type="text" />
                        </td>
                    </tr>
                    <tr>
                      <td>广告模版：<%--<select id="ad_model" name="ad_model" onchange="getfilelist()"></select>--%><%=Html.DropDownList("ad_model")%>行业：<%=Html.DropDownList("ad_type")%><div id="modeldiv"/></td>
                    </tr>
                    <tr style="background-color:#eee">
                      <td valign="top">
                       <div id="templetfiles" class="btn-group" data-toggle="buttons">

</div>

<table id="templetcontent" class="hor-minimalist-top table table-bordered table-striped" width="95%">

                  </table>
                  

                        
                        
                        
                        
                        
                        </td>
                    </tr>
                    <tr>
                      <td>&nbsp;
                        <input type="button" id="btn_save" class="go-ad-view" onclick="javascript:submit()" value="保存并预览" />&nbsp;<input type="button" onclick="javascript:postadonly();" value="提交审核"/>&nbsp;<input type="button" value="发布到方案" onclick="javascript:PubToCase(1,0);"/>&nbsp;<input type="button" value="发布到SSID" onclick="javascript:PubToSSID(1,0);"/></td>
                    </tr>
                  </table>
                </form>
              </div>
            </div>
          </div>
        </div>
        
        

        <div class="div-body col-md-4" id="ad-view" name="ad-view">
          <div class="widget-container  clearfix">
            <div class="heading"><i class='fa fa-fw fa-eye'></i>预览<a style="float:right" class='goNext visible-xs' href='#ad-edit'><i class='fa fa-fw fa-pencil'></i>参数</a></div>
            <div class="widget-content padded clearfix">
              <iframe id="PreviwPage" name="PreviwPage" style="float:right; width:100%; height:600px;"></iframe>
            </div>
          </div>
        </div>

        <div class="div-body col-md-4"  id="ad-audit" name="ad-audit">
          <div class="widget-container  clearfix">
            <div class="heading">发布</div>
    <div id="div_adpub">

    </div>

          </div>
        </div>
      </div>
    </div>



    <%--<div><input type="button" onclick="javascript:subaudit();" value="提交发布"/></div>--%>
    
        <div id="dialog-form" title="请选择图片：">
      <fieldset>
      <dl id="dl_serverpics">
      
      </dl>
      </fieldset>
    </div>
        <iframe id="upload" name="upload" style="width:0px; height:0px; border:0px;" ></iframe>
</body>
</html>
