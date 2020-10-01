using Abp.Application.Services.Dto;

namespace IIASA.FloodCitiSense.Datatypes.Dtos
{
    public class GetAllLocationsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}