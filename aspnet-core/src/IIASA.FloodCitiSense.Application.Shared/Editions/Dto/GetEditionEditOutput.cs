using Abp.Application.Services.Dto;
using System.Collections.Generic;

namespace IIASA.FloodCitiSense.Editions.Dto
{
    public class GetEditionEditOutput
    {
        public EditionEditDto Edition { get; set; }

        public List<NameValueDto> FeatureValues { get; set; }

        public List<FlatFeatureDto> Features { get; set; }
    }
}