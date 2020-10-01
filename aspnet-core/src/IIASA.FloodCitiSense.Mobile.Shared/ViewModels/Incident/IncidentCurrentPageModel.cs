//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IncidentMainPageModel.cs" company="IIASA">
//    EOS
//  </copyright>
//  <summary>
//   IncidentMainPageModel.cs
//  </summary>
//  --------------------------------------------------------------------------------------------------------------------

using Acr.UserDialogs;
using IIASA.FloodCitiSense.ApiClient;
using IIASA.FloodCitiSense.Datatypes;
using IIASA.FloodCitiSense.Mobile.Core.Base;
using IIASA.FloodCitiSense.Mobile.Core.Core.Entity;
using IIASA.FloodCitiSense.Mobile.Core.Extensions.MarkupExtensions;
using IIASA.FloodCitiSense.Mobile.Core.Interface;
using IIASA.FloodCitiSense.Mobile.Core.Services.Location;
using IIASA.FloodCitiSense.Mobile.Core.Services.Navigation;
using IIASA.FloodCitiSense.Mobile.Core.Services.Report;
using IIASA.FloodCitiSense.Mobile.Core.UI;
using IIASA.FloodCitiSense.Views;
using IIASA.FloodCitiSense.Views.Account;
using IIASA.FloodCitiSense.Views.Incident;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace IIASA.FloodCitiSense.ViewModels.Incident
{
    public class IncidentCurrentPageModel : XamarinViewModel
    {
        private readonly IAccessTokenManager _accessTokenManager;
        private readonly IApplicationContext _applicationContext;
        private readonly INavigationHelper _navigationHelper;
        private readonly IIncidentsAppService _incidentsAppService;
        private readonly ILocationService _locationService;
        private readonly INavigationService _navigationService;
        private readonly IIncidentService _incidentService;


        private bool _isEmpty;
        private ObservableCollection<LocalIncident> _reports;

        public IncidentCurrentPageModel(ILocationService locationService, IIncidentService incidentService,
            INavigationService navigationService, IApplicationContext applicationContext, INavigationHelper navigationHelper,
            IIncidentsAppService incidentsAppService, IAccessTokenManager accessTokenManager)
        {
            _locationService = locationService;
            _incidentService = incidentService;
            _navigationService = navigationService;
            _applicationContext = applicationContext;
            _navigationHelper = navigationHelper;
            _incidentsAppService = incidentsAppService;
            _accessTokenManager = accessTokenManager;
        }

        public bool IsEmpty
        {
            get => _isEmpty;
            set
            {
                _isEmpty = value;
                RaisePropertyChanged(() => IsEmpty);
            }
        }

        public ICommand SelectedCommand => new Command<LocalIncident>(this.SelectAsync);
        public ICommand HomeCommand => new Command(async () => await GoToHome());

        private async Task GoToHome()
        {
            await _navigationService.SetMainPage<MasterPage>();
            await _navigationService.SetDetailPageAsync(typeof(MainPage));
        }

        public ICommand UploadCommand => new Command<LocalIncident>(async r => await UploadAsync(r));
        public ICommand EditCommand => new Command<LocalIncident>(async r => await EditAsync(r));

        public ICommand DeleteCommand => new Command<LocalIncident>(async r => await DeleteAsync(r));

        public ObservableCollection<LocalIncident> Reports
        {
            get => _reports;
            set
            {
                _reports = value;
                RaisePropertyChanged(() => Reports);
            }
        }


        public override async Task InitializeAsync(object navigationData)
        {
            await UpdateReportsAsync();
            IsEmpty = Reports.Count == 0;
            await base.InitializeAsync(navigationData);
        }


        private async Task UpdateReportsAsync()
        {
            var reports = await _incidentService.GetAllAsync();
            if (Reports != null)
                Reports.Clear();
            else
                Reports = new ObservableCollection<LocalIncident>();

            foreach (var report in reports.Where(x => !x.IsUploaded)) Reports.Add(report);
            IsEmpty = Reports.Count == 0;
        }

        private async Task EditAsync(LocalIncident report)
        {
            if (report != null) await _navigationService.SetMainPage<IncidentMainPage>(report);
        }

        private async Task DeleteAsync(LocalIncident report)
        {
            var accepted = await UserDialogs.Instance.ConfirmAsync("AreYouSure".Translate(),
                "Delete".Translate(), "ok".Translate(), "cancel".Translate());
            if (accepted)
            {
                await SetBusyAsync(async () => await _incidentService.DeleteAsync(report.Id)).ConfigureAwait(false);
                await UpdateReportsAsync();
            }
        }

        private async Task UploadAsync(LocalIncident obj)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                if (_accessTokenManager.IsUserLoggedIn)
                    try
                    {
                        await _incidentService.Upload(obj);
                        await UpdateReportsAsync();
                    }
                    catch (UnauthorizedAccessException exception)
                    {
                        Crashes.TrackError(exception);
                        await _navigationHelper.HandleUnAuthorizedException();
                    }
                    catch (Exception e)
                    {
                        Crashes.TrackError(e);
                    }

                else
                    await _navigationService.SetMainPage<LoginView>();
            }
            else
            {
                UserDialogHelper.Error("NoInternet");
            }
        }

        private void SelectAsync(LocalIncident obj)
        {
            if (obj != null) _navigationService.SetMainPage<IncidentMainPage>(obj);
        }
    }
}