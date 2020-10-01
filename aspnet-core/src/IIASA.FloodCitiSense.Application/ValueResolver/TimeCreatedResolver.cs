using System;
using AutoMapper;
using IIASA.FloodCitiSense.Datatypes;
using IIASA.FloodCitiSense.Datatypes.Dtos;

namespace IIASA.FloodCitiSense.ValueResolver
{
    public class TimeCreatedResolver : IValueResolver<Location, LocationDto, DateTime>
    {
        public DateTime Resolve(Location source, LocationDto destination, DateTime destMember, ResolutionContext context)
        {
            return source.Incident != null ? source.Incident.TimeCreated : new DateTime();
        }
    }
}