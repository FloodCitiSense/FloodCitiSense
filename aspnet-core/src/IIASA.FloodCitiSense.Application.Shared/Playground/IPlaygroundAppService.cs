using Abp.Application.Services;
using Abp.Application.Services.Dto;
using IIASA.FloodCitiSense.Playground.Dtos;
using System.Threading.Tasks;

namespace IIASA.FloodCitiSense.Playground
{
    public interface IPlaygroundAppService : IApplicationService
    {
        Task<PagedResultDto<CreativeEntiyDto>> GetAll(GetAllCreativeEntiiesInput input);

        Task<CreateOrEditCreativeEntiyDto> GetCreativeEntiyForEdit(EntityDto<int> input);

        Task CreateOrEdit(CreateOrEditCreativeEntiyDto input);

        Task Delete(EntityDto<int> input);
    }
}