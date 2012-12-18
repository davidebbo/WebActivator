using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestLibrary;

namespace WebActivatorTest
{
    [TestClass]
    public class WebActivatorUnitTest : WebActivatorTestBase
    {
        [TestMethod]
        public void TestWebActivatorAllStartMethodsGetCalled()
        {
            WebActivator.ActivationManager.Run();

            Assert.IsTrue(TestLibrary.MyStartupCode.StartCalled);
            Assert.IsTrue(TestLibrary.MyStartupCode.Start2Called);
            Assert.IsTrue(TestLibrary.MyStartupCode.CallMeAfterAppStartCalled);
        }

        [TestMethod]
        public void TestWebActivatorPreStartMethodsGetCalled()
        {
            WebActivator.ActivationManager.RunPreStartMethods();

            Assert.IsTrue(TestLibrary.MyStartupCode.StartCalled);
            Assert.IsTrue(TestLibrary.MyStartupCode.Start2Called);
            Assert.IsFalse(TestLibrary.MyStartupCode.CallMeAfterAppStartCalled);
        }

        [TestMethod]
        public void TestWebActivatorPostStartMethodsGetCalled()
        {
            WebActivator.ActivationManager.RunPostStartMethods();

            Assert.IsFalse(TestLibrary.MyStartupCode.StartCalled);
            Assert.IsFalse(TestLibrary.MyStartupCode.Start2Called);
            Assert.IsTrue(TestLibrary.MyStartupCode.CallMeAfterAppStartCalled);
        }

        [TestMethod]
        public void TestWebActivatorShutdownMethodsGetCalled()
        {
            WebActivator.ActivationManager.RunShutdownMethods();

            Assert.IsTrue(MyStartupCode.CallMeWhenAppEndsCalled);
        }

        [TestMethod]
        public void TestWebActivatorMethodsCalledBySpecifiedOrder()
        {
            WebActivator.ActivationManager.Run();
            WebActivator.ActivationManager.RunShutdownMethods();
            Assert.AreEqual("StartStart3Start2CallMeAfterAppStartCallMeWhenAppEnds", MyStartupCode.ExecutedOrder);
        }
    }
}
