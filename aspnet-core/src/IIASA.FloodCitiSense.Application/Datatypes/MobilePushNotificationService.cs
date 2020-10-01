using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using IIASA.FloodCitiSense.Authorization;
using IIASA.FloodCitiSense.Datatypes.Dtos;
using IIASA.FloodCitiSense.MobilePushNotification;
using Microsoft.EntityFrameworkCore;

namespace IIASA.FloodCitiSense.Datatypes
{
    /// <summary>
    /// Defines the <see cref="MobilePushNotificationService" />
    /// </summary>
    [AbpAuthorize(AppPermissions.Pages_MobilePushNotification)]
    public class MobilePushNotificationService : FloodCitiSenseAppServiceBase, IMobilePushNotificationService
    {
        private readonly IRepository<MobilePushNotification> _notificationPushRepository;
        private readonly IMobilePushNotification _mobilePushNotification;

        public MobilePushNotificationService(IRepository<MobilePushNotification> notificationPushRepository, IMobilePushNotification mobilePushNotification)
        {
            _notificationPushRepository = notificationPushRepository;
            _mobilePushNotification = mobilePushNotification;
        }

        [AbpAuthorize(AppPermissions.Pages_MobilePushNotification_Send)]
        public async Task<OutputDto> Send(MobilePushNotificationViewModel input)
        {
            if (input == null)
                return await Task.FromResult(new OutputDto
                {
                    Message = "InvalidInput",
                    Success = false
                });

            try
            {
                var tags = input.Tags.Split(";");
                await _mobilePushNotification.Send(input.Message, input.MessageTitle, tags);

                var notification = ObjectMapper.Map<MobilePushNotification>(input);
                notification.Id = 0;
                notification.CreationTime = input.Date.UtcDateTime;
                notification.CreatorUserId = AbpSession.UserId ?? 1;
                notification.TenantId = AbpSession.TenantId ?? 1;
                var newNotification = await _notificationPushRepository.InsertOrUpdateAsync(notification);
                await UnitOfWorkManager.Current.SaveChangesAsync();

                return await Task.FromResult(new OutputDto
                {
                    Message = "CreateSuccess",
                    Success = true,
                    Id = newNotification.Id
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return await Task.FromResult(new OutputDto
                {
                    Message = e.ToString(),
                    Success = false
                });
            }
        }

        [AbpAuthorize(AppPermissions.Pages_MobilePushNotification_Get)]
        public async Task<PagedResultDto<MobilePushNotificationViewModel>> GetByUserId(EntityDto input)
        {
            var query = from o in _notificationPushRepository.GetAll()
                    .Where(x => x.CreatorUserId == AbpSession.UserId)
                select o;

            var totalCount = await query.CountAsync();

            List<MobilePushNotification> mobilePushNotifications = await query.ToListAsync();

            var mobilePushNotificationViewModels = mobilePushNotifications
                .Select(x => ObjectMapper.Map<MobilePushNotificationViewModel>(x)).ToList();

            return new PagedResultDto<MobilePushNotificationViewModel>(
                totalCount,
                mobilePushNotificationViewModels
            );
        }
    }
}