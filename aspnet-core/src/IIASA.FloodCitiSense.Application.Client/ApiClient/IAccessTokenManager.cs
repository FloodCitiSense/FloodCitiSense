using IIASA.FloodCitiSense.ApiClient.Models;
using System.Threading.Tasks;

namespace IIASA.FloodCitiSense.ApiClient
{
    public interface IAccessTokenManager
    {
        //Task<string> GetAccessTokenAsync();

        Task<AbpAuthenticateResultModel> LoginAsync();

        void Logout();

        bool IsUserLoggedIn { get; }
    }
}