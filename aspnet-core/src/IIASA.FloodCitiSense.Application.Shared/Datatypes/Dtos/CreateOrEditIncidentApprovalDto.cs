
using Abp.Application.Services.Dto;
using System;

namespace IIASA.FloodCitiSense.Datatypes.Dtos
{
    public class CreateOrEditIncidentApprovalDto : AuditedEntityDto<int?>
    {

        public bool Approved { get; set; }

        public DateTime TimeApproved { get; set; }

        public int ReviewerUserID { get; set; }

        public string Comment { get; set; }


    }
}