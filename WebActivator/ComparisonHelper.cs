using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebActivator
{
    internal class ComparisonHelper
    {
        /// <summary>
        /// Comparison for configuration tasks
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        internal static int ConfigComparison(Config a, Config b)
        {
            if (a.Priority > b.Priority)
                return -1;
            if (a.Priority < b.Priority)
                return 1;
            return 0;
        }
    }
}
