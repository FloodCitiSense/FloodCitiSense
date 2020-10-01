using Abp.Application.Services;
using Abp.Application.Services.Dto;
using IIASA.FloodCitiSense.MultiTenancy.Dto;
using IIASA.FloodCitiSense.MultiTenancy.Payments.Dto;
using System.Threading.Tasks;

namespace IIASA.FloodCitiSense.MultiTenancy.Payments
{
    public interface IPaymentAppService : IApplicationService
    {
        Task<PaymentInfoDto> GetPaymentInfo(PaymentInfoInput input);

        Task<CreatePaymentResponse> CreatePayment(CreatePaymentDto input);

        Task<ExecutePaymentResponse> ExecutePayment(ExecutePaymentDto input);

        Task<PagedResultDto<SubscriptionPaymentListDto>> GetPaymentHistory(GetPaymentHistoryInput input);
    }
}
