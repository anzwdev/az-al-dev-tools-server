using AnZwDev.ALTools.ALSymbols.ALAppPackages;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class TableFieldInformaton : SymbolWithIdInformation
    {

        [JsonProperty("dataType")]
        public string DataType { get; set; }

        public TableFieldInformaton()
        {
        }

        public TableFieldInformaton(int id, string name, string dataType)
        {
            this.Id = id;
            this.Name = name;
            this.Caption = name;
            this.DataType = dataType;
        }

        public TableFieldInformaton(ALAppTableField symbolReference)
        {
            this.Id = symbolReference.Id;
            this.Name = symbolReference.Name;

            if (symbolReference.Properties != null)
                this.Caption = symbolReference.Properties.GetValue("Caption");
            this.DataType = symbolReference.TypeDefinition.Name;
        }

    }
}
