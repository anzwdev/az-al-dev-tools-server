using AnZwDev.ALTools.ALSymbols.ALAppPackages;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class PageInformation : SymbolWithIdInformation
    {

        [JsonProperty("source")]
        public string Source { get; set; }
        [JsonProperty("pageTableFields")]
        public List<TableFieldInformaton> PageTableFields { get; set; }
        [JsonProperty("availableTableFields")]
        public List<TableFieldInformaton> AvailableTableFields { get; set; }

        public PageInformation()
        {
        }

        public PageInformation(ALAppPage page)
        {
            this.PageTableFields = null;
            this.AvailableTableFields = null;
            this.Id = page.Id;
            this.Name = page.Name;
            if (page.Properties != null)
            {
                this.Caption = page.Properties.GetValue("Caption");
                this.Source = page.Properties.GetValue("SourceTable");
            }
        }

    }
}
