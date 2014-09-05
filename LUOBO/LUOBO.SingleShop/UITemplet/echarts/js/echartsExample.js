var myChart = [];
//var domCode = $("[md='sidebar-code']");
//var domGraphic = $("[md='graphic']");
var domMain = $("[md='main']");
var domMessage = $("[md='wrong-message']");
//var iconResize = $("[md='icon-resize']");
var needRefresh = false;

//function findIdxFromEvent(event) {
//    event = event || window.event;
//    return findIdx(event.target || event.srcElement);
//}
//function findIdx(d) {
//    var p = d;
//    while (p.className != 'container-fluid') {
//        p = p.parentElement;
//    }
//    return $(p).attr('idx');
//}
var idx;
//function autoResize(event) {
//    idx = findIdxFromEvent(event);
//    if (iconResize[idx].className == 'icon-resize-full') {
//        focusCode();
//        iconResize[idx].className = 'icon-resize-small';
//    }
//    else {
//        focusGraphic();
//        iconResize[idx].className = 'icon-resize-full';
//    }
//}

//function focusCode() {
//    domCode[idx].className = 'span8 ani';
//    domGraphic[idx].className = 'span4 ani';
//}

function focusGraphic() {
    //domCode[idx].className = 'span4 ani';
    //domGraphic[idx].className = 'span8 ani';
    if (needRefresh) {
        myChart[idx].showLoading();
        setTimeout(refresh, 1000);
    }
}

//var domTextarea = $("[md='code']");
//var editor = [];
//for (var i = 0, l = domTextarea.length; i < l; i++) {
//    editor[i] = CodeMirror.fromTextArea(
//        domTextarea[i],
//        { lineNumbers: true }
//    );
//    editor[i].setOption("theme", 'monokai');
//    editor[i].on('change', function () { needRefresh = true; });
//}

function refresh(isBtnRefresh, idd) {
    if (isBtnRefresh) {
        idx = idd;
        needRefresh = true;
        focusGraphic();
        return;
    }
    needRefresh = false;
    if (myChart[idx] && myChart[idx].dispose) {
        myChart[idx].dispose();
    }
    myChart[idx] = echarts.init(domMain[idx]);
//    (new Function(editor[idx].doc.getValue().replace(
//        'option', 'option[' + idx + ']'))
//    )()
    myChart[idx].setOption(options[idx], true);
//    if (domMessage.length > 0)
//        domMessage[idx].innerHTML = '';
}

function showLoadings(reIndex) {
    if (reIndex)
        for (var i = 0, l = reIndex.length; i < l; i++)
            myChart[reIndex[i]].showLoading();
    else
        for (var i = 0, l = myChart.length; i < l; i++)
            myChart[i].showLoading();
}

function refreshAll(reIndex) {
    if (reIndex) {
        for (var i = 0, l = reIndex.length; i < l; i++) {
            myChart[reIndex[i]].setOption(options[reIndex[i]], true);
            myChart[reIndex[i]].hideLoading();
        }
    }
    else {
        for (var i = 0, l = myChart.length; i < l; i++) {
            //        (new Function(editor[i].doc.getValue().replace(
            //            'option', 'option[' + i + ']'))
            //        )();
            myChart[i].setOption(options[i], true);
            //        if (domMessage.length > 0)
            //            domMessage[i].innerHTML = '';
            myChart[i].hideLoading();
        }
    }
}

var developMode = false;
if (developMode) {
    // for develop
    require.config({
        packages: [
            {
                name: 'echarts',
                location: '../../src',
                main: 'echarts'
            },
            {
                name: 'zrender',
                //location: 'http://ecomfe.github.io/zrender/src',
                location: '../../../zrender/src',
                main: 'zrender'
            }
        ]
    });
}
else {
    // for echarts online home page
    var fileLocation = '../UITemplet/echarts/js/echarts-map';
    require.config({
        paths: {
            echarts: fileLocation,
            'echarts/chart/line': fileLocation,
            'echarts/chart/bar': fileLocation,
            'echarts/chart/scatter': fileLocation,
            'echarts/chart/k': fileLocation,
            'echarts/chart/pie': fileLocation,
            'echarts/chart/radar': fileLocation,
            'echarts/chart/map': fileLocation,
            'echarts/chart/chord': fileLocation,
            'echarts/chart/force': fileLocation
        }
    });
}

// 按需加载
require(
    [
        'echarts',
        'echarts/chart/line',
        'echarts/chart/bar',
        'echarts/chart/scatter',
        'echarts/chart/k',
        'echarts/chart/pie',
        'echarts/chart/radar',
        'echarts/chart/force',
        'echarts/chart/chord',
        'echarts/chart/map'
    ],
    requireCallback
);

