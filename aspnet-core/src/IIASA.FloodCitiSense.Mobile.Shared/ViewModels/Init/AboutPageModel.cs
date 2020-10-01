using IIASA.FloodCitiSense.Mobile.Core.Base;
using System;

namespace IIASA.FloodCitiSense.ViewModels.Init
{
    class AboutPageModel : XamarinViewModel
    {
        public AboutPageModel()
        {
            CopyrightText = $"© {DateTime.Now.Year} IIASA";
        }

        public string CopyrightText { get; set; }
    }
}
