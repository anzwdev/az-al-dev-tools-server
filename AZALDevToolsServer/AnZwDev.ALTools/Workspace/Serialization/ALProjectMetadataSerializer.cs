using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using AnZwDev.ALTools.Workspace;

namespace AnZwDev.ALTools.Workspace.Serialization
{
    public partial class ALProjectMetadataSerializer
    {

        public ALProjectMetadataSerializer()
        {
        }

        public static ALProjectProperties DeserializeFromFile(string path)
        {
            return DeserializeFromJson(File.ReadAllText(path));
        }

        public static ALProjectProperties DeserializeFromJson(string jsonContent)
        {
            //deserialize to internal structures
            ProjectMetadata metadata = JsonConvert.DeserializeObject<ProjectMetadata>(jsonContent);

            //return metadata
            return metadata.ToALProjectMetadata();
        }


    }
}
