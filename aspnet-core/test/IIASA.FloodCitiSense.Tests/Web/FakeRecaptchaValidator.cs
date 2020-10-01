using System.Threading.Tasks;
using IIASA.FloodCitiSense.Security.Recaptcha;

namespace IIASA.FloodCitiSense.Tests.Web
{
    public class FakeRecaptchaValidator : IRecaptchaValidator
    {
        public Task ValidateAsync(string captchaResponse)
        {
            return Task.CompletedTask;
        }
    }
}
