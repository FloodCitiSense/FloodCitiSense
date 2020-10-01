using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Abp.Extensions;
using Abp.IO.Extensions;
using Abp.Runtime.Session;
using Abp.UI;
using Abp.Web.Models;
using IIASA.FloodCitiSense.Authorization.Users.Profile.Dto;
using IIASA.FloodCitiSense.IO;
using IIASA.FloodCitiSense.Web.Helpers;

namespace IIASA.FloodCitiSense.Web.Controllers
{
    public abstract class ProfileControllerBase : FloodCitiSenseControllerBase
    {
        private readonly IAppFolders _appFolders;
        private const int MaxProfilePictureSize = 5242880; //5MB

        protected ProfileControllerBase(IAppFolders appFolders)
        {
            _appFolders = appFolders;
        }

        public UploadProfilePictureOutput UploadProfilePicture()
        {
            try
            {
                var profilePictureFile = Request.Form.Files.First();

                //Check input
                if (profilePictureFile == null)
                {
                    throw new UserFriendlyException(L("ProfilePicture_Change_Error"));
                }

                if (profilePictureFile.Length > MaxProfilePictureSize)
                {
                    throw new UserFriendlyException(L("ProfilePicture_Warn_SizeLimit", AppConsts.MaxProfilPictureBytesUserFriendlyValue));
                }

                byte[] fileBytes;
                using (var stream = profilePictureFile.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var fileInfo = new FileInfo(profilePictureFile.FileName);
                if (!new List<string> { ".JPG", ".JPEG", ".JPE", ".BMP", ".GIF", ".PNG" }.Contains(fileInfo.Extension.ToUpperInvariant()))
                {
                    throw new Exception("Uploaded file is not an accepted image file !");
                }

                //Delete old temp profile pictures
                var fileNameWithoutExtension = "userProfileImage_" + AbpSession.GetUserId();
                AppFileHelper.DeleteFilesInFolderIfExists(_appFolders.TempFileDownloadFolder, fileNameWithoutExtension);

                //Save new picture
                var tempFileName = fileNameWithoutExtension + fileInfo.Extension;
                var tempFilePath = Path.Combine(_appFolders.TempFileDownloadFolder, tempFileName);
                System.IO.File.WriteAllBytes(tempFilePath, fileBytes);

                return new UploadProfilePictureOutput
                {
                    FileName = tempFileName,
                };
            }
            catch (UserFriendlyException ex)
            {
                return new UploadProfilePictureOutput(new ErrorInfo(ex.Message));
            }
        }
    }
}