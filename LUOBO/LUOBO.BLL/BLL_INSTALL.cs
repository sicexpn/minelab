using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.Model;
using LUOBO.Entity;

namespace LUOBO.BLL
{
    public class BLL_INSTALL
    {
        //DAL.DAL_SYS_INSTALLPERSON installperson_dal = new DAL.DAL_SYS_INSTALLPERSON();
        DAL.DAL_SYS_USER userDal = new DAL.DAL_SYS_USER();
        DAL.DAL_SYS_APDEVICE ap_dal = new DAL.DAL_SYS_APDEVICE();

        public M_INSTALLCHECK InstallCheck(Int64 ssid, string mac)
        {
            SYS_USER user = userDal.CheckInstall(mac);
            if (user == null)
                return null;
            user.TOKENTIMESTAMP = DateTime.Now.AddHours(1);
            user.TOKEN = Guid.NewGuid().ToString("N");
            userDal.Update(user);

            SYS_AP_VIEW ap_view = ap_dal.SelectAPViewBySSID(ssid);

            M_INSTALLCHECK installcheck = new M_INSTALLCHECK();
            installcheck.ACCOUNT = user.ACCOUNT;
            installcheck.APMAC = ap_view.MAC;
            installcheck.APOID = ap_view.OID;
            installcheck.ONAME = ap_view.NAME;
            installcheck.UOID = user.OID;
            installcheck.USERNAME = user.USERNAME;
            installcheck.TOKEN = user.TOKEN;
            return installcheck;
        }
    }
}
