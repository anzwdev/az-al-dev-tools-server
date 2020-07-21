/****************************************************************
 *                                                              *
 * Legacy version of the library maintained to support Nav 2018 *
 *                                                              *
 ****************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Nav2018.WorkspaceCommands
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
