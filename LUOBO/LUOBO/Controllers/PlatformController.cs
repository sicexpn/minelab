using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using LUOBO.Helper;
using System.Collections;
using System.Text;

namespace LUOBO.Controllers
{
    public class PlatformController : Controller
    {
        BLL.BLL_Platform platformBll = new BLL.BLL_Platform();
        BLL.BLL_APManage apManageBll = new BLL.BLL_APManage();
        FileHelper fileHelper = new FileHelper();
        public ActionResult Index()
        {
            return View();
        }

        public string DeviceSetting(Int64 deviceSerial, string version)
        {
            StringBuilder result = new StringBuilder();
            
            return result.ToString();
        }

        public ActionResult DownloadFile(Int64? deviceSerial, string version, string filename)
        {
            filename = filename.Replace('_', '/');
            var path = Server.MapPath("~/APSettingFile/" + deviceSerial + "/" + version + "/" + filename);
            if (System.IO.File.Exists(path))
                return File(path, filename.Substring(filename.LastIndexOf('/')+1));
            else
            {
                Response.StatusCode = 404;
                return null;
            }
        }

        public string Test()
        {

            Helper.TarHelper help = new Helper.TarHelper();
            //help.CreatTarGzArchive("D:\\Project_VS\\LuoBo\\source\\LUOBO\\LUOBO\\APSettingFile\\", "www");
            help.TarFolder("D:\\Project_VS\\LuoBo\\source\\LUOBO\\LUOBO\\ADUserFile\\10035\\10", "E:\\LuoBo\\APSettingFile\\10043\\AD", "test");
            return "sucess";
        }

    }
}
