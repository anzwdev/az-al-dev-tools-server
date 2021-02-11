using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using AnZwDev.ALTools.Core;

namespace AnZwDev.ALTools.Workspace.Serialization
{
    public partial class ALProjectMetadataSerializer
    {

        private class ProjectMetadata
        {

            #region Public properties

            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("publisher")]
            public string Publisher { get; set; }

            [JsonProperty("version")]
            public string Version { get; set; }

            [JsonProperty("platform")]
            public string Platform { get; set; }

            [JsonProperty("application")]
            public string Application { get; set; }

            [JsonProperty("test")]
            public string Test { get; set; }

            [JsonProperty("dependencies")]
            public ProjectDependencyMetadata[] Dependencies { get; set; }

            [JsonProperty("range")]
            public ProjectIdRangeMetadata Range { get; set; }

            [JsonProperty("ranges")]
            public ProjectIdRangeMetadata[] Ranges { get; set; }

            #endregion

            #region Initialization

            public ProjectMetadata()
            {
            }

            #endregion

            #region Conversion to ALProjectMetadata

            public ALProjectProperties ToALProjectMetadata()
            {
                ALProjectProperties metadata = new ALProjectProperties();
                this.CopyProperties(metadata);
                this.CopyDependencies(metadata);
                this.CopyIdRanges(metadata);
                return metadata;
            }

            private void CopyProperties(ALProjectProperties targetMetadata)
            {
                targetMetadata.Id = this.Id;
                targetMetadata.Name = this.Name;
                targetMetadata.Publisher = this.Publisher;
                targetMetadata.Version = new Core.VersionNumber(this.Version);
            }

            private void CopyDependencies(ALProjectProperties targetMetadata)
            {
                //copy dependencies properties
                if (!String.IsNullOrWhiteSpace(this.Platform))
                    targetMetadata.Platform = targetMetadata.AddDependency(null, "Platform", "Microsoft", this.Platform);
                if (!String.IsNullOrWhiteSpace(this.Application))
                    targetMetadata.Application = targetMetadata.AddDependency(null, "Application", "Microsoft", this.Platform);
                if (!String.IsNullOrWhiteSpace(this.Test))
                    targetMetadata.Test = targetMetadata.AddDependency(null, "Test", "Microsoft", this.Platform);
                //copy other dependencies
                if (this.Dependencies != null)
                {
                    for (int i=0; i<this.Dependencies.Length; i++)
                    {
                        ProjectDependencyMetadata dependency = this.Dependencies[i];
                        targetMetadata.AddDependency(dependency.Id, dependency.Name, dependency.Publisher, dependency.Version);
                    }
                }
            }

            private void CopyIdRanges(ALProjectProperties targetMetadata)
            {
                if (this.Range != null)
                    targetMetadata.AddIdRange(this.Range.From, this.Range.To);
                if (this.Ranges != null)
                {
                    for (int i=0; i<this.Ranges.Length; i++)
                    {
                        ProjectIdRangeMetadata idRange = this.Ranges[i];
                        targetMetadata.AddIdRange(idRange.From, idRange.To);
                    }
                }
            }

            #endregion

        }


    }
}
