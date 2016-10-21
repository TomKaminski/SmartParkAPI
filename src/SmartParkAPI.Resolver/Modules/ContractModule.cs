using System.Linq;
using System.Net.Mail.Abstractions;
using Autofac;
using SmartParkAPI.Business.Providers.Chart;
using SmartParkAPI.Business.Services;
using SmartParkAPI.Business.Services.Base;
using SmartParkAPI.Contracts.Providers.Chart;
using SmartParkAPI.Contracts.Services.Base;
using SmartParkAPI.Shared.Helpers;
using SmtpClient = System.Net.Mail.Abstractions.SmtpClient;

namespace SmartParkAPI.Resolver.Modules
{
    public class ContractModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(EntityService<,,>)).As(typeof(IEntityService<,>)).InstancePerLifetimeScope().PropertiesAutowired();

            var repositoryAssembly = typeof(UserService).Assembly;
            var serviceTypes = repositoryAssembly.GetTypes().Where(n => n.IsClass && typeof(IDependencyService).IsAssignableFrom(n));

            foreach (var st in serviceTypes)
            {
                builder.RegisterType(st).AsImplementedInterfaces().InstancePerLifetimeScope().PropertiesAutowired();
            }

            builder.RegisterType<PasswordHasher>().As<IPasswordHasher>().InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(CustomExpressionVisitor<>))
                .As(typeof(ICustomExpressionVisitor<>))
                .InstancePerLifetimeScope();

            builder.RegisterType<SmtpClient>().As<ISmtpClient>().InstancePerLifetimeScope();

            builder.RegisterType<GateUsagesChartDataProvider>()
                .As<IGateUsagesChartDataProvider>()
                .InstancePerLifetimeScope();

            builder.RegisterType<OrdersDataChartProvider>()
                .As<IOrdersChartDataProvider>()
                .InstancePerLifetimeScope();
        }
    }
}
