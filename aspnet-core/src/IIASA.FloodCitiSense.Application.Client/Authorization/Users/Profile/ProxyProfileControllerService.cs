using IIASA.FloodCitiSense.Authorization.Users.Profile.Dto;
using System.IO;
using System.Threading.Tasks;

namespace IIASA.FloodCitiSense.Authorization.Users.Profile
{
    public class ProxyProfileControllerService : ProxyControllerBase
    {
        public async Task<UploadProfilePictureOutput> UploadProfilePicture(Stream stream, string fileName)
        {
            return await ApiClient
                .PostMultipartAsync<UploadProfilePictureOutput>(GetEndpoint(nameof(UploadProfilePicture)), stream, fileName);
        }
    }
}