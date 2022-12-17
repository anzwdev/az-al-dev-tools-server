using AnZwDev.ALTools.CodeTransformations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    internal class AddTableDataCaptionsWorkspaceCommand : SyntaxRewriterWorkspaceCommand<AddTableDataCaptionsRewriter>
    {

        public AddTableDataCaptionsWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "addTableDataCaptions")
        {
        }

    }
}
