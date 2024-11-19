using Application.Interfaces;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeatherForecastApp.Models;
using System.Security.Claims;

namespace WeatherForecastApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly WeatherBatchService _batchService;
        private readonly IFavoriteRepository _favoriteRepository;

        public HomeController(WeatherBatchService batchService, IFavoriteRepository favoriteRepository)
        {
            _batchService = batchService;
            _favoriteRepository = favoriteRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SearchCities(string cityNames)
        {
            var cityList = cityNames.Split(',').Select(city => city.Trim()).ToList();
            var cities = cityList.Select(city => new CityWeatherViewModel
            {
                CityName = city,
                Temperature = GetTemperatureFromApis(city).Result
            }).ToList();

            return View("Index", cities);
        }

        [Authorize]
        public IActionResult Favorites()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var favoriteCitiesList = _favoriteRepository.GetFavoriteCitiesAsync(int.Parse(userId));

            return View(favoriteCitiesList);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddToFavorites(string cityName)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(cityName))
                return BadRequest("City name cannot be empty");

            _favoriteRepository.AddFavoriteAsync(int.Parse(userId), int.Parse(cityName));

            return Json(new { success = true, city = cityName });
        }

        private async Task<double?> GetTemperatureFromApis(string city)
        {
            var temperature = await _batchService.GetWeatherAsync(city);

            if (temperature == null)
                return null;

            return temperature;
        }

    }
}
