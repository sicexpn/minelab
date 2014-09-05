<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>APSetting</title>
    <script src="../../Scripts/Common.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.ztree.core-3.5.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.ui.core.js" type="text/javascript"></script>
	<script src="../../Scripts/jquery.ui.widget.js" type="text/javascript"></script>
	<script src="../../Scripts/jquery.ui.tabs.js" type="text/javascript"></script>
    <script src="../../Scripts/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <link href="../../Content/themes/base/jquery.ui.all.css" rel="stylesheet" />
    <link href="../../Content/Site.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/zTreeStyle/zTreeStyle.css" rel="stylesheet" type="text/css" />
    
    <script type="text/javascript">
        var zNodes = new Array();
        var SSIDs;
        var g_ap;
        var g_cur_ssid_idx = -1;
        var orig_ssids;
        var setting = {
            data: {
                simpleData: {
                    enable: true,
                    idKey: "id",
                    pIdKey: "pId"
                }
            },
            callback: {
                onClick: onClick
            }
        };

        function onClick(event, treeId, treeNode, clickFlag){
            if (treeNode.level > 0) {
                for (var i = 0; i < SSIDs.length; i++) {
                    if (SSIDs[i].ID == treeNode.id) {
                        g_cur_ssid_idx = i;
                        $('#ssid_ison').attr("checked", SSIDs[i].ISON);
                        $('#ssid_interneton').attr("checked", SSIDs[i].ISINTERNET);
                        $('#ssid_name').val(SSIDs[i].NEWNAME);
                        $('#ssid_power').attr("checked", SSIDs[i].ISON);
                        $('#ssid_maxlinknum').val(SSIDs[i].MAXLINKCOUNT);
                        $('#ssid_vmaxus').val(SSIDs[i].VMAXUS);
                        $('#ssid_maxus').val(SSIDs[i].MAXUS);
                        $('#ssid_vmaxds').val(SSIDs[i].VMAXDS);
                        $('#ssid_maxds').val(SSIDs[i].MAXDS);
                        $('#ssid_vonlinetime').val(SSIDs[i].VONLINETIME);
                        $('#ssid_portal').val(SSIDs[i].PORTAL);
                        $('#ssid_banport').val(SSIDs[i].ports);
                        $('#ssid_banurl').val(SSIDs[i].urls);
                        $('#ssid_banmac').val(SSIDs[i].macs);
                        break;
                    }
                }
            }
        }

        $(document).ready(function () {
            $("#tab1").tabs();
            $("#tab2").tabs();
            g_ap = $('#apid').val();
            FindSSIDList(g_ap);
            $.ajax({
                type: 'post', //可选get
                url: 'GetAPCTListForSelect', //这里是接收数据的PHP程序
                dataType: 'json',
                success: function (obj) {
                    if (obj.ResultCode == 0) {
                        for (var i = 0; i < obj.ResultOBJ.length; i++) {
                            $("#sltAPCT").append("<option value='" + obj.ResultOBJ[i].ID + "'>" + obj.ResultOBJ[i].TNAME + "</option>");
                        }
                        $("#sltAPCT").val($("#apctid").val());
                    } else {
                        alert(obj.ResultMsg);
                    }
                }
            });

            for (var i = 0; i < 13; i++) {
                $("#sltCHANNEL").append("<option value='" + (i + 1) + "'>" + (i + 1) + "</option>")
            }
            $("#sltCHANNEL").val($("#channel").val());
            $("#sltAERIALTYPE").val($("#aerialtype").val());

            $('#btnSaveBase').click(function () { SaveAPBase(); });
            $('#btn_ssid_save').click(function () { SaveSSID(); });
            $('#btn_back').click(function () { history.back(); });
            $('#btn_submit').click(function () { CreateSettingFile(); });
        });

        function FindSSIDList(_apid) {
            $.ajax({
                type: 'post', //可选get
                url: 'FindSSIDListByAPID', //这里是接收数据的PHP程序
                data: 'apid=' + _apid,
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    zNodes.length = 0;
                    var data;
                    data = new Object();
                    data.id = _apid;
                    data.pId = -1;
                    data.open = true;
                    data.name = $('#apname').val() + "设备";
                    zNodes.push(data);
                    SSIDs = obj;

                    if (obj.length > 0) {
                        for (var i = 0; i < obj.length; i++) {
                            obj[i].NAME = obj[i].NAME.trim();
                            obj[i].NEWNAME = obj[i].NAME;
                            data = new Object();
                            data.id = obj[i].ID;
                            data.pId = _apid;
                            data.name = obj[i].NAME;
                            zNodes.push(data);
                        }
                    } else {
                        data = new Object();
                        data.id = 99999;
                        data.pId = _apid;
                        data.name = "无任何SSID";
                        zNodes.push(data);
                    }
                    orig_ssids = clone(obj);
                    $.fn.zTree.init($("#treeDemo"), setting, zNodes);
                }
            });
        }

        function SaveAPBase() {
            var _support3g = $("input[type='radio'][name='support3g']:checked").val() == 1 ? true : false;
            var _isssidon = $("input[type='radio'][name='isssidon']:checked").val() == 1 ? true : false;
            var _isreboot;
            if ($('#chkreboot') == null) {
                _isreboot = null;
            }
            else {
                if ($('#chkreboot').attr('checked') == null)
                    _isreboot = false;
                else
                    _isreboot = true;
            }

            $.ajax({
                type: 'post',
                url: 'SaveAPBase',
                data: { ID: $('#apid').val(), SERIAL: $('#txtSerial').val(), MODEL: $('#txtModel').val(), MANUFACTURER: $('#txtManuf').val(), PURCHASER: $('#txtPurchaser').val(), FIRMWAREVERSION: $('#txtFirmVersion').val(), MAXSSIDNUM: $('#txtMSSIDCount').val(), SUPPORT3G: _support3g, APCTID: $('#sltAPCT').val(), DESCRIPTION: $('#txtDescription').val(), HBINTERVAL: $('#txtHBInterval').val(), DATAINTERVAL: $('#txtDataInterval').val(), ISREBOOT: _isreboot, CHANNEL: $("#sltCHANNEL").val(), POWER: $("#txtPOWER").val(), AERIALTYPE: $("#sltAERIALTYPE").val(), ISSSIDON: _isssidon },
                dataType: 'json',
                success: function (obj) {
                    if (obj.ResultCode == 0) {
                        if (_isreboot)
                            $('#reboot_div').html("正在重启");
                        alert("保存成功");
                    }
                    else
                        alert("操作失败，错误原因：" + obj.ResultMsg);
                }
            });
        }

        function SaveSSID() {

            SSIDs[g_cur_ssid_idx].ISON = ($('#ssid_ison').attr("checked") == "checked");
            SSIDs[g_cur_ssid_idx].ISINTERNET = $('#ssid_interneton').attr("checked") == "checked";
            SSIDs[g_cur_ssid_idx].NEWNAME = $('#ssid_name').val();
            //SSIDs[g_cur_ssid_idx].ISON = $('#ssid_power').attr("checked") == "checked";
            SSIDs[g_cur_ssid_idx].MAXLINKCOUNT = $('#ssid_maxlinknum').val();
            SSIDs[g_cur_ssid_idx].VMAXUS = $('#ssid_vmaxus').val();
            SSIDs[g_cur_ssid_idx].MAXUS = $('#ssid_maxus').val();
            SSIDs[g_cur_ssid_idx].VMAXDS = $('#ssid_vmaxds').val();
            SSIDs[g_cur_ssid_idx].MAXDS = $('#ssid_maxds').val();
            SSIDs[g_cur_ssid_idx].VONLINETIME = $('#ssid_vonlinetime').val();
            SSIDs[g_cur_ssid_idx].PORTAL = $('#ssid_portal').val();
            SSIDs[g_cur_ssid_idx].ports = $('#ssid_banport').val();
            SSIDs[g_cur_ssid_idx].urls = $('#ssid_banurl').val();
            SSIDs[g_cur_ssid_idx].macs = $('#ssid_banmac').val();

            if (orig_ssids[g_cur_ssid_idx].ISON != SSIDs[g_cur_ssid_idx].ISON) {
                SSIDs[g_cur_ssid_idx].ISUPDATE = true;
            }
            if (orig_ssids[g_cur_ssid_idx].NAME != SSIDs[g_cur_ssid_idx].NEWNAME) {
                SSIDs[g_cur_ssid_idx].ISUPDATE = true;
                SSIDs[g_cur_ssid_idx].ISAUDIT = true;
            }

            $.ajax({
                type: 'post',
                url: 'SaveSSID',
                data: (SSIDs[g_cur_ssid_idx]),
                dataType: 'json',
                success: function (obj) {
                    if (obj.ResultCode == 0) {
                        alert("保存成功");

                        zNodes.length = 0;
                        var data;
                        data = new Object();
                        data.id = g_ap;
                        data.pId = -1;
                        data.open = true;
                        data.name = $('#apname').val() + "设备";
                        zNodes.push(data);

                        if (SSIDs.length > 0) {
                            for (var i = 0; i < SSIDs.length; i++) {
                                data = new Object();
                                data.id = SSIDs[i].ID;
                                data.pId = g_ap;
                                data.name = SSIDs[i].NAME;
                                zNodes.push(data);
                            }
                        } else {
                            data = new Object();
                            data.id = 99999;
                            data.pId = g_ap;
                            data.name = "无任何SSID";
                            zNodes.push(data);
                        }
                        $.fn.zTree.init($("#treeDemo"), setting, zNodes);
                    }
                    else {
                        alert("操作失败，错误原因：" + obj.ResultMsg);
                    }
                }
            });
        }

        function CreateSettingFile() {

                $.ajax({
                    type: 'post',
                    url: 'SubmitSetting',
                    data: 'APID=' + g_ap,
                    dataType: 'json',
                    success: function (obj) {
                        if (obj.ResultCode == 0) {
                            alert("发布成功,请稍后查看");
                        }
                        else {
                            alert("操作失败，错误原因：" + obj.ResultMsg);
                        }
                    }
                });
        }

    </script>
