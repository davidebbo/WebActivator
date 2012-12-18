using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using TestLibrary;
using WebActivator;

namespace TestWebApp.App_Start
{
    public class CallMeWhenAppEndsConfig : Config
    {
        /// <summary>
        ///  This method is going to be execute at the end of application
        /// </summary>
        public override void Shutdown()
        {
            Debug.WriteLine("CallMeWhenAppEndsConfig::Shutdown() method called.");

            if (MyStartupCode.ConfigCallMeWhenAppEndsCalled)
            {
                throw new Exception("Unexpected second call to CallMeWhenAppEndsConfig");
            }

            MyStartupCode.ConfigCallMeWhenAppEndsCalled = true;
            MyStartupCode.ConfigExecutedOrder += "CallMeWhenAppEnds";
        }
    }
}