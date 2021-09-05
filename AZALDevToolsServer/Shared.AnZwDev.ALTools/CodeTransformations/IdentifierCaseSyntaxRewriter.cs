using AnZwDev.ALTools.Logging;
using AnZwDev.ALTools.ALSymbols.Internal;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;
using AnZwDev.ALTools.Extensions;

namespace AnZwDev.ALTools.CodeTransformations
{
    /// <summary>
    /// Advanced case syntax rewriter
    /// Can fix case of variables, fields and function names
    /// It is much slower and complex solution than KeywordCaseSyntaxRewriter,
    /// but can fix much more cases
    /// </summary>
    public class IdentifierCaseSyntaxRewriter : KeywordCaseSyntaxRewriter
    {

        public SemanticModel SemanticModel { get; set; }

        public IdentifierCaseSyntaxRewriter()
        {
        }

        public override SyntaxNode VisitIdentifierName(IdentifierNameSyntax node)
        {
            if (!node.ContainsDiagnostics)
            {
                try
                {
                    string prevName = node.Identifier.ValueText;
                    string newName = prevName;
                    if (!String.IsNullOrWhiteSpace(prevName))
                    {
                        SymbolInfo info = this.SemanticModel.GetSymbolInfo(node);
                        if ((info != null) && (info.Symbol != null))
                            newName = info.Symbol.Name;

                        if ((prevName != newName) && (!String.IsNullOrWhiteSpace(newName)))
                        {
                            SyntaxToken identifier = node.Identifier;
                            node = node.WithIdentifier(
                                SyntaxFactory.Identifier(newName)
                                    .WithLeadingTrivia(identifier.LeadingTrivia)
                                    .WithTrailingTrivia(identifier.TrailingTrivia));
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageLog.LogError(e);
                }
            }

            return base.VisitIdentifierName(node);
        }

    }
}
