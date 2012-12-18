using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using TestLibrary;
using WebActivator;

namespace TestWebApp.App_Start
{
    public class CallMeAfterAppStartConfig : Config
    {
        /// <summary>
        /// This method is going to be execute after Application_Start():
        /// </summary>
        public override void Setup()
        {
            Debug.WriteLine("CallMeAfterAppStartConfig::Setup() method called.");

            if (MyStartupCode.ConfigCallMeAfterAppStartCalled)
            {
                throw new Exception("Unexpected second call to CallMeAfterAppStart");
            }

            MyStartupCode.ConfigCallMeAfterAppStartCalled = true;
            MyStartupCode.ConfigExecutedOrder += "CallMeAfterAppStart";
        }
    }
}