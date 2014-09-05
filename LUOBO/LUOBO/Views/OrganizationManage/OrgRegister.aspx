<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>OrgRegister</title>
    <script src="../../Scripts/Common.js" type="text/javascript"></script>
    <link href="../../Content/Site.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        var orgId = 0;
        var orgName = "";
        var orgDescription = "";
        var orgCategory = -99;
        var orgProvince = "";
        var orgCity = -99;
        var orgCountry = -99;
        var orgContacter = "";
        var orgContactPhone = "";
        var orgIsverify = false;
        var orgIsverify_end = false;
        var provice = "";
        var city = "";
        var AddOrg = function () {
            if (!GetOrgInfo())
                return;
            $.ajax({
                type: 'post',
                url: 'AddOrg',
                data: 'PROVINCE='+orgProvince+'&NAME=' + orgName + '&DESCRIPTION=' + orgDescription + '&CATEGORY=' + orgCategory + '&CITY=' + orgCity + '&COUNTIES=' + orgCountry + '&CONTACTER=' + orgContacter + '&CONTACT=' + orgContactPhone + '&ISVERIFY=' + orgIsverify + '&ISVERIFY_END=' + orgIsverify_end,
                dataType: 'json',
                success: function (obj) {
                    if (obj) {
                        //alert("机构添加成功!");
                        AddUser();
                    }
                    else
                        alert("机构添加失败!");
                }
            });
        }
        function GetOrgInfo() {
            if ($("#orgName").val().Trim() == "") {
                alert("请输入机构名称!");
                return false;
            }
            else
                orgName = $("#orgName").val().Trim();
            if ($("#orgDes").val().Trim() == "") {
                alert("请输入机构描述!");
                return false;
            }
            else
                orgDescription = $("#orgDes").val().Trim();
            if ($("#slcOrgCategory").val() == "-99") {
                alert("请选择机构类别!");
                return false;
            }
            else
                orgCategory = $("#slcOrgCategory").val().Trim();
            if ($("#slcProvince").val() == "" || $("#slcProvince").val()=="-99") {
                alert("请选择省份!");
                return false;
            }
            else
                orgProvince = $("#slcProvince").val().Trim();
            if ($("#slcCity").val() == "-99" || $("#slcCity").val() == "") {
                alert("请选择城市!");
                return false;
            }
            else
                orgCity = $("#slcCity").val().Trim();
            if ($("#slcCountry").val() == "-99"||$("#slcCountry").val() =="") {
                alert("请选择机区县!");
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
           
            $("input[name='isverify']:checked").each(function () {
                orgIsverify = true;
            });
            $("input[name='isverify_end']:checked").each(function () {
                orgIsverify_end = true;
            });            
            return true;
        }
        var userName = "";
        var userAccount = "";
        var userPwd = "";
        var userPhone = "";
        var AddUser = function () {
            if (!GetUserInfo())
                return;
            $.ajax({
                type: 'post',
                url: 'AddUser',
                data: 'USERNAME=' + userName + '&ACCOUNT=' + userAccount + '&PWD=' + userPwd + '&CONTACT=' + userPhone,
                dataType: 'json',
                success: function (obj) {
                    if (obj)
                        alert("机构和管理员添加成功!");
                    else
                        alert("机构和管理员添加失败!管理员账号已存在");
                }
            });
        }
        function GetUserInfo() {
            if ($("#userName").val().Trim() == "") {
                alert("请输入用户名");
                return false;
            }
            else
                userName = $("#userName").val().Trim();
            if ($("#userAccount").val().Trim() == "") {
                alert("请输入账户名");
                return false;
            }
            else
                userAccount = $("#userAccount").val().Trim();
            //密码判断
            if ($("#userPwd").val().Trim() == "") {
                alert("请输入密码");
                return false;
            }
            else if ($("#userPwdConfirm").val().Trim() != $("#userPwd").val().Trim()) {
                alert("两次输入密码不一致!");
                return false;
            }
            else
                userPwd = $("#userPwd").val().Trim();
            if ($("#userPhone").val().Trim() == "") {
                alert("请输入用户电话");
                return false;
            }
            else
                userPhone = $("#userPhone").val().Trim();
            return true;
        }
        $(function () {
            //得到机构类别列表
            $.ajax({
                type: 'post',
                url: 'GetOrgCategoryList',
                dataType: 'json',
                success: function (obj) {
                    for (var i = 0; i < obj.length; i++) {
                        $("#slcOrgCategory").append("<option value='" + obj[i].VALUE + "'>" + obj[i].NAME + "</option>");
                    }
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
                    provice = $("#slcProvince").val();
                    //alert(provice);
                    //alert(provice);
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
                            city = $("#slcCity").val();
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
                                }
                            });
                        }
                    });
                }
            });
            $("#slcProvince").change(function () {
                provice = $(this).val();
                //alert(provice);
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
                    }
                });
            });
            $("#btnSubmit").click(function () {
                AddOrg();
            });
        });
    </script>
