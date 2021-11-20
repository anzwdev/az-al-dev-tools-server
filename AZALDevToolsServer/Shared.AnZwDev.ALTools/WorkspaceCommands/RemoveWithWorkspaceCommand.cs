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
    public class RemoveWithWorkspaceCommand: SemanticModelWorkspaceCommand
    {

        public RemoveWithWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "removeWith")
        {
        }

        public override SyntaxNode ProcessSyntaxNode(SyntaxTree syntaxTree, SyntaxNode node, SemanticModel semanticModel, ALProject project, TextSpan span, Dictionary<string, string> parameters)
        {
            if (node != null)
            {
                //stage 1 - update calls
                WithIdentifiersSyntaxRewriter identifiersRewriter = new WithIdentifiersSyntaxRewriter();
                identifiersRewriter.SemanticModel = semanticModel;
                node = identifiersRewriter.Visit(node);

                //stage 2 - remove "with" statements
                if (node != null)
                {
                    WithRemoveSyntaxRewriter withRemoveSyntaxRewriter = new WithRemoveSyntaxRewriter();
                    node = withRemoveSyntaxRewriter.Visit(node);
                }
            }

            return base.ProcessSyntaxNode(syntaxTree, node, semanticModel, project, span, parameters);
        }

    }
#endif
}
