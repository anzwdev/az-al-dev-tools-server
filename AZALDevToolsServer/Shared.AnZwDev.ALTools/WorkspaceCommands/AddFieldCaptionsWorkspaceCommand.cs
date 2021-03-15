using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.CodeTransformations;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class AddFieldCaptionsWorkspaceCommand : SyntaxRewriterWorkspaceCommand<FieldCaptionSyntaxRewriter>
    {

        public AddFieldCaptionsWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "addFieldCaptions")
        {
        }

    }
}
