//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="PrepareModel.cs" company="IIASA">
//    EOS
//  </copyright>
//  <summary>
//   PrepareModel.cs
//  </summary>
//  --------------------------------------------------------------------------------------------------------------------

using IIASA.FloodCitiSense.Mobile.Core.Base;
using IIASA.FloodCitiSense.Mobile.Core.Core.Threading;
using IIASA.FloodCitiSense.Mobile.Core.Services.Navigation;
using IIASA.FloodCitiSense.Views.Prepare;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IIASA.FloodCitiSense.ViewModels.Prepare
{
    public class PrepareModel : XamarinViewModel
    {
        private readonly INavigationService _navigationService;
        public ICommand PageAppearingCommand => AsyncCommand.Create(PageAppearing);
        public ICommand GeneralAsyncCommand => AsyncCommand.Create(General);

        public ICommand CityAsyncCommand => AsyncCommand.Create(City);

        private async Task City()
        {
            await _navigationService.SetMainPage<PrepareCity>();
        }

        private async Task General()
        {
            await _navigationService.SetMainPage<PrepareGeneral>();
        }

        private async Task PageAppearing()
        {

        }

        public PrepareModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
    }
}