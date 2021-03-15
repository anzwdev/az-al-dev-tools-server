﻿using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.CodeTransformations;
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

        protected override SyntaxNode ProcessFile(SyntaxTree syntaxTree, SemanticModel semanticModel, Range range, Dictionary<string, string> parameters)
        {
            //stage 1 - update calls
            WithIdentifiersSyntaxRewriter identifiersRewriter = new WithIdentifiersSyntaxRewriter();
            identifiersRewriter.SemanticModel = semanticModel;
            SyntaxNode newNode = identifiersRewriter.Visit(syntaxTree.GetRoot());

            //stage 2 - remove "with" statements
            WithRemoveSyntaxRewriter withRemoveSyntaxRewriter = new WithRemoveSyntaxRewriter();
            newNode = withRemoveSyntaxRewriter.Visit(newNode);

            return this.FormatSyntaxNode(newNode);
        }

    }
#endif
}
