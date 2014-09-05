<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ADJump.aspx.cs" Inherits="LUOBO.SingleShop.UI.ADJump" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        html,body {
            width:100%;height:100%;margin:0px;padding:0px;overflow:hidden;
        }
        #box{
            width:100%;height:100%;padding-bottom:60px;display:table;text-align:center;border:0px solid #d3d3d3;background:#fff;
        }
        #box img{
            border:0px solid #ccc;
        }
    </style>
    <script type="text/javascript" language="Javascript">
        var reTime = "2000";

        function resizeimg() {
            var objList = document.getElementById("box").getElementsByTagName("img");
            var obj = objList[0];
            var maxH = window.innerHeight - 220;
            var maxW = window.innerWidth - 200;
            var imgW = obj.width;
            var imgH = obj.height;
            var ratioA = imgW / maxW;
            var ratioB = imgH / maxH;
            if (ratioA > ratioB) {
                obj.width = maxW;
                obj.height = maxW * (imgH / imgW);
            }
            else {
                obj.height = maxH;
                obj.width = maxH * (imgW / imgH);
            }
            marginH = (window.innerHeight - obj.height - 120) / 2;
            obj.style.marginTop = marginH + "px";

        }

        function getURLParam(n) {
            var s = "";
            var href = window.location.href;
            var idx = href.indexOf("?");
            if (idx > -1) {
                var qs = href.substr(idx + 1);
                idx = qs.indexOf(n + "=");
                if (idx > -1) {
                    s = qs.substr(idx + n.length + 1);
                    if (s.charAt(4) == '%' || s.charAt(5) == '%') {
                        s = s.replace(/%3a/g, ":");
                        s = s.replace(/%2f/g, "/");
                        s = s.replace(/%3f/g, "?");
                        s = s.replace(/%3d/g, "=");
                        s = s.replace(/%26/g, "&");
                        s = s.replace(/%25/g, "%");
                        s = s.replace(/\+/g, " ");
                    }
                    return s;
                }
            }
        }

        var loginUrl;
        function redirect() { window.location = loginUrl; return false; }
        window.onload = function () {
            resizeimg();
            loginUrl = getURLParam("loginurl");
            setTimeout(redirect, reTime);
        }
</script>
</head>
<body>
    <div id="div_redir" style="z-index: 2;">
		<center>
			<div id="box" style="margin: 0px auto 0px auto;">
				<a href="http://www.next-wifi.com"><img src="/ADTemplet/1/img/logo2.jpg"/></a>
                <h3 style="color:#CC481A">萝卜</h3>
                <p style="color:Gray">免费 <img src="/ADTemplet/1/img/ld (34).gif" width="16" height="16"  alt=""/> WIFI</p>
            </div>
		</center>
	</div>
</body>
</html>
