using AnZwDev.ALTools.CodeTransformations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    internal class GenerateCSVXmlPortHeadersWorkspaceCommand : SyntaxRewriterWorkspaceCommand<GenerateCSVXmlPortHeadersSyntaxRewriter>
    {

        public GenerateCSVXmlPortHeadersWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "generateCSVXmlPortHeaders")
        {
        }

    }
}
