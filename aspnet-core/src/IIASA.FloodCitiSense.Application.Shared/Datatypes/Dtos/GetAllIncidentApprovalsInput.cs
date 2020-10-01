using Abp.Application.Services.Dto;

namespace IIASA.FloodCitiSense.Datatypes.Dtos
{
    public class GetAllIncidentApprovalsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}