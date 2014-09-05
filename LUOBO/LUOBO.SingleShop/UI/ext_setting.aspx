<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ext_setting.aspx.cs" Inherits="LUOBO.SingleShop.UI.ext_setting" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../UITemplet/css/bootstrap.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/css/font-awesome.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/css/style.css" media="all" rel="stylesheet" type="text/css" />
    <script src="../UITemplet/js/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../UITemplet/js/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script type="text/javascript" language="Javascript">
        function GetQueryString(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
            var r = decodeURIComponent(window.location.search).substr(1).match(reg);
            if (r != null) {
                return unescape(r[2]);
            }
            else {
                r = window.location.search.substr(1).match(reg);
                if (r != null) {
                    return unescape(r[2]);
                }
                else {
                    return null;
                }
            }
        }
        var token = GetQueryString("token");
        var userproperty = null;

        $(function () {
            showpage();
            getproperty();
        });

        function getproperty() {
            userproperty = null;
            $("#tab").empty();
            $("#center").empty();
            $.ajax({
                type: 'post',
                url: 'AjaxComm.aspx',
                data: 'type=GetExtProperty&token=' + token,
                dataType: 'json',
                error: function (msg) {
                    //alert("服务器错误");
                },
                success: function (obj) {
                    if (obj.ResultCode == 0) {
                        var tmpstr = "";
                        var tmphtml = "";
                        userproperty = obj.ResultOBJ;
                        for (var i = 0; i < obj.ResultOBJ.length; i++) {
                            tmpstr = "";
                            if (i == 0) {
                                tmpstr = " class='active'";
                            }
                            $("#tab").append("<li role='presentation'" + tmpstr + "><a href='#'>" + obj.ResultOBJ[i].PRO_NAME + "</a></li>");
                            tmphtml = "<div class='widget-content padded clearfix'><form><table class='hor-minimalist-top table table-striped'>";
                            for (var m = 0; m < obj.ResultOBJ[i].PRO_ITEM.length; m++) {
                                if (obj.ResultOBJ[i].PRO_ITEM[m].PropertyInfo.USER_CANVIEW == "1") {
                                    tmphtml += "<tr><td width='160px'>" + gethelp(obj.ResultOBJ[i].PRO_ITEM[m].PropertyInfo.PROP_HELP) + obj.ResultOBJ[i].PRO_ITEM[m].PropertyInfo.PROP_NAME + "</td><td>";
                                    if (obj.ResultOBJ[i].PRO_ITEM[m].PropertyInfo.USER_CANEDIT == "1") {
                                        tmphtml += getcombox(obj.ResultOBJ[i].PRO_ITEM[m].PropertyInfo.PROP_INPUTTYPE, obj.ResultOBJ[i].PRO_ITEM[m].PropertyInfo.PROP_DATA, "input_" + i + "_" + m, obj.ResultOBJ[i].PRO_ITEM[m].ProValue);
                                    } else {
                                        tmphtml += obj.ResultOBJ[i].PRO_ITEM[m].ProValue;
                                        tmphtml += "<input id='input_" + i + "_" + m + "' name='input_" + i + "_" + m + "' type='hidden' value='" + obj.ResultOBJ[i].PRO_ITEM[m].ProValue + "'/>";
                                    }
                                    tmphtml += "</td></tr>";
                                } else {
                                    tmphtml += "<input id='input_" + i + "_" + m + "' name='input_" + i + "_" + m + "' type='hidden' value='" + obj.ResultOBJ[i].PRO_ITEM[m].ProValue + "'/>";
                                }
                            }
                            tmphtml += "<tr><td colspan='2'><center><input type='button' class='go-ad-view btn btn-primary' onclick='javascript:savapro(" + i + ")' value='保存' /></center></td></tr>";
                            tmphtml += "</table></form></div>";
                            $("#center").append(tmphtml);
                        }
                        showpage();
                    } else {
                        alert(obj.ResultMsg);
                    }
                }
            });
        }

        function gethelp(helpstr) {
            var result = "";
            if (trim(helpstr).length > 0) {
                var d = trim(helpstr).toLowerCase();
                result = "<a href='#' onclick='javascript:alert(\"" + helpstr + "\");'>？</a> ";
                if (d.length > 7) {
                    d = d.substring(0, 7);
                    if (d == "http://") {
                        result = "<a href='" + helpstr + "' target='_blank'>？</a> ";
                    }
                }
            }
            return result;
        }

        function getcombox(mytype, valuedict, myname, myvalue) {
            var result = "";
            if (mytype == "2" && trim(valuedict).indexOf(":") > 0) {
                result = "<select id='" + myname + "' name='" + myname + "' class='form-control'>";
                var tmpkey, tmpvalue, tmpsel;
                var words = valuedict.split("||")
                for (var i = 0; i < words.length; ++i) {
                    tmpkey = "";
                    tmpvalue = "";
                    tmpsel = "";
                    if (words[i].indexOf(":") > 0) {
                        tmpkey = words[i].substring(0, words[i].indexOf(":"));
                        tmpvalue = words[i].substring(words[i].indexOf(":") + 1);
                        if (myvalue == tmpvalue) {
                            tmpsel = " selected='selected'";
                        }
                        result += "<option value='" + tmpvalue + "' " + tmpsel + ">" + tmpkey + "</option>";
                    }
                }
                result += "</select>";
            } else {
                result = "<input class='form-control' id='" + myname + "' name='" + myname + "' type='text' value='" + myvalue + "' />";
            }
            return result;
        }

        function trim(str) {
            if (str == null)
                return "";
            else
                return str.replace(/(^\s*)|(\s*$)/g, "");
        }

        function showpage() {
            $("#tab li").first().addClass("active").siblings().removeClass("active");
            $("#center > div").first().show().siblings().hide();
            $("#tab li").click(function () {
                var index = $("#tab li").index(this);
                $(this).addClass("active").siblings().removeClass("active");
                $("#center > div").eq(index).show().siblings().hide();
            });
        }

        function savapro(m) {
            var str = '';
            for (var i = 0; i < userproperty[m].PRO_ITEM.length; i++) {
                if (userproperty[m].PRO_ITEM[i].PropertyInfo.PROP_ISNULL == "0" && $("#input_" + m + "_" + i).val() == "") {
                    alert("请输入 " + userproperty[m].PRO_ITEM[i].PROP_NAME);
                    return;
                }
                if ($("#input_" + m + "_" + i).val() != "" && userproperty[m].PRO_ITEM[i].PropertyInfo.PROP_REGEX.length > 0) {
                    var patrn = userproperty[m].PRO_ITEM[i].PropertyInfo.PROP_REGEX;
                    var re = new RegExp(patrn);
                    if (!re.test($("#input_" + m + "_" + i).val())) {
                        alert(userproperty[m].PRO_ITEM[i].PropertyInfo.PROP_NAME + " " + userproperty[m].PRO_ITEM[i].PropertyInfo.PROP_COMMENT);
                        return;
                    }
                }
                if (i > 0) {
                    str += ',';
                }
                str += '{"ID":"' + userproperty[m].PRO_ITEM[i].ID + '","ProType":"' + userproperty[m].PRO_ITEM[i].PropertyInfo.PROP_TYPE + '","ProID":"' + userproperty[m].PRO_ITEM[i].PropertyInfo.PROP_ID + '","ProValue":"' + $("#input_" + m + "_" + i).val() + '"}';
            }
            if (str.length > 0) {
                str = '[' + str + ']';
            }
            $.ajax({
                type: 'post',
                url: 'AjaxComm.aspx',
                data: 'type=SaveExtProperty&token=' + token + '&param=' + str,
                dataType: 'json',
                error: function (msg) {
                    //alert("服务器错误");
                },
                success: function (obj) {
                    if (obj.ResultCode == 0) {
                        alert(obj.ResultMsg);
                        getproperty();
                    } else {
                        alert(obj.ResultMsg);
                    }
                }
            });
        }
    </script>
</head>
<body>
<div class="container-fluid">
<div class="row-fluid">
    <div class="div-body col-md-12">
        <div class="panel panel-default">
            <div class="panel-heading">
                <ul class="nav nav-tabs" role="tablist" id="tab" style="font-size:14px;font-weight:bold;">
                </ul>
            </div>
            <div id="center" class="panel-body">
            </div>
        </div>
    </div>
</div>
</div>
</body>
</html>
