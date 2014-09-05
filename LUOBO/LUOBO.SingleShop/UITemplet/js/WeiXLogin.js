function WeiXLogin(Option) {
    var Search = decodeURIComponent(decodeURIComponent(window.location.search));
    var Type = 0;
    if (Search.lastIndexOf("http://192.155.100.100/?type=weixin") > -1)
        Type = 3;
    else
        return;
    var OpenID = GetQueryString("openid");
    var OID = GetQueryString("oid");
    var S = GetQueryString("s");

    $.ajax({
        type: 'post', //可选get
        url: '/UI/AjaxComm.aspx', //这里是接收数据的PHP程序
        data: 'type=Weixin/Auth&param={openid:"' + OpenID + '",oid:' + OID + ',s:"' + S + '"}',
        dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
        success: function (obj) {
            if (obj.ResultCode == 0)
                Option.CallBack();
            else
                alert(obj.ResultMsg);
        }
    });

    function GetQueryString(name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        r = decodeURIComponent(search).substr(1).match(reg);
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
}