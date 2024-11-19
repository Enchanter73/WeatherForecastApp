using Application.Configurations;
using Application.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace Infastructure.Services
{
    public class WeatherApiService : IWeatherApiService
    {
        private readonly HttpClient _httpClient;
        private readonly WeatherApiConfigurations _configurations;

        public WeatherApiService(HttpClient httpClient, IOptions<WeatherApiConfigurations> configurations)
        {
            _httpClient = httpClient;
            _configurations = configurations.Value;
        }

        public async Task<double?> GetAverageTemperatureAsync(string cityName)
        {
            var temp1 = await GetTemperatureFromApi1Async(cityName);
            var temp2 = await GetTemperatureFromApi2Async(cityName);

            // If both APIs fail, return null
            if (temp1 == null && temp2 == null) return default;

            // Calculate the average of the available temperatures
            if (temp1 != null && temp2 != null) return (temp1.Value + temp2.Value) / 2;

            return (temp1 ?? temp2); 
        }

        private async Task<double?> GetTemperatureFromApi1Async(string cityName)
        {
            try
            {
                string url = $"http://api.weatherstack.com/current?access_key={_configurations.OpenWeatherApiKey}&query={cityName}";

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(content);

                return json["current"]?["temperature"]?.Value<double>();
            }
            catch
            {
                return null; // Log the error later
            }
        }

        private async Task<double?> GetTemperatureFromApi2Async(string cityName)
        {
            try
            {
                string url = $"https://api.weatherapi.com/v1/current.json?key={_configurations.WeatherApiKey}&q={cityName}";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(content);

                return json["current"]?["temp_c"]?.Value<double>();
            }
            catch
            {
                return null; // Log the error later
            }
        }
    }
}
