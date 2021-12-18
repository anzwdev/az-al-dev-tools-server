using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class TrimTrailingWhitespaceWorkspaceCommand : SourceTextWorkspaceCommand
    {

        public TrimTrailingWhitespaceWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "trimTrailingWhitespace")
        {
        }

        protected override (string, bool, string) ProcessSourceCode(string sourceCode, string projectPath, string filePath, Range range, Dictionary<string, string> parameters)
        {
            sourceCode = sourceCode.MultilineTrimEnd();
            return (sourceCode, true, null);
        }


    }
}
