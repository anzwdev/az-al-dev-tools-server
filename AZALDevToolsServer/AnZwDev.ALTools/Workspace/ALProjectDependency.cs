using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using AnZwDev.ALTools.Core;
using AnZwDev.ALTools.ALSymbols.ALAppPackages;

namespace AnZwDev.ALTools.Workspace
{
    public class ALProjectDependency
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public string Publisher { get; set; }
        public VersionNumber Version { get; set; }

        public ALProject SourceProject { get; set; }
        public ALProjectAppPackageFile SourceAppPackage { get; set; }
        public ALAppPackage Symbols { get; set; }

        public ALProjectDependency()
        {
        }

        public ALProjectDependency(string id, string name, string publisher, string version)
        {
            this.Id = id;
            this.Name = name;
            this.Publisher = publisher;
            this.Version = new VersionNumber(version);
        }

        public void LoadSymbols()
        {
        }

    }
}
