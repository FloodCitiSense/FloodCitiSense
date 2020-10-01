using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using IIASA.FloodCitiSense.Datatypes.Dtos;

namespace IIASA.FloodCitiSense.Datatypes
{
    public interface IMobilePushNotificationService : IApplicationService
    {
        Task<OutputDto> Send(MobilePushNotificationViewModel input);

        Task<PagedResultDto<MobilePushNotificationViewModel>> GetByUserId(EntityDto input);
    }
}