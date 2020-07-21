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
    public class ALAppPageControlChange : ALAppElementWithName
    {

        public ALAppElementsCollection<ALAppPageControl> Controls { get; set; }

        public ALAppPageControlChange()
        {
        }

        protected override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.ControlModifyChange;
        }

        protected override void AddChildALSymbols(ALSymbolInformation symbol)
        {
            this.Controls?.AddToALSymbol(symbol);
            base.AddChildALSymbols(symbol);
        }

    }
}
