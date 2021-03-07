﻿using AnZwDev.ALTools.ALSymbols.ALAppPackages;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class CodeunitInformation : SymbolInformation
    {

        [JsonProperty("implements")]
        public string Implements { get; set; }

        public CodeunitInformation()
        {
        }

        public CodeunitInformation(ALAppCodeunit symbol) : base(symbol)
        {
//            this.Implements = symbol.Implements;
        }

    }
}
