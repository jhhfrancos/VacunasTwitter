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
            //var docsProfiles = Extract.GetDocuments();
            var docsBase = Extract.GetDocumentsBase();
            //repository.SaveTweet(docsProfiles);
            repository.SaveTweet(docsBase, "Tweets_Base");
            return true;
        }

        public bool DataCleansing()
        {
            var resultProfiles = repository.DataCleansing();
            return resultProfiles;
        }

    }
}
