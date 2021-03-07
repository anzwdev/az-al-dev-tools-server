﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols.ALAppPackages
{
    public class ALAppPageControlChange : ALAppElementWithName
    {

        public ALAppElementsCollection<ALAppPageControl> Controls { get; set; }

        public ALAppPageControlChange()
        {
        }

        public override ALSymbolKind GetALSymbolKind()
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
