using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[assembly: WebActivator.PostApplicationStartMethod(typeof(AppCodeStartupCode), "Start")]

public class AppCodeStartupCode {
    public static bool Called { get; set; }
    public static void Start() {
        Called = true;
    }
}
