using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Workspace;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{

#if BC
    public class RemoveUnusedVariablesWorkspaceCommand : SemanticModelWorkspaceCommand
    {

        public static string RemoveGlobalVariablesParameterName = "removeGlobalVariables";
        public static string RemoveLocalVariablesParameterName = "removeLocalVariables";
        public static string RemoveLocalMethodParametersParameterName = "removeLocalMethodParameters";

        public RemoveUnusedVariablesWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "removeUnusedVariables")
        {
        }

        protected override SyntaxNode ProcessFile(SyntaxTree syntaxTree, SemanticModel semanticModel, ALProject project, Range range, Dictionary<string, string> parameters)
        {
            RemoveUnusedVariablesSyntaxRewriter removeUnusedVariablesSyntaxRewriter = new RemoveUnusedVariablesSyntaxRewriter();
            removeUnusedVariablesSyntaxRewriter.SemanticModel = semanticModel;
            removeUnusedVariablesSyntaxRewriter.Project = project;
            removeUnusedVariablesSyntaxRewriter.RemoveGlobalVariables = parameters.GetBoolValue(RemoveGlobalVariablesParameterName);
            removeUnusedVariablesSyntaxRewriter.RemoveLocalVariables = parameters.GetBoolValue(RemoveLocalVariablesParameterName);
            removeUnusedVariablesSyntaxRewriter.RemoveLocalMethodParameters = parameters.GetBoolValue(RemoveLocalMethodParametersParameterName);

            SyntaxNode newNode = removeUnusedVariablesSyntaxRewriter.Visit(syntaxTree.GetRoot());
            return this.FormatSyntaxNode(newNode);
        }

    }
#endif

}
