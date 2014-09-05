using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using LUOBO.BLL;
using LUOBO.Model;
using LUOBO.Entity;
using LUOBO.Helper;
using System.ComponentModel;
using System.Net;
using System.IO;
using MongoDB.Bson;
using Newtonsoft.Json;
using System.Collections;
using System.Security.Cryptography;
using System.Web;

namespace LUOBO.BusinessService
{
    // Start the service and browse to http://<machine_name>:<port>/Service1/help to view the service's generated help page
    // NOTE: By default, a new instance of the service is created for each call; change the InstanceContextMode to Single if you want
    // a single instance of the service to process all calls.	
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    // NOTE: If the service is renamed, remember to update the global.asax.cs file
    public class BusinessService
    {
        BLL_SYS_USER uBll = new BLL_SYS_USER();
        BLL_APManage apBll = new BLL_APManage();
        BLL_SYS_SSID sBll = new BLL_SYS_SSID();
        BLL_ADManage adBll = new BLL_ADManage();
        BLL_SYS_ORGANIZATION oBll = new BLL_SYS_ORGANIZATION();
        BLL_SYS_DICT dicBll = new BLL_SYS_DICT();
        BLL_OpenSSID_Statistical osBll = new BLL_OpenSSID_Statistical();
        BLL_Statistics statBll = new BLL_Statistics();
        BLL_WarnManage warnBll = new BLL_WarnManage();
        BLL_SHARE shareBll = new BLL_SHARE();
        BLL_SHARE_INFO s_infoBll = new BLL_SHARE_INFO();
        BLL_INSTALL intallBll = new BLL_INSTALL();
        BLL_SYS_MENU mBll = new BLL_SYS_MENU();
        BLL.BLL_Tripartite tripBll = new BLL_Tripartite();
        BLL_Esper esperBll = BLL_Esper.Instance();
        BLL_Radius radiusBll = new BLL_Radius();
        #region 用户
        /// <summary>
        /// 用户登陆
        /// </summary>
        /// <param name="uLogin">用户登陆参数</param>
        /// <returns>是否登陆成功，成功返回Token</returns>
        [WebInvoke(UriTemplate = "Login", Method = "POST")]
        public M_WCF_Result<string> Login(UserLogin uLogin)
        {
            M_WCF_Result<string> result = new M_WCF_Result<string>();

            try
            {
                Entity.SYS_USER user = uBll.Select(uLogin.ACCOUNT, uLogin.PWD);
                if (user != null)
                {
                    user.TOKEN = Guid.NewGuid().ToString("N");
                    user.TOKENTIMESTAMP = DateTime.Now.AddHours(12);

                    if (uBll.Update(user))
                    {
                        result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                        result.ResultOBJ = user.TOKEN;
                        result.ResultMsg = "登录成功！";
                    }
                    else
                    {
                        result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                        result.ResultMsg = "登录失败！\n令牌生成失败。";
                    }
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "登录失败！\n用户名或密码错误。";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "登录失败！\n" + ex.Message;
            }
            finally { }

            return result;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="Token">令牌</param>
        /// <returns>是否获取成功</returns>
        [WebInvoke(UriTemplate = "UserManage/GetUserInfo/{Token}", Method = "POST")]
        public M_WCF_Result<SYS_USER> GetUserInfo(string Token)
        {
            M_WCF_Result<SYS_USER> result = new M_WCF_Result<SYS_USER>();

            try
            {
                SYS_USER user = uBll.SelectByToken(Token);
                if (user != null)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultOBJ = user;
                    result.ResultMsg = "获取成功！";
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "获取失败！\n无此用户信息。";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "获取失败！\n" + ex.Message;
            }
            finally { }

            return result;
        }

        /// <summary>
        /// 用户登出
        /// </summary>
        /// <param name="Token">令牌</param>
        /// <returns>是否登出成功</returns>
        [WebInvoke(UriTemplate = "Logout/{Token}", Method = "POST")]
        public M_WCF_Result<string> Logout(string Token)
        {
            M_WCF_Result<string> result = new M_WCF_Result<string>();
            try
            {
                SYS_USER user = uBll.SelectByToken(Token);
                if (user != null)
                {
                    user.TOKEN = "";
                    user.TOKENTIMESTAMP = DateTime.Now;
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    uBll.Update(user);
                    result.ResultMsg = "获取成功！";
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "令牌无效";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "获取失败！\n" + ex.Message;
            }
            finally { }

            return result;
        }

        /// <summary>
        /// 用户名是否存在
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="ACCOUNT">用户名</param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "UserManage/HasAccount/{Token}", Method = "POST")]
        public M_WCF_Result<bool> HasAccount(string Token, string ACCOUNT)
        {
            M_WCF_Result<bool> result = new M_WCF_Result<bool>();
            try
            {
                bool flag = uBll.Select(ACCOUNT);
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                result.ResultOBJ = flag;
                result.ResultMsg = "获取成功！";
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "获取失败！\n" + ex.Message;
            }
            finally { }

            return result;
        }
        #endregion

        #region 菜单
        [WebInvoke(UriTemplate = "Menu/GetMenuByToken/{Token}", Method = "POST")]
        public M_WCF_Result<List<SYS_MENU>> GetMenuByToken(string Token)
        {
            M_WCF_Result<List<SYS_MENU>> result = new M_WCF_Result<List<SYS_MENU>>();
            List<SYS_MENU> list = new List<SYS_MENU>();
            try
            {
                list = mBll.GetMenuByToken(Token);
                if (list.Count > 0)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultOBJ = list;
                    result.ResultMsg = "获取成功！";
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultOBJ = list;
                    result.ResultMsg = "获取失败！\n无可获取菜单。";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "获取失败！\n" + ex.Message;
            }
            finally { }

            return result;
        }
        #endregion

        #region 机构
        /// <summary>
        /// 获取机构信息
        /// </summary>
        /// <param name="Token">令牌</param>
        /// <returns>是否获取成功</returns>
        [WebInvoke(UriTemplate = "OrganizationManage/GetOrganizationInfo/{Token}", Method = "POST")]
        public M_WCF_Result<SYS_ORGANIZATION> GetOrganizationInfo(string Token, Int64 ID)
        {
            M_WCF_Result<SYS_ORGANIZATION> result = new M_WCF_Result<SYS_ORGANIZATION>();

            try
            {
                SYS_ORGANIZATION organization = oBll.Select(ID);
                if (organization != null)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultOBJ = organization;
                    result.ResultMsg = "获取成功！";
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "获取失败！\n无此机构信息。";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "获取失败！\n" + ex.Message;
            }
            finally { }

            return result;
        }

        [WebInvoke(UriTemplate = "OrganizationManage/GetOrganizationInfoByName/{Token}", Method = "POST")]
        public M_WCF_Result<SYS_ORGANIZATION> GetOrganizationInfoByName(string Token, string name)
        {
            M_WCF_Result<SYS_ORGANIZATION> result = new M_WCF_Result<SYS_ORGANIZATION>();

            try
            {
                SYS_ORGANIZATION organization = oBll.SelectByName(name);
                if (organization != null)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultOBJ = organization;
                    result.ResultMsg = "获取成功！";
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "获取成功！\n无此机构信息。";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "获取失败！\n" + ex.Message;
            }
            finally { }

            return result;
        }

        [WebInvoke(UriTemplate = "OrganizationManage/GetOrgType/{Token}", Method = "POST")]
        public M_WCF_Result<string> GetOrgType(string Token)
        {
            M_WCF_Result<string> result = new M_WCF_Result<string>();

            try
            {
                SYS_USER user = uBll.SelectByToken(Token);
                SYS_ORGANIZATION organization = oBll.Select(user.OID);
                bool flag = oBll.IsSSIDTemplateByOID(user.OID);
                if (organization != null)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultOBJ = organization.CATEGORY + "," + (flag ? 1 : 0);
                    result.ResultMsg = "获取成功！";
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "获取失败！\n无此机构信息。";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "获取失败！\n" + ex.Message;
            }
            finally { }

            return result;
        }

        /// <summary>
        /// 获取机构和子机构信息
        /// </summary>
        /// <param name="Token">令牌</param>
        /// <returns>是否获取成功</returns>
        [WebInvoke(UriTemplate = "OrganizationManage/GetOrganizationList/{Token}", Method = "POST")]
        public M_WCF_Result<List<SYS_ORGANIZATION>> GetOrganizationList(string Token)
        {
            M_WCF_Result<List<SYS_ORGANIZATION>> result = new M_WCF_Result<List<SYS_ORGANIZATION>>();

            try
            {
                SYS_USER user = uBll.SelectByToken(Token);
                List<SYS_ORGANIZATION> list = new List<SYS_ORGANIZATION>();
                list = oBll.SelectSub(user.OID);
                if (list.Count > 0)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultOBJ = list;
                    result.ResultMsg = "获取成功！";
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultOBJ = list;
                    result.ResultMsg = "获取失败！\n无此机构信息。";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "获取失败！\n" + ex.Message;
            }
            finally { }

            return result;
        }

        /// <summary>
        /// 获取连锁店信息列表
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "OrganizationManage/GetChainList/{Token}", Method = "POST")]
        public M_WCF_Result<List<SYS_ORGANIZATION>> GetChainList(string Token)
        {
            M_WCF_Result<List<SYS_ORGANIZATION>> result = new M_WCF_Result<List<SYS_ORGANIZATION>>();

            try
            {
                List<SYS_ORGANIZATION> list = new List<SYS_ORGANIZATION>();
                list = oBll.SelectByOrgType(LUOBO.Helper.CustomEnum.ENUM_Org_Type.Chain);
                if (list != null)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultOBJ = list;
                    result.ResultMsg = "获取成功！";
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultOBJ = list;
                    result.ResultMsg = "获取失败！";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "获取失败！\n" + ex.Message;
            }
            finally { }

            return result;
        }

