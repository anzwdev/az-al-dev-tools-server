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
    public class ALAppQueryColumn : ALAppElementWithName
    {

        public string SourceColumn { get; set; }

        public ALAppQueryColumn()
        {
        }

        protected override ALSymbolKind GetALSymbolKind()
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
