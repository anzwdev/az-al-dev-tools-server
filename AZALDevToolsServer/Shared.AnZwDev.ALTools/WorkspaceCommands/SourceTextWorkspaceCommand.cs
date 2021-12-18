﻿using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class SourceTextWorkspaceCommand : WorkspaceCommand
    {

        public SourceTextWorkspaceCommand(ALDevToolsServer alDevToolsServer, string name) : base(alDevToolsServer, name)
        {
        }

        public override WorkspaceCommandResult Run(string sourceCode, string projectPath, string filePath, Range range, Dictionary<string, string> parameters)
        {
            string newSourceCode = null;
            bool success = true;
            string errorMessage = null;

            if (!String.IsNullOrWhiteSpace(filePath))
            {
                if (!String.IsNullOrEmpty(sourceCode))
                {
                    (newSourceCode, success, errorMessage) = this.ProcessSourceCode(sourceCode, projectPath, filePath, range, parameters);
                    if (!success)
                        return new WorkspaceCommandResult(newSourceCode, true, errorMessage);
                }
            }
            else if (!String.IsNullOrWhiteSpace(projectPath))
                (success, errorMessage) = this.ProcessDirectory(projectPath, parameters);

            if (success)
                return new WorkspaceCommandResult(newSourceCode);
            return new WorkspaceCommandResult(newSourceCode, true, errorMessage);
        }

        protected virtual (string, bool, string) ProcessSourceCode(string sourceCode, string projectPath, string filePath, Range range, Dictionary<string, string> parameters)
        {
            return (sourceCode, true, null);
        }

        protected virtual (bool, string) ProcessDirectory(string projectPath, Dictionary<string, string> parameters)
        {
            string[] filePathsList = System.IO.Directory.GetFiles(projectPath, "*.al", System.IO.SearchOption.AllDirectories);
            for (int i = 0; i < filePathsList.Length; i++)
            {
                (bool success, string errorMessage) = this.ProcessFile(projectPath, filePathsList[i], parameters);
                if (!success)
                    return (false, errorMessage);
            }
            return (true, null);
        }

        protected virtual (bool, string) ProcessFile(string projectPath, string filePath, Dictionary<string, string> parameters)
        {
            string source = System.IO.File.ReadAllText(filePath);
            (string newSource, bool success, string errorMessage) = this.ProcessSourceCode(source, projectPath, filePath, null, parameters);
            if ((success) && (newSource != source) && (!String.IsNullOrWhiteSpace(newSource)))
                System.IO.File.WriteAllText(filePath, newSource);
            return (success, errorMessage);
        }

    }
}
