//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="AllDevices.cs" company="IIASA">
//    EOS
//  </copyright>
//  <summary>
//   AllDevices.cs
//  </summary>
//  --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

namespace IIASA.FloodCitiSense.Mobile.Core.Services.Observation.Dto
{
    public partial class AllDevices
    {
        public ObservationCollection ObservationCollection { get; set; }
    }

    public partial class ObservationCollection
    {
        public List<Member> Member { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
    }

    public partial class Member
    {
        public string Name { get; set; }
        public SamplingTime SamplingTime { get; set; }
        public Result Result { get; set; }
        public FeatureOfInterest FeatureOfInterest { get; set; }
        public ObservedProperty ObservedProperty { get; set; }
        public string Procedure { get; set; }
    }

    public partial class FeatureOfInterest
    {
        public string Geom { get; set; }
        public string Name { get; set; }
    }

    public partial class ObservedProperty
    {
        public List<string> Component { get; set; }
        public CompositePhenomenon CompositePhenomenon { get; set; }
    }

    public partial class CompositePhenomenon
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public long? Dimension { get; set; }
    }

    public partial class Result
    {
        public DataArray DataArray { get; set; }
    }

    public partial class DataArray
    {
        public long? ElementCount { get; set; }
        public List<List<string>> Values { get; set; }
        public List<Field> Field { get; set; }
    }

    public partial class Field
    {
        public string Definition { get; set; }
        public string Name { get; set; }
        public string Uom { get; set; }
    }

    public partial class SamplingTime
    {
        public string Duration { get; set; }
        public string BeginPosition { get; set; }
        public string EndPosition { get; set; }
    }

}