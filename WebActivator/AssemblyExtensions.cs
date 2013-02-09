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
            return assembly.GetCustomAttributes(
                typeof(T),
                inherit: false).OfType<T>();
        }
    }
}
