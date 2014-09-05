<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="alarmsite.aspx.cs" Inherits="LUOBO.SingleShop.UI.alarmsite" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>报警设置</title>
    <script src="../UITemplet/js/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../UITemplet/js/Common.js" type="text/javascript"></script>
    <script src="../UITemplet/js/json2.js" type="text/javascript"></script>

    <link href="../UITemplet/css/bootstrap.css" media="all" rel="stylesheet" type="text/css" />

    <style type="text/css">
        .form-group-margin-right
        {
            margin-right:-7px !important;
        }
    </style>    

    <script type="text/javascript">
        var token = GetQueryString("token");
        var data;
        $().ready(function () {
            InitContactList();

            $("#slcContact").change(function () {
                EnableAll();
                $("#btnSave").attr("disabled", false);
                for (var i = 0; i < data.length; i++) {
                    if (data[i].ID == $(this).val()) {
                        $("#txtNAME").val(data[i].NAME);
                        $("#txtCONTACT").val(data[i].CONTACT);
                        $("#txtEMAIL").val(data[i].EMAIL);
                        if (data[i].ISOWNORG == 0) {
                            DisabledAll();
                            $("#btnSave").attr("disabled", true);
                        }
                    }
                }
            });

            $("#btnSave").click(function () {
                var tmpData;
                for (var i = 0; i < data.length; i++) {
                    if (data[i].ID == $("#slcContact option:selected").val()) {
                        data[i].NAME = $("#txtNAME").val();
                        data[i].CONTACT = $("#txtCONTACT").val();
                        data[i].EMAIL = $("#txtEMAIL").val();
                        tmpData = data[i];
                        break;
                    }
                }
                $.ajax({
                    type: 'post',
                    url: 'AjaxComm.aspx',
                    data: 'type=SaveAPContact&token=' + token + '&param=' + JSON.stringify(tmpData),
                    dataType: 'json',
                    error: function (msg) {
                    },
                    success: function (obj) {
                        if (obj.ResultCode == "0") {
                            $("#slcContact option:selected").text(tmpData.NAME + "(" + tmpData.ONAME + ")");
                            Empty();
                            alert("编辑成功!");
                        }
                        else {
                            alert(obj.ResultMsg);
                        }
                    }
                });
            });

            $("#btnSubmit").click(function () {
                $.ajax({
                    type: 'post',
                    url: 'AjaxComm.aspx',
                    data: 'type=AddAPContact&token=' + token + '&param={"NAME":"' + $("#txtNAME").val() + '","CONTACT":"' + $("#txtCONTACT").val() + '","EMAIL":"' + $("#txtEMAIL").val() + '"}',
                    dataType: 'json',
                    error: function (msg) {
                    },
                    success: function (obj) {
                        if (obj.ResultCode == "0") {
                            InitContactList();
                            Empty();
                            alert("添加成功!");
                        }
                        else {
                            alert(obj.ResultMsg);
                        }
                    }
                });
            });

            $("#btnDel").click(function () {
                if (!confirm("确定要删除吗？"))
                    return;

                $.ajax({
                    type: 'post',
                    url: 'AjaxComm.aspx',
                    data: 'type=DelAPContact&token=' + token + "&param=" + $("#slcContact option:selected").val(),
                    dataType: 'json',
                    error: function (msg) {
                    },
                    success: function (obj) {
                        if (obj.ResultCode == "0") {
                            $("#slcContact option:selected").remove();
                            Empty();
                            alert("删除成功!");
                        }
                        else {
                            alert(obj.ResultMsg);
                        }
                    }
                });
            });
        });

        function InitContactList() {
            $("#slcContact").empty();
            $.ajax({
                type: 'post',
                url: 'AjaxComm.aspx',
                data: 'type=GetAPContactByOID&token=' + token,
                dataType: 'json',
                error: function (msg) {
                },
                success: function (obj) {
                    if (obj.ResultCode == "0") {
                        data = obj.ResultOBJ;
                        for (var i = 0; i < obj.ResultOBJ.length; i++) {
                            $("#slcContact").append("<option value='" + obj.ResultOBJ[i].ID + "'>" + obj.ResultOBJ[i].NAME + "(" + obj.ResultOBJ[i].ONAME + ")</option>");
                        }
                    }
                    else {
                        alert(obj.ResultMsg);
                    }
                }
            });
        }

        function DisabledAll() {
            $("input").attr("disabled", true);
            $(".radio-inline").attr("disabled", true);
        }

        function EnableAll() {
            $("input").attr("disabled", false);
            $(".radio-inline").attr("disabled", false);
        }

        function Empty() {
            $("input[type='text']").val("");
            $("#slcContact").val("");
            $("#btnSave").attr("disabled", true);
        }
    </script>
</head>
<body>
    <div class="container-fluid">
        <div class="row-fluid">
            <div style="margin-top:20px">
                <div class="div-body col-md-6">
                    <select id="slcContact" size="5" class="form-control">
                    </select>
                </div>
                <div class="div-body col-md-6 form-horizontal">
                    <div style="padding:7px 0px 7px 7px">
                        <div class="form-group form-group-margin-right">
                            <label class="col-sm-2 control-label">名称</label>
                            <div class="col-sm-10">
                              <input id="txtNAME" class="form-control input-sm" type="text" placeholder="请输入名称" />
                            </div>
                        </div>
                        <div class="form-group form-group-margin-right">
                            <label class="col-sm-2 control-label">联系电话</label>
                            <div class="col-sm-10">
                              <input id="txtCONTACT" class="form-control input-sm" type="text" placeholder="请输入联系电话" />
                            </div>
                        </div>
                        <div class="form-group form-group-margin-right">
                            <label class="col-sm-2 control-label">电子邮件</label>
                            <div class="col-sm-10">
                              <input id="txtEMAIL" class="form-control input-sm" type="text" placeholder="请输入电子邮件" />
                            </div>
                        </div>
                        <%--<div class="form-group form-group-margin-right">
                            <div class="col-sm-offset-2 col-sm-10">
                                <label class="radio-inline"><input name="rdNOTICETYPE" type="radio" value="1" /> 短信通知</label>
                                <label class="radio-inline"><input name="rdNOTICETYPE" type="radio" value="2" /> 邮件通知</label>
                            </div>
                        </div>--%>
                        <div class="form-group form-group-margin-right">
                            <div class="col-sm-offset-2 col-sm-10">
                                <input id="btnSave" class="btn btn-primary btn-sm" type="button" value="保存" disabled />
                                <input id="btnSubmit" class="btn btn-success btn-sm" type="button" value="添加" />
                                <input id="btnDel" class="btn btn-danger btn-sm" type="button" value="删除" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
