using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestLibrary;
using WebActivatorEx;

namespace WebActivatorTest
{
    [TestClass]
    public class WebActivatorUnitTest
    {
        [TestInitialize]
        public void TestInit()
        {
            ActivationManager.Reset();
            MyStartupCode.ExecutedOrder = "";
            MyStartupCode.StartCalled = false;
            MyStartupCode.Start2Called = false;
            MyStartupCode.CallMeAfterAppStartCalled = false;
            MyStartupCode.CallMeWhenAppEndsCalled = false;
        }

        [TestMethod]
        public void TestWebActivatorAllStartMethodsGetCalled()
        {
            ActivationManager.Run();

            Assert.IsTrue(TestLibrary.MyStartupCode.StartCalled);
            Assert.IsTrue(TestLibrary.MyStartupCode.Start2Called);
            Assert.IsTrue(TestLibrary.MyStartupCode.CallMeAfterAppStartCalled);
        }

        [TestMethod]
        public void TestWebActivatorPreStartMethodsGetCalled()
        {
            ActivationManager.RunPreStartMethods();

            Assert.IsTrue(TestLibrary.MyStartupCode.StartCalled);
            Assert.IsTrue(TestLibrary.MyStartupCode.Start2Called);
            Assert.IsFalse(TestLibrary.MyStartupCode.CallMeAfterAppStartCalled);
        }

        [TestMethod]
        public void TestWebActivatorPreStartMethodsInDesignerModeGetCalled()
        {
            ActivationManager.RunPreStartMethods(designerMode: true);

            Assert.IsTrue(TestLibrary.MyStartupCode.StartCalled);
            Assert.IsFalse(TestLibrary.MyStartupCode.Start2Called);
            Assert.IsFalse(TestLibrary.MyStartupCode.CallMeAfterAppStartCalled);
        }

        [TestMethod]
        public void TestWebActivatorPostStartMethodsGetCalled()
        {
            ActivationManager.RunPostStartMethods();

            Assert.IsFalse(TestLibrary.MyStartupCode.StartCalled);
            Assert.IsFalse(TestLibrary.MyStartupCode.Start2Called);
            Assert.IsTrue(TestLibrary.MyStartupCode.CallMeAfterAppStartCalled);
        }

        [TestMethod]
        public void TestWebActivatorShutdownMethodsGetCalled()
        {
            ActivationManager.RunShutdownMethods();

            Assert.IsTrue(MyStartupCode.CallMeWhenAppEndsCalled);
        }

        [TestMethod]
        public void TestWebActivatorMethodsCalledBySpecifiedOrder()
        {
            ActivationManager.Run();
            ActivationManager.RunShutdownMethods();
            Assert.AreEqual("StartStart3Start2CallMeAfterAppStartCallMeWhenAppEnds", MyStartupCode.ExecutedOrder);
        }
    }
}
