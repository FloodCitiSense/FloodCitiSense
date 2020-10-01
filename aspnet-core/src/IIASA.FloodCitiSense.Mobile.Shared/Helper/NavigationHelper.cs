using Abp.Dependency;
using Acr.UserDialogs;
using IIASA.FloodCitiSense.Mobile.Core.Core.DataStorage;
using IIASA.FloodCitiSense.Mobile.Core.Extensions.MarkupExtensions;
using IIASA.FloodCitiSense.Mobile.Core.Interface;
using IIASA.FloodCitiSense.Mobile.Core.Services.Navigation;
using IIASA.FloodCitiSense.Views;
using IIASA.FloodCitiSense.Views.Account;
using IIASA.FloodCitiSense.Views.Init;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace IIASA.FloodCitiSense.Helper
{
    /// <summary>NavigationHelper</summary>
    /// <seealso cref="IIASA.FloodCitiSense.Mobile.Core.Interface.INavigationHelper" />
    /// <seealso cref="Abp.Dependency.ISingletonDependency" />
    public class NavigationHelper : INavigationHelper, ISingletonDependency
    {
        private readonly INavigationService _navigationService;
        private readonly IDataStorageManager _dataStorageManager;

        /// <summary>Initializes a new instance of the <see cref="NavigationHelper"/> class.</summary>
        /// <param name="navigationService">The navigation service.</param>
        /// <param name="dataStorageManager">The data storage manager.</param>
        public NavigationHelper(INavigationService navigationService, IDataStorageManager dataStorageManager)
        {
            _navigationService = navigationService;
            _dataStorageManager = dataStorageManager;
        }

        /// <summary>Initializes the navigation.</summary>
        public async Task InitNavigation(bool lauchFromNotification)
        {
            if (VersionTracking.IsFirstLaunchEver && !lauchFromNotification)
            {
                ResetDataStore();
                await SetWelcomePage();
            }
            else
            {
                if (VersionTracking.IsFirstLaunchForCurrentBuild && !lauchFromNotification || CheckIfRefreshTokenExpired())
                {
                    ResetDataStore();
                    await SetLoginView();
                }
                else
                {
                    await SetMainPage();
                }
            }
        }

        private async Task SetWelcomePage()
        {
            _dataStorageManager.RemoveIfExists(DataStorageKey.WelcomeViewed);
            _dataStorageManager.RemoveIfExists(DataStorageKey.TermsAccepted);
            await _navigationService.SetMainPage<WelcomePage>(null, true);
        }

        private async Task SetLoginView()
        {
            await _navigationService.SetMainPage<MasterPage>();
            await _navigationService.SetDetailPageAsync(typeof(LoginView));
        }

        private async Task SetMainPage()
        {
            await _navigationService.SetMainPage<MasterPage>();
            await _navigationService.SetDetailPageAsync(typeof(MainPage));
        }

        /// <summary>Handles the tenant not available exception.</summary>
        public async Task HandleTenantNotAvailableException()
        {
            ResetDataStore();
            await _navigationService.SetMainPage<MasterPage>(clearNavigationHistory: true);
            await _navigationService.SetDetailPageAsync(typeof(SelectCityPage));
        }

        /// <summary>Handles the unauthorized exception.</summary>
        public async Task HandleUnAuthorizedException()
        {
            ResetDataStore();
            await _navigationService.SetMainPage<MasterPage>(clearNavigationHistory: true);
            await _navigationService.SetDetailPageAsync(typeof(LoginView));
            UserDialogs.Instance.Toast("ProblemVerifyingUserLoginAgain".Translate(), TimeSpan.FromSeconds(15));
        }

        /// <summary>Resets the data store.</summary>
        public void ResetDataStore()
        {
            _dataStorageManager.RemoveIfExists(DataStorageKey.AccessToken);
            _dataStorageManager.RemoveIfExists(DataStorageKey.LoginInfo);
            _dataStorageManager.RemoveIfExists(DataStorageKey.TenancyName);
            _dataStorageManager.RemoveIfExists(DataStorageKey.TenantId);
            _dataStorageManager.RemoveIfExists(DataStorageKey.UserConfiguration);
            _dataStorageManager.RemoveIfExists(DataStorageKey.RefreshToken);
            _dataStorageManager.RemoveIfExists(DataStorageKey.LastTokenRequestedTime);
        }

        public bool CheckIfRefreshTokenExpired()
        {
            if (_dataStorageManager.HasKey(DataStorageKey.LastTokenRequestedTime))
            {
                var lastTokenRequestedTime =
                    _dataStorageManager.Retrieve<DateTime>(DataStorageKey.LastTokenRequestedTime);

                var timeElapsedSinceLastTokenRequest = (DateTime.Now - lastTokenRequestedTime).TotalDays;
                if (timeElapsedSinceLastTokenRequest > 30.0)
                {
                    return true;
                }

                return false;
            }

            return true;
        }
    }
}
