<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Audit.aspx.cs" Inherits="LUOBO.SingleShop.UI.Audit" %>
<%@ Register src="header.ascx" tagname="header" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>next-wifi 审核</title>
<link href="../UITemplet/css/bootstrap.css" media="all" rel="stylesheet" type="text/css" />
<link href="../UITemplet/css/font-awesome.css" media="all" rel="stylesheet" type="text/css" />
<link href="../UITemplet/css/index.css" media="all" rel="stylesheet" type="text/css" />
<link href="../UITemplet/css/style.css" media="all" rel="stylesheet" type="text/css" />

<script type="text/javascript" src="../UITemplet/js/Common.js"></script>
<script src="../UITemplet/js/jquery-1.10.2.min.js" type="text/javascript"></script>
<script src="../UITemplet/js/jquery-ui-1.10.4.min.js" type="text/javascript"></script>
<script src="../UITemplet/js/bootstrap.js" type="text/javascript"></script>
<script src="../UITemplet/js/PageViewJS.js" type="text/javascript"></script>
<style type="text/css">
.adinfo-title
{
    background-color:#DDDDDD;
    font-weight:bold;
    padding-left:10px;  
}
</style>

<script type="text/javascript">
    var token = GetQueryString("token");
    var pageSize = 10;
    var curPage = 1;

    var freehostList = null;

    $(document).ready(function () {
        HighLightMenu("审核");
        GetAuditStateList();
        BindEvent();
    });


</script>

<script type="text/javascript">
    function BindEvent() {
        $('#search_btn').click(function () {
            curPage = 1;
            GetAuditList();
        });
    }

    function HandleAudit(handle, audid, adId) {
        var str = $('#mTr_' + audid).find("[name='audcontent']").val();
        var freehost = $('#mTr_' + audid).find("[name='freehost']").val();
        var defaultFreeDom = $('#chk_' + adId).find('input[type="checkbox"]:checked');
        var defaultStr = "";

        for (var i = 0; i < defaultFreeDom.length; i++) {
            if (i == 0) {
                defaultStr += $(defaultFreeDom[i]).val();
            } else {
                defaultStr += "," + $(defaultFreeDom[i]).val();
            }
        }

        $.ajax({
            type: 'post',
            url: 'AjaxComm.aspx',
            data: 'type=HandleAudit&Token=' + token + '&param={"handle": ' + handle + ',"audid":' + audid + ',"auditstr":"' + str + '","freehost":"' + freehost + '","adId":' + adId + ',"defaultfree":"' + defaultStr + '"}',
            dataType: 'json',
            success: function (obj) {
                if (obj.ResultCode == "0") {
                    alert("审核成功");
                    GetAuditList();
                } else {
                    alert(obj.ResultMsg);
                }
            }
        });
    }
</script>

