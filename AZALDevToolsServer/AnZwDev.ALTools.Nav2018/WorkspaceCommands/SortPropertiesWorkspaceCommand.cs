using AnZwDev.ALTools.Nav2018.CodeTransformations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Nav2018.WorkspaceCommands
{
    public class SortPropertiesWorkspaceCommand : SyntaxRewriterWorkspaceCommand<SortPropertiesSyntaxRewriter>
    {

        public SortPropertiesWorkspaceCommand(): base("sortProperties")
        {
        }

    }
}
