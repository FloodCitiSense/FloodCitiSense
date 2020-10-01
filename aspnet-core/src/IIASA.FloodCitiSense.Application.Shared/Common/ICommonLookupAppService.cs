using Abp.Application.Services;
using Abp.Application.Services.Dto;
using IIASA.FloodCitiSense.Common.Dto;
using IIASA.FloodCitiSense.Editions.Dto;
using System.Threading.Tasks;

namespace IIASA.FloodCitiSense.Common
{
    public interface ICommonLookupAppService : IApplicationService
    {
        Task<ListResultDto<SubscribableEditionComboboxItemDto>> GetEditionsForCombobox(bool onlyFreeItems = false);

        Task<PagedResultDto<NameValueDto>> FindUsers(FindUsersInput input);

        GetDefaultEditionNameOutput GetDefaultEditionName();
    }
}