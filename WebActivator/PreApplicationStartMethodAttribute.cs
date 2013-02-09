using System;

namespace WebActivatorEx
{
    // This attribute is similar to its System.Web namesake, except that
    // it can be used multiple times on an assembly.
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class PreApplicationStartMethodAttribute : BaseActivationMethodAttribute
    {
        public PreApplicationStartMethodAttribute(Type type, string methodName)
            : base(type, methodName)
        {
        }

        // Set this to true to have the method run in designer mode (in addition to running at runtime)
        public bool RunInDesigner { get; set; }

        public override bool ShouldRunInDesignerMode()
        {
            return RunInDesigner;
        }
    }
}
