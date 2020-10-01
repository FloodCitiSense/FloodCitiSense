using Abp.Application.Services;
using Abp.Application.Services.Dto;
using IIASA.FloodCitiSense.Authorization.Permissions.Dto;

namespace IIASA.FloodCitiSense.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}
