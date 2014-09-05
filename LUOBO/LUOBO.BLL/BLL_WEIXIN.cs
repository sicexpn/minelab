using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.Entity;

namespace LUOBO.BLL
{
    public class BLL_WEIXIN
    {
        private DAL.DAL_WEIXIN_AUTH weixinDal = new DAL.DAL_WEIXIN_AUTH();

        public WEIXIN_AUTH SelectByOpenID(Int64 oid, string openid)
        {
            try
            {
                return weixinDal.SelectByOpenID(oid, openid);
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        public bool Update(WEIXIN_AUTH data)
        {
            return weixinDal.Update(data);
        }

        public bool CheckAuth(Int64 oid, string openid, string pwd)
        {
            return weixinDal.CheckAuth(oid, openid, pwd);
        }
    }
}
