//获取链接参数
function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = decodeURIComponent(window.location.search).substr(1).match(reg);
    if (r != null) {
        return unescape(r[2]);
    }
    else {
        r = window.location.search.substr(1).match(reg);
        if (r != null) {
            return unescape(r[2]);
        }
        else {
            return null;
        }
    }
}

var uamip = GetQueryString("uamip");
var uamport = GetQueryString("uamport");
var mac = GetQueryString("mac");
var AcctSessionId = GetQueryString("sessionid");
var SSID = GetQueryString("nasid");
var CalledStationId = GetQueryString("called");
var CallingStationId = GetQueryString("mac");
var free_usr = "freeuser";
var free_pass = "F2FFD12F81567F535ACE07A8730CE92C";
var login_url = "http://" + uamip + ":" + uamport + "/www/login.chi";


//获取登录参数
           var PropertyOBJ = null;

           var isopenqq = "0";
           var isopenyj = "0";
           var isopensinawb = "0";
           var isopenwx = "0";
           var wxqrurl = "";

           var mfuid = "";
           var mfpass = "";
           var mftime="1200";
           var qquid = "";
           var qqpass = "";
           var wbuid = "";
           var wbpass = "";


            function getparmeter() {
                $.ajax({
                    type: 'post',
                    url: '/UI/AjaxComm.aspx',
                    data: 'type=GetLoginProperty&token=' + SysWifi_ORG_ID,
                    dataType: 'json',
                    error: function (msg) {
                        //alert("服务器错误");
                    },
                    success: function (obj) {
                        if (obj.ResultCode == 0) {
                            PropertyOBJ = obj.ResultOBJ;
                            var tt = "";

                            if (getPropertyValue("微信认证", "是否启用")=="1") {
                                isopenwx = "1";
                                var wxuid = getPropertyValue("微信认证", "Radius用户名");
                                var wxpass = getPropertyValue("微信认证", "Radius密码");
                                //wxqrurl = getPropertyValue("微信认证", "wxqrurl");
                                WeiXLogin({ CallBack: function () { login(0, wxuid, wxpass); } });
                            }
                            if (getPropertyValue("免费上网", "是否启用") == "1") {

                                 isopenyj = "1";

                                mfuid = getPropertyValue("免费上网", "Radius用户名");
                                mfpass = getPropertyValue("免费上网", "Radius密码");
                                mftime = getPropertyValue("免费上网", "上网时长（分钟）");
                            }
                            if (getPropertyValue("QQ认证", "是否启用") == "1") {
                                isopenqq = "1";
                                qquid = getPropertyValue("QQ认证", "Radius用户名");
                                qqpass = getPropertyValue("QQ认证", "Radius密码");
                            }
                            if (getPropertyValue("SINA微博认证", "是否启用") == "1") {

                                isopensinawb = "1";
                                wbuid = getPropertyValue("SINA微博认证", "Radius用户名");
                                wbpass = getPropertyValue("SINA微博认证", "Radius密码");
                            }
                            for (var i = 0; i < obj.ResultOBJ.length; i++) {
                                tt += "PTYPE=" + obj.ResultOBJ[i].PTYPE + ",PNAME=" + obj.ResultOBJ[i].PNAME + ",PVALUE=" + obj.ResultOBJ[i].PVALUE + "\n";
                            }
                            if (isopensinawb=="1" || isopenqq=="1" || isopenyj=="1" || isopenwx=="1") {
                                if (wxqrurl == null || wxqrurl == "") {
                                    wxqrurl = "http://ad.next-wifi.com/WXQR/10035.jpg";
                                }
                                login_list(isopenyj, isopenqq, isopensinawb, isopenwx, wxqrurl);
                                $("#form1").attr("action", login_url);
                                login_init(isopenqq, isopensinawb);
                                $("#login-btn").removeClass("ui-state-disabled");
                            }
                        } else {
                            alert(obj.ResultMsg);
                        }
                    }
                });
            }

            function getPropertyValue(ptype, pname) {
                var revalue = "";
                if (PropertyOBJ != null && PropertyOBJ.length > 0) {
                    for (var i = 0; i < PropertyOBJ.length; i++) {
                        if (PropertyOBJ[i].PTYPE == ptype && PropertyOBJ[i].PNAME == pname) {
                            revalue = PropertyOBJ[i].PVALUE;
                        }
                    }
                }
                return revalue;
            }

