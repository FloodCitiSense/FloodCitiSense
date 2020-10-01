using Abp.Application.Services.Dto;
using Abp.Domain.Entities;

namespace IIASA.FloodCitiSense.Datatypes.Dtos
{
    public class MobilePushNotification : FullAuditedEntityDto, IExtendableObject
    {
        /// <summary>
        ///     Gets or sets the Notification MessageTitle
        /// </summary>
        public string MessageTitle { get; set; }
        /// <summary>
        ///     Gets or sets the Notification Message
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        ///     Gets or sets the Tags, which correspond to countries to which messages are sent
        /// </summary>
        public string Tags { get; set; }
        /// <summary>
        ///     Gets or sets the TenantId
        /// </summary>
        public int? TenantId { get; set; }
        /// <summary>
        ///     Gets or sets the ExtensionData
        /// </summary>
        public string ExtensionData { get; set; }
    }
}