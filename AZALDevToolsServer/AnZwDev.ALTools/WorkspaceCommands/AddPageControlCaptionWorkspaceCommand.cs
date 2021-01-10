using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.ALTools.TypeInformation;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class AddPageControlCaptionWorkspaceCommand : SyntaxRewriterWorkspaceCommand<PageControlCaptionSyntaxRewriter>
    {

        public static string SetActionsCaptionsParameterName = "setActionsCaptions";
        public static string SetGroupsCaptionsParameterName = "setGroupsCaptions";
        public static string SetActionGroupsCaptionsParameterName = "setActionGroupsCaptions";
        public static string SetPartsCaptionsParameterName = "setPartsCaptions";
        public static string SetFieldsCaptionsParameterName = "setFieldsCaptions";
        
        public AddPageControlCaptionWorkspaceCommand() : base("addPageControlCaptions")
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
            this.SyntaxRewriter.SetActionsCaptions = this.GetBoolParameter(parameters, SetActionsCaptionsParameterName);
            this.SyntaxRewriter.SetGroupsCaptions = this.GetBoolParameter(parameters, SetGroupsCaptionsParameterName);
            this.SyntaxRewriter.SetActionGroupsCaptions = this.GetBoolParameter(parameters, SetActionGroupsCaptionsParameterName);
            this.SyntaxRewriter.SetPartsCaptions = this.GetBoolParameter(parameters, SetPartsCaptionsParameterName);
            this.SyntaxRewriter.SetFieldsCaptions = this.GetBoolParameter(parameters, SetFieldsCaptionsParameterName);
        }

        private bool GetBoolParameter(Dictionary<string, string> parameters, string value)
        {
            return ((parameters.ContainsKey(value)) && (parameters[value].ToLower() == "true"));
        }

    }
}
