function ShareAD() {
    var data = new Object();
    data.SSID = "";
    data.OID = "1";
    data.ADID = "1";
    data.TITLE = "";
    data.PATH = "";
    data.SESSION = getQueryString("sessionid");
    data.PSESSION = "";
    data.SHARETYPE = "";

    data.ToString = function () {
        var str = '{';
        str += '"SSID":"' + data.SSID + '",';
        str += '"OID":"' + data.OID + '",';
        str += '"ADID":"' + data.ADID + '",';
        str += '"TITLE":"' + data.TITLE + '",';
        str += '"PATH":"' + data.PATH + '",';
        str += '"SESSION":"' + data.SESSION + '",';
        str += '"PSESSION":"' + data.PSESSION + '",';
        str += '"SHARETYPE":"' + data.SHARETYPE + '"';
        str += '}';
        return str;
    }


    data.SSID = getQueryString("nasid");
    var url = window.location.href;
    url = url.match(/\d+[/]Pub[/]\d+_\d+/i);
    if (url != null) {
        var urlArr = url.toString().split("/");
        data.OID = urlArr[0];
        data.ADID = urlArr[2].split("_")[0];
    }
    var fpath = window.location.href;
    if (fpath.indexOf("?") > 0) {
        data.PATH = fpath.split("?")[0];
    } else {
        data.PATH = fpath;
    }
    data.TITLE = document.title;
    return data;
}

function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}

function postShareInfo(data) {
    $.ajax({
        type: 'post',
        url: '/UI/AjaxComm.aspx',
        data: 'type=ShareCount&param=' + data.ToString(),
        dataType: 'json',
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
        success: function (obj) {
            if (obj.ResultCode == 1)
                alert(obj.ResultMsg);
        }
    });
}

function postShareAD(data) {
    $.ajax({
        type: 'post',
        url: '/UI/AjaxComm.aspx',
        data: 'type=ShareAD&param=' + data.ToString(),
        dataType: 'json',
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
        success: function (obj) {
            if (obj.ResultCode == 1)
                alert(obj.ResultMsg);
        }
    });
}

function initShareCtrl() {
    if (document.getElementById("div_share") != null) {
        var str = "";
        str += "<div class='jiathis_style_24x24'><span class='jiathis_txt'>分享到：</span>";
        str += "<div name='divShare_qzone' style='position:absolute; left:88px; height:24px; width:28px; cursor:pointer'></div><a class='jiathis_button_qzone'></a>";
        str += "<div name='divShare_tsina' style='position:absolute; left:116px; height:24px; width:28px; cursor:pointer'></div><a class='jiathis_button_tsina'></a>";
        str += "<div name='divShare_weixin' style='position:absolute; left:144px; height:24px; width:28px; cursor:pointer'></div><a class='jiathis_button_weixin'></a>";
        str += "</div>";
        document.getElementById("div_share").innerHTML = str;
        //document.write("<script language=\"javascript\" src=\"http://v3.jiathis.com/code_mini/jia.js\" charset=\"utf-8\"><\/script>");
        document.write("<script language=\"javascript\" src=\"http://v3.jiathis.com/code_mini/jia.js?uid=1944539\" charset=\"utf-8\"><\/script>");

        document.write("<style type='text/css'>");
        document.write("#d_share_login{border: 1px solid rgba(0, 0, 0, 0.298039);border-top-left-radius: 6px;border-top-right-radius: 6px;border-bottom-right-radius: 6px;border-bottom-left-radius: 6px;box-shadow: rgba(0, 0, 0, 0.298039) 0px 3px 7px;left: 50%;margin: -110px 0px 0px -100px;padding: 22px;position: fixed;top: 50%;width: 190px;height: 210px;overflow: hidden;z-index: 1000000;background-color: rgb(255, 255, 255);background-clip: padding-box;-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif;font-size: 14px;line-height: 1.428571429;color: #333;background-color: #fff;display:none;}");
        document.write("#d_share_login span,#d_share_login div{font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif;font-size: 14px;line-height: 1.428571429;color: #333;-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;text-shadow: 0 0px 0 #111;}");
        document.write("#a_share_close{text-decoration: none;margin-top: -2px;color: #000000;float: right;font-size: 20px;font-weight: bold;cursor: pointer;line-height: 20px;opacity: 0.2;text-shadow: 0 1px 0 #FFFFFF;background: transparent;}");
        document.write("#a_share_close:hover{outline: 0;}");
        document.write("#a_share_free{background-image: none;outline: 0;box-shadow: inset 0 3px 5px rgba(0,0,0,0.125);padding: 5px 10px;font-size: 12px;line-height: 1.5;border-radius: 3px;display: inline-block;margin-bottom: 0;font-weight: normal;text-align: center;white-space: nowrap;vertical-align: middle;cursor: pointer;border: 1px solid transparent;-webkit-user-select: none;-moz-user-select: none;-ms-user-select: none;-o-user-select: none;user-select: none;background: transparent;-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif;color: #fff;background-color: #5cb85c;border-color: #4cae4c;text-decoration: none;}");
        document.write("#a_share_free:hover,#a_share_free:active{color: #fff;background-color: #47a447;border-color: #398439;text-decoration: none;}");
        document.write("</style>");

        str = "<div id='d_share_login'>";
        str += "<div>请先登陆!<a id='a_share_close' style='text-decoration:none; margin-top: -2px; color: #000000; float: right; font-size: 20px; font-weight: bold; cursor:pointer;line-height: 20px; opacity: 0.2; text-shadow: 0 1px 0 #FFFFFF;' target='_self' onclick='close_share_login()'>×</a></div> ";
        str += "<div style='padding: 10px; text-align: center;'><span id='s_share_qq'></span></div>";
        str += "<div style='padding: 10px; text-align: center;'><a id='a_share_free' href='javascript:login(0);'>试用20分钟</a></div>";
        str += "<div style='padding: 10px; text-align: center;'><span id='s_share_sina'></span></div>";
        str += "</div>";
        document.write(str);

        $("div[name^='divShare_']").click(function () {
            $("#d_share_login").show();
            //jiathis_sendto($(this).attr("name").split("_")[1]);
        });
    }
}

