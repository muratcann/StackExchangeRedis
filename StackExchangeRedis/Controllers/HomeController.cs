using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using StackExchangeRedis.Data;
using StackExchangeRedis.Extensions;
using StackExchangeRedis.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace StackExchangeRedis.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDistributedCache _cache;
        private readonly IWeatherForecastService _forecastService;

        public HomeController(ILogger<HomeController> logger, IDistributedCache cache, IWeatherForecastService forecastService)
        {
            _logger = logger;
            _cache = cache;
            _forecastService = forecastService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<ForecastViewModel> LoadForecast()
        {
            ForecastViewModel response = new ForecastViewModel();
            response.Forecasts = null;
            response.LoadLocation = null;

            string recordKey = "WeatherForecast_" + DateTime.Now.ToString("yyyyMMdd_hhmm");

            response.Forecasts = await _cache.GetRecordAsync<WeatherForecast[]>(recordKey);

            if (response.Forecasts is null)
            {
                response.Forecasts = await _forecastService.GetForecastAsync(DateTime.Now);

                response.LoadLocation = $"Loaded from API at { DateTime.Now }";
                response.IsCacheData = "";

                await _cache.SetRecordAsync(recordKey, response.Forecasts);
            }
            else
            {
                response.LoadLocation = $"Loaded from the cache at { DateTime.Now }";
                response.IsCacheData = "text-danger";
            }

            return response;
        }
    }
}
