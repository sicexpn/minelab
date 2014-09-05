using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.DAL;
using LUOBO.Entity;

namespace LUOBO.BLL
{
    public class BLL_SHARE
    {
        DAL_SHARE share = new DAL_SHARE();

        public bool Update(SHARE data)
        {
            return share.Update(data);
        }

        public bool Delete(Int64 id)
        {
            return share.Delete(id);
        }

        public bool Deletes(string ids)
        {
            return share.Deletes(ids);
        }

        public SHARE SelectByID(Int64 id)
        {
            return share.SelectByID(id);
        }
    }
}
