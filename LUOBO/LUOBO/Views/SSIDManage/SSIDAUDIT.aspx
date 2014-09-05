<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>SSID审核</title>
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
        var audit = 0;
        var keystr = "";
        var size = 10;
        var allPage = 0;
        var currentPage = 1;

        $(function () {
            $("#search_type").val(audit);
            FindSSIDAudList(1);
            $("#btnSearch").click(function () {
                audit = $("#search_type").val();
                keystr = $("#search_key").val();
                FindSSIDAudList(1);
            });
            $('#chkAll').click(function () {
                if ($(this).prop("checked"))
                    $("input[name='chkAudit']").prop('checked', true);
                else {
                    $("input[name='chkAudit']").prop('checked', false);
                }
            })
        });

        function FindSSIDAudList(_curPage) {
            currentPage = _curPage;
            var result = "";
            $("#tbdList").empty();
            $("#divPage").empty();
            $("#tbdList").append("<tr><td colspan='8'>加载中...</td></tr>");
            $.ajax({
                type: 'post',
                url: 'GetSSIDAudList',
                data: 'keystr=' + keystr + '&state=' + audit + '&curPage=' + _curPage + '&size=' + size,
                dataType: 'json',
                success: function (obj) {
                    $("#tbdList").empty();
                    $("#divPage").empty();
                    if (obj.ResultCode == "0") {
                        if (obj.ResultOBJ.SSIDList.length > 0) {
                            var item;
                            for (var i = 0; i < obj.ResultOBJ.SSIDList.length; i++) {
                                item = obj.ResultOBJ.SSIDList[i];
                                result = "<tr>";
                                result += "<td align=\"center\"><input name=\"chkAudit\" type=\"checkbox\" value=\"" + item.ID + "\"></td>";
                                result += "<td>" + item.ID + "</td>";
                                result += "<td>" + item.ONAME + "</td>";
                                result += "<td>" + item.SSIDID + "</td>";
                                result += "<td>" + item.SSIDNAME + "</td>";
                                result += "<td>" + dateFormat(item.APPLYTIME, "yyyy-MM-dd hh:mm") + "</td>";
                                result += "<td>" + getStaStr(item.STATE) + "</td>";
                                result += "<td>" + getDoHtml(item.STATE, item.ID) + "</td>";
                                result += "</tr>";
                                $("#tbdList").append(result);
                            }
                            allPage = obj.ResultOBJ.AllCount % size == 0 ? obj.ResultOBJ.AllCount / size : parseInt(obj.ResultOBJ.AllCount / size) + 1;
                            ShowPage({ CurrentPage: _curPage, MaxPageSize: allPage, PageShowSize: 5, IsUpDown: true, ShowElement: document.getElementById("divPage"), PageEvents: function (page) { FindSSIDAudList(page); } });
                        }
                        else {
                            $("#tbdList").append("<tr><td colspan='8'>没有任何数据</td></tr>");
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
                    str = "<span class='label label-warning'>未审核</span>";
                    break;
                case 1:
                    str = "<span class='label label-warning'>审核中</span>";
                    break;
                case 2:
                    str = "<span class='label label-success'>已审核</span>";
                    break;
                case 3:
                    str = "<span class='label label-danger'>未通过</span>";
                    break;
                case 4:
                    str = "<span class='label label-success'>撤销</span>";
                    break;
            }
            return str;
        }
        function getDoHtml(type,id){
            var str = "";
            switch (type) {
                case 0:
                    str = "<a href='javascript:AuditSSID(" + id + ")'>【通过】</a><a href='javascript:NoAuditSSID(" + id + ")'>【不通过】</a><a href='javascript:BackAuditSSID(" + id + ")'>【撤销】</a>";
                    break;
                case 1:
                    str = "<a href='javascript:AuditSSID(" + id + ")'>【通过】</a><a href='javascript:NoAuditSSID(" + id + ")'>【不通过】</a><a href='javascript:BackAuditSSID(" + id + ")'>【撤销】</a>";
                    break;
            }
            return str;
        }
        function changetype(){
            audit = $("#search_type").val();
            FindADList(1);
        }

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

        //通过审核
        function AuditSSID(_Aud_ids) {
            if (confirm("确定通过吗？")) {
                $.ajax({
                    type: 'post', //可选get
                    url: 'AuditSSID', //这里是接收数据的PHP程序
                    data: 'ids=' + _Aud_ids, //传给PHP的数据，多个参数用&连接
                    dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                    success: function (obj) {
                        if (obj.ResultCode == 0) {
                            keystr = "";
                            FindSSIDAudList(currentPage);
                            alert("审核成功，该SSID名称通过审核!");
                        }
                        else
                            alert("审核失败!");
                    }
                });
            }
        }

        //不通过审核
        function NoAuditSSID(_Aud_ids) {
            if (confirm("确定拒绝吗？")) {
                $.ajax({
                    type: 'post', //可选get
                    url: 'NoAuditSSID', //这里是接收数据的PHP程序
                    data: 'ids=' + _Aud_ids, //传给PHP的数据，多个参数用&连接
                    dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                    success: function (obj) {
                        if (obj.ResultCode == 0) {
                            keystr = "";
                            FindSSIDAudList(currentPage);
                            alert("审核成功，该SSID名称未通过审!");
                        }
                        else
                            alert("审核失败!");
                    }
                });
            }
        }

        // 删除审核
        function DelAuditSSID(_Aud_ids) {
            if (confirm("确定删除吗？")) {
                $.ajax({
                    type: 'post', //可选get
                    url: 'RemoveAuditSSID', //这里是接收数据的PHP程序
                    data: 'ids=' + _Aud_ids, //传给PHP的数据，多个参数用&连接
                    dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                    success: function (obj) {
                        if (obj.ResultCode == 0) {
                            FindSSIDAudList(currentPage);
                            alert("删除成功!");
                        }
                        else
                            alert("删除失败!");
                    }
                });
            }
        }

        // 撤销审核
        function BackAuditSSID(_Aud_ids) {
            if (confirm("确定撤销吗？")) {
                $.ajax({
                    type: 'post', //可选get
                    url: 'BackAuditSSID', //这里是接收数据的PHP程序
                    data: 'ids=' + _Aud_ids, //传给PHP的数据，多个参数用&连接
                    dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                    success: function (obj) {
                        if (obj.ResultCode == 0) {
                            keystr = "";
                            FindSSIDAudList(currentPage);
                            alert("撤销成功，该SSID进入审核中状态");
                        }
                        else
                            alert("撤销失败!");
                    }
                });
            }
        }
        
    </script>
</head>
<body>
        <div class="container-fluid">
      <div  class="row-fluid">
        <div class="div-body col-md-12" id="ad-list" name="ad-list">
          <div class="widget-container  clearfix">
            <div class="heading"><i class='fa fa-fw fa-list-ul'></i>SSID审核<span></span>
            <div class="top-search">
            <select id="search_type" name="search_type" onchange="changetype()";><option value="-99">全部</option><option selected="selected" value="0">未审核</option><option value="2">已审核</option><option value="2">未通过</option><option value="4">撤销</option></select>
              <input id="search_key" class="form-search" type="text" />
              <input id="btnSearch" value="查询" type="button" />
              </div>
            </div>
            <div class="widget-content padded clearfix">
              <table class="hor-minimalist-a  table table-bordered table-striped">
                <thead>
                  <tr>
                    <th scope="col" style="width:80px;">选择<input id="chkAll" name="chkAll" type="checkbox"></th>
                    <th scope="col" style="width:80px;">序号</th>
                    <th scope="col">申请机构</th>
                    <th scope="col">SSID编号</th>
                    <th scope="col">SSID名称</th>
                    <th scope="col">申请时间</th>
                    <th class='hidden-xs' scope="col">状态</th>
                    <th class='hidden-xs' style="width:300px;">操作</th>
                  </tr>
                </thead>
                <tbody id="tbdList">
                </tbody>
              </table><div id="divPage" class="hor-minimalist-page"></div>
            </div>
          </div>
        </div>
       </div>
     </div>
</body>
</html>
