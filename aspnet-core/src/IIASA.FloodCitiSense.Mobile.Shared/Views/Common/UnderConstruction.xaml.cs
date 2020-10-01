using IIASA.FloodCitiSense.Mobile.Core.Interface;
using Xamanimation;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace IIASA.FloodCitiSense.Views.Common
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UnderConstruction : ContentPage, IXamarinView, IAnimatedView
    {
        public UnderConstruction()
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
            StartAnimation();
        }

        public void StartAnimation()
        {
            if (Resources["BackgroundColorAnimation"] is ColorAnimation backgroundColorAnimation)
            {
                backgroundColorAnimation.Begin();
            }

            if (Resources["InfoPanelAnimation"] is StoryBoard animation)
            {
                animation.Begin();
            }
        }
    }
}