using System;
using System.Reflection;

namespace WebActivator {
    // This attribute is similar to its System.Web namesake, except that
    // it can be used multiple times on an assembly.
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class PreApplicationStartMethodAttribute : Attribute {
        private Type _type;
        private string _methodName;
        private bool _callAfterGlobalAppStart;

        public PreApplicationStartMethodAttribute(Type type, string methodName)
            : this(type, methodName, false) {
        }

        public PreApplicationStartMethodAttribute(Type type, string methodName, bool callAfterGlobalAppStart) {
            _type = type;
            _methodName = methodName;
            _callAfterGlobalAppStart = callAfterGlobalAppStart;
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

        public bool CallAfterGlobalAppStart {
            get {
                return _callAfterGlobalAppStart;
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