</head>
<body>
    <div style="float:left;width:40%">
        <fieldset>
            <legend>
                机构信息
            </legend>
            <table class="hor-minimalist-top">
                <tr>
                    <td>机构名称：</td>
                    <td><input type="text" id="orgName"  /></td><td style="color: #FF0000">*</td>
                </tr>
                <tr>
                    <td>机构描述：</td>
                    <td><input type="text" id="orgDes" /></td><td style="color: #FF0000">*</td>
                </tr>
                <tr>
                    <td>机构类别:</td>
                    <td><select id="slcOrgCategory">
                        </select>
                    </td><td style="color: #FF0000">*</td>
                </tr>
                <tr>
                    <td>省份:</td>
                    <td><select id="slcProvince">
                        </select>
                    </td><td style="color: #FF0000">*</td>
                </tr>
                 <tr>
                    <td>城市:</td>
                    <td><select id="slcCity">
                        </select>
                    </td><td style="color: #FF0000">*</td>
                </tr>
                 <tr>
                    <td>区县:</td>
                    <td><select id="slcCountry">
                            <option value="1">1</option>
                            <option value="2">2</option>
                        </select>
                    </td><td style="color: #FF0000">*</td>
                </tr>
                <tr>
                    <td>联系人:</td>
                    <td><input type="text" id="orgContacter" /></td><td style="color: #FF0000">*</td>
                </tr>
                <tr>
                    <td>联系电话:</td>
                    <td><input type="text" id="orgContactPhone" /></td><td style="color: #FF0000">*</td>
                </tr>
                <tr>
                    <td><input type="checkbox" name="isverify" id="isverify" value=""/></td>
                    <td>审核权</td>
                </tr>
               <%-- <tr>
                    <td><input type="checkbox" name="isverify_end" id="isverify_end" value="" /></td>
                    <td>最终审核权</td>
                </tr>--%>
            </table>
        </fieldset>
        
    </div>
    <div style="width:40%; margin-left:50%">
        <fieldset>
            <legend>
                管理人员信息
            </legend>
            <table class="hor-minimalist-top">
                <tr>
                    <td>名称:</td>
                    <td><input type="text" id="userName" /></td><td style="color: #FF0000">*</td>
                </tr>
                <tr>
                    <td>账号:</td>
                    <td><input type="text" id="userAccount" /></td><td style="color: #FF0000">*</td>
                </tr>
                <tr>
                    <td>密码:</td>
                    <td><input type="password" id="userPwd" /></td><td style="color: #FF0000">*</td>
                </tr>
                <tr>
                    <td>确认:</td>
                    <td><input type="password" id="userPwdConfirm" /></td><td style="color: #FF0000">*</td>
                </tr>
                <tr>
                    <td>联系电话:</td>
                    <td><input type="text" id="userPhone" /></td><td style="color: #FF0000">*</td>
                </tr>
            </table>
        </fieldset>
    </div>
    <div>
    <span style="display:block;text-align:center; margin-right:30%; margin-top:4% ">
            <input type="button" id="btnSubmit" value="确认"/>
            <a href="OrgManage"><input type="button" value="返回"/></a>
    </span>
    </div>
</body>
</html>
