using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.DAL;
using LUOBO.Entity;
using System.Transactions;

namespace LUOBO.BLL
{
    public class BLL_SYS_USERAPPCOMPETENCE
    {
        DAL_SYS_USERAPPCOMPETENCE uaDAL = new DAL_SYS_USERAPPCOMPETENCE();

        public bool Insert(SYS_USERAPPCOMPETENCE data)
        {
            return uaDAL.Insert(data);
        }

        public bool Inserts(List<SYS_USERAPPCOMPETENCE> datas)
        {
            bool flag = false;

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    uaDAL.DeleteByUID(datas[0].UID);

                    foreach (SYS_USERAPPCOMPETENCE data in datas)
                        uaDAL.Insert(data);

                    flag = true;
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("错误原因是：" + ex.Message);
                }
            }

            return flag;
        }

        public bool Update(SYS_USERAPPCOMPETENCE data)
        {
            return uaDAL.Update(data);
        }

        public bool Updates(List<SYS_USERAPPCOMPETENCE> datas)
        {
            bool flag = false;

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    foreach (SYS_USERAPPCOMPETENCE data in datas)
                        uaDAL.Update(data);

                    flag = true;
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("错误原因是：" + ex.Message);
                }
            }

            return flag;
        }

        public bool Delete(Int64 id)
        {
            return uaDAL.Delete(id);
        }

        public bool Deletes(string ids)
        {
            return uaDAL.Deletes(ids);
        }

        public bool DeleteByUID(Int64 UID)
        {
            return uaDAL.DeleteByUID(UID);
        }

        public List<SYS_USERAPPCOMPETENCE> Select()
        {
            return uaDAL.Select();
        }

        public SYS_USERAPPCOMPETENCE Select(Int64 id)
        {
            return uaDAL.Select(id);
        }

        public List<SYS_USERAPPCOMPETENCE> SelectByUID(Int64 uid)
        {
            return uaDAL.SelectByUID(uid);
        }
    }
}
