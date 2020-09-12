﻿/****************************************************************
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
    public class AddDataClassificationWorkspaceCommand : SyntaxRewriterWorkspaceCommand<DataClassificationSyntaxRewriter>
    {
        public static string DataClassificationParameterName = "dataClassification";
        
        public AddDataClassificationWorkspaceCommand() : base("addDataClassification")
        {
        }

        protected override DataClassificationSyntaxRewriter CreateSyntaxRewriter(string sourceCode, string path, Dictionary<string, string> parameters)
        {
            DataClassificationSyntaxRewriter syntaxRewriter = base.CreateSyntaxRewriter(sourceCode, path, parameters);
            if (parameters.ContainsKey(DataClassificationParameterName))
                syntaxRewriter.DataClassification = parameters[DataClassificationParameterName];
            if (String.IsNullOrWhiteSpace(syntaxRewriter.DataClassification))
                syntaxRewriter.DataClassification = "CustomerContent";
            return syntaxRewriter;
        }
    }
}
