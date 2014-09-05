using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.Access;
using LUOBO.Entity;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using MongoDB.Driver;

namespace LUOBO.DAL
{
    public class DAL_Tripartite
    {
        public bool InsertTodayJoke(List<string> list)
        {
            using (MongoDataAccess mySql = new MongoDataAccess())
            {
                mySql.Insert(list, "Joke");
            }
            return true;
        }

        public bool InsertTodayImage(List<string> list)
        {
            using (MongoDataAccess mySql = new MongoDataAccess())
            {
                mySql.Insert(list, "Image");
            }
            return true;
        }

        public List<TAPI_TodayJoke> SelectToDayJokeByIDs(List<string> json)
        {
            using (MongoDataAccess mySql = new MongoDataAccess())
            {
                List<BsonDocument> bson = new List<BsonDocument>();
                
                BsonDocument bd = null;
                List<string> ids = new List<string>();
                foreach (var item in json)
                {
                    bd = BsonDocument.Parse(item);
                    ids.Add(bd.GetValue("id").AsString);
                }
                var query = Query.In("id", new BsonArray(ids));
                return mySql.Find<TAPI_TodayJoke>(query, "Joke");
            }
        }

        public List<TAPI_TodayImage> SelectToDayImageByIDs(List<string> json)
        {
            using (MongoDataAccess mySql = new MongoDataAccess())
            {
                List<BsonDocument> bson = new List<BsonDocument>();

                BsonDocument bd = null;
                List<string> ids = new List<string>();
                foreach (var item in json)
                {
                    bd = BsonDocument.Parse(item);
                    ids.Add(bd.GetValue("id").AsString);
                }
                var query = Query.In("id", new BsonArray(ids));
                return mySql.Find<TAPI_TodayImage>(query, "Image");
            }
        }

        public List<TAPI_TodayJoke> SelectTodayJoke(object lastKey, int pageSize)
        {
            using (MongoDataAccess mySql = new MongoDataAccess())
            {
                return mySql.Find<TAPI_TodayJoke>(Query.Exists("_id"), "_id", lastKey, pageSize, -1, "Joke");
            }
        }

        public List<TAPI_TodayImage> SelectTodayImage(object lastKey, int pageSize)
        {
            using (MongoDataAccess mySql = new MongoDataAccess())
            {
                return mySql.Find<TAPI_TodayImage>(Query.Exists("_id"), "_id", lastKey, pageSize, -1, "Image");
            }
        }
    }
}
