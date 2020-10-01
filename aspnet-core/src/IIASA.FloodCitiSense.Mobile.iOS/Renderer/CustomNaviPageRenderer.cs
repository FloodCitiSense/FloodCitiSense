using IIASA.FloodCitiSense.Renderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(CustomNaviPageRenderer))]

namespace IIASA.FloodCitiSense.Renderer
{
    public class CustomNaviPageRenderer : NavigationRenderer
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