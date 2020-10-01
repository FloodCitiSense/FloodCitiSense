//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="TypeOfFlooding.cs" company="IIASA">
//    EOS
//  </copyright>
//  <summary>
//   TypeOfFlooding.cs
//  </summary>
//  --------------------------------------------------------------------------------------------------------------------

using System;

namespace IIASA.FloodCitiSense.Datatypes
{
    [Flags]
    public enum TypeOfFlood
    {
        NoFlooding,
        StreetFlooding,
        ParksFlooding,
        HouseFlooding,
        GardenFlooding,
        RiverFlooding,
        RiverLevelRising,
        SewerBlocked
    }
}