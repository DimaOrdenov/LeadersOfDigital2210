namespace LeadersApi.Models.Responses.Weather
{
    public class CurrentWeather
    {
        public float temp_c { get; set; }
        public int humidity { get; set; }
        public int cloud { get; set; }
        public float feelslike_c {get;set;}
    }
}