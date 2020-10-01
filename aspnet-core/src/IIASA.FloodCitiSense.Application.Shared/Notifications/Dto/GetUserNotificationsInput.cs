using Abp.Notifications;
using IIASA.FloodCitiSense.Dto;

namespace IIASA.FloodCitiSense.Notifications.Dto
{
    public class GetUserNotificationsInput : PagedInputDto
    {
        public UserNotificationState? State { get; set; }
    }
}