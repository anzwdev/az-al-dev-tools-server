using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Packaging;
using Microsoft.Dynamics.Nav.CodeAnalysis.SymbolReference;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class ALSyntaxRewriter : SyntaxRewriter
    {

        public int NoOfChanges { get; set; }
        public int TotalNoOfChanges { get; set; }
        public int NoOfChangedFiles { get; set; }
        public Range Range { get; set; }

        public ALSyntaxRewriter()
        {
            this.NoOfChanges = 0;
            this.NoOfChangedFiles = 0;
            this.TotalNoOfChanges = 0;
            this.Range = null;
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

    }
}
