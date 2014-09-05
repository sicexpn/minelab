function OpenSSID() {
    var data = new Object();
    data.SSID = ""; //NASID
    data.OID = ""; //机构ID
    data.AdId = ""; //广告ID
    data.AcctSessionId = ""; //SessionId
    data.CalledStationId = ""; //用户设备连接到的设备（比如路由器）Mac地址
    data.CallingStationId = ""; //用户当前使用的设备Mac地址
    data.Title = ""; //标题
    data.PageUrl = ""; //地址
    data.UserAgent = ""; //UserAgent信息

    data.ToString = function () {
        var str = '{';
        str += '"SSID":"' + data.SSID + '",';
        str += '"OID":"' + data.OID + '",';
        str += '"AdId":"' + data.AdId + '",';
        str += '"AcctSessionId":"' + data.AcctSessionId + '",';
        str += '"CalledStationId":"' + data.CalledStationId + '",';
        str += '"CallingStationId":"' + data.CallingStationId + '",';
        str += '"Title":"' + data.Title + '",';
        str += '"PageUrl":"' + data.PageUrl + '",';
        str += '"UserAgent":"' + data.UserAgent + '"';
        str += '}';
        return str;
    }

    //从URL中获取参数值
    //使用方法
    //alert(GetQueryString("参数名1"));
    //alert(GetQueryString("参数名2"));
    //alert(GetQueryString("参数名3"));
    function getQueryString(name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return null;
    }

    data.SSID = getQueryString("nasid");
    var url = window.location.href;
    url = url.match(/\d+[/]Pub[/]\d+_\d+/i);
    if (url != null) {
        var urlArr = url.toString().split("/");
        data.OID = urlArr[0];
        data.AdId = urlArr[2].split("_")[0];
    }
    var fpath = window.location.href;
    if (fpath.indexOf("?") > 0) {
        data.PageUrl = fpath.split("?")[0];
    } else {
        data.PageUrl = fpath;
    }
    data.AcctSessionId = getQueryString("sessionid");
    data.CalledStationId = getQueryString("called");
    data.CallingStationId = getQueryString("mac");
    data.Title = document.title;
    //data.PageUrl = window.location.href;
    data.UserAgent = navigator.userAgent;
    return data;
}

var thispageurldata = new OpenSSID();

function ReportStatisticalFromClick(page, title) {
    thispageurldata.PageUrl = page;
    thispageurldata.Title = title;
    if (thispageurldata.AcctSessionId != "" && thispageurldata.AcctSessionId != null) {
        PostReportStatistical(thispageurldata, page);
    }else{
        this.location.href = page;
    }
}

function ReportStatistical() {
    if (thispageurldata.AcctSessionId != "" && thispageurldata.AcctSessionId != null) {
        PostReportStatistical(thispageurldata, "");
    }
}

function PostReportStatistical(data, page){
    $.ajax({
        type: 'post', //可选get
        url: '/UI/AjaxComm.aspx', //这里是接收数据的PHP程序
        data: 'type=ReportStatistical&param=' + data.ToString(),
        dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            //alert(XMLHttpRequest.responseText);
            if(page != ""){
                document.location.href = page;
            }
        },
        success: function (obj) {
            //做其他操作
            if(page != ""){
                document.location.href = page;
            }
        }
    });
}

ReportStatistical();