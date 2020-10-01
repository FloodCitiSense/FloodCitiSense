using Abp.AutoMapper;
using IIASA.FloodCitiSense.Organizations.Dto;

namespace IIASA.FloodCitiSense.Mobile.Core.Models.Users
{
    [AutoMapFrom(typeof(OrganizationUnitDto))]
    public class OrganizationUnitModel : OrganizationUnitDto
    {
        public bool IsAssigned { get; set; }
    }
}