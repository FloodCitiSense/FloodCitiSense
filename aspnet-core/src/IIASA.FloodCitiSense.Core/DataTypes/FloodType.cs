using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using JetBrains.Annotations;

namespace IIASA.FloodCitiSense.Datatypes
{
    [Table("FloodTypes")]
    public class FloodType : FullAuditedEntity, IMayHaveTenant, IExtendableObject
    {
        public int? TenantId { get; set; }
        public TypeOfFlood TypeOfFlood { get; set; }
        [CanBeNull] public Incident Incident { get; set; }
        public string ExtensionData { get; set; }
    }
}