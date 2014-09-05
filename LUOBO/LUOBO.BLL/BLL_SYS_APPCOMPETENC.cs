using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.DAL;
using LUOBO.Entity;
using System.Transactions;
using LUOBO.Model;

namespace LUOBO.BLL
{
    public class BLL_SYS_APPCOMPETENC
    {
        DAL_SYS_APPCOMPETENC aDAL = new DAL_SYS_APPCOMPETENC();

        public bool Insert(SYS_APPCOMPETENC data)
        {
            return aDAL.Insert(data);
        }

        public bool Inserts(List<SYS_APPCOMPETENC> datas)
        {
            bool flag = false;

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    foreach (SYS_APPCOMPETENC data in datas)
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

        public bool Update(SYS_APPCOMPETENC data)
        {
            return aDAL.Update(data);
        }

        public bool Updates(List<SYS_APPCOMPETENC> datas)
        {
            return aDAL.Updates(datas);
        }

        public bool Delete(Int64 id)
        {
            return aDAL.Delete(id);
        }

        public List<SYS_APPCOMPETENC> Select()
        {
            return aDAL.Select();
        }

        public SYS_APPCOMPETENC Select(Int64 id)
        {
            return aDAL.Select(id);
        }

        public M_SYS_APPCOMPETENC Select(int size, Int64 curPage, string name, Int64 appID)
        {
            M_SYS_APPCOMPETENC mAC = new M_SYS_APPCOMPETENC();
            mAC.AllCount = aDAL.SelectCount(name, appID);
            mAC.AppcompetencList = aDAL.Select(size, curPage, name, appID);
            return mAC;
        }

        public List<SYS_APPCOMPETENC_VIEW> Select_view(Int64 uid)
        {
            return aDAL.Select_view(uid);
        }
    }
}
