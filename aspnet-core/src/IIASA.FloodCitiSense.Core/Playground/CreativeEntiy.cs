using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace IIASA.FloodCitiSense.Playground
{
	[Table("creative_entity")]
    public class CreativeEntiy : AuditedEntity , IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string name { get; set; }

        public virtual string comment { get; set; }

        public virtual double lat { get; set; }

        public virtual double lon { get; set; }
    }
}