using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppPragmaWarningDirective : ALAppDirective
    {

        public bool Disabled { get; set; }
        public List<string> Rules { get; set; }

        public ALAppPragmaWarningDirective(Range range, bool disabled, List<string> rules) : base(range)
        {
            Disabled = disabled;
            Rules = rules;
        }

    }
}
