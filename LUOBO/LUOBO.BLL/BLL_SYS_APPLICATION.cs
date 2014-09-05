using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.DAL;
using LUOBO.Entity;
using System.Transactions;

namespace LUOBO.BLL
{
    public class BLL_SYS_APPLICATION
    {
        DAL_SYS_APPLICATION aDAL = new DAL_SYS_APPLICATION();

        public bool Insert(SYS_APPLICATION data)
        {
            return aDAL.Insert(data);
        }

        public bool Inserts(List<SYS_APPLICATION> datas)
        {
            bool flag = false;

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    foreach (SYS_APPLICATION data in datas)
                    {
                        aDAL.Insert(data);
                    }
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

        public bool Update(SYS_APPLICATION data)
        {
            return aDAL.Update(data);
        }

        public bool Updates(List<SYS_APPLICATION> datas)
        {
            return aDAL.Updates(datas);
        }

        public bool Delete(Int64 id)
        {
            return aDAL.Delete(id);
        }

        public List<SYS_APPLICATION> Select()
        {
            return aDAL.Select();
        }

        public SYS_APPLICATION Select(Int64 id)
        {
            return aDAL.Select(id);
        }
    }
}
