using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.Entity;

namespace LUOBO.BLL
{
    public class BLL_Tripartite
    {
        DAL.DAL_Tripartite tripDal = new DAL.DAL_Tripartite();
        public bool InsertTodayJoke(List<string> json)
        {
            List<TAPI_TodayJoke> joke = tripDal.SelectToDayJokeByIDs(json);
            if(joke.Count == 0)
                return tripDal.InsertTodayJoke(json);
            return true;
        }
        public bool InsertTodayImage(List<string> json)
        {
            List<TAPI_TodayImage> joke = tripDal.SelectToDayImageByIDs(json);
            if (joke.Count == 0)
                return tripDal.InsertTodayImage(json);
            return true;
        }

        public List<TAPI_TodayJoke> SelectTodayJoke(object lastKey, int pageSize)
        {
            return tripDal.SelectTodayJoke(lastKey, pageSize);
        }
        
        public List<TAPI_TodayImage> SelectTodayImage(object lastKey, int pageSize)
        {
            return tripDal.SelectTodayImage(lastKey, pageSize);
        }
    }
}
