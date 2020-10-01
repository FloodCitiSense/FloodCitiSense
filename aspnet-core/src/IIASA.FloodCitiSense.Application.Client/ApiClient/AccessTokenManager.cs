using Abp.Dependency;
using Abp.UI;
using Abp.Web.Models;
using Flurl.Http;
using IIASA.FloodCitiSense.ApiClient.Models;
using JetBrains.Annotations;
using System;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;


namespace IIASA.FloodCitiSense.ApiClient
{
    public class AccessTokenManager : IAccessTokenManager, ISingletonDependency
    {
        private const string LoginUrlSegment = "api/TokenAuth/Authenticate";
        private const string TokenUrlSegment = "connect/token";

        private readonly AbpAuthenticateModel _authenticateModel;
        private readonly IApplicationContext _applicationContext;
        private int _authenticationRetrySeconds = 2;

        [CanBeNull]
        private AbpAuthenticateResultModel _authenticateResult;

        private Timer _refreshTimer;

        public bool IsUserLoggedIn => !string.IsNullOrEmpty(_applicationContext?.AppInformation?.AccessToken);

        public AccessTokenManager(
            AbpAuthenticateModel authenticateModel,
            IApplicationContext applicationContext)
        {
            _authenticateModel = authenticateModel;
            _applicationContext = applicationContext;
        }

        //public async Task<string> GetAccessTokenAsync()
        //{
        //    if (_authenticateResult != null)
        //    {
        //        return await Task.FromResult(_authenticateResult.AccessToken) ;
        //    }

        //   throw new UnauthorizedAccessException();
        //}

        public async Task<AbpAuthenticateResultModel> LoginAsync()
        {
            EnsureUserNameAndPasswordProvided();
            using (IFlurlClient client = new FlurlClient(ApiUrlConfig.BaseUrl))
            {
                client.AllowAnyHttpStatus();
                client.WithHeader("Accept", new MediaTypeWithQualityHeaderValue("application/json"));
                client.WithHeader("User-Agent", AbpApiClient.UserAgent);
                client.WithHeader("Abp.TenantId", "1");

                var response = await client
                    .Request(LoginUrlSegment)
                    .PostJsonAsync(_authenticateModel)
                    .ReceiveJson<AjaxResponse<AbpAuthenticateResultModel>>();

                if (!response.Success)
                {
                    _authenticateResult = null;
                    throw new UserFriendlyException(response.Error.Code,response.Error.Message , response.Error.Details);
                }

                _authenticateResult = response.Result;

                ConfigureTokenRefresh(response.Result);

                return _authenticateResult;
            }
        }

        public void Logout()
        {
            _authenticateResult = null;
        }

        private void ConfigureTokenRefresh(AbpAuthenticateResultModel authenticateResult)
        {
            if (authenticateResult.ExpireInSeconds <= 1)
            {
                //Normally, ExpireInSeconds should never be that small. However, we wanted to handle the case.
                _refreshTimer?.Dispose();
                _refreshTimer = null;
            }
            else
            {
                var reAuthenticationWaitTime = (authenticateResult.ExpireInSeconds - 1) * 1000;

                if (_refreshTimer == null)
                {
                    _refreshTimer = new Timer(RefreshTimerElapsed, _authenticateModel, reAuthenticationWaitTime, reAuthenticationWaitTime);
                }
                else
                {
                    _refreshTimer.Change(reAuthenticationWaitTime, reAuthenticationWaitTime);
                }
            }
        }

        private void EnsureUserNameAndPasswordProvided()
        {
            if (_authenticateModel.UserNameOrEmailAddress == null || _authenticateModel.Password == null)
            {
                throw new System.Exception("Username or password fields cannot be empty!");
            }
        }

        /// <summary>
        /// Renew token periodically
        /// </summary>
        private async void RefreshTimerElapsed(object authenticateModel)
        {
            try
            {
                await LoginAsync();
                _authenticationRetrySeconds = 2; //reset retry duration when no exception in login process.
            }
            catch
            {
                _authenticationRetrySeconds = GetExponentiallyIncreasingNumber(_authenticationRetrySeconds);
                _refreshTimer.Change(_authenticationRetrySeconds, _authenticationRetrySeconds);
            }
        }

        private static int GetExponentiallyIncreasingNumber(int currentValue, int maxValue = 300)
        {
            // This gives an increasing number 1 > 2 > 3 > 6 > 10 > 19 > 34 > 61 > 110 > 198 ...
            if (currentValue > maxValue)
            {
                return currentValue;
            }

            return Convert.ToInt32(Math.Pow(1.8, currentValue));
        }
    }
}