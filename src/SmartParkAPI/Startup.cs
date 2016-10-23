using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmartParkAPI.Mappings;
using SmartParkAPI.Model;
using SmartParkAPI.Resolver.Mappings;
using SmartParkAPI.Resolver.Modules;

namespace SmartParkAPI
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AccountFrontendMappings());
                cfg.AddProfile(new FrontendMappings());
                cfg.AddProfile(new GateUsageBackendMappings());
                cfg.AddProfile(new WeatherBackendMappings());
                cfg.AddProfile(new MessageBackendMappings());
                cfg.AddProfile(new OrderBackendMappings());
                cfg.AddProfile(new UserBackendMappings());
                cfg.AddProfile(new TokenBackendMappings());
                cfg.AddProfile(new PriceTresholdBackendMappings());
                cfg.AddProfile(new PortalMessageBackendMappings());
                cfg.AddProfile(new AdminMappingsProfile());
            });
            _mapper = config.CreateMapper();
        }

        private readonly IMapper _mapper;
        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

#if DEBUG
            services.AddDbContext<ParkingAthContext>(options =>
                options.UseSqlServer(Configuration["Data:DefaultConnection:LocalConnectionString"]));
#else
            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<ParkingAthContext>(options =>
                    options.UseSqlServer(Configuration["Data:DefaultConnection:AzureConnectionString"]));
#endif

            // Create the Autofac container builder.
            var builder = new ContainerBuilder();
            builder.Register(x => _mapper).As<IMapper>().SingleInstance();
            builder.RegisterModule(new ContractModule());
            builder.RegisterModule(new EfModule());
            builder.RegisterModule(new RepositoryModule());

            // Build the container
            builder.Populate(services);
            var container = builder.Build();



            // Resolve and return the service provider.
            return container.Resolve<IServiceProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            
            app.UseMvc();
        }
    }
}
