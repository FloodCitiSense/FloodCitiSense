using IIASA.FloodCitiSense.Dto;

namespace IIASA.FloodCitiSense.Common.Dto
{
    public class FindUsersInput : PagedAndFilteredInputDto
    {
        public int? TenantId { get; set; }
    }
}