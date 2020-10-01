using Abp.Application.Services;
using IIASA.FloodCitiSense.Sessions.Dto;
using System.Threading.Tasks;

namespace IIASA.FloodCitiSense.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();
    }
}
