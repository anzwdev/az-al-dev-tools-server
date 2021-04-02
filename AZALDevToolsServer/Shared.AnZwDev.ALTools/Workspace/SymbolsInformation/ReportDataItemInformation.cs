using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using AnZwDev.ALTools.ALSymbolReferences;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class ReportDataItemInformation : TableBasedSymbolInformation
    {

        public ReportDataItemInformation()
        {
        }

        public ReportDataItemInformation(ALAppReportDataItem dataItemSymbol) : base(dataItemSymbol, dataItemSymbol.RelatedTable)
        {
        }

    }
}
