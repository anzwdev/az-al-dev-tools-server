using AnZwDev.ALTools.Nav2018.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Nav2018.WorkspaceCommands
{
    public class SyntaxTreeWorkspaceCommand : WorkspaceCommand
    {

        public SyntaxTreeWorkspaceCommand(string name) : base(name)
        {
        }

        public override WorkspaceCommandResult Run(string sourceCode, string path, Dictionary<string, string> parameters)
        {
            string newSourceCode = null;
            if (!String.IsNullOrEmpty(sourceCode))
                newSourceCode = this.ProcessSourceCode(sourceCode, path, parameters);
            else if (!String.IsNullOrWhiteSpace(path))
                this.ProcessDirectory(path, parameters);

            return new WorkspaceCommandResult(newSourceCode);
        }

        protected string ProcessSourceCode(string sourceCode, string path, Dictionary<string, string> parameters)
        {
            //parse source code
            SyntaxTree syntaxTree = SyntaxTreeExtensions.SafeParseObjectText(sourceCode);

            //fix nodes
            SyntaxNode node = this.ProcessSyntaxNode(syntaxTree.GetRoot(), sourceCode, path, parameters);

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
                string newSource = this.ProcessSourceCode(source, path, parameters);
                if ((newSource != source) && (!String.IsNullOrWhiteSpace(newSource)))
                    System.IO.File.WriteAllText(path, newSource);
            }
            catch (Exception)
            {
            }
        }

        public virtual SyntaxNode ProcessSyntaxNode(SyntaxNode node, string sourceCode, string path, Dictionary<string, string> parameters)
        {
            return node;
        }

    }
}
