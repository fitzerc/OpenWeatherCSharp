namespace OpenWeatherCSharp.Shared.Models
{
    public class WeatherResposne
    {
        public string Condition { get; set; } = "";
        public string Temp { get; set; } = "";
        public bool HasAlert { get; set; }
        public List<string> Alerts { get; set; } = new List<string>();
    }

    public enum TempEnum
    {
        VeryCold,
        Cold,
        Tolerable,
        Nice,
        Hot
    }
}
