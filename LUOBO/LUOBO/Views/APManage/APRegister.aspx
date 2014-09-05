<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="../../Scripts/Common.js" type="text/javascript"></script>
    <script src="../../Scripts/ajaxfileupload.js" type="text/javascript"></script>
    <script src="../../Scripts/Layer/layer.min.js" type="text/javascript"></script>
    <link href="../../Content/Site.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/ajaxfileupload.css" rel="stylesheet" type="text/css" />
    <title>AP注册</title>
    <script type="text/javascript">
        $(function () {
            //获取模版列表
            $.ajax({
                type: 'post', //可选get
                url: 'GetAPCTListForSelect', //这里是接收数据的PHP程序
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    if (obj.ResultCode == 1) {
                        Alert(obj.ResultMsg);
                    }
                    else {
                        for (var i = 0; i < obj.ResultOBJ.length; i++) {
                            $("#sltAPCT").append("<option value='" + obj.ResultOBJ[i].ID + "'>" + obj.ResultOBJ[i].TNAME + "</option>");
                        }
                    }
                }
            });

            $('#btnEnter').click(function () { btnRegister() });
            $('#btnUpload').click(function () { ajaxFileUpload() });
            RadioCheck();
        });

        function btnRegister() {
            if (!CheckVal())
                return;

            var support3g = $("input[name='support3g']:checked").val();
            var isssidon = $("input[name='isssidon']:checked").val();
            if ($('#rdoOne').attr("checked") == "checked") {
                $.ajax({
                    type: 'post', //可选get
                    url: 'InsertAP', //这里是接收数据的PHP程序
                    data: 'alias=' + $('#txtAlias').val().trim() + '&mac=' + $('#txtMac').val().trim() + '&serial=' + $('#txtSerial').val().trim() + '&model=' + $('#txtModel').val().trim() + '&firmVersion=' + $('#txtFirmVersion').val().trim() + '&manuf=' + $('#txtManuf').val().trim() + '&maxssid=' + $('#txtMSSIDCount').val().trim() + '&purchaser=' + $('#txtPurchaser').val().trim() + '&apctID=' + $('#sltAPCT').val() + '&support3g=' + support3g + '&description=' + $('#txtDescription').val().trim() + '&channel=' + $("#sltCHANNEL").val() + '&power=' + $("#txtPOWER").val().trim() + '&aerialtype=' + $("#sltAERIALTYPE").val() + "&isssidon=" + isssidon,
                    dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                    success: function (obj) {
                        if (obj.ResultCode == 0) {
                            alert("注册成功");
                        }
                        else {
                            alert(obj.ResultMsg);
                        }
                    }
                });
            }else if ($('#rdoMost').attr("checked") == "checked") {
//                $("#loading")
//		        .ajaxStart(function () {
//		            $(this).show();
//		        })
//		        .ajaxComplete(function () {
//		            $(this).hide();
//		        });

            $.ajaxFileUpload({
                url: 'InsertAPs',
                secureuri: false,
                fileElementId: 'btnImport',
                dataType: 'json',
                data: { model: $('#txtModel').val().trim(), firmVersion: $('#txtFirmVersion').val().trim(), manuf: $('#txtManuf').val().trim(), maxssid: $('#txtMSSIDCount').val().trim(), purchaser: $('#txtPurchaser').val().trim(), apctID: $('#sltAPCT').val(), support3g: support3g, description: $('#txtDescription').val().trim() },
                success: function (obj, status) {
                    alert(obj.ResultMsg);
                },
                error: function (data, status, e) {
                    alert("提交失败");
                }
            });
            }
        }

        function CheckVal()
        {
            if (!$('#txtMac').is(':hidden')) {
                if ($('#txtMac').val().trim() == "") {
                    layer.alert("请输入MAC地址", -1);
                    //alert("请输入MAC地址");
                    return false;
                }

                //mac地址正则表达式
                var reg_mac = /[A-F\d]{2}-[A-F\d]{2}-[A-F\d]{2}-[A-F\d]{2}-[A-F\d]{2}-[A-F\d]{2}/;
                if (!reg_mac.test($('#txtMac').val().toUpperCase())) {
                    alert("mac地址格式不正确！mac地址格式为00-24-21-19-BD-E4");
                    $('#txtMac')[0].focus();
                    return false;
                }
            }

            if ($('#txtSerial').attr("disabled")==false) {
                if ($('#txtSerial').val().trim() == "") {
                    alert("请输入编号");
                    $('#txtSerial')[0].focus();
                    return false;
                }
            }

            //if ($('#txtModel').val().trim() == "") {
            //    alert("请输入型号");
            //    $('#txtModel')[0].focus();
            //    return false;
            //}

            if ($('#txtSerial').attr("disabled") == false) {
                if ($('#txtAlias').val().trim() == "") {
                    alert("请输入别名");
                    $('#txtAlias')[0].focus();
                    return false;
                }
            }

            if ($('#txtFirmVersion').val().trim() == "") {
                alert("请输入固件版本号");
                $('#txtFirmVersion')[0].focus();
                return false;
            }
            //if ($('#txtManuf').val().trim() == "") {
            //    alert("请输入生产商");
            //    $('#txtManuf')[0].focus();
            //    return false;
            //}
            
            if ($('#txtMSSIDCount').val().trim() == "") {
                alert("请输入最大SSID数");
                $('#txtMSSIDCount')[0].focus();
                return false;
            }
            var reg_num = /^[0-9]*[1-9][0-9]*$/;
            if (!reg_num.test($('#txtMSSIDCount').val())) {
                alert("输入的SSID最大数格式不对，请输入整数");
                $('#txtMSSIDCount')[0].focus();
                return false;
            }
            reg_num = /^(\d{1,2}|100)$/
            if (!reg_num.test($("#txtPOWER").val())) {
                alert("功率不能超过1-100之间，请重新输入");
                $('#txtPOWER')[0].focus();
                return false;
            }
            return true;
        }

        function RadioCheck() {
            $('#rdoOne').click(function () {
                if (this.checked) {
                    $('#txtMac').show();
                    $('#btnImport').hide();
                    $('#lblImportTips').hide();
                    //$('#txtModel').attr("disabled", false);
                    //$('#txtMostMac').attr("disabled", true);
                    $('#txtSerial').attr("disabled", false);
                    $('#txtAlias').attr("disabled", false);
                    $('#lblMac').text("MAC");
                }
            });
            $('#rdoMost').click(function () {
                if (this.checked) {
                    $('#txtMac').hide();
                    $('#btnImport').show();
                    $('#lblImportTips').show();
                    //$('#txtModel').attr("disabled", true);
                    //$('#txtMostMac').attr("disabled", false);
                    $('#txtAlias').attr("disabled", true);
                    $('#txtSerial').attr("disabled", true);
                    $('#lblMac').text("导入");
                }
            });
        }

    </script>
