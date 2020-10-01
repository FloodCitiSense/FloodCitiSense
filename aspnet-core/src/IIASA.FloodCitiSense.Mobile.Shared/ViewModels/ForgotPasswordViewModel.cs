using Acr.UserDialogs;
using IIASA.FloodCitiSense.Authorization.Accounts;
using IIASA.FloodCitiSense.Authorization.Accounts.Dto;
using IIASA.FloodCitiSense.Mobile.Core.Base;
using IIASA.FloodCitiSense.Mobile.Core.Core.Threading;
using IIASA.FloodCitiSense.Mobile.Core.Localization;
using IIASA.FloodCitiSense.Views.Account;
using System.Threading.Tasks;
using System.Windows.Input;
using IIASA.FloodCitiSense.Mobile.Core.Commands;
using IIASA.FloodCitiSense.Mobile.Core.Services.Navigation;
using IIASA.FloodCitiSense.Views;
using Xamarin.Forms;

namespace IIASA.FloodCitiSense.ViewModels
{
    public class ForgotPasswordViewModel : XamarinViewModel
    {
        public ICommand SendForgotPasswordCommand => AsyncCommand.Create(SendForgotPasswordAsync);

        public ICommand NavigateToLoginCommand => HttpRequestCommand.Create(NavigateToLoginAsync);

        private readonly IAccountAppService _accountAppService;
        private readonly INavigationService _navigationService;
        private bool _isForgotPasswordEnabled;

        public ForgotPasswordViewModel(IAccountAppService accountAppService, INavigationService navigationService)
        {
            _accountAppService = accountAppService;
            _navigationService = navigationService;
        }

        private string _emailAddress;
        public string EmailAddress
        {
            get => _emailAddress;
            set
            {
                _emailAddress = value;
                SetEmailActivationButtonEnabled();
                RaisePropertyChanged(() => EmailAddress);
            }
        }

        public bool IsForgotPasswordEnabled
        {
            get => _isForgotPasswordEnabled;
            set
            {
                _isForgotPasswordEnabled = value;
                RaisePropertyChanged(() => IsForgotPasswordEnabled);
            }
        }

        public void SetEmailActivationButtonEnabled()
        {
            IsForgotPasswordEnabled = !string.IsNullOrWhiteSpace(EmailAddress);
        }

        private async Task NavigateToLoginAsync()
        {
            await _navigationService.SetMainPage<MasterPage>(clearNavigationHistory: true);
            await _navigationService.SetDetailPageAsync(typeof(LoginView));
        }

        private async Task SendForgotPasswordAsync()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequestExecuter.Execute(
                    async () =>
                    await _accountAppService.SendPasswordResetCode(new SendPasswordResetCodeInput { EmailAddress = EmailAddress }),
                    PasswordResetMailSentAsync
                );
            });
        }

        private async Task PasswordResetMailSentAsync()
        {
            await UserDialogs.Instance.AlertAsync(L.Localize("PasswordResetMailSentMessage"), L.Localize("MailSent"), L.Localize("Ok"));

            await NavigationService.SetMainPage<LoginView>(clearNavigationHistory: true);
        }
    }
}
