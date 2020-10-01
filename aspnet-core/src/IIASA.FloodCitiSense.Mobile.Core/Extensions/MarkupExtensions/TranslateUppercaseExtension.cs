using IIASA.FloodCitiSense.Mobile.Core.Core;
using IIASA.FloodCitiSense.Mobile.Core.Localization;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IIASA.FloodCitiSense.Mobile.Core.Extensions.MarkupExtensions
{
    [ContentProperty("Text")]
    public class TranslateUppercaseExtension : IMarkupExtension
    {
        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (ApplicationBootstrapper.AbpBootstrapper == null || this.Text == null)
            {
                return this.Text;
            }

            return L.Localize(this.Text).ToUpper();
        }
    }
}