using Abp.Application.Services.Dto;

namespace IIASA.FloodCitiSense.Playground.Dtos
{
    public class GetAllCreativeEntiiesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}