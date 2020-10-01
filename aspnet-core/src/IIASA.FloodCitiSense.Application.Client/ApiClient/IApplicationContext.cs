using Abp.Localization;
using Abp.Web.Models.AbpUserConfiguration;
using IIASA.FloodCitiSense.Sessions.Dto;
using JetBrains.Annotations;

namespace IIASA.FloodCitiSense.ApiClient
{
    public interface IApplicationContext
    {
        [CanBeNull]
        TenantInformation CurrentTenant { get; }

        AbpUserConfigurationDto Configuration { get; set; }

        GetCurrentLoginInformationsOutput LoginInfo { get; set; }

        void SetAsHost();

        void SetAsTenant(string tenancyName, int tenantId);

        LanguageInfo CurrentLanguage { get; set; }

        [CanBeNull]
        AppInformation AppInformation { get; set; }
    }
}