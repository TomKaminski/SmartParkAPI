using System.Linq;
using AutoMapper;

namespace SmartParkAPI.Shared.Helpers
{
    public static class AutoMapperExtensions
    {
        public static void IgnoreNotExistingProperties<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression)
        {
            //var existingMaps = Mapper.Configuration.GetAllTypeMaps().FirstOrDefault(x => x.SourceType == typeof(TSource) && x.DestinationType == typeof(TDestination));

            //if (existingMaps == null) return;
            //foreach (var property in existingMaps.GetUnmappedPropertyNames())
            //{
            //    expression.ForMember(property, opt => opt.Ignore());
            //}
        }
    }
}
