//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="DevicePicture.cs" company="IIASA">
//    EOS
//  </copyright>
//  <summary>
//   DevicePicture.cs
//  </summary>
//  --------------------------------------------------------------------------------------------------------------------


using System;
using Xamarin.Forms;

namespace IIASA.FloodCitiSense.Mobile.Core.Core.Entity
{
    public class LocalImage
    {
        public Guid Id { get; set; }
        public string Path { get; set; }
        public ImageSource ImageSource => ImageSource.FromFile(Path);
        public bool IsUploaded { get; set; }
        public bool IsCreated { get; set; }
        public bool IsVideo { get; set; }
    }
}