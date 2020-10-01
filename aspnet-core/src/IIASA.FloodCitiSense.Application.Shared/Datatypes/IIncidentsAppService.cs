using Abp.Application.Services;
using Abp.Application.Services.Dto;
using IIASA.FloodCitiSense.Datatypes.Dtos;
using System.Threading.Tasks;

namespace IIASA.FloodCitiSense.Datatypes
{
    public interface IIncidentsAppService : IApplicationService
    {
        Task<PagedResultDto<GetAllIncidentsOutput>> GetAll(GetAllIncidentsInput input);

        Task<GetIncidentForView> GetIncidentById(EntityDto input);
        Task<PagedResultDto<GetAllIncidentsOutput>> GetIncidentByUserId(EntityDto input);

        Task Delete(EntityDto input);
        Task<int> DeleteTempData();

        Task<OutputDto> Create(IncidentViewModel input);

        Task<OutputDto> Update(IncidentViewModel input);
    }
}