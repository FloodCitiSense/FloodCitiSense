using System.Threading.Tasks;
using Abp.Application.Services;
using IIASA.FloodCitiSense.Install.Dto;

namespace IIASA.FloodCitiSense.Install
{
    public interface IInstallAppService : IApplicationService
    {
        Task Setup(InstallDto input);

        AppSettingsJsonDto GetAppSettingsJson();

        CheckDatabaseOutput CheckDatabase();
    }
}