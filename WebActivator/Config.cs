using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WebActivator
{
    /// <summary>
    /// Abstract class for WebActivator config task
    /// </summary>
    public abstract class Config
    {
        /// <summary>
        /// Priority of particular config
        /// </summary>
        public virtual EPriority Priority { get { return EPriority.Normal; } }

        /// <summary>
        /// Performs config task pre-setup (code being run once per application domain before Application_Start() method)
        /// </summary>
        public virtual void PreSetup() { return; }

        /// <summary>
        /// Performs config task setup (code being run once per application domain after Application_Start() method)
        /// </summary>
        public virtual void Setup() { return; }

        /// <summary>
        /// Allows to attach event handlers for an HttpApplication (code being run as many times as HttpApplication instance occurs)
        /// </summary>
        /// <param name="context"></param>
        public virtual void AttachEventHandlers(HttpApplication context) { return; }

        /// <summary>
        /// Performs config task when application is being shut down (code being run once per application domain when web application is being disposed)
        /// </summary>
        public virtual void Shutdown() { return; }
    }
}
