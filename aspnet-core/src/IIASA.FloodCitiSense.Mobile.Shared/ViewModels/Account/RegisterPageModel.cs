using Acr.UserDialogs;
using IIASA.FloodCitiSense.Authorization.Accounts;
using IIASA.FloodCitiSense.Authorization.Accounts.Dto;
using IIASA.FloodCitiSense.Datatypes;
using IIASA.FloodCitiSense.Mobile.Core.Base;
using IIASA.FloodCitiSense.Mobile.Core.Commands;
using IIASA.FloodCitiSense.Mobile.Core.Extensions;
using IIASA.FloodCitiSense.Mobile.Core.Extensions.MarkupExtensions;
using IIASA.FloodCitiSense.Mobile.Core.Services.Navigation;
using IIASA.FloodCitiSense.Mobile.Core.UI;
using IIASA.FloodCitiSense.Mobile.Core.Validations;
using IIASA.FloodCitiSense.Views.Account;
using Plugin.InputKit.Shared.Controls;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Windows.Input;
using IIASA.FloodCitiSense.Views;
using Xamarin.Forms;

namespace IIASA.FloodCitiSense.ViewModels.Account
{
    class RegisterPageModel : XamarinViewModel
    {
        private readonly IAccountAppService _accountAppService;
        private readonly INavigationService _navigationService;

        public ICommand RegisterUserCommand => HttpRequestCommand.Create(RegisterUserAsync);
        public ICommand NavigateToLoginCommand => HttpRequestCommand.Create(NavigateToLoginAsync);
        public ICommand ExperienceSelectedCommand => new Command(ChangeExperienceLevel);

        private void ChangeExperienceLevel(object o)
        {
            var radio = (RadioButtonGroupView)o;
            if (radio != null)
            {
                ExperienceLevel = EnumHelper<ExperienceLevel>.Parse(radio.SelectedItem?.ToString());
            }
        }


        private async Task NavigateToLoginAsync()
        {
            await SetLoginPage();
        }

        private async Task RegisterUserAsync()
        {
            var results = DataAnnotationsValidator.Validate(this);

            if (results.IsValid)
            {
                await SetBusyAsync(async () =>
                {
                    if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password) && ExperienceLevel != null)
                    {
                        var registerInput = new RegisterInput
                        {
                            UserName = Username.Trim(),
                            EmailAddress = Email.Trim(),
                            Password = Password,
                            Name = Username.Trim(),
                            Surname = Username.Trim(),
                            ExperienceLevel = ExperienceLevel.Value
                        };
                        var result = await _accountAppService.Register(registerInput);

                        if (result.CanLogin)
                        {
                            await UserDialogs.Instance.AlertAsync(result.Message, "RegistrationSuccess".Translate());
                            await SetLoginPage();
                        }
                        else
                        {
                            await UserDialogs.Instance.AlertAsync(result.Message, "RegistrationError".Translate());
                        }
                    }
                }, "RegisterUser".Translate());
            }
            else
            {
                UserDialogHelper.Error(results.ConsolidatedMessage);
            }
        }

        private async Task SetLoginPage()
        {
            await _navigationService.SetMainPage<MasterPage>(clearNavigationHistory: true);
            await _navigationService.SetDetailPageAsync(typeof(LoginView));
        }


        public RegisterPageModel(IAccountAppService accountAppService, INavigationService navigationService)
        {
            _accountAppService = accountAppService;
            _navigationService = navigationService;
        }

        private string _username;
        [Required]
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                RaisePropertyChanged(() => Username);
            }
        }

        private string _password;
        [Required]
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                RaisePropertyChanged(() => Password);
            }
        }

        private string _email;
        [Required]
        [EmailAddress]
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                RaisePropertyChanged(() => Email);
            }
        }

        [Required]
        public ExperienceLevel? ExperienceLevel { get; set; }

        //private ExperienceLevel _experienceLevel;
        //[Required]
        //public ExperienceLevel? ExperienceLevel
        //{
        //    get => _experienceLevel;
        //    set
        //    {
        //        _experienceLevel = value ?? Datatypes.ExperienceLevel.None;
        //        RaisePropertyChanged(() => ExperienceLevel);
        //    }
        //}
    }
}
