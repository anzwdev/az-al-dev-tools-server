using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols.ALAppPackages
{
    public class ALAppInterface : ALAppObject
    {
        public ALAppInterface()
        {
        }

        protected override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.Interface;
        }

    }
}
