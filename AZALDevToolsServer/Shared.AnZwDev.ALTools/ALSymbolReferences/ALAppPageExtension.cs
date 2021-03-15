﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.ALTools.ALSymbols;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppPageExtension : ALAppObject
    {

        public string TargetObject { get; set; }

        public ALAppElementsCollection<ALAppPageControlChange> ControlChanges { get; set; }
        public ALAppElementsCollection<ALAppPageActionChange> ActionChanges { get; set; }

        public ALAppPageExtension()
        {
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.PageExtensionObject;
        }

        protected override ALSymbolInformation CreateMainALSymbol()
        {
            ALSymbolInformation symbol = base.CreateMainALSymbol();
            symbol.extends = this.TargetObject;
            return symbol;
        }

        protected override void AddChildALSymbols(ALSymbolInformation symbol)
        {
            this.ControlChanges?.AddToALSymbol(symbol, ALSymbolKind.PageExtensionLayout, "layout");
            this.ActionChanges?.AddToALSymbol(symbol, ALSymbolKind.PageExtensionActionList, "actions");
            base.AddChildALSymbols(symbol);
        }

    }
}
