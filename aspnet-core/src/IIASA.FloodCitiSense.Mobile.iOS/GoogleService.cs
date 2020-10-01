using Abp.Dependency;
using IIASA.FloodCitiSense.Mobile.Core.Core.Entity;
using IIASA.FloodCitiSense.Mobile.Core.Services.Account;

namespace IIASA.FloodCitiSense
{
    using Foundation;
    using Google.SignIn;
    using System;
    using UIKit;

    /// <summary>
    /// </summary>
    /// <seealso cref="Foundation.NSObject" />
    /// <seealso cref="IGoogleService" />
    /// <seealso cref="Google.SignIn.ISignInDelegate" />
    /// <seealso cref="Google.SignIn.ISignInUIDelegate" />
    public class GoogleService : NSObject, IGoogleService, ISignInDelegate, ISignInUIDelegate, ISingletonDependency
    {
        /// <summary>
        ///     The on login complete
        /// </summary>
        private Action<AppUser, string> _onLoginComplete;

        /// <summary>
        ///     Initializes a new instance of the <see cref="GoogleService" /> class.
        /// </summary>
        public GoogleService()
        {
            SignIn.SharedInstance.UIDelegate = this;
            SignIn.SharedInstance.Delegate = this;
        }

        /// <summary>
        ///     Gets or sets the view controller.
        /// </summary>
        /// <value>
        ///     The view controller.
        /// </value>
        private UIViewController _viewController { get; set; }

        /// <summary>
        ///     Dids the disconnect.
        /// </summary>
        /// <param name="signIn">The sign in.</param>
        /// <param name="user">The user.</param>
        /// <param name="error">The error.</param>
        [Export("signIn:didDisconnectWithUser:withError:")]
        public void DidDisconnect(SignIn signIn, GoogleUser user, NSError error)
        {
            // Perform any operations when the user disconnects from app here.
        }

        /// <summary>
        ///     Dids the sign in.
        /// </summary>
        /// <param name="signIn">The sign in.</param>
        /// <param name="user">The user.</param>
        /// <param name="error">The error.</param>
        public void DidSignIn(SignIn signIn, GoogleUser user, NSError error)
        {
            if (user != null && error == null)
                this._onLoginComplete?.Invoke(
                    new AppUser()
                    {
                        Name = user.Profile.Name,
                        Email = user.Profile.Email,
                        Picture = user.Profile.HasImage
                                          ? new Uri(user.Profile.GetImageUrl(500).ToString())
                                          : new Uri(string.Empty)
                    },
                    string.Empty);
            else this._onLoginComplete?.Invoke(null, error.LocalizedDescription);
        }

        /// <summary>
        ///     Dismisses the view controller.
        /// </summary>
        /// <param name="signIn">The sign in.</param>
        /// <param name="viewController">The view controller.</param>
        [Export("signIn:dismissViewController:")]
        public void DismissViewController(SignIn signIn, UIViewController viewController)
        {
            this._viewController?.DismissViewController(true, null);
        }

        /// <summary>
        ///     Logins the specified on login complete.
        /// </summary>
        /// <param name="OnLoginComplete">The on login complete.</param>
        public void Login(Action<AppUser, string> OnLoginComplete)
        {
            this._onLoginComplete = OnLoginComplete;

            var window = UIApplication.SharedApplication.KeyWindow;
            var vc = window.RootViewController;
            while (vc.PresentedViewController != null)
            {
                vc = vc.PresentedViewController;
            }

            this._viewController = vc;

            SignIn.SharedInstance.SignInUser();
        }

        /// <summary>
        ///     Logout this instance.
        /// </summary>
        public void Logout()
        {
            SignIn.SharedInstance.SignOutUser();
        }

        /// <summary>
        ///     Presents the view controller.
        /// </summary>
        /// <param name="signIn">The sign in.</param>
        /// <param name="viewController">The view controller.</param>
        [Export("signIn:presentViewController:")]
        public void PresentViewController(SignIn signIn, UIViewController viewController)
        {
            this._viewController?.PresentViewController(viewController, true, null);
        }

        /// <summary>
        ///     Wills the dispatch.
        /// </summary>
        /// <param name="signIn">The sign in.</param>
        /// <param name="error">The error.</param>
        [Export("signInWillDispatch:error:")]
        public void WillDispatch(SignIn signIn, NSError error)
        {
            // myActivityIndicator.StopAnimating();
        }
    }
}