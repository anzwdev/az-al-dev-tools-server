using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using AnZwDev.ALTools.Core;

namespace AnZwDev.ALTools.Workspace
{
    public class ALProjectAppPackageFilesCollection : ALProjectFilesCollection<ALProjectAppPackageFile>
    {

        public ALProjectAppPackageFilesCollection(ALProject project) : base(project)
        {
        }

        public void LoadFromProjectFolder(string relativePath)
        {
            this.Clear();

            string[] fileNames = Directory.GetFiles(relativePath, "*.app");
            for (int i = 0; i < fileNames.Length; i++)
            {
                string fileRelativePath = Path.Combine(relativePath, Path.GetFileName(fileNames[i]));
                this.Add(new ALProjectAppPackageFile(this.Project, fileRelativePath));
            }
        }

        public ALProjectAppPackageFile Find(ALProjectDependency dependency)
        {
            return this.Find(dependency.Id, dependency.Name, dependency.Publisher, dependency.Version);
        }

        public ALProjectAppPackageFile Find(string id, string name, string publisher, VersionNumber version)
        {
            ALProjectAppPackageFile foundPackage = null;
            for (int i=0; i<this.Count; i++)
            {
                ALProjectAppPackageFile appPackage = this[i];
            }
            return foundPackage;
        }

    }
}
