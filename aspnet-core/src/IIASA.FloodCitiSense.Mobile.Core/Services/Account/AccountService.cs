using Abp.Dependency;
using Acr.UserDialogs;
using IIASA.FloodCitiSense.ApiClient;
using IIASA.FloodCitiSense.ApiClient.Models;
using IIASA.FloodCitiSense.Mobile.Core.Base;
using IIASA.FloodCitiSense.Mobile.Core.Core.DataStorage;
using IIASA.FloodCitiSense.Mobile.Core.Core.Threading;
using IIASA.FloodCitiSense.Mobile.Core.Localization;
using IIASA.FloodCitiSense.Mobile.Core.Services.Navigation;
using IIASA.FloodCitiSense.Sessions;
using IIASA.FloodCitiSense.Sessions.Dto;
using System;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.UI;
using IIASA.FloodCitiSense.Mobile.Core.Extensions.MarkupExtensions;

namespace IIASA.FloodCitiSense.Mobile.Core.Services.Account
{
    public class AccountService : IAccountService, ISingletonDependency
    {
        private readonly IDataStorageManager _dataStorageManager;
        private readonly IApplicationContext _applicationContext;
        private readonly ISessionAppService _sessionAppService;
        private readonly IAccessTokenManager _accessTokenManager;
        private readonly INavigationService _navigationService;

        public AccountService(
            IDataStorageManager dataStorageManager,
            IApplicationContext applicationContext,
            ISessionAppService sessionAppService,
            IAccessTokenManager accessTokenManager,
            INavigationService navigationService,
            AbpAuthenticateModel abpAuthenticateModel)
        {
            _dataStorageManager = dataStorageManager;
            _applicationContext = applicationContext;
            _sessionAppService = sessionAppService;
            _accessTokenManager = accessTokenManager;
            _navigationService = navigationService;
            AbpAuthenticateModel = abpAuthenticateModel;
        }

        public AbpAuthenticateModel AbpAuthenticateModel { get; set; }
        public AbpAuthenticateResultModel AuthenticateResultModel { get; set; }

        public async Task LoginUserAsync()
        {
            await WebRequestExecuter.Execute(LoginAsync, AuthenticateSucceed, ex => Task.CompletedTask);
        }

        private async Task<AbpAuthenticateResultModel> LoginAsync()
        {
            try
            {
                return await _accessTokenManager.LoginAsync();
            }
            catch (UserFriendlyException friendlyException)
            {
                throw new UserFriendlyException(GetLoginErrorMessage(friendlyException.Code, friendlyException));
            }
        }

        private async Task AuthenticateSucceed(AbpAuthenticateResultModel result)
        {
            AuthenticateResultModel = result;
            if (AuthenticateResultModel.ShouldResetPassword)
            {
                await UserDialogs.Instance.AlertAsync(L.Localize("ChangePasswordToLogin"), L.Localize("LoginFailed"),
                    L.Localize("Ok"));
                return;
            }

            if (AuthenticateResultModel.RequiresTwoFactorVerification)
            {
                //await _navigationService.SetMainPage<SendTwoFactorCodeView>(AuthenticateResultModel);
                return;
            }

            if (string.IsNullOrEmpty(AbpAuthenticateModel.TwoFactorVerificationCode))
            {
                await SaveCredentialsAsync(result);
            }

            await SetCurrentUserInfoAsync();
            //await UserConfigurationManager.GetAsync();
            await UserConfigurationManager.GetIfNeedsAsync();
        }

        private async Task SaveCredentialsAsync(AbpAuthenticateResultModel result)
        {

            await _dataStorageManager.StoreAsync(DataStorageKey.Username, AbpAuthenticateModel.UserNameOrEmailAddress);
            await _dataStorageManager.StoreAsync(DataStorageKey.AccessToken, result.AccessToken);
            await _dataStorageManager.StoreAsync(DataStorageKey.RefreshToken, result.RefreshToken, true);
            await _dataStorageManager.StoreAsync(DataStorageKey.LastTokenRequestedTime, DateTime.UtcNow);

            if (_applicationContext.CurrentTenant != null)
            {
                await _dataStorageManager.StoreAsync(DataStorageKey.TenancyName,
                    _applicationContext.CurrentTenant?.TenancyName);
                await _dataStorageManager.StoreAsync(DataStorageKey.TenantId,
                    _applicationContext.CurrentTenant?.TenantId);
            }

            var applicationContext = this._applicationContext;
            if (applicationContext != null)
            {
                applicationContext.AppInformation = new AppInformation();
                if (_dataStorageManager.HasKey(DataStorageKey.LastTokenRequestedTime))
                {
                    applicationContext.AppInformation.LastTokenRequestedTime =
                        _dataStorageManager.Retrieve<DateTime>(DataStorageKey.LastTokenRequestedTime);
                }

                if (_dataStorageManager.HasKey(DataStorageKey.AccessToken))
                {
                    applicationContext.AppInformation.AccessToken =
                        _dataStorageManager.Retrieve<string>(DataStorageKey.AccessToken);
                }

                if (_dataStorageManager.HasKey(DataStorageKey.RefreshToken))
                {
                    applicationContext.AppInformation.RefreshToken =
                        _dataStorageManager.Retrieve<string>(DataStorageKey.RefreshToken, shouldDecrpyt: true);
                }
            }
        }

        private async Task SetCurrentUserInfoAsync()
        {
            await WebRequestExecuter.Execute(async () =>
                await _sessionAppService.GetCurrentLoginInformations(), GetCurrentUserInfoExecutedAsync);
        }

        private Task GetCurrentUserInfoExecutedAsync(GetCurrentLoginInformationsOutput result)
        {
            _applicationContext.LoginInfo = result;
            _dataStorageManager.StoreAsync(DataStorageKey.LoginInfo, result);
            return Task.CompletedTask;
        }

        private static string GetLoginErrorMessage(int loginErrorCode, UserFriendlyException friendlyException)
        {
            switch (loginErrorCode)
            {
                case (int) AbpLoginResultType.InvalidPassword:
                    return $"{"FailedLogin".Translate()}: {"InvalidUserNameOrPassword".Translate()}";
                case (int) AbpLoginResultType.LockedOut:
                    return $"{"FailedLogin".Translate()}: {"UserLockedOut".Translate()}";
                case (int) AbpLoginResultType.InvalidUserNameOrEmailAddress:
                    return $"{"FailedLogin".Translate()}: {"InvalidUserNameOrEmailAddress".Translate()}";
                case (int) AbpLoginResultType.UserEmailIsNotConfirmed:
                    return $"{"FailedLogin".Translate()}: {"UserEmailIsNotConfirmed".Translate()}";
                default:
                    return $"{friendlyException.Message}: {friendlyException.Details}";
            }
        }
    }
}