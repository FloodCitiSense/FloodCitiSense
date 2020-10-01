//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="AlertSignUpModel.cs" company="IIASA">
//    EOS
//  </copyright>
//  <summary>
//   AlertSignUpModel.cs
//  </summary>
//  --------------------------------------------------------------------------------------------------------------------

using IIASA.FloodCitiSense.Mobile.Core.Base;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace IIASA.FloodCitiSense.ViewModels.Prepare
{
    public class AlertSignUpModel : XamarinViewModel
    {
        public AlertSignUpModel()
        {

        }

        public ICommand SignUpCommand => new Command(async () => await this.SignUp());

        private Task SignUp()
        {
            Device.OpenUri(new Uri("https://member.everbridge.net/index/453003085613079#/signup"));
            return Task.CompletedTask;
        }
    }
}