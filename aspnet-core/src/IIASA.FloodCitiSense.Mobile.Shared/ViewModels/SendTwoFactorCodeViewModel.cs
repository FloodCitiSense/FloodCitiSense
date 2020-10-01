using Acr.UserDialogs;
using IIASA.FloodCitiSense.ApiClient.Models;
using IIASA.FloodCitiSense.Authorization.Accounts;
using IIASA.FloodCitiSense.Mobile.Core.Base;
using IIASA.FloodCitiSense.Mobile.Core.Commands;
using IIASA.FloodCitiSense.Mobile.Core.Extensions.MarkupExtensions;
using IIASA.FloodCitiSense.Mobile.Core.Localization;
using IIASA.FloodCitiSense.Mobile.Core.Services.Account;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IIASA.FloodCitiSense.ViewModels
{
    public class SendTwoFactorCodeViewModel : XamarinViewModel
    {
        private readonly ProxyTokenAuthControllerService _proxyTokenAuthControllerService;
        private readonly IAccountService _accountService;

        public ICommand SendSecurityCodeCommand => HttpRequestCommand.Create(SendSecurityCodeAsync);

        public SendTwoFactorCodeViewModel(
            ProxyTokenAuthControllerService proxyTokenAuthControllerService,
            IAccountService accountService)
        {
            _proxyTokenAuthControllerService = proxyTokenAuthControllerService;
            _accountService = accountService;
            _twoFactorAuthProviders = new List<string>();
        }

        private List<string> _twoFactorAuthProviders;
        public List<string> TwoFactorAuthProviders
        {
            get => _twoFactorAuthProviders;
            set
            {
                _twoFactorAuthProviders = value;
                RaisePropertyChanged(() => TwoFactorAuthProviders);
            }
        }

        private string _selectedProvider;
        public string SelectedProvider
        {
            get => _selectedProvider;
            set
            {
                _selectedProvider = value;
                RaisePropertyChanged(() => SelectedProvider);
            }
        }

        public override Task InitializeAsync(object navigationData)
        {
            _accountService.AuthenticateResultModel = (AbpAuthenticateResultModel)navigationData;

            TwoFactorAuthProviders = _accountService.AuthenticateResultModel.TwoFactorAuthProviders.ToList();
            SelectedProvider = TwoFactorAuthProviders.FirstOrDefault();

            return Task.CompletedTask;
        }

        private async Task SendSecurityCodeAsync()
        {
            await SetBusyAsync(async () =>
            {
                await _proxyTokenAuthControllerService.SendTwoFactorAuthCode(
                    _accountService.AuthenticateResultModel.UserId,
                    _selectedProvider
                );
            }, "Authenticating".Translate());

            var promptResult = await UserDialogs.Instance.PromptAsync(new PromptConfig
            {
                Message = L.Localize("VerifySecurityCode_Information"),
                Text = "",
                OkText = L.Localize("Ok"),
                CancelText = L.Localize("Cancel"),
                Title = L.Localize("VerifySecurityCode"),
                Placeholder = L.LocalizeWithThreeDots("Code")
            });

            if (!promptResult.Ok)
            {
                return;
            }

            if (!string.IsNullOrEmpty(promptResult.Text))
            {
                _accountService.AbpAuthenticateModel.TwoFactorVerificationCode = promptResult.Text;
                _accountService.AbpAuthenticateModel.RememberClient = true;
                await SetBusyAsync(async () =>
                {
                    await _accountService.LoginUserAsync();
                }, "Authenticating".Translate());
            }
        }
    }
}
