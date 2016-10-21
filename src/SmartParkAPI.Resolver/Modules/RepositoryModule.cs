using System.Linq;
using Autofac;
using SmartParkAPI.DataAccess.Common;
using SmartParkAPI.DataAccess.Interfaces;
using Module = Autofac.Module;

namespace SmartParkAPI.Resolver.Modules
{
    public class RepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof (GenericRepository<,>)).As(typeof (IGenericRepository<,>)).InstancePerLifetimeScope().PropertiesAutowired();

            var repositoryAssembly = typeof(IUserRepository).Assembly;
            var repositoryTypes = repositoryAssembly.GetTypes().Where(n => n.IsClass && typeof(IDependencyRepository).IsAssignableFrom(n));

            foreach (var st in repositoryTypes)
            {
                builder.RegisterType(st).AsImplementedInterfaces().InstancePerLifetimeScope().PropertiesAutowired();
            }
        }
    }
}