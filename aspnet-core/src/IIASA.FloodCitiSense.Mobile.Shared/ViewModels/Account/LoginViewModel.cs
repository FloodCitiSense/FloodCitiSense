using Abp.Localization;
using Abp.MultiTenancy;
using Abp.UI;
using Abp.Web.Models;
using Acr.UserDialogs;
using Flurl.Http;
using IIASA.FloodCitiSense.ApiClient;
using IIASA.FloodCitiSense.Authorization.Accounts;
using IIASA.FloodCitiSense.Authorization.Accounts.Dto;
using IIASA.FloodCitiSense.Mobile.Core.Base;
using IIASA.FloodCitiSense.Mobile.Core.Commands;
using IIASA.FloodCitiSense.Mobile.Core.Core.DataStorage;
using IIASA.FloodCitiSense.Mobile.Core.Core.Entity;
using IIASA.FloodCitiSense.Mobile.Core.Core.Threading;
using IIASA.FloodCitiSense.Mobile.Core.Extensions.MarkupExtensions;
using IIASA.FloodCitiSense.Mobile.Core.Localization;
using IIASA.FloodCitiSense.Mobile.Core.Services.Account;
using IIASA.FloodCitiSense.Mobile.Core.Services.Navigation;
using IIASA.FloodCitiSense.Views;
using IIASA.FloodCitiSense.Views.Account;
using MvvmHelpers;
using Newtonsoft.Json.Linq;
using Plugin.FacebookClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using MonkeyCache.LiteDB;
using Xamarin.Forms;

namespace IIASA.FloodCitiSense.ViewModels.Account
{
    public class LoginViewModel : XamarinViewModel
    {
        public ICommand LoginUserCommand => HttpRequestCommand.Create(LoginUserAsync);
        public ICommand RegisterCommand => HttpRequestCommand.Create(RegisterAsync);

        private async Task RegisterAsync()
        {
            await NavigationService.SetMainPage<RegisterPage>(null, true);
        }

        public ICommand ChangeTenantCommand => AsyncCommand.Create(ChangeTenantAsync);
        public ICommand PageAppearingCommand => AsyncCommand.Create(PageAppearingAsync);
        public ICommand ForgotPasswordCommand => AsyncCommand.Create(ForgotPasswordAsync);
        public ICommand EmailActivationCommand => AsyncCommand.Create(EmailActivationAsync);

        public string CurrentTenancyNameOrDefault => _applicationContext.CurrentTenant != null
            ? _applicationContext.CurrentTenant.TenancyName
            : L.Localize("NotSelected");

        private readonly ProxyTokenAuthControllerService _proxyTokenAuthControllerService;
        private readonly IAccountAppService _accountAppService;
        private readonly IApplicationContext _applicationContext;
        private readonly IDataStorageManager _dataStorageManager;
        private readonly IAccountService _accountService;
        private readonly INavigationService _navigationService;
        private readonly IFacebookService _facebookManager;
        private readonly IGoogleService _googleManager;
        private bool _isLoginEnabled;
        private string _tenancyName;
        private string _navigationData;
        private bool _isAutoLoggingIn;
        private ObservableRangeCollection<LanguageInfo> _languages;
        private LanguageInfo _selectedLanguage;
        private bool _isInitialized;

        public LoginViewModel(
            ProxyTokenAuthControllerService proxyTokenAuthControllerService,
            IAccountAppService accountAppService,
            IApplicationContext applicationContext,
            IDataStorageManager dataStorageManager,
            IAccountService accountService,
            INavigationService navigationService,
            IFacebookService facebookManager,
            IGoogleService googleManager)
        {
            _proxyTokenAuthControllerService = proxyTokenAuthControllerService;
            _accountAppService = accountAppService;
            _applicationContext = applicationContext;
            _dataStorageManager = dataStorageManager;
            _accountService = accountService;
            _navigationService = navigationService;
            _facebookManager = facebookManager;
            _googleManager = googleManager;

            this.GoogleLoginCommand = new Command(this.GoogleLogin);
            this.GoogleLogoutCommand = new Command(this.GoogleLogout);

            FacebookLoginCommand = new Command(async () => await this.FacebookLoginAsync());
            this.FacebookLogoutCommand = new Command(this.FacebookLogout);

            if (_applicationContext?.Configuration != null)
            {
                _languages = new ObservableRangeCollection<LanguageInfo>(_applicationContext?.Configuration?.Localization?.Languages);
                _selectedLanguage = _languages?.FirstOrDefault(l => l.Name == _applicationContext?.CurrentLanguage?.Name);
            }
            _isInitialized = false;
            _isPasswordHide = true;
            ShowHideTapCommand = new Command(() =>
            {
                IsPasswordHide = !IsPasswordHide;
            });
        }

