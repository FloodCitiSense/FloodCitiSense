using IIASA.FloodCitiSense.Mobile.Core.Base;

namespace IIASA.FloodCitiSense.ViewModels.Init
{
    using System.Threading.Tasks;
    using System.Windows.Input;
    using IIASA.FloodCitiSense.Mobile.Core.Extensions;
    using IIASA.FloodCitiSense.Mobile.Core.Localization;
    using IIASA.FloodCitiSense.Mobile.Core.UI.Assets;

    using Xamarin.Forms;

    public class PrivacyPageModel : XamarinViewModel
    {
        private readonly ILocale _locale;

        public Command PageAppearingCommand { get; set; }

        private async Task PageAppearingAsync()
        {
            var fileName = AssetsHelper.GetFileNamespace("PrivacyText-en.txt");
            var culture = this._locale.GetCurrentCultureInfo().TwoLetterISOLanguageName;
            // var dict = new Dictionary<string, string> {
            //                                                   {"en", "PrivacyText-en.txt"},
            //                                                   {"es", "PrivacyText-es.txt"}
            //                                               };
            // if (dict.ContainsKey(culture))
            // {
            //     fileName = dict[culture];
            // }
            this.HtmlWebViewSource = new HtmlWebViewSource { Html = await fileName.ReadTextAsync() };
        }
        private HtmlWebViewSource _htmlWebViewSource;
        public HtmlWebViewSource HtmlWebViewSource
        {
            get => this._htmlWebViewSource;
            set
            {
                this._htmlWebViewSource = value;
                RaisePropertyChanged(() => this.HtmlWebViewSource);
            }
        }

        public PrivacyPageModel(ILocale locale)
        {
            this._locale = locale;
            this.PageAppearingCommand = new Command(async () => await PageAppearingAsync());
        }
    }
}