//登陆
function login(type,uid,pass) {
    switch (type) {
        case 0: //第三方认证登录
            $("#username").val(uid);
            $("#password").val(pass);
            $("#form1").submit();
            //window.open(login_url + "?username=" + uid + "&password=" + pass);
            break;
        case 1: //限时登录
            if (typeof (timer) != "undefined") { clearInterval(timer); }
            maxtime = Number(mftime);
            timer = setInterval("CountDown()", 1000);
            break;
        case 2: //
            break;
    }
}

//通用第三方登录密码获取，原微博密码生成。
function login_weibo(name, uid, pass) {
    $.ajax({
        type: 'post', //可选get
        url: '/UI/AjaxComm.aspx',
        data: 'type=GetPassword&param={"CalledStationId":"' + CalledStationId + '","UserName":"' + name + '","UserType":2}',
        dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
        success: function (obj) {
            login(0, uid, pass)
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {  //#3这个error函数调试时非常有用，如果解析不正确，将会弹出错误框
            alert(XMLHttpRequest.status);
            alert(XMLHttpRequest.readyState);
            alert(textStatus); // paser error;
        }
    });
}
//登录初始化
function login_init(isopenqq, isopensinawb) {
    //qq登录初始化
    if (isopenqq) {
        QC.Login({ btnId: "qq_login_btn", size: "A_L" }, function (reqData, opts) {//登录成功        //根据返回数据，更换按钮显示状态方法
            QC.Login.getMe(function (openId, accessToken) {
                ect.settext(2, openId);
                ect.settext(20, "subst");
                ect.settext(0);
                login_weibo(openId, qquid, qqpass)
            });
            liuyanForm.hidden_img.value = headImg.src = reqData.figureurl_2;
            liuyanForm.hidden_nicName.value = nicDiv.innerHTML = reqData.nickname;
            qq_login_btn.innerHTML = '<span><a href="javascript:QC.Login.signOut();"><span style="color:#000000;text-decoration: none;">安全退出</span></a></span>';
        }, function (opts) { alert("QQ已经退出！"); });
    }

    if (isopensinawb) {
        WB2.anyWhere(function (W) {
            W.widget.connectButton({
                id: "wb_connect_btn",
                type: '2,2',
                callback: {
                    login: function (o) {	//登录后的回调函数
                        login_weibo(name, wbuid, wbpass)
                    },
                    logout: function () {	//退出后的回调函数
                        alert('logout');
                    }
                }
            });
        });
    }
}

//登录菜单初始化
            var login_set = "<li data-role='list-divider' class='ui-li-divider ui-bar-a ui-first-child'>请选择登录方式</li>";

            function login_list(isopenyj, isopenqq, isopensinawb, isopenwx, wxqrurl) {

                if (isopenyj == "1") {
                    login_set += "<li><a class='btn btn-sm btn-success active ui-btn ui-icon-carat-r'  href='javascript:login(1);'>限时试用</a></li>";
                }
                if (isopenqq == "1") {
                    login_set += "<li><a class='btn btn-sm btn-success active ui-btn ui-icon-carat-r'><span id='qq_login_btn'></span></a></li>";
                }
                if (isopensinawb == "1") {
                    login_set += "<li><a class='btn btn-sm btn-success active ui-btn ui-icon-carat-r'><span id='wb_connect_btn'></span></a></li>";
                }
                if (isopenwx == "1") {

                    login_set += "<li><a onclick='savepic(wxqrurl)' herf='" + wxqrurl + "' class='btn btn-sm btn-success active ui-btn ui-icon-carat-r'>请用微信扫码后通过公众号登录<br><img src='" + wxqrurl + "' ></img></a></li>";
                }
                login_set += "<li data-role='list-divider' class='ui-li-divider ui-bar-a ui-last-child'></li>";
                document.getElementById("login-list").innerHTML = login_set;
            }


            function savepic(url) {
                window.open(url);
            }

//倒计时
//按秒计算自调整!   var maxtime = 20 * 60 
            function CountDown() {
                if (maxtime >= 0) {
                    minutes = Math.floor(maxtime/60);
                    seconds = Math.floor(maxtime%60);
                    msg = "限时：" + minutes + "分" + seconds + "秒";
                    document.getElementById("div_LoginTime").innerHTML = msg;
                    if (maxtime == 1 * 60) alert('注意距离使用结束还有1分钟!');
                    --maxtime;
                }
                else {
                    clearInterval(timer);
                    alert("试用时间结束!");
                }
            }
