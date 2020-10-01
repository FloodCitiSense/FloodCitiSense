using System;
using System.Globalization;
using System.IO;
using IIASA.FloodCitiSense.Mobile.Core.Extensions;
using IIASA.FloodCitiSense.Mobile.Core.UI.Assets;
using Xamarin.Forms;

namespace IIASA.FloodCitiSense.Mobile.Core.Converters
{
    public class LocalImageToPathConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(ImageSource))
            {
                throw new InvalidOperationException("The target must be a LocalImage");
            }
            if (!(value is string localImage)) return string.Empty;
            var isUrl = localImage.CheckUrlValid();
            var ext = Path.GetExtension(localImage);
            return ext != ".jpg" ? ImageSource.FromResource("IIASA.FloodCitiSense.Mobile.Core.UI.Assets.Images.film.png", typeof(AssetsHelper)) : isUrl ? ImageSource.FromUri(new Uri(localImage)) : ImageSource.FromFile(localImage);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}