using IIASA.FloodCitiSense.Mobile.Core.Interface;

namespace IIASA.FloodCitiSense.Views
{
    using Xamarin.Forms;

    /// <summary>
    ///     The master page.
    /// </summary>
    public partial class MasterPage : MasterDetailPage, IXamarinView
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MasterPage" /> class.
        /// </summary>
        public MasterPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}