        /// <summary>
        /// 获取单店信息列表
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "OrganizationManage/GetSingleList/{Token}", Method = "POST")]
        public M_WCF_Result<List<SYS_ORGANIZATION>> GetSingleList(string Token)
        {
            M_WCF_Result<List<SYS_ORGANIZATION>> result = new M_WCF_Result<List<SYS_ORGANIZATION>>();

            try
            {
                List<SYS_ORGANIZATION> list = new List<SYS_ORGANIZATION>();
                list = oBll.SelectByOrgType(LUOBO.Helper.CustomEnum.ENUM_Org_Type.Single);
                if (list != null)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultOBJ = list;
                    result.ResultMsg = "获取成功！";
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultOBJ = list;
                    result.ResultMsg = "获取失败！";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "获取失败！\n" + ex.Message;
            }
            finally { }

            return result;
        }

        /// <summary>
        /// 机构的扩展属性
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "GetExtProperty/{Token}", Method = "POST")]
        public M_WCF_Result<List<M_ORG_EXT_PROPERTY>> GetExtProperty(string Token)
        {
            M_WCF_Result<List<M_ORG_EXT_PROPERTY>> result = new M_WCF_Result<List<M_ORG_EXT_PROPERTY>>();
            try
            {
                SYS_USER user = uBll.SelectByToken(Token);
                List<M_ORG_EXT_PROPERTY> tmpList = oBll.GetOrgExtProperty(user.OID);
                if (tmpList.Count > 0)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "扩展属性信息读取成功！";
                    result.ResultOBJ = tmpList;
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "无扩展属性信息！";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "扩展属性信息读取失败！\n" + ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 机构的登录信息扩展属性
        /// </summary>
        /// <param name="Token">orgid</param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "GetLoginProperty/{OrgID}", Method = "POST")]
        public M_WCF_Result<List<SYS_ORG_PROPERTY>> GetLoginProperty(string OrgID)
        {
            M_WCF_Result<List<SYS_ORG_PROPERTY>> result = new M_WCF_Result<List<SYS_ORG_PROPERTY>>();
            try
            {
                List<SYS_ORG_PROPERTY> tmpList = oBll.GetLoginPropsByOID(long.Parse(OrgID));
                if (tmpList != null && tmpList.Count > 0)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "扩展属性信息读取成功！";
                    result.ResultOBJ = tmpList;
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "无扩展属性信息！";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "扩展属性信息读取失败！\n" + ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="ExtProperty"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "SaveExtProperty/{Token}", Method = "POST")]
        public M_WCF_Result<string> SaveExtProperty(string Token, List<UserExtProperty> ExtProperty)
        {
            M_WCF_Result<string> result = new M_WCF_Result<string>();
            try
            {
                SYS_USER user = uBll.SelectByToken(Token);
                List<SYS_ORG_PROPERTY> tmpl = new List<SYS_ORG_PROPERTY>();
                SYS_ORG_PROPERTY tmp;
                foreach (UserExtProperty data in ExtProperty)
                {
                    tmp = new SYS_ORG_PROPERTY();
                    tmp.ID = long.Parse(data.ID);
                    tmp.OID = user.OID;
                    tmp.PNAME = data.ProID;
                    tmp.PTYPE = data.ProType;
                    tmp.PVALUE = data.ProValue;
                    tmpl.Add(tmp);
                }

                if (tmpl.Count > 0)
                {
                    if (oBll.UpdateOrgPropList(tmpl) && radiusBll.setProperies(tmpl))
                    {
                        result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                        result.ResultMsg = "保存成功！";
                    }
                    else
                    {
                        result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                        result.ResultMsg = "保存失败！。";
                    }
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "没有扩展参数！。";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "保存失败！\n" + ex.Message;
            }
            finally { }
            return result;
        }

        #endregion

        #region 设备
        /// <summary>
        /// 根据令牌获取AP列表
        /// </summary>
        /// <param name="Token">用户令牌</param>
        /// <returns>AP列表</returns>
        [WebInvoke(UriTemplate = "APManage/GetAPList/{Token}", Method = "POST")]
        public M_WCF_Result<List<SYS_AP_VIEW>> GetAPList(string Token)
        {
            M_WCF_Result<List<SYS_AP_VIEW>> result = new M_WCF_Result<List<SYS_AP_VIEW>>();
            List<SYS_AP_VIEW> list = new List<SYS_AP_VIEW>();
            try
            {
                SYS_USER user = uBll.SelectByToken(Token);
                list = apBll.SelectAllAPByOID(user.OID, false);
                if (list.Count > 0)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "获取成功！";
                    result.ResultOBJ = list;
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "获取失败！\n没有可获取的列表。";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "获取失败！\n" + ex.Message;
            }
            finally { }
            return result;
        }

        /// <summary>
        /// 根据令牌获取AP状态列表
        /// </summary>
        /// <param name="Token">用户令牌</param>
        /// <returns>AP列表</returns>
        [WebInvoke(UriTemplate = "APManage/GetAPListForState/{Token}", Method = "POST")]
        public M_WCF_Result<M_APListForState> GetAPListForState(string Token, APValidation av)
        {
            M_WCF_Result<M_APListForState> result = new M_WCF_Result<M_APListForState>();
            List<SYS_APDEVICE> list = new List<SYS_APDEVICE>();
            try
            {
                SYS_USER user = uBll.SelectByToken(Token);
                if (av != null)
                    list = apBll.SelectApStateListByOID(user.OID, av.NAME, av.COLUMN, av.ORDERBY);
                else
                    list = apBll.SelectApStateListByOID(user.OID, "", "", "");
                if (list.Count > 0)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "获取成功！";
                    result.ResultOBJ = new M_APListForState();
                    result.ResultOBJ.APList = list;
                    result.ResultOBJ.AvgVisitNum = statBll.SelectAvgVisitNumByToken(Token);
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultOBJ = new M_APListForState();
                    result.ResultOBJ.APList = list;
                    result.ResultMsg = "获取失败！\n没有可获取的列表。";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "获取失败！\n" + ex.Message;
            }
            finally { }
            return result;
        }

        /// <summary>
        /// 根据令牌和APMac获取在线用户数据
        /// </summary>
        /// <param name="Token">用户令牌</param>
        /// <returns>AP列表</returns>
        [WebInvoke(UriTemplate = "APManage/GetUserForState/{Token}", Method = "POST")]
        public M_WCF_Result<M_UserForState> GetUserForState(string Token, SSIDValidation sv)
        {
            M_WCF_Result<M_UserForState> result = new M_WCF_Result<M_UserForState>();
            List<M_Passager> list = new List<M_Passager>();
            try
            {
                SYS_USER user = uBll.SelectByToken(Token);
                //list = osBll.SelectByUser(user.OID);
                if (sv.APMAC == null)
                    sv.APMAC = "";

                list = osBll.SelectPassagers(user.OID, sv.APMAC, sv.COLUMN, sv.ORDERBY, sv.PageSize, sv.CurrentPage);

                if (list.Count > 0)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "获取成功！";
                    result.ResultOBJ = new M_UserForState();
                    result.ResultOBJ.UserList = list;
                    result.ResultOBJ.AllPeopleCount = osBll.SelectPassagersCount(user.OID, sv.APMAC);
                    List<Int64> itemList = statBll.SelectUserForStateByToken(Token, sv.APMAC);
                    result.ResultOBJ.OnlinePeopleNum = itemList[0];
                    result.ResultOBJ.RZUserNum = itemList[1];
                    result.ResultOBJ.AvgVisitNum = itemList[2];
                    result.ResultOBJ.AvgVisitTime = itemList[3];
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultOBJ = new M_UserForState();
                    result.ResultOBJ.UserList = list;
                    result.ResultMsg = "获取失败！\n没有可获取的列表。";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "获取失败！\n" + ex.Message;
            }
            finally { }
            return result;
        }

        [WebInvoke(UriTemplate = "APManage/GetAPInfoByID/{Token}", Method = "POST")]
        public M_WCF_Result<SYS_APDEVICE> GetAPInfoByID(string Token, Int64 apid)
        {
            M_WCF_Result<SYS_APDEVICE> result = new M_WCF_Result<SYS_APDEVICE>();
            try
            {
                SYS_USER user = uBll.SelectByToken(Token);
                SYS_APDEVICE ap = apBll.SelectAPByID(apid);
                if (ap != null)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultOBJ = ap;
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "获取失败！\n没有可获取信息。";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "获取失败！\n" + ex.Message;
            }
            finally { }
            return result;
        }

        /// <summary>
        /// 获取AP的SSID数量
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="mac"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "APManage/GetSSIDNumByMAC/{Token}", Method = "POST")]
        public M_WCF_Result<Int32> GetSSIDNumByMAC(string Token, string mac)
        {
            M_WCF_Result<Int32> result = new M_WCF_Result<Int32>();
            try
            {
                Int32 ssidnum = apBll.GetSSIDNUMByMAC(mac);
                if (ssidnum != 0)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultOBJ = ssidnum;
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "获取失败！\n没有可获取信息。";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "获取失败！\n" + ex.Message;
            }
            finally { }
            return result;
        }

        /// <summary>
        /// 根据机构ID获取AP列表，不包含子机构
        /// </summary>
        /// <param name="OID"></param>
        /// <returns></returns>
        [Description("根据机构ID获取AP列表SYS_APDEVICE，不包含子机构")]
        [WebInvoke(UriTemplate = "APManage/GetAPListByOID/{Token}", Method = "POST")]
        public M_WCF_Result<List<SYS_APDEVICE>> GetAPListByOID(string Token, Int64 OID)
        {
            M_WCF_Result<List<SYS_APDEVICE>> result = new M_WCF_Result<List<SYS_APDEVICE>>();
            try
            {
                List<SYS_APDEVICE> list = apBll.SelectAPByOID(OID);
                if (list.Count > 0)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "获取成功！";
                    result.ResultOBJ = list;
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "获取失败！\n没有可获取的列表。";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "获取失败！\n" + ex.Message;
            }
            finally { }
            return result;
        }
        #endregion

        #region SSID
        /// <summary>
        /// 根据令牌获取SSID列表
        /// </summary>
        /// <param name="Token">令牌</param>
        /// <returns>SSID列表</returns>
        [WebInvoke(UriTemplate = "SSIDManage/GetSSIDList/{Token}", Method = "POST")]
        public M_WCF_Result<List<SYS_SSID>> GetSSIDList(string Token)
        {
            M_WCF_Result<List<SYS_SSID>> result = new M_WCF_Result<List<SYS_SSID>>();
            List<SYS_SSID> list = new List<SYS_SSID>();
            try
            {
                SYS_USER user = uBll.SelectByToken(Token);
                list = sBll.SelectByOID(user.OID, true);
                if (list.Count > 0)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "获取成功！";
                    result.ResultOBJ = list;
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "获取失败！\n没有可获取的列表。";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "获取失败！\n" + ex.Message;
            }
            finally { }
            return result;
        }

        [WebInvoke(UriTemplate = "SSIDManage/GetSSIDListByAPID/{Token}", Method = "POST")]
        public M_WCF_Result<List<M_WCF_SSID_VIEW>> GetSSIDListByAPID(string Token, Int64 APID)
        {
            M_WCF_Result<List<M_WCF_SSID_VIEW>> result = new M_WCF_Result<List<M_WCF_SSID_VIEW>>();
            List<M_WCF_SSID_VIEW> list = new List<M_WCF_SSID_VIEW>();
            try
            {
                list = sBll.SelectWcfSSIDViewByAPID(APID);
                if (list.Count > 0)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "获取成功！";
                    result.ResultOBJ = list;
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "获取失败！\n没有可获取的列表。";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "获取失败！\n" + ex.Message;
            }
            finally { }
            return result;
        }

        /// <summary>
        /// 获取AP分组的SSID列表
        /// </summary>
        /// <param name="Token">令牌</param>
        /// <returns>AP分组的SSID列表</returns>
        [WebInvoke(UriTemplate = "APManage/GetDeviceList/{Token}", Method = "POST")]
        public M_WCF_Result<List<M_WCF_SSIDGroupAP_VIEW>> GetDeviceList(string Token, SSIDValidation sv)
        {
            M_WCF_Result<List<M_WCF_SSIDGroupAP_VIEW>> result = new M_WCF_Result<List<M_WCF_SSIDGroupAP_VIEW>>();
            try
            {
                SYS_USER user = uBll.SelectByToken(Token);
                List<M_WCF_SSID_VIEW> list = sBll.SelectWcfSSIDViewByOID(user.OID);
                List<SYS_AP_VIEW> apList = apBll.SelectAPByOID(user.OID, false);

                if (sv != null)
                {
                    if (sv.TYPE == "AP")
                        apList = apList.Where(c => c.ALIAS.Contains(sv.KEY)).ToList();
                    else if (sv.TYPE == "SSID")
                        list = list.Where(c => c.NAME.Contains(sv.KEY)).ToList();
                }

                if (list.Count != 0)
                {
                    M_WCF_SSIDGroupAP_VIEW item = null;
                    result.ResultOBJ = new List<M_WCF_SSIDGroupAP_VIEW>();
                    foreach (var ap in apList)
                    {
                        item = new M_WCF_SSIDGroupAP_VIEW();
                        item.APID = ap.ID;
                        item.APNAME = ap.ALIAS;
                        if (sv != null && sv.STATE != -1)
                            item.SSIDList = list.Where(c => c.APID == ap.ID && c.STATE == sv.STATE).ToList();
                        else
                            item.SSIDList = list.Where(c => c.APID == ap.ID).ToList();
                        result.ResultOBJ.Add(item);
                    }
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "列表获取成功！";
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "列表获取失败！\n无可获取信息。";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "列表获取失败！\n" + ex.Message;
            }
            finally { }
            return result;
        }

        /// <summary>
        /// 获取AP分组的SSID列表，按发布广告ID
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "APManage/GetDeviceListByAdid/{ID}/{Token}", Method = "POST")]
        public M_WCF_Result<List<M_WCF_SSIDGroupAP_VIEW>> GetDeviceListByAdid(string Token, string ID)
        {
            M_WCF_Result<List<M_WCF_SSIDGroupAP_VIEW>> result = new M_WCF_Result<List<M_WCF_SSIDGroupAP_VIEW>>();
            try
            {
                SYS_USER user = uBll.SelectByToken(Token);
                List<M_WCF_SSID_VIEW> list = sBll.SelectWcfSSIDViewByOID(user.OID);
                List<SYS_AP_VIEW> apList = apBll.SelectAPByOID(user.OID, false);
                result.ResultOBJ = new List<M_WCF_SSIDGroupAP_VIEW>();
                list = list.Where(c => c.ADID == Int64.Parse(ID)).ToList();

                M_WCF_SSIDGroupAP_VIEW item = null;
                foreach (var ap in apList)
                {
                    item = new M_WCF_SSIDGroupAP_VIEW();
                    item.APID = ap.ID;
                    item.APNAME = ap.ALIAS;
                    item.SSIDList = list.Where(c => c.APID == ap.ID).ToList();
                    if (item.SSIDList.Count > 0)
                    {
                        result.ResultOBJ.Add(item);
                    }
                }
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                result.ResultMsg = "列表获取成功！";

            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "列表获取失败！\n" + ex.Message;
            }
            return result;
        }


        /// <summary>
        /// 获取SSID信息，按ID
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "APManage/GetSSIDListByIDs/{Token}", Method = "POST")]
        public M_WCF_Result<List<M_SSID_AP>> GetSSIDListByIDs(string Token, Int64 audid)
        {
            M_WCF_Result<List<M_SSID_AP>> result = new M_WCF_Result<List<M_SSID_AP>>();
           
            try
            {
                SYS_USER user = uBll.SelectByToken(Token);
                List<M_SSID_AP> list = sBll.GetSSIDInfoByIDs(audid);
                if (list != null)
                {
                    result.ResultOBJ = list;
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "列表获取成功！";
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "未获取到列表！\n";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "列表获取失败！\n" + ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 更新SSID
        /// </summary>
        /// <param name="Token">令牌</param>
        /// <param name="ssid">SSID信息</param>
        /// <returns>是否更新成功</returns>
        [WebInvoke(UriTemplate = "SSIDManage/SaveSSID/{Token}", Method = "POST")]
        public M_WCF_Result<string> SaveSSID(string Token, SYS_SSID ssid)
        {
            M_WCF_Result<string> result = new M_WCF_Result<string>();
            try
            {
                SYS_SSID tmpssid = sBll.SelectByID(ssid.ID);
                tmpssid.NAME = ssid.NAME;
                tmpssid.ISPWD = ssid.ISPWD;
                tmpssid.PWD = ssid.PWD;
                tmpssid.MAXFLOW = ssid.MAXFLOW;
                tmpssid.VONLINETIME = ssid.VONLINETIME;

                if (apBll.UpdateSSID(tmpssid, Token))
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "保存成功！";
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "保存失败！\n令牌生成失败。";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "保存失败！\n" + ex.Message;
            }
            finally { }
            return result;
        }

        /// <summary>
        /// 更新SSID名称
        /// </summary>
        /// <param name="Token">令牌</param>
        /// <param name="ssidns">SSID编号和SSID名称对象</param>
        /// <returns>是否更新成功</returns>
        [WebInvoke(UriTemplate = "SSIDManage/SaveSSIDForName/{Token}", Method = "POST")]
        public M_WCF_Result<string> SaveSSIDForName(string Token, SSIDNameSave ssidns)
        {
            M_WCF_Result<string> result = new M_WCF_Result<string>();
            try
            {
                SYS_SSID ssid = sBll.SelectByID(ssidns.ID);
                if (ssid != null)
                {
                    ssid.NAME = ssidns.Name;

                    if (apBll.UpdateSSID(ssid, Token))
                    {
                        result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                        result.ResultMsg = "修改成功！";
                    }
                    else
                    {
                        result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                        result.ResultMsg = "修改失败！";
                    }
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "修改失败！\n没有找到此SSID信息。";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "修改失败！\n" + ex.Message;
            }
            finally { }
            return result;
        }

        /// <summary>
        /// 禁用SSID
        /// </summary>
        /// <param name="Token">令牌</param>
        /// <param name="sClass">SSID参数对象</param>
        /// <returns>是否更新成功</returns>
        [WebInvoke(UriTemplate = "SSIDManage/DisableSSID/{Token}", Method = "POST")]
        public M_WCF_Result<string> DisableSSID(string Token, Int64 ID)
        {
            M_WCF_Result<string> result = new M_WCF_Result<string>();
            try
            {
                SYS_SSID ssid = sBll.SelectByID(ID);
                if (ssid != null)
                {
                    ssid.ISON = false;
                    if (apBll.UpdateSSID(ssid, Token))
                    {
                        result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                        result.ResultMsg = "禁用成功！";
                    }
                    else
                    {
                        result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                        result.ResultMsg = "禁用失败！";
                    }
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "禁用失败！\n没有找到此SSID信息。";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "禁用失败！\n" + ex.Message;
            }
            finally { }
            return result;
        }

        /// <summary>
        /// 添加SSID
        /// </summary>
        /// <param name="Token">令牌</param>
        /// <param name="aClass">AP参数对象</param>
        /// <returns>是否添加成功</returns>
        [WebInvoke(UriTemplate = "SSIDManage/AddSSID/{Token}", Method = "POST")]
        public M_WCF_Result<SYS_SSID> AddSSID(string Token, Int64 ID)
        {
            M_WCF_Result<SYS_SSID> result = new M_WCF_Result<SYS_SSID>();
            try
            {
                SYS_USER user = uBll.SelectByToken(Token);
                List<SYS_SSID> list = sBll.SelectByOwnerAndIsOn(user.OID, ID, false);
                if (list.Count > 0)
                {
                    SYS_SSID ssid = list[0];
                    ssid.ISON = true;

                    apBll.UpdateSSID(ssid, Token);

                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "添加成功！";
                    result.ResultOBJ = ssid;
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "添加失败！\n已达到最大SSID数量，无法添加。";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "添加失败！\n" + ex.Message;
            }
            finally { }
            return result;
        }
        #endregion

        #region 广告编辑
        /// <summary>
        /// 获取广告列表
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "ADManage/GetADList/{Token}", Method = "POST")]
        public M_WCF_Result<List<AD_INFO>> GetADList(string Token)
        {
            M_WCF_Result<List<AD_INFO>> result = new M_WCF_Result<List<AD_INFO>>();
            try
            {
                Int64 oid = uBll.SelectByToken(Token).OID;
                List<AD_INFO> adinfo = adBll.SelectADAll(oid);
                if (adinfo != null)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "成功！";
                    result.ResultOBJ = adinfo;
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "广告读取失败！\n不存在该广告信息";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "广告读取失败！\n" + ex.Message;
            }
            return result;
        }
        /// <summary>
        /// 获取广告列表，包含发布点数
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "ADManage/GetADPubList/{Token}", Method = "POST")]
        public M_WCF_Result<M_AD_PUBINFO> GetADPubList(string Token, ADKeyPage adkp)
        {
            M_WCF_Result<M_AD_PUBINFO> result = new M_WCF_Result<M_AD_PUBINFO>();
            try
            {
                Int64 oid = uBll.SelectByToken(Token).OID;
                M_AD_PUBINFO adinfo = adBll.SelectADPubByPage(oid, adkp.adaudit, adkp.pagesize, adkp.pagenum, adkp.keystr);
                if (adinfo != null)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "成功！";
                    result.ResultOBJ = adinfo;
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "广告读取失败！\n不存在该广告信息";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "广告读取失败！\n" + ex.Message;
            }
            return result;
        }
        /// <summary>
        /// 获取广告信息
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="AD_ID"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "ADManage/GetADInfo/{AD_ID}/{Token}", Method = "POST")]
        public M_WCF_Result<AD_INFO> GetADInfo(string Token, string AD_ID)
        {
            M_WCF_Result<AD_INFO> result = new M_WCF_Result<AD_INFO>();
            try
            {
                AD_INFO adinfo = adBll.get_ADInfo(Int64.Parse(AD_ID));
                if (adinfo != null)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "成功！";
                    result.ResultOBJ = adinfo;
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "广告读取失败！\n不存在该广告信息";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "广告读取失败！\n" + ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 获取广告和放行域名信息
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="AD_ID"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "ADManage/GetADInfoAndFreeHost/{AD_ID}/{Token}", Method = "POST")]
        public M_WCF_Result<AD_INFO_FREEHOST> GetADInfoAndFreeHost(string Token, string AD_ID)
        {
            M_WCF_Result<AD_INFO_FREEHOST> result = new M_WCF_Result<AD_INFO_FREEHOST>();
            try
            {
                AD_INFO_FREEHOST adinfo = adBll.SelectOneWithFreeHost(Int64.Parse(AD_ID));
                if (adinfo != null)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "成功！";
                    result.ResultOBJ = adinfo;
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "广告读取失败！\n不存在该广告信息";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "广告读取失败！\n" + ex.Message;
            }
            return result;
        }


        /// <summary>
        /// 获取广告文件内容
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="AD_ID"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "ADManage/GetADFiles/{AD_ID}/{Token}", Method = "POST")]
        public M_WCF_Result<List<M_ADTempletFile>> GetADFiles(string Token, string AD_ID)
        {
            M_WCF_Result<List<M_ADTempletFile>> result = new M_WCF_Result<List<M_ADTempletFile>>();
            try
            {
                Int64 oid = uBll.SelectByToken(Token).OID;
                Int64 adid = Int64.Parse(AD_ID);
                List<M_ADTempletFile> adinfo = adBll.GetADFiles(Int64.Parse(AD_ID), uBll.SelectByToken(Token).OID);
                if (adinfo != null)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "成功！";
                    result.ResultOBJ = adinfo;
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "广告读取失败！\n不存在该广告信息";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "广告读取失败！\n" + ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 获取广告模版列表
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "ADManage/GetTempletList/{Token}", Method = "POST")]
        public M_WCF_Result<List<SYS_ADTEMPLET>> GetTempletList(string Token)
        {
            M_WCF_Result<List<SYS_ADTEMPLET>> result = new M_WCF_Result<List<SYS_ADTEMPLET>>();
            try
            {
                List<SYS_ADTEMPLET> adinfo = adBll.SelectADTemplet();
                if (adinfo != null)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "成功！";
                    result.ResultOBJ = adinfo;
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "广告读取失败！\n没有广告模版信息";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "广告读取失败！\n" + ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 获取广告模版文件内容
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="AD_ID"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "ADManage/GetTempletFiles/{Temp_ID}/{Token}", Method = "POST")]
        public M_WCF_Result<List<M_ADTempletFile>> GetTempletFiles(string Token, string Temp_ID)
        {
            M_WCF_Result<List<M_ADTempletFile>> result = new M_WCF_Result<List<M_ADTempletFile>>();
            try
            {
                List<M_ADTempletFile> adinfo = adBll.GetTempletFiles(Int32.Parse(Temp_ID));
                if (adinfo != null)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "成功！";
                    result.ResultOBJ = adinfo;
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "广告读取失败！\n不存在该广告模版信息";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "广告读取失败！\n" + ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "ADManage/GetServerPics/{Token}", Method = "POST")]
        public M_WCF_Result<List<M_File>> GetServerPics(string Token)
        {
            M_WCF_Result<List<M_File>> result = new M_WCF_Result<List<M_File>>();
            try
            {
                Int64 org_id = uBll.SelectByToken(Token).OID;
                List<M_File> flist = adBll.GetServerPics(org_id);

                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                result.ResultMsg = "成功！";
                result.ResultOBJ = flist;
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "文件读取失败！\n" + ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="adaudit"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "ADManage/PostADAudit/{Token}", Method = "POST")]
        public M_WCF_Result<String> PostADAudit(string Token, ADAudit adaudit)
        {
            M_WCF_Result<String> result = new M_WCF_Result<String>();
            try
            {
                SYS_USER theuser = uBll.SelectByToken(Token);
                String restr = adBll.PostAudit(adaudit.ad_id, theuser.OID, adaudit.pub_type, adaudit.ids, theuser.USERNAME, adaudit.ascase, adaudit.isCopyName);
                if (restr == "ok")
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "成功！";
                    result.ResultOBJ = restr;
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "广告提交审核失败！\n" + restr;
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "广告提交审核失败！\n" + ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="adaudit"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "ADManage/TarAD/{Token}", Method = "POST")]
        public M_WCF_Result<String> TarAD(string Token, string path)
        {
            M_WCF_Result<String> result = new M_WCF_Result<String>();
            try
            {
                string dl = BLL_ZipQueue.Instance().TarFolderForControl(path);
                if (dl != null)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "成功！";
                    result.ResultOBJ = dl;
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "打包失败！";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "广告打包失败！\n" + ex.Message;
            }
            return result;
        }

        #endregion

        #region 公用
        /// <summary>
        /// 行业信息
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "GetDicByHangYe/{Token}", Method = "POST")]
        public M_WCF_Result<List<SYS_DICT>> GetDicByHangYe(string Token)
        {
            M_WCF_Result<List<SYS_DICT>> result = new M_WCF_Result<List<SYS_DICT>>();
            try
            {
                List<SYS_DICT> dicinfo = dicBll.Select().Where(c => c.CATEGORY == "行业").ToList();
                if (dicinfo != null)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "行业信息读取成功！";
                    result.ResultOBJ = dicinfo;
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "行业信息读取失败！";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "行业信息读取失败！\n" + ex.Message;
            }
            return result;
        }

        [WebInvoke(UriTemplate = "GetDicByMianJi/{Token}", Method = "POST")]
        public M_WCF_Result<List<SYS_DICT>> GetDicByMianJi(string Token)
        {
            M_WCF_Result<List<SYS_DICT>> result = new M_WCF_Result<List<SYS_DICT>>();
            try
            {
                List<SYS_DICT> dicinfo = dicBll.Select().Where(c => c.CATEGORY == "面积").ToList();
                if (dicinfo != null)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "面积信息读取成功！";
                    result.ResultOBJ = dicinfo;
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "面积信息读取失败！";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "面积信息读取失败！\n" + ex.Message;
            }
            return result;
        }

        [WebInvoke(UriTemplate = "GetDicByAuditState/{Token}", Method = "POST")]
        public M_WCF_Result<List<SYS_DICT>> GetDicByAuditState(string Token)
        {
            M_WCF_Result<List<SYS_DICT>> result = new M_WCF_Result<List<SYS_DICT>>();
            try
            {
                List<SYS_DICT> dicinfo = dicBll.Select().Where(c => c.CATEGORY == "审核申请处理状态").ToList();
                if (dicinfo != null)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "审核申请处理状态信息读取成功！";
                    result.ResultOBJ = dicinfo;
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "审核申请处理状态信息读取失败！";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "审核申请处理状态信息读取失败！\n" + ex.Message;
            }
            return result;
        }

        [WebInvoke(UriTemplate = "GetDicByFreeHost/{Token}", Method = "POST")]
        public M_WCF_Result<List<SYS_DICT>> GetDicByFreeHost(string Token)
        {
            M_WCF_Result<List<SYS_DICT>> result = new M_WCF_Result<List<SYS_DICT>>();
            try
            {
                List<SYS_DICT> dicinfo = dicBll.Select().Where(c => c.CATEGORY == "放行域名").ToList();
                if (dicinfo != null)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "默认放行域名信息读取成功！";
                    result.ResultOBJ = dicinfo;
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "默认放行域名信息读取失败！";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "默认放行域名信息读取失败！\n" + ex.Message;
            }
            return result;
        }

        public string getOlineType(Int32 _id)
        {
            string tmp = "";
            switch (_id)
            {
                case -1:
                    tmp = "未认证";
                    break;
                case 0:
                    tmp = "一键登录";
                    break;
                case 1:
                    tmp = "QQ登陆";
                    break;
                case 2:
                    tmp = "微博登陆";
                    break;
                case 3:
                    tmp = "微信登陆";
                    break;
                default:
                    tmp = "未知";
                    break;
            }
            return tmp;
        }

        /// <summary>
        /// URL请求参数UTF8编码
        /// </summary>
        /// <param name="value">源字符串</param>
        /// <returns>编码后的字符串</returns> 
        private static string Utf8Encode(string value)
        {
            return HttpUtility.UrlEncode(value, System.Text.Encoding.UTF8);
        }
        #endregion

        #region 统计
        /// <summary>
        /// 收集统计信息
        /// </summary>
        /// <param name="oSSID">统计收集对象</param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "ReportStatistical", Method = "POST")]
        public M_WCF_Result<string> ReportStatistical(OpenSSID oSSID)
        {
            M_WCF_Result<string> result = new M_WCF_Result<string>();

            try
            {
                oSSID.CurrentTime = DateTime.Now;
                if (osBll.Insert(oSSID))
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "插入成功！";
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "插入失败！";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "插入失败！\n" + ex.Message;
            }
            finally { }

            return result;
        }

        /// <summary>
        /// 人数统计地图中显示
        /// </summary>
        /// <param name="Token">令牌</param>
        /// <param name="sp"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "Statistical/People_Map/{Token}", Method = "POST")]
        public List<Array> StatisticalPeople_Map(string Token, StatisticalParam param)
        {
            System.Random random = new Random(~unchecked((int)DateTime.Now.Ticks));
            List<Array> result = new List<Array>();
            if (param.Mode == "RealTime")
            {
                Int64 RCNUM, ZNUM, RZNUM;

                DateTime date = DateTime.Now.AddHours(-1);
                List<List<Int64>> itemList = statBll.SelectOnlinePeopleNum_MapByToken(Token, param.ID, date, DateTime.Now, CustomEnum.ENUM_Statistical_Type.RealTime);

                RCNUM = itemList[0].Sum();
                ZNUM = itemList[1].Sum();
                RZNUM = itemList[2].Sum();

                string time = DateTime.Now.ToLongTimeString();
                result.Add(new object[] { 0, RCNUM, false, false, time });
                result.Add(new object[] { 1, ZNUM, false, false, time });
                result.Add(new object[] { 2, RZNUM, false, false, time });
            }
            else if (param.Mode == "Week")
            {
                DateTime date = DateTime.Now.AddDays(-6);
                List<List<Int64>> itemList = statBll.SelectOnlinePeopleNum_MapByToken(Token, param.ID, date.Date, DateTime.Now.Date, CustomEnum.ENUM_Statistical_Type.Day);
                foreach (List<Int64> item in itemList)
                    result.Add(item.Select(c => (object)c).ToArray());
            }
            else if (param.Mode == "Month")
            {
                DateTime date = DateTime.Now.AddDays(-29);
                List<List<Int64>> itemList = statBll.SelectOnlinePeopleNum_MapByToken(Token, param.ID, date.Date, DateTime.Now.Date, CustomEnum.ENUM_Statistical_Type.Day);
                foreach (List<Int64> item in itemList)
                    result.Add(item.Select(c => (object)c).ToArray());
            }
            else if (param.Mode == "Year")
            {
                DateTime date = DateTime.Now.AddMonths(-11);
                List<List<Int64>> itemList = statBll.SelectOnlinePeopleNum_MapByToken(Token, param.ID, date.Date, DateTime.Now.Date, CustomEnum.ENUM_Statistical_Type.Month);
                foreach (List<Int64> item in itemList)
                    result.Add(item.Select(c => (object)c).ToArray());
            }
            else if (param.Mode == "Date")
            {
                List<List<Int64>> itemList = statBll.SelectOnlinePeopleNum_MapByToken(Token, param.ID, DateTime.Parse(param.StartTime).Date, DateTime.Parse(param.EndTime).Date, CustomEnum.ENUM_Statistical_Type.Day);
                foreach (List<Int64> item in itemList)
                    result.Add(item.Select(c => (object)c).ToArray());
            }
            return result;
        }

        /// <summary>
        /// 在线人数统计,根据实时、近一周、近一月、时间区间
        /// </summary>
        /// <param name="Token">令牌</param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "Statistical/Online/{Token}", Method = "POST")]
        public List<Array> StatisticalOnline(string Token, StatisticalParam param)
        {
            List<Array> result = new List<Array>();
            if (param.Mode == "RealTime")
            {
                List<Int64> list = apBll.SelectOnlinePeopleNumByToken(Token, param.ID);
                Int64 onlinePeople = 0, onlinePeopleNum = 0;
                if (list.Count > 0)
                {
                    onlinePeople = list[0];
                    onlinePeopleNum = list[1];
                }
                string time = DateTime.Now.ToLongTimeString();
                result.Add(new object[] { 0, onlinePeople, false, false, time });
                result.Add(new object[] { 1, onlinePeopleNum, false, false, time });
            }
            else if (param.Mode == "Week")
            {
                DateTime date = DateTime.Now.AddDays(-6);
                List<List<Int64>> itemList = statBll.GetOLPeopleByDateAndOID(date.Date, DateTime.Now.Date, Token, param.ID, CustomEnum.ENUM_Statistical_Type.Day);
                result.Add(itemList[0].Select(c => (object)c).ToArray());
                result.Add(itemList[1].Select(c => (object)c).ToArray());
            }
            else if (param.Mode == "Month")
            {
                DateTime date = DateTime.Now.AddDays(-29);
                List<List<Int64>> itemList = statBll.GetOLPeopleByDateAndOID(date.Date, DateTime.Now.Date, Token, param.ID, CustomEnum.ENUM_Statistical_Type.Day);
                result.Add(itemList[0].Select(c => (object)c).ToArray());
                result.Add(itemList[1].Select(c => (object)c).ToArray());
            }
            else if (param.Mode == "Year")
            {
                DateTime date = DateTime.Now.AddMonths(-11);
                List<List<Int64>> itemList = statBll.GetOLPeopleByDateAndOID(date.Date, DateTime.Now.Date, Token, param.ID, CustomEnum.ENUM_Statistical_Type.Month);
                result.Add(itemList[0].Select(c => (object)c).ToArray());
                result.Add(itemList[1].Select(c => (object)c).ToArray());
            }
            else if (param.Mode == "Date")
            {
                List<List<Int64>> itemList = statBll.GetOLPeopleByDateAndOID(DateTime.Parse(param.StartTime).Date, DateTime.Parse(param.EndTime).Date, Token, param.ID, CustomEnum.ENUM_Statistical_Type.Day);
                result.Add(itemList[0].Select(c => (object)c).ToArray());
                result.Add(itemList[1].Select(c => (object)c).ToArray());
            }
            return result;
        }

        /// <summary>
        /// 人数统计,根据实时、近一周、近一月、时间区间
        /// </summary>
        /// <param name="Token">令牌</param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "Statistical/People/{Token}", Method = "POST")]
        public List<Array> StatisticalPeople(string Token, StatisticalParam param)
        {
            List<Array> result = new List<Array>();
            if (param.Mode == "RealTime")
            {

            }
            else if (param.Mode == "Week")
            {
                DateTime date = DateTime.Now.AddDays(-6);
                List<List<Int64>> itemList = statBll.SelectStatisticalWIFIByToken(Token, param.ID, date.Date, DateTime.Now.Date, CustomEnum.ENUM_Statistical_Type.Day);
                foreach (List<Int64> item in itemList)
                    result.Add(item.Select(c => (object)c).ToArray());
            }
            else if (param.Mode == "Month")
            {
                DateTime date = DateTime.Now.AddDays(-29);
                List<List<Int64>> itemList = statBll.SelectStatisticalWIFIByToken(Token, param.ID, date.Date, DateTime.Now.Date, CustomEnum.ENUM_Statistical_Type.Day);
                foreach (List<Int64> item in itemList)
                    result.Add(item.Select(c => (object)c).ToArray());
            }
            else if (param.Mode == "Year")
            {
                DateTime date = DateTime.Now.AddMonths(-11);
                List<List<Int64>> itemList = statBll.SelectStatisticalWIFIByToken(Token, param.ID, date.Date, DateTime.Now.Date, CustomEnum.ENUM_Statistical_Type.Month);
                foreach (List<Int64> item in itemList)
                    result.Add(item.Select(c => (object)c).ToArray());
            }
            else if (param.Mode == "Date")
            {
                List<List<Int64>> itemList = statBll.SelectStatisticalWIFIByToken(Token, param.ID, DateTime.Parse(param.StartTime).Date, DateTime.Parse(param.EndTime).Date, CustomEnum.ENUM_Statistical_Type.Day);
                foreach (List<Int64> item in itemList)
                    result.Add(item.Select(c => (object)c).ToArray());
            }
            return result;
        }

        /// <summary>
        /// SSID统计,根据实时、近一周、近一月、时间区间
        /// </summary>
        /// <param name="Token">令牌</param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "Statistical/SSID/{Token}", Method = "POST")]
        public List<PieData> StatisticalSSID(string Token, StatisticalParam param)
        {
            List<PieData> result = new List<PieData>();
            if (param.Mode == "RealTime")
            {

            }
            else if (param.Mode == "Week")
            {
                DateTime date = DateTime.Now.AddDays(-6);
                List<M_Statistical> list = sBll.SelectStatisticalSSIDByToken(Token, param.ID, date.Date, DateTime.Now.Date);
                result = list.Select(c => new PieData { ID = c.ID, NAME = c.NAME, NUM = c.NUM }).ToList();
            }
            else if (param.Mode == "Month")
            {
                DateTime date = DateTime.Now.AddDays(-29);
                List<M_Statistical> list = sBll.SelectStatisticalSSIDByToken(Token, param.ID, date.Date, DateTime.Now.Date);
                result = list.Select(c => new PieData { ID = c.ID, NAME = c.NAME, NUM = c.NUM }).ToList();
            }
            else if (param.Mode == "Year")
            {
                DateTime date = DateTime.Now.AddMonths(-11);
                List<M_Statistical> list = sBll.SelectStatisticalSSIDByToken(Token, param.ID, date.Date, DateTime.Now.Date);
                result = list.Select(c => new PieData { ID = c.ID, NAME = c.NAME, NUM = c.NUM }).ToList();
            }
            else if (param.Mode == "Date")
            {
                DateTime sTime = DateTime.Parse(param.StartTime);
                DateTime eTime = DateTime.Parse(param.EndTime);
                int day = (eTime - sTime).Days + 1;
                List<M_Statistical> list = sBll.SelectStatisticalSSIDByToken(Token, param.ID, sTime, eTime);
                result = list.Select(c => new PieData { ID = c.ID, NAME = c.NAME, NUM = c.NUM }).ToList();
            }
            return result;
        }

        /// <summary>
        /// 广告统计,根据实时、近一周、近一月、时间区间
        /// </summary>
        /// <param name="Token">令牌</param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "Statistical/AD/{Token}", Method = "POST")]
        public List<PieData> StatisticalAD(string Token, StatisticalParam param)
        {
            List<PieData> result = new List<PieData>();
            if (param.Mode == "RealTime")
            {

            }
            else if (param.Mode == "Week")
            {
                DateTime date = DateTime.Now.AddDays(-6);
                List<M_Statistical> list = statBll.SelectStatisticalADByToken(Token, param.ID, date.Date, DateTime.Now.Date);
                result = list.Select(c => new PieData { ID = c.ID, NAME = c.NAME, NUM = c.NUM }).ToList();
            }
            else if (param.Mode == "Month")
            {
                DateTime date = DateTime.Now.AddDays(-29);
                List<M_Statistical> list = statBll.SelectStatisticalADByToken(Token, param.ID, date.Date, DateTime.Now.Date);
                result = list.Select(c => new PieData { ID = c.ID, NAME = c.NAME, NUM = c.NUM }).ToList();
            }
            else if (param.Mode == "Year")
            {
                DateTime date = DateTime.Now.AddMonths(-11);
                List<M_Statistical> list = statBll.SelectStatisticalADByToken(Token, param.ID, date.Date, DateTime.Now.Date);
                result = list.Select(c => new PieData { ID = c.ID, NAME = c.NAME, NUM = c.NUM }).ToList();
            }
            else if (param.Mode == "Date")
            {
                DateTime sTime = DateTime.Parse(param.StartTime);
                DateTime eTime = DateTime.Parse(param.EndTime);
                int day = (eTime - sTime).Days + 1;
                List<M_Statistical> list = statBll.SelectStatisticalADByToken(Token, param.ID, sTime, eTime);
                result = list.Select(c => new PieData { ID = c.ID, NAME = c.NAME, NUM = c.NUM }).ToList();
            }
            return result;
        }

        /// <summary>
        /// 获取该机构所有设备的总人次数和当前人次数
        /// </summary>
        /// <param name="Token">令牌</param>
        /// <returns>是否获取成功</returns>
        [WebInvoke(UriTemplate = "Statistical/GetPeopleCount/{Token}", Method = "POST")]
        public M_WCF_Result<M_PeopleCount> GetPeopleCount(string Token)
        {
            M_WCF_Result<M_PeopleCount> result = new M_WCF_Result<M_PeopleCount>();

            try
            {
                SYS_USER user = uBll.SelectByToken(Token);
                M_PeopleCount peopleCount = statBll.GetPeopleCountByOID(user.OID + "");
                if (peopleCount != null)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultOBJ = peopleCount;
                    result.ResultMsg = "获取成功！";
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "获取失败！。";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "获取失败！\n" + ex.Message;
            }
            finally { }

            return result;
        }

        /// <summary>
        /// 获取单一访客的所有访问广告信息
        /// </summary>
        /// <param name="Token">令牌</param>
        /// <param name="apv">查询排序</param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "Statistical/GetAdInfoByCallingID/{Token}", Method = "POST")]
        public M_WCF_Result<List<OpenSSID_VIEW>> GetAdInfoByCallingID(string Token, APValidation apv)
        {
            M_WCF_Result<List<OpenSSID_VIEW>> result = new M_WCF_Result<List<OpenSSID_VIEW>>();
            try
            {
                if (string.IsNullOrEmpty(apv.NAME))
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "获取失败，没有该访客信息";
                    return result;
                }
                SYS_USER user = uBll.SelectByToken(Token);
                List<OpenSSID_VIEW> tmp = osBll.SelectAdInfoByCallingID(user.OID, apv.NAME, apv.COLUMN, apv.ORDERBY);
                if (tmp != null)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultOBJ = tmp;
                    result.ResultMsg = "获取成功！";
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "获取失败！。";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "获取失败！\n" + ex.Message;
            }
            finally { }

            return result;
        }

        /// <summary>
        /// 获取单一访客的上网流量信息
        /// </summary>
        /// <param name="Token">令牌</param>
        /// <param name="mac">访客标识</param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "Statistical/GetVisitOLInfoByCalingID/{Token}", Method = "POST")]
        public M_WCF_Result<M_VISIT_INFO> GetVisitOLInfoByCalingID(string Token, string mac)
        {
            M_WCF_Result<M_VISIT_INFO> result = new M_WCF_Result<M_VISIT_INFO>();
            try
            {
                if (string.IsNullOrEmpty(mac))
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "获取失败，没有该访客信息";
                    return result;
                }
                List<Int64> tmp = osBll.SelectOLInfoByCallingID(mac);
                if (tmp != null && tmp.Count > 1)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    M_VISIT_INFO info = new M_VISIT_INFO();
                    info.OLZSC = tmp[0];
                    info.OLZLL = tmp[1];
                    result.ResultOBJ = info;
                    result.ResultMsg = "获取成功！";
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "获取失败！。";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "获取失败！\n" + ex.Message;
            }
            finally { }

            return result;
        }

        /// <summary>
        /// 获取单一访客的所有认证方式
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "Statistical/GetCalingAuthentication/{Token}", Method = "POST")]
        public M_WCF_Result<List<string>> GetCalingAuthentication(string Token, string mac)
        {
            M_WCF_Result<List<string>> result = new M_WCF_Result<List<string>>();

            try
            {
                List<Int32> list = statBll.GetCheckTypeListByMac(Token, mac);

                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                if (list.Count > 0)
                    result.ResultOBJ = list.Select(c => getOlineType(c)).ToList();
                else
                    result.ResultOBJ = new List<string> { "未认证" };
                result.ResultMsg = "获取成功！";
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "获取失败！\n" + ex.Message;
            }
            finally { }

            return result;
        }

        [WebInvoke(UriTemplate = "Statistical/GetSSIDPeopleStatistical/{Token}", Method = "POST")]
        public M_WCF_Result<List<StatisticalAP>> GetSSIDPeopleStatistical(string Token, StatisticalParam param)
        {
            M_WCF_Result<List<StatisticalAP>> result = new M_WCF_Result<List<StatisticalAP>>();
            try
            {
                if (param.Mode == "RealTime")
                {

                }
                else if (param.Mode == "Week")
                {
                    DateTime date = DateTime.Now.AddDays(-6);
                    List<StatisticalAP> list = osBll.SelectSSIDPeopleStatistical(Token, param.ID, date.Date, DateTime.Now.Date);
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultOBJ = list;
                }
                else if (param.Mode == "Month")
                {
                    DateTime date = DateTime.Now.AddDays(-29);
                    List<StatisticalAP> list = osBll.SelectSSIDPeopleStatistical(Token, param.ID, date.Date, DateTime.Now.Date);
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultOBJ = list;
                }
                else if (param.Mode == "Year")
                {
                    DateTime date = DateTime.Now.AddMonths(-11);
                    List<StatisticalAP> list = osBll.SelectSSIDPeopleStatistical(Token, param.ID, date.Date, DateTime.Now.Date);
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultOBJ = list;
                }
                else if (param.Mode == "Date")
                {
                    DateTime sTime = DateTime.Parse(param.StartTime);
                    DateTime eTime = DateTime.Parse(param.EndTime);
                    int day = (eTime - sTime).Days + 1;
                    List<StatisticalAP> list = osBll.SelectSSIDPeopleStatistical(Token, param.ID, sTime, eTime);
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultOBJ = list;
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "获取失败！\n" + ex.Message;
            }
            finally { }
            return result;
        }

        [WebInvoke(UriTemplate = "Statistical/GetAuthenticationPeopleStatistical/{Token}", Method = "POST")]
        public M_WCF_Result<List<M_Statistical>> GetAuthenticationPeopleStatistical(string Token, StatisticalParam param)
        {
            M_WCF_Result<List<M_Statistical>> result = new M_WCF_Result<List<M_Statistical>>();
            try
            {
                if (param.Mode == "RealTime")
                {

                }
                else if (param.Mode == "Week")
                {
                    DateTime date = DateTime.Now.AddDays(-6);
                    List<M_Statistical> list = osBll.SelectAuthenticationPeopleStatistical(Token, param.ID, date.Date, DateTime.Now.Date);
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultOBJ = list;
                }
                else if (param.Mode == "Month")
                {
                    DateTime date = DateTime.Now.AddDays(-29);
                    List<M_Statistical> list = osBll.SelectAuthenticationPeopleStatistical(Token, param.ID, date.Date, DateTime.Now.Date);
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultOBJ = list;
                }
                else if (param.Mode == "Year")
                {
                    DateTime date = DateTime.Now.AddMonths(-11);
                    List<M_Statistical> list = osBll.SelectAuthenticationPeopleStatistical(Token, param.ID, date.Date, DateTime.Now.Date);
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultOBJ = list;
                }
                else if (param.Mode == "Date")
                {
                    DateTime sTime = DateTime.Parse(param.StartTime);
                    DateTime eTime = DateTime.Parse(param.EndTime);
                    int day = (eTime - sTime).Days + 1;
                    List<M_Statistical> list = osBll.SelectAuthenticationPeopleStatistical(Token, param.ID, sTime, eTime);
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultOBJ = list;
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "获取失败！\n" + ex.Message;
            }
            finally { }
            return result;
        }

        [WebInvoke(UriTemplate = "Statistical/GetSSIDUseTimeStatistical/{Token}", Method = "POST")]
        public M_WCF_Result<List<StatisticalAP>> GetSSIDUseTimeStatistical(string Token, StatisticalParam param)
        {
            M_WCF_Result<List<StatisticalAP>> result = new M_WCF_Result<List<StatisticalAP>>();
            try
            {
                if (param.Mode == "RealTime")
                {

                }
                else if (param.Mode == "Week")
                {
                    DateTime date = DateTime.Now.AddDays(-6);
                    List<StatisticalAP> list = osBll.SelectSSIDUseTimeStatistical(Token, param.ID, date.Date, DateTime.Now.Date);
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultOBJ = list;
                }
                else if (param.Mode == "Month")
                {
                    DateTime date = DateTime.Now.AddDays(-29);
                    List<StatisticalAP> list = osBll.SelectSSIDUseTimeStatistical(Token, param.ID, date.Date, DateTime.Now.Date);
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultOBJ = list;
                }
                else if (param.Mode == "Year")
                {
                    DateTime date = DateTime.Now.AddMonths(-11);
                    List<StatisticalAP> list = osBll.SelectSSIDUseTimeStatistical(Token, param.ID, date.Date, DateTime.Now.Date);
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultOBJ = list;
                }
                else if (param.Mode == "Date")
                {
                    DateTime sTime = DateTime.Parse(param.StartTime);
                    DateTime eTime = DateTime.Parse(param.EndTime);
                    int day = (eTime - sTime).Days + 1;
                    List<StatisticalAP> list = osBll.SelectSSIDUseTimeStatistical(Token, param.ID, sTime, eTime);
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultOBJ = list;
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "获取失败！\n" + ex.Message;
            }
            finally { }
            return result;
        }

        [WebInvoke(UriTemplate = "Statistical/GetSSIDTrafficStatistical/{Token}", Method = "POST")]
        public M_WCF_Result<List<StatisticalAP>> GetSSIDTrafficStatistical(string Token, StatisticalParam param)
        {
            M_WCF_Result<List<StatisticalAP>> result = new M_WCF_Result<List<StatisticalAP>>();
            try
            {
                if (param.Mode == "RealTime")
                {

                }
                else if (param.Mode == "Week")
                {
                    DateTime date = DateTime.Now.AddDays(-6);
                    List<StatisticalAP> list = osBll.SelectSSIDTrafficStatistical(Token, param.ID, date.Date, DateTime.Now.Date);
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultOBJ = list;
                }
                else if (param.Mode == "Month")
                {
                    DateTime date = DateTime.Now.AddDays(-29);
                    List<StatisticalAP> list = osBll.SelectSSIDTrafficStatistical(Token, param.ID, date.Date, DateTime.Now.Date);
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultOBJ = list;
                }
                else if (param.Mode == "Year")
                {
                    DateTime date = DateTime.Now.AddMonths(-11);
                    List<StatisticalAP> list = osBll.SelectSSIDTrafficStatistical(Token, param.ID, date.Date, DateTime.Now.Date);
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultOBJ = list;
                }
                else if (param.Mode == "Date")
                {
                    DateTime sTime = DateTime.Parse(param.StartTime);
                    DateTime eTime = DateTime.Parse(param.EndTime);
                    int day = (eTime - sTime).Days + 1;
                    List<StatisticalAP> list = osBll.SelectSSIDTrafficStatistical(Token, param.ID, sTime, eTime);
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultOBJ = list;
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "获取失败！\n" + ex.Message;
            }
            finally { }
            return result;
        }

        [WebInvoke(UriTemplate = "Statistical/GetAPOfADStatistical/{Token}", Method = "POST")]
        public M_WCF_Result<List<StatisticalAP>> GetAPOfADStatistical(string Token, StatisticalParam param)
        {
            M_WCF_Result<List<StatisticalAP>> result = new M_WCF_Result<List<StatisticalAP>>();
            try
            {
                if (param.Mode == "RealTime")
                {

                }
                else if (param.Mode == "Week")
                {
                    DateTime date = DateTime.Now.AddDays(-6);
                    List<StatisticalAP> list = osBll.SelectAPOfADStatistical(Token, param.ID, date.Date, DateTime.Now.Date);
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultOBJ = list;
                }
                else if (param.Mode == "Month")
                {
                    DateTime date = DateTime.Now.AddDays(-29);
                    List<StatisticalAP> list = osBll.SelectAPOfADStatistical(Token, param.ID, date.Date, DateTime.Now.Date);
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultOBJ = list;
                }
                else if (param.Mode == "Year")
                {
                    DateTime date = DateTime.Now.AddMonths(-11);
                    List<StatisticalAP> list = osBll.SelectAPOfADStatistical(Token, param.ID, date.Date, DateTime.Now.Date);
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultOBJ = list;
                }
                else if (param.Mode == "Date")
                {
                    DateTime sTime = DateTime.Parse(param.StartTime);
                    DateTime eTime = DateTime.Parse(param.EndTime);
                    int day = (eTime - sTime).Days + 1;
                    List<StatisticalAP> list = osBll.SelectAPOfADStatistical(Token, param.ID, sTime, eTime);
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultOBJ = list;
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "获取失败！\n" + ex.Message;
            }
            finally { }
            return result;
        }

        [WebInvoke(UriTemplate = "Statistical/SelectTowHourIntervalPeopleCount/{Token}", Method = "POST")]
        public M_WCF_Result<List<Int64>> SelectTowHourIntervalPeopleCount(string Token, Int64 APID)
        {
            M_WCF_Result<List<Int64>> result = new M_WCF_Result<List<Int64>>();
            try
            {
                List<Int64> list = osBll.SelectTowHourIntervalPeopleCount(Token, APID);
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                result.ResultOBJ = list;
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "获取失败！\n" + ex.Message;
            }
            finally { }
            return result;
        }

        [WebInvoke(UriTemplate = "Statistical/SelectTowHourIntervalModelCount/{Token}", Method = "POST")]
        public M_WCF_Result<List<StatisticalAP>> SelectTowHourIntervalModelCount(string Token, Int64 APID)
        {
            M_WCF_Result<List<StatisticalAP>> result = new M_WCF_Result<List<StatisticalAP>>();
            try
            {
                List<StatisticalAP> list = osBll.SelectTowHourIntervalModelCount(Token, APID);
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                result.ResultOBJ = list;
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "获取失败！\n" + ex.Message;
            }
            finally { }
            return result;
        }

        [WebInvoke(UriTemplate = "Statistical/SelectTowHourIntervalSSIDCount/{Token}", Method = "POST")]
        public M_WCF_Result<List<StatisticalAP>> SelectTowHourIntervalSSIDCount(string Token, Int64 APID)
        {
            M_WCF_Result<List<StatisticalAP>> result = new M_WCF_Result<List<StatisticalAP>>();
            try
            {
                List<StatisticalAP> list = osBll.SelectTowHourIntervalSSIDCount(Token, APID);
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                result.ResultOBJ = list;
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "获取失败！\n" + ex.Message;
            }
            finally { }
            return result;
        }
        #endregion

        #region 月统计
        [Description("总体情况")]
        [WebInvoke(UriTemplate = "StatisticalMonth/Total/{Token}", Method = "POST")]
        public M_WCF_Result<M_Month_Total> StatisticalTotal(string Token, StatisticalParam param)
        {
            M_WCF_Result<M_Month_Total> result = new M_WCF_Result<M_Month_Total>();
            try
            {
                SYS_USER user = uBll.SelectByToken(Token);

                var zonghe = osBll.GetTotalInfoByDate(user.OID, DateTime.Parse(param.StartTime));
                M_Month_Total total = new M_Month_Total()
                {
                    ZONGSWRC = zonghe.NUM[0],
                    ZONGRS = zonghe.NUM[1],
                    XIAZRS = zonghe.NUM[2],
                    GUANGGDJRC = zonghe.NUM[3],
                    XIAZRC = zonghe.NUM[4],
                    BUTCZXTXZB = new List<M_Statistical>() 
                    {
                        new M_Statistical(){NAME="iPhone", NUM=zonghe.NUM[5]},
                        new M_Statistical(){NAME="iPad", NUM = zonghe.NUM[6]},
                        new M_Statistical(){NAME="Android", NUM = zonghe.NUM[7]},
                        new M_Statistical(){NAME="Windows Phone", NUM = zonghe.NUM[8]},
                        new M_Statistical(){NAME="Windows NT", NUM = zonghe.NUM[9]},
                        new M_Statistical(){NAME="Mac OS", NUM = zonghe.NUM[10]},
                        new M_Statistical(){NAME="其他", NUM = zonghe.NUM[11]}
                    }
                };
                total.PINGJSWCS = total.ZONGRS != 0 ? (total.ZONGSWRC + 0.0) / total.ZONGRS : 0;
                total.LIANJRQXZB = (total.XIAZRS + 0.0) / total.ZONGRS;

                var shangwangsc = osBll.GetApproveTimeByAPAndDate(user.OID, -99, DateTime.Parse(param.StartTime));
                total.PINGJSWSC = shangwangsc[1];

                var rencORrens = osBll.GetVisitsByDate(user.OID, DateTime.Parse(param.StartTime));
                total.PINGJYYTXZB = rencORrens.Where(c => c.NAME != "合计").Average(c => c.NUM[2] * 100.0 / c.NUM[1]);

                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                result.ResultMsg = "成功！";
                result.ResultOBJ = total;

            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "信息读取失败！\n" + ex.Message;
            }
            return result;
        }

        [Description("用户行为与构成")]
        [WebInvoke(UriTemplate = "StatisticalMonth/UserBehavior/{Token}", Method = "POST")]
        public M_WCF_Result<List<M_Month_UserBehavior>> StatisticalUserBehavior(string Token, StatisticalParam param)
        {
            M_WCF_Result<List<M_Month_UserBehavior>> result = new M_WCF_Result<List<M_Month_UserBehavior>>();
            try
            {
                SYS_USER user = uBll.SelectByToken(Token);
                List<M_Month_UserBehavior> userBehviorList = new List<M_Month_UserBehavior>();

                //不同操作系统不同SSID的点击次数
                var butongssid = osBll.GetUserBehaviorByAPAndDate(user.OID, -99, DateTime.Parse(param.StartTime));
                M_Month_UserBehavior behvior = null;
                foreach (var item in butongssid)
                {
                    behvior = new M_Month_UserBehavior();
                    behvior.SHOUJZD = item.NAME;
                    behvior.SSID = item.VALUE;
                    userBehviorList.Add(behvior);
                }

                var butongosrenshu = osBll.GetDifferentOSPersonAPAndDate(user.OID, -99, DateTime.Parse(param.StartTime));
                Int64 sum = butongosrenshu.Sum(c => c.VALUE[0].NUM);

                foreach (var item in butongosrenshu)
                {
                    behvior = userBehviorList.Where(c => c.SHOUJZD == item.NAME).First();
                    behvior.SHUL = item.VALUE[0].NUM;
                    behvior.BIL = item.VALUE[0].NUM * 100.0 / sum;
                }

                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                result.ResultMsg = "成功！";
                result.ResultOBJ = userBehviorList;

            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "信息读取失败！\n" + ex.Message;
            }
            return result;
        }

        [Description("App下载分析，VALUE里的NUM[0]是点击量，NUM[1]是总量")]
        [WebInvoke(UriTemplate = "StatisticalMonth/AppDownload/{Token}", Method = "POST")]
        public M_WCF_Result<List<M_Month_ADOrDown>> StatisticalAppDownload(string Token, StatisticalParam param)
        {
            M_WCF_Result<List<M_Month_ADOrDown>> result = new M_WCF_Result<List<M_Month_ADOrDown>>();
            try
            {
                SYS_USER user = uBll.SelectByToken(Token);
                List<M_Month_ADOrDown> appDownloadList = new List<M_Month_ADOrDown>();

                var butongxitong = osBll.GetOSADClicksByAPAndDate(user.OID, -99, DateTime.Parse(param.StartTime), true);
                var butongyingyeting = osBll.GetOSADClicksByDate(user.OID, DateTime.Parse(param.StartTime), true);

                StatisticalAP stat = null;

                M_Month_ADOrDown diffYingYeTing = new M_Month_ADOrDown() { LEIX = "不同营业厅", VALUE = new List<StatisticalAP>() };
                var sum1 = butongyingyeting.Sum(c => c.NUM[0]);
                foreach (var item in butongyingyeting)
                {
                    stat = new StatisticalAP() { NUM = new List<long>() };
                    stat.NAME = item.NAME;
                    stat.NUM.Add(item.NUM[0]);
                    stat.NUM.Add(sum1);
                    diffYingYeTing.VALUE.Add(stat);
                }
                appDownloadList.Add(diffYingYeTing);

                M_Month_ADOrDown diffOS = new M_Month_ADOrDown() { LEIX = "不同终端", VALUE = new List<StatisticalAP>() };
                M_Month_ADOrDown diffTime = new M_Month_ADOrDown() { LEIX = "不同时间段", VALUE = new List<StatisticalAP>() };
                diffTime.VALUE.Add(new StatisticalAP() { NAME = "上午(06:00 - 12:00)", NUM = new List<long> { 0, 0 } });
                diffTime.VALUE.Add(new StatisticalAP() { NAME = "下午(12:00 - 18:00)", NUM = new List<long> { 0, 0 } });
                diffTime.VALUE.Add(new StatisticalAP() { NAME = "晚上(18:00 - 06:00)", NUM = new List<long> { 0, 0 } });

                var sum = butongxitong.Sum(c => c.NUM[0]);
                foreach (var item in butongxitong)
                {
                    diffTime.VALUE[0].NUM[0] += item.NUM[1];
                    diffTime.VALUE[1].NUM[0] += item.NUM[2];
                    diffTime.VALUE[2].NUM[0] += item.NUM[3];

                    stat = new StatisticalAP() { NUM = new List<long>() { 0, 0 } };
                    stat.NAME = item.NAME;
                    stat.NUM[0] = item.NUM[0];
                    stat.NUM[1] = sum;
                    diffOS.VALUE.Add(stat);
                }
                diffTime.VALUE[0].NUM[1] = sum;
                diffTime.VALUE[1].NUM[1] = sum;
                diffTime.VALUE[2].NUM[1] = sum;

                appDownloadList.Add(diffTime);
                appDownloadList.Add(diffOS);
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                result.ResultMsg = "成功！";
                result.ResultOBJ = appDownloadList;

            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "信息读取失败！\n" + ex.Message;
            }
            return result;
        }


        [Description("业务推广分析")]
        [WebInvoke(UriTemplate = "StatisticalMonth/Promotion/{Token}", Method = "POST")]
        public M_WCF_Result<List<M_Month_ADOrDown>> StatisticalPromotion(string Token, StatisticalParam param)
        {
            M_WCF_Result<List<M_Month_ADOrDown>> result = new M_WCF_Result<List<M_Month_ADOrDown>>();
            try
            {
                SYS_USER user = uBll.SelectByToken(Token);
                List<M_Month_ADOrDown> appDownloadList = new List<M_Month_ADOrDown>();

                var butongxitong = osBll.GetOSADClicksByAPAndDate(user.OID, -99, DateTime.Parse(param.StartTime), false);
                var butongyingyeting = osBll.GetOSADClicksByDate(user.OID, DateTime.Parse(param.StartTime), false);
                var butongguanggao = osBll.GetADByAPAndDate(user.OID, -99, DateTime.Parse(param.StartTime), false);

                StatisticalAP stat = null;

                M_Month_ADOrDown diffYingYeTing = new M_Month_ADOrDown() { LEIX = "不同营业厅", VALUE = new List<StatisticalAP>() };
                var sum = butongyingyeting.Sum(c => c.NUM[0]);
                foreach (var item in butongyingyeting)
                {
                    stat = new StatisticalAP() { NUM = new List<long>() };
                    stat.NAME = item.NAME;
                    stat.NUM.Add(item.NUM[0]);
                    stat.NUM.Add(sum);
                    diffYingYeTing.VALUE.Add(stat);
                }
                appDownloadList.Add(diffYingYeTing);

                M_Month_ADOrDown diffOS = new M_Month_ADOrDown() { LEIX = "不同终端", VALUE = new List<StatisticalAP>() };
                M_Month_ADOrDown diffTime = new M_Month_ADOrDown() { LEIX = "不同时间段", VALUE = new List<StatisticalAP>() };
                M_Month_ADOrDown diffAD = new M_Month_ADOrDown() { LEIX = "不同广告", VALUE = new List<StatisticalAP>() };

                diffTime.VALUE.Add(new StatisticalAP() { NAME = "上午(06:00 - 12:00)", NUM = new List<long> { 0, 0 } });
                diffTime.VALUE.Add(new StatisticalAP() { NAME = "下午(12:00 - 18:00)", NUM = new List<long> { 0, 0 } });
                diffTime.VALUE.Add(new StatisticalAP() { NAME = "晚上(18:00 - 06:00)", NUM = new List<long> { 0, 0 } });

                var sum1 = butongxitong.Sum(c => c.NUM[0]);
                foreach (var item in butongxitong)
                {
                    diffTime.VALUE[0].NUM[0] += item.NUM[1];
                    diffTime.VALUE[1].NUM[0] += item.NUM[2];
                    diffTime.VALUE[2].NUM[0] += item.NUM[3];
                    diffOS.VALUE.Add(new StatisticalAP() { NAME = item.NAME, NUM = new List<long>() { item.NUM[0], sum1 } });
                }

                diffTime.VALUE[0].NUM[1] = sum1;
                diffTime.VALUE[1].NUM[1] = sum1;
                diffTime.VALUE[2].NUM[1] = sum1;

                appDownloadList.Add(diffTime);
                appDownloadList.Add(diffOS);

                var num2 = butongguanggao.Sum(c => c.NUM);
                foreach (var item in butongguanggao)
                {
                    diffAD.VALUE.Add(new StatisticalAP() { NAME = item.NAME, NUM = new List<long>() { item.NUM, num2 } });
                }
                appDownloadList.Add(diffAD);
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                result.ResultMsg = "成功！";
                result.ResultOBJ = appDownloadList;

            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "信息读取失败！\n" + ex.Message;
            }
            return result;
        }

        [Description("营业厅分析")]
        [WebInvoke(UriTemplate = "StatisticalMonth/YingYeTing/{Token}", Method = "POST")]
        public M_WCF_Result<List<M_Month_YingYeTing>> StatisticalYingYeTing(string Token, StatisticalParam param)
        {
            M_WCF_Result<List<M_Month_YingYeTing>> result = new M_WCF_Result<List<M_Month_YingYeTing>>();
            try
            {
                SYS_USER user = uBll.SelectByToken(Token);
                List<M_Month_YingYeTing> list = new List<M_Month_YingYeTing>();
                M_Month_YingYeTing yingYeTing = null;
                //var butongguanggao = osBll.GetADByAPAndDate(user.OID, -99, DateTime.Parse(param.StartTime), false);

                // 总人次、总人数、下载人数、广告点击次数、下载点击次数
                var rencORrens = osBll.GetVisitsByDate(user.OID, DateTime.Parse(param.StartTime));
                foreach (var item in rencORrens)
                {
                    list.Add(new M_Month_YingYeTing()
                    {
                        MINGC = item.NAME,
                        RENC = item.NUM[0],
                        RENS = item.NUM[1],
                        XIAZRS = item.NUM[2],
                        GUANGGDJCS = item.NUM[3],
                        XIAZCS = item.NUM[4]
                        ,
                        BUTCZXTXZB = new List<M_Statistical>() 
                        {
                            new M_Statistical(){NAME="iPhone", NUM=item.NUM[5]},
                            new M_Statistical(){NAME="iPad", NUM = item.NUM[6]},
                            new M_Statistical(){NAME="Android", NUM = item.NUM[7]},
                            new M_Statistical(){NAME="Windows Phone", NUM = item.NUM[8]},
                            new M_Statistical(){NAME="Windows NT", NUM = item.NUM[9]},
                            new M_Statistical(){NAME="Mac OS", NUM = item.NUM[10]},
                            new M_Statistical(){NAME="其他", NUM = item.NUM[11]}
                        }
                    });
                }
                // 时长和平均时长
                var shicORpingjsc = osBll.GetApproveTimeByDate(user.OID, DateTime.Parse(param.StartTime));
                foreach (var item in shicORpingjsc)
                {
                    yingYeTing = list.Where(c => c.MINGC == item.NAME).First();
                    yingYeTing.PINGJFWCS = yingYeTing.RENS != 0 ? yingYeTing.RENC * 1.0 / yingYeTing.RENS : 0;
                    yingYeTing.PINGJFWSC = item.VALUE[1];
                }

                // 总和
                var zonghe = osBll.GetTotalInfoByDate(user.OID, DateTime.Parse(param.StartTime));

                M_Month_YingYeTing total = new M_Month_YingYeTing()
                {
                    MINGC = zonghe.NAME,
                    RENC = zonghe.NUM[0],
                    RENS = zonghe.NUM[1],
                    XIAZRS = zonghe.NUM[2],
                    GUANGGDJCS = zonghe.NUM[3],
                    XIAZCS = zonghe.NUM[4],
                    BUTCZXTXZB = new List<M_Statistical>() 
                    {
                        new M_Statistical(){NAME="iPhone", NUM=zonghe.NUM[5]},
                        new M_Statistical(){NAME="iPad", NUM = zonghe.NUM[6]},
                        new M_Statistical(){NAME="Android", NUM = zonghe.NUM[7]},
                        new M_Statistical(){NAME="Windows Phone", NUM = zonghe.NUM[8]},
                        new M_Statistical(){NAME="Windows NT", NUM = zonghe.NUM[9]},
                        new M_Statistical(){NAME="Mac OS", NUM = zonghe.NUM[10]},
                        new M_Statistical(){NAME="其他", NUM = zonghe.NUM[11]}
                    }
                };
                total.PINGJFWCS = total.RENS != 0 ? total.RENC * 1.0 / total.RENS : 0;
                total.PINGJFWSC = shicORpingjsc.Average(c => c.VALUE[1]);

                list.Add(total);
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                result.ResultMsg = "成功！";
                result.ResultOBJ = list;
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "信息读取失败！\n" + ex.Message;
            }
            return result;
        }

        [Description("安全分析")]
        [WebInvoke(UriTemplate = "StatisticalMonth/AnQuan/{Token}", Method = "POST")]
        public M_WCF_Result<List<M_Month_AnQuan>> StatisticalAnQuan(string Token, StatisticalParam param)
        {
            M_WCF_Result<List<M_Month_AnQuan>> result = new M_WCF_Result<List<M_Month_AnQuan>>();
            try
            {
                List<M_Month_AnQuan> list = new List<M_Month_AnQuan>();
                M_Month_AnQuan anQuan;
                List<StatisticalAP> sapList = statBll.GetAPNearStatistical(Token, DateTime.Parse(param.StartTime));

                foreach (StatisticalAP item in sapList)
                {
                    anQuan = new M_Month_AnQuan();
                    anQuan.ALIAS = item.NAME;
                    anQuan.KEY = item.NUM[0];
                    anQuan.XINR = item.NUM[1];
                    anQuan.ZHONGW = item.NUM[2];
                    anQuan.TONGY = item.NUM[3];
                    anQuan.XINZ = item.NUM[4];
                    anQuan.ZONGS = item.NUM[0] + item.NUM[1] + item.NUM[2] + item.NUM[3] + item.NUM[4];
                    list.Add(anQuan);
                }

                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                result.ResultMsg = "成功！";
                result.ResultOBJ = list;
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "信息读取失败！\n" + ex.Message;
            }
            return result;
        }
        #endregion

        #region GIS
        /// <summary>
        /// 根据令牌获取AP列表
        /// </summary>
        /// <param name="Token">用户令牌</param>
        /// <returns>AP列表</returns>
        [WebInvoke(UriTemplate = "APManage/GetAPListForGIS/{Token}", Method = "POST")]
        public M_WCF_Result<List<SYS_AP_GIS>> GetAPListForGIS(string Token)
        {
            M_WCF_Result<List<SYS_AP_GIS>> result = new M_WCF_Result<List<SYS_AP_GIS>>();
            try
            {
                SYS_USER user = uBll.SelectByToken(Token);
                List<SYS_AP_GIS> list = apBll.SelectAPListForGIS(user.OID);
                //list.ForEach(c => c.ADDRESS = "");
                if (list.Count > 0)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "获取成功！";
                    result.ResultOBJ = list;
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "获取失败！\n没有可获取的列表。";
                    result.ResultOBJ = list;
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "获取失败！\n" + ex.Message;
            }
            finally { }
            return result;
        }
        #endregion

        #region 警告信息
        /// <summary>
        /// 警告图接口
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "GetWarnGraph/{Token}", Method = "POST")]
        public M_WCF_Result<List<M_Alert_Graph>> GetWarnGraph(string Token, bool isRealTime)
        {
            M_WCF_Result<List<M_Alert_Graph>> result = new M_WCF_Result<List<M_Alert_Graph>>();
            try
            {
                Int64 oid = uBll.SelectByToken(Token).OID;
                List<M_Alert_Graph> adinfo = warnBll.getWarnGraphList(oid, isRealTime);
                if (adinfo != null)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "成功！";
                    result.ResultOBJ = adinfo;
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "信息读取失败！";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "信息读取失败！\n" + ex.Message;
            }
            return result;
        }
        /// <summary>
        /// 获取警告列表
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "GetWarnInfo/{Token}", Method = "POST")]
        public M_WCF_Result<List<M_Alert_Object>> GetWarnInfo(string Token)
        {
            M_WCF_Result<List<M_Alert_Object>> result = new M_WCF_Result<List<M_Alert_Object>>();
            try
            {
                Int64 oid = uBll.SelectByToken(Token).OID;
                List<M_Alert_Object> adinfo = warnBll.getWarnList(oid);
                if (adinfo != null)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "成功！";
                    result.ResultOBJ = adinfo;
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "信息读取失败！";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "信息读取失败！\n" + ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 获取警告条数 
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "GetAlertCount/{Token}", Method = "POST")]
        public M_WCF_Result<Int64> GetAlertCount(string Token)
        {

            M_WCF_Result<Int64> result = new M_WCF_Result<Int64>();
            Int64 oid = uBll.SelectByToken(Token).OID;
            Int64? datacount = warnBll.GetAlertCount(oid);
            if (datacount != null)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                result.ResultMsg = "成功！";
                result.ResultOBJ = (long)datacount;
            }
            else
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "信息读取失败！";
            }
            //M_WCF_Result<List<SYS_LOG_APNEAR>> result = new M_WCF_Result<List<SYS_LOG_APNEAR>>();
            //try
            //{
            //    Int64 oid = uBll.SelectByToken(Token).OID;
            //    List<SYS_LOG_APNEAR> adinfo = warnBll.getWarnList(oid);
            //    if (adinfo != null)
            //    {
            //        result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
            //        result.ResultMsg = "成功！";
            //        result.ResultOBJ = adinfo;
            //    }
            //    else
            //    {
            //        result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
            //        result.ResultMsg = "信息读取失败！";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
            //    result.ResultMsg = "信息读取失败！\n" + ex.Message;
            //}
            return result;
        }
        /// <summary>
        /// 添加白名单
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="MAC"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "AddWhiteList/{Token}", Method = "POST")]
        public M_WCF_Result<SYS_LOG_ALERTWHITELIST> AddWhiteList(string Token, string MAC)
        {
            M_WCF_Result<SYS_LOG_ALERTWHITELIST> result = new M_WCF_Result<SYS_LOG_ALERTWHITELIST>();
            try
            {
                SYS_USER user = uBll.SelectByToken(Token);
                if (warnBll.AddWhiteList(user.OID, MAC))
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "插入成功！";
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "插入失败";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "插入失败\n" + ex.Message;
            }

            return result;

        }
        /// <summary>
        /// 删除白名单
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="MAC"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "DelWhiteList/{Token}", Method = "POST")]
        public M_WCF_Result<SYS_LOG_ALERTWHITELIST> DelWhiteList(string Token, string MAC)
        {
            M_WCF_Result<SYS_LOG_ALERTWHITELIST> result = new M_WCF_Result<SYS_LOG_ALERTWHITELIST>();
            try
            {
                SYS_USER user = uBll.SelectByToken(Token);
                if (warnBll.DeleteWhiteList(user.OID, MAC))
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "删除成功！";
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "删除失败";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "删除失败\n" + ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 获取白名单列表
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="MAC"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "GetWhiteList/{Token}", Method = "POST")]
        public M_WCF_Result<List<SYS_LOG_ALERTWHITELIST>> GetWhiteList(string Token)
        {
            M_WCF_Result<List<SYS_LOG_ALERTWHITELIST>> result = new M_WCF_Result<List<SYS_LOG_ALERTWHITELIST>>();
            try
            {
                SYS_USER user = uBll.SelectByToken(Token);
                List<SYS_LOG_ALERTWHITELIST> list = warnBll.GetAlertWhiteListByOID(user.OID);
                if (list != null)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultOBJ = list;
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultOBJ = new List<SYS_LOG_ALERTWHITELIST>();
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "获取失败\n" + ex.Message;
            }

            return result;
        }


        /// <summary>
        /// 添加警告关键词
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="KeyWord"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "AddAlertKeyWord/{Token}", Method = "POST")]
        public M_WCF_Result<SYS_LOG_ALERTKEYWORD> AddAlertKeyWord(string Token, string KeyWord)
        {
            M_WCF_Result<SYS_LOG_ALERTKEYWORD> result = new M_WCF_Result<SYS_LOG_ALERTKEYWORD>();
            try
            {
                SYS_USER user = uBll.SelectByToken(Token);
                if (warnBll.AddKeyWordList(user.OID, KeyWord))
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "插入成功！";
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "插入失败";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "插入失败\n" + ex.Message;
            }

            return result;

        }
        /// <summary>
        /// 删除警告关键词
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="KeyWord"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "DelAlertKeyWord/{Token}", Method = "POST")]
        public M_WCF_Result<SYS_LOG_ALERTKEYWORD> DelAlertKeyWord(string Token, Int64 ID)
        {
            M_WCF_Result<SYS_LOG_ALERTKEYWORD> result = new M_WCF_Result<SYS_LOG_ALERTKEYWORD>();
            try
            {
                SYS_USER user = uBll.SelectByToken(Token);
                if (warnBll.DeleteKeyWordList(ID))
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "删除成功！";
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "删除失败";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "删除失败\n" + ex.Message;
            }

            return result;
        }


        /// <summary>
        /// 获取警告关键词列表
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="KeyWord"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "GetAlertKeyWord/{Token}", Method = "POST")]
        public M_WCF_Result<List<SYS_LOG_ALERTKEYWORD>> GetAlertKeyWord(string Token)
        {
            M_WCF_Result<List<SYS_LOG_ALERTKEYWORD>> result = new M_WCF_Result<List<SYS_LOG_ALERTKEYWORD>>();
            try
            {
                SYS_USER user = uBll.SelectByToken(Token);
                List<SYS_LOG_ALERTKEYWORD> list = warnBll.GetAlertKeyWordByOID(user.OID);
                if (list != null)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultOBJ = list;
                }

                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultOBJ = new List<SYS_LOG_ALERTKEYWORD>();
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "获取失败\n" + ex.Message;
            }
            return result;

        }

        /// <summary>
        ///获取未处理的警告列表
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "GetAlertListNotHandle/{Token}", Method = "POST")]
        public M_WCF_Result<List<M_Alert_Object>> GetAlertListNotHandle(string Token)
        {
            M_WCF_Result<List<M_Alert_Object>> result = new M_WCF_Result<List<M_Alert_Object>>();

            try
            {
                SYS_USER user = uBll.SelectByToken(Token);
                List<M_Alert_Object> list = warnBll.GetAlertListNotHandle(user.OID);
                if (list.Count > 0)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "读取成功！";
                    result.ResultOBJ = list;
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "没有可读取的数据！";
                    result.ResultOBJ = list;
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "获取失败！\n" + ex.Message;
            }
            finally { }
            return result;
        }

        /// <summary>
        ///获取未处理的警告列表
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "GetAlertListByMAC/{MAC}/{Token}", Method = "POST")]
        public M_WCF_Result<List<M_Alert_Object>> GetAlertListByMAC(string Token, string MAC)
        {
            M_WCF_Result<List<M_Alert_Object>> result = new M_WCF_Result<List<M_Alert_Object>>();

            try
            {
                SYS_USER user = uBll.SelectByToken(Token);
                List<M_Alert_Object> list = warnBll.GetAlertListByMAC(user.OID, MAC);
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                result.ResultMsg = "读取成功！";
                result.ResultOBJ = list;
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "获取失败！\n" + ex.Message;
            }
            finally { }
            return result;
        }
        #endregion

        #region 安全页面接口
        [WebInvoke(UriTemplate = "GetFilterSSIDInfo/{curPage}/{pageSize}/{Token}", Method = "POST")]
        [Description("获取过滤之后SSID列表信息")]
        public M_WCF_Result<M_SECURITY_SSID_Page> GetFilterSSIDInfo(string Token, string pageSize, string curPage, M_SECURITY_SSID_Filter filter)
        {
            M_WCF_Result<M_SECURITY_SSID_Page> result = new M_WCF_Result<M_SECURITY_SSID_Page>();
            try
            {
                Int64 oid = (filter.apid == 0 ? uBll.SelectByToken(Token).OID : filter.apid);
                M_SECURITY_SSID_Page ssidPage = new M_SECURITY_SSID_Page();
                List<M_SECURITY_SSID> ssidInfo = warnBll.GetFilterSSIDList(oid, int.Parse(pageSize), int.Parse(curPage), filter);
                ssidPage.ssidList = ssidInfo;
                ssidPage.AllCount = warnBll.GetFilterSSIDListCounts(oid, filter);
                if (ssidInfo != null)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "成功！";
                    result.ResultOBJ = ssidPage;
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "信息读取失败！";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "信息读取失败！\n" + ex.Message;
            }
            return result;
        }

        [WebInvoke(UriTemplate = "GetFilterSSIDInfoByAPID/{curPage}/{pageSize}/{Token}", Method = "POST")]
        [Description("根据设备ID获取过滤之后SSID列表信息")]
        public M_WCF_Result<M_SECURITY_SSID_Page> GetFilterSSIDInfoByAPID(string Token, string pageSize, string curPage, M_SECURITY_SSID_Filter filter)
        {
            M_WCF_Result<M_SECURITY_SSID_Page> result = new M_WCF_Result<M_SECURITY_SSID_Page>();
            try
            {
                Int64 oid = uBll.SelectByToken(Token).OID;
                M_SECURITY_SSID_Page ssidPage = new M_SECURITY_SSID_Page();
                List<M_SECURITY_SSID> ssidInfo = warnBll.GetFilterSSIDListByAPID(oid, int.Parse(pageSize), int.Parse(curPage), filter);
                ssidPage.ssidList = ssidInfo;
                ssidPage.AllCount = 0;//warnBll.GetFilterSSIDListCountsByAPID(oid, filter);
                if (ssidInfo != null)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "成功！";
                    result.ResultOBJ = ssidPage;
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "信息读取失败！";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "信息读取失败！\n" + ex.Message;
            }
            return result;
        }

        [WebInvoke(UriTemplate = "GetSameSSIDInfo/{Token}", Method = "POST")]
        [Description("获取同名SSID列表")]
        public M_WCF_Result<List<M_SECURITY_SSID>> GetSameSSIDInfo(string Token, SSIDNameSave ssidName)
        {
            M_WCF_Result<List<M_SECURITY_SSID>> result = new M_WCF_Result<List<M_SECURITY_SSID>>();
            try
            {
                Int64 oid = uBll.SelectByToken(Token).OID;
                List<M_SECURITY_SSID> ssidInfo = warnBll.GetSameSSIDList(oid, ssidName.Name, ssidName.ID);
                if (ssidInfo != null)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "成功！";
                    result.ResultOBJ = ssidInfo;
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "信息读取失败！";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "信息读取失败！\n" + ex.Message;
            }
            return result;
        }

        [WebInvoke(UriTemplate = "GetAPContactByOID/{Token}", Method = "POST")]
        [Description("获取告警联系人信息")]
        public M_WCF_Result<List<SYS_APCONTACT>> GetAPContactByOID(string Token)
        {
            M_WCF_Result<List<SYS_APCONTACT>> result = new M_WCF_Result<List<SYS_APCONTACT>>();
            try
            {
                Int64 oid = uBll.SelectByToken(Token).OID;
                List<SYS_APCONTACT> ssidInfo = warnBll.GetAPContactByOID(oid);
                if (ssidInfo != null)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "成功！";
                    result.ResultOBJ = ssidInfo;
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "信息读取失败！";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "信息读取失败！\n" + ex.Message;
            }
            return result;
        }


        [WebInvoke(UriTemplate = "AddAPContact/{Token}", Method = "POST")]
        [Description("添加告警联系人信息")]
        public M_WCF_Result<int> AddAPContact(string Token, SYS_APCONTACT apcontact)
        {
            M_WCF_Result<int> result = new M_WCF_Result<int>();
            try
            {
                Int64 oid = uBll.SelectByToken(Token).OID;
                apcontact.ID = -1;
                apcontact.OID = oid;
                apcontact.CREATETIME = DateTime.Now;

                if (warnBll.AddAPContact(apcontact))
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "成功！";
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "添加失败！";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "信息读取失败！\n" + ex.Message;
            }
            return result;
        }

        [WebInvoke(UriTemplate = "SaveAPContact/{Token}", Method = "POST")]
        [Description("保存告警联系人信息")]
        public M_WCF_Result<int> SaveAPContact(string Token, SYS_APCONTACT apcontact)
        {
            M_WCF_Result<int> result = new M_WCF_Result<int>();
            try
            {
                //Int64 oid = uBll.SelectByToken(Token).OID;
                //apcontact.OID = oid;
                //apcontact.CREATETIME = DateTime.Now;

                if (warnBll.SaveAPContact(apcontact))
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "成功！";
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "保存失败！";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "信息读取失败！\n" + ex.Message;
            }
            return result;
        }

        [WebInvoke(UriTemplate = "DelAPContact/{Token}", Method = "POST")]
        [Description("删除告警联系人信息")]
        public M_WCF_Result<int> DelAPContact(string Token, Int64 id)
        {
            M_WCF_Result<int> result = new M_WCF_Result<int>();
            try
            {
                if (warnBll.DelAPContact(id))
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "成功！";
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "删除失败！";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "信息读取失败！\n" + ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 获取告警列表统计
        /// </summary>
        /// <param name="Token">令牌</param>
        /// <param name="OID">机构ID</param>
        /// <returns>元素0:可疑,元素1:中文,元素:2新增</returns>
        [WebInvoke(UriTemplate = "GetAPNearCountByOID/{Token}", Method = "POST")]
        [Description("获取告警列表统计")]
        public M_WCF_Result<List<Int64>> GetAPNearCountByOID(string Token, M_SECURITY_SSID_Filter filter)
        {
            M_WCF_Result<List<Int64>> result = new M_WCF_Result<List<Int64>>();
            try
            {
                Int64 OID = uBll.SelectByToken(Token).OID;
                if (OID == 0)
                    OID = uBll.SelectByToken(Token).OID;

                List<Int64> list = warnBll.GetAPNearCountByOID(OID, filter);
                if (list.Count > 0)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultOBJ = list;
                    result.ResultMsg = "成功！";
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultOBJ = list;
                    result.ResultMsg = "删除失败！";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "信息读取失败！\n" + ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 获取告警列表统计
        /// </summary>
        /// <param name="Token">令牌</param>
        /// <param name="OID">机构ID</param>
        /// <returns>元素0:可疑,元素1:中文,元素:2新增</returns>
        [WebInvoke(UriTemplate = "GetAPNearCountByAPID/{Token}", Method = "POST")]
        [Description("获取告警列表统计")]
        public M_WCF_Result<List<Int64>> GetAPNearCountByAPID(string Token, M_SECURITY_SSID_Filter filter)
        {
            M_WCF_Result<List<Int64>> result = new M_WCF_Result<List<Int64>>();
            try
            {
                Int64 OID = uBll.SelectByToken(Token).OID;
                if (OID == 0)
                    OID = uBll.SelectByToken(Token).OID;

                List<Int64> list = warnBll.GetAPNearCountByAPID(OID, filter);
                if (list.Count > 0)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultOBJ = list;
                    result.ResultMsg = "成功！";
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultOBJ = list;
                    result.ResultMsg = "删除失败！";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "信息读取失败！\n" + ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 处理告警之通知
        /// </summary>
        /// <param name="Token">令牌</param>
        /// <param name="LOG_ID">告警ID</param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "ProcessForNotice/{Token}", Method = "POST")]
        [Description("处理告警之通知")]
        public M_WCF_Result<Int64> ProcessForNotice(string Token, Int64 LOG_ID)
        {
            M_WCF_Result<Int64> result = new M_WCF_Result<Int64>();
            try
            {
                if (warnBll.ProcessForNotice(LOG_ID))
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "通知成功！";
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "暂未开通";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "信息读取失败！\n" + ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 处理告警之白名单
        /// </summary>
        /// <param name="Token">令牌</param>
        /// <param name="LOG_ID">告警ID</param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "ProcessForWhiteList/{Token}", Method = "POST")]
        [Description("处理告警之白名单")]
        public M_WCF_Result<List<Int64>> ProcessForWhiteList(string Token, Int64 LOG_ID)
        {
            M_WCF_Result<List<Int64>> result = new M_WCF_Result<List<Int64>>();
            try
            {
                if (warnBll.ProcessForWhiteList(LOG_ID))
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "处理成功！";
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "处理失败！";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "处理失败！\n" + ex.Message;
            }
            return result;
        }
        #endregion

        #region 分享
        [WebInvoke(UriTemplate = "ShareAD", Method = "POST")]
        [Description("更新分享记录")]
        public M_WCF_Result<string> ShareAD(SHARE_INFO info)
        {
            M_WCF_Result<string> result = new M_WCF_Result<string>();
            try
            {

                if (s_infoBll.GetShareInfoByUserName(info.SESSION, info.SSID, info.OID, info.ADID, info.PATH, info.SHARETYPE) != null)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "成功！";
                }
                else
                {
                    info.ID = -1;
                    info.VISITCOUNT = 0;
                    info.CREATETIME = DateTime.Now;
                    info.UPDATETIME = DateTime.Now;
                    if (s_infoBll.Update(info))
                    {
                        result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                        result.ResultMsg = "成功！";
                    }
                    else
                    {
                        result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                        result.ResultMsg = "无此记录！";
                    }
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "信息读取失败！\n" + ex.Message;
            }
            return result;
        }

        [WebInvoke(UriTemplate = "ShareCount", Method = "POST")]
        [Description("更新分享访问次数记录")]
        public M_WCF_Result<String> ShareCount(SHARE_INFO info)
        {
            M_WCF_Result<string> result = new M_WCF_Result<string>();
            try
            {
                SHARE_INFO sInfo = s_infoBll.GetShareInfoByUserName(info.PSESSION, info.SSID, info.OID, info.ADID, info.PATH, info.SHARETYPE);
                if (sInfo != null)
                {
                    s_infoBll.ShareCount(sInfo);
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "成功！";
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "信息读取失败！";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "信息读取失败！\n" + ex.Message;
            }
            return result;
        }
        #endregion

        [WebInvoke(UriTemplate = "GetOrgNameByLatLon/{Token}", Method = "POST")]
        [Description("传入一个经纬度返回最近的营业厅名称")]
        public M_WCF_Result<SYS_APDEVICE> GetOrgByLatLon(string Token, M_LOCATION Location)
        {
            M_WCF_Result<SYS_APDEVICE> result = new M_WCF_Result<SYS_APDEVICE>();
            try
            {
                SYS_APDEVICE res = apBll.GetOrgNameByLatLon(Location);
                if (res != null)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "成功！";
                    result.ResultOBJ = res;
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "信息读取失败！";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "信息读取失败！\n" + ex.Message;
            }
            return result;
        }


        #region 告警提醒
        [WebInvoke(UriTemplate = "IgnoreDeviceAlarm/{APID}/{type}", Method = "GET")]
        [Description("今天指定设备不再进行告警提醒")]
        public string IgnoreDeviceAlarm(string APID, string type)
        {
            if (esperBll.IgnoreDevice(APID, type))
                return "操作成功，今天不会再对您选择的设备进行" + (type == "1" ? "宕机" : "访问异常") + "提醒了";
            else
                return "操作失败，请尝试刷新";
        }
        #endregion

        #region 安装
        [WebInvoke(UriTemplate = "Install/{Token}", Method = "POST")]
        [Description("安装服务")]
        public M_WCF_Result<string> Install(string Token, M_INSTALL install)
        {
            M_WCF_Result<string> result = new M_WCF_Result<string>();
            try
            {
                if (apBll.Install(install, Token))
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "安装成功！";
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "安装失败！";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "安装失败！\n" + ex.Message;
            }
            return result;
        }


        [WebInvoke(UriTemplate = "CheckInstallPerson/{ssid}/{mac}", Method = "POST")]
        [Description("是否是安装人员的设备")]
        public M_WCF_Result<M_INSTALLCHECK> CheckInstallPerson(string ssid, string mac)
        {
            M_WCF_Result<M_INSTALLCHECK> result = new M_WCF_Result<M_INSTALLCHECK>();
            try
            {
                M_INSTALLCHECK installcheck = intallBll.InstallCheck(Convert.ToInt64(ssid), mac);
                if (installcheck != null)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "获取成功！";
                    result.ResultOBJ = installcheck;
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "您的手持设备没有权限进行安装！";
                    result.ResultOBJ = installcheck;
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "认证错误！\n" + ex.Message;
            }
            return result;
        }

        [WebInvoke(UriTemplate = "LoginInstall", Method = "POST")]
        [Description("安装人员登录")]
        public M_WCF_Result<string> LoginInstall(UserLogin uLogin)
        {
            M_WCF_Result<string> result = new M_WCF_Result<string>();

            try
            {
                Entity.SYS_USER user = uBll.Select(uLogin.ACCOUNT, uLogin.PWD, uLogin.MAC);
                if (user != null)
                {
                    user.TOKEN = Guid.NewGuid().ToString("N");
                    user.TOKENTIMESTAMP = DateTime.Now.AddHours(12);

                    if (uBll.Update(user))
                    {
                        result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                        result.ResultOBJ = user.TOKEN;
                        result.ResultMsg = "登录成功！";
                    }
                    else
                    {
                        result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                        result.ResultMsg = "登录失败！\n令牌生成失败。";
                    }
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "登录失败！\n用户名或密码错误。";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "登录失败！\n" + ex.Message;
            }
            finally { }

            return result;
        }
        #endregion

        #region Radius
        /*
        [WebInvoke(UriTemplate = "Radius/RadiusAuth/", Method = "POST")]
        [Description("Radius验证")]
        public M_WCF_Result<RadiusAuthResult> RadiusAuth(RadiusAuth radiusAuth)
        {
            M_WCF_Result<RadiusAuthResult> result = new M_WCF_Result<RadiusAuthResult>();
            try
            {
                RadiusAuthResult res = radiusBll.GetRadiusAuth(radiusAuth);
                if (res != null)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "成功！";
                    result.ResultOBJ = res;
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "失败！";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "失败！\n" + ex.Message;
            }
            return result;
        }*/
        #endregion

        #region 前台审核页
        [WebInvoke(UriTemplate = "GetAuditList/{size}/{curPage}/{Token}", Method = "POST")]
        [Description("获取待审核的数据列表")]
        public M_WCF_Result<M_AD_AUDIT> FindAuditList(string token,string size, string curPage, int statu)
        {
            BLL_AuditManage auditBll = new BLL_AuditManage();
            M_WCF_Result<M_AD_AUDIT> result = new M_WCF_Result<M_AD_AUDIT>();
            try
            {
                int _size = Convert.ToInt32(size);
                int _curPage = Convert.ToInt32(curPage);
                SYS_USER user = uBll.SelectByToken(token);
                result.ResultCode = 0;
                result.ResultOBJ = auditBll.SelectAuditByPage(user.OID, _size, _curPage, statu);
            }
            catch (Exception e)
            {
                result.ResultCode = 1;
                result.ResultMsg = e.Message;
            }
            return result;
        }

        [WebInvoke(UriTemplate = "GetAuditHistoryList/{size}/{curPage}/{Token}", Method = "POST")]
        [Description("获取历史审核的数据列表")]
        public M_WCF_Result<M_AD_AUDIT> FindAuditHistoryList(string token, string size, string curPage, int statu)
        {
            BLL_AuditManage auditBll = new BLL_AuditManage();
            M_WCF_Result<M_AD_AUDIT> result = new M_WCF_Result<M_AD_AUDIT>();
            try
            {
                int _size = Convert.ToInt32(size);
                int _curPage = Convert.ToInt32(curPage);
                SYS_USER user = uBll.SelectByToken(token);
                result.ResultCode = 0;
                result.ResultOBJ = auditBll.SelectAuditHisrtoryByPage(user.OID, _size, _curPage, statu);
            }
            catch (Exception e)
            {
                result.ResultCode = 1;
                result.ResultMsg = e.Message;
            }
            return result;
        }

        /// <summary>
        /// 获取审核进度
        /// </summary>
        /// <param name="audid"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "GetAuditProgress/{Token}", Method = "POST")]
        [Description("获取审核的数据列表")]
        public M_WCF_Result<List<AD_AUDIT>> GetAuditProgress(string Token, long audid)
        {
            BLL_AuditManage auditBll = new BLL_AuditManage();
            M_WCF_Result<List<AD_AUDIT>> result = new M_WCF_Result<List<AD_AUDIT>>();
            try
            {
                result.ResultCode = 0;
                result.ResultOBJ = auditBll.GetAuditProgress(audid);
                return result;
            }
            catch (Exception e)
            {
                result.ResultCode = 1;
                result.ResultMsg = e.Message;
                return result;
            }
        }

        /// <summary>
        /// 获取审核进度
        /// </summary>
        /// <param name="audid"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "GetAuditHistoryProgress/{Token}", Method = "POST")]
        [Description("获取历史审核的审核进度列表")]
        public M_WCF_Result<List<AD_AUDIT>> GetAuditHistoryProgress(string Token, long audid)
        {
            BLL_AuditManage auditBll = new BLL_AuditManage();
            M_WCF_Result<List<AD_AUDIT>> result = new M_WCF_Result<List<AD_AUDIT>>();
            try
            {
                result.ResultCode = 0;
                result.ResultOBJ = auditBll.GetAuditHistoryProgress(audid);
                return result;
            }
            catch (Exception e)
            {
                result.ResultCode = 1;
                result.ResultMsg = e.Message;
                return result;
            }
        }

        [WebInvoke(UriTemplate = "HandleAudit/{Token}", Method = "POST")]
        [Description("处理审核")]
        public M_WCF_Result<string> HandleAudit(string Token, M_HandleAudit handle)
        {
            BLL.BLL_AuditManage auditBll = new BLL.BLL_AuditManage();
            BLL.BLL_DeviceService dsBll = new BLL.BLL_DeviceService();

            M_WCF_Result<string> result = new M_WCF_Result<string>();

            try
            {
                SYS_USER user = uBll.SelectByToken(Token);

                bool flag = dsBll.UpdateFreeHost(handle.adId, handle.freehost, handle.defaultfree);
                //handle 1通过  2不通过
                String resultstr = auditBll.HandleAudit(user.OID, user.USERNAME, handle.audid, handle.handle, handle.auditstr);
                
                result.ResultMsg = resultstr;
                if (resultstr == "ok" && flag)
                {
                    result.ResultCode = 0;
                    result.ResultMsg = "审核成功";
                }
                else
                {
                    result.ResultCode = 1;
                    result.ResultMsg = resultstr;
                }
            }
            catch (Exception e)
            {

                result.ResultCode = 1;
                result.ResultMsg = "审核失败!\n" + e.Message;
            }
            return result;
        }
        #endregion

        #region 三方接口调用
        #region 笑话
        /// <summary>
        /// 获取今日搞笑段子
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "TripartiteAPI/RequestTodayJoke/{limit}", Method = "POST")]
        public M_WCF_Result<string> RequestTodayJoke(string limit)
        {
            M_WCF_Result<string> result = new M_WCF_Result<string>();

            List<string> list = new List<string>();
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            StreamReader sr = null;
            Int32 count = 0;

            Int32 tmpLimit = Convert.ToInt32(limit);
            for (int i = 0; i < tmpLimit; i++)
            {
                try
                {
                    if (count > 5)
                    {
                        count = 0;
                        continue;
                    }
                    request = (HttpWebRequest)WebRequest.Create("http://xjjapi.duapp.com/api/show.action?m=joke");
                    request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
                    response = (HttpWebResponse)request.GetResponse();
                    sr = new StreamReader(response.GetResponseStream());
                    list.Add(sr.ReadToEnd());
                    tripBll.InsertTodayJoke(list);
                    list.Clear();
                }
                catch (Exception ex) { i--; count++; }
                finally { }
            }

            
            
            result.ResultCode = 0;            
            return result;
        }

        [WebInvoke(UriTemplate = "TripartiteAPI/GetTodayJokeByPage/{size}", Method = "POST")]
        public M_WCF_Result<List<TAPI_TodayJoke>> GetTodayJokeByPage(string size, ObjectId lastkey)
        {
            M_WCF_Result<List<TAPI_TodayJoke>> result = new M_WCF_Result<List<TAPI_TodayJoke>>();
            List<TAPI_TodayJoke> list = new List<TAPI_TodayJoke>();
            Int32 tmpSize = Convert.ToInt32(size);

            try
            {
                if (lastkey == ObjectId.Empty)
                    list = tripBll.SelectTodayJoke(null, tmpSize);
                else
                    list = tripBll.SelectTodayJoke(lastkey, tmpSize);
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                result.ResultOBJ = list;
                if (list.Count > 0)
                    result.ResultMsg = "获取成功！";
                else
                    result.ResultMsg = "获取失败，\r\n没有相关信息！";
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "获取失败！\n" + ex.Message;
            }
            finally { }

            return result;
        }

        /// <summary>
        /// 获取今日搞笑图片
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "TripartiteAPI/RequestTodayImage/{limit}", Method = "POST")]
        public M_WCF_Result<string> RequestTodayImage(string limit)
        {
            M_WCF_Result<string> result = new M_WCF_Result<string>();

            List<string> list = new List<string>();
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            StreamReader sr = null;

            Int32 count = 0;

            Int32 tmpLimit = Convert.ToInt32(limit);
            for (int i = 0; i < tmpLimit; i++)
            {
                try
                {
                    if (count > 5)
                    {
                        count = 0;
                        continue;
                    }
                    request = (HttpWebRequest)WebRequest.Create("http://xjjapi.duapp.com/api/show.action?m=image");
                    request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
                    response = (HttpWebResponse)request.GetResponse();
                    sr = new StreamReader(response.GetResponseStream());
                    list.Add(sr.ReadToEnd());
                    tripBll.InsertTodayImage(list);
                    list.Clear();
                }
                catch (Exception ex) { i--; count++; }
                finally { }
            }

            

            return result;
        }

        [WebInvoke(UriTemplate = "TripartiteAPI/GetTodayImageByPage/{size}", Method = "POST")]
        public M_WCF_Result<List<TAPI_TodayImage>> GetTodayImageByPage(string size, ObjectId lastkey)
        {
            M_WCF_Result<List<TAPI_TodayImage>> result = new M_WCF_Result<List<TAPI_TodayImage>>();
            List<TAPI_TodayImage> list = new List<TAPI_TodayImage>();
            Int32 tmpSize = Convert.ToInt32(size);

            try
            {
                if (lastkey == ObjectId.Empty)
                    list = tripBll.SelectTodayImage(null, tmpSize);
                else
                    list = tripBll.SelectTodayImage(lastkey, tmpSize);

                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                result.ResultOBJ = list;
                if (list.Count > 0)
                    result.ResultMsg = "获取成功！";
                else
                    result.ResultMsg = "获取失败，\r\n没有相关信息！";
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "获取失败！\n" + ex.Message;
            }
            finally { }

            return result;
        }
        #endregion

        #region 优酷
        [WebInvoke(UriTemplate = "TripartiteAPI/GetYouKuShowsCategory/{category}/{count}/{page}", Method = "POST")]
        public M_WCF_Result<TAPI_YOUKU_SHOWCATEGORY> GetYouKuShowsList(string category, string count, string page)
        {
            M_WCF_Result<TAPI_YOUKU_SHOWCATEGORY> result = new M_WCF_Result<TAPI_YOUKU_SHOWCATEGORY>();
            TAPI_YOUKU_SHOWCATEGORY item = null;
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            StreamReader sr = null;

            string client_id = "f05939a00d3f4adb";

            try
            {
                request = (HttpWebRequest)WebRequest.Create("https://openapi.youku.com/v2/shows/by_category.json?client_id=" + client_id + "&category=" + category + "&count=" + count + "&page=" + page);
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
                response = (HttpWebResponse)request.GetResponse();
                sr = new StreamReader(response.GetResponseStream());
                item = JsonConvert.DeserializeObject<TAPI_YOUKU_SHOWCATEGORY>(sr.ReadToEnd());
                result.ResultCode = 0;
                result.ResultOBJ = item;
            }
            catch (Exception ex)
            {
                result.ResultCode = 1;
                result.ResultMsg = ex.Message;
            }
            finally { }

            return result;
        }
        #endregion

        #region 大众点评
        /// <summary>
        /// SHA1加密字符串
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <returns>加密后的字符串</returns> 
        public static string SHA1(string source)
        {
            byte[] value = Encoding.UTF8.GetBytes(source);
            SHA1 sha = new SHA1CryptoServiceProvider();
            byte[] result = sha.ComputeHash(value);

            string delimitedHexHash = BitConverter.ToString(result);
            string hexHash = delimitedHexHash.Replace("-", "");

            return hexHash;
        }

        [WebInvoke(UriTemplate = "TripartiteAPI/GetDianPingFindDealsList/{city}/{latitude}/{longitude}/{limit}/{page}", Method = "POST")]
        public M_WCF_Result<TAPI_DIANPING_FINDDEALS> GetDianPingFindDealsList(string city, string latitude, string longitude, string limit, string page)
        {
            M_WCF_Result<TAPI_DIANPING_FINDDEALS> result = new M_WCF_Result<TAPI_DIANPING_FINDDEALS>();
            TAPI_DIANPING_FINDDEALS item = null;
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            StreamReader sr = null;

            #region 大众点评加密操作
            string appkey = "12523998";
            string secret = "0688a756a7e24571834173a21e7c0ca3";
            string sign = "";
            string param = "";

            Hashtable ht = new Hashtable();
            ht.Add("city", city);
            ht.Add("latitude", latitude);
            ht.Add("longitude", longitude);
            ht.Add("radius", "5000");
            ht.Add("limit", limit);
            ht.Add("page", page);

            //参数按照参数名排序
            ArrayList akeys = new ArrayList(ht.Keys);
            akeys.Sort();

            //拼接字符串
            foreach (string skey in akeys)
            {
                sign += skey + ht[skey].ToString();
                param += "&" + skey + "=" + Utf8Encode(ht[skey].ToString());
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(appkey);
            sb.Append(sign);
            sb.Append(secret);
            sign = sb.ToString();
            #endregion

            try
            {
                request = (HttpWebRequest)WebRequest.Create("http://api.dianping.com/v1/deal/find_deals?appkey=" + appkey + "&sign=" + SHA1(sign) + param);
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
                response = (HttpWebResponse)request.GetResponse();
                sr = new StreamReader(response.GetResponseStream());
                item = JsonConvert.DeserializeObject<TAPI_DIANPING_FINDDEALS>(sr.ReadToEnd());
                result.ResultCode = 0;
                result.ResultOBJ = item;
            }
            catch (Exception ex)
            {
                result.ResultCode = 1;
                result.ResultMsg = ex.Message;
            }
            finally { }

            return result;
        }
        #endregion
        #endregion
    }
}