<script type="text/javascript">
    function GetAuditList() {
        $('#divPage').empty();
        $('#audTable tbody').html("<tr><td colspan='7'>载入中</td></tr>");

        var postDataType = "";
        var state = $("#slt_audit_state").val();
        if (state == 0 || state == 1) {
            postDataType = "GetAuditList";
        }
        else {
            postDataType = "GetAuditHistoryList";
        }

        $.ajax({
            type: 'post',
            url: 'AjaxComm.aspx',
            data: 'type=' + postDataType + '&Token=' + token + '&size=' + pageSize + '&curPage=' + curPage + '&param=' + state,
            dataType: 'json',
            error: function (msg) {
            },
            success: function (obj) {
                if (obj.ResultCode == 0) {
                    if (obj.ResultOBJ.AuditList.length > 0) {
                        var str = "";
                        var item;
                        for (var i = 0, length = obj.ResultOBJ.AuditList.length; i < length; i++) {
                            item = obj.ResultOBJ.AuditList[i];
                            str += "<tr id='audTr_" + item.AUD_ID + "'>";
                            str += "<td><i class='fa fa-chevron-circle-right'></i>&nbsp;&nbsp;&nbsp;" + (i + 1 + (curPage - 1) * pageSize) + "</td>";
                            str += "<td>" + item.AUD_ID + "</td>";
                            str += "<td>" + getPubTypeStr(item.PUB_TYPE) + "</td>";
                            str += "<td>" + item.FROM_USER + "</td>";
                            str += "<td>" + dateFormat(item.FROM_DATE, "yyyy-MM-dd hh:mm:ss") + "</td>";
                            str += "<td>" + getStaStr(item.AUD_STAT) + "</td>";
                            str += "<td name='op'>" + getOpStr(state, item.AD_ID, item.AUD_ID, item.AUD_PARENTID, true, (item.PUB_TYPE == 4 ? item.SSID_NAME : null)) + "</td>";
                            str += "</tr>";

                            str += "<tr id='mTr_" + item.AUD_ID + "' style='display:none;'><td colspan='7'></td></tr>";
                        }
                        $('#audTable tbody').html(str);
                    }
                    else {
                        $('#audTable tbody').html("<tr><td colspan='7'>暂时没有需要审核的数据</td></tr>");
                    }

                    if (obj.ResultOBJ.AllCount == 0)
                        allPage = 1;
                    else
                        allPage = obj.ResultOBJ.AllCount % pageSize == 0 ? obj.ResultOBJ.AllCount / pageSize : parseInt(obj.ResultOBJ.AllCount / pageSize) + 1;
                    ShowPage({ CurrentPage: curPage, MaxPageSize: allPage, PageShowSize: pageSize, IsUpDown: true, ShowElement: document.getElementById("divPage"), PageEvents: function (page) { curPage = page; GetAuditList(); } });
                } else {
                    //alert(obj.ResultMsg);
                }
            }
        });
    }

    function GetAuditStateList() {
        $.ajax({
            type: 'post',
            url: 'AjaxComm.aspx',
            data: 'type=GetDicByAuditState&token=' + token,
            dataType: 'json',
            error: function (msg) {
                //alert("服务器错误");
            },
            success: function (obj) {
                if (obj.ResultCode == 0) {
                    $("#slt_audit_state").empty();
                    for (var i = 0; i < obj.ResultOBJ.length; i++) {
                        $("#slt_audit_state").append("<option value='" + obj.ResultOBJ[i].VALUE + "'>" + obj.ResultOBJ[i].NAME + "</option>");
                    }
                    $("#slt_audit_state")[0].selectedIndex = 0;
                    GetAuditList();
                } else {
                    //alert(obj.ResultMsg);
                }
            }
        });
    }

    function GetDefaultFreeUrlList(_dom, adid, _adinfo, type) {
        if ($('#' + _dom) == null)
            return;

        $('#' + _dom).html("读取中");
        if (freehostList == null) {
            $.ajax({
                type: 'post',
                url: 'AjaxComm.aspx',
                data: 'type=GetDicByFreeHost&token=' + token,
                dataType: 'json',
                error: function (msg) {
                    //alert("服务器错误");
                },
                success: function (obj) {
                    if (obj.ResultCode == 0) {
                        freehostList = obj.ResultOBJ;
                        $('#' + _dom).html(GetFreeHostStr(adid, _adinfo, type));
                    } else {
                        //alert(obj.ResultMsg);
                    }
                }
            });
        } else {
            $('#' + _dom).html(GetFreeHostStr(adid, _adinfo, type));
        }
    }

    function GetFreeHostStr(adid, _adinfo, type) {
        var info = "";
        info += "<div class='checkbox' style='margin-left:5px;'><label><input type='checkbox' name='chk_" + adid + "' value='" + freehostList[0].VALUE + "' checked disabled>" + freehostList[0].NAME + "</label></div>"
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
                info += "<div class='checkbox' style='margin-left:10px;'><label><input type='checkbox' name='chk_" + adid + "' value='" + freehostList[i].VALUE + "' " + (type != 0 ? "disabled" : "") + " " + (flag ? "checked" : "") + " > " + freehostList[i].NAME + "</label></div>";
            }
        }
        return info;
    }

    function GetAdInfo(adid, audid, pid, type, ssidname) {
        $("#audTr_" + audid).find("i").removeClass().addClass("fa fa-chevron-circle-down");
        $('#audTr_' + audid).find("td[name='op']").html(getOpStr(type, adid, audid, pid, false, ssidname));
        $('#mTr_' + audid).html("<td colspan='7'>读取中</td>");
        $("#mTr_" + audid).slideDown("fast");
        $.ajax({
            type: 'post',
            url: 'AjaxComm.aspx',
            data: 'type=GetADInfoAndFreeHost&token=' + token + '&ad_id=' + adid,
            dataType: 'json',
            error: function (msg) {
                //alert("服务器错误");
            },
            success: function (obj) {
                if (obj.ResultCode == 0) {

                    GetAuditProgess(audid, type);
                    GetSSIDInfo(audid);
                    var adinfo = "";
                    adinfo += "广告名称：" + obj.ResultOBJ.AD_Title + "<br/>";

                    if (ssidname != "null") {
                        adinfo += "待审核SSID名称：" + ssidname;
                    }
                    var info = "";
                    info += "<div>";
                    info += "<div class='adinfo-title'>广告信息</div>";
                    info += "<div>" + adinfo + "</div>";
                    info += "<div class='adinfo-title'>审核进度</div>";
                    info += "<div><table style='font-size:12px;width:100%'><thead><tr>";
                    info += "<th scope='col'>时间</th>";
                    info += "<th scope='col'>商家名称</th>";
                    info += "<th scope='col'>管理员</th>";
                    info += "<th scope='col'>操作</th></tr>";
                    info += "</thead>";
                    info += "<tbody name='auditProgess'></tbody>";
                    info += "</table></div>";

                    info += "<div class='adinfo-title'>审核意见</div>";

                    info += "<div><textarea name='audcontent' style='width:100%; height:50px;' " + (type != 0 ? "disabled" : "") + " ></textarea></div>";
                    if (ssidname == "null") {
                        info += "<div class='adinfo-title'>放行域名</div>";
                        info += "<div class='form-inline' id='chk_" + obj.ResultOBJ.AD_ID + "'></div>";
                        info += "<div><textarea name='freehost' style='width:100%; height:50px;' " + (type != 0 ? "disabled" : "") + ">" + (obj.ResultOBJ.F_Host == null ? "" : obj.ResultOBJ.F_Host) + "</textarea></div>";
                    }
                    if (type == 0) {
                        info += "<div style='margin-left:50px;margin-top:20px;'><a class='btn btn-default' href='javascript:HandleAudit(1," + audid + "," + adid + ");'>审核通过</a><a class='btn btn-default' style='margin-left:20px;' href='javascript:HandleAudit(2," + audid + "," + adid + ");'>审核拒绝</a></div>";
                    }

                    info += "</div>";

                    var mTrStr = "<td colspan='7'><table style='width:100%'><tr style='height:28px;'><td><span class='label label-info'>广告预览</span></td><td><span class='label label-info'>详情</span></td><td><span class='label label-info'>发布到店</span></td></tr><tr>";
                    mTrStr += "<td valign='top' style='width:400px;'><iframe src='/ADUserFile/" + obj.ResultOBJ.ORG_ID + "/" + obj.ResultOBJ.AD_ID + "/" + obj.ResultOBJ.AD_HomePage + "' style='width:100%;height:460px;'></iframe></td>";
                    mTrStr += "<td valign='top' style='width:470px;font-size:14px;'>" + info + "</td>";
                    mTrStr += "<td valign='top'><table name='ssidTable' class='hor-minimalist-a table table-bordered table-striped'><thead><tr><th>序号</th><th>机构</th><th>营业厅</th><th>SSID名称</th></tr></thead><tbody><tr><td colspan='4'>读取中</td></tr></tbody></table></td>";
                    mTrStr += "</tr></table></td>";

                    $("#mTr_" + audid).html(mTrStr);
                    GetDefaultFreeUrlList("chk_" + adid, adid, obj.ResultOBJ, type);

                } else {
                    alert(obj.ResultMsg);
                    //if (obj.ResultCode == -100) {
                    //    window.location.href = "login.aspx";
                    //}
                }
            }
        });
    }

    function GetAuditProgess(audid, type) {
        var postDataType = "";
        if (type == 0 || type == 1) {
            postDataType = "GetAuditProgress";
        } else {
            postDataType = "GetAuditHistoryProgress";
        }
        $.ajax({
            type: 'post',
            url: 'AjaxComm.aspx',
            data: 'type=' + postDataType + '&token=' + token + '&param=' + audid,
            dataType: 'json',
            success: function (obj) {
                if (obj.ResultCode == "0") {
                    result = "";
                    if (obj.ResultOBJ.length > 0) {
                        var item;
                        for (var i = 0; i < obj.ResultOBJ.length; i++) {
                            item = obj.ResultOBJ[i];
                            result += "<tr>";
                            result += "<td>" + dateFormat(item.FROM_DATE,"yyyy-MM-dd hh:mm:ss") + "</td>";
                            result += "<td>" + item.FROM_ORG_NAME_V + "</td>";
                            result += "<td>" + item.FROM_USER + "</td>";
                            result += "<td>" + getFromTypeStr(item.FROM_TYPE) + "</td>";
                            result += "</tr>";
                        }
                        $("#mTr_" + audid).find("[name='auditProgess']").html(result);
                    }
                } else {
                    alert(obj.ResultMsg);
                }
            }
        });
    }

    function GetSSIDInfo(audid) {
        $.ajax({
            type: 'post',
            url: 'AjaxComm.aspx',
            data: 'type=APManage/GetSSIDListByIDs&token=' + token + '&param=' + audid,
            dataType: 'json',
            success: function (obj) {
                if (obj.ResultCode == "0") {
                    result = "";
                    if (obj.ResultOBJ.length > 0) {
                        var item;
                        for (var i = 0; i < obj.ResultOBJ.length; i++) {
                            item = obj.ResultOBJ[i];
                            result += "<tr>";
                            result += "<td>" + (i + 1) + "</td>";
                            result += "<td>" + item.ONAME + "</td>";
                            result += "<td>" + item.APNAME + "</td>";
                            result += "<td>" + item.SSIDNAME + "</td>";
                            result += "</tr>";
                        }
                        $("#mTr_" + audid).find("[name='ssidTable'] tbody").html(result);
                    } else {
                        $("#mTr_" + audid).find("[name='ssidTable'] tbody").html("<tr><td colspan='4'>未发布到SSID</td></tr>");
                    }
                } else {
                    alert(obj.ResultMsg);
                }
            }
        });
    }


    function HideAdInfo(adid, audid, pid, type, ssidname) {
        $("#mTr_" + audid).slideUp("fast");
        $("#audTr_" + audid).find("i").removeClass().addClass("fa fa-chevron-circle-right");
        $('#audTr_' + audid).find("td[name='op']").html(getOpStr(type, adid, audid, pid, true, ssidname));
    }
