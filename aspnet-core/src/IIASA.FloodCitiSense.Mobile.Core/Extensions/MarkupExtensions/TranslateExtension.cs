using IIASA.FloodCitiSense.Mobile.Core.Core;
using IIASA.FloodCitiSense.Mobile.Core.Localization;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IIASA.FloodCitiSense.Mobile.Core.Extensions.MarkupExtensions
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (ApplicationBootstrapper.AbpBootstrapper == null || Text == null)
            {
                return Text;
            }

            return L.Localize(Text);
        }
    }
}