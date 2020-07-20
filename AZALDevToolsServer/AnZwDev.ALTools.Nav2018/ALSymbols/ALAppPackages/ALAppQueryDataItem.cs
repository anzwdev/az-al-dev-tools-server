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
    public class ALAppQueryDataItem : ALAppElementWithName
    {

        public string RelatedTable { get; set; }
        public ALAppElementsCollection<ALAppQueryDataItem> DataItems { get; set; }
        public ALAppElementsCollection<ALAppQueryColumn> Columns { get; set; }
        public ALAppElementsCollection<ALAppQueryFilter> Filters { get; set; }

        public ALAppQueryDataItem()
        {
        }

        protected override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.QueryDataItem;
        }

        protected override ALSymbolInformation CreateMainALSymbol()
        {
            ALSymbolInformation symbol = base.CreateMainALSymbol();
            symbol.fullName = symbol.kind.ToName() + " " + ALSyntaxHelper.EncodeName(this.Name);
            if (!String.IsNullOrWhiteSpace(this.RelatedTable))
                symbol.fullName = symbol.fullName + ": Table " + this.RelatedTable;
            return symbol;
        }

        protected override void AddChildALSymbols(ALSymbolInformation symbol)
        {
            this.DataItems?.AddToALSymbol(symbol);
            this.Columns?.AddToALSymbol(symbol);
            this.Filters?.AddToALSymbol(symbol);
            base.AddChildALSymbols(symbol);
        }


    }
}
