using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace WebActivator {
    static class AssemblyExtensions {
        public static IEnumerable<PreApplicationStartMethodAttribute> GetPreAppStartAttributes(this Assembly assembly) {
            // Go through all the PreApplicationStartMethodAttribute attributes
            // Note that this is *our* attribute, not the System.Web namesake

            return assembly.GetCustomAttributes(
                typeof(PreApplicationStartMethodAttribute),
                inherit: false).OfType<PreApplicationStartMethodAttribute>();
        }

        public static IEnumerable<PostApplicationStartMethodAttribute> GetPostAppStartAttributes(this Assembly assembly) {
            return assembly.GetCustomAttributes(
                typeof(PostApplicationStartMethodAttribute),
                inherit: false).OfType<PostApplicationStartMethodAttribute>();
        }
    }
}
