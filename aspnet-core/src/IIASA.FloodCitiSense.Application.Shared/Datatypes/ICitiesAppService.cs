using Abp.Application.Services;
using Abp.Application.Services.Dto;
using IIASA.FloodCitiSense.DataTypes.Dtos;
using IIASA.FloodCitiSense.Dto;
using System.Threading.Tasks;

namespace IIASA.FloodCitiSense.DataTypes
{
    public interface ICitiesAppService : IApplicationService
    {
        Task<PagedResultDto<GetCityForView>> GetAll(GetAllCitiesInput input);

        Task<GetCityForEditOutput> GetCityForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditCityDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetCitiesToExcel(GetAllCitiesForExcelInput input);


        Task<PagedResultDto<UserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);

    }
}