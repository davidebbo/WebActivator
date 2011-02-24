using System;
using System.Reflection;

namespace WebActivator {
    // This attribute is similar to its System.Web namesake, except that
    // it can be used multiple times on an assembly.
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class PreApplicationStartMethodAttribute : BaseApplicationStartMethodAttribute {
        public PreApplicationStartMethodAttribute(Type type, string methodName)
            : base(type, methodName) {
        }
    }
}
