using AnZwDev.ALTools.Nav2018.ALSymbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Nav2018.WorkspaceCommands
{
    public class FixWithUsageWorkspaceCommand : WorkspaceCommand
    {

        public FixWithUsageWorkspaceCommand(): base("fixWithUsage")
        {
        }

        public override WorkspaceCommandResult Run(string sourceCode, string path, Range range, Dictionary<string, string> parameters)
        {
            return base.Run(sourceCode, path, range, parameters);
        }

    }
}