        private const string ExternalAuthUri = "/api/TokenAuth/ExternalAuthenticate";

        /// <summary>
        ///     The google user
        /// </summary>
        private AppUser appUser;

        /// <summary>
        ///     Gets or sets the google user.
        /// </summary>
        /// <value>
        ///     The google user.
        /// </value>
        public AppUser AppUser
        {
            get => this.appUser;

            set
            {
                appUser = value;
                RaisePropertyChanged(() => AppUser);
            }
        }

        /// <summary>
        ///     Gets or sets the facebook login command.
        /// </summary>
        /// <value>
        ///     The facebook login command.
        /// </value>
        public ICommand FacebookLoginCommand { get; set; }

        /// <summary>
        /// Gets or sets the GoogleLogoutCommand
        /// </summary>
        /// <value>
        /// Google Logout Command
        /// </value>
        public ICommand GoogleLogoutCommand { get; set; }


        /// <summary>
        ///     Gets or sets the facebook logout command.
        /// </summary>
        /// <value>
        ///     The facebook logout command.
        /// </value>
        public ICommand FacebookLogoutCommand { get; set; }

        /// <summary>
        ///     Gets or sets the google login command.
        /// </summary>
        /// <value>
        ///     The google login command.
        /// </value>
        public ICommand GoogleLoginCommand { get; set; }


        public bool IsLogedIn
        {
            get => this._isLogedIn;

            set
            {
                _isLogedIn = value;
                RaisePropertyChanged(() => IsLogedIn);
            }
        }

        /// <summary>
        ///     The is loged in
        /// </summary>
        private bool _isLogedIn;


        /// <summary>
        /// Facebooks the login.
        /// </summary>
        private async Task FacebookLoginAsync()
        {
            //this._facebookManager.Login(this.OnLoginComplete);
            await CrossFacebookClient.Current.LoginAsync(new string[] { "email" });

            EventHandler<FBEventArgs<bool>> onLogin = async (s, a) =>
               {
                   switch (a.Status)
                   {
                       case FacebookActionStatus.Completed:
                           var appUser = new AppUser
                           {
                               Provider = "Facebook",
                           };
                           Type myType = s.GetType();
                           IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());

                           foreach (PropertyInfo prop in props)
                           {
                               object propValue = prop.GetValue(s, null);
                               if (prop.Name == "ActiveToken")
                               {
                                   appUser.Token = propValue?.ToString();
                               }
                               if (prop.Name == "ActiveUserId")
                               {
                                   appUser.Id = propValue?.ToString();
                               }
                           }

                           await OnLoginComplete(appUser, "LoginSuccess");
                           break;
                   }
               };
            CrossFacebookClient.Current.OnLogin -= onLogin;
            CrossFacebookClient.Current.OnLogin += onLogin;
        }

        /// <summary>
        /// Facebooks the logout.
        /// </summary>
        private void FacebookLogout()
        {
            this._facebookManager.Logout();
            this.IsLogedIn = false;
        }

        /// <summary>
        ///     Google the login.
        /// </summary>
        private void GoogleLogin()
        {
            //this._googleManager.Login(this.OnLoginComplete);
        }

        /// <summary>
        ///     Google the logout.
        /// </summary>
        private void GoogleLogout()
        {
            this._googleManager.Logout();
            this.IsLogedIn = false;
        }

