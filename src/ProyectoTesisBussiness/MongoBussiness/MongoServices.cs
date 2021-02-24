using ProyectoTesisDataAccess.Repository;
using ProyectoTesisModels.Modelos;
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
        
        public bool UpdateDBProfiles()
        {
            var docsProfiles = Extract.GetDocuments();
            repository.SaveTweet(docsProfiles);
            return true;
        }

        public bool UpdateDBBase()
        {
            var docsBase = Extract.GetDocumentsBase();
            repository.SaveTweet(docsBase, "Tweets_Base");
            return true;
        }

        public bool DataCleansing()
        {
            var resultProfiles = repository.DataCleansing();
            return resultProfiles;
        }

        public Statistics DBStatistics()
        {
            var resultProfiles = repository.DBStatistics();
            return resultProfiles;
        }

        public Tweet GetTweet(string id)
        {
            return repository.GetTweet(id);
        }

        public User GetUser(string id)
        {
            return repository.GetUser(id);
        }
    }
}
