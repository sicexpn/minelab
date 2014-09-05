using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.DAL;
using LUOBO.Entity;
using LUOBO.Model;
using System.Transactions;
using System.Diagnostics;
using System.IO;

namespace LUOBO.BLL
{
    public class BLL_SYS_USER
    {
        DAL_SYS_USER uDAL = new DAL_SYS_USER();

        public bool Insert(SYS_USER data)
        {
            return uDAL.Insert(data);
        }

        public bool Inserts(List<SYS_USER> datas)
        {
            bool flag = false;

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    foreach (SYS_USER data in datas)
                        uDAL.Insert(data);

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

        public bool Update(SYS_USER data)
        {
            return uDAL.Update(data);
        }

        public bool Updates(List<SYS_USER> datas)
        {
            bool flag = false;

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    foreach (SYS_USER data in datas)
                        uDAL.Update(data);

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
            return uDAL.Delete(id);
        }

        public bool Deletes(string ids)
        {
            return uDAL.Deletes(ids);
        }

        public bool Disables(string ids)
        {
            return uDAL.Disables(ids);
        }

        public List<SYS_USER> Select()
        {
            return uDAL.Select();
        }

        public SYS_USER Select(Int64 id)
        {
            return uDAL.Select(id);
        }

        public bool Select(string ACCOUNT)
        {
            return uDAL.Select(ACCOUNT);
        }

        public List<SYS_USER> SelectByOID(Int64 OID)
        {
            return uDAL.SelectByOID(OID);
        }

        public List<SYS_USER> SelectByACCOUNTs(List<string> ACCOUNTs)
        {
            string result = "";
            foreach (string item in ACCOUNTs)
            {
                if (result != "")
                    result += ",";
                result += "'" + item + "'";
            }
            return uDAL.SelectByACCOUNTs(result);
        }

        public SYS_USER Select(string ACCOUNT, string PWD)
        {
            return uDAL.Select(ACCOUNT, PWD);
        }

        public SYS_USER Select(string ACCOUNT, string PWD, string MAC)
        {
            return uDAL.Select(ACCOUNT, PWD, MAC);
        }

        public M_SYS_USER Select(int size, Int64 curPage, string jgName, string userName, Int32 userType)
        {
            M_SYS_USER mUser = new M_SYS_USER();
            mUser.AllCount = uDAL.SelectCount(jgName, userName, userType);
            mUser.UserList = uDAL.Select(size, curPage, jgName, userName, userType);
            return mUser;
        }

        public SYS_USER SelectByToken(string token)
        {
            return uDAL.SelectByToken(token);
        }

        public SYS_USER SelectByToken(string token, string auth)
        {
            SYS_USER user = uDAL.SelectByToken(token);
            if (user != null)
            {
                BLL_Log.Instance().WriterLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + user.ACCOUNT + " " + user.USERNAME + " " + auth);
            }
            return user;
        }

        public bool CheckAccount(string account)
        {
            return uDAL.CheckAccount(account);
        }
    }
}
