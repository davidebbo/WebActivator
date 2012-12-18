using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestLibrary;

namespace WebActivatorTest
{
    [TestClass]
    public class WebActivatorConfigUnitTest : WebActivatorTestBase
    {
        [TestMethod]
        public void TestWebActivatorAllStartMethodsGetCalled()
        {
            WebActivator.ActivationManager.Run();

            Assert.IsTrue(MyStartupCode.ConfigStartCalled);
            Assert.IsTrue(MyStartupCode.ConfigStart2Called);
            Assert.IsTrue(MyStartupCode.ConfigCallMeAfterAppStartCalled);
        }

        [TestMethod]
        public void TestWebActivatorPreStartMethodsGetCalled()
        {
            WebActivator.ActivationManager.RunPreStartMethods();

            Assert.IsTrue(MyStartupCode.ConfigStartCalled);
            Assert.IsTrue(MyStartupCode.ConfigStart2Called);
            Assert.IsFalse(MyStartupCode.ConfigCallMeAfterAppStartCalled);
        }

        [TestMethod]
        public void TestWebActivatorPostStartMethodsGetCalled()
        {
            WebActivator.ActivationManager.RunPostStartMethods();

            Assert.IsFalse(MyStartupCode.ConfigStartCalled);
            Assert.IsFalse(MyStartupCode.ConfigStart2Called);
            Assert.IsTrue(MyStartupCode.ConfigCallMeAfterAppStartCalled);
        }

        [TestMethod]
        public void TestWebActivatorShutdownMethodsGetCalled()
        {
            WebActivator.ActivationManager.RunShutdownMethods();

            Assert.IsTrue(MyStartupCode.ConfigCallMeWhenAppEndsCalled);
        }

        [TestMethod]
        public void TestWebActivatorMethodsCalledBySpecifiedOrder()
        {
            WebActivator.ActivationManager.Run();
            WebActivator.ActivationManager.RunShutdownMethods();
            Assert.AreEqual("StartStart3Start2CallMeAfterAppStartCallMeWhenAppEnds", MyStartupCode.ConfigExecutedOrder);
        }
    }
}
