using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using ProyectoTesisModels.Modelos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ProyectoTesisDataAccess.Context
{
    public class DataBaseContext
    {
        private MongoClient dbClient;
        
        public DataBaseContext()
        {
            dbClient = new MongoClient("mongodb://192.168.1.215:27017/?readPreference=primary&appname=MongoDB%20Compass&ssl=false");
        }

        public IMongoCollection<BsonDocument> getCollection(string DataBase, string collection)
        {
            try
            {
                var collec = dbClient.GetDatabase(DataBase).GetCollection<BsonDocument>("Tweets_Profiles");
                return collec;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
    }
}