</script>

<script type="text/javascript">
    function getStaStr(type) {
        var str = "";
        switch (type) {
            case 0:
                str = "<span class='label label-warning'>待审核</span>";
                break;
            case 1:
                str = "<span class='label label-warning'>审核中</span>";
                break;
            case 2:
                str = "<span class='label label-success'>审核通过</span>";
                break;
            case 3:
                str = "<span class='label label-danger'>审核未通过</span>";
                break;
        }
        return str;
    }

    function getOpStr(type, adid, audid, pid, toOpen, ssidname) {
        var str = "";

        if (toOpen) {
            str = "<a href='javascript:GetAdInfo(" + adid + "," + audid + "," + pid + "," + type + ",\"" + ssidname + "\")'>【查看详情】</a>";
        } else {
            str = "<a href='javascript:HideAdInfo(" + adid + "," + audid + "," + pid + "," + type + ",\"" + ssidname + "\")'>【隐藏详情】</a>";
        }
        return str;
    }

    function getPubTypeStr(type) {
        var str = "";
        switch (type) {
            case 0:
                str = "广告发布到单一设备";
                break;
            case 1:
                str = "广告发布到多设备";
                break;
            case 2:
                str = "广告发布到方案";
                break;
            case 3:
                str = "SSID发布到多设备";
                break;
            case 4:
                str = "SSID发布到单一设备";
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
</script>
</head>
<body>
<uc1:header runat="server" />

<div class="container-fluid">
<div class="row-fluid">
    <div class="div-body col-md-12">
        <div class="panel panle_ssid">
            <div class="panel-heading">
                <span class="label label-success" style="font-size:14px;line-height: 30px;">内容发布审核</span>
                <span class="pull-right">
                    <select id="slt_audit_state" style="width:130px;font-size:12px;height:28px"><option>载入中</option></select>
                    <input id="search_txt" type="search" class="input-sm" placeholder="查询关键字.."/>
                    <a id="search_btn" class="btn btn-sm btn-success" href="javascript:void(0);"><i class="icon-twitter"></i>查询</a>
                </span>
                <div class="clearfix"></div>
            </div>

            <div class="widget-content padded clearfix">
               <table class="hor-minimalist-a table table-bordered table-striped" id="audTable" style="margin-bottom:0px;">
                <thead>
                    <tr>
                    <th scope="col">序号</th>
                    <th scope="col">审核单号</th>
                    <th scope="col">发布方式</th>
                    <th scope="col">提交人</th>
                    <th scope="col">提交时间</th>
                    <th scope="col">审核状态</th>
                    <th scope="col">操作</th>
                    </tr>
                </thead>
                <tbody><tr><td colspan='7'>载入中</td></tr></tbody>
                </table>
                <ul class="pagination" id="divPage"></ul>
            </div>
        </div>
    </div>
</div>
</div>
</body>
</html>
