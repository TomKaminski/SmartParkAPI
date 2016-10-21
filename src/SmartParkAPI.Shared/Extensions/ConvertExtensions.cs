using System.ComponentModel;

namespace SmartParkAPI.Shared.Extensions
{
    public static class ConvertExtensions
    {
        public static T? ToNullable<T>(this string s) where T : struct
        {
            var result = new T?();
            try
            {
                if (!string.IsNullOrEmpty(s) && s.Trim().Length > 0)
                {
                    var conv = TypeDescriptor.GetConverter(typeof(T));
                    result = (T)conv.ConvertFrom(s);
                }
            }
            catch
            {
                // ignored
            }
            return result;
        }
    }
}
