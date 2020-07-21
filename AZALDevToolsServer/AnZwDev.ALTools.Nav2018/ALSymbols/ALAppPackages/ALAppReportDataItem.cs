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
    public class ALAppReportDataItem : ALAppElementWithName
    {
        
        public string RelatedTable { get; set; }

        public ALAppReportDataItem()
        {
        }

        protected override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.ReportDataItem;
        }

        protected override ALSymbolInformation CreateMainALSymbol()
        {
            ALSymbolInformation symbol = base.CreateMainALSymbol();
            if (!String.IsNullOrWhiteSpace(this.RelatedTable))
                symbol.fullName = ALSyntaxHelper.EncodeName(this.Name) + ": " + ALSyntaxHelper.EncodeName(this.RelatedTable);
            return symbol;
        }

    }
}
