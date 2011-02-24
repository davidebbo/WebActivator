using System;
using System.Reflection;

namespace WebActivator {
    // Same as PreApplicationStartMethodAttribute, but for methods to be called after App_Start
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class PostApplicationStartMethodAttribute : BaseApplicationStartMethodAttribute {
        public PostApplicationStartMethodAttribute(Type type, string methodName)
            : base(type, methodName) {
        }
    }
}
