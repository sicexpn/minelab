<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AP回收</title>
    <script src="../../Scripts/Common.js" type="text/javascript"></script>
    <script src="../../Scripts/json2.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.ztree.core-3.5.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.ui.core.js" type="text/javascript"></script>
	<script src="../../Scripts/jquery.ui.widget.js" type="text/javascript"></script>
	<script src="../../Scripts/jquery.ui.tabs.js" type="text/javascript"></script>
    <script src="../../Scripts/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <link href="../../Content/zTreeStyle/zTreeStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/themes/base/jquery.ui.all.css" rel="stylesheet" />
    <link href="../../Content/Site.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        var OID = 0;
        var data;

        var size = 10;
        var allPage = 0;
        var curPage = 1;
        var SSIDs;
        var jgID = 1;
        var benJGID = 1;

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
                        jgID = treeNode.id;
                        FindAPList(1);
                    }
                }
            }
        };
        $(document).ready(function () {
            benJGID = $('#benJGID').val();
            FindJGList();
            FindAPList(1);
        });


        function FindJGList() {
            $.ajax({
                type: 'post', //可选get
                url: 'FindJGList', //这里是接收数据的PHP程序
                data: 'jgID=' + OID,
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    zNodes.length = 0;
                    if (obj.length > 0) {
                        var item;
                        var data;
                        for (var i = 0; i < obj.length; i++) {
                            data = new Object();
                            if (obj[i].ID == OID)
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

        function FindAPList(_curPage) {
            curPage = _curPage;
            $.ajax({
                type: 'post',
                url: 'FindAPListForBack',
                //data: 'jgID=' + jgID + '&benJGID=' + benJGID + '&startSerial=' + startSerial + '&endSerial=' + endSerial + '&curPage=' + curPage + "&size=" + size,
                data: 'jgID=' + jgID + '&curPage=' + curPage + "&size=" + size,
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    $("#tbdAPList").empty();
                    $("#divPage").empty();
                    
                    if (obj.APList.length > 0) {
                        var item;
                        for (var i = 0; i < obj.APList.length; i++) {
                            item = obj.APList[i];

                            result = "<tr>";
                            result += "<td><input name='ckAP' type='checkbox' value='" + item.ID + "' /></td>";
                            result += "<td>" + item.SERIAL + "</td>";
                            result += "<td>" + item.MAC + "</td>";
                            result += "<td>" + item.SDATEStr + "</td>";
                            result += "<td>" + item.EDATEStr + "</td>";
                            
                            result += "<td>" + item.STATE + "</td>";
                            result += "</tr>";
                            $("#tbdAPList").append(result);
                        }
                        allPage = obj.AllCount % size == 0 ? obj.AllCount / size : parseInt(obj.AllCount / size) + 1;
                        ShowPage({ CurrentPage: curPage, MaxPageSize: allPage, PageShowSize: 5, IsUpDown: true, ShowElement: document.getElementById("divPage"), PageEvents: function (page) { FindAPList(page); } });
                    }
                }
            });
        }
        $(function () {
            $("#btn_back").click(function () { BackAP(); });
        })

        $(function () {
            $("#checkAll").click(function () {
                $('input[name="ckAP"]').attr("checked", this.checked);
            });
            var $subBox = $("input[name='ckAP']");
            $subBox.click(function () {
                $("#checkAll").attr("checked", $subBox.length == $("input[name='ckAP']:checked").length ? true : false);
            });
        });
        function BackAP() {
            var ids = "";
            $("input[name='ckAP']:checked").each(function () {
                if (ids != "")
                    ids += ",";
                ids += $(this).val();
            });
            if (ids == "") {
                alert("没有选择回收的数据!");
                return;
            }
            $.ajax({
                type: 'post', //可选get
                url: 'BackAP', //这里是接收数据的PHP程序
                data: 'jgID=' + benJGID + '&ids=' + ids, //传给PHP的数据，多个参数用&连接
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    if (obj) {
                        alert("回收成功!");
                        FindAPList(1);
                    }
                    else {
                        alert("回收失败!");
                        FindAPList(1);
                    }
                }
            });
        }
    </script>
</head>
<body>
    <input id="benJGID" type="hidden" value="<%=ViewData["OID"] %>" />
    <div class="div-body">
        <table>
            <tr>
                <td style="width: 170px">
                    <%--<input id="txtJGName" type="text" class="txt-normal" />--%>
                    <%--<input id="btnJGSearch" type="button" value="查询" />--%>
                    <div class="zTreeDemoBackground left">
                        <ul id="treeDemo" class="ztree"></ul>
                    </div>
                </td>

                <td style="vertical-align:top">
                    <%--<table>
                        <tr>
                            <td>设备编号</td>
                            <td><input id="startSerial" type="text" /> - <input id="endSerial" type="text" /></td>
                            <td>状态</td>
                            <td>
                                <select id="slt_state">
                                    <option>正常</option>
                                    <option>过期</option>
                                </select>
                            </td>
                            <td><input id="btn_findAP" type="button" value="查询" /></td>
                        </tr>
                    </table>--%>
                    <table class="hor-minimalist-a" >
                        <tr>
                            <th>选择</th>
                            <th>编号</th>
                            <th>MAC地址</th>
                            <th>起始日期</th>
                            <th>结束日期</th>
                            <th>状态</th>
                        </tr>
                        <tbody id="tbdAPList" ></tbody>
                        <tr><td><input id="checkAll" type="checkbox" value="全选"/>全选</td></tr>
                    </table>
                    <div id="divPage" class="hor-minimalist-page"></div>
                    <div style="float:right"><input id="btn_back" type="button" value="回收" /></div>
                </td>
            </tr>
            
        </table>
    </div>
</body>
</html>
