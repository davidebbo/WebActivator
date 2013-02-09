using System;

namespace WebActivatorEx
{
    // Same as PreApplicationStartMethodAttribute, but for methods to be called when the app shuts down
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class ApplicationShutdownMethodAttribute : BaseActivationMethodAttribute
    {
        public ApplicationShutdownMethodAttribute(Type type, string methodName)
            : base(type, methodName)
        {
        }
    }
}
