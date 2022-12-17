using AnZwDev.ALTools.CodeTransformations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class AddDropDownFieldGroupsWorkspaceCommand : SyntaxRewriterWorkspaceCommand<AddDropDownFieldGroupsRewriter>
    {

        public AddDropDownFieldGroupsWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "addDropDownFieldGroups")
        {
        }

    }
}
