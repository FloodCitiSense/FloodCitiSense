using Abp.Application.Services.Dto;

namespace IIASA.FloodCitiSense.Playground.Dtos
{
    public class CreativeEntiyDto : EntityDto
    {
        public string name { get; set; }

        public string comment { get; set; }

        public double lat { get; set; }

        public double lon { get; set; }
    }
}