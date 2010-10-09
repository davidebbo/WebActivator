using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.ComponentModel.Composition;

namespace WebActivator {
    [InheritedExport(typeof(IApplicationStart))]
    public interface IApplicationStart {
        void Run();
    }
}
