using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols.ALAppPackages
{
    public class ALAppPageActionChange : ALAppElementWithName
    {

        public ALAppElementsCollection<ALAppPageAction> Actions { get; set; }

        public ALAppPageActionChange()
        {
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.ActionModifyChange;
        }

        protected override void AddChildALSymbols(ALSymbolInformation symbol)
        {
            this.Actions?.AddToALSymbol(symbol);
            base.AddChildALSymbols(symbol);
        }

    }
}
