using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace IIASA.FloodCitiSense.EntityFrameworkCore
{
    public static class FloodCitiSenseDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<FloodCitiSenseDbContext> builder, string connectionString)
        {
            builder.UseNpgsql(connectionString, o => o.UseNetTopologySuite());
            //.ConfigureWarnings(warnings => warnings.Throw(CoreEventId.IncludeIgnoredWarning));;
        }

        public static void Configure(DbContextOptionsBuilder<FloodCitiSenseDbContext> builder, DbConnection connection)
        {
            builder.UseNpgsql(connection);
        }
    }
}