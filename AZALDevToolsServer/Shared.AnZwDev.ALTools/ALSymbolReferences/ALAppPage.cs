﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.ALTools.ALSymbols;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppPage : ALAppObject
    {

        public ALAppElementsCollection<ALAppPageControl> Controls { get; set; }
        public ALAppElementsCollection<ALAppPageAction> Actions { get; set; }

        public ALAppPage()
        {
        }

        public override ALSymbolKind GetALSymbolKind()
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
