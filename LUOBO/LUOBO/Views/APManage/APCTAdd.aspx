<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>AP配置模版新增</title>
    <script src="../../Scripts/Common.js" type="text/javascript"></script>
    <script src="../../Scripts/Layer/layer.min.js" type="text/javascript"></script>
    <link href="../../Content/Site.css" rel="stylesheet" type="text/css" />

    <!-- 弹出层 -->
    <script type="text/javascript">
        $(function () {
            layer.use('extend/layer.ext.js');
            //预览点击时事件
            $('#btnPreview').on('click', function () {

                var datas = TabDataProcess($('#txtTContent').val());
                layer.tab({
                    data: datas,
                    area: ['600px', '300px'] //宽度，高度
                });
            });

            $("#btnSave").click(function () { SaveAPCT(); });
        });

        function TabDataProcess(str) {
            var array = new Array();
            var obj;
            for (var i = 0; i < 4; i++) {
                obj = new Object();
                obj.title = i + "";
                obj.content = "这是第" + i + "个Tab";
                array.push(obj);
            }
            return array;
        }

        function SaveAPCT() {
            if ($("#txtTContent").val() == "") {
                layer.msg("请填写模版内容", 1, -1);
                return;
            }
            if ($("#txtTName").val() == "") {
                layer.msg("请填写模版名称", 1, -1);
                return;
            }
            if ($("#txtFirmName").val() == "") {
                layer.msg("请填写固件名称", 1, -1);
                return;
            }
            if ($("#txtFirmVersion").val() == "") {
                layer.msg("请填写固件版本号", 1, -1);
                return;
            }

            $.ajax({
                type: 'post', //可选get
                url: 'UpdateAPCT', //这里是接收数据的PHP程序
                data: { apctid: -1, tName: $('#txtTName').val().trim(), tContent: encodeURI($('#txtTContent').val().trim()), firmName: $('#txtFirmName').val().trim(), firmVersion: $('#txtFirmVersion').val().trim(), description: $('#txtDescription').val().trim() },
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    if (obj.ResultCode == 0)
                        layer.alert("添加成功");
                    else
                        layer.alert("操作失败,错误原因：" + obj.ResultMsg);
                }
            });
        }
           
    </script>
</head>
<body>
    <div class="div-body">
        <div style="float:left;">
            <table class="hor-minimalist-top">
                <tr>
                    <td>模版内容：</td>
                </tr>
                <tr>
                    <td><textarea id="txtTContent" rows="30" cols="70" ></textarea></td>
                </tr>
            </table>
            
        </div>

        <div style="float:left;">
            <table class="hor-minimalist-top">
                <tr>
                    <td scope="row">模版名称：</td>
                    <td><input id="txtTName" type="text" class="txt-normal" /></td>
                </tr>
                <tr>
                    <td scope="row">固件名称：</td>
                    <td><input id="txtFirmName" type="text" class="txt-normal"/></td>
                </tr>
                <tr>
                    <td scope="row">固件版本号：</td>
                    <td><input id="txtFirmVersion" type="text" class="txt-normal"/></td>
                </tr>
                <tr>
                    <td scope="row">模版描述：</td>
                    <td><textarea id="txtDescription" rows="5" cols="20"></textarea></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div style="border:1px solid #00F;height:300px;">
                            模版示例
                        </div>
                    </td>
                </tr>
                <tr>
                    <td><input id="btnPreview" type="button" value="预览" class="btn-normal"/></td>
                    <td><input id="btnSave" type="button"  value="保存" class="btn-normal"/></td>
                </tr>
            </table>
        </div>

    </div>
</body>
</html>
