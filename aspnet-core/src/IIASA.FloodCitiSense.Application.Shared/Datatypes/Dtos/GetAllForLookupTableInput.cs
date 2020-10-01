using Abp.Application.Services.Dto;

namespace IIASA.FloodCitiSense.DataTypes.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}