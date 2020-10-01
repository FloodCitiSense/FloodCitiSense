using System;
using System.Threading.Tasks;
using Abp.Dependency;
using IIASA.FloodCitiSense.Mobile.Core.Core.DataStorage;
using Plugin.Media;
using Plugin.Media.Abstractions;
using XF.Material.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace IIASA.FloodCitiSense.Mobile.Core.Services.Camera
{
    public class CameraService : ICameraService, ISingletonDependency
    {
        /// <summary>
        /// The media
        /// </summary>
        private readonly IMedia media;

        /// <summary>
        /// The page dialog service
        /// </summary>
        /// <summary>
        /// The data storage manager
        /// </summary>
        private readonly IDataStorageManager dataStorageManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraService"/> class.
        /// </summary>
        /// <param name="dataStorageManager"></param>
        public CameraService(IDataStorageManager dataStorageManager)
        {
            media = CrossMedia.Current;
            media.Initialize();
            this.dataStorageManager = dataStorageManager;
        }

        /// <summary>
        /// The take picture.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<string> TakePicture()
        {
            if (!media.IsCameraAvailable || !media.IsTakePhotoSupported)
            {
                await MaterialDialog.Instance.AlertAsync("No Camera", "No camera available", "ok");
                return string.Empty;
            }

            var fileName = $"{Guid.NewGuid().ToString()}.jpg";
            var file = await media.TakePhotoAsync(
                           new StoreCameraMediaOptions { SaveToAlbum = true, Name = fileName, AllowCropping = false, CompressionQuality = 20 });

            if (file == null) return string.Empty;
            await this.dataStorageManager.StoreAsync(DataStorageKey.PicturePath, file.Path);
            return file.Path;
        }

        /// <summary>
        /// Picks the picture.
        /// </summary>
        /// <returns></returns>
        public async Task<string> PickPicture()
        {
            if (!media.IsPickPhotoSupported)
            {
                await MaterialDialog.Instance.AlertAsync("No Upload", "Picking photo not supported", "ok");
                return string.Empty;
            }

            var file = await media.PickPhotoAsync();

            if (file == null) return string.Empty;
            await this.dataStorageManager.StoreAsync(DataStorageKey.PicturePath, file.Path);
            return file.Path;
        }

        /// <summary>
        /// The take video.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<string> TakeVideo()
        {
            if (!media.IsCameraAvailable || !media.IsTakeVideoSupported)
            {
                await MaterialDialog.Instance.AlertAsync("No Camera", ":( No camera available.", "OK");
                return string.Empty;
            }

            var fileName = $"{Guid.NewGuid().ToString()}.mp4";
            var file = await media.TakeVideoAsync(
                           new StoreVideoOptions { Directory = "DefaultVideos", Name = fileName, CompressionQuality = 30 });

            if (file == null) return string.Empty;
            var filePath = file.Path;
            await this.dataStorageManager.StoreAsync(DataStorageKey.VideoPath, file.Path);
            file.Dispose();

            return filePath;
        }
    }
}