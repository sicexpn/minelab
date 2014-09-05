function InitPeopleChart(_token, _apid) {
    token = _token;
    apid = _apid;

    clearInterval(timeTicket);
    timeTicket = setInterval(StatisticalPeople, time);
}

var token;
var apid;
var timeTicket;
var lineNum = 60;
var time = 5000;
var echarts;
var options = [];
var optionPeople = {
    grid: {
        x2: 100,
        y2: 30,
        x: 50,
        y: 30
    },
    tooltip: {
        trigger: 'axis'
    },
    legend: {
        data: ['人次数', '用户数', '认证用户数'],
        orient : 'vertical',
        x : 'right',
        y : 'center'
    },
    xAxis: [
        {
            type: 'category',
            boundaryGap: false,
            axisLabel: {
                interval: 'auto',
                rotate: 0
            },
            data: (function () {
                var now = new Date();
                var res = [];
                var len = lineNum;
                while (len--) {
                    res.unshift(now.toTimeString().substr(0, 8));
                    now = new Date(now - time);
                }
                return res;
            })()
        }
    ],
    yAxis: [
        {
            type: 'value',
            scale: true,
            name: '人数',
            boundaryGap: [0.2, 0.2]
        }
    ],
    series: [
        {
            name: '人次数',
            type: 'line',
            smooth: true,
            data: (function () {
                var res = [];
                var len = lineNum;
                while (len--) {
                    res.push(0);
                }
                return res;
            })()
        },
        {
            name: '用户数',
            type: 'line',
            smooth: true,
            data: (function () {
                var res = [];
                var len = lineNum;
                while (len--) {
                    res.push(0);
                }
                return res;
            })()
        },
        {
            name: '认证用户数',
            type: 'line',
            smooth: true,
            data: (function () {
                var res = [];
                var len = lineNum;
                while (len--) {
                    res.push(0);
                }
                return res;
            })()
        }
    ]
};

function StatisticalPeople() {
    $.ajax({
        type: 'post', //可选get
        url: 'AjaxComm.aspx', //这里是接收数据的PHP程序
        data: 'type=StatisticalPeople_Map&token=' + token + '&param={"ID":' + apid + ',"Type":"AP","Mode":"RealTime"}',
        dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
        success: function (obj) {
            myChart[0].addData(obj);
        }
    });
}

function StatisticalPeopleRealTime() {
    optionPeople.xAxis[0].axisLabel.interval = 'auto';
    optionPeople.xAxis[0].axisLabel.rotate = 0;
    optionPeople.xAxis[0].data = (function () {
        var now = new Date();
        var res = [];
        var len = lineNum;
        while (len--) {
            res.unshift(now.toTimeString().substr(0, 8));
            now = new Date(now - time);
        }
        return res;
    })();

    optionPeople.series[0].data = (function () {
        var res = [];
        var len = lineNum;
        while (len--) {
            res.push(0);
        }
        return res;
    })();
    optionPeople.series[1].data = (function () {
        var res = [];
        var len = lineNum;
        while (len--) {
            res.push(0);
        }
        return res;
    })();
    optionPeople.series[2].data = (function () {
        var res = [];
        var len = lineNum;
        while (len--) {
            res.push(0);
        }
        return res;
    })();


    clearInterval(timeTicket);
    refresh(true, 0);
    timeTicket = setInterval(StatisticalPeople, time);
}

function StatisticalPeopleWeek() {
    clearInterval(timeTicket);

    $.ajax({
        type: 'post', //可选get
        url: 'AjaxComm.aspx', //这里是接收数据的PHP程序
        data: 'type=StatisticalPeople_Map&token=' + token + '&param={"ID":' + apid + ',"Type":"AP","Mode":"Week"}',
        dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
        success: function (obj) {
            optionPeople.xAxis[0].data = (function () {
                var now = new Date();
                var res = [];
                var len = 7;
                while (len--) {
                    res.unshift(now.toLocaleDateString().replace(/^\D*/, ''));
                    now = new Date(now - (3600000 * 24));
                }
                return res;
            })();

            optionPeople.series[0].data = obj[0];
            optionPeople.series[1].data = obj[1];
            optionPeople.series[2].data = obj[2];

            optionPeople.xAxis[0].axisLabel.interval = 'auto';
            optionPeople.xAxis[0].axisLabel.rotate = 0;

            refresh(true, 0);
        }
    });
}

function StatisticalPeopleMonth() {
    clearInterval(timeTicket);
    $.ajax({
        type: 'post', //可选get
        url: 'AjaxComm.aspx', //这里是接收数据的PHP程序
        data: 'type=StatisticalPeople_Map&token=' + token + '&param={"ID":' + apid + ',"Type":"AP","Mode":"Month"}',
        dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
        success: function (obj) {
            optionPeople.xAxis[0].data = (function () {
                var now = new Date();
                var res = [];
                var len = 30;
                while (len--) {
                    res.unshift(now.toLocaleDateString().replace(/^\D*/, ''));
                    now = new Date(now - (3600000 * 24));
                }
                return res;
            })();

            optionPeople.series[0].data = obj[0];
            optionPeople.series[1].data = obj[1];
            optionPeople.series[2].data = obj[2];

            optionPeople.xAxis[0].axisLabel.interval = 0;
            optionPeople.xAxis[0].axisLabel.rotate = -30;

            refresh(true, 0);
        }
    });
}

function StatisticalPeopleYear() {
    clearInterval(timeTicket);
    $.ajax({
        type: 'post', //可选get
        url: 'AjaxComm.aspx', //这里是接收数据的PHP程序
        data: 'type=StatisticalPeople_Map&token=' + token + '&param={"ID":' + apid + ',"Type":"AP","Mode":"Year"}',
        dataType: 'json', //服务器返回的数据类型 可选XML ,Json jsonp script html text等
        success: function (obj) {
            optionPeople.xAxis[0].data = (function () {
                var now = new Date();
                var res = [];
                var len = 12;
                while (len--) {
                    res.unshift(now.toLocaleDateString().replace(/^\D*/, '').substr(0, now.toLocaleDateString().replace(/^\D*/, '').indexOf('月') + 1));
                    now = new Date(now - (3600000 * 24 * 30));
                }
                return res;
            })();

            optionPeople.series[0].data = obj[0];
            optionPeople.series[1].data = obj[1];
            optionPeople.series[2].data = obj[2];

            optionPeople.xAxis[0].axisLabel.interval = 'auto';
            optionPeople.xAxis[0].axisLabel.rotate = 0;

            refresh(true, 0);
        }
    });
}

function requireCallback(ec) {
    echarts = ec;
    if (myChart.length > 0) {
        for (var i = 0, l = myChart.length; i < l; i++) {
            myChart[i].dispose && myChart[i].dispose();
        }
    }
    myChart = [];
    for (var i = 0, l = domMain.length; i < l; i++) {
        myChart[i] = echarts.init(domMain[i]);
    }
    options.push(optionPeople);
    refreshAll();

    window.onresize = function () {
        for (var i = 0, l = myChart.length; i < l; i++) {
            myChart[i].resize && myChart[i].resize();
        }
    };
}