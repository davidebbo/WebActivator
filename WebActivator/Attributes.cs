using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WebActivator
{
    // Base class of all the activation attributes
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public abstract class BaseActivationMethodAttribute : Attribute
    {
        private Type _type;
        private string _methodName;

        public BaseActivationMethodAttribute(Type type, string methodName)
        {
            _type = type;
            _methodName = methodName;
        }

        public Type Type
        {
            get
            {
                return _type;
            }
        }

        public string MethodName
        {
            get
            {
                return _methodName;
            }
        }

        public int Order { get; set; }


        public void InvokeMethod()
        {
            // Get the method
            MethodInfo method = Type.GetMethod(
                MethodName,
                BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);

            if (method == null)
            {
                throw new ArgumentException(
                    String.Format("The type {0} doesn't have a static method named {1}",
                        Type, MethodName));
            }

            // Invoke it
            method.Invoke(null, null);
        }
    }

    // This attribute is similar to its System.Web namesake, except that
    // it can be used multiple times on an assembly.
    [Obsolete("Create class that iherits from WebActivator.Config and override method PreSetup()")]
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class PreApplicationStartMethodAttribute : BaseActivationMethodAttribute
    {
        public PreApplicationStartMethodAttribute(Type type, string methodName)
            : base(type, methodName)
        {
        }
    }

    // Same as PreApplicationStartMethodAttribute, but for methods to be called after App_Start
    [Obsolete("Create class that iherits from WebActivator.Config and define method Setup()")]
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class PostApplicationStartMethodAttribute : BaseActivationMethodAttribute
    {
        public PostApplicationStartMethodAttribute(Type type, string methodName)
            : base(type, methodName)
        {
        }
    }

    // Same as PreApplicationStartMethodAttribute, but for methods to be called when the app shuts down
    [Obsolete("Create class that iherits from WebActivator.Config and override method Shutdown()")]
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class ApplicationShutdownMethodAttribute : BaseActivationMethodAttribute
    {
        public ApplicationShutdownMethodAttribute(Type type, string methodName)
            : base(type, methodName)
        {
        }
    }
}
