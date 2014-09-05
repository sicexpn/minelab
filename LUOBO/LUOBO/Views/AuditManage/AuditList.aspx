<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>ADList</title>
    <script src="../../Scripts/Common.js" type="text/javascript"></script>
    <link href="../../Content/Site.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../../Scripts/jquery-ui.css"/>
    <link href="../../Content/bootstrap.css" media="all" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/bootstrap.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.10.2.js"></script>
    <script src="../../Scripts/jquery-ui.js"></script>

    <script type="text/javascript">
        var keystr = "";
        var size = 10;
        var allPage = 0;
        var currentPage = 1;
        var astatu = -1;
        var freehostList = null;

        $(function () {
            FindAuditList(1);

            $("#dialog-form").dialog({
                autoOpen: false,
                height: 650,
                width: 750,
                modal: true,
                buttons: {
                    "审核通过": function () {
                        if (confirm("确认要【通过】该广告审核申请？")) {
                            //alert("确认");
                            HandleAudit(1, $("#audcontent").val(),$("#freehost").val());
                            //$(this).dialog("close");
                        }
                    },
                    "审核拒绝": function () {
                        if (confirm("确认要【拒绝】该广告审核申请？")) {
                            //alert("确认");
                            HandleAudit(2, $("#audcontent").val(),$("#freehost").val());
                            //$(this).dialog("close");
                        }
                    },
                    "关闭": function () {
                        $(this).dialog("close");
                    }
                },
                close: function () {

                }
            });

            $("#dialog-view").dialog({
                autoOpen: false,
                height: 500,
                width: 400,
                modal: true,
                buttons: {
                    "关闭": function () {
                        $(this).dialog("close");
                    }
                },
                close: function () {

                }
            });
        });

        function FindAuditList(curPage) {
            currentPage = curPage;
            var result = "";
            $.ajax({
                type: 'post',
                url: 'FindAuditList',
                data: 'size=' + size + '&curPage=' + curPage + '&statu=' + astatu,
                dataType: 'json',
                success: function (obj) {
                    $("#tbdAPCTList").empty();
                    $("#divPage").empty();
                    if (obj.ResultCode == "0") {
                        if (obj.ResultOBJ.AuditList.length > 0) {
                            var item;
                            for (var i = 0; i < obj.ResultOBJ.AuditList.length; i++) {
                                item = obj.ResultOBJ.AuditList[i];
                                result = "<tr>";
                                result += "<td>" + item.AUD_ID + "</td>";
                                result += "<td>" + item.ORG_NAME_V + "</td>";
                                result += "<td>" + item.AD_NAME_V + "</td>";
                                result += "<td>" + item.AD_SSID_V + "</td>";
                                result += "<td>" + item.FROM_ORG_NAME_V + "</td>";
                                result += "<td>" + item.FROM_DATE_S + "</td>";
                                result += "<td>" + getStaStr(item.AUD_STAT, item.AD_ID, item.AUD_ID, item.AUD_PARENTID) + "</td>";
                                result += "</tr>";
                                $("#tbdAPCTList").append(result);
                            }
                            allPage = obj.ResultOBJ.AllCount % size == 0 ? obj.ResultOBJ.AllCount / size : parseInt(obj.ResultOBJ.AllCount / size) + 1;
                            ShowPage({ CurrentPage: curPage, MaxPageSize: allPage, PageShowSize: 5, IsUpDown: true, ShowElement: document.getElementById("divPage"), PageEvents: function (page) { FindAuditList(page); } });
                        }
                    } else {
                        alert(obj.ResultMsg);
                    }
                }
            });
        }

        function getStaStr(type, adid, audid, pid) {
            var str = "";
            switch (type) {
                case 0:
                    str = "<span class='label label-warning'>待审核</span> <a href='javascript:AuditAD(" + adid + "," + audid + "," + pid + "," + type + ")'>【审核】</a>";
                    break;
                case 1:
                    str = "<span class='label label-warning'>审核中</span> <a href='javascript:viewaudit(" + audid + "," + pid + "," + type + ")'>【查看进度】</a>";
                    break;
                case 2:
                    str = "<span class='label label-success'>审核通过</span>";
                    break;
                case 3:
                    str = "<span class='label label-success'>审核未通过</span>";
                    break;
            }
            return str;
        }

        function getFromTypeStr(type) {
            var str = "";
            switch (type) {
                case 0:
                    str = "提交申请";
                    break;
                case 1:
                    str = "同意申请";
                    break;
                case 2:
                    str = "拒绝申请";
                    break;
            }
            return str;
        }

        var this_audid;
        var this_adid;
        function AuditAD(adid, audid, pid,type) {
            this_audid = audid;
            this_adid = adid;
            $("#adinfo").empty();
            $("#PreviwPage").attr("src", "");
            $.ajax({
                type: 'post',
                url: '../ADManage/GetADInfo',
                data: 'AD_ID=' + adid,
                dataType: 'json',
                success: function (obj) {
                    if (obj.ResultCode == "0") {
                        GetDefaultFreeUrlList(obj.ResultOBJ, type);
                        result = "";
                        result += "广告名称：" + obj.ResultOBJ.AD_Title + "<br/>";
                        result += "显示SSID：" + obj.ResultOBJ.AD_SSID;
                        $("#adinfo").append(result);
                        $("#PreviwPage").attr("src", "/ADUserFile/" + obj.ResultOBJ.ORG_ID + "/" + obj.ResultOBJ.AD_ID + "/" + obj.ResultOBJ.AD_HomePage);
                    } else {
                        alert(obj.ResultMsg);
                    }
                }
            });
            getaudit(audid, pid);
            $("#dialog-form").dialog("open");
        }

        function viewaudit(audid, pid,type) {
            getaudit(audid, pid);
            $("#dialog-view").dialog("open");
        }

        function getaudit(audid, pid) {
            var tmpid;
            if (pid > 0) {
                tmpid = pid;
            } else {
                tmpid = audid;
            }
            $("#auditinfo").empty();
            $("#auditinfo_v").empty();
            $.ajax({
                type: 'post',
                url: 'GetAuditProgress',
                data: 'audid=' + tmpid,
                dataType: 'json',
                success: function (obj) {
                    if (obj.ResultCode == "0") {
                        result = "";
                        if (obj.ResultOBJ.length > 0) {
                            var item;
                            for (var i = 0; i < obj.ResultOBJ.length; i++) {
                                item = obj.ResultOBJ[i];
                                result = "<tr>";
                                result += "<td>" + item.FROM_DATE_S + "</td>";
                                result += "<td>" + item.FROM_ORG_NAME_V + "</td>";
                                result += "<td>" + item.FROM_USER + "</td>";
                                result += "<td>" + getFromTypeStr(item.FROM_TYPE) + "</td>";
                                result += "</tr>";
                                $("#auditinfo").append(result);
                                $("#auditinfo_v").append(result);
                            }
                        }
                    } else {
                        alert(obj.ResultMsg);
                    }
                }
            });
        }

        function HandleAudit(handle, str, freehost) {
            //alert('handle=' + handle + '&audid=' + this_audid + '&auditstr=' + str + "&freehost=" + freehost + "&adId" + this_adid);

            var defaultStr = "";
            var defaultFreeDom = $('#chk_freehost').find('input[type="checkbox"]:checked');

            for (var i = 0; i < defaultFreeDom.length; i++) {
                if (i == 0) {
                    defaultStr += $(defaultFreeDom[i]).val();
                } else {
                    defaultStr += "," + $(defaultFreeDom[i]).val();
                }
            }



            $.ajax({
                type: 'post',
                url: 'HandleAudit',
                data: 'handle=' + handle + '&audid=' + this_audid + '&auditstr=' + str + "&freehost=" + freehost + "&adId=" + this_adid + "&defaultfree=" + defaultStr,
                dataType: 'json',
                success: function (obj) {
                    if (obj.ResultCode == "0") {
                        alert("审核成功");
                        $("#dialog-form").dialog("close");
                        FindAuditList(currentPage);
                    } else {
                        alert(obj.ResultMsg);
                    }
                }
            });
        }


        function GetDefaultFreeUrlList(_adinfo, type) {

            $('#chk_freehost').html("读取中");
            if (freehostList == null) {
                $.ajax({
                    type: 'post',
                    url: 'GetDicByFreeHost',
                    dataType: 'json',
                    error: function (msg) {
                        //alert("服务器错误");
                    },
                    success: function (obj) {
                        if (obj.ResultCode == 0) {
                            freehostList = obj.ResultOBJ;
                            $('#chk_freehost').html(GetFreeHostStr(_adinfo, type));
                        } else {
                            //alert(obj.ResultMsg);
                        }
                    }
                });
            } else {
                $('#chk_freehost').html(GetFreeHostStr(_adinfo, type));
            }
        }

        function GetFreeHostStr(_adinfo, type) {
            var info = "";
            info += "<div class='checkbox' style='margin-left:5px;'><label><input type='checkbox' name='chk' value='" + freehostList[0].VALUE + "' checked disabled>" + freehostList[0].NAME + "</label></div>"
            if (_adinfo.F_Default == null) {
                for (var i = 1; i < freehostList.length; i++) {
                    info += "<div class='checkbox' style='margin-left:5px;'><label><input type='checkbox' value='" + freehostList[i].VALUE + "' " + (type != 0 ? "disabled" : "") + ">" + freehostList[i].NAME + "</label></div>"
                }
            } else {
                var def = _adinfo.F_Default.split(',');
                var flag = false;
                for (var i = 1; i < freehostList.length; i++) {
                    flag = false;
                    for (var j = 0; j < def.length; j++) {
                        if (def[j] == freehostList[i].VALUE) {
                            flag = true;
                            break;
                        }
                    }
                    info += "<div class='checkbox' style='margin-left:10px;'><label><input type='checkbox' name='chk' value='" + freehostList[i].VALUE + "' " + (type != 0 ? "disabled" : "") + " " + (flag ? "checked" : "") + " > " + freehostList[i].NAME + "</label></div>";
                }
            }
            return info;
        }
    </script>
