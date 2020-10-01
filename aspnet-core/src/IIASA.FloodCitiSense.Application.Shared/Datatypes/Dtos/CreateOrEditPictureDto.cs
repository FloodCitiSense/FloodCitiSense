using Abp.Application.Services.Dto;

namespace IIASA.FloodCitiSense.Datatypes.Dtos
{
    public class CreateOrEditPictureDto : AuditedEntityDto<int?>
    {

        public string URL { get; set; }

        public string Base64 { get; set; }

        public int TenantID { get; set; }


    }
}