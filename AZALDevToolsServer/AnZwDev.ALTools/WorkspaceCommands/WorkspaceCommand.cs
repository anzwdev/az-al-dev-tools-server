using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class WorkspaceCommand
    {

        public string Name { get; set; }

        public WorkspaceCommand(string newName)
        {
            Name = newName;
        }

        public virtual WorkspaceCommandResult Run(string sourceCode, string path, Dictionary<string, string> parameters)
        {
            return WorkspaceCommandResult.Empty;
        }

    }
}
