using IIASA.FloodCitiSense.Mobile.Core.UI.Assets;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace IIASA.FloodCitiSense.Mobile.Core.Converters
{
    public class ImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var resource = AssetsHelper.GetImageNamespace(value.ToString());
            return ImageSource.FromResource(resource);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}


