<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<IEnumerable<LUOBO.Model.M_SYS_ORGANIZATION>>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>OrgPropList</title>
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
    <link rel="stylesheet" href="../../Scripts/jquery-ui.css"></link>
    <script src="../../Scripts/jquery-1.10.2.js"></script>
    <script src="../../Scripts/jquery-ui.js"></script>
    <script type="text/javascript">
        var orgName = "";
        var orgProvince = "";
        var city = "";
        var country = "";
        var currentPage = 0;
        var size = 5;
        var allPage = 0;
        $(function () {

            FindOrgList(1);
            $("#btnSearch").click(function () { FindOrgList(1); });
            $("#btnDelete").click(function () { DeleteOrg() });
            $("#btnAdd").click(function () { window.location.href = "OrgRegister"; });
            //得到省份
            $.ajax({
                type: 'post',
                url: '/OrganizationManage/GetAllProviceList',
                dataType: 'json',
                success: function (obj) {
                    for (var i = 0; i < obj.length; i++) {
                        $("#slcProvince").append("<option value='" + obj[i] + "'>" + obj[i] + "</option>");
                    }
                }
            });

            var provice = "";
            var city = "";
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
        });
        function FindOrgList(curPage) {
            currentPage = curPage;
            orgName = $("#textOrg").val();
            city = $("#slcCity").val();
            country = $("#slcCountry").val();
            orgProvince = $("#slcProvince").val();
            var result = "";

            $.ajax({
                type: 'post',
                url: 'FindOrgList',
                data: 'province=' + orgProvince + '&orgName=' + orgName + '&city=' + city + '&country=' + country + '&size=' + size + '&curPage=' + curPage,
                dataType: 'json',
                success: function (obj) {
                    $("#tbdOrgList").empty();
                    $("#divPage").empty();
                    if (obj.OrgList.length > 0) {
                        var item;
                        for (var i = 0; i < obj.OrgList.length; i++) {
                            item = obj.OrgList[i];
                            result = "<tr>";
                            //result += "<td><input name='ckOrg' type='checkbox' value ='" + item.ID + "'></input></td>";
                            result += "<td>" + item.NAME + "</td>";
                            result += "<td>" + item.CATEGORYNAME + "</td>";
                            result += "<td>" + item.DESCRIPTION + "</td>";
                            result += "<td>" + item.PROVINCE + "</td>";
                            result += "<td>" + item.CITY + "</td>";
                            result += "<td>" + item.COUNTIES + "</td>";
                            result += "<td><a href='OrgPropConfig?id=" + item.ID + "'>扩展属性配置</a></td>";
                            /*result += "<td>";
                            result += "<select id='org_log_type'>";
                            result += "<option value='" + item.PVALUE + "'>" + item.PNAME + "</option>";
                            result += "</select>";*/
                            result += "</td>";
                            result += "</tr>";
                            $("#tbdOrgList").append(result);
                        }
                        allPage = obj.AllCount % size == 0 ? obj.AllCount / size : parseInt(obj.AllCount / size) + 1;

                        ShowPage({
                            CurrentPage: curPage,
                            MaxPageSize: allPage,
                            PageShowSize: 3,
                            IsUpDown: true,
                            ShowElement: document.getElementById("divPage"),
                            PageEvents: function (page) {
                                FindOrgList(page);
                            }
                        });

                    }
                }
            });
        }

        $(function () {
            $("#checkAll").click(function () {
                $('input[name="ckOrg"]').attr("checked", this.checked);
            });
            var $subBox = $("input[name='ckOrg']");
            $subBox.click(function () {
                $("#checkAll").attr("checked", $subBox.length == $("input[name='ckOrg']:checked").length ? true : false);
            });
        });

        function DeleteOrg() {
            var ids = "";
            $("input[name='ckOrg']:checked").each(function () {
                if (ids != "")
                    ids += ",";
                ids += $(this).val();
            });
            if (ids == "") {
                alert("没有选择删除的数据!");
                return;
            }
            $.ajax({
                type: 'post', //可选get
                url: 'DeleteOrg', //这里是接收数据的PHP程序
                data: 'ids=' + ids, //传给PHP的数据，多个参数用&连接
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    if (obj) {
                        alert("删除成功!");
                        FindOrgList(1);
                    }
                    else
                        alert("删除失败!");
                }
            });
        }
    </script>
</head>
<body>

<div class="container-fluid">
      <div  class="row-fluid">
        <div class="div-body col-md-12">
        <div class="widget-container  clearfix">
            <div class="heading"><i class="fa fa-fw fa-list-ul"></i>机构扩展管理

            </div>
            <div class="widget-content padded clearfix">
                        <div class="top-search">
              机构名称：<input id="textOrg" type="text" style="width:50" />
              所在省份：<select id="slcProvince">
                        <option value="" selected="selected">全部</option>
                    </select> 
              所在城市：<select id="slcCity">
                        <option value="" selected="selected">全部</option>
                    </select> 

              区县：<select id="slcCountry">
                        <option value="">全部</option>
                    </select> 
              <input id="btnSearch" type="button" value="查询" />&nbsp;&nbsp;
              </div>
             <br></br>


              <table class="hor-minimalist-a  table table-bordered table-striped">
<thead>
            
                <tr>
                    <th scope="col">机构名称</th>
                    <th scope="col">机构类别</th>
                    <th scope="col">机构描述</th>
                    <th scope="col">省份</th>
                    <th scope="col">城市</th>
                    <th scope="col">区县</th>
                    <th scope="col">操作</th>
                </tr>
            </thead>
            <tbody id="tbdOrgList">
                
            </tbody>
            </table>
            <div id="divPage" class="hor-minimalist-page"></div>
            </div>
            
            </div>
        </div>
        </div>

    </div>
</body>
</html>

