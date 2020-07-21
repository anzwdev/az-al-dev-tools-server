/****************************************************************
 *                                                              *
 * Legacy version of the library maintained to support Nav 2018 *
 *                                                              *
 ****************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Nav2018.CodeAnalysis
{
    public class CodeAnalyzerRule
    {

        public string id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string category { get; set; }
        public string defaultSeverity { get; set; }
        public bool isEnabledByDefault { get; set; }
    }
}
