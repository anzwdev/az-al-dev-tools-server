using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols.ALAppPackages
{
    public class ALAppQuery : ALAppObject
    {

        public ALAppElementsCollection<ALAppQueryDataItem> Elements { get; set; }

        public ALAppQuery()
        {
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.QueryObject;
        }

        protected override void AddChildALSymbols(ALSymbolInformation symbol)
        {
            this.Elements?.AddToALSymbol(symbol, ALSymbolKind.QueryElements, "elements");
            base.AddChildALSymbols(symbol);
        }

    }
}
