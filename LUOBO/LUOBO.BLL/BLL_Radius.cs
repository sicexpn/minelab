using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.DAL;
using LUOBO.Entity;
using System.Transactions;
using LUOBO.Helper;
using LUOBO.Model;

namespace LUOBO.BLL
{
    public class BLL_Radius
    {
        //public DAL_RadAcct dal_radAcct = new DAL_RadAcct();
        public BLL_OpenSSID_Statistical bll_openSSID_statical = new BLL_OpenSSID_Statistical();
        public DAL_RadCheck dal_radCheck = new DAL_RadCheck();
        public PasswordGenerator pwdGenete = new PasswordGenerator();
        public DAL_RadGroupReply dal_radGroupReply = new DAL_RadGroupReply();
        public DAL_UserGroup dal_userGroup = new DAL_UserGroup();
        public DAL_BlackList dal_blackList = new DAL_BlackList();
        public DAL_RadCheck_Log dal_radChek_log = new DAL_RadCheck_Log();
        /// <summary>
        /// 从数据库radCheck中查找用户名，若存在，返回ture，否则返回false
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool CheckUserName(UserLogin userLogin)
        {
            int count = dal_radCheck.Select(userLogin);
            if (count > 0)
            {
                return true;
            }
            else
                return false;
        }
        /// <summary>
        /// 用户名存在数据库radcheck的情况：
        /// 根据userName从raccheck中选择value，此时字段Attribute应该为User-Password
        /// </summary>
        /// <returns></returns>
        public string SelectPassword(UserLogin userLogin)
        {
            return dal_radCheck.SelectPassword(userLogin);
        }


