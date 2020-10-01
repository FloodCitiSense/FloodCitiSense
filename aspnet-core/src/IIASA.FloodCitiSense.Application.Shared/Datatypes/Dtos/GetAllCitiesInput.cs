using Abp.Application.Services.Dto;

namespace IIASA.FloodCitiSense.DataTypes.Dtos
{
    public class GetAllCitiesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }


        public string UserNameFilter { get; set; }


    }
}