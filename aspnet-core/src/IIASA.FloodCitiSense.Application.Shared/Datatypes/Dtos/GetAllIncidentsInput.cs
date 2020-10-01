using Abp.Application.Services.Dto;

namespace IIASA.FloodCitiSense.Datatypes.Dtos
{
    public class GetAllIncidentsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}