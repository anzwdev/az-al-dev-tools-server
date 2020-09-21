using AnZwDev.ALTools.Nav2018.ALSymbols;
using AnZwDev.ALTools.Nav2018.CodeTransformations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Nav2018.WorkspaceCommands
{
    public class AddAppAreasWorkspaceCommand: SyntaxRewriterWorkspaceCommand<AppAreaSyntaxRewriter>
    {

        public static string AppAreaParameterName = "appArea";

        public AddAppAreasWorkspaceCommand(): base("addAppAreas")
        {
        }

        protected override void SetParameters(string sourceCode, string path, Range range, Dictionary<string, string> parameters)
        {
            base.SetParameters(sourceCode, path, range, parameters);
            if (parameters.ContainsKey(AppAreaParameterName))
                this.SyntaxRewriter.ApplicationAreaName = parameters[AppAreaParameterName];
            if (String.IsNullOrWhiteSpace(this.SyntaxRewriter.ApplicationAreaName))
                this.SyntaxRewriter.ApplicationAreaName = "All";
        }

    }
}
