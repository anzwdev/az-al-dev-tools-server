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
    public class ALAppPageExtension : ALAppObject
    {

        public ALAppElementsCollection<ALAppPageControlChange> ControlChanges { get; set; }
        public ALAppElementsCollection<ALAppPageActionChange> ActionChanges { get; set; }

        public ALAppPageExtension()
        {
        }

        protected override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.PageExtensionObject;
        }

        protected override void AddChildALSymbols(ALSymbolInformation symbol)
        {
            this.ControlChanges?.AddToALSymbol(symbol, ALSymbolKind.PageExtensionLayout, "layout");
            this.ActionChanges?.AddToALSymbol(symbol, ALSymbolKind.PageExtensionActionList, "actions");
            base.AddChildALSymbols(symbol);
        }

    }
}
