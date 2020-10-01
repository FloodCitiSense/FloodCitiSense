using Acr.UserDialogs;
using IIASA.FloodCitiSense.Mobile.Core.Base;
using IIASA.FloodCitiSense.Mobile.Core.Commands;
using IIASA.FloodCitiSense.Mobile.Core.Core.DataStorage;
using IIASA.FloodCitiSense.Mobile.Core.Extensions.MarkupExtensions;
using IIASA.FloodCitiSense.Mobile.Core.Interface;
using IIASA.FloodCitiSense.Mobile.Core.Services.Navigation;
using IIASA.FloodCitiSense.MultiTenancy;
using IIASA.FloodCitiSense.MultiTenancy.Dto;
using IIASA.FloodCitiSense.Views;
using IIASA.FloodCitiSense.Views.Account;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace IIASA.FloodCitiSense.ViewModels.Init
{
    public class SelectCityPageModel : XamarinViewModel
    {
        private readonly IDataStorageManager _dataStorageManager;
        private readonly INavigationService _navigationService;
        private readonly ITenantAppService _tenantAppService;
        private readonly INavigationHelper _navigationHelper;
        public Command SubmitCommand { get; set; }

        public ICommand PageAppearingCommand => HttpRequestCommand.Create(PageAppearingAsync);

        private async Task PageAppearingAsync()
        {
            AllTenants.Clear();
            await SetBusyAsync(async () =>
            {
                var allTenants = await _tenantAppService.GetAllTenants();
                foreach (var tenant in allTenants)
                {
                    AllTenants.Add(tenant);
                }
            }, "LoadingCity".Translate());
        }

        public SelectCityPageModel(IDataStorageManager dataStorageManager, INavigationService navigationService, ITenantAppService tenantAppService, INavigationHelper navigationHelper)
        {
            AllTenants = new ObservableCollection<AllTenantDto>();
            this._dataStorageManager = dataStorageManager;
            this._navigationService = navigationService;
            _tenantAppService = tenantAppService;
            _navigationHelper = navigationHelper;
            this.SubmitCommand = new Command(OnSubmitClickAsync);
            //AllTenants.Add(new AllTenantDto
            //{
            //    Id = 0,
            //    Name = "Birmingham"
            //});
            //AllTenants.Add(new AllTenantDto
            //{
            //    Id = 1,
            //    Name = "Brussels"
            //});
            //AllTenants.Add(new AllTenantDto
            //{
            //    Id = 2,
            //    Name = "Rotterdam"
            //});
        }

        private async void OnSubmitClickAsync()
        {
            if (SelectedTenant == null)
            {
                await UserDialogs.Instance.ConfirmAsync("SelectCityToProceed".Translate(), "SelectCity".Translate(), "Ok".Translate(), "Cancel".Translate());
            }
            else
            {
                _navigationHelper.ResetDataStore();
                await _dataStorageManager.StoreAsync(DataStorageKey.TenancyName, SelectedTenant.Name);
                await _dataStorageManager.StoreAsync(DataStorageKey.TenantId, SelectedTenant.Id);
                await _navigationService.SetMainPage<MasterPage>();
                await _navigationService.SetDetailPageAsync(typeof(LoginView));
            }
        }

        private ObservableCollection<AllTenantDto> _allTenants;

        public ObservableCollection<AllTenantDto> AllTenants
        {
            get => _allTenants;
            set
            {
                _allTenants = value;
                RaisePropertyChanged(() => AllTenants);
            }
        }

        //public List<AllTenantDto> AllTenants { get; set; }
        public AllTenantDto SelectedTenant { get; set; }
    }
}