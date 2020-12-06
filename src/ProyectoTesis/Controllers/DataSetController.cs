using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProyectoTesisBussiness.BussinessControllers;
using ProyectoTesisModels.Modelos;

namespace ProyectoTesis.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DataSetController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        private TweetsProfilesBussiness bussiness;
        public DataSetController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            bussiness = new TweetsProfilesBussiness();
        }

        [Route("api/getLDA")]
        [HttpGet]
        public IEnumerable<TableTopics> GetLDA(int limit)
        {
            var tweet = bussiness.GetTweets(limit);
            var result = bussiness.LDATweets();
            
            return result;
        }

        [Route("api/getNER")]
        [HttpGet]
        public IEnumerable<TableTopics> GetNER(int limit)
        {
            var tweet = bussiness.GetTweets(limit);
            var result = bussiness.NERTweets();

            return result;
        }
    }
}
