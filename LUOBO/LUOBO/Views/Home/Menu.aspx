<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Menu</title>
    <script src="../../Scripts/Common.js" type="text/javascript"></script>

    <link href="../../Content/Site.css" rel="stylesheet" type="text/css" />

    <link href="../../Content/lhbStyle/stylesheets/bootstrap.min.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../../Content/lhbStyle/stylesheets/font-awesome.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../../Content/lhbStyle/stylesheets/se7en-font.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../../Content/lhbStyle/stylesheets/style.css" media="all" rel="stylesheet" type="text/css" />
    <script src="../../Content/lhbStyle/javascripts/bootstrap.min.js" type="text/javascript"></script>
    <script src="../../Content/lhbStyle/javascripts/jquery.mousewheel.js" type="text/javascript"></script>
    <script src="../../Content/lhbStyle/javascripts/jquery.dataTables.min.js" type="text/javascript"></script>
<!--<script src="../../Content/lhbStyle/javascripts/bootstrap-editable.min.js" type="text/javascript"></script>
    <script src="../../Content/lhbStyle/javascripts/xeditable-demo.js" type="text/javascript"></script>-->
    <script src="../../Scripts/Common.js" type="text/javascript"></script>
    <link rel="stylesheet" href="../../Scripts/jquery-ui.css">
    <script src="../../Scripts/jquery-1.10.2.js"></script>
    <script src="../../Scripts/jquery-ui.js"></script>

</head>
<body>
  <div class="navbar navbar-fixed-top scroll-hide">
    
    <div class="container-fluid main-nav clearfix">
      <div class="nav-collapse">
        <ul class="nav">
         <li class="dropdown open"><a href="/MenuManage/MenuList" target="right"> <span aria-hidden="true" class="se7en-star"></span>菜单管理<b class="caret"></b></a>
          <ul class="dropdown-menu">
              <li> <a href="/MenuManage/MenuList" target="right"><span aria-hidden="true" class="se7en-star"> </span> 菜单管理</a> </li>
              <li> <a href="/MenuManage/MenuItem" target="right"><span aria-hidden="true" class="se7en-star"> </span> 菜单添加</a> </li>
              <li> <a href="/MenuManage/MenuAllocation" target="right"><span aria-hidden="true" class="se7en-star"> </span> 菜单分配</a> </li>
            </ul>
         </li>
         <li class="dropdown open"><a href="/APManage/APManage" target="right"> <span aria-hidden="true" class="se7en-star"></span>AP管理<b class="caret"></b></a>
          <ul class="dropdown-menu">
              <li> <a href="/APManage/APManage" target="right"><span aria-hidden="true" class="se7en-star"> </span> AP管理</a> </li>
              <li><a href="/APManage/APCTManage" target="right"><span aria-hidden="true" class="se7en-star"> </span> 配置模版管理</a> </li>
              <li> <a href="/APManage/APDistribution" target="right"><span aria-hidden="true" class="se7en-star"> </span> AP分配</a> </li>
              <li> <a href="/APManage/APBack" target="right"><span aria-hidden="true" class="se7en-star"> </span> AP回收</a> </li>
              <li> <a href="/SSIDManage/Default" target="right"><span aria-hidden="true" class="se7en-star"> </span> SSID管理</a> </li>
              <li> <a href="/SSIDManage/SSIDAUDIT" target="right"><span aria-hidden="true" class="se7en-star"> </span> SSID审核</a> </li>
            </ul>
         </li>
         <%--<li class="dropdown open"><a href="/ProbeManage/ProbeManage" target="right"> <span aria-hidden="true" class="se7en-star"></span>探测设备管理<b class="caret"></b></a>
          <ul class="dropdown-menu">
              <li> <a href="/ProbeManage/ProbeManage" target="right"><span aria-hidden="true" class="se7en-star"> </span> 探测设备管理</a> </li>
              <li> <a href="/ProbeManage/ProbeDistribution" target="right"><span aria-hidden="true" class="se7en-star"> </span> 探测设备分配</a> </li>
              <li> <a href="/ProbeManage/ProbeBack" target="right"><span aria-hidden="true" class="se7en-star"> </span> 探测设备回收</a> </li>
            </ul>
         </li>--%>
         <li class="dropdown open"><a  href="/ADManage/ADListEdit"  target="right"> <span aria-hidden="true" class="se7en-feed"></span>广告管理<b class="caret"></b></a>
          <ul class="dropdown-menu">
              <li> <a href="/ADManage/ADList" target="right"><span aria-hidden="true" class="se7en-feed"></span> 广告管理</a> </li>
              <li> <a href="/ADManage/ADEdit?ad_id=0" target="right"><span aria-hidden="true" class="se7en-feed"></span> 新建广告</a> </li>
              <li><a href="/ADManage/ADPubHistory" target="right"><span aria-hidden="true" class="se7en-feed"></span> 广告发布历史</a> </li>
            </ul>
         </li>
         <li class="dropdown open"><a href="/AppManage/Default"  target="right"> <span aria-hidden="true" class="se7en-flag"></span>应用管理<b class="caret"></b></a>
          <ul class="dropdown-menu">
              <li> <a href="/AppManage/Default" target="right"><span aria-hidden="true" class="se7en-flag"></span> 应用权限管理</a> </li>
              <li> <a href="/AppManage/Authorize" target="right"><span aria-hidden="true" class="se7en-flag"></span> 应用权限设置</a> </li>
            </ul>
         </li>
         <li class="dropdown open"><a href="/UsersManage/Default"  target="right"> <span aria-hidden="true" class="se7en-flag"></span>用户管理<b class="caret"></b></a>
          <ul class="dropdown-menu">
              <li> <a href="/UsersManage/Default" target="right"><span aria-hidden="true" class="se7en-flag"></span> 用户管理</a> </li>
              <li> <a href="/UsersManage/Search" target="right"><span aria-hidden="true" class="se7en-flag"></span> 用户查询</a> </li>
            </ul>
         </li>
         <li class="dropdown open"><a href="/OrganizationManage/OrgManage"  target="right"> <span aria-hidden="true" class="se7en-flag"></span>机构管理</a>
          <ul class="dropdown-menu">
              <li> <a href="/OrganizationManage/OrgManage" target="right"><span aria-hidden="true" class="se7en-flag"></span> 机构管理</a> </li>
              <li> <a href="/OrganizationManage/OrgPropExpend" target="right"><span aria-hidden="true" class="se7en-flag"></span> 机构扩展属性管理</a> </li>
            </ul>
         </li>
         <li class="dropdown open"><a href="/UsersManage/Default"  target="right"> <span aria-hidden="true" class="se7en-flag"></span>广告审核<b class="caret"></b></a>
          <ul class="dropdown-menu">
              <li> <a href="/AuditManage/AuditList" target="right"><span aria-hidden="true" class="se7en-flag"></span> 待审核广告</a> </li>
              <li> <a href="/AuditManage/AuditHistory" target="right"><span aria-hidden="true" class="se7en-flag"></span> 已审核广告</a> </li>
            </ul>
         </li>
          
          <li class="open"><a href="#"> <span aria-hidden="true" class="se7en-tables"  target="right"></span>模版管理</a> </li>
          <li class="open"><a href="#"> <span aria-hidden="true" class="se7en-charts"  target="right"></span>统计分析</a> </li>
         <li ><a href="#"> <span aria-hidden="true" class="se7en-pages"  target="right"></span>广告交换</a> </li>

        </ul>
      </div>
    </div>
</div>
</body>
</html>
