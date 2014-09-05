<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<LUOBO.Entity.SYS_ORGANIZATION>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {

    }
</script>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>OrgEdit</title>
    <script src="../../Scripts/Common.js" type="text/javascript"></script>
    <link href="../../Content/Site.css" rel="stylesheet" type="text/css" />
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
        $(function () {
            $("#btnSubmit").click(function () {
                UpdateOrgProp();
            });
            //得到机构扩展属性列表
            $("#OrgProp").empty();
            $("#slcPtype").empty();
            $.ajax({
                type: 'post',
                url: '/OrganizationManage/GetOrgPropList',
                dataType: 'json',
                success: function (obj) {
                    for (var i = 0; i < obj.length; i++) {
                        $("#slcPtype").append("<option value='" + obj[i].NAME + "'>" + obj[i].NAME + "</option>");
                    }
                    /*var str = "<ul id='menu'>";
                    for (var i = 0; i < obj.length; i++) {
                        var row = "<li><input type='hidden' value='" + obj[i].NAME + "'/>" + obj[i].NAME + "   Name:<input type='text' value=''/> Value:<input type='text' value=''/></li>";
                        str += row;

                    }
                    str += "</ul>";
                    $("#orgProp").append(str);*/
                }
            });
            freshTable();

        });
        function freshTable() {
            $("#tbdOrgPropList").empty();
            $.ajax({
                type: 'post',
                url: '/OrganizationManage/GetOrgProps',
                dataType: 'json',
                data: "oid=" + $("#hdID").val(),
                success: function (obj) {
                    for (var i = 0; i < obj.length; i++) {
                        var item = obj[i];
                        var row = "<tr><td>" + item.PTYPE + "</td><td>" + item.PNAME + "</td><td>" + item.PVALUE + "</td></tr>";
                        //var row = "<li><input type='hidden' value='" + obj[i].NAME + "'/>" + obj[i].NAME + "   Name:<input type='text' value=''/> Value:<input type='text' value=''/></li>";
                        $("#tbdOrgPropList").append(row);
                    }
                }
            });
        }
        var orgId = 0;
        var pType = 0;
        var pName = 0;
        var pValue = 0;
        function checkInput() {
            if ($("#slcPtype").val() == "") {
                alert("请选择属性类别");
                return false;
            }
            if ($("#pName").val().trim() == "") {
                alert("请选择属性名字");
                return false;
            }
            return true;
        }
        function UpdateOrgProp() {
            
            if (!checkInput()) {
                return;
            }
            orgId = $("#hdID").val();
            /*var data = $("#orgProp ul li input");
            var rows = 3;
            var len = data.length / rows;
            var tmpPropList = new Array();
            for (var i = 0; i < len; i++) {
                var prop = Object();
                prop.OID = orgId;
                prop.PTYPE = $(data[3 * i]).val();
                prop.PNAME = $(data[1 + 3 * i]).val();
                prop.PVALUE = $(data[2 + 3 * i]).val();

                tmpPropList.push(prop);
            }
            var orgPropstr = String.toSerialize({ Items: tmpPropList });
            */
            /*var jsonData = "";
            for (var i = 0; i < len; i++) {
                var jsonRow = "{\"OID\":" + orgId + ",\"PTYPE\":\"" + $(data[ 3 * i]).val() + "\",\"PNAME\":\"" + $(data[ 1 + 3 * i]).val() + "\",\"PVALUE\":\"" + $(data[ 2 + 3 * i]).val() + "\"}";
                //alert(jsonRow);
                if (i == 0) {
                    jsonData += "{\"OrgProps\":[";
                }
                if (i == len - 1) {
                    jsonData += jsonRow+"]}";
                } else {
                    jsonData += jsonRow + ",";
                }
            }
            */
            //alert('OID=' + orgId + '&PTYPE=' + pType + '&PNAME=' + pName + '&PVALUE=' + pValue);
            //alert(jsonData);
            //var postData = jQuery.parseJSON(jsonData);
            //alert(jQuery.parseJSON(jsonData).OrgProps[0].OID);
            //alert(JSON.stringify(jsonData).OrgProps[0].OID);

            $.ajax({
                type: 'post',
                url: '/OrganizationManage/UpdateOrgProp',
                data: { OID: orgId, PTYPE: $("#slcPtype").val(), PNAME: $("#pName").val().trim(), PVALUE: $("#pValue").val().trim() },

                dataType: 'json',
                success: function (obj) {
                    if (obj) {
                        alert("配置成功！");
                        freshTable();
                    }
                    else
                        alert("配置失败！");
                }
            });
            
        }
    </script>
</head>
<body>
 <input id="hdID" type="hidden" value="<%:Model.ID %>"/>
<div class="container-fluid">
      <div  class="row-fluid">
        <div class="div-body col-md-12">
        <div class="widget-container  clearfix">
            <div class="heading"><i class="fa fa-fw fa-list-ul"></i><%:Model.NAME %>:机构扩展属性配置

            </div>
            <table class="hor-minimalist-a  table table-bordered table-striped">
            <thead>
            
                <tr>
                    <th scope="col">扩展类型</th>
                    <th scope="col">属性名字</th>
                    <th scope="col">属性值</th>
                </tr>
            </thead>
            <tbody id="tbdOrgPropList">
                
            </tbody>
            </table>
            <div class="widget-content padded clearfix">
            <div id="orgProp">
                扩展类型:<select id="slcPtype">
                    
                </select>
                属性名字:<input id="pName" type="text" />
                属性值:<input id="pValue" type="text" />
            </div>
              <br />
             <input type="button" id="btnSubmit" value="增加/修改" /><a href="OrgPropExpend" ><input type="button"  value="返回" /></a>

        
        </div>
    </div>

    </div>
        </div>
            </div>

</body>
</html>

