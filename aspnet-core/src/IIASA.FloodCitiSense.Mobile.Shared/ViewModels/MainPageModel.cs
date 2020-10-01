//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MainPageViewModel.cs" company="IIASA">
//    EOS
//  </copyright>
//  <summary>
//   MainPageViewModel.cs
//  </summary>
//  --------------------------------------------------------------------------------------------------------------------

using IIASA.FloodCitiSense.Mobile.Core.Base;
using IIASA.FloodCitiSense.Mobile.Core.Core.DataStorage;
using IIASA.FloodCitiSense.Mobile.Core.Core.Threading;
using IIASA.FloodCitiSense.Mobile.Core.Extensions.MarkupExtensions;
using IIASA.FloodCitiSense.Mobile.Core.Services.Location;
using IIASA.FloodCitiSense.Mobile.Core.Services.Navigation;
using IIASA.FloodCitiSense.Mobile.Core.Services.Weather;
using IIASA.FloodCitiSense.Mobile.Core.UI;
using IIASA.FloodCitiSense.Views.Incident;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IIASA.FloodCitiSense.ViewModels
{
    public class MainPageModel : XamarinViewModel
    {
        public ICommand PageAppearingCommand => AsyncCommand.Create(PageAppearing);
        public ICommand IncidentCommand => AsyncCommand.Create(GotoIncident);

        private async Task GotoIncident()
        {
            if (await _locationService.IsLocationEnabledAsync())
            {
                await _navigationService.SetMainPage<IncidentMainPage>();
            }
            else
            {
                UserDialogHelper.Warn("LocationDisabled");
            }
        }

        private async Task PageAppearing()
        {
            LocationText = await _weatherService.GetCurrentCityText();
            WeatherText = await _weatherService.GetCurrentWeatherText();
        }

        public MainPageModel(IWeatherService weatherService, IDataStorageManager dataStorageManager,
            ILocationService locationService, INavigationService navigationService)
        {
            this._weatherService = weatherService;
            _dataStorageManager = dataStorageManager;
            _locationService = locationService;
            _navigationService = navigationService;
            ReportAnIncident = "ReportAnIncident".Translate().ToUpperInvariant();
        }

        private readonly IWeatherService _weatherService;
        private readonly IDataStorageManager _dataStorageManager;
        private readonly ILocationService _locationService;
        private readonly INavigationService _navigationService;

        private string _weatherText;

        public string WeatherText
        {
            get => _weatherText;
            set
            {
                _weatherText = value;
                RaisePropertyChanged(() => WeatherText);
            }
        }

        public string ReportAnIncident
        {
            get => _reportAnIncident;
            set
            {
                _reportAnIncident = value;
                RaisePropertyChanged(() => ReportAnIncident);
            }
        }

        private string _locationText;
        private string _reportAnIncident;
        private string _notificationTag;

        public string LocationText
        {
            get => _locationText;
            set
            {
                _locationText = value;
                RaisePropertyChanged(() => LocationText);
            }
        }

        public string NotificationTag
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_notificationTag) || _notificationTag == "default")
                {
                    _notificationTag = _dataStorageManager.Retrieve<string>(DataStorageKey.NotificationTag);

                    if(string.IsNullOrWhiteSpace(_notificationTag))
                    {
                        _notificationTag = "-----";
                    }
                }

                return _notificationTag;
            }
            set
            {
                _notificationTag = value;
                RaisePropertyChanged(() => NotificationTag);
            }
        }
    }
}