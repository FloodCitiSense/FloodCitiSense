using Abp.Application.Services;
using Abp.Application.Services.Dto;
using IIASA.FloodCitiSense.Datatypes.Dtos;
using IIASA.FloodCitiSense.Dto;
using System.Threading.Tasks;

namespace IIASA.FloodCitiSense.Datatypes
{
    public interface ISensorsAppService : IApplicationService
    {
        Task<PagedResultDto<GetSensorForView>> GetAll(GetAllSensorsInput input);
        Task<PagedResultDto<GetSensorForView>> GetSensorByUserId(EntityDto<long> input);

        Task<GetSensorForEditOutput> GetSensorForEdit(EntityDto input);
        Task<SensorDto> GetSensorById(EntityDto input);

        Task<SensorDto> CreateOrEdit(CreateOrEditSensorDto input);

        Task<SensorDto> Create(CreateOrEditSensorDto input);

        Task<SensorDto> Update(CreateOrEditSensorDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetSensorsToExcel(GetAllSensorsForExcelInput input);


    }
}