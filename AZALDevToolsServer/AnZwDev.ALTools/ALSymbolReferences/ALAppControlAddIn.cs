using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols.ALAppPackages
{
    public class ALAppControlAddIn : ALAppObject
    {

        public ALAppElementsCollection<ALAppMethod> Events { get; set; }

        public override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.ControlAddInObject;
        }

        protected override void AddChildALSymbols(ALSymbolInformation symbol)
        {
            this.Events?.AddToALSymbol(symbol);
            base.AddChildALSymbols(symbol);
        }


    }
}
