using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using AnZwDev.ALTools.Workspace.Serialization;

namespace AnZwDev.ALTools.Workspace
{
    public class ALProject
    {

        public ALWorkspace Workspace { get; set; }
        public List<ALProjectFile> Files { get; }
        public ALProjectAppPackageFilesCollection AppPackageFiles { get; }
        public string RootPath { get; set; }
        public ALProjectProperties Properties { get; set; }

        public ALProject() : this(null, null)
        {
        }

        public ALProject(ALWorkspace workspace, string rootPath)
        {
            this.Workspace = workspace;
            this.RootPath = rootPath;
            this.Files = new List<ALProjectFile>();
            this.AppPackageFiles = new ALProjectAppPackageFilesCollection(this);
            this.Properties = null;
        }

        public void LoadProperties()
        {
            this.Properties = ALProjectMetadataSerializer.DeserializeFromFile(Path.Combine(this.RootPath, "app.json"));
        }

        public void LoadAppPackageFilesList()
        {
            this.AppPackageFiles.LoadFromProjectFolder(".alpackages");            
        }

        #region Dependencies

        public void UpdateDependencies()
        {
            if (this.Properties == null)
                return;

            foreach (ALProjectDependency dependency in this.Properties.Dependencies)
            {
                dependency.SourceProject = this.Workspace.FindProject(dependency.Id, dependency.Name, dependency.Publisher, dependency.Version);
                if (dependency.SourceProject != null)
                    dependency.SourceAppPackage = null;
                else
                { 
                    //dependency.SourceAppPackage = this.AppPackageFiles.
                }
            }
        }

        #endregion

    }
}
