using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class FormatDocumentWorkspaceCommand : SyntaxTreeWorkspaceCommand
    {

        public FormatDocumentWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "formatDocument")
        {
        }

        public override WorkspaceCommandResult Run(string sourceCode, string projectPath, string filePath, Range range, Dictionary<string, string> parameters)
        {
            WorkspaceCommandResult result = base.Run(sourceCode, projectPath, filePath, range, parameters);
            result.SetParameter(NoOfChangesParameterName, "");
            result.SetParameter(NoOfChangedFilesParameterName, "");
            return result;
        }


    }
}
