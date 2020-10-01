using IIASA.FloodCitiSense.ApiClient.Models;
using System.Threading.Tasks;

namespace IIASA.FloodCitiSense.Mobile.Core.Services.Account
{
    public interface IAccountService
    {
        AbpAuthenticateModel AbpAuthenticateModel { get; set; }
        AbpAuthenticateResultModel AuthenticateResultModel { get; set; }
        Task LoginUserAsync();
    }
}
