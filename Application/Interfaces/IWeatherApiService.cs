
namespace Application.Interfaces
{
    public interface IWeatherApiService
    {
        Task<double?> GetAverageTemperatureAsync(string cityName);
    }
}
