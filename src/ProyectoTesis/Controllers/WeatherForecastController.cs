using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using ProyectoTesisBussiness.BussinessControllers;
using ProyectoTesisModels.Modelos;

namespace ProyectoTesis.Controllers
{

    

    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        private TweetsProfilesBussiness bussiness;

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            bussiness = new TweetsProfilesBussiness();
        }

        [HttpGet]
        
        public IEnumerable<string> Get()
        {
            var tweet = bussiness.GetFirstTweet();
            Console.WriteLine(tweet.ToString());
            var rng = new Random();
            return new List<string>() { tweet.ToString() };
            /*return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();*/
        }

        [Route("api/getAll")]
        [HttpGet]
        public IEnumerable<string> GetAll(int limit)
        {
            var tweet = bussiness.GetTweets(limit);
            return tweet.Select(i => i.ToString()); ;
        }
    }
}