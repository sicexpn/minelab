<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Default</title>
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
            $("#btnSubmit").click(function () { if (LoginValidate()) { Login(); } });
        });

        function Login() {
            var ACCOUNT = $("#txtACCOUNT").val().Trim();
            var PWD = $("#txtPWD").val().Trim();

            $.ajax({
                type: 'post', //可选get
                url: 'Login', //这里是接收数据的PHP程序
                data: "ACCOUNT=" + ACCOUNT + "&PWD=" + PWD,
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    if (obj == 0)
                        alert("用户名或密码错误!");
                    else
                        window.location.href = "/Home/Default";
                }
            });
        }

        function LoginValidate() {
            if ($("#txtACCOUNT").val().Trim() == "") {
                alert("用户名不能为空!");
                return false;
            }
            if ($("#txtPWD").val().Trim() == "") {
                alert("密码不能为空!");
                return false;
            }
            return true;
        }
    </script>
</head>

  <body class="login2">
    <!-- Login Screen -->
    <div class="login-wrapper">
      <a href="#"><img width="180" height="81" src="../../Content/lhbStyle/images/logo2blue.png" /></a>

        <div class="form-group">
          <div class="input-group">
            <span class="input-group-addon"><i class="fa fa-envelope"></i></span><input id="txtACCOUNT" class="form-control" placeholder="用户名 或 Email" type="text">
          </div>
        </div>
        <div class="form-group">
          <div class="input-group">
            <span class="input-group-addon"><i class="fa fa-lock"></i></span><input id="txtPWD"  type="password" class="form-control" placeholder="密 码" type="text">
          </div>
        </div>
        <a class="pull-right" href="#">忘记密码?</a>
        <div class="text-left">
          <label class="checkbox"><input type="checkbox"><span>保持登录状态</span></label>
        </div>
        <input id="btnSubmit" class="btn btn-lg btn-primary btn-block" type="submit" value="登 录">


    </div>
    <!-- End Login Screen -->
  </body>


</html>
