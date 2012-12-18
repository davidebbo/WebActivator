using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WebActivator
{
    internal class WebActivatorHttpModule :IHttpModule
    {
        private static object _lock = new object();
        private static int _initializedModuleCount;

        public void Init(HttpApplication context)
        {

            lock (_lock)
            {
                // Keep track of the number of modules initialized and
                // make sure we only call the post start methods once per app domain
                if (_initializedModuleCount++ == 0)
                {
                    ActivationManager.RunPostStartMethods();
                }
            }
            ActivationManager.RunAttachEventsMethods(context);
        }

        public void Dispose()
        {
            lock (_lock)
            {
                // Call the shutdown methods when the last module is disposed
                if (--_initializedModuleCount == 0)
                {
                    ActivationManager.RunShutdownMethods();
                }
            }
        }
    }
}
