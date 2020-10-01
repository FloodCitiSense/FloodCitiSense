using IIASA.FloodCitiSense.EntityFrameworkCore;

namespace IIASA.FloodCitiSense.Migrations.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly FloodCitiSenseDbContext _context;

        public InitialHostDbBuilder(FloodCitiSenseDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            new DefaultEditionCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();

            _context.SaveChanges();
        }
    }
}
