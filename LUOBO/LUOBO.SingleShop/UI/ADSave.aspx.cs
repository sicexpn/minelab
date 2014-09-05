using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LUOBO.Entity;
using LUOBO.BLL;
using System.Configuration;
using LUOBO.Helper;
using System.Text.RegularExpressions;

namespace LUOBO.SingleShop.UI
{
    public partial class ADSave : System.Web.UI.Page
    {
        BLL_ADManage adBll = new BLL_ADManage();
        BLL_SYS_USER uBll = new BLL_SYS_USER();
        private String ADTempletPath_WEB = ConfigurationSettings.AppSettings["ADTempletPath_WEB"];
        private String ADTempletPath_File = ConfigurationSettings.AppSettings["ADTempletPath_File"];
        private String UserADPath_WEB = ConfigurationSettings.AppSettings["UserADPath_WEB"];
        private String UserADPath_File = ConfigurationSettings.AppSettings["UserADPath_File"];

        protected String resultstr;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Int64 org_id = uBll.SelectByToken(Request.Form["UserToken"]).OID;
                AD_INFO adinfo = adBll.adModify(long.Parse(Request.Form["ad_id"]), org_id, Request.Form["ad_title"], Request.Form["ad_ssid"], int.Parse(Request.Form["ad_model"]), Request.Form["homepage"], int.Parse(Request.Form["ad_type"]), int.Parse(Request.Form["pubcount"]), Request.Form["ad_pubpath"]);
                //String tmpPath = AppDomain.CurrentDomain.BaseDirectory + UserADPath + "/" + org_id;
                String tmpPath = UserADPath_File + org_id;
                if (!System.IO.Directory.Exists(tmpPath))
                {
                    System.IO.Directory.CreateDirectory(tmpPath);
                    System.IO.Directory.CreateDirectory(tmpPath + "/UserPic");
                }
                String tpage = Request.Form["temppage"];
                if (tpage.Length > 0)
                {
                    List<M_ADContentItem> templetitems = new List<M_ADContentItem>();
                    M_ADContentItem item;
                    String tmpS;
                    for (int i = 0; i < Request.Files.Count; ++i)
                    {
                        if (Request.Files[i].FileName.Length > 0)
                        {
                            tmpS = adinfo.AD_ID + "_" + i + "_" + DateTime.Now.ToString("yyMMddHHmmssfff") + Request.Files[i].FileName.Substring(Request.Files[i].FileName.LastIndexOf('.')).ToLower();
                            item = new M_ADContentItem();
                            item.TKey = Request.Files.AllKeys[i];
                            item.TValue = tmpS;
                            templetitems.Add(item);
                            Request.Files[i].SaveAs(tmpPath + "/UserPic/" + tmpS);
                        }
                    }

                    if (!System.IO.Directory.Exists(tmpPath + "/" + adinfo.AD_ID))
                    {
                        //PubFun.CopyDirectory(AppDomain.CurrentDomain.BaseDirectory + AD_TempPath + "/" + adinfo.AD_Model, tmpPath + "/" + adinfo.AD_ID);
                        PubFun.CopyDirectory(ADTempletPath_File + adinfo.AD_Model, tmpPath + "/" + adinfo.AD_ID);
                    }

                    for (int i = 0; i < Request.Form.Count; ++i)
                    {
                        if (Request.Form.AllKeys[i].StartsWith("Templet_"))
                        {
                            item = new M_ADContentItem();
                            item.TKey = Request.Form.AllKeys[i];
                            item.TValue = formatStr(Request.Form[i]);
                            templetitems.Add(item);
                        }
                    }
                    AD_Templet.WriteADFile(tmpPath + "/" + adinfo.AD_ID + "/" + tpage, templetitems, adinfo.AD_Title);
                }

                resultstr = "this.parent.savesuss('" + adinfo.AD_ID.ToString() + "');";
            }
            catch (Exception ex)
            {
                resultstr = "this.parent.saveerr('" + ex.Message + "');";
            }
        }

        private String formatStr(String str)
        {
            if (str == null)
            {
                return "";
            }
            Regex r = new Regex(@"(<!--.*?-->|<script.*?</script>)");
            return r.Replace(str, String.Empty);
        }
    }
}