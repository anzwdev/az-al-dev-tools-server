using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols.ALAppPackages
{
    public class ALAppReport : ALAppObject
    {

        public ALAppRequestPage RequestPage { get; set; }
        public ALAppElementsCollection<ALAppReportDataItem> DataItems { get; set; }

        public ALAppReport()
        {
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.ReportObject;
        }

        protected override void AddChildALSymbols(ALSymbolInformation symbol)
        {
            this.DataItems?.AddToALSymbol(symbol, ALSymbolKind.ReportDataSetSection, "dataset");
            if (this.RequestPage != null)
                symbol.AddChildSymbol(this.RequestPage.ToALSymbol());
            base.AddChildALSymbols(symbol);
        }

    }
}
