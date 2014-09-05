using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using LitJson;
using System.IO;
using System.Globalization;
using LUOBO.BLL;

namespace LUOBO.SingleShop.UI
{
    /// <summary>
    /// upload 的摘要说明
    /// </summary>
    public class upload : IHttpHandler
    {
        BLL_SYS_USER uBll = new BLL_SYS_USER();
        private HttpContext context;

        public void ProcessRequest(HttpContext context)
        {
            this.context = context;

            String token = context.Request.QueryString["token"];
            if (String.IsNullOrEmpty(token))
            {
                showError("身份信息丢失。");
            }
            String org_id = uBll.SelectByToken(token).OID.ToString();
            String adid = context.Request.QueryString["adid"];
            String sid = context.Request.QueryString["sid"];
            if (String.IsNullOrEmpty(adid))
            {
                adid = "0";
            } 
            if (String.IsNullOrEmpty(sid))
            {
                sid = "0";
            }

            String aspxUrl = context.Request.Path.Substring(0, context.Request.Path.LastIndexOf("/") + 1);

            //文件保存目录路径
            String savePath = "../ADUserFile/";

            //文件保存目录URL
            String saveUrl = aspxUrl + "../ADUserFile/";

            //定义允许上传的文件扩展名
            Hashtable extTable = new Hashtable();
            extTable.Add("image", "gif,jpg,jpeg,png,bmp");
            extTable.Add("flash", "swf,flv");
            extTable.Add("media", "swf,flv,mp3,wav,wma,wmv,mid,avi,mpg,asf,rm,rmvb");
            extTable.Add("file", "doc,docx,xls,xlsx,ppt,htm,html,txt,zip,rar,gz,bz2");

            //最大文件大小
            int maxSize = 1000000;

            HttpPostedFile imgFile = context.Request.Files["imgFile"];
            if (imgFile == null)
            {
                showError("请选择文件。");
            }

            String dirPath = context.Server.MapPath(savePath);
            if (!Directory.Exists(dirPath))
            {
                showError("上传目录不存在。");
            }

            String dirName = context.Request.QueryString["dir"];
            if (String.IsNullOrEmpty(dirName))
            {
                dirName = "image";
            }
            if (!extTable.ContainsKey(dirName))
            {
                showError("目录名不正确。");
            }

            String fileName = imgFile.FileName;
            String fileExt = Path.GetExtension(fileName).ToLower();

            if (imgFile.InputStream == null || imgFile.InputStream.Length > maxSize)
            {
                showError("上传文件大小超过限制。");
            }

            if (String.IsNullOrEmpty(fileExt) || Array.IndexOf(((String)extTable[dirName]).Split(','), fileExt.Substring(1).ToLower()) == -1)
            {
                showError("上传文件扩展名是不允许的扩展名。\n只允许" + ((String)extTable[dirName]) + "格式。");
            }

            //创建文件夹
            dirPath += org_id + "/";
            saveUrl += org_id + "/";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            dirPath += "UserPic/";
            saveUrl += "UserPic/";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }


            String newFileName = adid + "_" + sid + "_" + DateTime.Now.ToString("yyMMddHHmmssfff", DateTimeFormatInfo.InvariantInfo) + fileExt;
            String filePath = dirPath + newFileName;

            imgFile.SaveAs(filePath);

            String fileUrl = saveUrl + newFileName;

            Hashtable hash = new Hashtable();
            hash["error"] = 0;
            hash["url"] = fileUrl;
            context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
            context.Response.Write(JsonMapper.ToJson(hash));
            context.Response.End();
        }

        private void showError(string message)
        {
            Hashtable hash = new Hashtable();
            hash["error"] = 1;
            hash["message"] = message;
            context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
            context.Response.Write(JsonMapper.ToJson(hash));
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}