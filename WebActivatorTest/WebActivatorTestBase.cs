using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestLibrary;

namespace WebActivatorTest
{
    [TestClass]
    public abstract class WebActivatorTestBase
    {
        [TestInitialize]
        public void TestInit()
        {
            WebActivator.ActivationManager.Reset();
            MyStartupCode.StartCalled = false;
            MyStartupCode.Start2Called = false;
            MyStartupCode.CallMeAfterAppStartCalled = false;
            MyStartupCode.CallMeWhenAppEndsCalled = false;
            MyStartupCode.ExecutedOrder = "";
            MyStartupCode.ConfigStartCalled = false;
            MyStartupCode.ConfigStart2Called = false;
            MyStartupCode.ConfigCallMeAfterAppStartCalled = false;
            MyStartupCode.ConfigCallMeWhenAppEndsCalled = false;
            MyStartupCode.ConfigExecutedOrder = "";
        }
    }
}
