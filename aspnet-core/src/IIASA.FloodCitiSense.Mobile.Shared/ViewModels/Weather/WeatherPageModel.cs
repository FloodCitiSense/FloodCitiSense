//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WeatherPageModel.cs" company="IIASA">
//    EOS
//  </copyright>
//  <summary>
//   WeatherPageModel.cs
//  </summary>
//  --------------------------------------------------------------------------------------------------------------------

using IIASA.FloodCitiSense.Mobile.Core.Base;
using IIASA.FloodCitiSense.Mobile.Core.Core.Threading;
using IIASA.FloodCitiSense.Mobile.Core.Services.Element;
using IIASA.FloodCitiSense.Mobile.Core.Services.Weather;
using IIASA.FloodCitiSense.Mobile.Core.Services.Weather.Dto;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IIASA.FloodCitiSense.ViewModels.Weather
{
    public class WeatherPageModel : XamarinViewModel
    {
        private readonly IWeatherService _weatherService;
        public readonly IElementService ElementService;
        private WeatherResponse _weatherResponse;

        public WeatherPageModel(IWeatherService weatherService, IElementService elementService)
        {
            _weatherService = weatherService;
            ElementService = elementService;
            WeatherResponse = new WeatherResponse();
        }

        public WeatherResponse WeatherResponse
        {
            get => _weatherResponse;
            set
            {
                _weatherResponse = value;
                RaisePropertyChanged(() => WeatherResponse);
            }
        }

        public ICommand PageAppearingCommand => AsyncCommand.Create(PageAppearing);

        private async Task PageAppearing()
        {
            var res = await _weatherService.GetCurrentWeather();

            if (res != null)
            {
                WeatherResponse = res;
            }
        }
    }
}