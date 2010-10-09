using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.ComponentModel.Composition;

namespace WebActivation {
    [InheritedExport(typeof(IApplicationStart))]
    public interface IApplicationStart {
        void Start();
    }
}
