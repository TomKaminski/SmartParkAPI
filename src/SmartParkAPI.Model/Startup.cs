//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Infrastructure;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;

//namespace SmartParkAPI.Model
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//        }
//    }

//    public class TemporaryDbContextFactory : IDbContextFactory<ParkingAthContext>
//    {
//        public ParkingAthContext Create(DbContextFactoryOptions options)
//        {
//            var builder = new DbContextOptionsBuilder<ParkingAthContext>();
//            builder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=ParkingATHWeb.Data;Trusted_Connection=True;MultipleActiveResultSets=true");
//            return new ParkingAthContext(builder.Options);
//        }
//    }

//    public class Startup
//    {
//        public IConfigurationRoot Configuration { get; set; }

//        public Startup(IHostingEnvironment env)
//        {
//            var builder = new ConfigurationBuilder()
//                .SetBasePath(env.ContentRootPath)
//                .AddJsonFile("appsettings.json");
//            Configuration = builder.Build();
//        }

//        public void ConfigureServices(IServiceCollection services)
//        {
//#if DEBUG
//            services.AddDbContext<ParkingAthContext>(options =>
//                    options.UseSqlServer(Configuration["Data:DefaultConnection:LocalConnectionString"]));
//#else
//                services.AddDbContext<ParkingAthContext>(options =>
//                        options.UseSqlServer(Configuration["Data:DefaultConnection:AzureConnectionString"]));
//#endif
//        }

//        public void Configure(IApplicationBuilder app)
//        {
//        }
//    }
//}
