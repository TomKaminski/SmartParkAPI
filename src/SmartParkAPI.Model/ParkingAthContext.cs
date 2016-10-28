using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SmartParkAPI.Model.Concrete;

namespace SmartParkAPI.Model
{
    public class ParkingAthContext : DbContext
    {
        private readonly bool _useInMemory;

        public ParkingAthContext(bool useInMemory = false)
        {
            _useInMemory = useInMemory;
        }

        public ParkingAthContext(DbContextOptions options) : base(options)
        {
            _useInMemory = false;
        }


        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserPreferences> UserPreferences { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<GateUsage> GateUsage { get; set; }
        public virtual DbSet<PriceTreshold> PriceTreshold { get; set; }
        public virtual DbSet<Token> Token { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<PortalMessage> PortalMessage { get; set; }
        public virtual DbSet<Weather> Weather { get; set; }
        public virtual DbSet<WeatherInfo> WeatherInfo { get; set; }
        public virtual DbSet<UserDevice> UserDevice { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_useInMemory)
            {
                optionsBuilder.UseInMemoryDatabase();
            }
            else
            {
#if DEBUG
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=ParkingATHWeb.Data;Trusted_Connection=True;MultipleActiveResultSets=true");
#else
                optionsBuilder.UseSqlServer(@"Server=tcp:smartpark.database.windows.net,1433;Database=SmartPark;User ID=smartpark@smartpark;Password=J5cdmwg6tpm1;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30");
#endif
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>()
                .HasMany(x => x.GateUsages)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);

            modelBuilder.Entity<User>()
                .HasOne(x => x.PasswordChangeToken)
                .WithOne(x => x.User)
                .HasForeignKey<User>(x => x.PasswordChangeTokenId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasOne(x => x.UserPreferences)
                .WithOne(x => x.User)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PriceTreshold>()
                .HasMany(x => x.Orders)
                .WithOne(x => x.PriceTreshold)
                .HasForeignKey(x => x.PriceTresholdId);


            modelBuilder.Entity<User>()
                .HasMany(x => x.Orders)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);

            modelBuilder.Entity<User>()
                .HasMany(x => x.UserDevices)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);

            modelBuilder.Entity<Order>()
                .HasOne(x => x.PriceTreshold)
                .WithMany(x => x.Orders)
                .HasPrincipalKey(x => x.Id);


            modelBuilder.Entity<Message>()
                .HasOne(x => x.User)
                .WithMany(x => x.UserMessages)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Weather>().HasMany(x => x.WeatherInfo).WithOne(x => x.Weather).HasForeignKey(x => x.WeatherId);

            //modelBuilder.Entity<PortalMessage>().HasOne(x => x.User).WithMany(x => x.UserPortalMessages).OnDelete(DeleteBehavior.Restrict).IsRequired(true);
            //modelBuilder.Entity<PortalMessage>().HasOne(x => x.ReceiverUser).WithMany(x => x.UserPortalMessages).OnDelete(DeleteBehavior.Restrict).IsRequired(false);

            base.OnModelCreating(modelBuilder);

        }
    }
}