        public bool Insert(OpenSSID openSSID)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                bool flag = false;
                try
                {
                    RadGroupReply radGroupReply = new RadGroupReply();
                    radGroupReply.GroupName = openSSID.CalledStationId;//APID作为用户组名称
                    radGroupReply.Attribute = "User-Password";//待修改
                    radGroupReply.Value = "xxxx";//
                    flag = dal_radGroupReply.Insert(radGroupReply);
                    //flag = dal_openSSID.Insert(openSSID);
                    //flag = bll_openSSID_statical.Insert(openSSID);//向统计数据库添加对应的用户数据
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("错误原因是：" + ex.Message);
                }
                return flag;
            }
        }
        /// <summary>
        /// 检查session一致性
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public bool CheckSessionId(string sessionId)
        {
            return false;// dal_openSSID.CheckSessionId(sessionId);
        }
        /// <summary>
        /// 检查用户名，若存在，返回密码，否则生成新密码并将用户加入表中
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        public string GetPassword(UserLogin userLogin)
        {
            if (CheckUserName(userLogin))
            {
                return SelectPassword(userLogin);
            }
            else
            {
                return SelectAndInsertPassword(userLogin);
            }
        }

        public string SelectAndInsertPassword(UserLogin userLogin)
        {
            //return dal_radCheck.SelectAndInsertPassword(userLogin);
            bool flag = false;
            string pwd = pwdGenete.Generate();
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    UserGroup userGroup = new UserGroup();
                    userGroup.UserName = userLogin.UserName;
                    userGroup.GroupName = userLogin.CalledStationId;//APId作为组名
                    userGroup.Priority = 1;
                    flag = dal_userGroup.Insert(userGroup);//向userGroup中加入数据

                    flag = dal_radCheck.Insert(userLogin, pwd);

                    scope.Complete();
                    return pwd;
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("错误原因是：" + ex.Message);
                }
            }
        }

        public List<OpenSSID> SelectAllOpenSSID()
        {

            string s = RadiusAttribute.Simultaneous_Use;
            return null;// dal_openSSID.Select();

        }
        /// <summary>
        /// Ap组控制流量、连接人数、时间控制
        /// </summary>
        /// <param name="apControl"></param>
        /// <returns></returns>
        public bool Insert(APControl apControl)
        {
            //RadiusAttribute attr = new RadiusAttribute();
            List<RadGroupReply> list = new List<RadGroupReply>();
            RadGroupReply data = new RadGroupReply();

            data.GroupName = apControl.CalledStationId;
            data.Attribute = RadiusAttribute.Acct_Input_Octets;
            data.Value = apControl.Acct_Input_Octets;
            list.Add(data);

            data.Attribute = RadiusAttribute.Acct_Output_Octets;
            data.Value = apControl.Acct_Output_Octets;
            list.Add(data);

            data.Attribute = RadiusAttribute.Acct_Interim_Interval;
            data.Value = apControl.Acct_Interim_Interval;
            list.Add(data);

            data.Attribute = RadiusAttribute.Session_Timeout;
            data.Value = apControl.Session_Timeout;
            list.Add(data);

            data.Attribute = RadiusAttribute.Idel_Timeout;
            data.Value = apControl.Idel_Timeout;
            list.Add(data);

            data.Attribute = RadiusAttribute.Simultaneous_Use;
            data.Value = apControl.Simultaneous_Use;
            list.Add(data);

            return dal_radGroupReply.Insert(list);
        }
        public String GenerateGroupName(int type, string apMac, int ssid)
        {
            return type.ToString() + "$" + apMac.Substring(2, 6) + "$" + ssid.ToString();
        }
        public string GenerateUserName(string userName, string groupName)
        {
            return userName + "_" + groupName;
        }
        /// <summary>
        /// 配置SSID相关设置
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool SSIDConfig(List<M_SSIDConfig> list)
        {
            List<RadGroupReply> GroupList = new List<RadGroupReply>();
            RadGroupReply group = new RadGroupReply();

            using (TransactionScope scope = new TransactionScope())
            {
                bool flag = false;
                try
                {
                    foreach (M_SSIDConfig data in list)
                    {
                        group = null;
                        //拼凑groupName
                        group.GroupName = GenerateGroupName(data.ConfigType, data.APMac, data.SSID);
                        dal_radGroupReply.DeleteByGroupName(group.GroupName);//clear db groupName

                        group.Value = data.TimeLimit.ToString();
                        group.Attribute = "Session-TimeOut";
                        dal_radGroupReply.Insert(group);

                        group.Value = data.MaxTraffic.ToString();
                        group.Attribute = "ChilliSpot-Max-Total-Octets";
                        dal_radGroupReply.Insert(group);

                        group.Value = data.MaxUpRate.ToString();
                        group.Attribute = "ChilliSpot-Bandwidth-Max-Up";
                        dal_radGroupReply.Insert(group);

                        group.Value = data.MaxDownRate.ToString();
                        group.Attribute = "ChilliSpot-Bandwidth-Max-Down";
                        dal_radGroupReply.Insert(group);

                    }

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("错误原因是：" + ex.Message);
                }
                return flag;
            }
        }
        /// <summary>
        /// 获取radius用户名和密码
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public M_RadiusLoginInfo GetLoginInfo(M_RadiusUser user)
        {
            string groupName = GenerateGroupName(user.UserType, user.ApMac, user.SSID);
            string userName = GenerateUserName(user.UserName, groupName);
            M_RadiusLoginInfo result = new M_RadiusLoginInfo();
            string password = null;
            int count = dal_radCheck.Select(user);
            if (count > 0)//用户已经存在
            {
                password = dal_radCheck.SelectPassword(user);
            }
            else//用户不存在
            {
                bool flag = false;
                password = pwdGenete.Generate();
                using (TransactionScope scope = new TransactionScope())
                {
                    try
                    {
                        UserGroup userGroup = new UserGroup();
                        userGroup.UserName = userName;
                        userGroup.GroupName = groupName;//APId作为组名
                        userGroup.Priority = 1;
                        flag = dal_userGroup.Insert(userGroup);//向userGroup中加入数据

                        flag = dal_radCheck.Insert(user, userName, password);

                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        scope.Dispose();
                        throw new Exception("错误原因是：" + ex.Message);
                    }
                }
            }
            result.UserName = userName;
            result.Password = password;
            return result;

        }

        public bool CheckBlackList(BlackList data)
        {
            return dal_blackList.CheckBlackList(data);
        }
        /// <summary>
        /// 将用户加入到黑名单
        /// </summary>
        /// <param name="userMac"></param>
        /// <returns></returns>
        public bool AddBlackList(string userMac)
        {
            BlackList data = new BlackList();
            data.UserMac = userMac;
            data.Enable = 1;//enable
            if (CheckBlackList(data))
                return dal_blackList.ToogleBlackList(data);
            else
                return dal_blackList.Insert(data);
        }
        /// <summary>
        /// 讲用户从黑名单中移除
        /// </summary>
        /// <param name="userMac"></param>
        /// <returns></returns>
        public bool DisableBlackList(string userMac)
        {
            BlackList data = new BlackList();
            data.UserMac = userMac;
            data.Enable = 0;//删除黑名单
            return dal_blackList.ToogleBlackList(data);
        }

        /// <summary>
        /// 查询用户是否存在于黑名单中
        /// </summary>
        /// <param name="userMac"></param>
        /// <returns></returns>
        public bool CheckIsBlackList(string userMac)
        {
            return dal_blackList.CheckIsBlackList(userMac);
        }
        //设置radius配置用户上网设置
        /*
         * 18	机构扩展属性	免费上网	101
           21	机构扩展属性	QQ认证	102
           22	机构扩展属性	SINA微博认证	103
        table: sys_dic_extprop
         1	101	上行速度（K / 秒）	512	0	^\d{1,3}$	请输入1-3位的数字格式	1
        2	101	下行速度（K / 秒）	512	0	^\d{1,3}$	请输入1-3位的数字格式	1
        3	101	上行流量（M）	0	0	^\d{1,3}$	请输入1-3位的数字格式	1
        4	101	下行流量（M）	0	0	^\d{1,3}$	请输入1-3位的数字格式	1
        5	101	上网时长（分钟）	30	0	^\d{1,3}$	请输入1-3位的数字格式	1
        6	101	免费用户名		0			0
        7	101	免费用户密码		0			0
         * 
        8	102	上行速度（K / 秒）	512	0	^\d{1,3}$	请输入1-3位的数字格式	1
        9	102	下行速度（K / 秒）	512	0	^\d{1,3}$	请输入1-3位的数字格式	1
        10	102	上行流量（M）	0	0	^\d{1,3}$	请输入1-3位的数字格式	1
        11	102	下行流量（M）	0	0	^\d{1,3}$	请输入1-3位的数字格式	1
        12	102	APP_ID		1			1
        13	102	跳转地址		1			1
         * 
        14	103	上行速度（K / 秒）	512	0	^\d{1,3}$	请输入1-3位的数字格式	1
        15	103	下行速度（K / 秒）	512	0	^\d{1,3}$	请输入1-3位的数字格式	1
        16	103	上行流量（M）	0	0	^\d{1,3}$	请输入1-3位的数字格式	1
        17	103	下行流量（M）	0	0	^\d{1,3}$	请输入1-3位的数字格式	1
        18	103	APP_KEY		1			1
        19	103	WeiboID		1			1

        table sys_org_property
         * 1	10035	101	5	1015
            2	10035	103	12	256
            3	10035	101	1	256
            4	10035	101	2	128
            6	10035	102	7	128
            7	10035	102	8	128
            8	10035	102	9	0
            9	10035	102	10	
            10	10035	103	13	256
            11	10035	101	3	0
            12	10035	101	4	30
            13	10035	101	6	1016
            14	10035	102	11	
            15	10035	103	14	0
            16	10035	103	15	
            17	10035	103	16	

         * */
        public bool setProperies(List<SYS_ORG_PROPERTY> properties)
        {
            //RadiusAttribute attr = new RadiusAttribute();
            RadGroupReply groupReply = new RadGroupReply();
            using (TransactionScope scope = new TransactionScope())
            {
                bool flag = false;
                try
                {
                    foreach (SYS_ORG_PROPERTY data in properties)
                    {
                        #region 免费用户设置

                        if (data.PTYPE.Equals(((int)CustomEnum.ENUM_PType.FreeOnLine).ToString()))
                        {
                            groupReply.GroupName = data.OID + "$" + "freegroup";
                            //UserGroup user-group
                            UserGroup ug = new UserGroup();
                            ug.GroupName = groupReply.GroupName;
                            ug.UserName = data.OID + "FreeUser";
                            flag = dal_userGroup.UpdateUserGroup(ug);
                            //User name-password
                            RadCheck userCheck = new RadCheck();
                            userCheck.UserName = ug.UserName;
                            userCheck.Attribute = RadiusAttribute.User_Password;
                            userCheck.Value = ug.UserName;
                            userCheck.UserType = 0;
                            flag = dal_radCheck.UpdateFreeUser(userCheck);
                            /*
                              *  1	101	上行速度（K / 秒）	512	0	^\d{1,3}$	请输入1-3位的数字格式	1
                                2	101	下行速度（K / 秒）	512	0	^\d{1,3}$	请输入1-3位的数字格式	1
                                3	101	上行流量（M）	0	0	^\d{1,3}$	请输入1-3位的数字格式	1
                                4	101	下行流量（M）	0	0	^\d{1,3}$	请输入1-3位的数字格式	1
                                5	101	上网时长（分钟）	30	0	^\d{1,3}$	请输入1-3位的数字格式	1
                                6	101	免费用户名		0			0
                                7	101	免费用户密码		0			0
                              * */
                            switch (Convert.ToInt32(data.PNAME))
                            {
                                case (int)CustomEnum.ENUM_Radius_Prop.ChilliSpot_Bandwidth_Max_Up:
                                    groupReply.Attribute = RadiusAttribute.ChilliSpot_Bandwidth_Max_Up;
                                    groupReply.Value = (Convert.ToInt64(data.PVALUE) * 1024).ToString();
                                    break;
                                case (int)CustomEnum.ENUM_Radius_Prop.ChilliSpot_Bandwidth_Max_Down:
                                    groupReply.Attribute = RadiusAttribute.ChilliSpot_Bandwidth_Max_Down;
                                    groupReply.Value = (Convert.ToInt64(data.PVALUE) * 1024).ToString();
                                    break;
                                case (int)CustomEnum.ENUM_Radius_Prop.Acct_Input_Octets:
                                    groupReply.Attribute = RadiusAttribute.Acct_Input_Octets;
                                    groupReply.Value = Convert.ToString(Convert.ToInt64(data.PVALUE) * 1024 * 1024, 8);
                                    break;
                                case (int)CustomEnum.ENUM_Radius_Prop.Acct_Output_Octets:
                                    groupReply.Attribute = RadiusAttribute.Acct_Output_Octets;
                                    groupReply.Value = Convert.ToString(Convert.ToInt64(data.PVALUE) * 1024 * 1024, 8);
                                    break;
                                case (int)CustomEnum.ENUM_Radius_Prop.Session_Timeout://time
                                    groupReply.Attribute = RadiusAttribute.Session_Timeout;
                                    groupReply.Value = (Convert.ToInt64(data.PVALUE) * 60).ToString();
                                    break;
                            }
                        }
                        #endregion
                        #region QQ 设置

                        else if (data.PTYPE.Equals(((int)CustomEnum.ENUM_PType.QQ).ToString()))
                        {
                            groupReply.GroupName = data.OID + "$" + "QQ";
                            //UserGroup user-group
                            UserGroup ug = new UserGroup();
                            ug.GroupName = groupReply.GroupName;
                            ug.UserName = data.OID + "QQ";
                            flag = dal_userGroup.UpdateUserGroup(ug);
                            //User name-password
                            RadCheck userCheck = new RadCheck();
                            userCheck.UserName = ug.UserName;
                            userCheck.Attribute = RadiusAttribute.User_Password;
                            userCheck.Value = ug.UserName;
                            userCheck.UserType = 1;
                            flag = dal_radCheck.UpdateFreeUser(userCheck);
                            /*8	102	上行速度（K / 秒）	512	0	^\d{1,3}$	请输入1-3位的数字格式	1
                            9	102	下行速度（K / 秒）	512	0	^\d{1,3}$	请输入1-3位的数字格式	1
                            10	102	上行流量（M）	0	0	^\d{1,3}$	请输入1-3位的数字格式	1
                            11	102	下行流量（M）	0	0	^\d{1,3}$	请输入1-3位的数字格式	1
                            12	102	APP_ID		1			1
                            13	102	跳转地址		1			1
                            */
                            switch (data.PNAME)
                            {
                                case "8":
                                    groupReply.Attribute = RadiusAttribute.ChilliSpot_Bandwidth_Max_Up;
                                    groupReply.Value = (Convert.ToInt64(data.PVALUE) * 1024).ToString();
                                    break;
                                case "9":
                                    groupReply.Attribute = RadiusAttribute.ChilliSpot_Bandwidth_Max_Down;
                                    groupReply.Value = (Convert.ToInt64(data.PVALUE) * 1024).ToString();
                                    break;
                                case "10":
                                    groupReply.Attribute = RadiusAttribute.Acct_Input_Octets;
                                    groupReply.Value = Convert.ToString(Convert.ToInt64(data.PVALUE) * 1024 * 1024, 8);
                                    break;
                                case "11":
                                    groupReply.Attribute = RadiusAttribute.Acct_Output_Octets;
                                    groupReply.Value = Convert.ToString(Convert.ToInt64(data.PVALUE) * 1024 * 1024, 8);
                                    break;
                                default:
                                    break;
                            }
                        }
                        #endregion
                        #region WeiBo设置

                        else if (data.PTYPE.Equals(((int)CustomEnum.ENUM_PType.WeiBo).ToString()))
                        {
                            groupReply.GroupName = data.OID + "$" + "WeiBo";
                            //UserGroup user-group
                            UserGroup ug = new UserGroup();
                            ug.GroupName = groupReply.GroupName;
                            ug.UserName = data.OID + "WB";
                            flag = dal_userGroup.UpdateUserGroup(ug);
                            //User name-password
                            RadCheck userCheck = new RadCheck();
                            userCheck.UserName = ug.UserName;
                            userCheck.Attribute = RadiusAttribute.User_Password;
                            userCheck.Value = ug.UserName;
                            userCheck.UserType = 2;
                            flag = dal_radCheck.UpdateFreeUser(userCheck);
                            /*
                             * 14	103	上行速度（K / 秒）	512	0	^\d{1,3}$	请输入1-3位的数字格式	1
                                15	103	下行速度（K / 秒）	512	0	^\d{1,3}$	请输入1-3位的数字格式	1
                                16	103	上行流量（M）	0	0	^\d{1,3}$	请输入1-3位的数字格式	1
                                17	103	下行流量（M）	0	0	^\d{1,3}$	请输入1-3位的数字格式	1
                                18	103	APP_KEY		1			1
                                19	103	WeiboID		1			1
                             * */
                            switch (data.PNAME)
                            {
                                case "14":
                                    groupReply.Attribute = RadiusAttribute.ChilliSpot_Bandwidth_Max_Up;
                                    groupReply.Value = (Convert.ToInt64(data.PVALUE) * 1024).ToString();
                                    break;
                                case "15":
                                    groupReply.Attribute = RadiusAttribute.ChilliSpot_Bandwidth_Max_Down;
                                    groupReply.Value = (Convert.ToInt64(data.PVALUE) * 1024).ToString();
                                    break;
                                case "16":
                                    groupReply.Attribute = RadiusAttribute.Acct_Input_Octets;
                                    groupReply.Value = Convert.ToString(Convert.ToInt64(data.PVALUE) * 1024 * 1024, 8);
                                    break;
                                case "17":
                                    groupReply.Attribute = RadiusAttribute.Acct_Output_Octets;
                                    groupReply.Value = Convert.ToString(Convert.ToInt64(data.PVALUE) * 1024 * 1024, 8);
                                    break;
                                default:
                                    break;
                            }
                        }
                        else if (data.PTYPE.Equals(((int)CustomEnum.ENUM_PType.WeiXin).ToString()))
                        {
                            groupReply.GroupName = data.OID + "$" + "WeiXin";
                            //UserGroup user-group
                            UserGroup ug = new UserGroup();
                            ug.GroupName = groupReply.GroupName;
                            ug.UserName = data.OID + "WX";
                            flag = dal_userGroup.UpdateUserGroup(ug);
                            //User name-password
                            RadCheck userCheck = new RadCheck();
                            userCheck.UserName = ug.UserName;
                            userCheck.Attribute = RadiusAttribute.User_Password;
                            userCheck.Value = ug.UserName;
                            userCheck.UserType = 3;
                            flag = dal_radCheck.UpdateFreeUser(userCheck);
                            /*
                             * 14	103	上行速度（K / 秒）	512	0	^\d{1,3}$	请输入1-3位的数字格式	1
                                15	103	下行速度（K / 秒）	512	0	^\d{1,3}$	请输入1-3位的数字格式	1
                                16	103	上行流量（M）	0	0	^\d{1,3}$	请输入1-3位的数字格式	1
                                17	103	下行流量（M）	0	0	^\d{1,3}$	请输入1-3位的数字格式	1
                                18	103	APP_KEY		1			1
                                19	103	WeiboID		1			1
                             * */
                            switch (data.PNAME)
                            {
                                case "25":
                                    groupReply.Attribute = RadiusAttribute.ChilliSpot_Bandwidth_Max_Up;
                                    groupReply.Value = (Convert.ToInt64(data.PVALUE) * 1024).ToString();
                                    break;
                                case "26":
                                    groupReply.Attribute = RadiusAttribute.ChilliSpot_Bandwidth_Max_Down;
                                    groupReply.Value = (Convert.ToInt64(data.PVALUE) * 1024).ToString();
                                    break;
                                case "27":
                                    groupReply.Attribute = RadiusAttribute.Acct_Input_Octets;
                                    groupReply.Value = Convert.ToString(Convert.ToInt64(data.PVALUE) * 1024 * 1024, 8);
                                    break;
                                case "28":
                                    groupReply.Attribute = RadiusAttribute.Acct_Output_Octets;
                                    groupReply.Value = Convert.ToString(Convert.ToInt64(data.PVALUE) * 1024 * 1024, 8);
                                    break;
                                default:
                                    break;
                            }
                        }
                        #endregion
                        else
                        {

                        }
                        if (groupReply.Attribute != null)
                        {
                            flag = dal_radGroupReply.UpdateGroupAttr(groupReply);
                        }

                    }

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("错误原因是：" + ex.Message);
                }
                return flag;
            }
        }

        public RadiusAuthResult GetRadiusAuth(RadiusAuth radiusAuth)
        {
            RadCheck radCheck = new RadCheck();
            string type = "";
            switch (Convert.ToInt32(radiusAuth.UserType))
            {
                case (int)CustomEnum.ENUM_Auth_Type.FreeUser:
                    type = CustomEnum.ENUM_Auth_Type.FreeUser.ToString();
                    break;
                case (int)CustomEnum.ENUM_Auth_Type.QQ:
                    type = CustomEnum.ENUM_Auth_Type.QQ.ToString();
                    break;
                case (int)CustomEnum.ENUM_Auth_Type.WB:
                    type = CustomEnum.ENUM_Auth_Type.WB.ToString();
                    break;
                case (int)CustomEnum.ENUM_Auth_Type.WX:
                    type = CustomEnum.ENUM_Auth_Type.WX.ToString();
                    break;
                default:
                    break;
            }
            RadCheck_Log log = new RadCheck_Log();
            log.CreateTime = DateTime.Now;
            log.OID = radiusAuth.OID;
            log.OpenID = radiusAuth.OpenID;
            log.UserMac = radiusAuth.UserMac;
            log.UserType = radiusAuth.UserType;
            String userName = radiusAuth.OID + type;
            if (dal_radChek_log.Insert(log))
            {
                return dal_radCheck.GetAuthUser(userName);
            }
            return null;
            /*PasswordGenerator pg = new PasswordGenerator();
            radCheck.UserName = radiusAuth.OID + "$" + type + "$" + radiusAuth.UserName;
            radCheck.Attribute = RadiusAttribute.User_Password;
            radCheck.UserType = radiusAuth.UserType;
            radCheck.Value = pg.Generate();


            UserGroup ug = new UserGroup();
            ug.UserName = radCheck.UserName;
            ug.GroupName = radiusAuth.OID + "$" + type;
            if (dal_radCheck.UpdateAuthUser(radCheck) && dal_userGroup.UpdateUserGroup(ug))
            {
                return dal_radCheck.GetAuthUser(radCheck);
            }
            return null;
             * */
        }
    }
}
