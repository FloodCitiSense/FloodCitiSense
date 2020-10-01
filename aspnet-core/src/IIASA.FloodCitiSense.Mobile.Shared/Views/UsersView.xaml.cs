using IIASA.FloodCitiSense.Mobile.Core.Interface;
using IIASA.FloodCitiSense.Mobile.Core.Models.Users;
using IIASA.FloodCitiSense.ViewModels;
using Xamarin.Forms;

namespace IIASA.FloodCitiSense.Views
{
    public partial class UsersView : ContentPage, IXamarinView
    {
        public UsersView()
        {
            InitializeComponent();
        }

        public async void ListView_OnItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            await ((UsersViewModel)BindingContext).LoadMoreUserIfNeedsAsync(e.Item as UserListModel);
        }
    }
}