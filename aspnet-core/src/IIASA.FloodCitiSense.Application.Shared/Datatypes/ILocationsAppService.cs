using Abp.Application.Services;
using Abp.Application.Services.Dto;
using IIASA.FloodCitiSense.Datatypes.Dtos;
using System.Threading.Tasks;

namespace IIASA.FloodCitiSense.Datatypes
{
    public interface ILocationsAppService : IApplicationService
    {
        Task<PagedResultDto<GetAllLocationsOutput>> GetAll(GetAllLocationsInput input);

        Task<PagedResultDto<GetAllLocationsOutput>> GetAllIncident(GetAllLocationsInput input);
        Task<PagedResultDto<GetFilterLocationForView>> GetFilteredIncident(FilterInput input);

        Task<PagedResultDto<GetAllLocationsOutput>> GetAllOtherUserIncident(GetAllLocationsInput input);
        Task<PagedResultDto<GetAllLocationsOutput>> GetAllCurrentUserIncident(GetAllLocationsInput input);

        Task<PagedResultDto<GetAllLocationsOutput>> GetAllUser(GetAllLocationsInput input);

        Task<LocationDto> GetLocationById(EntityDto input);

        Task<ListResultDto<LocationDto>> GetLocationByIncidentId(EntityDto input);

        Task<GetLocationForEditOutput> GetLocationForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditLocationDto input);

        Task Delete(EntityDto input);
    }
}