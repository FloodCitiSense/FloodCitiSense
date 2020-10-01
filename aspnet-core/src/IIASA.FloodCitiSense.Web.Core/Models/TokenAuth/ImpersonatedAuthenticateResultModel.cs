namespace IIASA.FloodCitiSense.Web.Models.TokenAuth
{
    public class ImpersonatedAuthenticateResultModel
    {
        public string AccessToken { get; set; }

        public string EncryptedAccessToken { get; set; }

        public string RefreshToken { get; set; }

        public int ExpireInSeconds { get; set; }
    }
}