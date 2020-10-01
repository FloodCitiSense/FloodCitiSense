using Abp.Application.Services;
using IIASA.FloodCitiSense.Dto;
using IIASA.FloodCitiSense.Logging.Dto;

namespace IIASA.FloodCitiSense.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
