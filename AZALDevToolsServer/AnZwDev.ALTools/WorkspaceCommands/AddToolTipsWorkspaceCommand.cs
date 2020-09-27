﻿using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.ALTools.TypeInformation;
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

        public AddToolTipsWorkspaceCommand() : base("addToolTips")
        {
        }

        public override WorkspaceCommandResult Run(string sourceCode, string path, Range range, Dictionary<string, string> parameters)
        {
            //load table information
            this.SyntaxRewriter.TypeInformationCollector.Clear();
            if ((!String.IsNullOrWhiteSpace(sourceCode)) && (parameters != null) &&
                (parameters.ContainsKey("table")))
                this.SyntaxRewriter.TypeInformationCollector.VisitFile(parameters["table"]);
            else if (!String.IsNullOrWhiteSpace(path))
                this.SyntaxRewriter.TypeInformationCollector.VisitDirectory(path);

            //process files
            return base.Run(sourceCode, path, range, parameters);
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
