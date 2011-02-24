using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestLibrary;

namespace WebActivatorTest {
    [TestClass]
    public class WebActivatorUnitTest {
        [TestMethod]
        public void TestWebActivatorPreStartMethodsGetCalled() {
            MyStartupCode.StartCalled = MyStartupCode.Start2Called = MyStartupCode.CallMeAfterAppStartCalled = false;

            WebActivator.ActivationManager.RunPreStartMethods();

            Assert.IsTrue(TestLibrary.MyStartupCode.StartCalled);
            Assert.IsTrue(TestLibrary.MyStartupCode.Start2Called);
            Assert.IsFalse(TestLibrary.MyStartupCode.CallMeAfterAppStartCalled);
        }

        [TestMethod]
        public void TestWebActivatorPostStartMethodsGetCalled() {
            MyStartupCode.StartCalled = MyStartupCode.Start2Called = MyStartupCode.CallMeAfterAppStartCalled = false;

            WebActivator.ActivationManager.RunPostStartMethods();

            Assert.IsFalse(TestLibrary.MyStartupCode.StartCalled);
            Assert.IsFalse(TestLibrary.MyStartupCode.Start2Called);
            Assert.IsTrue(TestLibrary.MyStartupCode.CallMeAfterAppStartCalled);
        }
    }
}
