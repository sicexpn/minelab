<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="YouKuShows.aspx.cs" Inherits="LUOBO.SingleShop.UI.YouKuShows" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
    <title></title>
    <script type="text/javascript" src="../UITemplet/js/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="../UITemplet/js/Common.js"></script>

    <link href="../UITemplet/css/bootstrap-3.2.min.css" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/css/font-awesome.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/css/index.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/css/sidebar.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/echarts/css/monokai.css" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/echarts/css/codemirror.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        table>tbody>tr>td:hover
        {
            background-color: #f5f5f5;
            cursor:pointer;
        }
        
        .bodyMain
        {
            background: #FFF;
            border-radius: 4px;
            margin: 0 -10px;
            padding: 5px;
            box-shadow: 0 1px 1px rgba(0, 0, 0, 0.15);
        }
        
        .show-grid {
            margin-bottom: 15px;
        }
        
        .div-hover:hover
        {
            border-width: 5px;
            border-color: #66afe9;
            outline: 0;
            -webkit-box-shadow: inset 0 1px 1px rgba(0, 0, 0, 0.175), 0 0 8px rgba(102, 175, 233, 1);
                    box-shadow: inset 0 1px 1px rgba(0, 0, 0, 0.175), 0 0 8px rgba(102, 175, 233, 1);
        }
    </style>

    <script type="text/javascript">
        var category = "全部";
        var count = 21;
        var page = 1;

        $().ready(function () {
            GetShows();

            $(window).scroll(function () {
                if ($(document).scrollTop() > $(window).height()) {
                    $('#flow_to_top').show();
                } else {
                    $('#flow_to_top').hide();
                }
                if ($(document).scrollTop() > ($("#divList").offset().top + $("#divList").height() - $(window).height())) {
                    GetShows();
                }
            });

            $("#tbCategory td").click(function () {
                if ($(this).html() == "更多..") {
                    $("#tbCategory tbody:eq(1)").toggle();
                    return;
                }
                category = $(this).html();
                page = 1;
                GetShows();
            });
        });

        function gotoTop() {
            $("body,html").animate({
                scrollTop: 0
            }, 800);
        }

        function CreateVideoItem_3(item) {
            var result = "";
            if (item != null) {
                result += '<div class="div-body col-xs-4">';
                result += '<div class="bodyMain div-hover" alt="' + item.name + '" title="' + item.name + '" link="' + item.play_link + '">';
                result += '<div><img src="' + item.poster + '" width="100%" height="100%" alt="' + item.name + '" /></div>';
                result += '<div style="display:block;white-space:nowrap; overflow:hidden; text-overflow:ellipsis;">' + item.name + '</div>';
                result += '<div style="color:#ff4400"><b>' + parseFloat(item.score).toFixed(1) + '</b></div>';
                result += '</div>';
                result += '</div>';
            }
            return result;
        }

        function CreateVideoItem_2(item) {
            var result = "";
            if (item != null) {
                result += '<div class="div-body col-xs-6">';
                result += '<div class="bodyMain div-hover" alt="' + item.name + '" title="' + item.name + '" link="' + item.play_link + '">';
                result += '<div><img src="' + item.thumbnail + '" width="100%" alt="' + item.name + '" /></div>';
                result += '<div style="display:block;white-space:nowrap; overflow:hidden; text-overflow:ellipsis;">' + item.name + '</div>';
                result += '<div style="color:#ff4400"><b>' + parseFloat(item.score).toFixed(1) + '</b></div>';
                result += '</div>';
                result += '</div>';
            }
            return result;
        }

        function GetShows() {
            if (page == 1)
                $("#divList").empty();
            $.ajax({
                type: 'post',
                url: 'AjaxComm.aspx',
                data: 'type=GetYouKuShowsCategory&category=' + category + "&count=" + count + "&page=" + page,
                dataType: 'json',
                error: function (msg) {
                },
                success: function (obj) {
                    if (obj.ResultCode == 0) {
                        if (obj.ResultOBJ.shows.length > 0) {
                            page++;
                            var result = '';

                            for (var i = 0; i < obj.ResultOBJ.shows.length; i += 3) {
                                result += '<div class="row show-grid">';
                                result += CreateVideoItem_3(obj.ResultOBJ.shows[i]);
                                result += CreateVideoItem_3(obj.ResultOBJ.shows[i + 1]);
                                result += CreateVideoItem_3(obj.ResultOBJ.shows[i + 2]);
                                result += '</div>';
                            }
                            //for (var i = 0; i < obj.ResultOBJ.shows.length; i += 2) {
                            //    result += '<div class="row show-grid">';
                            //    result += CreateVideoItem_2(obj.ResultOBJ.shows[i]);
                            //    result += CreateVideoItem_2(obj.ResultOBJ.shows[i + 1]);
                            //    result += '</div>';
                            //}

                            $("#divList").append(result);
                            $("#divList").find(".bodyMain").attr("style", "cursor:pointer;").click(function () { window.location.href = $(this).attr("link"); });
                        }
                    }
                    else {
                        alert(obj.ResultMsg);
                    }
                }
            });
        }
    </script>
</head>
<body>
    <div class="container-fluid">
        <div class="row show-grid">
            <div class="div-body col-xs-12">
                <div class="bodyMain" style="min-height: 0px">
                    <table id="tbCategory" class="table table-bordered" style="margin-bottom:0px; text-align:center">
                        <tr>
                            <td style="width:20%">全部</td>
                            <td style="width:20%">电视剧</td>
                            <td style="width:20%">电影</td>
                            <td style="width:20%">综艺</td>
                            <td style="width:20%">更多..</td>
                        </tr>
                        <tbody style="border-top: 0px solid #ddd; display:none;">
                            <tr>
                                <td>动漫</td>
                                <td>音乐</td>
                                <td>教育</td>
                                <td>纪录片</td>
                                <td>体育</td>
                            </tr>
                            <%--<tr>
                                <td>娱乐</td>
                                <td>原创</td>
                                <td>资讯</td>
                                <td>汽车</td>
                                <td>科技</td>
                            </tr>
                            <tr>
                                <td>游戏</td>
                                <td>生活</td>
                                <td>时尚</td>
                                <td>旅游</td>
                                <td>母婴</td>
                            </tr>
                            <tr>
                                <td>搞笑</td>
                                <td>微电影</td>
                                <td>网剧</td>
                                <td>自制栏目</td>
                                <td>拍客</td>
                            </tr>
                            <tr>
                                <td>创意视频</td>
                                <td>自拍</td>
                                <td>广告</td>
                                <td>其他</td>
                            </tr>--%>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
        <div id="divList">
        </div>
    </div>
    <div id="flow_to_top" class="div-hover" style="position:fixed; bottom:50px; right:40px; display:none; background: rgba(255, 255, 255, 0.7); border-radius: 4px;"><a href="javascript:gotoTop();" class="btn"><i class="fa fa-arrow-up fa-2x"></i></a></div>
</body>
</html>
