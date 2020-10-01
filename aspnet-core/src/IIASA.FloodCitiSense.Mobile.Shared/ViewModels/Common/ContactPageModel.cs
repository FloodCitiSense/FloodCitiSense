//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ContactPageModel.cs" company="IIASA">
//    EOS
//  </copyright>
//  <summary>
//   ContactPageModel.cs
//  </summary>
//  --------------------------------------------------------------------------------------------------------------------

using IIASA.FloodCitiSense.Mobile.Core.Base;
using IIASA.FloodCitiSense.Mobile.Core.Core.Threading;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace IIASA.FloodCitiSense.ViewModels.Common
{
    public class ContactPageModel : XamarinViewModel
    {
        public ICommand EmailCommand => AsyncCommand.Create(OpenEmail);
        public ICommand TwitterCommand => AsyncCommand.Create(OpenTwitter);
        public ICommand FacebookCommand => AsyncCommand.Create(OpenFacebook);


        public ContactPageModel()
        {

        }

        private Task OpenFacebook()
        {
            Launcher.OpenAsync(new Uri("https://www.facebook.com/FloodCitiSense/"));
            return Task.CompletedTask;
        }


        private Task OpenTwitter()
        {
            Launcher.OpenAsync(new Uri("https://twitter.com/floodcitisense"));
            return Task.CompletedTask;
        }

        private Task OpenEmail()
        {
            Launcher.OpenAsync(new Uri("mailto:info@floodcitisense.eu"));
            return Task.CompletedTask;
        }
    }
}