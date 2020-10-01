using Abp.Application.Services;
using Abp.Application.Services.Dto;
using IIASA.FloodCitiSense.Timing.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IIASA.FloodCitiSense.Timing
{
    public interface ITimingAppService : IApplicationService
    {
        Task<ListResultDto<NameValueDto>> GetTimezones(GetTimezonesInput input);

        Task<List<ComboboxItemDto>> GetTimezoneComboboxItems(GetTimezoneComboboxItemsInput input);
    }
}
