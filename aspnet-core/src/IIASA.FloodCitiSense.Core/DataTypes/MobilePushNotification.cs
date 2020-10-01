using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace IIASA.FloodCitiSense.Datatypes
{
    [Table("MobilePushNotification")]
    public class MobilePushNotification : FullAuditedEntity, IMayHaveTenant, IExtendableObject
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