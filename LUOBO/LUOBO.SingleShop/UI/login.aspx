<%@ Page Language="C#"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Luobo WiFi</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" charset="utf-8"/>
    <link href="../UITemplet/css/bootstrap.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/css/font-awesome.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/css/login.css" media="all" rel="stylesheet" type="text/css" />
    <script src="../UITemplet/js/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../UITemplet/js/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../UITemplet/js/bootstrap.js" type="text/javascript"></script>
    <script src="../UITemplet/js/login.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#btnSubmit").click(function () { if (LoginValidate()) { Login(); } });
        });

        function Login() {
            var ACCOUNT = $("#txtACCOUNT").val();
            var PWD = $("#txtPWD").val();

            $.ajax({
                type: 'post', //可选get
                url: 'AjaxComm.aspx', //这里是接收数据的PHP程序
                data: 'type=Login&param={"ACCOUNT":"' + ACCOUNT + '","PWD":"' + PWD + '"}',
                dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
                success: function (obj) {
                    if (obj.ResultCode == 1)
                        alert(obj.ResultMsg);
                    else {
                        var token = obj.ResultOBJ;
                        window.location.href = "gis.aspx?token=" + token;
                    }
                }
            });
        }

        function LoginValidate() {
            if ($("#txtACCOUNT").val() == "") {
                alert("用户名不能为空!");
                return false;
            }
            if ($("#txtPWD").val() == "") {
                alert("密码不能为空!");
                return false;
            }
            return true;
        }

        $(document).ready(function () {
            if (window.ActiveXObject) {
                alert("平台采用最新HTML5技术实现对AP的控制与数据展现，建议您下载使用对HTML5支持良好的Google Chrome浏览器。");
            }
        });
    </script>
</head>
<body class="login1">
<!-- Login Screen -->
<div class="login-wrapper">
  <div class="login-container"> <a href="#"><img width="180" height="81" src="../UITemplet/img/logo2blue.png" /></a>
    <form action="index.aspx">
      <div class="form-group">
        <input type="text" name="txtACCOUNT" id="txtACCOUNT"  class="form-control" placeholder="&#xf007;   用户名  / Username">
      </div>
      <div class="form-group">
        <input type="password" name="txtPWD" id="txtPWD"  class="form-control" placeholder="&#xf09c;   密码 / Password">
      </div>
      
    </form>
    <div class="social-login clearfix"><a id="btnSubmit" class="btn btn-primary  blue" href="javascript:void(0);"><i class="icon-twitter"></i>登录 / login</a> </div>
    <p class="signup"> 萝卜wifi云平台 V1.68, <a href="signup.aspx"> 万虔云志</a> 出品</p>
    <p class="signup" style="margin-top:5px;font-size:12px">推荐使用谷歌Chrome浏览器访问本平台</p>
  </div>
  
</div>
<!-- End Login Screen -->
</body>
</html>
