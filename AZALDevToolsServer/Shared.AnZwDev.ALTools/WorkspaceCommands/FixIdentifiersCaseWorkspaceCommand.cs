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

        public override SyntaxNode ProcessSyntaxNode(SyntaxTree syntaxTree, SyntaxNode node, SemanticModel semanticModel, ALProject project, TextSpan span, Dictionary<string, string> parameters)
        {
            if (node != null)
            {
                IdentifierCaseSyntaxRewriter identifierCaseSyntaxRewriter = new IdentifierCaseSyntaxRewriter();
                identifierCaseSyntaxRewriter.SemanticModel = semanticModel;
                identifierCaseSyntaxRewriter.Project = project;

                node = identifierCaseSyntaxRewriter.Visit(node);
            }

            return base.ProcessSyntaxNode(syntaxTree, node, semanticModel, project, span, parameters);
        }

    }

#endif
}
