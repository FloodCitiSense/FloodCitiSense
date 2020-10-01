using Abp.Application.Services.Dto;
using Abp.Domain.Entities;

namespace IIASA.FloodCitiSense.Datatypes.Dtos
{
    public class FloodTypeDto : EntityDto, IMayHaveTenant, IExtendableObject
    {
        public int? TenantId { get; set; }

        public TypeOfFlood TypeOfFlood { get; set; }
        public string ExtensionData { get; set; }
    }
}