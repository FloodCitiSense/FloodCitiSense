using System.Threading.Tasks;
using Abp.Application.Services;
using IIASA.FloodCitiSense.Configuration.Dto;

namespace IIASA.FloodCitiSense.Configuration
{
    public interface IUiCustomizationSettingsAppService : IApplicationService
    {
        Task<UiCustomizationSettingsEditDto> GetUiManagementSettings();

        Task UpdateUiManagementSettings(UiCustomizationSettingsEditDto settings);

        Task UpdateDefaultUiManagementSettings(UiCustomizationSettingsEditDto settings);

        Task UseSystemDefaultSettings();
    }
}
