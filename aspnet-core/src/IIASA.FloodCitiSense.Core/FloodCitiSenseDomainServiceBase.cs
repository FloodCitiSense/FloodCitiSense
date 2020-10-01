using Abp.Domain.Services;

namespace IIASA.FloodCitiSense
{
    public abstract class FloodCitiSenseDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected FloodCitiSenseDomainServiceBase()
        {
            LocalizationSourceName = FloodCitiSenseConsts.LocalizationSourceName;
        }
    }
}
