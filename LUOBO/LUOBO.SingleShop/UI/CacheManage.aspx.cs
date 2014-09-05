using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LUOBO.BLL;

namespace LUOBO.SingleShop.UI
{
    public partial class CacheManage : System.Web.UI.Page
    {
        BLL_CacheManage cm = new BLL_CacheManage();
        List<string> list = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            list = cm.GetAllCacheKey();
            list.Add("aasdasd1");
            list.Add("aasdasd2");
            list.Add("aasdasd3");
            gvCacheList.DataSource = list.Select(c => new { Name = c });
            gvCacheList.DataBind();
        }

        protected void gvCacheList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            string key = list[rowIndex];
            if (cm.RemoveOneCache(key))
            {
                Response.Write("<Script Language='JavaScript'>alert('清除成功');</script>");
            }
            else
            {
                Response.Write("<Script Language='JavaScript'>alert('清除失败');</script>");
            }
        }
    }
}