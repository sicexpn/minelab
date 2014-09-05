using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LuoBo.Api
{
    public abstract class FrienDevApplication
    {
        public FrienDevApplication()
        {
        }

        public abstract string GetTrackText(int type, string[] parameters);
    }
}
