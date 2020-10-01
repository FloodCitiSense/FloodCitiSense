using Abp.Application.Services;
using Abp.Application.Services.Dto;
using IIASA.FloodCitiSense.Authorization.Users.Dto;
using System.Threading.Tasks;

namespace IIASA.FloodCitiSense.Authorization.Users
{
    public interface IUserLoginAppService : IApplicationService
    {
        Task<ListResultDto<UserLoginAttemptDto>> GetRecentUserLoginAttempts();
    }
}
