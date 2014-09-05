<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="LUOBO.SingleShop.UI.test" validateRequest="false"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../UITemplet/css/bootstrap.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/css/font-awesome.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/css/index.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../UITemplet/css/style.css" media="all" rel="stylesheet" type="text/css" />
    <script src="../UITemplet/js/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../UITemplet/js/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script type="text/javascript" src="../UITemplet/js/Common.js"></script>
    <script src="../UITemplet/js/bootstrap.js" type="text/javascript"></script>
    <script src="../UITemplet/js/login.js" type="text/javascript"></script>
    <script src="../UITemplet/js/respond.js" type="text/javascript"></script>
    <link rel="stylesheet" href="../UITemplet/kindeditor/themes/default/default.css" />
	<link rel="stylesheet" href="../UITemplet/kindeditor/plugins/code/prettify.css" />
	<script charset="utf-8" src="../UITemplet/kindeditor/kindeditor.js"></script>
	<script charset="utf-8" src="../UITemplet/kindeditor/lang/zh_CN.js"></script>
	<script charset="utf-8" src="../UITemplet/kindeditor/plugins/code/prettify.js"></script>
	<script>

	    function kindeditcontrol(area, orgid, adid, sid) {
	        var K = KindEditor;
	        var editor1 = K.create('#' + area, {
	            cssPath: '/UITemplet/kindeditor/plugins/code/prettify.css',
	            uploadJson: 'upload.ashx?org_id=' + orgid + "&adid=" + adid + "&sid=" + sid,
	            allowFileManager: false,
	            items: ['source', '|', 'undo', 'redo', 'cut', 'copy', 'paste',
                        'plainpaste', 'wordpaste', '|', 'justifyleft', 'justifycenter', 'justifyright',
                        'justifyfull', 'insertorderedlist', 'insertunorderedlist', 'indent', 'outdent', 'subscript',
                        'superscript', '|', 'selectall', '/',
                        'title', 'fontname', 'fontsize', '|', 'forecolor', 'hilitecolor', 'bold',
                        'italic', 'underline', 'strikethrough', 'removeformat', '|', 'image',
                        'advtable', 'hr', 'emoticons', 'link', 'unlink']
	        });
	        prettyPrint();
	    }
	</script>
</head>
<body>
    <form id="form1" runat="server">
    <div><br><br><br>
    <textarea id="content1" cols="100" rows="8" style="width:700px;height:200px;visibility:hidden;" runat="server"></textarea>
    <input type="button" onclick="kindeditcontrol('content1',10035,0,0);"  value="dddd" />
    </div>
    </form>
</body>
</html>
