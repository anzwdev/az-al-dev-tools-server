using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols.ALAppPackages
{
    public class ALAppPage : ALAppObject
    {

        public ALAppElementsCollection<ALAppPageControl> Controls { get; set; }
        public ALAppElementsCollection<ALAppPageAction> Actions { get; set; }

        public ALAppPage()
        {
        }

        protected override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.PageObject;
        }

        protected override void AddChildALSymbols(ALSymbolInformation symbol)
        {
            this.Controls?.AddToALSymbol(symbol, ALSymbolKind.PageLayout, "layout");
            this.Actions?.AddToALSymbol(symbol, ALSymbolKind.PageActionList, "actions");
            base.AddChildALSymbols(symbol);
        }


    }
}
