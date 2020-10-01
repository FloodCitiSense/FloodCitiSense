using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IIASA.FloodCitiSense.Mobile.Core.Extensions
{
    public static class ImageSourceExtensions
    {
        public static async Task<Stream> GetSourceStreamAsync(this ImageSource imageSource)
        {
            return await ((StreamImageSource)imageSource).Stream(CancellationToken.None);
        }
    }
}