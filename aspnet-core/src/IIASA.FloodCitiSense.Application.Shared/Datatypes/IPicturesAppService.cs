using Abp.Application.Services;
using Abp.Application.Services.Dto;
using IIASA.FloodCitiSense.Datatypes.Dtos;
using System.Threading.Tasks;


namespace IIASA.FloodCitiSense.Datatypes
{
    public interface IPicturesAppService : IApplicationService
    {
        Task<PagedResultDto<GetAllPicturesOutput>> GetAll(GetAllPicturesInput input);

        Task<GetPictureForEditOutput> GetPictureForEdit(EntityDto<int> input);

        Task<PictureDto> GetPictureById(EntityDto<int> input);

        Task<ListResultDto<PictureDto>> GetPictureByIncidentId(EntityDto<int> input);

        Task CreateOrEdit(CreateOrEditPictureDto input);

        Task Delete(EntityDto<int> input);

        Task<CreatePictureDto> CreatePicture();
    }
}