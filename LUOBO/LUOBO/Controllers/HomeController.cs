using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LUOBO.BLL;
using LUOBO.Entity;
using LUOBO.Model;

namespace LUOBO.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        BLL_SYS_USER uBll = new BLL_SYS_USER();
        [SupportFilter]
        public ActionResult Default()
        {
            return View();
        }

        [SupportFilter]
        public ActionResult Main()
        {
            return View();
        }

        [SupportFilter]
        public ActionResult Menu()
        {
            return View();
        }

        [SupportFilter]
        public ActionResult Header()
        {
            HttpCookie cookie = Request.Cookies["LUOBO"];
            SYS_USER user = uBll.Select(Convert.ToInt64(cookie.Values["userid"]));
            if(user != null)
                ViewData["username"] = user.USERNAME;

            return View();
        }

        public int Logout()
        {
            Response.Cookies.Clear();
            HttpCookie c = new HttpCookie("LUOBO");
            c.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(c);
            return 1;
        }
    }
}
