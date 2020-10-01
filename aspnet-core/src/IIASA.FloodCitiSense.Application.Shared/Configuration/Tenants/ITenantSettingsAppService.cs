using Abp.Application.Services;
using IIASA.FloodCitiSense.Configuration.Tenants.Dto;
using System.Threading.Tasks;

namespace IIASA.FloodCitiSense.Configuration.Tenants
{
    public interface ITenantSettingsAppService : IApplicationService
    {
        Task<TenantSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(TenantSettingsEditDto input);

        Task ClearLogo();

        Task ClearCustomCss();
    }
}
