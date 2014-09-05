<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>OrgAuthorize</title>
    <script src="../../Scripts/Common.js" type="text/javascript"></script>
    <link href="../../Content/lhbStyle/stylesheets/bootstrap.min.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../../Content/lhbStyle/stylesheets/font-awesome.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../../Content/lhbStyle/stylesheets/se7en-font.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../../Content/lhbStyle/stylesheets/style.css" media="all" rel="stylesheet" type="text/css" />
    <script src="../../Content/lhbStyle/javascripts/bootstrap.min.js" type="text/javascript"></script>
    <script src="../../Content/lhbStyle/javascripts/jquery.mousewheel.js" type="text/javascript"></script>
    <script src="../../Content/lhbStyle/javascripts/jquery.dataTables.min.js" type="text/javascript"></script>
<!--<script src="../../Content/lhbStyle/javascripts/bootstrap-editable.min.js" type="text/javascript"></script>
    <script src="../../Content/lhbStyle/javascripts/xeditable-demo.js" type="text/javascript"></script>-->
    <link rel="stylesheet" href="../../Scripts/jquery-ui.css">
    <script src="../../Scripts/jquery-1.10.2.js"></script>
    <script src="../../Scripts/jquery-ui.js"></script>
    <script type="text/javascript">
    //全选
        $(function () {
            $("#chAppUnAuthAll").click(function () {
                $('input[name="chAppUnAuth"]').attr("checked", this.checked);
            });
            var $subBox = $("input[name='chAppUnAuth']");
            $subBox.click(function () {
                $("#chAppUnAuthAll").attr("checked", $subBox.length == $("input[name='chAppUnAuth']:checked").length ? true : false);
            });
        });
        $(function () {
            $("#chAppAuthAll").click(function () {
                $('input[name="chAppAuth"]').attr("checked", this.checked);
            });
            var $subBox = $("input[name='chAppAuth']");
            $subBox.click(function () {
                $("#chAppAuthAll").attr("checked", $subBox.length == $("input[name='chAppAuth']:checked").length ? true : false);
            });
        });
        //授权
        $(function () {
            $("#btnUnAuth").click(function () {
                var ids = "";
                var orgId = $("#orgId").val();
                $("input[name='chAppAuth']:checked").each(function () {
                    if (ids != "")
                        ids += ",";
                    ids += $(this).val();
                });
                if (ids == "") {
                    alert("没有选择授权的应用!");
                    return;
                }
                $.ajax({
                    type: 'post',
                    url: '/OrganizationManage/UnAuthApp',
                    data: 'id=' + orgId + '&ids=' + ids,
                    dataType: 'json',
                    success: function (obj) {
                        var result = "";
                        var item;
                        $("#tbdAppAuthList").empty();
                        $("#tbdAppNoAuthList").empty();
                        for (var i = 0; i < obj.appsAuth.length; i++) {
                            item = obj.appsAuth[i];
                            result = "<tr>";
                            result += "<td><input name='chAppAuth' type='checkbox' value='" + item.ID + "' /></td>";
                            result += "<td>" + item.APPLICATIONNAME + "</td>";
                            result += "<td>" + "" + "</td>";
                            result += "</tr>";
                            $("#tbdAppAuthList").append(result);
                        }

                        for (var i = 0; i < obj.appsNoAuth.length; i++) {
                            item = obj.appsNoAuth[i];
                            result = "<tr>";
                            result += "<td><input name='chAppUnAuth' type='checkbox' value='" + item.ID + "' /></td>";
                            result += "<td>" + item.APPLICATIONNAME + "</td>";
                            result += "<td>" + "" + "</td>";
                            result += "</tr>";
                            $("#tbdAppNoAuthList").append(result);
                        }
                    }
                });
            });
            $("#btnAuth").click(function () {
                var ids = "";
                var orgId = $("#orgId").val();
                $("input[name='chAppUnAuth']:checked").each(function () {
                    if (ids != "")
                        ids += ",";
                    ids += $(this).val();
                });
                if (ids == "") {
                    alert("没有选择未授权的应用!");
                    return;
                }
                $.ajax({
                    type: 'post',
                    url: '/OrganizationManage/AuthApp',
                    data: 'id=' + orgId + '&ids=' + ids,
                    dataType: 'json',
                    success: function (obj) {
                        var result = "";
                        var item;
                        $("#tbdAppAuthList").empty();
                        $("#tbdAppNoAuthList").empty();
                        for (var i = 0; i < obj.appsAuth.length; i++) {
                            item = obj.appsAuth[i];
                            result = "<tr>";
                            result += "<td><input name='chAppAuth' type='checkbox' value='" + item.ID + "' /></td>";
                            result += "<td>" + item.APPLICATIONNAME + "</td>";
                            result += "<td>" + "" + "</td>";
                            result += "</tr>";
                            $("#tbdAppAuthList").append(result);
                        }

                        for (var i = 0; i < obj.appsNoAuth.length; i++) {
                            item = obj.appsNoAuth[i];
                            result = "<tr>";
                            result += "<td><input name='chAppUnAuth' type='checkbox' value='" + item.ID + "' /></td>";
                            result += "<td>" + item.APPLICATIONNAME + "</td>";
                            result += "<td>" + "" + "</td>";
                            result += "</tr>";
                            $("#tbdAppNoAuthList").append(result);
                        }
                    }
                });
            });
        });
    </script>
 <script type="text/javascript">
