<%@ Page Language="C#"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Luobo WiFi</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" charset="utf-8"/>
    <link href="../UITemplet/css/bootstrap-3.2.min.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/css/font-awesome.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/css/login.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/css/offline-theme-default.css" media="all" rel="stylesheet" type="text/css" />
    

    <script src="../UITemplet/js/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../UITemplet/js/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../UITemplet/js/bootstrap-3.2.min.js" type="text/javascript"></script>
    <script src="../UITemplet/js/login.js" type="text/javascript"></script>
    <script src="../UITemplet/js/common.js" type="text/javascript"></script>
    <script src="../UITemplet/js/offline.min.js" type="text/javascript"></script>
    
  <script type="text/javascript">
    var ssid = GetQueryString("nasid");
    var myMac = GetQueryString("mac");
    var checkModel;

    $(function () {
        CheckAllow();
        //      $("#btnSubmit").click(function () { if (LoginValidate()) { Login(); } });
        $("#btnSubmit").click(function () { Login(); });
    });

    function CheckAllow() {
      if (myMac == null || ssid == null) {
        $('#lbl_check').html("请您打开任意其他网页，待其自动跳转到本页面再进行安装");
        $('#progress').hide();
      } else {
        $.ajax({
          type: 'post',
          url: 'AjaxComm.aspx',
          data: 'type=CheckInstallPerson&ssid=' + ssid + '&mac=' + myMac,
          dataType: 'json',
          success: function (obj) {

            if (obj.ResultCode == 1) {
              $('#lbl_check').html(obj.ResultMsg);
              $('#progress').hide();
            }
            else {
              checkModel = obj.ResultOBJ;
              if (checkModel == null) {
                $('#lbl_check').html(obj.ResultMsg);
                $('#progress').hide();
              }
              else {
                if (checkModel.UOID != checkModel.APOID) {
                  $('#lbl_check').html("该设备不归属于您的代理商管理，请联系代理商");
                  $('#progress').hide();
                } else {
                  $('#lbl_check').html("您有权限对该设备进行安装，正在读取您的资料，请稍候...");
                  setTimeout(showLogin, 2000);
                }
              }
            }
          }
        });
      }
    }
    function showLogin() {
      //$('#lbl_check').hide();
        $('#progress').hide();
        $('#lbl_check').html("欢迎您，<span style='color:red;'>" + checkModel.ONAME + "</span>的<span style='color:red;'>" + checkModel.USERNAME + "</span>，请您进入安装");
      //$('#txtACCOUNT').show();
      //$('#txtPWD').show();
      //$('#txtPWD').focus();
      $("#btnSubmit").css("visibility", "visible");
      //$('#txtACCOUNT').val(checkModel.ACCOUNT);
    }

    function Login() {
      window.location.href = "install.aspx?token=" + checkModel.TOKEN + "&mac=" + checkModel.APMAC + "&umac=" + myMac + "&username=" + encodeURIComponent(checkModel.USERNAME);
      //var ACCOUNT = $("#txtACCOUNT").val();
      //var PWD = $("#txtPWD").val();
      //
      //$.ajax({
      //  type: 'post',
      //  url: 'AjaxComm.aspx',
      //  data: 'type=LoginInstall&param={"ACCOUNT":"' + ACCOUNT + '","PWD":"' + PWD + '","MAC":"' + myMac + '"}',
      //  dataType: 'json',
      //  success: function (obj) {
      //    if (obj.ResultCode == 1)
      //      alert(obj.ResultMsg);
      //    else {
      //      var token = obj.ResultOBJ;
      //      window.location.href = "install.aspx?token=" + token + "&mac=" + checkModel.APMAC + "&umac=" + myMac + "&username=" + encodeURIComponent(checkModel.USERNAME);
      //    }
      //  }
      //});
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
  </script>
</head>
<body class="login1">
<!-- Login Screen -->
<div class="login-wrapper">
  <div class="login-container"> <a href="#"><img width="180" height="81" src="../UITemplet/img/logo2blue.png" /></a>
    <form action="index.aspx">
      <div class="form-group">
        <input type="text" name="txtACCOUNT" id="txtACCOUNT" style="display:none;" class="form-control" placeholder="&#xf007;   用户名  / Username">
        <label id="lbl_check">正在检查您的设备是否有权限进行安装操作</label>
      </div>
      <div class="form-group">
        <input type="password" name="txtPWD" id="txtPWD" style="display:none;" class="form-control" placeholder="&#xf09c;   密码 / Password">
        <div class="progress" id="progress">
          <div class="progress-bar progress-bar-striped active"  role="progressbar" aria-valuemin="0" aria-valuemax="100" style="width: 100%">
          <span class="sr-only">45% Complete</span>
          </div>
        </div>
      </div>
    </form>
    <div class="social-login clearfix"><a id="btnSubmit" class="btn btn-primary  blue" style="visibility: hidden;" href="javascript:void(0);"><i class="icon-twitter"></i>进入 / Enter</a> </div>
    <p class="signup"> 萝卜wifi云平台 V1.68, <a href="signup.aspx"> 万虔云志</a> 出品</p>
    <p class="signup" style="margin-top:5px;font-size:12px">推荐使用谷歌Chrome浏览器访问本平台</p>
  </div>
</div>
<!-- End Login Screen -->
</body>
</html>
