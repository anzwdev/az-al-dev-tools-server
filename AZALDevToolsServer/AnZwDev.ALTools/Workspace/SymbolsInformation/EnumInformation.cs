using AnZwDev.ALTools.ALSymbols.ALAppPackages;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class EnumInformation : SymbolInformation
    {

        public EnumInformation()
        {
        }

        public EnumInformation(ALAppEnum symbol) : base(symbol)
        {
            //this.Implements = symbol.Implements;
        }

    }
}
