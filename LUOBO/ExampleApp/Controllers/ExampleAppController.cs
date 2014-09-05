using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExampleApp.Controllers
{
    [HandleError]
    public class ExampleAppController : Controller
    {
        public ActionResult Default()
        {
            ViewData["Message"] = "欢迎使用 ASP.NET MVC! ";

            return View();
        }
    }
}
