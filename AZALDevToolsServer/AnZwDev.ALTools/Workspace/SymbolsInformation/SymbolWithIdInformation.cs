using AnZwDev.ALTools.ALSymbols.ALAppPackages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class SymbolWithIdInformation : SymbolInformation
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        public SymbolWithIdInformation()
        {
        }

        public SymbolWithIdInformation(ALAppElementWithNameId symbol)
        {
            
        }

    }
}
