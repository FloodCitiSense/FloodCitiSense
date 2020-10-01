using Abp.Application.Services.Dto;
using IIASA.FloodCitiSense.Editions.Dto;
using System.Collections.Generic;

namespace IIASA.FloodCitiSense.MultiTenancy.Dto
{
    public class GetTenantFeaturesEditOutput
    {
        public List<NameValueDto> FeatureValues { get; set; }

        public List<FlatFeatureDto> Features { get; set; }
    }
}