using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IIASA.FloodCitiSense.Mobile.Core.Extensions.MarkupExtensions
{
    [ContentProperty(nameof(Source))]
    public class ImageSourceExtension : IMarkupExtension
    {
        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (!string.IsNullOrEmpty(Source))
            {
                var source = $"IIASA.FloodCitiSense.Mobile.Core.UI.Assets.Images.{Source}";

                if (!string.IsNullOrEmpty(Source) && Source.Contains("IIASA.FloodCitiSense.Mobile.Core.UI.Assets.Images"))
                {
                    source = Source;
                }

                return ImageSource.FromResource(source);
            }

            return null;
        }
    }
}
