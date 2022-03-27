﻿using AnZwDev.ALTools.ALSymbolReferences;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class MethodInformation
    {

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("header")]
        public string Header { get; set; }

        public MethodInformation()
        {
        }

        public MethodInformation(ALAppMethod method)
        {
            this.Name = method.Name;
            this.Header = method.GetHeaderSourceCode();
        }

    }
}