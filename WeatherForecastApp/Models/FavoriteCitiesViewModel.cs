namespace WeatherForecastApp.Models
{
    public class FavoriteCitiesViewModel
    {
        public List<CityWeatherViewModel>? FavoriteCities { get; set; }
        public CityWeatherViewModel? HottestCity { get; set; }
        public CityWeatherViewModel? ColdestCity { get; set; }
    }
}
