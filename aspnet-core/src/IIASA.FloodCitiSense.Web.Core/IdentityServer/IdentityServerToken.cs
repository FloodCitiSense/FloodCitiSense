using IdentityServer4.Models;

namespace IIASA.FloodCitiSense.Web.IdentityServer
{
    public class IdentityServerToken
    {
        public IdentityServerToken(Token token, string accessToken, string refreshToken)
        {
            Token = token;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        public Token Token { get; protected set; }

        public string AccessToken { get; protected set; }

        public string RefreshToken { get; protected set; }
    }
}