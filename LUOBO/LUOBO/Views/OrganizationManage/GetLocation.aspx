<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>从地图选点</title>

    <link href="../../Content/lhbStyle/stylesheets/bootstrap.min.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../../Content/lhbStyle/stylesheets/font-awesome.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../../Content/lhbStyle/stylesheets/se7en-font.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../../Content/lhbStyle/stylesheets/style.css" media="all" rel="stylesheet" type="text/css" />
    
    <script type="text/javascript" src="/Scripts/Common.js"></script>
    <script type="text/javascript" src="/Scripts/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="../../Content/lhbStyle/javascripts/bootstrap.min.js" ></script>
    <script type="text/javascript" src="http://api.map.baidu.com/api?v=2.0&ak=10de0613056269b007c32743d586d9d7"></script>

    <script type="text/javascript">
        var map;
        var localSearch;
        var marker;
        var infoWindow;
        var content;

        $(function () {
            map = new BMap.Map("container");
            map.centerAndZoom("北京", 12);
            map.enableScrollWheelZoom();
            map.enableContinuousZoom();

            map.addControl(new BMap.NavigationControl());  //添加默认缩放平移控件
            map.addControl(new BMap.OverviewMapControl()); //添加默认缩略地图控件
            map.addControl(new BMap.OverviewMapControl({ isOpen: true, anchor: BMAP_ANCHOR_BOTTOM_RIGHT }));   //右下角，打开
            localSearch = new BMap.LocalSearch(map);
            localSearch.enableAutoViewport(); //允许自动调节窗体大小

            
            infoWindow = new BMap.InfoWindow("");

            marker = new BMap.Marker(new BMap.Point(0, 0), { enableDragging: true });  // 创建标注，为要查询的地方对应的经纬度
            marker.addEventListener("dragend", function () {
                var point = this.getPosition();
                content = document.getElementById("text_").value + "<br/><br/>经度：" + this.point.lng + "<br/>纬度：" + this.point.lat;
                this.openInfoWindow(infoWindow);
              });
            marker.addEventListener("click", function () { this.openInfoWindow(infoWindow); });

        });
        
        function searchByStationName() {
            map.clearOverlays(); //清空原来的标注
            var keyword = document.getElementById("text_").value;
            localSearch.setSearchCompleteCallback(function (searchResult) {
                var poi = searchResult.getPoi(0);
                document.getElementById("result_").value = poi.point.lng + "," + poi.point.lat;
                map.centerAndZoom(poi.point, 13);
                //marker = new BMap.Marker(new BMap.Point(poi.point.lng, poi.point.lat), { enableDragging: true });  // 创建标注，为要查询的地方对应的经纬度
                marker.setPosition(new BMap.Point(poi.point.lng, poi.point.lat));  // 创建标注，为要查询的地方对应的经纬度
                map.addOverlay(marker);
            });
            localSearch.search(keyword);
        } 
    </script>
</head>

<body>
    <div style="width:730px;margin:auto;">   
        要查询的地址：<input id="text_" type="text" value="天安门" style="margin-right:100px;"/>
        查询结果(经纬度)：<input id="result_" type="text" />
        <input type="button" value="查询" onclick="searchByStationName();"/>
        <div id="container" style="position: absolute;margin-top:30px;width:730px;height: 590px;top: 50;border: 1px solid gray;overflow:hidden;">
        </div>
    </div>
</body>
</html>