</head>
<body>
    <div class="div-body">
        <table>
            <tr>
                <td style="width:80%">
                    <table class="hor-minimalist-top">
                        <tr>
                            <td colspan="2">
                                <input id="rdoOne" type="radio" name="regCount" value="One" checked="checked" />单条  
                                <input id="rdoMost" type="radio" name="regCount" value="Most" />批量
                            </td>
                            <td><label id="lblMac">MAC</label></td>

                            <td>
                                <input id="txtMac" class="txt-apreg" type="text" />
                                <input id="btnImport" type="file" name="fileToUpload" class="input" style="display:none;"/>
                            </td>
                        </tr>
                        <tr>
                            <td>设备别名</td>
                            <td><input id="txtAlias" class="txt-apreg" type="text" /></td>
                            <td>设备编号</td>
                            <td><input id="txtSerial" class="txt-apreg" type="text" /></td>
                            
                        </tr>
                        <tr>
                            <td>生产商</td>
                            <td><input id="txtManuf" class="txt-apreg" type="text" /></td>
                            <td>固件版本号</td>
                            <td><input id="txtFirmVersion" class="txt-apreg" type="text" /></td>
                            
                        </tr>
                        <tr>
                            <td>购买人</td>
                            <td><input id="txtPurchaser" class="txt-apreg" type="text" /></td>
                            <td>AP配置模版</td>
                            <td><select id="sltAPCT" style="width:100px"></select></td>
                        </tr>
                        <tr>
                            <td>最大SSID数</td>
                            <td><input id="txtMSSIDCount" class="txt-apreg" type="text" /></td>
                            <td>信道</td>
                            <td>
                                <select id="sltCHANNEL" style="width:100px">
                                    <option value="1">1</option>
                                    <option value="2">2</option>
                                    <option value="3">3</option>
                                    <option value="4">4</option>
                                    <option value="5">5</option>
                                    <option value="6">6</option>
                                    <option value="7">7</option>
                                    <option value="8">8</option>
                                    <option value="9">9</option>
                                    <option value="10">10</option>
                                    <option value="11">11</option>
                                    <option value="12">12</option>
                                    <option value="13">13</option>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td>设备型号</td>
                            <td><input id="txtModel" class="txt-apreg" type="text" /></td>
                            <td>功率</td>
                            <td><input id="txtPOWER" class="txt-apreg" type="text" value="17" /></td>
                        </tr>
                        <tr>
                            <td>是否支持3G</td>
                            <td><input type="radio" name="support3g" value="true" checked="checked" />是 
                                <input type="radio" name="support3g" value="false" />否</td>
                            <td>天线类型</td>
                            <td><select id="sltAERIALTYPE" style="width:100px"><option value="0">全向</option><option value="1">定向</option></select></td>
                        </tr>
                        <tr>
                            <td>是否开启SSID</td>
                            <td><input type="radio" name="isssidon" value="true" checked="checked" />是 
                                <input type="radio" name="isssidon" value="false" />否</td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>产品简要说明</td>
                            <td colspan="3"><textarea id="txtDescription" rows="10" cols="50"></textarea></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td><input id="btnEnter" type="button" value="提交" /></td>
                            <td><input id="btnBack" type="button" value="返回" /></td>
                        </tr>
                    </table>
                    <div id="lblImportTips" style="display:none; float:left">
                        <p>目前支持txt导入。别名和经纬度可以省略不写</p>
                        <p>txt文本格式：MAC+Tab+编号+别名+Tab+纬度,经度+换行</p>
                        <p>AA-AA-AA-AA-AA-AA    10001   万虔云志总部   113.33111,22.23123</p>
                        <p>AA-AA-AA-AA-AA-AA    10002   万虔云志总部   113.33111,22.23123</p>
                        <p>AA-AA-AA-AA-AA-AA    10003   万虔云志总部   113.33111,22.23123</p>
                        <p>AA-AA-AA-AA-AA-AA    10004   万虔云志总部   113.33111,22.23123</p>
                    </div>

                </td>
                <%--<td style="width:20%">
                    <div style="float:right">
                       <img alt="loading" id="loading" src="/Content/image/loading.gif" style="display:none;" />
                        <input id="btnImport" type="file" name="fileToUpload" class="input" disabled="disabled"/>
                        <input id="btnUpload" type="button" class="button" value="上传" />
 `                    </div>
                    <div>
                        <textarea id="txtMostMac" rows="20" cols="30" disabled="disabled"></textarea>
                    </div>
                    <div>
                        <label id="lblInportCount" style="float:right">共导入XX条数据</label>
                    </div>
                </td>--%>
            </tr>
        </table>
    </div>
</body>
</html>
