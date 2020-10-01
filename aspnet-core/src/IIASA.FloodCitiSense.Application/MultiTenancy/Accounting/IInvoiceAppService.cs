using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using IIASA.FloodCitiSense.MultiTenancy.Accounting.Dto;

namespace IIASA.FloodCitiSense.MultiTenancy.Accounting
{
    public interface IInvoiceAppService
    {
        Task<InvoiceDto> GetInvoiceInfo(EntityDto<long> input);

        Task CreateInvoice(CreateInvoiceDto input);
    }
}
