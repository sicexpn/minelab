using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.DAL;
using LUOBO.Entity;
using System.Transactions;

namespace LUOBO.BLL
{
    public class BLL_SHARE_INFO
    {
        DAL_SHARE share = new DAL_SHARE();
        DAL_SHARE_INFO share_info = new DAL_SHARE_INFO();

        public bool ShareCount(SHARE_INFO info)
        {
            bool flag = false;

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    SHARE s = share.Select(info.SSID, info.OID, info.ADID);
                    if (s == null)
                    {
                        s = new SHARE() {
                            ID = -1,
                            ADID = info.ADID,
                            OID = info.OID,
                            SSID = info.SSID,
                            TITLE = info.TITLE,
                            PATH = info.PATH
                        };
                    }

                    if (info.VISITCOUNT == 0)
                    {
                        info.UPDATETIME = DateTime.Now;
                        s.SHARECOUNT++;
                        s.CREATETIME = info.UPDATETIME;
                        s.UPDATETIME = info.UPDATETIME;
                        share.Update(s);
                    }

                    info.VISITCOUNT++;
                    share_info.Update(info);
                    scope.Complete();
                    flag = true;
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("错误原因是：" + ex.Message);
                }
            }

            return flag;
        }

        public bool Update(SHARE_INFO data)
        {
            return share_info.Update(data);
        }

        public bool Delete(Int64 id)
        {
            return share_info.Delete(id);
        }

        public bool Deletes(string ids)
        {
            return share_info.Deletes(ids);
        }

        public SHARE_INFO GetShareInfoByUserName(string SESSION, Int64 SSID, Int64 OID, Int64 ADID, string PATH, string SHARETYPE)
        {
            return share_info.Select(SESSION, SSID, OID, ADID, PATH, SHARETYPE);
        }
    }
}
