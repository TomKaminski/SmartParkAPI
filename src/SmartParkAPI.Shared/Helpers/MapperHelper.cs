using System.Linq;

namespace SmartParkAPI.Shared.Helpers
{
      public static class MapperHelper<TIn,TOut> 
        where TIn : class
        where TOut : class
    {
        public static TOut MapNoIdToEntityOnEdit(TIn model, TOut entity)
        {
            foreach (var propertyInfo in model.GetType().GetProperties().Where(propertyInfo => propertyInfo.Name.ToLower() != "id"))
            {
                entity.GetType().GetProperty(propertyInfo.Name).SetValue(entity, propertyInfo.GetValue(model));
            }
            return entity;
        }
    }
}