</head>
<body>
    <input id="apid" type="hidden" value="<%=((LUOBO.Entity.SYS_AP_VIEW)ViewData["APDEVICE"]).ID%>" />
    <input id="apname" type="hidden" value="<%=((LUOBO.Entity.SYS_AP_VIEW)ViewData["APDEVICE"]).ALIAS%>" />
    <input id="apctid" type="hidden" value="<%=((LUOBO.Entity.SYS_AP_VIEW)ViewData["APDEVICE"]).APCTID%>" />
    <input id="issuport3g" type="hidden" value="<%=((LUOBO.Entity.SYS_AP_VIEW)ViewData["APDEVICE"]).SUPPORT3G%>" />
    <input id="isssidon" type="hidden" value="<%=((LUOBO.Entity.SYS_AP_VIEW)ViewData["APDEVICE"]).ISSSIDON%>" />
    <input id="channel" type="hidden" value="<%=((LUOBO.Entity.SYS_AP_VIEW)ViewData["APDEVICE"]).APCHANNEL%>" />
    <input id="aerialtype" type="hidden" value="<%=((LUOBO.Entity.SYS_AP_VIEW)ViewData["APDEVICE"]).AERIALTYPE%>" />
    <input id="ssid_portal" type="hidden" />
    <div class="div-body" style="font-size:12px">
        <div id="tab1">
            <ul>
                <li><a href="#tab1-1">AP基本属性</a></li>
            </ul>
            <div id="tab1-1" style="height:234px">
                <table>
                    <tr>
                        <td>设备编号</td>
                        <td><input id="txtSerial" class="txt-apreg" type="text" value="<%=((LUOBO.Entity.SYS_AP_VIEW)ViewData["APDEVICE"]).SERIAL%>"/></td>
                        <td>设备型号</td>
                        <td><input id="txtModel" class="txt-apreg" type="text" value="<%=((LUOBO.Entity.SYS_AP_VIEW)ViewData["APDEVICE"]).MODEL%>"/></td>
                        <td>生产商</td>
                        <td><input id="txtManuf" class="txt-apreg" type="text" value="<%=((LUOBO.Entity.SYS_AP_VIEW)ViewData["APDEVICE"]).MANUFACTURER%>"/></td>
                    </tr>
                    <tr>
                        <td>购买人</td>
                        <td><input id="txtPurchaser" class="txt-apreg" type="text" value="<%=((LUOBO.Entity.SYS_AP_VIEW)ViewData["APDEVICE"]).PURCHASER%>"/></td>
                        <td>固件版本号</td>
                        <td><input id="txtFirmVersion" class="txt-apreg" type="text" value="<%=((LUOBO.Entity.SYS_AP_VIEW)ViewData["APDEVICE"]).FIRMWAREVERSION%>"/></td>
                        <td>最大SSID数</td>
                        <td><input id="txtMSSIDCount" class="txt-apreg" type="text" value="<%=((LUOBO.Entity.SYS_AP_VIEW)ViewData["APDEVICE"]).MAXSSIDNUM%>"/></td>
                    </tr>
                    <tr>
                        <td>是否支持3G</td>
                        <td>
                            <input type="radio" name="support3g" value="1" checked="checked" />是 
                            <input type="radio" name="support3g" value="0" />否
                        </td>
                        <td>AP配置模版</td>
                        <td><select id="sltAPCT" style="width:100px"></select></td>
                        <td>心跳间隔</td>
                        <td><input id="txtHBInterval" class="txt-apreg" type="text" value="<%=((LUOBO.Entity.SYS_AP_VIEW)ViewData["APDEVICE"]).HBINTERVAL%>" /></td>
                    </tr>
                    <tr>
                        <td>功率</td>
                        <td><input id="txtPOWER" class="txt-apreg" type="text" value="<%=((LUOBO.Entity.SYS_AP_VIEW)ViewData["APDEVICE"]).POWER%>"  /></td>
                        <td>信道</td>
                        <td><select id="sltCHANNEL" style="width:100px"></select></td>
                        <td>数据上报间隔</td>
                        <td><input id="txtDataInterval" class="txt-apreg" type="text" value="<%=((LUOBO.Entity.SYS_AP_VIEW)ViewData["APDEVICE"]).DATAINTERVAL%>"  /></td>
                    </tr>
                    <tr>
                        <td>是否开启SSID</td>
                        <td>
                            <input type="radio" name="isssidon" value="1" checked="checked" />是 
                            <input type="radio" name="isssidon" value="0" />否
                        </td>
                        <td>天线类型</td>
                        <td><select id="sltAERIALTYPE" style="width:100px"><option value="0">全向</option><option value="1">定向</option></select></td>
                        <td>远程重启</td>
                        <td><div id="reboot_div">
                        <%=(((bool)((LUOBO.Entity.SYS_AP_VIEW)ViewData["APDEVICE"]).ISREBOOT))? MvcHtmlString.Create("<span>正在重启</span>") : MvcHtmlString.Create("<input id=\"chkreboot\" class=\"txt-apreg\" type=\"checkbox\" />")%>
                        </div></td>
                    </tr>
                    <tr style="vertical-align:top">
                        <td><span style=" margin:2px">产品简要说明</span></td>
                        <td colspan="5"><textarea id="txtDescription" rows="5" cols="80" ><%=((LUOBO.Entity.SYS_AP_VIEW)ViewData["APDEVICE"]).DESCRIPTION%></textarea></td>
                        <td valign="bottom"><input id="btnSaveBase" type="button" value="保存" /></td>
                    </tr>

                </table>
            </div>
            <%--<div id="tab1-2" style="height:180px">
                <table>
                    
                    <tr>
                        <td>起始有效时间</td> 
                        <td><input id="txtSDate" class="Wdate" type="text" value="<%=((LUOBO.Entity.SYS_AP_VIEW)ViewData["APDEVICE"]).SDATE%>" onFocus="WdatePicker({isShowWeek:true,dateFmt:'yyyy/MM/dd'})"/></td>
                        <td>截至有效时间</td>
                        <td><input id="txtEDate" class="Wdate" type="text" value="<%=((LUOBO.Entity.SYS_AP_VIEW)ViewData["APDEVICE"]).EDATE%>" onFocus="WdatePicker({isShowWeek:true,dateFmt:'yyyy/MM/dd'})"/></td>
                        <td>远程重启</td>
                        <td><input id="chkreboot" class="txt-apreg" type="checkbox" /></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td><input id="btnSaveRemote" type="button" value="保存" /></td>
                    </tr>
                </table>
            </div>--%>
        </div>

        <div>
            <div style="float:left;width:170px">
                <div class="zTreeDemoBackground left">
                    <ul id="treeDemo" class="ztree" style="height:250px"></ul>
                </div>
                <input id="btn_submit" type="button" value="发布修改到设备" />
                <%--<input id="Button1" type="button" value="删除" />--%>
            </div>
            <div style="float:left;width:700px;margin-left:25px">
            <div id="tab2">
                <ul>
                    <li><a href="#tab2-1">基本属性</a></li>
                </ul>
                <div id="tab2-1">
                    <table>
                        <tr>
                            <td>是否启用</td>
                            <td><input id="ssid_ison" class="txt-apreg" type="checkbox" /></td>
                            <td>外网开关</td>
                            <td><input id="ssid_interneton" class="txt-apreg" type="checkbox" /></td>
                        </tr>
                        <tr>
                            <td>名称</td>
                            <td><input id="ssid_name" class="txt-apreg" type="text" /></td>
                            <td>最大连接数</td>
                            <td><input id="ssid_maxlinknum" class="txt-apreg" type="text" /></td>
                            <%--<td>入口地址</td>
                            <td><input id="ssid_portal" type="text" /></td>--%>
                           <%-- <td>电源开关</td>
                            <td><input id="ssid_power" class="txt-apreg" type="checkbox" /></td>--%>
                        </tr>
                        <tr>
                            <td>最大上行速率</td>
                            <td><input id="ssid_maxus" class="txt-apreg" type="text" /></td>
                            <td>访客最大上行速率</td>
                            <td><input id="ssid_vmaxus" class="txt-apreg" type="text" /></td>
                        </tr>
                        <tr>
                            <td>最大下行速率</td>
                            <td><input id="ssid_maxds" class="txt-apreg" type="text" /></td>
                            <td>访客最大下行速率</td>
                            <td><input id="ssid_vmaxds" class="txt-apreg" type="text" /></td>
                        </tr>
                        <tr>
                           
                            <td>访客上网时长</td>
                            <td><input id="ssid_vonlinetime" class="txt-apreg" type="text" /></td>
                        </tr>
                        <tr>
                            <td>禁止MAC</td>
                            <td colspan="4"><input id="ssid_banmac" type="text" /></td>
                        </tr>
                         <tr>
                            <td>禁止端口</td>
                            <td colspan="4"><input id="ssid_banport" type="text" /></td>
                        </tr>
                         <tr>
                            <td>禁止域名</td>
                            <td colspan="4"><input id="ssid_banurl" type="text" /></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td><input id="btn_ssid_save" type="button" value="提交" /></td>
                        </tr>
                    </table>
                    <div style="float:right"><input id="btn_back" type="button" value="返回" /></div>
                </div>
            </div>
            </div>
        </div>
    </div>
</body>
</html>
