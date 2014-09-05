<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Header</title>
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
        function Logout() {
            $.ajax({
                type: 'post', //可选get
                url: 'Logout', //这里是接收数据的PHP程序
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    if (obj == 1)
                        window.parent.location.href = "/Login/Default";
                }
            });
        }
    </script>
</head>
<body>
      <!-- Navigation -->
  <div class="navbar navbar-fixed-top scroll-hide">
    <div class="container-fluid top-bar">
      <div class="pull-right">
      

      <ul class="nav navbar-nav pull-right">
          <li class="dropdown settings hidden-xs"> <a href="#">
[<%=ViewData["username"]%>]
            </a>
            
          </li>
          <li class="dropdown hidden-xs">      <a href="javascript:Logout();"> <i class="icon-signout"></i>退出</a>
            
          </li>
        </ul>
      </div>
      <a class="logo" href="">nextwifi</a> <center> <h2><small>欢迎来到萝卜网络广告营销平台!</small></h2></center>
    </div>

  </div>
  <!-- End Navigation -->
</body>
</html>
