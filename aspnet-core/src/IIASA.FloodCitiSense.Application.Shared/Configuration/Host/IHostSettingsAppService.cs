using Abp.Application.Services;
using IIASA.FloodCitiSense.Configuration.Host.Dto;
using System.Threading.Tasks;

namespace IIASA.FloodCitiSense.Configuration.Host
{
    public interface IHostSettingsAppService : IApplicationService
    {
        Task<HostSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(HostSettingsEditDto input);

        Task SendTestEmail(SendTestEmailInput input);
    }
}
