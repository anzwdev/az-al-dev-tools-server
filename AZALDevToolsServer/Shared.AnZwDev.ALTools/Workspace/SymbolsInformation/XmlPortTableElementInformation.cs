using AnZwDev.ALTools.ALSymbolReferences;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class XmlPortTableElementInformation : SymbolInformation
    {

        [JsonProperty("source")]
        public string Source { get; set; }
        [JsonProperty("xmlPortTableFields")]
        public List<TableFieldInformaton> XmlPortTableFields { get; set; }
        [JsonProperty("availableTableFields")]
        public List<TableFieldInformaton> AvailableTableFields { get; set; }

        public XmlPortTableElementInformation()
        {
        }

        public XmlPortTableElementInformation(ALAppXmlPortNode xmlPortTableElement)
        {
            this.Name = xmlPortTableElement.Name;
            this.Source = xmlPortTableElement.Expression;
            this.XmlPortTableFields = null;
            this.AvailableTableFields = null;
        }


    }
}
