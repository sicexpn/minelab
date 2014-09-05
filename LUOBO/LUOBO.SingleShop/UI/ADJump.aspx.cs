using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LUOBO.SingleShop.UI
{
    public partial class ADJump : System.Web.UI.Page
    {
        protected string RedURL;
        protected void Page_Load(object sender, EventArgs e)
        {
            RedURL = Request.QueryString["RedURL"];
        }
    }
}