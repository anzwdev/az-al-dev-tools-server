using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class RemoveEmptySectionsSyntaxRewriter : ALSyntaxRewriter
    {

        public RemoveEmptySectionsSyntaxRewriter()
        {
        }

        public override SyntaxNode VisitPageExtensionLayout(PageExtensionLayoutSyntax node)
        {
            return base.VisitPageExtensionLayout(node);
        }

        public override SyntaxNode VisitPage(PageSyntax node)
        {
            return base.VisitPage(node);
        }

        public override SyntaxNode VisitRequestPage(RequestPageSyntax node)
        {
            return base.VisitRequestPage(node);
        }

    }
}
