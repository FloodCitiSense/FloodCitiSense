using IIASA.FloodCitiSense.Mobile.Core.Interface;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace IIASA.FloodCitiSense.Views.Account
{
    public partial class LoginView : ContentPage, IXamarinView
    {
        public LoginView()
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
            SetControlFocuses();
        }

        private void SetControlFocuses()
        {
            UsernameEntry.Completed += (s, e) =>
            {
                if (string.IsNullOrEmpty(PasswordEntry.Text))
                {
                    PasswordEntry.Focus();
                }
                else
                {
                    ExecuteLoginCommand();
                }
            };

            PasswordEntry.Completed += (s, e) =>
            {
                ExecuteLoginCommand();
            };
        }

        private void ExecuteLoginCommand()
        {
            if (LoginButton.IsEnabled)
            {
                LoginButton.Command.Execute(null);
            }
        }
    }
}