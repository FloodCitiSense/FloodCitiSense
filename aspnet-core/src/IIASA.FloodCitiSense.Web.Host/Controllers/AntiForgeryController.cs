using Microsoft.AspNetCore.Antiforgery;

namespace IIASA.FloodCitiSense.Web.Controllers
{
    public class AntiForgeryController : FloodCitiSenseControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public AntiForgeryController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public void GetToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }
    }
}
