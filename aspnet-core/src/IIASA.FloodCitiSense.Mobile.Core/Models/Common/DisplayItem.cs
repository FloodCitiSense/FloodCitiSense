//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="DisplayItem.cs" company="IIASA">
//    EOS
//  </copyright>
//  <summary>
//   DisplayItem.cs
//  </summary>
//  --------------------------------------------------------------------------------------------------------------------

namespace IIASA.FloodCitiSense.Mobile.Core.Models.Common
{
    public class DisplayItem
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public override string ToString() => Text;
    }
}