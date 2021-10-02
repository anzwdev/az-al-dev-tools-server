using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.ALTools.Workspace;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
#if BC

    public class FixIdentifiersCaseWorkspaceCommand : SemanticModelWorkspaceCommand
    {

        public FixIdentifiersCaseWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "fixIdentifiersCase")
        {
        }

        protected override SyntaxNode ProcessFile(SyntaxTree syntaxTree, SemanticModel semanticModel, ALProject project, Range range, Dictionary<string, string> parameters)
        {
            IdentifierCaseSyntaxRewriter identifierCaseSyntaxRewriter = new IdentifierCaseSyntaxRewriter();
            identifierCaseSyntaxRewriter.SemanticModel = semanticModel;
            identifierCaseSyntaxRewriter.Project = project;

            SyntaxNode newNode = identifierCaseSyntaxRewriter.Visit(syntaxTree.GetRoot());
            return this.FormatSyntaxNode(newNode);
        }

    }

#endif
}
