﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols.ALAppPackages
{
    public class ALAppEnumExtension : ALAppEnum
    {

        public string TargetObject { get; set; }

        public ALAppEnumExtension()
        {
        }

        protected override ALSymbolInformation CreateMainALSymbol()
        {
            ALSymbolInformation symbol = base.CreateMainALSymbol();
            symbol.extends = this.TargetObject;
            return symbol;
        }

        protected override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.EnumExtensionType;
        }

    }
}
