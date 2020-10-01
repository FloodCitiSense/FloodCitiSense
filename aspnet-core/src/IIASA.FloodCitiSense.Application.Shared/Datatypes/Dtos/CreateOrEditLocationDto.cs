using Abp.Application.Services.Dto;

namespace IIASA.FloodCitiSense.Datatypes.Dtos
{
    public class CreateOrEditLocationDto : AuditedEntityDto<int?>
    {

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double Accuracy { get; set; }

        public int TenantID { get; set; }


    }
}