/****************************************************************
 *                                                              *
 * Legacy version of the library maintained to support Nav 2018 *
 *                                                              *
 ****************************************************************/
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

        protected override AppAreaSyntaxRewriter CreateSyntaxRewriter(string sourceCode, string path, Dictionary<string, string> parameters)
        {
            AppAreaSyntaxRewriter syntaxRewriter = base.CreateSyntaxRewriter(sourceCode, path, parameters);
            if (parameters.ContainsKey(AppAreaParameterName))
                syntaxRewriter.ApplicationAreaName = parameters[AppAreaParameterName];
            if (String.IsNullOrWhiteSpace(syntaxRewriter.ApplicationAreaName))
                syntaxRewriter.ApplicationAreaName = "All";
            return syntaxRewriter;
        }

    }
}