        /// <summary>
        ///     Called when [login complete].
        /// </summary>
        /// <param name="user">The google user.</param>
        /// <param name="message">The message.</param>
        private async Task OnLoginComplete(AppUser user, string message)
        {
            if (user != null)
            {
                this.AppUser = user;
                try
                {
                    var ext = await _proxyTokenAuthControllerService.ExternalAuthenticate(user.Provider, user.Id, user.Token);

                    if (ext != null)
                    {
                        Debug.WriteLine("Success");
                        Debug.WriteLine(ext.AccessToken);
                        Debug.WriteLine(ext.RefreshToken);

                        await _dataStorageManager.StoreAsync(DataStorageKey.AccessToken, ext.AccessToken);
                        await _dataStorageManager.StoreAsync(DataStorageKey.RefreshToken, ext.RefreshToken, true);
                        await _dataStorageManager.StoreAsync(DataStorageKey.LastTokenRequestedTime, DateTime.UtcNow);
                        this.IsLogedIn = true;
                        await _navigationService.SetMainPage<MasterPage>();
                        await _navigationService.SetDetailPageAsync(typeof(MainPage));
                    }
                }
                catch (System.Exception e)
                {
                    Console.WriteLine(e);
                    await UserDialogs.Instance.ConfirmAsync("Error".Translate(), "AuthFailed".Translate(), "Ok".Translate());
                }

            }
            else
            {
                await UserDialogs.Instance.ConfirmAsync("Error".Translate(), message, "Ok".Translate());
            }
        }

        private async Task LoginExternalUserAsync(AppUser googleUser)
        {

            using (IFlurlClient client = new FlurlClient(ApiUrlConfig.BaseUrl))
            {
                client.WithHeader("Accept", new MediaTypeWithQualityHeaderValue("application/json"));
                client.WithHeader("User-Agent", AbpApiClient.UserAgent);
                client.WithHeader("Abp.TenantId", "1");

                var response = await client
                    .Request(ExternalAuthUri)
                    .PostJsonAsync(new
                    {
                        AuthProvider = "Google",
                        ProviderKey = googleUser.Id,
                        ProviderAccessCode = googleUser.Token
                    })
                    .ReceiveJson<AjaxResponse<JObject>>();

                if (!response.Success)
                {
                    throw new UserFriendlyException(response.Error.Message + ": " + response.Error.Details);
                }
            }
        }

        private bool _isPasswordHide;
        public bool IsPasswordHide
        {
            get { return _isPasswordHide; }
            set
            {
                _isPasswordHide = value;
                OnPropertyChanged();
                OnPropertyChanged("ShowHideIcon");
            }
        }

        public string ShowHideIcon
        {
            get
            {
                return IsPasswordHide ? "hidePass.png" : "showPass.png";
            }
        }

        public ICommand LoginCommand { get; }
        public ICommand ShowHideTapCommand { get; }

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

        private async Task ForgotPasswordAsync()
        {
            await NavigationService.SetMainPage<ForgotPasswordView>(null,true);
        }

        private async Task EmailActivationAsync()
        {
            await NavigationService.SetMainPage<EmailActivationView>();
        }

        private async Task ChangeLanguage()
        {
            _applicationContext.CurrentLanguage = _selectedLanguage;

            await UserConfigurationManager.GetAsync(async () =>
            {
                await NavigationService.SetMainPage<LoginView>(clearNavigationHistory: true);
            });
        }

        private bool IsFromLogout()
        {
            return _navigationData == "From-Logout";
        }

        private void PopulateCredentialsFromStorage()
        {
            UserName = _dataStorageManager.Retrieve(DataStorageKey.Username, "");
            TenancyName = _dataStorageManager.Retrieve(DataStorageKey.TenancyName, "");
            var tenantId = _dataStorageManager.Retrieve<int?>(DataStorageKey.TenantId);

            if (tenantId == null)
            {
                _applicationContext.SetAsHost();
            }
            else
            {
                _applicationContext.SetAsTenant(TenancyName, tenantId.Value);
            }

            SetPassword();
            RaisePropertyChanged(() => CurrentTenancyNameOrDefault);
        }

        private Task PageAppearingAsync()
        {
            PopulateCredentialsFromStorage();
            return Task.CompletedTask;
            //await AutoLoginIfRequired();
        }

        public override Task InitializeAsync(object navigationData)
        {
            _navigationData = (string)navigationData;
            _isInitialized = true;
            return Task.CompletedTask;
        }

        public string TenancyName
        {
            get => _tenancyName;
            set
            {
                _tenancyName = value;
                RaisePropertyChanged(() => TenancyName);
            }
        }

        public string UserName
        {
            get => _accountService.AbpAuthenticateModel.UserNameOrEmailAddress;
            set
            {
                _accountService.AbpAuthenticateModel.UserNameOrEmailAddress = value;
                SetLoginButtonEnabled();
                RaisePropertyChanged(() => UserName);
            }
        }

