//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IncidentMainPageModel.cs" company="IIASA">
//    EOS
//  </copyright>
//  <summary>
//   IncidentMainPageModel.cs
//  </summary>
//  --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Abp.Application.Services.Dto;
using IIASA.FloodCitiSense.ApiClient;
using IIASA.FloodCitiSense.Datatypes;
using IIASA.FloodCitiSense.Mobile.Core.Base;
using IIASA.FloodCitiSense.Mobile.Core.Commands;
using IIASA.FloodCitiSense.Mobile.Core.Core.Entity;
using IIASA.FloodCitiSense.Mobile.Core.Core.Threading;
using IIASA.FloodCitiSense.Mobile.Core.Extensions.MarkupExtensions;
using IIASA.FloodCitiSense.Mobile.Core.Interface;
using IIASA.FloodCitiSense.Mobile.Core.Models.Common;
using IIASA.FloodCitiSense.Mobile.Core.Services.Navigation;
using IIASA.FloodCitiSense.Mobile.Core.Services.Report;
using IIASA.FloodCitiSense.Views.Account;
using IIASA.FloodCitiSense.Views.Incident;
using MonkeyCache.LiteDB;
using MvvmHelpers;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace IIASA.FloodCitiSense.ViewModels.Incident
{
    public class IncidentUploadPageModel : XamarinViewModel
    {
        private readonly IIncidentService _incidentService;
        private readonly INavigationService _navigationService;
        private readonly IAccessTokenManager _accessTokenManager;
        private readonly IIncidentsAppService _incidentsAppService;
        private ObservableCollection<LocalIncident> _reports;

        public IncidentUploadPageModel(IIncidentService incidentService, INavigationService navigationService, INavigationHelper navigationHelper,
            IAccessTokenManager accessTokenManager, IIncidentsAppService incidentsAppService)
        {
            _incidentService = incidentService;
            _navigationService = navigationService;
            _accessTokenManager = accessTokenManager;
            _incidentsAppService = incidentsAppService;


            Items = new ObservableRangeCollection<LocalIncident>();
            AllItems = new ObservableRangeCollection<LocalIncident>();

            FilterOptions = new ObservableRangeCollection<DisplayItem>
            {
                new DisplayItem
                {
                    Text = "ShowAll".Translate(),
                    Value = string.Empty
                },
                new DisplayItem
                {
                    Text = "Showlastday".Translate(),
                    Value = DateTime.Today.AddDays(-1).ToString(CultureInfo.InvariantCulture)
                },
                new DisplayItem
                {
                    Text = "Showlastweek".Translate(),
                    Value = DateTime.Today.AddDays(-7).ToString(CultureInfo.InvariantCulture)
                }
            };

            SelectedFilter = new DisplayItem
            {
                Text = "ShowAll",
                Value = string.Empty
            };

            _navigationHelper = navigationHelper;
        }

        public ICommand PageAppearingCommand => AsyncCommand.Create(PageAppearing);

        public ICommand RefreshCommand => HttpRequestCommand.Create(RefreshClicked);

        private async Task RefreshClicked()
        {
            Barrel.Current.EmptyAll();
            await UpdateReportsAsync();
        }

        private async Task PageAppearing()
        {
            using (await MaterialDialog.Instance.LoadingDialogAsync(message: "LoadWithThreeDot".Translate()))
            {
                IsEmpty = false;
                await UpdateReportsAsync();
                IsEmpty = Items.Count == 0;
            }
        }


        private async Task UpdateReportsAsync()
        {
            var allReport = await _incidentService.GetAllAsync();
            AllItems.Clear();
            if (allReport != null && allReport.Any())
                foreach (var report in allReport.Where(x => x.IsUploaded)) AllItems.Add(report);
            FilterItems();
        }

        public ICommand SelectedCommand => new Command<LocalIncident>(async r => await SelectAsync(r));
        public ICommand UploadCommand => new Command<LocalIncident>(async r => await UploadAsync(r));
        public ICommand DeleteCommand => new Command<LocalIncident>(async r => await DeleteAsync(r));
        public ICommand DeleteAllCommand => new Command<LocalIncident>(async r => await DeleteAllAsync());


        public ObservableRangeCollection<LocalIncident> Items { get; set; }
        public ObservableRangeCollection<LocalIncident> AllItems { get; set; }
        public ObservableRangeCollection<DisplayItem> FilterOptions { get; }


        private bool _isEmpty;

        public bool IsEmpty
        {
            get => _isEmpty;
            set
            {
                _isEmpty = value;
                RaisePropertyChanged(() => IsEmpty);
            }
        }

        private DisplayItem _selectedFilter;
        private readonly INavigationHelper _navigationHelper;

        public DisplayItem SelectedFilter
        {
            get => _selectedFilter;
            set
            {
                _selectedFilter = value;
                RaisePropertyChanged(() => SelectedFilter);
                FilterItems();
            }
        }

        /// <summary>
        /// Filters the items.
        /// </summary>
        private void FilterItems()
        {
            Items.ReplaceRange(AllItems.Where(a =>
            {
                try
                {
                    if (_selectedFilter.Value?.Length == 0) return true;

                    var filterTime = DateTime.Parse(_selectedFilter.Value, CultureInfo.InvariantCulture);

                    return a.Date.DateTime > filterTime;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return true;
                }
            }).OrderByDescending(x => x.Date));

            IsEmpty = Items.Count == 0;
        }


        private async Task DeleteAsync(LocalIncident report)
        {
            var confirm = await MaterialDialog.Instance.ConfirmAsync("AreYouSure".Translate(), "Delete".Translate(),
                "ok".Translate(), "cancel".Translate());

            if (confirm.HasValue &&confirm.Value)
            {
                try
                {
                    using (await MaterialDialog.Instance.LoadingDialogAsync(message: "Deleting".Translate()))
                    {
                        await _incidentService.DeleteAsync(report.Id);
                        await _incidentsAppService.Delete(new EntityDto(report.IncidentId));
                        await RefreshClicked();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    await MaterialDialog.Instance.SnackbarAsync("DeleteFailed".Translate());
                }
            }
        }

        private async Task DeleteAllAsync()
        {
            var confirm = await MaterialDialog.Instance.ConfirmAsync("AreYouSure".Translate(), "Delete".Translate(),
                "ok".Translate(), "cancel".Translate());

            if (confirm.HasValue && confirm.Value)
            {
                try
                {
                    using (await MaterialDialog.Instance.LoadingDialogAsync(message: "Deleting".Translate()))
                    {
                        foreach (var localIncident in Items)
                        {
                            await _incidentService.DeleteAsync(localIncident.Id);
                            await _incidentsAppService.Delete(new EntityDto(localIncident.IncidentId));
                        }
                        await RefreshClicked();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    await MaterialDialog.Instance.SnackbarAsync("DeleteFailed".Translate());
                }
            }
        }

        private async Task UploadAsync(LocalIncident obj)
        {
            if (_accessTokenManager.IsUserLoggedIn)
                await SetBusyAsync(async () =>
                {
                    var output = await _incidentService.Upload(obj);
                });
            else
                await _navigationService.SetMainPage<LoginView>();
        }

        private async Task SelectAsync(LocalIncident obj)
        {
            using (await MaterialDialog.Instance.LoadingDialogAsync(message: "LoadWithThreeDot".Translate()))
            {
                if (obj != null)
                {
                    await _navigationService.SetMainPage<IncidentMainPage>(obj).ConfigureAwait(false);
//                    if (obj.IsLocal)
//                    {
//                        await _navigationService.SetMainPage<IncidentMainPage>(obj);
//                    }
//                    else
//                    {
//                        await _navigationService.SetMainPage<IncidentDetailsPage>(obj.IncidentId);
//                    }
                }
            }
        }

        public ObservableCollection<LocalIncident> Reports
        {
            get => _reports;
            set
            {
                _reports = value;
                RaisePropertyChanged(() => Reports);
            }
        }
    }
}