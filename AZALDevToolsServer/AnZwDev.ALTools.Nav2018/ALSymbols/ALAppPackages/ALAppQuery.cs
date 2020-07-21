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

namespace AnZwDev.ALTools.Nav2018.ALSymbols.ALAppPackages
{
    public class ALAppQuery : ALAppObject
    {

        public ALAppElementsCollection<ALAppQueryDataItem> Elements { get; set; }

        public ALAppQuery()
        {
        }

        protected override ALSymbolKind GetALSymbolKind()
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
