<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <script src="../../Scripts/Common.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.ztree.core-3.5.min.js" type="text/javascript"></script>
    <script src="../../Scripts/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <link href="../../Content/Site.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/zTreeStyle/zTreeStyle.css" rel="stylesheet" type="text/css" />
    <title>SSID管理</title>
    <script type="text/javascript">
        var size =6;
        var allPage = 0;
        var curPage = 1;
        var g_ap = -1;
        var g_curJG = -1;
        var jgList;
        var SSIDs;

        $(function () {
            FindJGList();
            $("#btnJGSearch").click(function () { FindJGList(); });
            $("#btn_search").click(function () { FindAPList($('#txt_oid').val()); });
            $('#btn_ssid_save').click(function () { SaveSSID(); });
        })

        function FindAPList(_oid) {
            $("#tbdAPList").empty();
            $("#divPage").empty();
            $("#tbdAPList").html("<tr><td colspan='2'>正在加载...</td></tr>");
            var _benjgID = $('#txt_oid').val();
            $.ajax({
                type: 'post',
                url: '/APManage/FindAPList',
                data: 'jgID=' + _oid + '&benJGID=' + _benjgID + '&curPage=' + curPage + "&size=" + size,
                dataType: 'json',
                success: function (obj) {
                    $("#tbdAPList").empty();
                    $("#divPage").empty();
                    if (obj.APList.length > 0) {
                        var item;
                        for (var i = 0; i < obj.APList.length; i++) {
                            item = obj.APList[i];
                            result = "<tr>";
                            result += "<td><input name='rdo_" + _oid + "' type='radio' value='" + item.ID + "' /></td>";
                            result += "<td>" + item.ALIAS + "</td>";
                            result += "<td>" + item.SERIAL + "</td>";
                            result += "<td>" + item.MAC + "</td>";
                            result += "<td>" + item.MODEL + "</td>";
                            result += "<td>" + item.MAXSSIDNUM + "</td>";
                            result += "<td>" + item.FIRMWAREVERSION + "</td>";
                            result += "<td>" + (item.SUPPORT3G == true ? "支持" : "不支持") + "</td>";
                            result += "<td name='oid' oid='" + item.OID + "'>" + (item.OID == _oid ? "本机构" : getJGNameByOID(item.OID)) + "</td>";
                            result += "</tr>";
                            $("#tbdAPList").append(result);
                        }
                        allPage = obj.AllCount % size == 0 ? obj.AllCount / size : parseInt(obj.AllCount / size) + 1;
                        ShowPage({ CurrentPage: curPage, MaxPageSize: allPage, PageShowSize: size, IsUpDown: true, ShowElement: document.getElementById("divPage"), PageEvents: function (page) { curPage = page; FindAPList(g_curJG); } });
                    } else {
                        $("#tbdAPList").html("<tr><td colspan='2'>没有数据!</td></tr>");
                    }

                    var tbList = $(".apList");
                    tbList.each(function () {
                        var self = this;

                        $("tr", $(self)).click(function () {
                            var trThis = this;
                            if ($(trThis).get(0) == $("tr:first", $(self)).get(0)) {
                                return;
                            }
                            var rdo = $(trThis).find("[type='radio']");
                            $(rdo).attr("checked", true);
                            g_ap = $(rdo).val();
                            FindSSIDList($(trThis).find("[name='oid']").attr("oid"), g_ap);
                        });
                    });

                }
            });
        }

        function getJGNameByOID(_oid) {
            for (var i = 0; i < jgList.length; i++) {
                if (jgList[i].ID == _oid)
                    return jgList[i].NAME;
            }
        }

        function ScrapAP() {
            var ids = "";
            $("input[name='ckAP']:checked").each(function () {
                if (ids != "")
                    ids += ",";
                ids += $(this).val();
            });

            if (ids == "") {
                alert("没有选择要报废的设备");
                return;
            }

            if (confirm("确定要报废已选择的设备吗？")) {

            }
        }


        var zNodesOrg = new Array();
        var settingOrg = {
            data: {
                simpleData: {
                    enable: true,
                    idKey: "id",
                    pIdKey: "pId",
                    rootPId: ""
                }
            },
            callback: {
                onMouseUp: function (event, treeId, treeNode) {
                    if (treeNode != null) {
                        curPage = 1;
                        g_curJG = treeNode.id;
                        FindAPList(treeNode.id);
                    }
                }
            }
        };

        function FindJGList() {
            $.ajax({
                type: 'post', //可选get
                url: '/APManage/FindJGList', //这里是接收数据的PHP程序
                data: 'jgID=' + $('#txt_oid').val(),
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    jgList = obj;
                    zNodesOrg.length = 0;
                    if (obj.length > 0) {
                        var item;
                        var data;
                        if ($('#txt_oid').val() == "0") {
                            data = new Object();
                            data.id = 0;
                            data.pId = -1;
                            data.name = "总公司";
                            data.open = true;
                            data.selected = true;
                            zNodesOrg.push(data);
                        }
                        for (var i = 0; i < obj.length; i++) {
                            data = new Object();
                            if (obj[i].ID == $('#txt_oid').val())
                                data.open = true;
                            else
                                data.open = false;
                            data.id = obj[i].ID;
                            data.pId = obj[i].PID;
                            data.name = obj[i].NAME;
                            zNodesOrg.push(data);
                        }
                    }
                    $.fn.zTree.init($("#orgTree"), settingOrg, zNodesOrg);
                }
            });
        }



        var zNodesSSID = new Array();
        var settingSSID = {
            data: {
                simpleData: {
                    enable: true,
                    idKey: "id",
                    pIdKey: "pId",
                    rootPId: ""
                }
            },
            callback: {
                onClick: SSIDonClick
            }
        };

        function SSIDonClick(event, treeId, treeNode, clickFlag) {
            if (treeNode.level > 0) {
                for (var i = 0; i < SSIDs.length; i++) {
                    if (SSIDs[i].ID == treeNode.id) {
                        g_cur_ssid_idx = i;
                        $('#ssid_ison').attr("checked", SSIDs[i].ISON);
                        $('#ssid_interneton').attr("checked", SSIDs[i].ISINTERNET);
                        $('#ssid_name').val(SSIDs[i].NAME);
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

        function FindSSIDList(_oid, _apid) {
            zNodesSSID.length = 0;
            $.fn.zTree.init($("#ssidTree"), settingSSID, zNodesSSID);
            $.ajax({
                type: 'post', //可选get
                url: '/APManage/FindSSIDListByAPID', //这里是接收数据的PHP程序
                data: 'apid=' + _apid + '&oid=' + _oid,
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    zNodesSSID.length = 0;
                    var data;
                    data = new Object();
                    data.id = _apid;
                    data.pId = -1;
                    data.open = true;
                    data.name = _apid + "设备";
                    zNodesSSID.push(data);
                    SSIDs = obj;

                    if (obj.length > 0) {
                        for (var i = 0; i < obj.length; i++) {
                            obj[i].NAME = obj[i].NAME.trim();
                            data = new Object();
                            data.id = obj[i].ID;
                            data.pId = _apid;
                            data.name = obj[i].NAME;
                            zNodesSSID.push(data);
                        }
                    } else {
                        data = new Object();
                        data.id = 99999;
                        data.pId = _apid;
                        data.name = "无任何SSID";
                        zNodesSSID.push(data);
                    }
                    $.fn.zTree.init($("#ssidTree"), settingSSID, zNodesSSID);
                }
            });
        }

        function SaveSSID() {

            SSIDs[g_cur_ssid_idx].ISON = ($('#ssid_ison').attr("checked") == "checked");
            SSIDs[g_cur_ssid_idx].ISINTERNET = $('#ssid_interneton').attr("checked") == "checked";
            SSIDs[g_cur_ssid_idx].NAME = $('#ssid_name').val();
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

            $.ajax({
                type: 'post',
                url: '/APManage/SaveSSID',
                data: (SSIDs[g_cur_ssid_idx]),
                dataType: 'json',
                success: function (obj) {
                    if (obj.ResultCode == 0) {
                        alert("保存成功");
                        zNodesSSID.length = 0;
                        var data;
                        data = new Object();
                        data.id = g_ap;
                        data.pId = -1;
                        data.open = true;
                        data.name = g_ap + "设备";
                        zNodesSSID.push(data);

                        if (SSIDs.length > 0) {
                            for (var i = 0; i < SSIDs.length; i++) {
                                data = new Object();
                                data.id = SSIDs[i].ID;
                                data.pId = g_ap;
                                data.name = SSIDs[i].NAME;
                                zNodesSSID.push(data);
                            }
                        } else {
                            data = new Object();
                            data.id = 99999;
                            data.pId = g_ap;
                            data.name = "无任何SSID";
                            zNodesSSID.push(data);
                        }
                        $.fn.zTree.init($("#ssidTree"), settingSSID, zNodesSSID);
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
    <input id="txt_oid" type="hidden" value="<%=ViewData["OID"] %>" />
    <div style="width:100%" >
        <table>
            <tr>
                <td valign="top" style="width:170px">
                    <input id="txtJGName" type="text" class="txt-normal" />
                    <input id="btnJGSearch" type="button" value="查询" />
                    <div class="zTreeDemoBackground left" >
                        <ul id="orgTree" class="ztree" style="height:500px"></ul>
                    </div>
                </td>
                <td valign="top">
                <div>
                    <div class="div-body-bottom" style="height:250px">
                        <table class="hor-minimalist-a apList">
                            <tr>
                                <th>选择</th>
                                <th>别名</th>
                                <th>编号</th>
                                <th>MAC地址</th>
                                <th>型号</th>
                                <th>最大SSID数</th>
                                <th>固件版本</th>
                                <th>是否支持3g</th>
                                <th>所在位置</th>
                            </tr>
                            <tbody id="tbdAPList"></tbody>
                        </table>
                        <div id="divPage" class="hor-minimalist-page"></div>
                    </div>
                    

                    <div>
                        <div class="zTreeDemoBackground left">
                            <ul id="ssidTree" class="ztree" style="height:250px"></ul>
                        </div>

                        <div class="left">
                        
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
                                <td>入口地址</td>
                                <td><input id="ssid_portal" type="text" /></td>
                                <td style="display:none">电源开关</td>
                                <td style="display:none"><input id="ssid_power" class="txt-apreg" type="checkbox" /></td>
                            </tr>
                            <tr>
                                <td>最大连接数</td>
                                <td><input id="ssid_maxlinknum" class="txt-apreg" type="text" /></td>
                                <td>访客最大上行速率</td>
                                <td><input id="ssid_vmaxus" class="txt-apreg" type="text" /></td>
                            </tr>
                            <tr>
                                <td>最大上行速率</td>
                                <td><input id="ssid_maxus" class="txt-apreg" type="text" /></td>
                                <td>访客最大下行速率</td>
                                <td><input id="ssid_vmaxds" class="txt-apreg" type="text" /></td>
                            </tr>
                            <tr>
                                <td>最大下行速率</td>
                                <td><input id="ssid_maxds" class="txt-apreg" type="text" /></td>
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

                        </div>
                    </div>
                </div>
                </td>
            </tr>
        </table>
    </div>
</body>
</html>