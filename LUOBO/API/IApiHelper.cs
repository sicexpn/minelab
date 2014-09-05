using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LuoBo.Api
{
    public interface IApiHelper
    {
        void AddTrack(string userId, Type source, int type, string[] parameters);
        void GetOnlineUser();
        void GetUserData(string userId);
        string GetUserHomepageLink(string userId);
        void SendMail(string to, string subject, string body, bool isBodyHtml);
        void SendMessage();
    }
}
