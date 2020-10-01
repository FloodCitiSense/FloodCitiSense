using System.Linq;
using Abp.Localization;
using IIASA.FloodCitiSense.ApiClient;
using IIASA.FloodCitiSense.ApiClient.Models;
using IIASA.FloodCitiSense.Authorization.Users.Dto;
using IIASA.FloodCitiSense.Authorization.Users.Profile;
using IIASA.FloodCitiSense.Mobile.Core.Base;
using IIASA.FloodCitiSense.Mobile.Core.Core.DataStorage;
using IIASA.FloodCitiSense.Mobile.Core.Core.Threading;
using IIASA.FloodCitiSense.Views;
using IIASA.FloodCitiSense.Views.Account;
using MvvmHelpers;
using System.Threading.Tasks;
using System.Windows.Input;
using IIASA.FloodCitiSense.Mobile.Core.Core.Entity;
using IIASA.FloodCitiSense.Mobile.Core.Services.Core;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace IIASA.FloodCitiSense.ViewModels
{
    public class MySettingsViewModel : XamarinViewModel
    {
        public ICommand LogoutCommand => AsyncCommand.Create(Logout);
        public ICommand ChangePasswordCommand => AsyncCommand.Create(ChangePasswordAsync);
        public ICommand OpenSettingsCommand => new Command(OpenSettings);

        private readonly IAccessTokenManager _accessTokenManager;
        private readonly IDataStorageManager _dataStorageManager;
        private readonly IApplicationContext _applicationContext;
        private readonly AbpAuthenticateModel _abpAuthenticateModel;
        private readonly IDbService<LocalIncident> _dbService;
        private readonly IProfileAppService _profileAppService;
        private ObservableRangeCollection<LanguageInfo> _languages;
        private LanguageInfo _selectedLanguage;
        private bool _isInitialized;

        public MySettingsViewModel(
            IAccessTokenManager accessTokenManager,
            IDataStorageManager dataStorageManager,
            IApplicationContext applicationContext,
            AbpAuthenticateModel abpAuthenticateModel,
            IDbService<LocalIncident> dbService,
            IProfileAppService profileAppService)
        {
            _accessTokenManager = accessTokenManager;
            _dataStorageManager = dataStorageManager;
            _applicationContext = applicationContext;
            _abpAuthenticateModel = abpAuthenticateModel;
            _dbService = dbService;
            _profileAppService = profileAppService;
            //_languages = new ObservableRangeCollection<LanguageInfo>(_applicationContext.Configuration.Localization.Languages);
            //_selectedLanguage = _languages.FirstOrDefault(l => l.Name == _applicationContext.CurrentLanguage.Name);
            _isInitialized = false;
        }

        public override Task InitializeAsync(object navigationData)
        {
            _isInitialized = true;

            return Task.CompletedTask;
        }

        public ObservableRangeCollection<LanguageInfo> Languages
        {
            get => _languages;
            set
            {
                _languages = value;
                RaisePropertyChanged(() => Languages);
            }
        }

        public LanguageInfo SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                _selectedLanguage = value;
                RaisePropertyChanged(() => SelectedLanguage);

                if (_isInitialized)
                {
                    AsyncRunner.Run(ChangeLanguage());
                }
            }
        }

        private async Task ChangeLanguage()
        {
            _applicationContext.CurrentLanguage = _selectedLanguage;

            await WebRequestExecuter.Execute(
                async () =>
                    await _profileAppService.ChangeLanguage(new ChangeUserLanguageDto
                    {
                        LanguageName = _selectedLanguage.Name
                    }),
                async () =>
                    await UserConfigurationManager.GetAsync(async () =>
                    {
                        MessagingCenter.Send(this, MessagingCenterKeys.LanguagesChanged);
                        await NavigationService.SetDetailPageAsync(typeof(MySettingsView));
                    }));
        }

        private void OpenSettings()
        {
            AppInfo.ShowSettingsUI();
        }

        private async Task ChangePasswordAsync()
        {
            await NavigationService.SetMainPage<ChangePasswordView>();
        }

        private async Task Logout()
        {
            _accessTokenManager.Logout();
            _applicationContext.LoginInfo = null;
            _abpAuthenticateModel.Password = null;
            _applicationContext.AppInformation = null;
            foreach (var incident in _dbService.ReadAllItems().Where(x=> !x.IsLocal))
            {
                _dbService.DeleteItem(incident.Id);
            }
            _dataStorageManager.RemoveIfExists(DataStorageKey.RefreshToken);
            _dataStorageManager.RemoveIfExists(DataStorageKey.LastTokenRequestedTime);
            _dataStorageManager.RemoveIfExists(DataStorageKey.AccessToken);
            _dataStorageManager.RemoveIfExists(DataStorageKey.LoginInfo);
            //await NavigationService.SetMainPage<LoginView>("From-Logout", clearNavigationHistory: true);
            await NavigationService.SetMainPage<MasterPage>();
            await NavigationService.SetDetailPageAsync(typeof(LoginView), "From-Logout");
            //await NavigationService.SetMainPage<MainPage>();
        }
    }
}
