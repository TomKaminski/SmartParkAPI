using System;
using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SmartParkAPI.Mappings;
using SmartParkAPI.Model;
using SmartParkAPI.Models.Auth;
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
                cfg.AddProfile(new UserDeviceBackendMappings());
            });
            _mapper = config.CreateMapper();
        }

        private readonly IMapper _mapper;

        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("AUTH_SECRET_KEY")));
        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddOptions();

            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });


            // Use policy auth.
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminUser", policy => policy.RequireClaim("isAdmin", "True"));
            });


            // Get options from app settings
            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));

            // Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });

            //#if DEBUG
            //            services.AddDbContext<ParkingAthContext>(options =>
            //                options.UseSqlServer(Configuration["Data:DefaultConnection:LocalConnectionString"]));
            //#else
            //            services.AddEntityFramework()
            //                .AddSqlServer()
            //                .AddDbContext<ParkingAthContext>(options =>
            //                    options.UseSqlServer(Configuration["Data:DefaultConnection:AzureConnectionString"]));
            //#endif

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

            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,

                RequireExpirationTime = true,
                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = tokenValidationParameters
            });

            app.UseMvc();
        }
    }
}
