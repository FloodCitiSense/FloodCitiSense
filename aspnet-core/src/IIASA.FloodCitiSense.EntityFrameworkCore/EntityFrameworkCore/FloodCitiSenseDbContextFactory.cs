using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using IIASA.FloodCitiSense.Configuration;
using IIASA.FloodCitiSense.Web;

namespace IIASA.FloodCitiSense.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class FloodCitiSenseDbContextFactory : IDesignTimeDbContextFactory<FloodCitiSenseDbContext>
    {
        public FloodCitiSenseDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<FloodCitiSenseDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder(), addUserSecrets: true);

            FloodCitiSenseDbContextConfigurer.Configure(builder, configuration.GetConnectionString(FloodCitiSenseConsts.ConnectionStringName));

            return new FloodCitiSenseDbContext(builder.Options);
        }
    }
}