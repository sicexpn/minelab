<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>
<html>
<head>
<title>nextwifi - admin - Dashboard</title>
<link href="../../Content/lhbStyle/stylesheets/bootstrap.min.css" media="all" rel="stylesheet" type="text/css" />
<link href="../../Content/lhbStyle/stylesheets/font-awesome.css" media="all" rel="stylesheet" type="text/css" />
<link href="../../Content/lhbStyle/stylesheets/se7en-font.css" media="all" rel="stylesheet" type="text/css" />
<link href="../../Content/lhbStyle/stylesheets/isotope.css" media="all" rel="stylesheet" type="text/css" />
<link href="../../Content/lhbStyle/stylesheets/jquery.fancybox.css" media="all" rel="stylesheet" type="text/css" />
<link href="../../Content/lhbStyle/stylesheets/fullcalendar.css" media="all" rel="stylesheet" type="text/css" />
<link href="../../Content/lhbStyle/stylesheets/wizard.css" media="all" rel="stylesheet" type="text/css" />
<link href="../../Content/lhbStyle/stylesheets/select2.css" media="all" rel="stylesheet" type="text/css" />
<link href="../../Content/lhbStyle/stylesheets/morris.css" media="all" rel="stylesheet" type="text/css" />
<link href="../../Content/lhbStyle/stylesheets/datatables.css" media="all" rel="stylesheet" type="text/css" />
<link href="../../Content/lhbStyle/stylesheets/datepicker.css" media="all" rel="stylesheet" type="text/css" />
<link href="../../Content/lhbStyle/stylesheets/timepicker.css" media="all" rel="stylesheet" type="text/css" />
<link href="../../Content/lhbStyle/stylesheets/colorpicker.css" media="all" rel="stylesheet" type="text/css" />
<link href="../../Content/lhbStyle/stylesheets/bootstrap-switch.css" media="all" rel="stylesheet" type="text/css" />
<link href="../../Content/lhbStyle/stylesheets/daterange-picker.css" media="all" rel="stylesheet" type="text/css" />
<link href="../../Content/lhbStyle/stylesheets/typeahead.css" media="all" rel="stylesheet" type="text/css" />
<link href="../../Content/lhbStyle/stylesheets/summernote.css" media="all" rel="stylesheet" type="text/css" />
<link href="../../Content/lhbStyle/stylesheets/pygments.css" media="all" rel="stylesheet" type="text/css" />
<link href="../../Content/lhbStyle/stylesheets/style.css" media="all" rel="stylesheet" type="text/css" />
<script src="../../Content/lhbStyle/javascripts//jquery-1.10.2.min.js" type="text/javascript"></script><script src="../../Content/lhbStyle/javascripts//jquery-ui.js" type="text/javascript"></script><script src="../../Content/lhbStyle/javascripts//bootstrap.min.js" type="text/javascript"></script><script src="../../Content/lhbStyle/javascripts//raphael.min.js" type="text/javascript"></script><script src="../../Content/lhbStyle/javascripts//selectivizr-min.js" type="text/javascript"></script><script src="../../Content/lhbStyle/javascripts//jquery.mousewheel.js" type="text/javascript"></script><script src="../../Content/lhbStyle/javascripts//jquery.vmap.min.js" type="text/javascript"></script>
<script src="../../Content/lhbStyle/javascripts//editableTable.js" type="text/javascript"></script><script src="../../Content/lhbStyle/javascripts//jquery.vmap.sampledata.js" type="text/javascript"></script><script src="../../Content/lhbStyle/javascripts//jquery.vmap.world.js" type="text/javascript"></script><script src="../../Content/lhbStyle/javascripts//jquery.bootstrap.wizard.js" type="text/javascript"></script><script src="../../Content/lhbStyle/javascripts//fullcalendar.min.js" type="text/javascript"></script><script src="../../Content/lhbStyle/javascripts//gcal.js" type="text/javascript"></script><script src="../../Content/lhbStyle/javascripts//jquery.dataTables.min.js" type="text/javascript"></script><script src="../../Content/lhbStyle/javascripts//datatable-editable.js" type="text/javascript"></script><script src="../../Content/lhbStyle/javascripts//jquery.easy-pie-chart.js" type="text/javascript"></script><script src="../../Content/lhbStyle/javascripts//excanvas.min.js" type="text/javascript"></script><script src="../../Content/lhbStyle/javascripts//jquery.isotope.min.js" type="text/javascript"></script><script src="../../Content/lhbStyle/javascripts//isotope_extras.js" type="text/javascript"></script><script src="../../Content/lhbStyle/javascripts//modernizr.custom.js" type="text/javascript"></script><script src="../../Content/lhbStyle/javascripts//jquery.fancybox.pack.js" type="text/javascript"></script><script src="../../Content/lhbStyle/javascripts//select2.js" type="text/javascript"></script><script src="../../Content/lhbStyle/javascripts//styleswitcher.js" type="text/javascript"></script><script src="../../Content/lhbStyle/javascripts//wysiwyg.js" type="text/javascript"></script><script src="../../Content/lhbStyle/javascripts//summernote.min.js" type="text/javascript"></script><script src="../../Content/lhbStyle/javascripts//jquery.inputmask.min.js" type="text/javascript"></script><script src="../../Content/lhbStyle/javascripts//jquery.validate.js" type="text/javascript"></script><script src="../../Content/lhbStyle/javascripts//bootstrap-fileupload.js" type="text/javascript"></script><script src="../../Content/lhbStyle/javascripts//bootstrap-datepicker.js" type="text/javascript"></script><script src="../../Content/lhbStyle/javascripts//bootstrap-timepicker.js" type="text/javascript"></script><script src="../../Content/lhbStyle/javascripts//bootstrap-colorpicker.js" type="text/javascript"></script><script src="../../Content/lhbStyle/javascripts//bootstrap-switch.min.js" type="text/javascript"></script><script src="../../Content/lhbStyle/javascripts//typeahead.js" type="text/javascript"></script><script src="../../Content/lhbStyle/javascripts//daterange-picker.js" type="text/javascript"></script><script src="../../Content/lhbStyle/javascripts//date.js" type="text/javascript"></script><script src="../../Content/lhbStyle/javascripts//morris.min.js" type="text/javascript"></script><script src="../../Content/lhbStyle/javascripts//skycons.js" type="text/javascript"></script><script src="../../Content/lhbStyle/javascripts//fitvids.js" type="text/javascript"></script><script src="../../Content/lhbStyle/javascripts//jquery.sparkline.min.js" type="text/javascript"></script><script src="../../Content/lhbStyle/javascripts//main.js" type="text/javascript"></script><script src="../../Content/lhbStyle/javascripts//respond.js" type="text/javascript"></script>
<meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" name="viewport" />
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
</head>
<body>
<div class="modal-shiftfix"> 
  <!-- Navigation -->
  <div class="navbar navbar-fixed-top scroll-hide">
    <div class="container-fluid top-bar">
      <div class="pull-right">
        <ul class="nav navbar-nav pull-right">
          <li class="dropdown settings hidden-xs"> <a class="dropdown-toggle" data-toggle="dropdown" href="#"><span aria-hidden="true" class="se7en-gear"></span>
            <div class="sr-only"> Settings </div>
            </a>
            <ul class="dropdown-menu">
              <li> <a class="settings-link blue" href="javascript:chooseStyle('none', 30)"><span></span>Blue</a> </li>
              <li> <a class="settings-link green" href="javascript:chooseStyle('green-theme', 30)"><span></span>Green</a> </li>
              <li> <a class="settings-link orange" href="javascript:chooseStyle('orange-theme', 30)"><span></span>Orange</a> </li>
              <li> <a class="settings-link magenta" href="javascript:chooseStyle('magenta-theme', 30)"><span></span>Magenta</a> </li>
              <li> <a class="settings-link gray" href="javascript:chooseStyle('gray-theme', 30)"><span></span>Gray</a> </li>
            </ul>
          </li>
          <li class="dropdown user hidden-xs"><a data-toggle="dropdown" class="dropdown-toggle" href="#"> <img width="34" height="34" src="images/avatar-male.jpg" />系统管理员<b class="caret"></b></a>
            <ul class="dropdown-menu">
              <li><a href="#"> <i class="icon-user"></i>My Account</a> </li>
              <li><a href="#"> <i class="icon-gear"></i>Account Settings</a> </li>
              <li><a href="login1.html"> <i class="icon-signout"></i>Logout</a> </li>
            </ul>
          </li>
        </ul>
      </div>
      <button class="navbar-toggle"><span class="icon-bar"></span><span class="icon-bar"></span><span class="icon-bar"></span></button>
      <a class="logo" href="index-dandian.html">nextwifi</a>
      <form class="navbar-form form-inline col-lg-2 hidden-xs">
        <input class="form-control" placeholder="Search" type="text">
      </form>
    </div>
    <div class="container-fluid main-nav clearfix">
      <div class="nav-collapse">
        <ul class="nav">
          <li> <a class="current" href="index.html"><span aria-hidden="true" class="se7en-home"></span>用户中心</a> </li>
          <li><a href="ad_moban.html"> <span aria-hidden="true" class="se7en-feed"></span>广告管理</a> </li>
          <li class="dropdown"><a data-toggle="dropdown" href="#"> <span aria-hidden="true" class="se7en-star"></span>设备管理<b class="caret"></b></a>
            
          </li>
          <li class="dropdown"><a data-toggle="dropdown" href="#"> <span aria-hidden="true" class="se7en-forms"></span>ssid管理<b class="caret"></b></a>
            
          </li>
          <li class="dropdown"><a data-toggle="dropdown" href="#"> <span aria-hidden="true" class="se7en-tables"></span>模板管理<b class="caret"></b></a>
            
          </li>
          <li><a href="charts.html"> <span aria-hidden="true" class="se7en-charts"></span>统计分析</a> </li>
          <li class="dropdown"><a data-toggle="dropdown" href="#"> <span aria-hidden="true" class="se7en-pages"></span>广告交换<b class="caret"></b></a>
            
          </li>
          <!--              <li><a href="gallery.html">
                <span aria-hidden="true" class="se7en-gallery"></span>Gallery</a>
              </li>-->
        </ul>
      </div>
    </div>
  </div>
  <!-- End Navigation -->
  <div class="container-fluid main-content">
      <!-- Statistics -->
    <div class="row">
      <div class="col-lg-12">
        <div class="widget-container stats-container">
          <div class="col-md-3">
            <div class="number">
              <div class="icon globe"></div>
              86<small>%</small> </div>
            <div class="text"> 设备活跃度 </div>
          </div>
          <div class="col-md-3">
            <div class="number">
              <div class="icon visitors"></div>
              613 </div>
            <div class="text"> 访问量 </div>
          </div>
          <div class="col-md-3">
            <div class="number">
              <div class="icon money"></div>
              <small>$</small>924 </div>
            <div class="text"> 促进营业额 </div>
          </div>
          <div class="col-md-3">
            <div class="number">
              <div class="icon chat-bubbles"></div>
              325 </div>
            <div class="text"> 新用户 </div>
          </div>
        </div>
      </div>
    </div>
    <!-- End Statistics -->
    <div class="row">
      <div class="col-lg-12">
        <div class="widget-container fluid-height clearfix">
          <div class="heading"> <i class="icon-table"></i>广告列表 </div>
          <div class="widget-content padded clearfix">
            <table class="table table-bordered table-striped editableTable" id="dataTable1">
              <thead>
              <th> 名称 </th>
                <th> SSID名称 </th>
                <th class="hidden-xs"> 模版 </th>
                <th class="hidden-xs"> 用户 </th>
                <th class="hidden-xs"> 修改时间 </th>
                <th class="hidden-xs"> 状态 </th>
                <th></th>
                  </thead>
              <tbody>
                <tr>
                  <td class="editable"> 广告1 </td>
                  <td class="editable"> 欢迎使用欢迎使用欢迎使用 <i class="icon-pencil"></i></td>
                  <td class="hidden-xs"> 模板1 </td>
                  <td class="hidden-xs"> 用户1 </td>
                  <td class="hidden-xs"> 2014-03-26 </td>
                  <td class="hidden-xs"><span class="label label-success">正常</span></td>
                  <td class="actions"><div class="action-buttons"> <a class="table-actions" data-toggle="modal" href="#myModal"><i class="icon-eye-open"></i></a><a class="table-actions" href="#"><i class="icon-pencil"></i></a></div>

                    <div class="modal fade" id="myModal">
                      <div class="modal-dialog">
                        <div class="modal-content">
                          <div class="modal-header">
                            <button aria-hidden="true" class="close" data-dismiss="modal" type="button">&times;</button>
                            <h4 class="modal-title"> 预览 </h4>
                          </div>
                          <div class="modal-body">
                            <h1> Welcome </h1>
                            <iframe width="100%" height="800" src="404-page.html"></iframe>
                          </div>
                          <div class="modal-footer">
                            <button class="btn btn-primary" type="button">Save Changes</button>
                            <button class="btn btn-default-outline" data-dismiss="modal" type="button">Close</button>
                          </div>
                        </div>
                      </div>
                    </div></td>
                </tr>
                <tr>
                  <td class="editable"> 广告1 </td>
                  <td><i class="icon-signal"></i> 欢迎使用 </td>
                  <td class="hidden-xs"> 模板1 </td>
                  <td class="hidden-xs"> 用户1 </td>
                  <td class="hidden-xs"> 2014-03-26 </td>
                  <td class="hidden-xs"><span class="label label-danger">禁用</span></td>
                  <td class="actions"><div class="action-buttons"> <a class="table-actions" data-toggle="modal" href="#myModal"><i class="icon-eye-open"></i></a><a class="table-actions" href="#"><i class="icon-pencil"></i></a></div></td>
                </tr>
                <tr>
                  <td class="editable"> 广告1 </td>
                  <td><i class="icon-signal"></i> 欢迎使用 </td>
                  <td class="hidden-xs"> 模板1 </td>
                  <td class="hidden-xs"> 用户1 </td>
                  <td class="hidden-xs"> 2014-03-26 </td>
                  <td class="hidden-xs"><span class="label label-warning">审核中</span></td>
                  <td class="actions"><div class="action-buttons"> <a class="table-actions" data-toggle="modal" href="#myModal"><i class="icon-eye-open"></i></a><a class="table-actions" href="#"><i class="icon-pencil"></i></a></div></td>
                </tr>
                <tr>
                  <td class="editable"> 广告1 </td>
                  <td><i class="icon-signal"></i> 欢迎使用 </td>
                  <td class="hidden-xs"> 模板1 </td>
                  <td class="hidden-xs"> 用户1 </td>
                  <td class="hidden-xs"> 2014-03-26 </td>
                  <td class="hidden-xs"><span class="label label-success">正常</span></td>
                  <td class="actions"><div class="action-buttons"> <a class="table-actions" data-toggle="modal" href="#myModal"><i class="icon-eye-open"></i></a><a class="table-actions" href="#"><i class="icon-pencil"></i></a></div></td>
                </tr>
                <tr>
                  <td class="editable"> 广告1 </td>
                  <td><i class="icon-signal"></i> 欢迎使用 </td>
                  <td class="hidden-xs"> 模板1 </td>
                  <td class="hidden-xs"> 用户1 </td>
                  <td class="hidden-xs"> 2014-03-26 </td>
                  <td class="hidden-xs"><span class="label label-success">正常</span></td>
                  <td class="actions"><div class="action-buttons"> <a class="table-actions" data-toggle="modal" href="#myModal"><i class="icon-eye-open"></i></a><a class="table-actions" href="#"><i class="icon-pencil"></i></a></div></td>
                </tr>
                <tr>
                  <td class="editable"> 广告1 </td>
                  <td><i class="icon-signal"></i> 欢迎使用 </td>
                  <td class="hidden-xs"> 模板1 </td>
                  <td class="hidden-xs"> 用户1 </td>
                  <td class="hidden-xs"> 2014-03-26 </td>
                  <td class="hidden-xs"><span class="label label-success">正常</span></td>
                  <td class="actions"><div class="action-buttons"> <a class="table-actions" data-toggle="modal" href="#myModal"><i class="icon-eye-open"></i></a><a class="table-actions" href="#"><i class="icon-pencil"></i></a></div></td>
                </tr>
                <tr>
                  <td class="editable"> 广告1 </td>
                  <td><i class="icon-signal"></i> 欢迎使用 </td>
                  <td class="hidden-xs"> 模板1 </td>
                  <td class="hidden-xs"> 用户1 </td>
                  <td class="hidden-xs"> 2014-03-26 </td>
                  <td class="hidden-xs"><span class="label label-success">正常</span></td>
                  <td class="actions"><div class="action-buttons"> <a class="table-actions" data-toggle="modal" href="#myModal"><i class="icon-eye-open"></i></a><a class="table-actions" href="#"><i class="icon-pencil"></i></a></div></td>
                </tr>
                <tr>
                  <td class="editable"> 广告1 </td>
                  <td><i class="icon-signal"></i> 欢迎使用 </td>
                  <td class="hidden-xs"> 模板1 </td>
                  <td class="hidden-xs"> 用户1 </td>
                  <td class="hidden-xs"> 2014-03-26 </td>
                  <td class="hidden-xs"><span class="label label-success">正常</span></td>
                  <td class="actions"><div class="action-buttons"> <a class="table-actions" data-toggle="modal" href="#myModal"><i class="icon-eye-open"></i></a><a class="table-actions" href="#"><i class="icon-pencil"></i></a></div></td>
                </tr>
                <tr>
                  <td class="editable"> 广告1 </td>
                  <td><i class="icon-signal"></i> 欢迎使用 </td>
                  <td class="hidden-xs"> 模板1 </td>
                  <td class="hidden-xs"> 用户1 </td>
                  <td class="hidden-xs"> 2014-03-26 </td>
                  <td class="hidden-xs"><span class="label label-success">正常</span></td>
                  <td class="actions"><div class="action-buttons"> <a class="table-actions" data-toggle="modal" href="#myModal"><i class="icon-eye-open"></i></a><a class="table-actions" href="#"><i class="icon-pencil"></i></a></div></td>
                </tr>
                <tr>
                  <td class="editable"> 广告1 </td>
                  <td><i class="icon-signal"></i> 欢迎使用 </td>
                  <td class="hidden-xs"> 模板1 </td>
                  <td class="hidden-xs"> 用户1 </td>
                  <td class="hidden-xs"> 2014-03-26 </td>
                  <td class="hidden-xs"><span class="label label-success">正常</span></td>
                  <td class="actions"><div class="action-buttons"> <a class="table-actions" data-toggle="modal" href="#myModal"><i class="icon-eye-open"></i></a><a class="table-actions" href="#"><i class="icon-pencil"></i></a></div></td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>
      <!--<div class="col-lg-3" name="ad_view" id="ad_view">
        <div class="widget-container" style="height: 645px;">
          <div class="heading"> <i class="icon-camera"></i>预览 </div>
          <div class="widget-content padded">
            <div class="ad_view_div" id=""> <img src="images/image-iso.png" style="width: 100%;height: auto;"> </div>
          </div>
        </div>
      </div>-->
    
    </div>
  </div>
</div>
</body>
</html>