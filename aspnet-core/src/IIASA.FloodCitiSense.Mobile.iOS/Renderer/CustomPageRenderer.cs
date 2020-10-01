using IIASA.FloodCitiSense.Renderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ContentPage), typeof(CustomPageRenderer))]

namespace IIASA.FloodCitiSense.Renderer
{
    public class CustomPageRenderer : PageRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
            {
                OverrideUserInterfaceStyle = UIUserInterfaceStyle.Light;
            }
        }
    }
}