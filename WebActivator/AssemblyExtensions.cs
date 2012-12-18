using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WebActivator
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

        public static IEnumerable<Config> GetConfigTasks(this Assembly assembly)
        {
            List<Config> result = new List<Config>();
            foreach (Type t in assembly.GetTypes())
            {
                if (t.IsSubclassOf(typeof(Config)))
                {
                    Config instance = (Config)Activator.CreateInstance(t);
                    result.Add(instance);
                }
            }
            return result;
        }
    }
}
