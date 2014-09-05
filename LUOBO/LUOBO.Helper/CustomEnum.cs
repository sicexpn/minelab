using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LUOBO.Helper
{
    public static class CustomEnum
    {
        public enum ENUM_Auth_Type
        {
            FreeUser = 0,
            QQ = 1,
            WB = 2,
            WX = 3
        }
        /*
                             *  1	101	上行速度（K / 秒）	512	0	^\d{1,3}$	请输入1-3位的数字格式	1
                               2	101	下行速度（K / 秒）	512	0	^\d{1,3}$	请输入1-3位的数字格式	1
                               3	101	上行流量（M）	0	0	^\d{1,3}$	请输入1-3位的数字格式	1
                               4	101	下行流量（M）	0	0	^\d{1,3}$	请输入1-3位的数字格式	1
                               5	101	上网时长（分钟）	30	0	^\d{1,3}$	请输入1-3位的数字格式	1
                               6	101	免费用户名		0			0
                               7	101	免费用户密码		0			0
                             * */
        public enum ENUM_Radius_Prop
        {
            ChilliSpot_Bandwidth_Max_Up = 1,
            ChilliSpot_Bandwidth_Max_Down = 2,
            Acct_Input_Octets = 3,
            Acct_Output_Octets = 4,
            Session_Timeout = 5

        }
        public enum ENUM_PType
        {
            FreeOnLine = 101,
            QQ = 102,
            WeiBo = 103,
            WeiXin = 104
        }
        public enum ENUM_SqlConn
        {
            LUOBO = 0,
            Radius = 1,
            Statistical = 2
        }

        /// <summary>
        /// 文件扩展名
        /// </summary>
        public enum FileExtension
        {
            JPG = 255216,
            GIF = 7173,
            BMP = 6677,
            PNG = 13780,
            COM = 7790,
            EXE = 7790,
            DLL = 7790,
            RAR = 8297,
            ZIP = 8075,
            XML = 6063,
            HTML = 6033,
            ASPX = 239187,
            CS = 117115,
            JS = 119105,
            TXT = 210187,
            SQL = 255254,
            BAT = 64101,
            BTSEED = 10056,
            RDP = 255254,
            PSD = 5666,
            PDF = 3780,
            CHM = 7384,
            LOG = 70105,
            REG = 8269,
            HLP = 6395,
            DOC = 208207,
            XLS = 208207,
            DOCX = 208207,
            XLSX = 208207,
            UNKNOW = -1
        }

        public enum ENUM_Result_Code : int
        {
            /// <summary>
            /// 成功
            /// </summary>
            OK = 0,
            /// <summary>
            /// 失败
            /// </summary>
            Fail = 1,
            /// <summary>
            /// 登录超时
            /// </summary>
            LoginTimeout = -100,
            /// <summary>
            /// 没有权限
            /// </summary>
            NoCompetence = -101
        }

        /// <summary>
        /// 广告状态
        /// </summary>
        public enum ENUM_AD_Statu : int
        {
            /// <summary>
            /// 编辑状态
            /// </summary>
            Modify = 0,
            /// <summary>
            /// 待审核状态
            /// </summary>
            Audit = 1,
            /// <summary>
            /// 发布状态
            /// </summary>
            Publish = 2,
            /// <summary>
            /// 审核拒绝
            /// </summary>
            Cansle = 3
        }

        /// <summary>
        /// 审核类型
        /// </summary>
        public enum ENUM_ADC_Type : int
        {
            /// <summary>
            /// 仅广告审核
            /// </summary>
            ADOnly = 0,
            /// <summary>
            /// 发布到SSID
            /// </summary>
            ToSSID = 1,
            /// <summary>
            /// 发布到发布方案
            /// </summary>
            ToCase = 2,
            /// <summary>
            /// 替换已发布该广告的所有设备
            /// </summary>
            ToAdPub = 3,
            /// <summary>
            /// 仅SSID审核
            /// </summary>
            SSIDOnly = 4
        }

        /// <summary>
        /// 审核申请操作
        /// </summary>
        public enum ENUM_Aud_Type : int
        {
            /// <summary>
            /// 提交审核
            /// </summary>
            NewAudit = 0,
            /// <summary>
            /// 同意申请
            /// </summary>
            PassAudit = 1,
            /// <summary>
            /// 拒绝申请
            /// </summary>
            CansleAudit = 2,
            /// <summary>
            /// 撤销申请
            /// </summary>
            BackAudit = 3
        }

        /// <summary>
        /// 审核申请处理状态
        /// </summary>
        public enum ENUM_Aud_Stat : int
        {
            /// <summary>
            /// 待审核
            /// </summary>
            WaitAudit = 0,
            /// <summary>
            /// 审核中
            /// </summary>
            AuditIng = 1,
            /// <summary>
            /// 审核通过
            /// </summary>
            PassAudit = 2,
            /// <summary>
            /// 审核拒绝
            /// </summary>
            CansleAudit = 3,
            /// <summary>
            /// 审核撤销
            /// </summary>
            BackAudit = 4,
            /// <summary>
            /// 自动通过审核
            /// </summary>
            AutoPub = 5
        }

        /// <summary>
        /// 访问限制类型
        /// </summary>
        public enum ENUM_Ban_Type : int
        {
            /// <summary>
            /// 端口限制
            /// </summary>
            Port = 1,
            /// <summary>
            /// 域名限制
            /// </summary>
            Url = 2,
            /// <summary>
            /// MAC限制
            /// </summary>
            Mac = 3
        }

        /// <summary>
        /// 机构类别
        /// </summary>
        public enum ENUM_Org_Type : int
        {
            /// <summary>
            /// 代理商
            /// </summary>
            Agency = 1,
            /// <summary>
            /// 连锁店
            /// </summary>
            Chain = 2,
            /// <summary>
            /// 单店
            /// </summary>
            Single = 3,
            /// <summary>
            /// 演示机组
            /// </summary>
            Demo = 4
        }

        /// <summary>
        /// 配置类型
        /// </summary>
        public enum ENUM_Setting_Type : int
        {
            /// <summary>
            /// 配置文件
            /// </summary>
            Setting = 0,
            /// <summary>
            /// 广告文件
            /// </summary>
            Ad = 1
        }

        /// <summary>
        /// 统计类别
        /// </summary>
        public enum ENUM_Statistical_Type : int
        {
            /// <summary>
            /// 年
            /// </summary>
            Year = 0,
            /// <summary>
            /// 月
            /// </summary>
            Month = 1,
            /// <summary>
            /// 日
            /// </summary>
            Day = 2,
            /// <summary>
            /// 实时
            /// </summary>
            RealTime = 3
        }

        public enum ENUM_Alarm_Mode : int
        {
            /// <summary>
            /// 全部检查
            /// </summary>
            All = 0,
            /// <summary>
            /// 只检查心跳
            /// </summary>
            HeartBeat = 1,
            /// <summary>
            /// 只检查异常
            /// </summary>
            Except = 2
        }

        public enum ENUM_Alarm_Type : int
        {
            /// <summary>
            /// 关闭
            /// </summary>
            Close = -99
        }

        public enum ENUM_User_Type : int
        {
            /// <summary>
            /// 机构管理员
            /// </summary>
            Admin = 1,
            /// <summary>
            /// 普通用户
            /// </summary>
            Normal = 2,
            /// <summary>
            /// 安装人员
            /// </summary>
            Install = 5,
        }
        public enum ENUM_User_State : int
        {
            /// <summary>
            /// 停用
            /// </summary>
            Disable = 0,
            /// <summary>
            /// 正常
            /// </summary>
            Normal = 1
        }
        /// <summary>
        /// 设备类型
        /// </summary>
        public enum ENUM_ApState : int
        {
            /// <summary>
            /// 正式设备
            /// </summary>
            Normal = 1,
            /// <summary>
            /// 手持演示设备
            /// </summary>
            Demo = 2,
            /// <summary>
            /// 销毁
            /// </summary>
            Destroy = 5
        }

        public enum ENUM_Industry : int
        {
            /// <summary>
            /// 默认
            /// </summary>
            Default = 0,
            /// <summary>
            /// 餐饮
            /// </summary>
            Dining = 1,
            /// <summary>
            /// 娱乐
            /// </summary>
            Entertainment = 2,
            /// <summary>
            /// 银行
            /// </summary>
            Bank = 3
        }
        public enum ENUM_System_SSID_Tempate : int
        {
            /// <summary>
            /// 安装模版ID
            /// </summary>
            Setup = 1
        }

        /// <summary>
        /// 菜单类型
        /// </summary>
        public enum ENUM_Menu_Type : int
        {
            /// <summary>
            /// 普通菜单
            /// </summary>
            Normal = 0,
            /// <summary>
            /// 右键菜单
            /// </summary>
            Mouse = 1
        }

        /// <summary>
        /// 菜单图标类型
        /// </summary>
        public enum ENUM_Menu_IconType : int
        {
            /// <summary>
            /// 无图标
            /// </summary>
            No = 0,
            /// <summary>
            /// 样式图标
            /// </summary>
            Style = 1,
            /// <summary>
            /// 文件图标
            /// </summary>
            File = 2
        }
    }
}