using Abp.Dependency;
using Android.App;
using Android.Content;
using Android.OS;
using IIASA.FloodCitiSense.Mobile.Core.Core.Entity;
using IIASA.FloodCitiSense.Mobile.Core.Services.Account;
using Org.Json;
using System;
using System.Collections.Generic;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using Object = Java.Lang.Object;

namespace IIASA.FloodCitiSense
{
    /// <summary>
    /// FacebookService
    /// </summary>
    /// <seealso cref="Java.Lang.Object" />
    /// <seealso cref="IFacebookService" />
    /// <seealso cref="Xamarin.Facebook.IFacebookCallback" />
    /// <seealso cref="Xamarin.Facebook.GraphRequest.IGraphJSONObjectCallback" />
    public class FacebookService : Object, IFacebookService, IFacebookCallback, GraphRequest.IGraphJSONObjectCallback,
        ISingletonDependency
    {
        /// <summary>
        ///     The callback manager
        /// </summary>
        public ICallbackManager CallbackManager;

        /// <summary>
        ///     The on login complete
        /// </summary>
        public Action<AppUser, string> OnLoginComplete;

        /// <summary>
        ///     Initializes a new instance of the <see cref="FacebookService" /> class.
        /// </summary>
        public FacebookService()
        {
            Instance = this;
            CallbackManager = CallbackManagerFactory.Create();
            LoginManager.Instance.RegisterCallback(CallbackManager, this);
        }


        static Context _context;

        public static void Init(Context context)
        {
            _context = context;
        }

        /// <summary>
        ///     Gets the instance.
        /// </summary>
        public static FacebookService Instance { get; private set; }

        /// <summary>
        ///     Called when [cancel].
        /// </summary>
        public void OnCancel()
        {
            OnLoginComplete?.Invoke(null, "Canceled!");
        }

        /// <summary>
        ///     Called when [error].
        /// </summary>
        /// <param name="error">The error.</param>
        public void OnError(FacebookException error)
        {
            OnLoginComplete?.Invoke(null, error.Message);
        }

        /// <summary>
        ///     Called when [success].
        /// </summary>
        /// <param name="result">The result.</param>
        public void OnSuccess(Object result)
        {
            if (result is LoginResult n)
            {
                var request = GraphRequest.NewMeRequest(n.AccessToken, this);
                var bundle = new Bundle();
                bundle.PutString("fields", "id, first_name, email, last_name, picture.width(500).height(500)");
                request.Parameters = bundle;
                request.ExecuteAsync();
            }
        }

        /// <summary>
        ///     Logins the specified on login complete.
        /// </summary>
        /// <param name="onLoginComplete">The on login complete.</param>
        public void Login(Action<AppUser, string> onLoginComplete)
        {
            OnLoginComplete = onLoginComplete;
            LoginManager.Instance.SetLoginBehavior(LoginBehavior.NativeWithFallback);
            LoginManager.Instance.LogInWithReadPermissions(
                _context as Activity,
                new List<string> { "public_profile", "email" });
        }

        /// <summary>
        ///     Logout this instance.
        /// </summary>
        public void Logout()
        {
            LoginManager.Instance.LogOut();
        }

        /// <summary>
        ///     Called when [completed].
        /// </summary>
        /// <param name="p0">The p0.</param>
        /// <param name="p1">The p1.</param>
        public void OnCompleted(JSONObject p0, GraphResponse p1)
        {
            var id = string.Empty;
            var firstName = string.Empty;
            var email = string.Empty;
            var pictureUrl = string.Empty;

            if (p0.Has("id")) id = p0.GetString("id");

            if (p0.Has("first_name")) firstName = p0.GetString("first_name");

            if (p0.Has("email")) email = p0.GetString("email");

            if (p0.Has("picture"))
            {
                var p2 = p0.GetJSONObject("picture");
                if (p2.Has("data"))
                {
                    var p3 = p2.GetJSONObject("data");
                    if (p3.Has("url")) pictureUrl = p3.GetString("url");
                }
            }

            OnLoginComplete?.Invoke(
                new AppUser(id, AccessToken.CurrentAccessToken.Token, firstName, firstName, email, pictureUrl,
                    "Facebook"),
                string.Empty);
        }
    }
}