using AnZwDev.ALTools.CodeTransformations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class SortVariablesWorkspaceCommand : SyntaxRewriterWorkspaceCommand<SortVariablesSyntaxRewriter>
    {

        public SortVariablesWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "sortVariables")
        {
        }

    }
}
