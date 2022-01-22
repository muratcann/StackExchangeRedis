using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackExchangeRedis.Models
{
    public class ForecastViewModel
    {
        public WeatherForecast[] Forecasts { get; set; }
        public string LoadLocation { get; set; }
        public string IsCacheData { get; set; }
    }
}
