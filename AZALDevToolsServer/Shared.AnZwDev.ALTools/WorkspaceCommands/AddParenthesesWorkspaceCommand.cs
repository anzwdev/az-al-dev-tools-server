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

    public class AddParenthesesWorkspaceCommand : SemanticModelWorkspaceCommand
    {

        public AddParenthesesWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "addParentheses")
        {
        }

        protected override SyntaxNode ProcessFile(SyntaxTree syntaxTree, SemanticModel semanticModel, ALProject project, Range range, Dictionary<string, string> parameters)
        {
            AddParenthesesSyntaxRewriter addParenthesesSyntaxRewriter = new AddParenthesesSyntaxRewriter();
            addParenthesesSyntaxRewriter.SemanticModel = semanticModel;
            addParenthesesSyntaxRewriter.Project = project;

            SyntaxNode newNode = addParenthesesSyntaxRewriter.Visit(syntaxTree.GetRoot());
            return this.FormatSyntaxNode(newNode);
        }

    }

#endif

}
