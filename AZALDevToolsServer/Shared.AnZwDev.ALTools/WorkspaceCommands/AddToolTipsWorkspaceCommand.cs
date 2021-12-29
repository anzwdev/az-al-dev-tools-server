﻿using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Workspace;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class AddToolTipsWorkspaceCommand : SyntaxRewriterWorkspaceCommand<ToolTipSyntaxRewriter>
    {

        public static string FieldTooltipParameterName = "toolTipField";
        public static string FieldTooltipCommentParameterName = "toolTipFieldComment";
        public static string ActionTooltipParameterName = "toolTipAction";
        public static string UseFieldDescriptionParameterName = "useFieldDescription";

        public AddToolTipsWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "addToolTips")
        {
        }

        protected override void SetParameters(string sourceCode, string projectPath, string filePath, TextSpan span, Dictionary<string, string> parameters)
        {
            base.SetParameters(sourceCode, projectPath, filePath, span, parameters);
            this.SyntaxRewriter.PageFieldTooltip = parameters.GetStringValue(FieldTooltipParameterName);
            this.SyntaxRewriter.PageActionTooltip = parameters.GetStringValue(ActionTooltipParameterName);
            this.SyntaxRewriter.PageFieldTooltipComment = parameters.GetStringValue(FieldTooltipCommentParameterName);
            this.SyntaxRewriter.UseFieldDescription = parameters.GetBoolValue(UseFieldDescriptionParameterName);

            if (this.SyntaxRewriter.Project?.Symbols?.Tables != null)
            {
                PageInformationProvider provider = new PageInformationProvider();
                this.SyntaxRewriter.ToolTipsCache = provider.CollectProjectTableFieldsToolTips(this.SyntaxRewriter.Project, null);
            }
        }

        protected override void ClearParameters()
        {
            base.ClearParameters();
            this.SyntaxRewriter.ToolTipsCache = null;
        }

    }
}
