//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ProxyPictureControllerService.cs" company="IIASA">
//    EOS
//  </copyright>
//  <summary>
//   ProxyPictureControllerService.cs
//  </summary>
//  --------------------------------------------------------------------------------------------------------------------

using IIASA.FloodCitiSense.Datatypes.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IIASA.FloodCitiSense.Picture
{
    public class ProxyPictureControllerService : ProxyControllerBase
    {
        public async Task<FilesDto> UploadPicture(string fileName)
        {
            return await ApiClient
                .PostMultipartAsync<FilesDto>(GetEndpoint(nameof(UploadPicture)), fileName);
        }

        public async Task<FilesDto> UploadPictures(List<string> fileName)
        {
            return await ApiClient
                .PostMultipartAsync<FilesDto>(GetEndpoint(nameof(UploadPictures)), fileName);
        }
    }
}