        public string Password
        {
            get => _accountService.AbpAuthenticateModel.Password;
            set
            {
                _accountService.AbpAuthenticateModel.Password = value;
                SetLoginButtonEnabled();
                RaisePropertyChanged(() => Password);
            }
        }

        public void SetLoginButtonEnabled()
        {
            IsLoginEnabled = !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password);
        }

        public bool IsLoginEnabled
        {
            get => _isLoginEnabled;
            set
            {
                _isLoginEnabled = value;
                RaisePropertyChanged(() => IsLoginEnabled);
            }
        }

        public bool IsAutoLoggingIn
        {
            get => _isAutoLoggingIn;
            set
            {
                _isAutoLoggingIn = value;
                RaisePropertyChanged(() => IsAutoLoggingIn);
            }
        }

        private void SetPassword()
        {
            if (IsFromLogout())
            {
                Password = null;
                //_dataStorageManager.RemoveIfExists(DataStorageKey.Password);
            }
            else
            {
                //Password = _dataStorageManager.Retrieve(DataStorageKey.Password, "", true);
            }
        }

        private async Task AutoLoginIfRequired()
        {
            if (Password == null)
            {
                return;
            }

            IsAutoLoggingIn = true;
            await SetBusyAsync(async () =>
            {
                await LoginUserAsync();
                IsAutoLoggingIn = false;
            }, "Authenticating".Translate());
        }

        private async Task LoginUserAsync()
        {
            await SetBusyAsync(async () =>
            {
                UserName = UserName.Trim();
                await _accountService.LoginUserAsync();
                if (_applicationContext?.AppInformation?.AccessToken != null)
                {
                    Barrel.Current.EmptyAll();
                    await _navigationService.SetMainPage<MasterPage>(clearNavigationHistory: true);
                    await _navigationService.SetDetailPageAsync(typeof(MainPage));
                }
            });
        }

        private async Task ChangeTenantAsync()
        {
            var promptResult = await UserDialogs.Instance.PromptAsync(new PromptConfig
            {
                Message = L.Localize("LeaveEmptyToSwitchToHost"),
                Text = _applicationContext.CurrentTenant?.TenancyName ?? "",
                OkText = L.Localize("Ok"),
                CancelText = L.Localize("Cancel"),
                Title = L.Localize("ChangeTenant"),
                Placeholder = L.LocalizeWithThreeDots("TenancyName"),
                MaxLength = AbpTenantBase.MaxTenancyNameLength
            });

            if (!promptResult.Ok)
            {
                return;
            }

            if (string.IsNullOrEmpty(promptResult.Text))
            {
                _applicationContext.SetAsHost();
                ApiUrlConfig.ResetBaseUrl();
                RaisePropertyChanged(() => CurrentTenancyNameOrDefault);
            }
            else
            {
                await SetTenantAsync(promptResult.Text);
            }
        }

        private async Task SetTenantAsync(string tenancyName)
        {
            await SetBusyAsync(async () =>
            {
                await WebRequestExecuter.Execute(
                    async () => await _accountAppService.IsTenantAvailable(
                        new IsTenantAvailableInput { TenancyName = tenancyName }),
                    result => IsTenantAvailableExecuted(result, tenancyName)
                );
            });
        }

        private async Task IsTenantAvailableExecuted(IsTenantAvailableOutput result, string tenancyName)
        {
            var tenantAvailableResult = result;

            switch (tenantAvailableResult.State)
            {
                case TenantAvailabilityState.Available:
                    _applicationContext.SetAsTenant(tenancyName, tenantAvailableResult.TenantId.Value);
                    ApiUrlConfig.ChangeBaseUrl(tenantAvailableResult.ServerRootAddress);
                    RaisePropertyChanged(() => CurrentTenancyNameOrDefault);
                    break;
                case TenantAvailabilityState.InActive:
                    UserDialogs.Instance.HideLoading();
                    await UserDialogs.Instance.AlertAsync(L.Localize("TenantIsNotActive", tenancyName));
                    break;
                case TenantAvailabilityState.NotFound:
                    UserDialogs.Instance.HideLoading();
                    await UserDialogs.Instance.AlertAsync(L.Localize("ThereIsNoTenantDefinedWithName{0}", tenancyName));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
