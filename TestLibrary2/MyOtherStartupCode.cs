
using System;
using TestLibrary;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(TestLibrary2.MyOtherStartupCode), "Start", Order = 2)]
[assembly: PreApplicationStartMethod(typeof(TestLibrary2.MyOtherStartupCode), "Start2", Order = 4)]


namespace TestLibrary2
{
    public static class MyOtherStartupCode
    {
        public static bool StartCalled { get; set; }
        public static bool Start2Called { get; set; }

        internal static void Start()
        {
            if (StartCalled)
            {
                throw new Exception("Unexpected second call to Start");
            }

            StartCalled = true;
            ExecutionLogger.ExecutedOrder += "OtherStart";
        }

        public static void Start2()
        {
            if (Start2Called)
            {
                throw new Exception("Unexpected second call to Start2");
            }

            Start2Called = true;
            ExecutionLogger.ExecutedOrder += "OtherStart2";
        }
    }
}
