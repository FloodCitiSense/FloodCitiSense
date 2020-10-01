using System.Threading.Tasks;
using Abp.Dependency;

namespace IIASA.FloodCitiSense.MultiTenancy.Accounting
{
    public interface IInvoiceNumberGenerator : ITransientDependency
    {
        Task<string> GetNewInvoiceNumber();
    }
}