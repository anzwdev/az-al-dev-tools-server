using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.CodeTransformations;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class AddToolTipsWorkspaceCommand : SyntaxRewriterWorkspaceCommand<ToolTipSyntaxRewriter>
    {

        public static string FieldTooltipParameterName = "toolTipField";
        public static string ActionTooltipParameterName = "toolTipAction";

        public AddToolTipsWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "addToolTips")
        {
        }

        protected override void SetParameters(string sourceCode, string path, TextSpan span, Dictionary<string, string> parameters)
        {
            base.SetParameters(sourceCode, path, span, parameters);
            if (parameters.ContainsKey(FieldTooltipParameterName))
                this.SyntaxRewriter.PageFieldTooltip = parameters[FieldTooltipParameterName];
            if (parameters.ContainsKey(ActionTooltipParameterName))
                this.SyntaxRewriter.PageActionTooltip = parameters[ActionTooltipParameterName];
        }

    }
}
