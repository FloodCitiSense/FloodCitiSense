using IIASA.FloodCitiSense.Dto;

namespace IIASA.FloodCitiSense.Organizations.Dto
{
    public class FindOrganizationUnitUsersInput : PagedAndFilteredInputDto
    {
        public long OrganizationUnitId { get; set; }
    }
}
