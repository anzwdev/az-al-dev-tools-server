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
            {
                SyntaxTriviaList leadingTrivia = token.LeadingTrivia;
                SyntaxTriviaList trailingTrivia = token.TrailingTrivia;
                bool parsed = false;

#if BC
                ConvertedSyntaxKind kind = token.Kind.ConvertToLocalType();
                SyntaxNode parentToken = token.Parent;
                ConvertedSyntaxKind parentKind = parentToken.Kind.ConvertToLocalType();
                if ((parentKind == ConvertedSyntaxKind.SubtypedDataType) || (parentKind == ConvertedSyntaxKind.EnumDataType) || (parentKind == ConvertedSyntaxKind.LabelDataType))
                {
                    switch (kind)
                    {
                        case ConvertedSyntaxKind.CodeunitKeyword:
                            token = SyntaxFactory.ParseKeyword("Codeunit");
                            parsed = true;
                            break;
                        case ConvertedSyntaxKind.PageKeyword:
                            token = SyntaxFactory.ParseKeyword("Page");
                            parsed = true;
                            break;
                        case ConvertedSyntaxKind.XmlPortKeyword:
                            token = SyntaxFactory.ParseKeyword("XmlPort");
                            parsed = true;
                            break;
                        case ConvertedSyntaxKind.QueryKeyword:
                            token = SyntaxFactory.ParseKeyword("Query");
                            parsed = true;
                            break;
                        case ConvertedSyntaxKind.ReportKeyword:
                            token = SyntaxFactory.ParseKeyword("Report");
                            parsed = true;
                            break;
                        case ConvertedSyntaxKind.InterfaceKeyword:
                            token = SyntaxFactory.ParseKeyword("Interface");
                            parsed = true;
                            break;
                        case ConvertedSyntaxKind.DotNetKeyword:
                            token = SyntaxFactory.ParseKeyword("DotNet");
                            parsed = true;
                            break;
                        case ConvertedSyntaxKind.ControlAddInKeyword:
                            token = SyntaxFactory.ParseKeyword("ControlAddIn");
                            parsed = true;
                            break;
                        case ConvertedSyntaxKind.LabelKeyword:
                            token = SyntaxFactory.ParseKeyword("Label");
                            parsed = true;
                            break;
                        case ConvertedSyntaxKind.EnumKeyword:
                            token = SyntaxFactory.ParseKeyword("Enum");
                            parsed = true;
                            break;
                    }
                }
#endif

                if (!parsed)
                    token = SyntaxFactory.Token(token.Kind);
                token = token.WithLeadingTrivia(leadingTrivia).WithTrailingTrivia(trailingTrivia);
            }
            return base.VisitToken(token);
        }

    }
}
