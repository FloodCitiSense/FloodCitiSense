using Abp;
using Abp.Dependency;
using Abp.Localization;
using Abp.Web.Models.AbpUserConfiguration;
using IIASA.FloodCitiSense.Sessions.Dto;
using JetBrains.Annotations;

namespace IIASA.FloodCitiSense.ApiClient
{
    public class ApplicationContext : IApplicationContext, ISingletonDependency
    {
        public TenantInformation CurrentTenant { get; private set; }

        public AbpUserConfigurationDto Configuration { get; set; }

        public GetCurrentLoginInformationsOutput LoginInfo { get; set; }

        public void SetAsTenant([NotNull] string tenancyName, int tenantId)
        {
            Check.NotNull(tenancyName, nameof(tenancyName));

            CurrentTenant = new TenantInformation(tenancyName, tenantId);
        }

        public LanguageInfo CurrentLanguage { get; set; }
        public AppInformation AppInformation { get; set; }

        public void SetAsHost()
        {
            CurrentTenant = null;
        }
    }
}