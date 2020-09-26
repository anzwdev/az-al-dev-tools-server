/****************************************************************
 *                                                              *
 * Legacy version of the library maintained to support Nav 2018 *
 *                                                              *
 ****************************************************************/
using AnZwDev.ALTools.Nav2018.ALSymbols;
using AnZwDev.ALTools.Nav2018.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Packaging;
using Microsoft.Dynamics.Nav.CodeAnalysis.SymbolReference;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Nav2018.CodeTransformations
{
    public class ALSyntaxRewriter : SyntaxRewriter
    {

        public int NoOfChanges { get; set; }
        public int TotalNoOfChanges { get; set; }
        public int NoOfChangedFiles { get; set; }
        public TextSpan Span { get; set; }

        public ALSyntaxRewriter()
        {
            this.NoOfChanges = 0;
            this.NoOfChangedFiles = 0;
            this.TotalNoOfChanges = 0;
            this.Span = new TextSpan(0, 0);
        }

        public SyntaxNode ProcessNode(SyntaxNode node)
        {
            this.NoOfChanges = 0;
            node = this.Visit(node);
            node = this.AfterVisitNode(node);

            if (node != null)
            {
                this.TotalNoOfChanges += this.NoOfChanges;
                if (this.NoOfChanges > 0)
                    this.NoOfChangedFiles++;
            }

            return node;
        }

        protected virtual SyntaxNode AfterVisitNode(SyntaxNode node)
        {
            return node;
        }

        protected bool NodeInSpan(SyntaxNode node)
        {
            return ((this.Span.IsEmpty) || (this.Span.Contains(node.FullSpan)));
        }

    }
}
