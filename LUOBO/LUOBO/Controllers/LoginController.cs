using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LUOBO.BLL;
using LUOBO.Entity;

namespace LUOBO.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        BLL_SYS_USER uBll = new BLL_SYS_USER();
        public ActionResult Default()
        {
            return View();
        }

        public int Login(string ACCOUNT,string PWD)
        {
            int flag = 0;

            SYS_USER user = uBll.Select(ACCOUNT, PWD);
            if (user != null)
            {
                flag = 1;
                HttpCookie cookie = new HttpCookie("LUOBO");
                DateTime dt = DateTime.Now;
                TimeSpan ts = new TimeSpan(0, 12, 0, 0, 0);
                cookie.Expires = dt.Add(ts);//设置过期时间
                cookie.Values.Add("userid", user.ID.ToString());
                cookie.Values.Add("username", user.USERNAME);
                cookie.Values.Add("account", user.ACCOUNT);
                cookie.Values.Add("oid", user.OID.ToString());
                Response.AppendCookie(cookie);
            }

            return flag;
        }
    }
}
