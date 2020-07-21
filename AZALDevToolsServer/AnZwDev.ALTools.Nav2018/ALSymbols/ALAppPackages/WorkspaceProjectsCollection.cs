/****************************************************************
 *                                                              *
 * Legacy version of the library maintained to support Nav 2018 *
 *                                                              *
 ****************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Nav2018.ALSymbols.ALAppPackages
{
    public class WorkspaceProjectsCollection : List<WorkspaceProject>
    {

        public string[] WorkspaceFolders { get; private set; }

        public WorkspaceProjectsCollection(string[]workspaceFolders)
        {
            this.WorkspaceFolders = workspaceFolders;
        }

        public void Load()
        {
            this.Clear();
            for (int i=0; i<this.WorkspaceFolders.Length; i++)
            {
                string filePath = System.IO.Path.Combine(this.WorkspaceFolders[i], "app.json");
                if (System.IO.File.Exists(filePath))
                    this.Add(new WorkspaceProject(ALProjectFile.Load(filePath), this.WorkspaceFolders[i]));
            }
        }

        public WorkspaceProject FindByReference(string id, string publisher, string name, string version)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].ValidReference(id, publisher, name, version))
                    return this[i];
            }
            return null;
        }


    }
}
