using Abp.Application.Services;
using IIASA.FloodCitiSense.Editions.Dto;
using IIASA.FloodCitiSense.MultiTenancy.Dto;
using System.Threading.Tasks;

namespace IIASA.FloodCitiSense.MultiTenancy
{
    public interface ITenantRegistrationAppService : IApplicationService
    {
        Task<RegisterTenantOutput> RegisterTenant(RegisterTenantInput input);

        Task<EditionsSelectOutput> GetEditionsForSelect();

        Task<EditionSelectDto> GetEdition(int editionId);
    }
}