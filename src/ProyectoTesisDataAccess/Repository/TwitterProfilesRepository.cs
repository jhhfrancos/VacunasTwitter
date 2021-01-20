using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using ProyectoTesisDataAccess.Context;
using ProyectoTesisModels.Modelos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ProyectoTesisDataAccess.Repository
{
    public class TwitterProfilesRepository
    {
        private DataBaseContext dataContext;
        private IMongoCollection<BsonDocument> collection;
        public TwitterProfilesRepository()
        {
            dataContext = new DataBaseContext();
            collection = dataContext.getCollection("Twitter", "Tweets_Profiles");
        }

        public Tweet GetFirstTweet()
        {
            var firstDocument = collection.Find(new BsonDocument()).FirstOrDefault();
            Tweet tweet = BsonSerializer.Deserialize<Tweet>(firstDocument);
            return tweet;
        }

        public List<Tweet> GetAllTweets(int limit)
        {
            var documents = collection.Find(new BsonDocument()).Limit(limit).ToList();
            List<Tweet> tweets = new List<Tweet>();
            foreach (var item in documents)
            {
                Tweet tweet = BsonSerializer.Deserialize<Tweet>(item);
                tweets.Add(tweet);
            }
            return tweets;
        }

        public bool SaveTweet(string[] tweets)
        {
            foreach (var item in tweets)
            {
                try
                {
                    var jsonDoc = BsonDocument.Parse(item);
                    var filter = Builders<BsonDocument>.Filter.Eq("id", jsonDoc.GetValue("id"));
                    var studentDocument = collection.Find(filter).FirstOrDefault();
                    if (studentDocument == null)
                        collection.InsertOne(jsonDoc);
                    }
                catch (Exception)
                {
                    continue;
                }
            }
            return true;
        }
    }
}
