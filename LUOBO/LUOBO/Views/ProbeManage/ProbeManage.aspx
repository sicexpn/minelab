<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="../../Scripts/Common.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.ztree.core-3.5.min.js" type="text/javascript"></script>
    <script src="../../Scripts/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <link href="../../Content/Site.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/zTreeStyle/zTreeStyle.css" rel="stylesheet" type="text/css" />
    <title>AP管理</title>
    <script type="text/javascript">
        var size = 10;
        var allPage = 0;
        var curPage = 1;

        var g_curJG = -1;
        var jgList;

        $(function () {
            FindJGList();
            $("#btnJGSearch").click(function () { FindJGList(); });
            $("#btn_search").click(function () { FindAPList($('#txt_oid').val()); });
        })


        function FindAPList(_oid) {
            $("#tbdAPList").empty();
            $("#divPage").empty();
            $("#tbdAPList").html("<tr><td colspan='2'>正在加载...</td></tr>");
            var _benjgID = $('#txt_oid').val();
            var startSerial = $('#startSerial').val() == "" ? 0 : $('#startSerial').val();
            var endSerial = $('#endSerial').val() == "" ? 0 : $('#endSerial').val();
            var mac = $('#txtMac').val();
            var startDate = $('#startDate').val();
            var endDate = $('#endDate').val();
            var FPState = $('#sltFENP').val();
            $.ajax({
                type: 'post',
                url: 'FindProbeList',
                data: 'jgID=' + _oid + '&benJGID=' + _benjgID + '&startSerial=' + startSerial + '&endSerial=' + endSerial + '&mac=' + mac + '&startDate=' + startDate + '&endDate=' + endDate + '&FPState=' + FPState + '&curPage=' + curPage + "&size=" + size,
                dataType: 'json',
                success: function (obj) {
                    $("#tbdAPList").empty();
                    $("#divPage").empty();
                    if (obj.APList.length > 0) {
                        var item;
                        for (var i = 0; i < obj.APList.length; i++) {
                            item = obj.APList[i];
                            result = "<tr>";
                            result += "<td><input name='ckAP' type='checkbox' value='" + item.ID + "' /></td>";
                            result += "<td>" + item.ALIAS + "</td>";
                            result += "<td>" + item.SERIAL + "</td>";
                            result += "<td>" + item.MAC + "</td>";
                            result += "<td>" + item.MODEL + "</td>";
                            result += "<td>" + item.MAXSSIDNUM + "</td>";
                            result += "<td>" + item.FIRMWAREVERSION + "</td>";
                            result += "<td>" + (item.SUPPORT3G == true ? "支持" : "不支持") + "</td>";
                            result += "<td>" + (item.OID == _oid ? "未分配" : "分配至【" + getJGNameByOID(item.OID) + "】") + "</td>";
                            result += "<td><a href='APSetting?apid=" + item.ID + "'>配置</a> <a href='#'>重启</a></td>";
                            result += "</tr>";
                            $("#tbdAPList").append(result);
                        }
                        allPage = obj.AllCount % size == 0 ? obj.AllCount / size : parseInt(obj.AllCount / size) + 1;
                        ShowPage({ CurrentPage: curPage, MaxPageSize: allPage, PageShowSize: size, IsUpDown: true, ShowElement: document.getElementById("divPage"), PageEvents: function (page) { curPage = page; FindAPList(g_curJG); } });
                    } else {
                        $("#tbdAPList").html("<tr><td colspan='2'>没有数据!</td></tr>");
                    }
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

            if(confirm("确定要报废已选择的设备吗？"))
            {
                
            }
        }


        var zNodes = new Array();
        var setting = {
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
                url: 'FindJGList', //这里是接收数据的PHP程序
                data: 'jgID=' + $('#txt_oid').val(),
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    jgList = obj;
                    zNodes.length = 0;
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
                            zNodes.push(data);
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
                            zNodes.push(data);
                        }
                    }
                    $.fn.zTree.init($("#treeDemo"), setting, zNodes);
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
                <td style="width:170px">
                    <input id="txtJGName" type="text" class="txt-normal" />
                    <input id="btnJGSearch" type="button" value="查询" />
                    <div class="zTreeDemoBackground left">
                        <ul id="treeDemo" class="ztree"></ul>
                    </div>
                </td>
                <td valign="top">
                <div>
                    <table>
                        <tr>
                            <td>编号：</td>
                            <td><input id="startSerial" type="text" class="txt-normal"/></td>
                            <td>-</td>
                            <td><input id="endSerial" type="text" class="txt-normal"/></td>
                            <td>分配状态：</td>
                            <td><select id="sltFENP">
                                    <option value="-99">全部</option>
                                    <option value="1">已分配</option>
                                    <option value="0">未分配</option>
                                </select>
                            </td>
                            <td>注册日期：</td>
                            <td><input id="startDate" type="text" class="Wdate" onFocus="WdatePicker({isShowWeek:true})"/></td>
                            <td>-</td>
                            <td><input id="endDate" type="text" class="Wdate" onFocus="WdatePicker({isShowWeek:true})"/></td>
                            <td><input id="btn_search" type="button" value="查询" /></td>
                        </tr>
                        <tr>
                            <td>MAC: </td>
                            <td><input id="txtMac" type="text" class="txt-normal"/></td>
                            <td></td>
                            <td align="right" colspan="8">
                                <a href="APRegister">注册</a>
                                <a href="javascript:ScrapAP();">报废</a>
                            </td>
                        </tr>
                    </table>
                    <div class="div-body-bottom">
                        <table class="hor-minimalist-a">
                            <tr>
                                <th>选择</th>
                                <th>别名</th>
                                <th>编号</th>
                                <th>MAC地址</th>
                                <th>型号</th>
                                <th>最大SSID数</th>
                                <th>固件版本</th>
                                <th>是否支持3g</th>
                                <th>分配状态</th>
                                <th>操作</th>
                            </tr>
                            <tbody id="tbdAPList"></tbody>
                        </table>
                    </div>
                    <div id="divPage" class="hor-minimalist-page"></div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</body>
</html>
