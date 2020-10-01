using System.Threading.Tasks;
using Abp;
using Abp.Notifications;
using IIASA.FloodCitiSense.Authorization.Users;
using IIASA.FloodCitiSense.MultiTenancy;

namespace IIASA.FloodCitiSense.Notifications
{
    public interface IAppNotifier
    {
        Task WelcomeToTheApplicationAsync(User user);

        Task NewUserRegisteredAsync(User user);

        Task NewTenantRegisteredAsync(Tenant tenant);

        Task SendMessageAsync(UserIdentifier user, string message, NotificationSeverity severity = NotificationSeverity.Info);
    }
}
