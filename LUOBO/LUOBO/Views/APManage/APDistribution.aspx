<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>APAllot</title>
    <script src="../../Scripts/Common.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.ztree.core-3.5.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.ui.core.js" type="text/javascript"></script>
	<script src="../../Scripts/jquery.ui.widget.js" type="text/javascript"></script>
	<script src="../../Scripts/jquery.ui.tabs.js" type="text/javascript"></script>
    <script src="../../Scripts/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../../Scripts/ajaxfileupload.js" type="text/javascript"></script>

    <link href="../../Content/zTreeStyle/zTreeStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/themes/base/jquery.ui.all.css" rel="stylesheet" />
    <link href="../../Content/Site.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        var POID = 0;
        var OID = 0;
        var apList = new Array();
        var allotAPList = new Array();
        var tmpAPList = new Array();

        $(document).ready(function () {
            POID = $("#hdPID").val();
            $("#divTab").tabs();
            FindJGList();
            GetAPList();
            GetSSIDTemplate();
            GetInvalidAPList();
            $("#btnSubmit").click(function () { AllotAP(); });
        });

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
                        GetAllotAPList(jgID);
                    }
                }
            },
            view: {
                addDiyDom: addDiyDom
            }
        };

        function GetSSIDTemplate() {
            
            $('#ssidTemplate').html();

            $.ajax({
                type: 'post',
                url: 'GetSSIDTemplate',
                data: 'OID=' + OID,
                dataType: 'json',
                success: function (obj) {
                    var html = "";
                    if (obj.ResultOBJ.length > 0) {

                        for (var i = 0; i < obj.ResultOBJ.length; i++) {
                            html += "<option value='" + obj.ResultOBJ[i].ID + "'>" + obj.ResultOBJ[i].NAME + "</option>"
                        }
                    } else {
                        html+= "<option value='-1'>暂无模版</option>"
                    }

                    $('#ssidTemplate').html(html);
                }
            });
        }

        function FindJGList() {
            $.ajax({
                type: 'post', //可选get
                url: 'FindJGList', //这里是接收数据的PHP程序
                data: 'jgID=' + POID,
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    zNodes.length = 0;
                    if (obj.length > 0) {
                        var item;
                        var data;
                        for (var i = 0; i < obj.length; i++) {
                            data = new Object();
                            if (obj[i].ID == POID)
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

        function GetAPList() {
            $("#tbdAPList").html("<tr><td colspan='2'>正在加载...</td></tr>");
            $.ajax({
                type: 'post', //可选get
                url: 'GetAPListByOID', //这里是接收数据的PHP程序
                data: 'OID=' + POID + "&isInvalid=false",
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    $("#tbdAPList").empty();
                    if (obj.ResultCode == 0) {
                        apList = obj.ResultOBJ;
                        $("#tbdAPList").append(GetAPHtml(obj.ResultOBJ, true));
                    }
                }
            });
        }

        function GetAllotAPList(id) {
            OID = id;
            $("#tbdAPAllot").html("<tr><td colspan='2'>正在加载...</td></tr>");
            $.ajax({
                type: 'post', //可选get
                url: 'GetAPListByOID', //这里是接收数据的PHP程序
                data: 'OID=' + id + "&isInvalid=false",
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    $("#tbdAPAllot").empty();
                    if (obj.ResultCode == 0) {
                        allotAPList = obj.ResultOBJ;
                        $("#tbdAPAllot").append(GetAPHtml(obj.ResultOBJ, false));
                    }
                }
            });
        }

        function GetInvalidAPList() {
            $("#tbdAPInvalid").html("<tr><td colspan='2'>正在加载...</td></tr>");
            $.ajax({
                type: 'post', //可选get
                url: 'GetAPListByOID', //这里是接收数据的PHP程序
                data: 'OID=' + POID + "&isInvalid=true",
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    $("#tbdAPInvalid").empty();
                    if (obj.ResultCode == 0) {
                        $("#tbdAPInvalid").append(GetAPHtml(obj.ResultOBJ, false));
                    }
                }
            });
        }

        function GetAPHtml(obj, isCK) {
            var result = "";
            for (var i = 0; i < obj.length; i++) {
                result += "<tr>";
                if (isCK)
                    result += "<td><input id='ck" + obj[i].ID + "' name='ckAPID' type='checkbox' value='" + obj[i].ID + "'></td>";
                result += "<td>" + obj[i].ALIAS + "</td>";
                result += "<td>" + obj[i].SERIAL + "</td>";
                result += "<td>" + obj[i].MAC + "</td>";
                result += "<td>" + obj[i].SSIDNUM + "</td>";
                result += "<td>" + obj[i].SDATEStr + "</td>";
                result += "<td>" + obj[i].EDATEStr + "</td>";
                if (obj[i].NAME != null)
                    result += "<td>" + obj[i].NAME + "</td>";
                result += "</tr>";
            }

            if (result == "")
                result = "<tr><td colspan='2'>没有数据!</td></tr>";
            return result;
        }

        function AllotAP() {
            var apidStr = "";
            $("input[name='ckAPID']:checked").each(function () { apidStr += "@" + $(this).val() + "@"; });
            
            if (CheckAllot(apidStr))
                return;

            var tmpAllotAPList = new Array();
            tmpAPList = new Array();
            for (var i = 0; i < apList.length; i++) {
                if (apidStr.indexOf("@" + apList[i].ID + "@") > -1) {
                    apList[i].SDATE = new Date($("#txtSDate").val().Trim());
                    apList[i].EDATE = new Date($("#txtEDate").val().Trim());
                    apList[i].SSIDNUM = parseInt($("#txtSSIDNum").val().Trim());
                    apList[i].POID = POID;
                    apList[i].OID = OID;
                    apList[i].APCTID = $('#ssidTemplate option:selected').val();
                    tmpAllotAPList.push(apList[i]);
                    allotAPList.push(apList[i]);
                }
                else {
                    tmpAPList.push(apList[i]);
                }
            }
            if (tmpAPList.length > 0)
                apList = tmpAPList;
            else
                apList = new Array();

            var str = String.toSerialize({ Items: tmpAllotAPList });
            $.ajax({
                type: 'post', //可选get
                url: 'AllotAP', //这里是接收数据的PHP程序
                data: str,
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    if (obj) {
                        $("#tbdAPList").empty();
                        $("#tbdAPList").append(GetAPHtml(apList, true));
                        $("#tbdAPAllot").empty();
                        $("#tbdAPAllot").append(GetAPHtml(allotAPList, false));
                        alert("分配成功!");
                    }
                    else {
                        alert("分配失败!");
                    }
                }
            });
        }

        function CheckAllot(allotAP) {
            if (OID == 0) {
                alert("请选择分配机构!");
                return true;
            }
            if ($("#txtSDate").val().Trim() == "") {
                alert("请选择起始时间!");
                return true;
            }
            if ($("#txtEDate").val().Trim() == "") {
                alert("请选择结束时间!");
                return true;
            }
            if (new Date($("#txtSDate").val().Trim()) > new Date($("#txtEDate").val().Trim())) {
                alert("起始日期必须小于结束日期!");
                return true;
            }
            if ($("#txtSSIDNum").val().Trim() == "") {
                alert("请输入SSID数量!");
                return true;
            }
            else if (isNaN($("#txtSSIDNum").val().Trim()) || parseInt($("#txtSSIDNum").val().Trim()) <= 0) {
                alert("输入的SSID数量必须为大于0的数字!");
                return true;
            }
            if ($("input[name='ckAPID']:checked").length == 0) {
                alert("请选择分配的AP!");
                return true;
            }
            var tmpSDATE = null;
            var tmpEDATE = null;
            var tmpSSIDNUM = null;
            for (var i = 0; i < apList.length; i++) {
                if (allotAP.indexOf("@" + apList[i].ID + "@") > -1) {
                    if (tmpSDATE == null || tmpSDATE < new Date(+/\d+/.exec(apList[i].SDATE)))
                        tmpSDATE = new Date(+/\d+/.exec(apList[i].SDATE));
                    if (tmpEDATE == null || tmpEDATE > new Date(+/\d+/.exec(apList[i].EDATE)))
                        tmpEDATE = new Date(+/\d+/.exec(apList[i].EDATE));
                    if (tmpSSIDNUM == null || tmpSSIDNUM > apList[i].SSIDNUM)
                        tmpSSIDNUM = apList[i].SSIDNUM;
                }
            }

            if (new Date($("#txtSDate").val().Trim()) < tmpSDATE) {
                alert("分配的起始时间不能小于" + tmpSDATE.format("yyyy-MM-dd"));
                return true;
            }
            if (new Date($("#txtEDate").val().Trim()) > tmpEDATE) {
                alert("分配的结束时间不能大于" + tmpEDATE.format("yyyy-MM-dd"));
                return true;
            }
            if (parseInt($("#txtSSIDNum").val().Trim()) > tmpSSIDNUM) {
                alert("分配的SSID数量不能大于" + tmpSSIDNUM);
                return true;
            }

            return false;
        }

        function addDiyDom(treeId, treeNode) {
            var aObj = $("#" + treeNode.tId + "_a");
            var editStr = "&nbsp;&nbsp;&nbsp;<span class='demoIcon' id='diyBtn_" + treeNode.id + "' title='" + treeNode.name + "' onfocus='this.blur();'>导入</span>";
            aObj.append(editStr);
            var btn = $("#diyBtn_" + treeNode.id);
            if (btn) btn.bind("click", function () {
                OID = treeNode.id;
                $("#btnImport").click();
            });
        }

        function ImportFile() {
            $.ajaxFileUpload({
                url: 'ImportAP',
                secureuri: false,
                fileElementId: 'btnImport',
                dataType: 'json',
                data: { OID: OID },
                success: function (obj, status) {
                    if (obj.ResultCode == 0) {
                        GetAPList();
                        GetAllotAPList(OID);
                    }
                    alert(obj.ResultMsg);
                },
                error: function (data, status, e) {
                    alert("提交失败");
                }
            });
        }
    </script>
</head>
<body>
    <input id="hdPID" type="hidden" value="<%=Request.Cookies["LUOBO"]["OID"] %>" />
    <input id="btnImport" type="file" name="fileToUpload" class="input" onchange="ImportFile()" style="display:none;"/>
    <div>
        <div style="float:left; width:192px">
            <input id="txtJGName" type="text" class="txt-normal" />
            <input id="btnJGSearch" type="button" value="查询" />
            <div class="zTreeDemoBackground left">
                <ul id="treeDemo" class="ztree"></ul>
            </div>
        </div>
        <div id="divTab" style="float:left;font-size:12px">
            <ul>
                <li><a href="#divTab-1">AP分配</a></li>
                <li><a href="#divTab-2">已失效</a></li>
            </ul>
            <div id="divTab-1">
                <div>
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>起始时间</td>
                            <td><input id="txtSDate" class="Wdate" type="text" onFocus="WdatePicker({isShowWeek:true,dateFmt:'yyyy/MM/dd',minDate:'2000-01-01 00:00:00'})"/></td>
                            <td>结束时间</td>
                            <td><input id="txtEDate" class="Wdate" type="text" onFocus="WdatePicker({isShowWeek:true,dateFmt:'yyyy/MM/dd',minDate:'2000-01-01 00:00:00'})"/></td>
                        </tr>
                        <tr>
                            <td>SSID数量</td>
                            <td><input id="txtSSIDNum" type="text" style="width:50px" /></td>
                            <td><select id="ssidTemplate"></select></td>
                            <td><input id="btnSubmit" type="button" value="分配" /></td>
                        </tr>
                    </table>
                    <table class="hor-minimalist-a">
                        <thead>
                            <tr>
                                <th scope="col">选择</th>
                                <th scope="col">设备名称</th>
                                <th scope="col">设备编号</th>
                                <th scope="col">MAC地址</th>
                                <th scope="col">最大SSID数</th>
                                <th scope="col">起始时间</th>
                                <th scope="col">结束时间</th>
                            </tr>
                        </thead>
                        <tbody id="tbdAPList">
                        </tbody>
                    </table>
                </div>
                <div>
                    <table class="hor-minimalist-a">
                        <thead>
                            <tr><td colspan="5">已分配</td></tr>
                            <tr>
                                <th scope="col">设备名称</th>
                                <th scope="col">设备编号</th>
                                <th scope="col">MAC地址</th>
                                <th scope="col">SSID数</th>
                                <th scope="col">起始时间</th>
                                <th scope="col">结束时间</th>
                            </tr>
                        </thead>
                        <tbody id="tbdAPAllot">
                        </tbody>
                    </table>
                </div>
            </div>
            <div id="divTab-2">
                <table class="hor-minimalist-a">
                    <thead>
                        <tr>
                            <th scope="col">设备编号</th>
                            <th scope="col">MAC地址</th>
                            <th scope="col">SSID数</th>
                            <th scope="col">起始时间</th>
                            <th scope="col">结束时间</th>
                            <th scope="col">机构名称</th>
                        </tr>
                    </thead>
                    <tbody id="tbdAPInvalid">
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</body>
</html>
