using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.DAL;
using LUOBO.Entity;
using System.Transactions;

namespace LUOBO.BLL
{
    public class BLL_SYS_MENU
    {
        DAL_SYS_USER uDAL = new DAL_SYS_USER();
        DAL_SYS_MENU mDAL = new DAL_SYS_MENU();
        DAL_SYS_MENUUSER muDAL = new DAL_SYS_MENUUSER();

        /// <summary>
        /// 获取所有菜单列表(除禁用菜单)
        /// </summary>
        /// <returns></returns>
        public List<SYS_MENU> GetMenuList()
        {
            return mDAL.Select();
        }

        /// <summary>
        /// 获取所有菜单列表(包括禁用菜单)
        /// </summary>
        /// <returns></returns>
        public List<SYS_MENU> GetAllMenuList()
        {
            return mDAL.SelectAll();
        }

        /// <summary>
        /// 根据菜单ID获取菜单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SYS_MENU GetMenuByID(Int64 id)
        {
            return mDAL.SelectByID(id);
        }

        /// <summary>
        /// 根据token获取用户菜单
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public List<SYS_MENU> GetMenuByToken(string token)
        {
            SYS_USER user = uDAL.SelectByToken(token);
            return mDAL.SelectByUID(user.ID);
        }

        /// <summary>
        /// 根据用户ID获取用户菜单
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public List<SYS_MENU> GetMenuByUID(Int64 uid)
        {
            return mDAL.SelectByUID(uid);
        }

        /// <summary>
        /// 根据用户ID获取用户所有菜单
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public List<SYS_MENU> GetMenuByUIDAll(Int64 uid)
        {
            return mDAL.SelectByUIDAll(uid);
        }

        /// <summary>
        /// 设置是否启用
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ison"></param>
        /// <returns></returns>
        public bool SetMenuIsOn(Int64 id, bool ison)
        {
            return mDAL.UpdateForIsOn(id, ison);
        }

        public bool SetUserMenuPermissions(string menuids, string userids)
        {
            bool flag = false;

            string[] mids = menuids.Split(',');
            string[] uids = userids.Split(',');
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    if (!muDAL.DeletesByUIDs(userids))
                        throw new Exception("权限更改失败!");

                    SYS_MENUUSER mu = null;
                    foreach (string mid in mids)
                    {
                        foreach (string uid in uids)
                        {
                            mu = new SYS_MENUUSER();
                            mu.ID = -1;
                            mu.M_ID = Convert.ToInt64(mid);
                            mu.U_ID = Convert.ToInt64(uid);
                            muDAL.Update(mu);
                        }
                    }

                    flag = true;
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("错误原因是：" + ex.Message);
                }
                finally { }
            }

            return flag;
        }

        /// <summary>
        /// 保存菜单信息
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public bool SaveMenu(SYS_MENU menu)
        {
            return mDAL.Update(menu);
        }
    }
}
