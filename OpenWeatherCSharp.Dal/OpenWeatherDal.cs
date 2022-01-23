using Newtonsoft.Json;
using OpenWeatherCSharp.Shared;
using OpenWeatherCSharp.Shared.Models;
using System.Collections.Specialized;
using System.Web;

namespace OpenWeatherCSharp.Dal
{
    public class OpenWeatherDal : IWeatherDal
    {
        private readonly string _url = "https://api.openweathermap.org/data/2.5/onecall";
        private readonly IHttpClientFactory _httpClientFactory;

        public OpenWeatherDal(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public WeatherResposne GetWeather(string lat, string lon, string appId)
        {
            WeatherResposne weatherResposne = new();

            HttpRequestMessage request = BuildOpenWeatherRequest(lat, lon, appId);

            var client = _httpClientFactory.CreateClient();


            using var response = client.Send(request);
            response.EnsureSuccessStatusCode();

            if (response.Content is object && response.Content?.Headers?.ContentType?.MediaType == "application/json")
            {
                var respStream = response.Content.ReadAsStream();

                using var streamReader = new StreamReader(respStream);
                using var jsonReader = new JsonTextReader(streamReader);

                var serializer = new JsonSerializer();

                try
                {
                    OpenWeatherResponse resp = serializer.Deserialize<OpenWeatherResponse>(jsonReader);

                    weatherResposne.Condition = resp.current.weather.FirstOrDefault().main;
                    weatherResposne.Temp = GetTempFromResponse(resp);
                    weatherResposne.HasAlert = HasAlerts(resp);
                    weatherResposne.Alerts = weatherResposne.HasAlert
                        ? resp.alerts.Select(a => a.@event).ToList()
                        : new List<string>();
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

            return weatherResposne;
        }

        private HttpRequestMessage BuildOpenWeatherRequest(string lat, string lon, string appId)
        {
            NameValueCollection query = BuildQuery(lat, lon, appId);

            var builder = new UriBuilder(_url);
            builder.Query = query.ToString();

            var request = new HttpRequestMessage(HttpMethod.Get, builder.ToString());
            request.Headers.Add("Accept", "application/json");

            return request;
        }

        private NameValueCollection BuildQuery(string lat, string lon, string appId)
        {
            var query = HttpUtility.ParseQueryString("");
            query["lat"] = lat;
            query["lon"] = lon;
            query["appid"] = appId;
            query["exclude"] = "minutely,hourly,daily";
            query["units"] = "imperial";

            return query;
        }

        private bool HasAlerts(OpenWeatherResponse resp)
        {
            if (resp.alerts == null)
            {
                return false;
            }

            return resp.alerts.Any();
        }

        private string GetTempFromResponse(OpenWeatherResponse resp)
        {
            var temp = resp.current.feels_like;

            if (temp > -150 && temp < 10)
            {
                return TempEnum.VeryCold.ToString();
            }

            if (temp < 40)
            {
                return TempEnum.Cold.ToString();
            }

            if (temp < 60)
            {
                return TempEnum.Tolerable.ToString();
            }

            if (temp < 80)
            {
                return TempEnum.Nice.ToString();
            }

            if (temp >= 80 && temp < 150)
            {
                return TempEnum.Hot.ToString();
            }

            return "Unable to get temp";
        }
    }

    public class Weather
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }

    public class Current
    {
        public double feels_like { get; set; }
        public List<Weather> weather { get; set; }
    }

    public class Alert
    {
        public string sender_name { get; set; }
        public string @event { get; set; }
        public int start { get; set; }
        public int end { get; set; }
        public string description { get; set; }
        public List<string> tags { get; set; }
    }

    public class OpenWeatherResponse
    {
        public Current current { get; set; }
        public List<Alert> alerts { get; set; }
    }
}
