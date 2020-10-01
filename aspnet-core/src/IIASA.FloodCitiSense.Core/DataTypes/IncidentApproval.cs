using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace IIASA.FloodCitiSense.Datatypes
{
    [Table("IncidentApprovals")]
    public class IncidentApproval : AuditedEntity, IMayHaveTenant
    {
        public virtual bool Approved { get; set; }

        public virtual DateTime TimeApproved { get; set; }

        public virtual int ReviewerUserID { get; set; }

        public virtual string Comment { get; set; }
        public int? TenantId { get; set; }
    }
}