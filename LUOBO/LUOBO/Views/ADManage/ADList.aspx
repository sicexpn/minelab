<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>ADList</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta charset="utf-8">
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
        var audit = <%:ViewData["aud_id"]%>;
        var keystr = "";
        var size = 10;
        var allPage = 0;
        var currentPage = 1;

        $(function () {
            $("#search_type").val(audit);
            FindADList(1);

            $("#btnAdd").click(function () { AddAPCT(); });
            $("#btnSearch").click(function () { 
                audit = $("#search_type").val();
                keystr = $("#search_key").val();
                FindADList(1);
            });
        });

        function FindADList(curPage) {
            currentPage = curPage;
            var result = "";
            $.ajax({
                type: 'post',
                url: 'FindADList',
                data: 'audit=' + audit + '&size=' + size + '&curPage=' + curPage + '&keystr=' + keystr,
                dataType: 'json',
                success: function (obj) {
                    $("#tbdAPCTList").empty();
                    $("#divPage").empty();
                    if (obj.ResultCode == "0") {
                        if (obj.ResultOBJ.APCTList.length > 0) {
                            var item;
                            for (var i = 0; i < obj.ResultOBJ.APCTList.length; i++) {
                                item = obj.ResultOBJ.APCTList[i];
                                result = "<tr>";
                                result += "<td>" + item.AD_ID + "</td>";
                                result += "<td>" + item.AD_Title + "</td>";
                                result += "<td>" + item.AD_SSID + "</td>";
                                result += "<td>" + getStaStr(item.AD_Stat) + "</td>";
                                result += "<td>" + getDoHtml(item.AD_Stat,item.AD_ID) + "</td>";
                                result += "</tr>";
                                $("#tbdAPCTList").append(result);
                            }
                            allPage = obj.ResultOBJ.AllCount % size == 0 ? obj.ResultOBJ.AllCount / size : parseInt(obj.ResultOBJ.AllCount / size) + 1;
                            ShowPage({ CurrentPage: curPage, MaxPageSize: allPage, PageShowSize: 5, IsUpDown: true, ShowElement: document.getElementById("divPage"), PageEvents: function (page) { FindADList(page); } });
                        }
                    } else {
                        alert(obj.ResultMsg);
                    }
                }
            });
        }
        function getStaStr(type){
            var str = "";
            switch (type) {
                case 0:
                    str = "<span class='label label-danger'>未审核</span>";
                    break;
                case 1:
                    str = "<span class='label label-warning'>审核中</span>";
                    break;
                case 2:
                    str = "<span class='label label-success'>已审核</span>";
                    break;
            }
            return str;
        }
        function getDoHtml(type,id){
            var str = "";
            switch (type) {
                case 0:
                    str = "<a href='ADEdit?ad_id=" + id + "'>【编辑】</a> <a href='ADPubHistory?ADID=" + id + "'>【发布历史】</a> <a href='javascript:DelAD(" + id + ")'>【删除】</a>";
                    break;
                case 1:
                    str = "<a href='ADEdit?ad_id=" + id + "'>【取消审核】</a> <a href='ADPubHistory?ADID=" + id + "'>【发布历史】</a> <a href='javascript:DelAD(" + id + ")'>【删除】</a>";
                    break;
                case 2:
                    str = "<a href='ADEdit?ad_id=" + id + "'>【再次编辑】</a> <a href='ADPubHistory?ADID=" + id + "'>【发布历史】</a> <a href='javascript:DelAD(" + id + ")'>【删除】</a>";
                    break;
            }
            return str;
        }
        function changetype(){
            audit = $("#search_type").val();
            FindADList(1);
        }
        function DelAD(ADID) {
            alert("del:"+ADID);
//            var ids = "";
//            $("input[name='ckAPCT']:checked").each(function () {
//                if (ids != "")
//                    ids += ",";
//                ids += $(this).val();
//            });

//            if (ids == "") {
//                alert("没有选择删除的数据!");
//                return;
//            }
//            $.ajax({
//                type: 'post', //可选get
//                url: 'DelAPCT', //这里是接收数据的PHP程序
//                data: 'ids=' + ids, //传给PHP的数据，多个参数用&连接
//                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
//                success: function (obj) {
//                    if (obj) {
//                        FindAPCTList(currentPage);
//                        alert("删除成功!");
//                    }
//                    else
//                        alert("删除失败!");
//                }
//            });
        }

        function AddAPCT() {
            window.location.href = "ADEdit?ad_id=0";
        }

    </script>
</head>
<body>
        <div class="container-fluid">
      <div  class="row-fluid">
        <div class="div-body col-md-12" id="ad-list" name="ad-list">
          <div class="widget-container  clearfix">
            <div class="heading"><i class='fa fa-fw fa-list-ul'></i>广告管理<span>  </span>
            <div class="top-search">
            <select id="search_type" name="search_type" onchange="changetype()";><option value="-1">全部</option><option value="0">未审核</option><option value="1">审核中</option><option value="2">已审核</option></select>
              <input id="search_key" class="form-search" type="text" />
              <input id="btnSearch" value="查询" type="button" />
              <input id="btnAdd" class="go-ad-edit" value="新增" type="button" />
              </div>
            </div>
            <div class="widget-content padded clearfix">
              <table class="hor-minimalist-a  table table-bordered table-striped">
                <thead>
                  <tr>
                  <th scope="col" style="width:40px;">序号</th>
                  <th scope="col">广告名称</th>
                    <th scope="col">SSID名称</th>
                    <th class='hidden-xs' scope="col">状态</th>
                    <th class='hidden-xs' >操作</th>
                  </tr>
                </thead>
                <tbody id="tbdAPCTList">
                </tbody>
              </table><div id="divPage" class="hor-minimalist-page"></div>
            </div>
            
          </div>
        </div>


</body>
</html>
