//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IncidentPin.cs" company="IIASA">
//    EOS
//  </copyright>
//  <summary>
//   IncidentPin.cs
//  </summary>
//  --------------------------------------------------------------------------------------------------------------------

using Xamarin.Forms.GoogleMaps;

namespace IIASA.FloodCitiSense.Mobile.Core.Controls
{
    public static class PinExtensions
    {
        public static PinInfo GetPinInfo(this Pin pin)
        {
            if (pin?.Tag is PinInfo info)
            {
                return info;
            }
            return null;
        }

        public static void SetPinInfo(this Pin pin, PinInfo pinInfo)
        {
            pin.Tag = pinInfo;
        }
    }

    public enum PinType
    {
        CurrentUser,
        Device,
        OtherUser
    }

    public class PinInfo
    {
        public int IncidentId { get; set; }
        public string MobileId { get; set; }
        public string DeviceId { get; set; }
        public bool IsLocal { get; set; }
        public bool IsDevice { get; set; }
        public object PinObject { get; set; }
        public PinType PinType { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}