
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using System;

namespace IIASA.FloodCitiSense.Datatypes.Dtos
{
    public class PictureDto : FullAuditedEntityDto, IExtendableObject
    {
        public string URL { get; set; }
        public string Base64 { get; set; }
        public string PhysicalPath { get; set; }
        public int TenantID { get; set; }
        public int IncidentId { get; set; }

        public Guid MobileDataId { get; set; }
        public string ExtensionData { get; set; }
    }
}