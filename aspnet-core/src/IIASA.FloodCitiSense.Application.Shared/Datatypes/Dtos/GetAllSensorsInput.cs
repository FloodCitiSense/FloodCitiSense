using Abp.Application.Services.Dto;

namespace IIASA.FloodCitiSense.Datatypes.Dtos
{
    public class GetAllSensorsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }



    }
}