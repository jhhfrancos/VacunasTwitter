﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProyectoTesisBussiness.BussinessControllers;
using ProyectoTesisBussiness.MongoBussiness;
using ProyectoTesisModels.Modelos;

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

        [Route("api/getLDA")]
        [HttpGet]
        public IEnumerable<TableTopics> GetLDA(int limit)
        {
            var result = bussiness.LDATweets(limit);
            return result;
        }

        [Route("api/getNER")]
        [HttpGet]
        public IEnumerable<TableTopics> GetNER(int limit)
        {
            var result = bussiness.NERTweets(limit);
            return result;
        }

        [Route("api/updateDB")]
        [HttpGet]
        public bool UpdateDB(int limit)
        {
            mongoServices.UpdateDB();
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
        public IEnumerable<FrequencyWord> WordCloud(int limit)
        {
            return bussiness.WordCloud(limit);
        }

    }
}
