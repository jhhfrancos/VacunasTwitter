using ProyectoTesisDataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using TwitterRetrieval;

namespace ProyectoTesisBussiness.MongoBussiness
{
    public class MongoServices
    {
        TwitterProfilesRepository repository;
        public MongoServices()
        {
            repository = new TwitterProfilesRepository();
        }
        
        public bool UpdateDB()
        {
            var docs = Extract.GetDocuments();
            repository.SaveTweet(docs);
            return true;
        }

        public bool DataCleansing()
        {
            var result = repository.DataCleansing();
            return result;
        }

    }
}
