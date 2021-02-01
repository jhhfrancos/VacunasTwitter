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
        private IMongoCollection<BsonDocument> collectionBase;
        private IMongoCollection<BsonDocument> collectionClean;
        private static string collectionBaseName = "Tweets_Profiles";
        private static string collectionCleanName = "Tweets_Clean";
        public TwitterProfilesRepository()
        {
            dataContext = new DataBaseContext();
            collectionBase = dataContext.getCollection(collectionBaseName);
            collectionClean = dataContext.getCollection(collectionCleanName);
        }

        public Tweet GetFirstTweet()
        {
            var firstDocument = collectionBase.Find(new BsonDocument()).FirstOrDefault();
            Tweet tweet = BsonSerializer.Deserialize<Tweet>(firstDocument);
            return tweet;
        }

        public List<Tweet> GetAllBaseTweets(int limit)
        {
            var documents = collectionBase.Find(new BsonDocument()).Limit(limit).ToList();
            List<Tweet> tweets = new List<Tweet>();
            foreach (var item in documents)
            {
                Tweet tweet = BsonSerializer.Deserialize<Tweet>(item);
                tweets.Add(tweet);
            }
            return tweets;
        }

        public List<TweetClean> GetCleanTweets(int limit)
        {
            var documents = collectionClean.Find(new BsonDocument()).Limit(limit).ToList();
            List<TweetClean> tweets = new List<TweetClean>();
            foreach (var item in documents)
            {
                TweetClean tweet = BsonSerializer.Deserialize<TweetClean>(item);
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
                    var studentDocument = collectionBase.Find(filter).FirstOrDefault();
                    if (studentDocument == null)
                        collectionBase.InsertOne(jsonDoc);
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
            if(dataContext.CollectionExists(collectionCleanName))  dataContext.DropCollection(collectionCleanName);
            string map = @"
                function() {
                    emit({id:this.id, user:this.user.id}, this.text);
                }";
            /*
             * //Transformar camel case a texto separado
             *      //Pasar a minusculas
             *///Quitar caracteres especiales
            //TODO: No recupera correctamente el user
            string reduce = @"
                function(id, tweet) {
                    var texto = tweet[0];
                    texto = texto.replace(/([a-z0-9])([A-Z])/g, '$1 $2');
                    texto = texto.toLowerCase();
                    texto = texto.replace(/[^a-zA-Z0-9áéíóú ]/g, '');
                    return texto;
                }";
            BsonJavaScript mapScript = new BsonJavaScript(map);
            BsonJavaScript reduceScript = new BsonJavaScript(reduce);

            FilterDefinitionBuilder<BsonDocument> filterBuilder = new FilterDefinitionBuilder<BsonDocument>();
            FilterDefinition<BsonDocument> filter = filterBuilder.Empty;
            MapReduceOptions<BsonDocument, BsonDocument> options = new MapReduceOptions<BsonDocument, BsonDocument>
            {
                Filter = filter,
                OutputOptions = MapReduceOutputOptions.Reduce(collectionCleanName, "Twitter"),
                Verbose = true
            };
            try
            {
                collectionBase.MapReduce(mapScript, reduceScript, options).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred {ex.Message}");
            }
            return true;
        }
    }
}
