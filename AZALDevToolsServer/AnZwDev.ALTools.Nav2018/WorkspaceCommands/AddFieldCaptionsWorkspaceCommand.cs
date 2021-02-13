using AnZwDev.ALTools.Nav2018.CodeTransformations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Nav2018.WorkspaceCommands
{
    public class AddFieldCaptionsWorkspaceCommand : SyntaxRewriterWorkspaceCommand<FieldCaptionSyntaxRewriter>
    {

        public AddFieldCaptionsWorkspaceCommand() : base("addFieldCaptions")
        {
        }

    }
}
