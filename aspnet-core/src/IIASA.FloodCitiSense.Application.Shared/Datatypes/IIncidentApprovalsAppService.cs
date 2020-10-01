using Abp.Application.Services;
using Abp.Application.Services.Dto;
using IIASA.FloodCitiSense.Datatypes.Dtos;
using System.Threading.Tasks;

namespace IIASA.FloodCitiSense.Datatypes
{
    public interface IIncidentApprovalsAppService : IApplicationService
    {
        Task<PagedResultDto<GetAllIncidentApprovalsOutput>> GetAll(GetAllIncidentApprovalsInput input);

        Task<GetIncidentApprovalForEditOutput> GetIncidentApprovalForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditIncidentApprovalDto input);

        Task Delete(EntityDto input);


    }
}