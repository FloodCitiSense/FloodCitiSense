using Abp.Dependency;
using Android.Content;
using IIASA.FloodCitiSense.Mobile.Core.Core.Entity;
using IIASA.FloodCitiSense.Mobile.Core.Services.Account;

namespace IIASA.FloodCitiSense
{
    using Android.Gms.Auth.Api;
    using Android.Gms.Auth.Api.SignIn;
    using Android.Gms.Common;
    using Android.Gms.Common.Apis;
    using Android.OS;
    using System;
    using Object = Java.Lang.Object;

    /// <summary>
    /// GoogleService
    /// </summary>
    /// <seealso cref="Java.Lang.Object" />
    /// <seealso cref="IGoogleService" />
    /// <seealso cref="Android.Gms.Common.Apis.GoogleApiClient.IConnectionCallbacks" />
    /// <seealso cref="Android.Gms.Common.Apis.GoogleApiClient.IOnConnectionFailedListener" />
    public class GoogleService : Object,
                                 IGoogleService,
                                 GoogleApiClient.IConnectionCallbacks,
                                 GoogleApiClient.IOnConnectionFailedListener, ISingletonDependency
    {

        static Context _context;

        public static void Init(Context context)
        {
            _context = context;
        }

        /// <summary>
        ///     The on login complete
        /// </summary>
        public Action<AppUser, string> OnLoginComplete;

        /// <summary>
        ///     Initializes a new instance of the <see cref="GoogleService" /> class.
        /// </summary>
        public GoogleService()
        {
            Instance = this;
            var gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
               .RequestIdToken("526829940303-4p7sa6kh8j5ojcb8gjnf6i3t4sind0sr.apps.googleusercontent.com").RequestId().RequestEmail().Build();
            //var gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
            //                    .RequestId().RequestEmail().Build();

            GoogleApiClient = new GoogleApiClient.Builder(((MainActivity)_context).ApplicationContext)
                .AddConnectionCallbacks(this).AddOnConnectionFailedListener(this).AddApi(Auth.GOOGLE_SIGN_IN_API, gso)
                .AddScope(new Scope(Scopes.Profile)).Build();
        }

        /// <summary>
        ///     Gets or sets the google API client.
        /// </summary>
        /// <value>
        ///     The google API client.
        /// </value>
        public static GoogleApiClient GoogleApiClient { get; set; }

        /// <summary>
        ///     Gets the instance.
        /// </summary>
        /// <value>
        ///     The instance.
        /// </value>
        public static GoogleService Instance { get; private set; }

        /// <summary>
        ///     Logins the specified on login complete.
        /// </summary>
        /// <param name="onLoginComplete">The on login complete.</param>
        public void Login(Action<AppUser, string> onLoginComplete)
        {
            OnLoginComplete = onLoginComplete;
            var signInIntent = Auth.GoogleSignInApi.GetSignInIntent(GoogleApiClient);
            ((MainActivity)_context).StartActivityForResult(signInIntent, 1);
            GoogleApiClient.Connect();
        }

        /// <summary>
        ///     Logouts this instance.
        /// </summary>
        public void Logout()
        {
            GoogleApiClient.Disconnect();
        }

        /// <summary>
        ///     Called when [authentication completed].
        /// </summary>
        /// <param name="result">The result.</param>
        public void OnAuthCompleted(GoogleSignInResult result)
        {
            if (result.IsSuccess)
            {
                var account = result.SignInAccount;
                OnLoginComplete?.Invoke(
                    new AppUser
                    {
                        Provider = "Google",
                        Id = account.Id,
                        Token = account.IdToken,
                        Name = account.DisplayName,
                        Email = account.Email,
                        Picture = new Uri(
                                account.PhotoUrl != null
                                     ? $"{account.PhotoUrl}"
                                     : $"")
                    },
                    string.Empty);
            }
            else
            {
                OnLoginComplete?.Invoke(null, "An error occurred!");
            }
        }

        /// <summary>
        ///     Called when [connected].
        /// </summary>
        /// <param name="connectionHint">The connection hint.</param>
        public void OnConnected(Bundle connectionHint)
        {
        }

        /// <summary>
        ///     Called when [connection failed].
        /// </summary>
        /// <param name="result">The result.</param>
        public void OnConnectionFailed(ConnectionResult result)
        {
            OnLoginComplete?.Invoke(null, result.ErrorMessage);
        }

        /// <summary>
        ///     Called when [connection suspended].
        /// </summary>
        /// <param name="cause">The cause.</param>
        public void OnConnectionSuspended(int cause)
        {
            OnLoginComplete?.Invoke(null, "Canceled!");
        }
    }
}