using System;
using System.Reflection;

namespace WebActivator {
    // This attribute is similar to its System.Web namesake, except that
    // it can be used multiple times on an assembly.
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class PreApplicationStartMethodAttribute : Attribute {
        private Type _type;
        private string _methodName;

        public PreApplicationStartMethodAttribute(Type type, string methodName) {
            _type = type;
            _methodName = methodName;
        }

        public Type Type {
            get {
                return _type;
            }
        }

        public string MethodName {
            get {
                return _methodName;
            }
        }

        public void InvokeMethod() {
            // Get the method
            MethodInfo method = Type.GetMethod(
                MethodName,
                BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);

            if (method == null) {
                throw new ArgumentException(
                    String.Format("The type {0} doesn't have a static method named {1}",
                        Type, MethodName));
            }

            // Invoke it
            method.Invoke(null, null);
        }
    }
}
