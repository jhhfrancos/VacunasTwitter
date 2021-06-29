using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProyectoTesisBussiness.BussinessControllers;
using ProyectoTesisBussiness.MongoBussiness;
using ProyectoTesisModels.Modelos;
using ProyectoTesisModels.Modelos.ForceDirectedGraph;
using static Catalyst.Models.LDA;

namespace ProyectoTesis.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DataSetController : ControllerBase
    {

        private TweetsProfilesBussiness bussiness;
        private MongoServices mongoServices;
        private IConfiguration configuration;
        public DataSetController(IConfiguration _iConfig)
        {
            bussiness = new TweetsProfilesBussiness();
            mongoServices = new MongoServices();
            configuration = _iConfig;
        }

        [Route("api/getTrainLDA")]
        [HttpGet]
        public bool GetTrainLDA(int limit, int numTopics = 100, int NumOfTerms = 10)
        {
            var result = bussiness.LDATrainTweets(limit, numTopics, NumOfTerms);
            return result;
        }

        [Route("api/getTestResultLDA")]
        [HttpGet]
        public IEnumerable<TableTopics> GetTestResultLDA(int limit)
        {
            var result = bussiness.LDATestResultTweets(limit);
            return result;
        }

        [Route("api/getTweet")]
        [HttpGet]
        public Tweet GetTweet(string id)
        {
            var result = mongoServices.GetTweet(id);
            return result;
        }

        [Route("api/getUser")]
        [HttpGet]
        public User GetUser(string id)
        {
            var result = mongoServices.GetUser(id);
            return result;
        }

        [Route("api/getNER")]
        [HttpGet]
        public async Task<bool> GetNERAsync(int limit)
        {
            var result = await bussiness.NERTrainTweetsAsync(limit);
            return result;
        }

        [Route("api/getTweetNER")]
        [HttpGet]
        public TableTopics GetTweetNER(string text)
        {
            var result = bussiness.GetTweetNER(text);
            return result;
        }

        [Route("api/getTestResultNER")]
        [HttpGet]
        public IEnumerable<TableTopics> GetTestResultNER(int limit)
        {
            var result = bussiness.NERTestResultTweets(limit);
            return result;
        }

        [Route("api/getTweetLDA")]
        [HttpGet]
        public IEnumerable<TableTopics> GetTweetLDA(string text)
        {
            var result = bussiness.GetTweetLDA(text);
            return result;
        }

        [Route("api/getTweetLDAModel")]
        [HttpGet]
        public IEnumerable<TopicModel> GetTweetLDAModel(int limit)
        {
            var result = bussiness.GetTweetLDAModel(limit);
            var texto = JsonSerializer.Serialize(result);
            return result;
        }

        [Route("api/updateDB")]
        [HttpGet]
        public bool UpdateDB(int limit)
        {
            Thread t1 = new Thread(() =>
            {
                mongoServices.UpdateDBProfiles();
            });

            Thread t2 = new Thread(() =>
            {
                mongoServices.UpdateDBBase();
            });
            t1.Start();
            t2.Start();

            //wait for t1 to fimish
            t1.Join();

            //wait for t2 to finish
            t2.Join();

            return true;
        }

        [Route("api/dataCleansing")]
        [HttpGet]
        public bool DataCleansingDB()
        {
            return mongoServices.DataCleansing(); ;
        }

        [Route("api/wordCloud")]
        [HttpGet]
        public IEnumerable<FrequencyWord> WordCloud(int limit, string db = "Tweets_Profiles_Clean")
        {
            return bussiness.WordCloud(limit, db);
        }

        [Route("api/focesGraph")]
        [HttpGet]
        public Graph ForcesGraph(int limit, string db = "Tweets_Base_Clean")
        {
            return bussiness.ForcesGraph(limit, db);
        }

        [Route("api/executeBash")]
        [HttpGet]
        public bool ExecuteBash()
        {
            return bussiness.ExecuteBash();
        }

        [Route("api/dbStatistics")]
        [HttpGet]
        public Statistics DBStatistics()
        {
            return mongoServices.DBStatistics();
        }

        [Route("api/tsne")]
        [HttpGet]
        public TSNEResponse Tsnegraphics()
        {
            return bussiness.Tsnegraphics();
        }

        [Route("api/getalltopics")]
        [HttpGet]
        public List<LDATopicDescription> GetAllTopics()
        {
            return bussiness.GetAllTopics();
        }
    }
}
