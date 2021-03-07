using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols.ALAppPackages
{
    public class ALAppReportDataItem : ALAppElementWithName
    {
        
        public string RelatedTable { get; set; }
        public ALAppElementsCollection<ALAppReportColumn> Columns { get; set; }
        public ALAppElementsCollection<ALAppReportDataItem> DataItems { get; set; }

        public ALAppReportDataItem()
        {
        }

        public override ALSymbolKind GetALSymbolKind()
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

        protected override void AddChildALSymbols(ALSymbolInformation symbol)
        {
            this.Columns?.AddToALSymbol(symbol);
            this.DataItems?.AddToALSymbol(symbol);
            base.AddChildALSymbols(symbol);
        }

    }
}
