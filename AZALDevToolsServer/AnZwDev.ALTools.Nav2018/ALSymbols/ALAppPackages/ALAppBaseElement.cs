﻿/****************************************************************
 *                                                              *
 * Legacy version of the library maintained to support Nav 2018 *
 *                                                              *
 ****************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Nav2018.ALSymbols.ALAppPackages
{
    public class ALAppBaseElement
    {

        public ALAppBaseElement()
        {
        }

        public virtual ALSymbolInformation ToALSymbol()
        {
            ALSymbolInformation symbol = this.CreateMainALSymbol();
            this.AddChildALSymbols(symbol);
            return symbol;
        }

        protected virtual ALSymbolInformation CreateMainALSymbol()
        {
            return new ALSymbolInformation();
        }

        protected virtual void AddChildALSymbols(ALSymbolInformation symbol)
        {
        }

        protected virtual ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.Undefined;
        }

        public virtual string GetSourceCode()
        {
            return "";
        }

    }
}