//            var result = "";
//            $.ajax({
//                type: 'post',
//                //url: 'GetAppsAuth',
//                dataType: 'json',
//                success: function (obj) {
//                    $("#tbdAppAuthList").empty();
//                    if (obj.length > 0) {
//                        var item;
//                        for (var i = 0; i < obj.length; i++) {
//                            item = obj[i];
//                            result = "<tr>";
//                            result += "<td><input name='ckAppAuth' type='checkbox' value='" + item.ID + "'</input></td>";
//                            result += "<td>" + item.APPLICATIONNAME + "</td>";
//                            result += "<td>" + "" + "</td>";
//                            result += "</tr>";
//                            $("#tbdAppAuthList").append(result);
//                        }
//                    }
//                }
//            });
//            $.ajax({
//                type: 'post',
//                //url: 'GetAppsNoAuth',
//                dataType: 'json',
//                success: function (obj) {
//                    $("#tbdAppAuthList").empty();
//                    if (obj.length > 0) {
//                        var item;
//                        for (var i = 0; i < obj.length; i++) {
//                            item = obj[i];
//                            result = "<tr>";
//                            result += "<td><input name='ckAppNoAuth' type='checkbox' value='" + item.ID + "'</input></td>";
//                            result += "<td>" + item.APPLICATIONNAME + "</td>";
//                            result += "<td>" + "" + "</td>";
//                            result += "</tr>";
//                            $("#tbdAppNoAuthList").append(result);
//                        }
//                    }
//                }
//            });
//        });
    </script>
</head>
<body>
    <input id="orgId" type="hidden" value='<%:ViewData["id"] %>' />
<div class="container-fluid">
      <div  class="row-fluid">
        <div class="div-body col-md-12">
        <div class="widget-container  clearfix">
            <div class="heading"><i class="fa fa-fw fa-list-ul"></i><%:ViewData["orgName"] %>机构

            </div>
            <div class="widget-content padded clearfix">
                            <div style="float:left; width:40%">
        <fieldset>
            <legend>
                已授权
            </legend>
 <table class="hor-minimalist-a  table table-bordered table-striped">
                <thead>
                    <tr>
                        <th style="width:70px" scope="col"><input id="chAppAuthAll" type="checkbox" /> 选择</th>
                        <th scope="col">应用名称</th>
                        <th scope="col">描述</th>
                    </tr>
                </thead>
                <tbody id="tbdAppAuthList">
                    <%foreach (var item in (List<LUOBO.Entity.SYS_APPLICATION>)ViewData["appsAuth"]) { %>
                    <tr>
                        <td><input name="chAppAuth" type="checkbox" value='<%:item.ID %>' /></td>
                        <td><%:item.APPLICATIONNAME %></td>
                        <td></td>
                    </tr>
                    <%} %>
                </tbody>
            </table>
       </fieldset>
    </div>
    <div style="float:left;width:20%; margin-top:2%;text-align:left " >
        <span style="display:block; text-align:center">
            <input type="button" id="btnUnAuth" value="→"/>
            <br />
            <br />
            <input style="margin-left:-0%" type="button" id="btnAuth" value="←"/>
            <br />
            <br />
            <a href="OrgManage"><input type="button" value="返回"/></a>
        </span>
    </div>
    
    <div style="float:left; width:40%">
        <fieldset>
            <legend>
                未授权
            </legend>
 <table class="hor-minimalist-a  table table-bordered table-striped">
            <thead>
                <tr>
                    <th style="width:70px" scope="col"><input id="chAppUnAuthAll" type="checkbox"/> 选择</th>
                    <th scope="col">应用名称</th>
                    <th scope="col">描述</th>
                </tr>
            </thead>
            <tbody id="tbdAppNoAuthList">
                    <%foreach (var item in ((List<LUOBO.Entity.SYS_APPLICATION>)ViewData["appsNoAuth"]))
                      { %>
                    <tr>
                        <td><input name="chAppUnAuth" type="checkbox" value='<%:item.ID %>' /></td>
                        <td><%:item.APPLICATIONNAME %></td>
                        <td></td>
                    </tr>
                    <%} %>
            </tbody>
        </table>
        </fieldset>
    </div>      
    
     



            </div>
            
            </div>
        </div>
        </div>
      

    </div>
    <div>

    </div>

</body>
</html>
