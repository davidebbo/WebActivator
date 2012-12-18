using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using TestLibrary;
using WebActivator;

namespace TestWebApp.App_Start
{
    public class StartConfig : Config
    {
        /// <summary>
        /// This method is going to be executed before Application_Start():
        /// </summary>
        public override void PreSetup()
        {
            if (MyStartupCode.ConfigStartCalled)
            {
                throw new Exception("Unexpected second call to Start");
            }

            MyStartupCode.ConfigStartCalled = true;
            MyStartupCode.ConfigExecutedOrder += "Start";
        }

        /// <summary>
        /// This method is going to be execute after Application_Start():
        /// </summary>
        public override void Setup()
        {
            Debug.WriteLine("StartConfig::Setup() method called.");
        }

        /// <summary>
        /// This method allows to attach event handlers for an HttpApplication
        /// </summary>
        /// <param name="context"></param>
        public override void AttachEventHandlers(HttpApplication context)
        {
            context.EndRequest += context_EndRequest;
            Debug.WriteLine("StartConfig::AttachEventHandlers() called.");
        }

        void context_EndRequest(object sender, EventArgs e)
        {
            Debug.WriteLine("StartConfig::context_EndRequest() called.");
        }

        /// <summary>
        ///  This method is going to be execute at the end of application
        /// </summary>
        public override void Shutdown()
        {
            Debug.WriteLine("StartConfig::Shutdown() method called.");
        }
    }
}