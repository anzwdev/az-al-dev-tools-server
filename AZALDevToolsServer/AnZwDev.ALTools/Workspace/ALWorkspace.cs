using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AnZwDev.ALTools.Core;

namespace AnZwDev.ALTools.Workspace
{
    public class ALWorkspace
    {

        public List<ALProject> Projects { get; }

        public ALWorkspace()
        {
            this.Projects = new List<ALProject>();
        }

        #region Projects management

        public void Load(string[] workspaceFolders)
        {
            this.Projects.Clear();
            foreach (string path in workspaceFolders)
            {
                this.AddProject(path);
            }
        }

        public void AddProject(string path)
        {
            ALProject project = new ALProject(this, path);
            project.LoadProperties();
            this.Projects.Add(project);
        }

        public void RemoveProject(string path)
        {
            ALProject project = this.GetProject(path);
            if (project != null)
                this.Projects.Remove(project);
        }

        public ALProject GetProject(string path)
        {
            return this.Projects.Where(p => (p.RootPath == path)).FirstOrDefault();
        }

        public ALProject FindProject(string id, string name, string publisher, VersionNumber version)
        {
            return null;
        }

        #endregion

        public void UpdateDependencies()
        {
            foreach (ALProject project in this.Projects)
            {
                project.UpdateDependencies();
            }
        }

    }
}
