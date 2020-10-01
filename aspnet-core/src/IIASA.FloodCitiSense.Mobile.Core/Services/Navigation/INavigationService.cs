using IIASA.FloodCitiSense.Mobile.Core.Interface;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IIASA.FloodCitiSense.Mobile.Core.Services.Navigation
{
    public interface INavigationService
    {
        Task SetMainPage<TView>(object navigationParameter = null, bool clearNavigationHistory = false)
            where TView : IXamarinView;

        Task NavigateToPage(string page);

        Task SetDetailPageAsync(Type viewType, object navigationParameter = null, bool pushToStack = false);

        Task<Page> GoBackAsync();
    }
}