using IIASA.FloodCitiSense.Mobile.Core.Core.Entity;
using System;

namespace IIASA.FloodCitiSense.Mobile.Core.Services.Account
{
    public interface IFacebookService
    {
        /// <summary>
        ///     Logins the specified on login complete.
        /// </summary>
        /// <param name="onLoginComplete">The on login complete.</param>
        void Login(Action<AppUser, string> onLoginComplete);

        /// <summary>
        ///     Logouts this instance.
        /// </summary>
        void Logout();
    }
}