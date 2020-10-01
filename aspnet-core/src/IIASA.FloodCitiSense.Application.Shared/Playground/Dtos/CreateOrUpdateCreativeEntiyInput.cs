using Abp.Application.Services.Dto;

namespace IIASA.FloodCitiSense.Playground.Dtos
{
    public class CreateOrEditCreativeEntiyDto : AuditedEntityDto<int?>
    {
        public string name { get; set; }

        public string comment { get; set; }

        public double lat { get; set; }

        public double lon { get; set; }
    }
}