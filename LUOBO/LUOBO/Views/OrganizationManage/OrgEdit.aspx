<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<LUOBO.Entity.SYS_ORGANIZATION>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {

    }
</script>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
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
                UpdateOrg();
            });
            //得到机构类别列表
            $("#slcOrgCategory").empty();
            $.ajax({
                type: 'post',
                url: '/OrganizationManage/GetOrgCategoryList',
                dataType: 'json',
                success: function (obj) {
                    for (var i = 0; i < obj.length; i++) {
                        $("#slcOrgCategory").append("<option value='" + obj[i].VALUE + "'>" + obj[i].NAME + "</option>");
                    }
                    $("#slcOrgCategory").val($("#hdOrgCategory").val());
                }
            });
            //得到省份
            $.ajax({
                type: 'post',
                url: '/OrganizationManage/GetAllProviceList',
                dataType: 'json',
                success: function (obj) {
                    for (var i = 0; i < obj.length; i++) {
                        $("#slcProvince").append("<option value='" + obj[i] + "'>" + obj[i] + "</option>");
                    }
                    $("#slcProvince").val($("#hdProvince").val());
                }
            });

            var provice = "";
            var city = "";
            $("#slcProvince").change(function () {
                provice = $(this).val();
                //alert(provice);
                //alert($("#hdCountry").val());
                $("#slcCity").empty();
                $("#slcCountry").empty();
                //得到城市列表
                $.ajax({
                    type: 'post',
                    url: '/OrganizationManage/GetCityList',
                    data: 'Provice=' + provice,
                    dataType: 'json',
                    success: function (obj) {
                        for (var i = 0; i < obj.length; i++) {
                            $("#slcCity").append("<option value='" + obj[i] + "'>" + obj[i] + "</option>");
                        }
                        $("#slcCity").val($("#hdCity").val());
                        //$("#slcCity option[text='ff']").attr("selected", true);
                    }
                });
            });
            //得到区县列表
            $("#slcCity").change(function () {
                city = $(this).val();
                $("#slcCountry").empty();
                $.ajax({
                    type: 'post',
                    url: 'GetCountryList',
                    data: 'Provice=' + provice + '&City=' + city,
                    dataType: 'json',
                    success: function (obj) {
                        for (var i = 0; i < obj.length; i++) {
                            $("#slcCountry").append("<option value='" + obj[i] + "'>" + obj[i] + "</option>");
                        }
                        //$("#slcCountry").val($("#hdCountry").val());
                    }
                });
            });
            $("#slcCountry").val($("#hdCountry").val());

            //            //得到城市列表
            //            $.ajax({
            //                type: 'post',
            //                url: '/OrganizationManage/GetCityList',
            //                dataType: 'json',
            //                success: function (obj) {
            //                    for (var i = 0; i < obj.length; i++) {
            //                        $("#slcCity").append("<option value='" + obj[i].NAME + "'>" + obj[i].NAME + "</option>");
            //                    }
            //                    $("#slcCity").val($("#hdCity").val());
            //                }
            //            });
            //            //得到区县列表
            //            $.ajax({
            //                type: 'post',
            //                url: 'GetCountryList',
            //                dataType: 'json',
            //                success: function (obj) {
            //                    for (var i = 0; i < obj.length; i++) {
            //                        $("#slcCountry").append("<option value='" + obj[i].NAME + "'>" + obj[i].NAME + "</option>");
            //                    }
            //                    $("#slcCountry").val($("#hdCountry").val());
            //                }
            //            });

        });
        var orgId = 0;
        var orgName = "";
        var orgDescription = "";
        var orgCategory = "";
        var orgContacter = "";
        var orgContactPhone = "";
        var orgProvince = "";
        var orgCity="";
        var orgCountry = "";
        function GetOrgInfo() {
            
            if ($("#orgName").val().Trim() == "") {
                alert("请输入机构名称!");
                return false;
            }
            else
                orgName = $("#orgName").val().Trim();
            if ($("#orgDescription").val().Trim() == "") {
                alert("请输入机构描述!");
                return false;
            }
            else
                orgDescription = $("#orgDescription").val().Trim();
            if ($("#slcOrgCategory").val() == "-99") {
                alert("请选择机构类别!");
                return false;
            }
            else
                orgCategory = $("#slcOrgCategory").val().Trim();
            if ($("#slcProvince").val().Trim() == "-99"&&$("#slcProvince").val().Trim() =="") {
                alert("请选择省份!");
                return false;
            }
            else
                orgProvince = $("#slcProvince").val().Trim();
            if ($("#slcCity").val().Trim() == "-99"&&$("#slcCity").val().Trim() =="") {
                alert("请选择城市!");
                return false;
            }
            else
                orgCity = $("#slcCity").val().Trim();
            if ($("#slcCountry").val().Trim() == "-99"&&$("#slcCountry").val().Trim() =="") {
                alert("请选择区县!");
                return false;
            }
            else
                orgCountry = $("#slcCountry").val().Trim();
            if ($("#orgContacter").val().Trim() == "") {
                alert("请输入机构联系人!");
                return false;
            }
            else
                orgContacter = $("#orgContacter").val().Trim();
            if ($("#orgContactPhone").val().Trim() == "") {
                alert("请输入机构联系电话!");
                return false;
            }
            else
                orgContactPhone = $("#orgContactPhone").val().Trim();
            
            return true;
        }
        function UpdateOrg() {

            if (!GetOrgInfo()) {
                return;
                }
            orgId = $("#hdID").val();
            $.ajax({
                type: 'post',
                url: '/OrganizationManage/UpdateOrg',
                data: 'ID=' + orgId +'&PROVINCE='+orgProvince+ '&NAME=' + orgName + '&DESCRIPTION=' + orgDescription + '&CATEGORY=' + orgCategory + '&CONTACTER=' + orgContacter + '&CONTACT=' + orgContactPhone + '&CITY=' + orgCity + '&COUNTIES=' + orgCountry,
                dataType: 'json',
                success: function (obj) {
                    if (obj)
                        alert("修改成功！");
                    else
                        alert("修改失败！");
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
            <div class="heading"><i class="fa fa-fw fa-list-ul"></i>机构编辑

            </div>

            <div class="widget-content padded clearfix">

        <table class="table table-bordered table-striped">
        <%--<table class="hor-minimalist-top">--%>

                    <tr>
                        <td style="width:120px;text-align:right">机构名称:</td>
                        <td><input type="text" id="orgName" value="<%:Model.NAME %>" style="width:80%"/></td><td style="color: #FF0000">*</td>
                    </tr>
                    <tr>
                        <td style="text-align:right">机构描述:</td>
                        <td><input type="text" id="orgDescription" value="<%:Model.DESCRIPTION %>" style="width:80%" /></td><td style="color: #FF0000">*</td>
                    </tr>
                    <tr>
                        <td style="text-align:right">机构类别:</td>
                        <td><select id="slcOrgCategory">
                                <option value=""><%:Model.CATEGORY %></option>
                            </select>
                            <input id="hdOrgCategory" type="hidden" value='<%:Model.CATEGORY %>' />
                        </td><td style="color: #FF0000">*</td>
                    </tr>
                    <tr >
                        <td style="text-align:right">省份:</td>
                        <td><select id="slcProvince">
                                <option value='<%:Model.PROVINCE %>'><%:Model.PROVINCE %></option>
                            </select>
                            <input id="hdProvince" type="hidden" value='<%:Model.PROVINCE %>' />
                        </td><td style="color: #FF0000">*</td>
                    </tr>
                    <tr>
                        <td style="text-align:right">城市:</td>
                        <td><select id="slcCity">
                                <option value='<%:Model.CITY %>'><%:Model.CITY %></option>
                            </select>
                            <input id="hdCity" type="hidden" value='<%:Model.CITY %>' />
                        </td><td style="color: #FF0000">*</td>
                    </tr>
                    <tr>
                        <td style="text-align:right">区县:</td>
                        <td><select id="slcCountry">
                                <option value='<%:Model.COUNTIES %>'><%:Model.COUNTIES %></option>
                            </select>
                            <input id="hdCountry" type="hidden" value='<%:Model.COUNTIES %>' />
                        </td><td style="color: #FF0000">*</td>
                    </tr>
                    <tr>
                        <td style="text-align:right">联系人:</td>
                        <td><input type="text" id="orgContacter" value="<%:Model.CONTACTER %>" style="width:80%"/></td><td style="color: #FF0000">*</td>
                    </tr>
                    <tr>
                        <td style="text-align:right">联系电话:</td>
                        <td><input type="text" id="orgContactPhone" value="<%:Model.CONTACT %>" style="width:80%"/> </td><td style="color: #FF0000">*</td>
                    </tr>
                                <tr>
                <td></td>
                <td>
             <input type="button" id="btnSubmit" value="确定" /><a href="OrgManage" ><input type="button"  value="返回" /></a></td>
            </tr>

        </table>
        </div>
    </div>

    </div>
        </div>
            </div>

</body>
</html>

