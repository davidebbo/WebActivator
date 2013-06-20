using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WebActivatorEx
{
    static class AssemblyExtensions
    {
        // Return all the attributes of a given type from an assembly
        public static IEnumerable<T> GetActivationAttributes<T>(this Assembly assembly) where T : BaseActivationMethodAttribute
        {
            try
            {
                return assembly.GetCustomAttributes(
                    typeof(T),
                    inherit: false).OfType<T>();
            }
            catch
            {
                // In some very odd (and not well understood) cases, GetCustomAttributes throws. Just ignore it.
                // See https://github.com/davidebbo/WebActivator/issues/12 for details
                return Enumerable.Empty<T>();
            }
        }
    }
}
