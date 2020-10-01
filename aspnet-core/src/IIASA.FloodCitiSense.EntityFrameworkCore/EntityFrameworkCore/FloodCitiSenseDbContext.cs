using Abp.IdentityServer4;
using Abp.Localization;
using Abp.Zero.EntityFrameworkCore;
using IIASA.FloodCitiSense.Authorization.Roles;
using IIASA.FloodCitiSense.Authorization.Users;
using IIASA.FloodCitiSense.Chat;
using IIASA.FloodCitiSense.Datatypes;
using IIASA.FloodCitiSense.DataTypes;
using IIASA.FloodCitiSense.Editions;
using IIASA.FloodCitiSense.Friendships;
using IIASA.FloodCitiSense.MultiTenancy;
using IIASA.FloodCitiSense.MultiTenancy.Accounting;
using IIASA.FloodCitiSense.MultiTenancy.Payments;
using IIASA.FloodCitiSense.Playground;
using IIASA.FloodCitiSense.Storage;
using Microsoft.EntityFrameworkCore;

namespace IIASA.FloodCitiSense.EntityFrameworkCore
{
    public class FloodCitiSenseDbContext : AbpZeroDbContext<Tenant, Role, User, FloodCitiSenseDbContext>,
        IAbpPersistedGrantDbContext
    {
        public FloodCitiSenseDbContext(DbContextOptions<FloodCitiSenseDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<City> Cities { get; set; }

        public virtual DbSet<Sensor> Sensors { get; set; }

        public virtual DbSet<IncidentApproval> IncidentApprovals { get; set; }

        public virtual DbSet<Incident> Incidents { get; set; }

        public virtual DbSet<MobilePushNotification> MobilePushNotifications { get; set; }

        public virtual DbSet<Location> Locations { get; set; }

        public virtual DbSet<Picture> Pictures { get; set; }

        public virtual DbSet<FloodType> FloodTypes { get; set; }

        public virtual DbSet<CreativeEntiy> CreativeEntiies { get; set; }

        /* Define an IDbSet for each entity of the application */

        public virtual DbSet<BinaryObject> BinaryObjects { get; set; }

        public virtual DbSet<Friendship> Friendships { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }

        public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }

        public virtual DbSet<PersistedGrantEntity> PersistedGrants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasPostgresExtension("postgis");
            modelBuilder.Entity<City>(c => c.HasIndex(e => new {e.Id}));

            modelBuilder.Entity<User>(u => u.HasOne(c => c.City));

            modelBuilder.Entity<Sensor>(i =>
            {
                i.HasIndex(e => new {e.TenantId});
                i.HasMany(p => p.Pictures);
                i.HasMany(l => l.Locations);
            });

            modelBuilder.Entity<Incident>(i =>
            {
                i.HasMany(p => p.Pictures);
                i.HasMany(l => l.Locations);
            });

            modelBuilder.Entity<Picture>(i =>
            {
                i.HasOne(p => p.Incident);
                i.HasOne(l => l.Sensor);
            });

            modelBuilder.Entity<Location>(i =>
            {
                i.HasOne(p => p.Incident);
                i.HasOne(l => l.Sensor);
            });

            modelBuilder.Entity<ApplicationLanguageText>()
                .Property(p => p.Value)
                .HasMaxLength(100); // any integer that is smaller than 10485760

            modelBuilder.Entity<BinaryObject>(b => { b.HasIndex(e => new {e.TenantId}); });

            modelBuilder.Entity<ChatMessage>(b =>
            {
                b.HasIndex(e => new {e.TenantId, e.UserId, e.ReadState});
                b.HasIndex(e => new {e.TenantId, e.TargetUserId, e.ReadState});
                b.HasIndex(e => new {e.TargetTenantId, e.TargetUserId, e.ReadState});
                b.HasIndex(e => new {e.TargetTenantId, e.UserId, e.ReadState});
            });

            modelBuilder.Entity<Friendship>(b =>
            {
                b.HasIndex(e => new {e.TenantId, e.UserId});
                b.HasIndex(e => new {e.TenantId, e.FriendUserId});
                b.HasIndex(e => new {e.FriendTenantId, e.UserId});
                b.HasIndex(e => new {e.FriendTenantId, e.FriendUserId});
            });

            modelBuilder.Entity<Tenant>(b =>
            {
                b.HasIndex(e => new {e.SubscriptionEndDateUtc});
                b.HasIndex(e => new {e.CreationTime});
            });

            modelBuilder.Entity<SubscriptionPayment>(b =>
            {
                b.HasIndex(e => new {e.Status, e.CreationTime});
                b.HasIndex(e => new {e.PaymentId, e.Gateway});
            });

            modelBuilder.ConfigurePersistedGrantEntity();
        }
    }
}