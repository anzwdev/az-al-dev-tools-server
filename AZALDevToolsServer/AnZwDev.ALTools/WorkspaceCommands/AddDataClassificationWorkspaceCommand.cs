using AnZwDev.ALTools.CodeTransformations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class AddDataClassificationWorkspaceCommand : SyntaxRewriterWorkspaceCommand<DataClassificationSyntaxRewriter>
    {
        public static string DataClassificationParameterName = "dataClassification";
        
        public AddDataClassificationWorkspaceCommand() : base("addDataClassification")
        {
        }

        protected override void SetParameters(string sourceCode, string path, Dictionary<string, string> parameters)
        {
            base.SetParameters(sourceCode, path, parameters);
            if (parameters.ContainsKey(DataClassificationParameterName))
                this.SyntaxRewriter.DataClassification = parameters[DataClassificationParameterName];
            if (String.IsNullOrWhiteSpace(this.SyntaxRewriter.DataClassification))
                this.SyntaxRewriter.DataClassification = "CustomerContent";
        }

    }
}
