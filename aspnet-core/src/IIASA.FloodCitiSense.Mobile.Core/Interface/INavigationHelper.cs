//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="INavigationHelper.cs" company="IIASA">
//    EOS
//  </copyright>
//  <summary>
//   INavigationHelper.cs
//  </summary>
//  --------------------------------------------------------------------------------------------------------------------

using System.Threading.Tasks;

namespace IIASA.FloodCitiSense.Mobile.Core.Interface
{
    public interface INavigationHelper
    {
        Task InitNavigation(bool launchFromNotification);
        Task HandleTenantNotAvailableException();
        Task HandleUnAuthorizedException();

        void ResetDataStore();
    }
}