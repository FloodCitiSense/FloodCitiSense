using IIASA.FloodCitiSense.Authorization.Accounts.Dto;
using System.Threading.Tasks;

namespace IIASA.FloodCitiSense.Authorization.Accounts
{
    public class ProxyTokenAuthControllerService : ProxyControllerBase
    {
        public async Task SendTwoFactorAuthCode(long userId, string provider)
        {
            await ApiClient
                .PostAsync("api/" + GetEndpoint(nameof(SendTwoFactorAuthCode)), new { UserId = userId, Provider = provider });
        }

        public async Task<ExternalAuthenticateResultModelDto> ExternalAuthenticate(string authProvider, string providerKey, string providerAccessCode)
        {
            return await ApiClient.PostAnonymousAsync<ExternalAuthenticateResultModelDto>("api/" + GetEndpoint(nameof(ExternalAuthenticate)), new { AuthProvider = authProvider, ProviderKey = providerKey, ProviderAccessCode = providerAccessCode });
        }
    }
}
