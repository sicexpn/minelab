using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.DAL;
using LUOBO.Entity;
using System.Transactions;

namespace LUOBO.BLL
{
    public class BLL_SYS_ORGAPP
    {
        DAL_SYS_ORGAPP orgAppDAL = new DAL_SYS_ORGAPP();
        public bool Insert(SYS_ORGAPP data)
        {
            return orgAppDAL.Insert(data);
        }

        public bool Inserts(List<SYS_ORGAPP> datas)
        {
            bool flag = false;
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    foreach (SYS_ORGAPP data in datas)
                        orgAppDAL.Insert(data);
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
            //return orgAppDAL.Inserts(datas);
        }

        public bool Update(SYS_ORGAPP data)
        {
            return orgAppDAL.Update(data);
        }

        public bool Updates(List<SYS_ORGAPP> datas)
        {
            return orgAppDAL.Updates(datas);
        }

        public bool Delete(Int64 id)
        {
            return orgAppDAL.Delete(id);
        }

        public List<SYS_ORGAPP> Select()
        {
            return orgAppDAL.Select();
        }

        public SYS_ORGAPP Select(Int64 id)
        {
            return orgAppDAL.Select(id);
        }

        public bool Deletes(int id, string ids)
        {
            return orgAppDAL.Deletes(id, ids);
        }
    }
}
