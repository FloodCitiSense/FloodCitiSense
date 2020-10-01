using Abp.Application.Services.Dto;

namespace IIASA.FloodCitiSense.Organizations.Dto
{
    public class OrganizationUnitDto : AuditedEntityDto<long>
    {
        public long? ParentId { get; set; }

        public string Code { get; set; }

        public string DisplayName { get; set; }

        public int MemberCount { get; set; }
    }
}