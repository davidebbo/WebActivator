using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using TestLibrary;
using WebActivator;

namespace TestWebApp.App_Start
{
    public class Start2Config : Config
    {
        public override EPriority Priority
        {
            get
            {
                //Setting priority to very low level, so all code will be executed AFTER config tasks with higher priority
                return EPriority.VeryLow;
            }
        }

        /// <summary>
        /// This method is going to be executed before Application_Start():
        /// </summary>
        public override void PreSetup()
        {
            if (MyStartupCode.ConfigStart2Called)
            {
                throw new Exception("Unexpected second call to Start2");
            }

            MyStartupCode.ConfigStart2Called = true;
            MyStartupCode.ConfigExecutedOrder += "Start2";
        }

        /// <summary>
        /// This method is going to be execute after Application_Start():
        /// </summary>
        public override void Setup()
        {
            Debug.WriteLine("Start2Config::Setup() method called.");
        }

        /// <summary>
        /// This method allows to attach event handlers for an HttpApplication
        /// </summary>
        /// <param name="context"></param>
        public override void AttachEventHandlers(HttpApplication context)
        {
            context.EndRequest += context_EndRequest;
            Debug.WriteLine("Start2Config::AttachEventHandlers() called.");
        }

        void context_EndRequest(object sender, EventArgs e)
        {
            Debug.WriteLine("Start2Config::context_EndRequest() called.");
        }

        /// <summary>
        ///  This method is going to be execute at the end of application
        /// </summary>
        public override void Shutdown()
        {
            Debug.WriteLine("Start2Config::Shutdown() method called.");
        }
    }
}