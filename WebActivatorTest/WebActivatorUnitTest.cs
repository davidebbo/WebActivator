using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebActivatorTest {
    [TestClass]
    public class WebActivatorUnitTest {
        [TestMethod]
        public void TestWebActivatorMethodsGetCalled() {
            WebActivator.ActivationManager.Run();

            Assert.IsTrue(TestLibrary.MyStartupCode.AllStartMethodsWereCalled());
        }
    }
}
