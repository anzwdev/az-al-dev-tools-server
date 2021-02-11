using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using AnZwDev.ALTools.Core;

namespace AnZwDev.ALTools.Workspace
{
    public class ALProjectProperties
    {

        #region Public properties

        public string Id { get; set; }
        public string Name { get; set; }
        public string Publisher { get; set; }
        public VersionNumber Version { get; set; }

        public ALProjectDependency Platform { get; set; }
        public ALProjectDependency Application { get; set; }
        public ALProjectDependency Test { get; set; }

        public List<ALProjectDependency> Dependencies { get; set; }
        public List<ALProjectIdRange> Ranges { get; set; }

        #endregion

        public ALProjectProperties()
        {
            this.Dependencies = new List<ALProjectDependency>();
            this.Ranges = new List<ALProjectIdRange>();
        }

        public ALProjectDependency AddDependency(string id, string name, string publisher, string version)
        {
            ALProjectDependency dependency = new ALProjectDependency(id, name, publisher, version);
            this.Dependencies.Add(dependency);
            return dependency;
        }

        public ALProjectIdRange AddIdRange(long fromId, long toId)
        {
            ALProjectIdRange idRange = new ALProjectIdRange(fromId, toId);
            this.Ranges.Add(idRange);
            return idRange;
        }


    }
}
