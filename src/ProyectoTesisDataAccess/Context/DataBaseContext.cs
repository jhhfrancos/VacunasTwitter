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
        private string dbName = "Twitter";
        
        public DataBaseContext()
        {
            dbClient = new MongoClient("mongodb://localhost:27017/?readPreference=primary&appname=MongoDB%20Compass&ssl=false"); //192.168.1.215
        }

        public IMongoCollection<BsonDocument> getCollection(string DataBase, string collection)
        {
            try
            {
                var collec = dbClient.GetDatabase(DataBase).GetCollection<BsonDocument>(collection);
                return collec;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public MongoDatabaseBase getDataBase()
        {
            try
            {
                return (MongoDatabaseBase) dbClient.GetDatabase("Twitter");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool CollectionExists(string collectionName)
        {
            var filter = new BsonDocument("name", collectionName);
            var options = new ListCollectionNamesOptions { Filter = filter };

            return dbClient.GetDatabase(dbName).ListCollectionNames(options).Any();
        }

    }
}
