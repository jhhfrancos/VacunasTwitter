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

        [Route("api/getAll")]
        [HttpGet]
        public IEnumerable<TableTopics> GetAll(int limit)
        {
            var tweet = bussiness.GetTweets(limit);
            var result = bussiness.LDATweets();
            
            return result;
        }
    }
}
