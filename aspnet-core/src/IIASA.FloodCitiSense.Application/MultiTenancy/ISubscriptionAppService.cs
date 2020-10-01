using System.Threading.Tasks;
using Abp.Application.Services;

namespace IIASA.FloodCitiSense.MultiTenancy
{
    public interface ISubscriptionAppService : IApplicationService
    {
        Task UpgradeTenantToEquivalentEdition(int upgradeEditionId);
    }
}
