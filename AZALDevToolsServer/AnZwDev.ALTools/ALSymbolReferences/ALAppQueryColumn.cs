﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols.ALAppPackages
{
    public class ALAppQueryColumn : ALAppElementWithName
    {

        public string SourceColumn { get; set; }

        public ALAppQueryColumn()
        {
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.QueryColumn;
        }

        protected override ALSymbolInformation CreateMainALSymbol()
        {
            ALSymbolInformation symbol =  base.CreateMainALSymbol();
            if (!String.IsNullOrWhiteSpace(this.SourceColumn))
                symbol.fullName = ALSyntaxHelper.EncodeName(this.Name) + ": " + ALSyntaxHelper.EncodeName(this.SourceColumn);
            return symbol;
        }

    }
}
