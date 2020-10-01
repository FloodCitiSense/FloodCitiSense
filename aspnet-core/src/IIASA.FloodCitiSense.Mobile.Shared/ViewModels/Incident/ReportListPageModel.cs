using IIASA.FloodCitiSense.Mobile.Core.Base;
using IIASA.FloodCitiSense.Mobile.Core.Core.Entity;
using IIASA.FloodCitiSense.Mobile.Core.Services.Navigation;
using IIASA.FloodCitiSense.Views.Incident;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace IIASA.FloodCitiSense.ViewModels.Incident
{
    public class ReportListPageModel : XamarinViewModel
    {
        private readonly INavigationService _navigationService;

        public ReportListPageModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public ICommand SelectedCommand => new Command<LocalIncident>(async r => await this.SelectAsync(r));


        private async Task SelectAsync(LocalIncident obj)
        {
            if (obj != null)
            {
                await SetBusyAsync(() =>
                {
                    _navigationService.SetMainPage<IncidentMainPage>(obj);
                    return null;
                });
            }
        }
    }
}