using AnZwDev.ALTools.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols.ALAppPackages
{
    public class WorkspaceProject
    {

        public ALProjectFile ALProjectFile { get; set; }
        public string ProjectPath { get; set; }

        public WorkspaceProject(ALProjectFile alProjectFile, string projectPath)
        {
            this.ALProjectFile = alProjectFile;
            this.ProjectPath = projectPath;
        }

        public bool ValidReference(string id, string publisher, string name, string version)
        {
            return (
                (id.EqualsOrEmpty(this.ALProjectFile.id)) &&
                (publisher.EqualsOrEmpty(this.ALProjectFile.publisher)) &&
                (name.EqualsOrEmpty(this.ALProjectFile.name)));
        }

    }
}
