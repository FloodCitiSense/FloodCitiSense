using IIASA.FloodCitiSense.Mobile.Core.Interface;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IIASA.FloodCitiSense.Mobile.Core.Services.Modal
{
    public interface IModalService
    {
        Task ShowModalAsync(Page page);

        Task ShowModalAsync<TView>(object navigationParameter) where TView : IXamarinView;

        Task<Page> CloseModalAsync();
    }
}
