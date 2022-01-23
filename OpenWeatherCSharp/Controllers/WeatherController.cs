using Microsoft.AspNetCore.Mvc;
using OpenWeatherCSharp.Dal;
using OpenWeatherCSharp.Shared;
using OpenWeatherCSharp.Shared.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OpenWeatherCSharp.Controllers
{
    [Route("api/weather")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherDal _dal;
        private readonly IConfiguration _config;

        public WeatherController(IWeatherDal weatherDal, IConfiguration config)
        {
            _dal = weatherDal;
            _config = config;
        }

        // GET: api/<WeatherController>
        [HttpGet]
        public WeatherResposne Get(string lat, string lon)
        {
            return _dal.GetWeather(lat, lon, _config.GetValue<string>("ApiKey"));
        }
    }
}
