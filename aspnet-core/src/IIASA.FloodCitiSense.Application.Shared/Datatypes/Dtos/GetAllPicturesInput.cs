using Abp.Application.Services.Dto;

namespace IIASA.FloodCitiSense.Datatypes.Dtos
{
    public class GetAllPicturesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}