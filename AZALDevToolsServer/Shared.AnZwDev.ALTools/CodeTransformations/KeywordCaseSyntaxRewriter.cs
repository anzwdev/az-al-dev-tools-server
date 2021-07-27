using AnZwDev.ALTools.ALSymbols.Internal;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class KeywordCaseSyntaxRewriter : ALSyntaxRewriter
    {

        public KeywordCaseSyntaxRewriter()
        {
        }

        public override SyntaxToken VisitToken(SyntaxToken token)
        {
            if (token.Kind.IsKeyword())
                token = SyntaxFactory.Token(token.Kind).WithLeadingTrivia(token.LeadingTrivia).WithTrailingTrivia(token.TrailingTrivia);
            return base.VisitToken(token);
        }

    }
}
