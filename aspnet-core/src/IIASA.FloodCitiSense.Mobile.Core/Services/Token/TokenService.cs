//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="TokenService.cs" company="IIASA">
//    EOS
//  </copyright>
//  <summary>
//   TokenService.cs
//  </summary>
//  --------------------------------------------------------------------------------------------------------------------

using Abp.Dependency;
using Flurl.Http;
using IIASA.FloodCitiSense.ApiClient;
using IIASA.FloodCitiSense.Authorization.Accounts.Dto;
using IIASA.FloodCitiSense.Mobile.Core.Core.DataStorage;
using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace IIASA.FloodCitiSense.Mobile.Core.Services.Token
{
    public class TokenService : ITokenService, ISingletonDependency
    {

        private const string TokenUrlSegment = "connect/token";
        private readonly IDataStorageManager _dataStorageManager;
        private readonly AbpApiClient _apiClient;

        private DateTime _lastTokenRequestedTime;

        private string _accessToken;

        private string _refreshToken;

        public TokenService(IDataStorageManager dataStorageManager, AbpApiClient apiClient)
        {
            _dataStorageManager = dataStorageManager;
            _apiClient = apiClient;

            if (dataStorageManager.HasKey(DataStorageKey.LastTokenRequestedTime))
            {
                this._lastTokenRequestedTime = dataStorageManager.Retrieve<DateTime>(DataStorageKey.LastTokenRequestedTime);
            }

            if (dataStorageManager.HasKey(DataStorageKey.AccessToken))
            {
                this._accessToken = dataStorageManager.Retrieve<string>(DataStorageKey.AccessToken);
            }

            if (dataStorageManager.HasKey(DataStorageKey.RefreshToken))
            {
                this._refreshToken = dataStorageManager.Retrieve<string>(DataStorageKey.RefreshToken);
            }
        }

        public async Task<string> GetAccessToken()
        {
            if (this._lastTokenRequestedTime == DateTime.MinValue)
                if (_accessToken != null)
                    return _accessToken;
            var min = (DateTime.UtcNow - this._lastTokenRequestedTime).Minutes;
            if (min <= 25) return _accessToken;
            var newToken = await RenewTokenAsync();
            if (newToken != null)
            {
                return newToken;
            }

            throw new UnauthorizedAccessException();
        }

        private async Task<string> RenewTokenAsync()
        {

            using (IFlurlClient client = new FlurlClient(ApiUrlConfig.BaseUrl))
            {
                client.WithHeader("Accept", new MediaTypeWithQualityHeaderValue("application/json"));
                client.WithHeader("User-Agent", AbpApiClient.UserAgent);

                var response = await client
                    .Request(TokenUrlSegment)
                    .PostUrlEncodedAsync(new
                    {
                        grant_type = "refresh_token",
                        client_id = "FloodCitiSense",
                        refresh_token = this._refreshToken
                    })
                    .ReceiveJson<RefreshTokenDto>();

                if (response != null)
                {
                    await _dataStorageManager.StoreAsync(DataStorageKey.AccessToken, response.AccessToken);
                    await _dataStorageManager.StoreAsync(DataStorageKey.RefreshToken, response.RefreshToken, true);
                    await _dataStorageManager.StoreAsync(DataStorageKey.LastTokenRequestedTime, DateTime.UtcNow);
                    return response.AccessToken;
                }

            }
            throw new UnauthorizedAccessException();
        }
    }
}