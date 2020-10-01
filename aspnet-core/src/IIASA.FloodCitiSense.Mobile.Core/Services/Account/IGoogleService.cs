using IIASA.FloodCitiSense.Mobile.Core.Core.Entity;
using System;

namespace IIASA.FloodCitiSense.Mobile.Core.Services.Account
{
    /// <summary>
    /// Google manager
    /// </summary>
    public interface IGoogleService
    {
        /// <summary>
        /// Login the specified on login complete.
        /// </summary>
        /// <param name="onLoginComplete">The on login complete.</param>
        void Login(Action<AppUser, string> onLoginComplete);

        /// <summary>
        /// Logout this instance.
        /// </summary>
        void Logout();
    }
}