//验证是否登陆成功后分享上的遮罩去掉
function IsLogin() {
    if (GetQueryString("res") == "success") {
        $("div[name^='divShare_']").hide();
    }
}

var shareInfo = new ShareAD();
var tmpUrl = window.location.href;
if (getQueryString("isinfo") != null && getQueryString("isinfo") == "1") {
    shareInfo.PSESSION = shareInfo.SESSION;
    shareInfo.SESSION = "";
    if (tmpUrl.indexOf('#') > -1)
        shareInfo.SHARETYPE = tmpUrl.split('#')[1].split('-')[1];
    postShareInfo(shareInfo);
    tmpUrl = window.location.href;
} else if (getQueryString("isinfo") != null && getQueryString("isinfo") != "1") {
    tmpUrl.replace(/isinfo=\d/, "isinfo=1");
} else {
    if (tmpUrl.indexOf('?') == -1)
        tmpUrl += "?isinfo=1";
    else
        tmpUrl += "&isinfo=1";
}

var jiathis_config = {
    url: tmpUrl,
    title: document.title,
    data_track_clickback: true,
    evt: { "share": "share_callback" }
}

initShareCtrl();
IsLogin();

QC.Login({ btnId: "s_share_qq", size: "A_M" }, function (reqData, opts) {//登录成功        //根据返回数据，更换按钮显示状态方法
    QC.Login.getMe(function (openId, accessToken) {
        ect.settext(2, openId);
        ect.settext(20, "subst");
        ect.settext(0);
        alert("与QQ号对应的唯一标识" + openId);
        login_qq(openId);
    });
    liuyanForm.hidden_img.value = headImg.src = reqData.figureurl_2;
    liuyanForm.hidden_nicName.value = nicDiv.innerHTML = reqData.nickname;
    s_share_qq.innerHTML = '<span><a href="javascript:QC.Login.signOut();"><span style="color:#000000;text-decoration: none;">安全退出</span></a></span>';
}, function (opts) { alert("QQ已经退出！"); });

WB2.anyWhere(function (W) {
    W.widget.connectButton({
        id: "s_share_sina",
        type: '3,2',
        callback: {
            login: function (o) {	//登录后的回调函数
                alert(o.screen_name + " 欢迎登陆!")
                login_weibo(o.screen_name);
            },
            logout: function () {	//退出后的回调函数
                alert('logout');
            }
        }
    });
});

function share_callback(evt) {
    shareInfo.SHARETYPE = evt.data.service;
    postShareAD(shareInfo);
}

function close_share_login() {
    $("#d_share_login").hide();
}