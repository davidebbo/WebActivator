using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using TestLibrary;
using WebActivator;

namespace TestWebApp.App_Start
{
    public class Start3Config : Config
    {
        public override EPriority Priority
        {
            get
            {
                //Setting priority to very low level, so all code will be executed AFTER config tasks with higher priority
                return EPriority.Low;
            }
        }

        /// <summary>
        /// This method is going to be executed before Application_Start():
        /// </summary>
        public override void PreSetup()
        {
            MyStartupCode.ConfigStartCalled = true;
            MyStartupCode.ConfigExecutedOrder += "Start3";
        }

        /// <summary>
        /// This method is going to be execute after Application_Start():
        /// </summary>
        public override void Setup()
        {
            Debug.WriteLine("Start3Config::Setup() method called.");
        }

        /// <summary>
        /// This method allows to attach event handlers for an HttpApplication
        /// </summary>
        /// <param name="context"></param>
        public override void AttachEventHandlers(HttpApplication context)
        {
            context.EndRequest += context_EndRequest;
            Debug.WriteLine("Start3Config::AttachEventHandlers() called.");
        }

        void context_EndRequest(object sender, EventArgs e)
        {
            Debug.WriteLine("Start3Config::context_EndRequest() called.");
        }

        /// <summary>
        ///  This method is going to be execute at the end of application
        /// </summary>
        public override void Shutdown()
        {
            Debug.WriteLine("Start3Config::Shutdown() method called.");
        }
    }
}