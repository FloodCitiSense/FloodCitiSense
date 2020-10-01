//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="RespondModel.cs" company="IIASA">
//    EOS
//  </copyright>
//  <summary>
//   RespondModel.cs
//  </summary>
//  --------------------------------------------------------------------------------------------------------------------

using IIASA.FloodCitiSense.Mobile.Core.Base;
using IIASA.FloodCitiSense.Mobile.Core.Core.Threading;
using IIASA.FloodCitiSense.Mobile.Core.Services.Navigation;
using IIASA.FloodCitiSense.Views.Respond;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IIASA.FloodCitiSense.ViewModels.Respond
{
    public class RespondModel : XamarinViewModel
    {

        private readonly INavigationService _navigationService;
        public ICommand PageAppearingCommand => AsyncCommand.Create(PageAppearing);
        public ICommand GeneralAsyncCommand => AsyncCommand.Create(General);

        public ICommand CityAsyncCommand => AsyncCommand.Create(City);

        private async Task City()
        {
            await _navigationService.SetMainPage<RespondCity>();
        }

        private async Task General()
        {
            await _navigationService.SetMainPage<RespondGeneral>();
        }

        private async Task PageAppearing()
        {

        }

        public RespondModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
    }
}