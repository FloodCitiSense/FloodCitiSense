using IIASA.FloodCitiSense.EntityFrameworkCore;

namespace IIASA.FloodCitiSense.Tests.TestDatas
{
    public class TestDataBuilder
    {
        private readonly FloodCitiSenseDbContext _context;
        private readonly int _tenantId;

        public TestDataBuilder(FloodCitiSenseDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            new TestOrganizationUnitsBuilder(_context, _tenantId).Create();
            new TestSubscriptionPaymentBuilder(_context, _tenantId).Create();

            _context.SaveChanges();
        }
    }
}
