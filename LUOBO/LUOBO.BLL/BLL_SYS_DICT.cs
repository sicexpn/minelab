using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.DAL;
using LUOBO.Entity;
using LUOBO.Model;

namespace LUOBO.BLL
{
    public class BLL_SYS_DICT
    {
        DAL_SYS_DICT dDAL = new DAL_SYS_DICT();

        public List<SYS_DICT> Select()
        {
            return dDAL.Select();
        }

        public SYS_DICT Select(Int64 id)
        {
            return dDAL.Select(id);
        }

        public List<SYS_DICT_EXTPROP> SelectExtProperty()
        {
            return dDAL.SelectExtProperty();
        }
    }
}
