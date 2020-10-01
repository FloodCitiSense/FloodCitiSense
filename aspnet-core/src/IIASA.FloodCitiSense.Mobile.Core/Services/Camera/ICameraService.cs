using System.Threading.Tasks;

namespace IIASA.FloodCitiSense.Mobile.Core.Services.Camera
{
    /// <summary>
    /// The CameraService interface.
    /// </summary>
    public interface ICameraService
    {
        /// <summary>
        /// Takes the picture.
        /// </summary>
        /// <returns></returns>
        Task<string> TakePicture();

        /// <summary>
        /// Picks the picture.
        /// </summary>
        /// <returns></returns>
        Task<string> PickPicture();

        /// <summary>
        /// Takes the video.
        /// </summary>
        /// <returns></returns>
        Task<string> TakeVideo();
    }
}