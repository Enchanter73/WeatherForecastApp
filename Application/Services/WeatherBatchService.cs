using Application.Models;
using Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

namespace Application.Services
{
    public class WeatherBatchService
    {
        private readonly IWeatherApiService _weatherApiService;
        private readonly ICacheService _cache;
        private readonly ConcurrentDictionary<string, List<WeatherRequest>> _requestQueue = new();

        public WeatherBatchService(IWeatherApiService weatherApiService, ICacheService cache)
        {
            _weatherApiService = weatherApiService;
            _cache = cache;
        }

        public async Task<double?> GetWeatherAsync(string city)
        {
            var temparature = await _cache.GetAsync<double?>($"weather:{city.ToLower()}");
            
            if(temparature.HasValue)
            {
                return temparature.Value;
            }
            
            var request = new WeatherRequest
            {
                City = city,
                CompletionSource = new TaskCompletionSource<double?>()
            };

            var requests = _requestQueue.GetOrAdd(city, _ => new List<WeatherRequest>());
            lock (requests)
            {
                requests.Add(request);
            }

            if (requests.Count == 1)
            {
                _ = Task.Run(() => ProcessBatch(city));
            }

            return await request.CompletionSource.Task;
        }

        private async Task ProcessBatch(string city)
        {
            await Task.Delay(5000);

            if (_requestQueue.TryRemove(city, out var requests))
            {
                double? temperature = null;

                try
                {
                    temperature = await _weatherApiService.GetAverageTemperatureAsync(city);
                }
                catch
                {
                    temperature = null;
                }

                if (temperature.HasValue)
                {
                    await _cache.SetAsync($"weather:{city.ToLower()}", temperature.Value, TimeSpan.FromMinutes(5)); // Cache for 5 minutes
                }

                foreach (var req in requests)
                {
                    req.CompletionSource.TrySetResult(temperature);
                }
            }
        }
    }
}