</head>
<body>
    
    <div class="div-body">
        <div class="div-body-top">
            <table class="hor-minimalist-top" width="100%">
                <tr>
                    <td>
                        广告审核
                    </td>
                </tr>
            </table>
        </div>
        <div class="widget-content padded clearfix">
              <table class="hor-minimalist-a table table-bordered table-striped">
                <thead>
                    <tr>
                        <th scope="col">序号</th>
                        <th scope="col">商家名称</th>
                        <th scope="col">广告名称</th>
                        <th scope="col">SSID名称</th>
                        <th scope="col">提交机构</th>
                        <th scope="col">提交时间</th>
                        <th scope="col">审核状态</th>
                    </tr>
                </thead>
                <tbody id="tbdAPCTList">
                
                </tbody>
            </table>
        </div>
    </div>
    <div id="divPage" class="hor-minimalist-page"></div>

    <div id="dialog-form" title="广告审核：">
        <table width="100%">
            <tr>
                <td width="300px">预览
                
                </td>
                <td rowspan="2" valign="top">
                    <div style="width:100%; background-color:#EEEEEE;">广告信息</div>
                    <div id="adinfo"></div>
                    <div style="width:100%; background-color:#EEEEEE;">审核进度</div>
                    <div>
                        <table>
                            <thead><tr>
                                <th scope='col'>时间</th>
                                <th scope='col'>商家名称</th>
                                <th scope='col'>管理员</th>
                                <th scope='col'>操作</th></tr>
                            </thead>
                            <tbody id="auditinfo"></tbody>
                        </table>
                    </div>
                    <div style="width:100%; background-color:#EEEEEE;">审核意见</div>
                    <div style="width:100%;"><textarea id="audcontent" name="audcontent" style="width:100%; height:50px;"></textarea></div>
                    <div style="width:100%; background-color:#EEEEEE;">放行域名</div>
                    <div class='form-inline' id="chk_freehost"></div>
                    <div style="width:100%;"><textarea id="freehost" name="freehost" style="width:100%; height:50px;"></textarea></div>

                </td>
            </tr>
            <tr>
                <td><iframe id="PreviwPage" name="PreviwPage" style="float:right; width:100%; height:460px;"></iframe></td>
            </tr>
        </table>
    </div>
    
    <div id="dialog-view" title="广告审核进度：">
        <table width="100%">
            <thead><tr>
                <th scope='col'>时间</th>
                <th scope='col'>商家名称</th>
                <th scope='col'>管理员</th>
                <th scope='col'>操作</th></tr>
            </thead>
            <tbody id="auditinfo_v"></tbody>
        </table>
    </div>
</body>
</html>
