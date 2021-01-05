using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.ALTools.TypeInformation;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class AddPageFieldCaptionWorkspaceCommand : SyntaxRewriterWorkspaceCommand<PageFieldCaptionSyntaxRewriter>
    {

        public static string UseNameIfNoCaptionParameterName = "useNameIfNoCaption";

        public AddPageFieldCaptionWorkspaceCommand() : base("addPageFieldCaptions")
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
            if (parameters.ContainsKey(UseNameIfNoCaptionParameterName))
                this.SyntaxRewriter.UseNameIfNoCaption = (parameters[UseNameIfNoCaptionParameterName] == "true");
        }


    }
}
