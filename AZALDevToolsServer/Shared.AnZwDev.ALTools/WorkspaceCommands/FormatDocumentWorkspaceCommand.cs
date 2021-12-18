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

    }
}
