using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using LUOBO.BLL;
using LUOBO.Entity;

namespace LUOBO.BusinessService
{
    // TODO: Edit the SampleItem class
    public class PubFun
    {
        BLL_SYS_USER uBll = new BLL_SYS_USER();

        public bool ValidationTokenTimeout(string token, string auth)
        {
            SYS_USER user = uBll.SelectByToken(token, auth);

            if (user != null)
            {
                if (user.TOKENTIMESTAMP >= DateTime.Now)
                {
                    if ((user.TOKENTIMESTAMP - DateTime.Now).Hours <= 1)
                    {
                        user.TOKENTIMESTAMP = DateTime.Now.AddHours(12);
                        uBll.Update(user);
                    }

                    return true;
                }
            }

            return false;
        }

        public bool ValidationAppCompetenc(string token)
        {
            return true;
        }
    }
}
