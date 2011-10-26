using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using System;

[assembly: WebActivator.PreApplicationStartMethod(typeof(TestLibrary.MyStartupCode), "Start")]
[assembly: WebActivator.PreApplicationStartMethod(typeof(TestLibrary.MyStartupCode), "Start2",Order = 2)]
[assembly: WebActivator.PreApplicationStartMethod(typeof(TestLibrary.MyStartupCode), "Start3",Order = 1)]
[assembly: WebActivator.PostApplicationStartMethod(typeof(TestLibrary.MyStartupCode), "CallMeAfterAppStart")]
[assembly: WebActivator.ApplicationShutdownMethod(typeof(TestLibrary.MyStartupCode), "CallMeWhenAppEnds")]

namespace TestLibrary {
    public static class MyStartupCode {

        public static string ExecutedOrder = "";
        public static bool StartCalled { get; set; }
        public static bool Start2Called { get; set; }
        public static bool CallMeAfterAppStartCalled { get; set; }
        public static bool CallMeWhenAppEndsCalled { get; set; }

        internal static void Start() {
            if (StartCalled) {
                throw new Exception("Unexpected second call to Start");
            }

            StartCalled = true;
            ExecutedOrder += "Start";
        }

        public static void Start2() {
            if (Start2Called) {
                throw new Exception("Unexpected second call to Start2");
            }

            Start2Called = true;
            ExecutedOrder += "Start2";
        }

        public static void Start3() {
            ExecutedOrder += "Start3";
        }

        public static void CallMeAfterAppStart() {
            // This gets called after global.asax's Application_Start

            if (CallMeAfterAppStartCalled) {
                throw new Exception("Unexpected second call to CallMeAfterAppStart");
            }

            CallMeAfterAppStartCalled = true;
            ExecutedOrder += "CallMeAfterAppStart";
        }

        public static void CallMeWhenAppEnds() {
            // This gets called when the app shuts down

            if (CallMeWhenAppEndsCalled) {
                throw new Exception("Unexpected second call to CallMeWhenAppEnds");
            }

            CallMeWhenAppEndsCalled = true;
            ExecutedOrder += "CallMeWhenAppEnds";
        }
    }
}
