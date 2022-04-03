using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.ALTools.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{

    public class RemoveEmptyLinesWorkspaceCommand : SyntaxRewriterWorkspaceCommand<RemoveEmptyLinesSyntaxRewriter>
    {

        public RemoveEmptyLinesWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "removeEmptyLines")
        {
        }

        protected override (string, bool, string) ProcessSourceCode(string sourceCode, string projectPath, string filePath, Range range, Dictionary<string, string> parameters)
        {
            string newSourceCode = sourceCode.RemoveDuplicateEmptyLines();
            int prevNoOfChangedFiles = this.SyntaxRewriter.NoOfChangedFiles;
            bool fileChanged = (newSourceCode != sourceCode);

            (string processedSource, bool isError, string errorMessage) = base.ProcessSourceCode(newSourceCode, projectPath, filePath, range, parameters);

            if ((fileChanged) && (prevNoOfChangedFiles == this.SyntaxRewriter.NoOfChangedFiles))
                this.SyntaxRewriter.NoOfChangedFiles++;

            return (processedSource, isError, errorMessage);
        }

    }

    /*
    public class RemoveDuplicateEmptyLinesWorkspaceCommand : SourceTextWorkspaceCommand
    {
        protected int _totalNoOfChanges = 0;
        protected int _noOfChangedFiles = 0;

        public RemoveDuplicateEmptyLinesWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "removeDuplicateEmptyLines")
        {
        }

        protected override (string, bool, string) ProcessSourceCode(string sourceCode, string projectPath, string filePath, Range range, Dictionary<string, string> parameters)
        {
            string newSourceCode = sourceCode.RemoveDuplicateEmptyLines();

            if (newSourceCode != sourceCode)
            {
                _totalNoOfChanges++;
                _noOfChangedFiles++;
            }

            return (newSourceCode, true, null);
        }

        public override WorkspaceCommandResult Run(string sourceCode, string projectPath, string filePath, Range range, Dictionary<string, string> parameters, List<string> excludeFiles)
        {
            this._totalNoOfChanges = 0;
            this._noOfChangedFiles = 0;

            WorkspaceCommandResult result = base.Run(sourceCode, projectPath, filePath, range, parameters, excludeFiles);

            result.SetParameter(NoOfChangesParameterName, this._totalNoOfChanges.ToString());
            result.SetParameter(NoOfChangedFilesParameterName, this._noOfChangedFiles.ToString());
            return result;
        }
    }
    */
}
