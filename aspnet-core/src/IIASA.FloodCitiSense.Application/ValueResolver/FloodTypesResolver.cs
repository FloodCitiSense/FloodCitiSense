using System;
using System.Collections.Generic;
using System.Linq;
using Abp.ObjectMapping;
using AutoMapper;
using IIASA.FloodCitiSense.Datatypes;
using IIASA.FloodCitiSense.Datatypes.Dtos;

namespace IIASA.FloodCitiSense.ValueResolver
{
    public class FloodTypesResolver : IValueResolver<Location, LocationDto, List<FloodTypeDto>>
    {
        public List<FloodTypeDto> Resolve(Location source, LocationDto destination, List<FloodTypeDto> destMember, ResolutionContext context)
        {
            if (source.Incident != null && source.Incident.FloodTypes != null)
            {
                return source.Incident.FloodTypes.Select(x => new FloodTypeDto
                {
                    Id = x.Id,
                    TenantId = x.TenantId,
                    TypeOfFlood = x.TypeOfFlood
                }).ToList();
            }
            return new List<FloodTypeDto>();
        }
    }
}