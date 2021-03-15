using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using AnZwDev.ALTools.ALSymbolReferences;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class ReportDataItemInformation
    {

        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("source")]
        public string Source { get; set; }
        [JsonProperty("pageTableFields")]
        public List<TableFieldInformaton> DataItemTableFields { get; set; }
        [JsonProperty("availableTableFields")]
        public List<TableFieldInformaton> AvailableTableFields { get; set; }

        public ReportDataItemInformation()
        {
        }

        public ReportDataItemInformation(ALAppReportDataItem dataItemSymbol)
        {
            this.DataItemTableFields = null;
            this.AvailableTableFields = null;
            this.Name = dataItemSymbol.Name;
            this.Source = dataItemSymbol.RelatedTable;
        }

    }
}
