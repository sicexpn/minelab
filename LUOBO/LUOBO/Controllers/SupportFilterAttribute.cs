using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LUOBO.Controllers
{
    public class SupportFilterAttribute : ActionFilterAttribute
    {
        //
        // GET: /ActionFilterAttribute/

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string controllerName = (string)filterContext.RouteData.Values["controller"];
            string actionName = (string)filterContext.RouteData.Values["action"];

            if (filterContext.HttpContext.Request.Cookies["LUOBO"] == null)
            {
                filterContext.HttpContext.Response.Redirect(new UrlHelper(filterContext.RequestContext).Action("Default", "Login"));
                filterContext.Result = new EmptyResult();
            }
            else if (filterContext.HttpContext.Request.Cookies["LUOBO"].Values.Count == 0)
            {
                filterContext.HttpContext.Response.Redirect(new UrlHelper(filterContext.RequestContext).Action("Default", "Login"));
                filterContext.Result = new EmptyResult();
            }
        }
    }
}
