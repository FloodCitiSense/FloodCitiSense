//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="RefreshTokenDto.cs" company="IIASA">
//    EOS
//  </copyright>
//  <summary>
//   RefreshTokenDto.cs
//  </summary>
//  --------------------------------------------------------------------------------------------------------------------

namespace IIASA.FloodCitiSense.Authorization.Accounts.Dto
{
    using Newtonsoft.Json;

    public partial class RefreshTokenDto
    {
        [JsonProperty("id_token")]
        public string IdToken { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }
}