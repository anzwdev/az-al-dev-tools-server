using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppBaseElement : IComparable
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

        public virtual ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.Undefined;
        }

        public virtual string GetSourceCode()
        {
            return "";
        }

        public virtual int CompareTo(object obj)
        {
            return 0;
        }

    }
}
