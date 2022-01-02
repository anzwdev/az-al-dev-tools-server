using System;
using System.Collections.Generic;
using System.Text;
using AnZwDev.ALTools.ALSymbolReferences;
using Newtonsoft.Json;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class TableInformation : BaseObjectInformation
    {

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public List<TableFieldInformaton> Fields { get; set; }

        public TableInformation()
        {
            this.Fields = null;
        }

        public TableInformation(ALAppTable table): base(table)
        {
            this.Fields = null;
        }


    }
}
