using Abp.Application.Services.Dto;
using System;

namespace IIASA.FloodCitiSense.Organizations.Dto
{
    public class OrganizationUnitUserListDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }

        public Guid? ProfilePictureId { get; set; }

        public DateTime AddedTime { get; set; }
    }
}