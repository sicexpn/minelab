<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>萝卜网络广告营销平台</title>
    
</head>
    <frameset id="frame" framespacing="0" border="false" rows="46,*" frameborder="0">
        <frame name="top"  scrolling="no" marginwidth="0" marginheight="0" src="/Home/Header" noresize  />
        <frameset framespacing="0" border="false" cols="240,*" frameborder="0" >
            <frame name="left" style="width:240px" scrolling="auto" src="/Home/Menu"/>
            <frame name="right" scrolling="auto" src="/Home/Main"/>
        </frameset>
    </frameset>
</html>
