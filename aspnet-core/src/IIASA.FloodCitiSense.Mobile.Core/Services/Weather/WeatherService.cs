//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WeatherService.cs" company="IIASA">
//    EOS
//  </copyright>
//  <summary>
//   WeatherService.cs
//  </summary>
//  --------------------------------------------------------------------------------------------------------------------

using Abp.Dependency;
using Flurl.Http;
using IIASA.FloodCitiSense.Mobile.Core.Core;
using IIASA.FloodCitiSense.Mobile.Core.Services.Location;
using IIASA.FloodCitiSense.Mobile.Core.Services.Weather.Dto;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using MonkeyCache.LiteDB;

namespace IIASA.FloodCitiSense.Mobile.Core.Services.Weather
{
    public class WeatherService : IWeatherService, ISingletonDependency
    {
        private readonly ILocationService _locationService;

        private const string CacheKey = "WeatherInfo";

        public WeatherService(ILocationService locationService)
        {
            _locationService = locationService;
        }

        public async Task<string> GetCurrentWeatherText()
        {
            try
            {
                var data = await GetCurrentWeather();
                return data == null ? string.Empty : $"{data.Main.Temp:F1} °C   {data.Weather.FirstOrDefault()?.Description}";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return await Task.FromResult(string.Empty);
            }
        }

        public async Task<string> GetCurrentCityText()
        {
            try
            {
                var data = await GetCurrentWeather();
                return data == null ? string.Empty : data.Name;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return await Task.FromResult(string.Empty);
            }
        }

        public async Task<WeatherResponse> GetCurrentWeather()
        {
            try
            {
                if (!Barrel.Current.IsExpired(CacheKey)) return Barrel.Current.Get<WeatherResponse>(CacheKey);
                var cul = CultureInfo.CurrentCulture.Name.Substring(0, 2);
                var pos = await _locationService.GetLastKnownLocationAsync();
                if (pos == null) return null;
                var data = await string.Format(Constant.WeatherApiUrl, pos.Latitude, pos.Longitude, cul).GetJsonAsync<WeatherResponse>();
                if (data != null)
                    Barrel.Current.Add(CacheKey, data, TimeSpan.FromHours(2));
                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}