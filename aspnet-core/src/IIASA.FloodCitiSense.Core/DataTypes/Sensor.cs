using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using IIASA.FloodCitiSense.Authorization.Users;

namespace IIASA.FloodCitiSense.Datatypes
{
    [Table("Sensors")]
    public class Sensor : FullAuditedEntity, IMayHaveTenant, IExtendableObject
    {
        public virtual string Name { get; set; }
        public List<Picture> Pictures { get; set; }
        public List<Location> Locations { get; set; }
        public User User { get; set; }
        public string ExtensionData { get; set; }
        public int? TenantId { get; set; }
    }
}