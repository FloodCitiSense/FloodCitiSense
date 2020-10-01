//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="AppInformation.cs" company="IIASA">
//    EOS
//  </copyright>
//  <summary>
//   AppInformation.cs
//  </summary>
//  --------------------------------------------------------------------------------------------------------------------

using IdentityModel.Client;
using JetBrains.Annotations;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace IIASA.FloodCitiSense.ApiClient
{
    public class AppInformation : INotifyPropertyChanged
    {
        public string IdToken { get; set; }
        public string AccessToken
        {
            get => _accessToken;
            set
            {
                _accessToken = value;
                OnPropertyChanged(nameof(AccessToken));
            }
        }

        public long ExpiresIn { get; set; }
        public string TokenType { get; set; }

        public string RefreshToken
        {
            get => _refreshToken;
            set
            {
                _refreshToken = value;
                OnPropertyChanged(nameof(RefreshToken));
            }
        }

        public DateTime LastTokenRequestedTime
        {
            get => _lastTokenRequestedTime;
            set
            {
                _lastTokenRequestedTime = value;
                OnPropertyChanged(nameof(LastTokenRequestedTime));
            }
        }

        private const string TokenUrlSegment = "connect/token";
        private string _accessToken;
        private string _refreshToken;
        private DateTime _lastTokenRequestedTime;

        public async Task<string> GetAccessTokenAsync()
        {
            try
            {
                //throw new UnauthorizedAccessException();
                if (AccessToken == null)
                {
                    throw new UnauthorizedAccessException();
                }

                if (this.LastTokenRequestedTime == DateTime.MinValue)
                {
                    return AccessToken;
                }

                var difference = DateTime.UtcNow.Subtract(this.LastTokenRequestedTime);

                var min = difference.TotalMinutes;

                if (min <= 20) return AccessToken;
                var newToken = await RenewTokenAsync();
                if (newToken != null)
                {
                    return newToken;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new UnauthorizedAccessException();
            }
            throw new UnauthorizedAccessException();
        }

        private async Task<string> RenewTokenAsync()
        {
            try
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri(ApiUrlConfig.BaseUrl)
                };

                var response = await client.RequestRefreshTokenAsync(new RefreshTokenRequest
                {
                    Address = TokenUrlSegment,
                    ClientId = "FloodCitiSense",
                    RefreshToken = RefreshToken
                });

                if (response != null)
                {
                    AccessToken = response.AccessToken;
                    ExpiresIn = response.ExpiresIn;
                    TokenType = response.TokenType;
                    LastTokenRequestedTime = DateTime.UtcNow;
                    RefreshToken = response.RefreshToken;
                    return response.AccessToken;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new UnauthorizedAccessException();
            }
            throw new UnauthorizedAccessException();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}