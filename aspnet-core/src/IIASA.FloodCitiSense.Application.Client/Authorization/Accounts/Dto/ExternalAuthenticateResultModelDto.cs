namespace IIASA.FloodCitiSense.Authorization.Accounts.Dto
{
    public class ExternalAuthenticateResultModelDto
    {
        public string AccessToken { get; set; }

        public string EncryptedAccessToken { get; set; }

        public string RefreshToken { get; set; }

        public int ExpireInSeconds { get; set; }

        public bool WaitingForActivation { get; set; }

        public string ReturnUrl { get; set; }
    }
}
