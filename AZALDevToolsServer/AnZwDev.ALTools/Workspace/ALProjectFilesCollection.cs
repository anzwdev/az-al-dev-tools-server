using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace
{
    public class ALProjectFilesCollection<T> : List<T> where T : ALProjectFile
    {

        public ALProject Project { get; set; }

        public ALProjectFilesCollection(ALProject project)
        {
            this.Project = project;
        }

    }
}
