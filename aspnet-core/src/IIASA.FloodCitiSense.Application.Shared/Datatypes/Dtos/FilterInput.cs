using System;

namespace IIASA.FloodCitiSense.Datatypes.Dtos
{
    public class FilterInput
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public DateTime LastTime { get; set; }
    }
}