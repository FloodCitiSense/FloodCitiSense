using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using JetBrains.Annotations;

namespace IIASA.FloodCitiSense.Datatypes
{
    [Table("Pictures")]
    public class Picture : FullAuditedEntity, IMayHaveTenant, IExtendableObject
    {
        [CanBeNull] public Incident Incident { get; set; }
        [CanBeNull] public Sensor Sensor { get; set; }
        public virtual string Url { get; set; }

        public virtual string Base64 { get; set; }
        public virtual string PhysicalPath { get; set; }

        public virtual Guid MobileDataId { get; set; }
        public string ExtensionData { get; set; }

        public int? TenantId { get; set; }
    }
}