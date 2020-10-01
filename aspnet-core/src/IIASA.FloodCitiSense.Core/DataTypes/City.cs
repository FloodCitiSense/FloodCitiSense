using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using IIASA.FloodCitiSense.Authorization.Users;

namespace IIASA.FloodCitiSense.DataTypes
{
    [Table("Cities")]
    public class City : FullAuditedEntity, IExtendableObject
    {
        public virtual string Name { get; set; }
        public virtual string Country { get; set; }
        public virtual string Code { get; set; }
        public string ExtensionData { get; set; }
    }
}