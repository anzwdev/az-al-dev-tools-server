using AnZwDev.ALTools.Nav2018.ALSymbols;
using AnZwDev.ALTools.Nav2018.CodeTransformations;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Nav2018.WorkspaceCommands
{
    public class SyntaxRewriterWorkspaceCommand<T> : SyntaxTreeWorkspaceCommand where T: ALSyntaxRewriter, new()
    {

        public static string NoOfChangesParameterName = "noOfChanges";
        public static string NoOfChangedFilesParameterName = "noOfChangedFiles";

        public T SyntaxRewriter { get; }

        public SyntaxRewriterWorkspaceCommand(string name): base(name)
        {
            this.SyntaxRewriter = new T();
        }

        public override WorkspaceCommandResult Run(string sourceCode, string path, Range range, Dictionary<string, string> parameters)
        {
            WorkspaceCommandResult result = base.Run(sourceCode, path, range, parameters);
            result.SetParameter(NoOfChangesParameterName, this.SyntaxRewriter.TotalNoOfChanges.ToString());
            result.SetParameter(NoOfChangedFilesParameterName, this.SyntaxRewriter.NoOfChangedFiles.ToString());
            return result;
        }

        public override SyntaxNode ProcessSyntaxNode(SyntaxNode node, string sourceCode, string path, TextSpan span, Dictionary<string, string> parameters)
        {
            this.SetParameters(sourceCode, path, span, parameters);
            node = this.SyntaxRewriter.ProcessNode(node);
            return base.ProcessSyntaxNode(node, sourceCode, path, span, parameters);
        }

        protected virtual void SetParameters(string sourceCode, string path, TextSpan span, Dictionary<string, string> parameters)
        {
            this.SyntaxRewriter.Span = span;
        }

    }
}
