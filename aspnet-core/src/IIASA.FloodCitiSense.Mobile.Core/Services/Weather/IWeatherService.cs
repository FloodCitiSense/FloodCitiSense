//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IWeatherService.cs" company="IIASA">
//    EOS
//  </copyright>
//  <summary>
//   IWeatherService.cs
//  </summary>
//  --------------------------------------------------------------------------------------------------------------------

using IIASA.FloodCitiSense.Mobile.Core.Services.Weather.Dto;
using System.Threading.Tasks;

namespace IIASA.FloodCitiSense.Mobile.Core.Services.Weather
{
    public interface IWeatherService
    {
        Task<string> GetCurrentWeatherText();
        Task<string> GetCurrentCityText();
        Task<WeatherResponse> GetCurrentWeather();
    }
}