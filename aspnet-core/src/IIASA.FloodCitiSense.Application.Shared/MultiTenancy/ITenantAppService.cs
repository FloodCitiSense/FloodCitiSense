using Abp.Application.Services;
using Abp.Application.Services.Dto;
using IIASA.FloodCitiSense.MultiTenancy.Dto;
using System.Threading.Tasks;

namespace IIASA.FloodCitiSense.MultiTenancy
{
    using System.Collections.Generic;

    public interface ITenantAppService : IApplicationService
    {
        Task<PagedResultDto<TenantListDto>> GetTenants(GetTenantsInput input);

        Task<List<AllTenantDto>> GetAllTenants();

        Task CreateTenant(CreateTenantInput input);

        Task<TenantEditDto> GetTenantForEdit(EntityDto input);

        Task UpdateTenant(TenantEditDto input);

        Task DeleteTenant(EntityDto input);

        Task<GetTenantFeaturesEditOutput> GetTenantFeaturesForEdit(EntityDto input);

        Task UpdateTenantFeatures(UpdateTenantFeaturesInput input);

        Task ResetTenantSpecificFeatures(EntityDto input);

        Task UnlockTenantAdmin(EntityDto input);
    }
}
