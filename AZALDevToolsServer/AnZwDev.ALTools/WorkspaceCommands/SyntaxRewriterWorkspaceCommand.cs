using AnZwDev.ALTools.CodeTransformations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class SyntaxRewriterWorkspaceCommand<T> : WorkspaceCommand where T: ALSyntaxRewriter, new()
    {

        public static string NoOfChangesParameterName = "noOfChanges";
        public static string NoOfChangedFilesParameterName = "noOfChangedFiles";

        public SyntaxRewriterWorkspaceCommand(string name): base(name)
        {
        }

        public override WorkspaceCommandResult Run(string sourceCode, string path, Dictionary<string, string> parameters)
        {
            T syntaxRewriter = this.CreateSyntaxRewriter(sourceCode, path, parameters);
            
            string newSourceCode = null;
            if (!String.IsNullOrEmpty(sourceCode))
                newSourceCode = syntaxRewriter.RewriteSourceCode(sourceCode);
            else if (!String.IsNullOrWhiteSpace(path))
                syntaxRewriter.RewriteDirectory(path);

            return this.CreateResult(syntaxRewriter, newSourceCode, path, parameters);
        }

        protected virtual T CreateSyntaxRewriter(string sourceCode, string path, Dictionary<string, string> parameters)
        {
            return new T();
        }

        protected virtual WorkspaceCommandResult CreateResult(T syntaxRewriter, string newSourceCode, string path, Dictionary<string, string> parameters)
        {
            WorkspaceCommandResult result = new WorkspaceCommandResult(newSourceCode);
            result.SetParameter(NoOfChangesParameterName, syntaxRewriter.TotalNoOfChanges.ToString());
            result.SetParameter(NoOfChangedFilesParameterName, syntaxRewriter.NoOfChangedFiles.ToString());
            return result;
        }



    }
}
