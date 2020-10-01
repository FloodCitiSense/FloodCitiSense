using IIASA.FloodCitiSense.Mobile.Core.Interface;
using IIASA.FloodCitiSense.Mobile.Core.Models.Tenants;
using IIASA.FloodCitiSense.ViewModels;
using Xamarin.Forms;

namespace IIASA.FloodCitiSense.Views
{
    public partial class TenantsView : ContentPage, IXamarinView
    {
        public TenantsView()
        {
            InitializeComponent();
        }

        private async void ListView_OnItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            await ((TenantsViewModel)BindingContext).LoadMoreTenantsIfNeedsAsync(e.Item as TenantListModel);
        }
    }
}