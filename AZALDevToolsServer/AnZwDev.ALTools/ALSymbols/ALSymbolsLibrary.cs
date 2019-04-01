using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols
{
    public class ALSymbolsLibrary
    {

        public ALSymbolInformation Root { get; set; }

        public ALSymbolsLibrary() : this(null)
        {
        }

        public ALSymbolsLibrary(ALSymbolInformation rootSymbol)
        {
            this.Root = rootSymbol;
        }

        public virtual bool Load(bool forceReload)
        {
            return false;
        }

    }
}
