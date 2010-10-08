using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebActivation;
using System.ComponentModel.Composition;

namespace TestLibrary {
    [Export(typeof(IApplicationStart))]
    public class MyStartupCode : IApplicationStart {
        public void Start() {
            throw new Exception("Blah!");
        }
    }
}
