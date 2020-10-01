//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="PictureController.cs" company="IIASA">
//    EOS
//  </copyright>
//  <summary>
//   PictureController.cs
//  </summary>
//  --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Abp.Authorization;
using Abp.Extensions;
using Abp.IO.Extensions;
using Abp.Runtime.Session;
using Abp.UI;
using Abp.Web.Models;
using IIASA.FloodCitiSense.Authorization.Users.Profile.Dto;
using IIASA.FloodCitiSense.Datatypes.Dtos;
using IIASA.FloodCitiSense.Dto;
using IIASA.FloodCitiSense.IO;
using IIASA.FloodCitiSense.Web.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IIASA.FloodCitiSense.Web.Controllers
{
    [AbpAuthorize]
    public class PictureController :  FloodCitiSenseControllerBase
    {
        private readonly IAppFolders _appFolders;

        public PictureController(IAppFolders appFolders)
        {
            _appFolders = appFolders;
        }


        public FilesDto UploadPictures(List<IFormFile> files)
        {
            try
            {
                var fileCollection = files;

                //Check input
                if (fileCollection == null)
                {
                    throw new UserFriendlyException(L("ProfilePicture_Change_Error"));
                }
                var tempFilePaths = new List<string>();
                foreach (var file in fileCollection)
                {
                    byte[] fileBytes;
                    var fileInfo = new FileInfo(file.FileName);
                    using (var stream = file.OpenReadStream())
                    {
                        fileBytes = stream.GetAllBytes();
                    }

                    if (!new List<string> { ".JPG", ".JPEG", ".JPE", ".BMP", ".GIF", ".PNG" }.Contains(fileInfo.Extension.ToUpperInvariant()))
                    {
                        throw new Exception("Uploaded file is not an accepted image file !");
                    }

                    //Save new picture
                    var tempFileName = "Image_"+ Guid.NewGuid().ToString() + AbpSession.GetUserId() + fileInfo.Extension;//
                    var tempFilePath = Path.Combine(_appFolders.ImagesFolder, tempFileName);
                    System.IO.File.WriteAllBytes(tempFilePath, fileBytes);
                    tempFilePaths.Add(tempFilePath);
                }

                return new FilesDto
                {
                    Files = tempFilePaths
                };
            }
            catch (UserFriendlyException ex)
            {
                return null;
            }
        }


        public FilesDto UploadPicture(IFormFile file)
        {
            try
            {
                //Check input
                if (file == null)
                {
                    throw new UserFriendlyException(L("ProfilePicture_Change_Error"));
                }

                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var fileInfo = new FileInfo(file.FileName);

                if (!new List<string> { ".JPG", ".JPEG", ".JPE", ".BMP", ".GIF", ".PNG" }.Contains(fileInfo.Extension.ToUpperInvariant()))
                {
                    throw new Exception("Uploaded file is not an accepted image file !");
                }

                //Save new picture
                var tempFileName = "Image_" + Guid.NewGuid().ToString() + AbpSession.GetUserId() + fileInfo.Extension;// +
                var tempFilePath = Path.Combine(_appFolders.ImagesFolder, tempFileName);
                System.IO.File.WriteAllBytes(tempFilePath, fileBytes);
                return new FilesDto
                {
                    Files = new List<string> { tempFilePath }
                };
            }
            catch (UserFriendlyException ex)
            {
                return null;
            }
        }

    }
}