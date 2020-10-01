using System.Threading.Tasks;

namespace IIASA.FloodCitiSense.Security.Recaptcha
{
    public interface IRecaptchaValidator
    {
        Task ValidateAsync(string captchaResponse);
    }
}