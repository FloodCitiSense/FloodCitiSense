using System;
using System.ComponentModel.DataAnnotations;

namespace IIASA.FloodCitiSense.Datatypes.Dtos
{
    public class MobilePushNotificationViewModel
    {
        /// <summary>
        ///     Gets the record Id, during send value is not used.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        ///     Gets the TenantId, during send logged-in session value will be used.
        /// </summary>
        public int? TenantId { get; set; }

        [Required]
        public DateTimeOffset Date { get; set; }
        /// <summary>
        ///     Gets or sets the Notification Message Title
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Message title should be less than 100 characters and minimum 3 characters")]
        public string MessageTitle { get; set; }
        /// <summary>
        ///     Gets or sets the Notification Message
        /// </summary>
        [Required]
        [StringLength(300, MinimumLength = 3, ErrorMessage = "Message title should be less than 300 characters and minimum 3 characters")]
        public string Message { get; set; }
        /// <summary>
        ///     Gets or sets the Tags, tags are created using 2 letter country ISO code and 2 letter Language ISO code, eg "GB-en" for UK and english language.
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Tags should be less than 100 characters and minimum 2 characters")]
        public string Tags { get; set; }
    }
}