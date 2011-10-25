using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestLibrary;

namespace WebActivatorTest
{
    [TestClass]
    public class WebActivatorUnitTest
    {
        [TestMethod]
        public void TestWebActivatorAllStartMethodsGetCalled()
        {
            MyStartupCode.StartCalled = MyStartupCode.Start2Called = MyStartupCode.CallMeAfterAppStartCalled = false;

            WebActivator.ActivationManager.Run();

            Assert.IsTrue(TestLibrary.MyStartupCode.StartCalled);
            Assert.IsTrue(TestLibrary.MyStartupCode.Start2Called);
            Assert.IsTrue(TestLibrary.MyStartupCode.CallMeAfterAppStartCalled);
        }

        [TestMethod]
        public void TestWebActivatorPreStartMethodsGetCalled()
        {
            MyStartupCode.StartCalled = MyStartupCode.Start2Called = MyStartupCode.CallMeAfterAppStartCalled = false;

            WebActivator.ActivationManager.RunPreStartMethods();

            Assert.IsTrue(TestLibrary.MyStartupCode.StartCalled);
            Assert.IsTrue(TestLibrary.MyStartupCode.Start2Called);
            Assert.IsFalse(TestLibrary.MyStartupCode.CallMeAfterAppStartCalled);
        }

        [TestMethod]
        public void TestWebActivatorPostStartMethodsGetCalled()
        {
            MyStartupCode.StartCalled = MyStartupCode.Start2Called = MyStartupCode.CallMeAfterAppStartCalled = false;

            WebActivator.ActivationManager.RunPostStartMethods();

            Assert.IsFalse(TestLibrary.MyStartupCode.StartCalled);
            Assert.IsFalse(TestLibrary.MyStartupCode.Start2Called);
            Assert.IsTrue(TestLibrary.MyStartupCode.CallMeAfterAppStartCalled);
        }

        [TestMethod]
        public void TestWebActivatorShutdownMethodsGetCalled()
        {
            MyStartupCode.CallMeWhenAppEndsCalled = false;

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