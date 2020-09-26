using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class SyntaxTreeWorkspaceCommand : WorkspaceCommand
    {

        public SyntaxTreeWorkspaceCommand(string name) : base(name)
        {
        }

        public override WorkspaceCommandResult Run(string sourceCode, string path, Range range, Dictionary<string, string> parameters)
        {
            string newSourceCode = null;
            if (!String.IsNullOrEmpty(sourceCode))
                newSourceCode = this.ProcessSourceCode(sourceCode, path, range, parameters);
            else if (!String.IsNullOrWhiteSpace(path))
                this.ProcessDirectory(path, parameters);

            return new WorkspaceCommandResult(newSourceCode);
        }

        protected string ProcessSourceCode(string sourceCode, string path, Range range, Dictionary<string, string> parameters)
        {
            //parse source code
            SourceText sourceText = SourceText.From(sourceCode);
            SyntaxTree syntaxTree = SyntaxTree.ParseObjectText(sourceText);

            //convert range to TextSpan
            TextSpan span = new TextSpan(0, 0);
            if (range != null)
            {
                LinePositionSpan srcRange = new LinePositionSpan(new LinePosition(range.start.line, range.start.character), new LinePosition(range.end.line, range.end.character));
                span = sourceText.Lines.GetTextSpan(srcRange);
            }

            //fix nodes
            SyntaxNode node = this.ProcessSyntaxNode(syntaxTree.GetRoot(), sourceCode, path, span, parameters);

            //return new source code
            if (node == null)
                return null;

            return node.ToFullString();
        }

        protected virtual void ProcessDirectory(string path, Dictionary<string, string> parameters)
        {
            string[] filePathsList = System.IO.Directory.GetFiles(path, "*.al", System.IO.SearchOption.AllDirectories);
            for (int i = 0; i < filePathsList.Length; i++)
            {
                this.ProcessFile(filePathsList[i], parameters);
            }
        }

        protected virtual void ProcessFile(string path, Dictionary<string, string> parameters)
        {
            try
            {
                string source = System.IO.File.ReadAllText(path);
                string newSource = this.ProcessSourceCode(source, path, null, parameters);
                if ((newSource != source) && (!String.IsNullOrWhiteSpace(newSource)))
                    System.IO.File.WriteAllText(path, newSource);
            }
            catch (Exception)
            {
            }
        }

        public virtual SyntaxNode ProcessSyntaxNode(SyntaxNode node, string sourceCode, string path, TextSpan span, Dictionary<string, string> parameters)
        {
            return node;
        }

    }
}
