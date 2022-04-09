﻿using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class SortVariablesWorkspaceCommand : SyntaxRewriterWorkspaceCommand<SortVariablesSyntaxRewriter>
    {

        public static string SortModeParameterName = "sortMode";

        public SortVariablesWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "sortVariables")
        {
        }

        protected override void SetParameters(string sourceCode, string projectPath, string filePath, TextSpan span, Dictionary<string, string> parameters)
        {
            base.SetParameters(sourceCode, projectPath, filePath, span, parameters);
            this.SyntaxRewriter.SortByMainTypeNameOnly = (parameters.GetStringValue(SortModeParameterName) == "mainTypeNameOnly");
        }


    }
}
