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

        public bool DataCleansing()
        {
            if(dataContext.CollectionExists("Tweets_Clean")) dataContext.getCollection("Twitter", "Tweets_Clean");
            string map = @"
                function() {
                    emit(this.id, this);
                }";
            string reduce = @"        
                function(id, tweet) {
                    var texto = tweet[0].text;
                    //Pasar a minusculas
                    texto = texto.toLowerCase();
                    //Quitar caracteres especiales
                    texto = texto.replace(/[^a-zA-Z0-9áéíóú ]/g, '');
                    return {
                        _id:id,
                        text:texto};
                }";
            BsonJavaScript mapScript = new BsonJavaScript(map);
            BsonJavaScript reduceScript = new BsonJavaScript(reduce);

            FilterDefinitionBuilder<BsonDocument> filterBuilder = new FilterDefinitionBuilder<BsonDocument>();
            FilterDefinition<BsonDocument> filter = filterBuilder.Empty;
            MapReduceOptions<BsonDocument, BsonDocument> options = new MapReduceOptions<BsonDocument, BsonDocument>
            {
                Filter = filter,
                OutputOptions = MapReduceOutputOptions.Reduce("Tweets_Clean", "Twitter"),
                Verbose = true
            };
            try
            {
                collection.MapReduce(mapScript, reduceScript, options).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred {ex.Message}");
            }
            return true;
        }
    }
}
