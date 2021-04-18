using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class ALCaptionsSyntaxRewriter : ALSyntaxRewriter
    {

        public ALCaptionsSyntaxRewriter()
        {
        }

        protected T UpdateCaptionFromName<T>(T node, PropertySyntax oldCaptionPropertySyntax) where T : SyntaxNode
        {
            string valueText = oldCaptionPropertySyntax.Value.ToString();
            if (String.IsNullOrWhiteSpace(valueText))
            {
                NoOfChanges++;
                return node.ReplaceNode(oldCaptionPropertySyntax, this.CreateCaptionPropertyFromName(node));
            }
            return node;
        }

        protected PropertySyntax CreateCaptionPropertyFromName(SyntaxNode node)
        {
            string value = node.GetNameStringValue().RemovePrefixSuffix(this.Project.MandatoryPrefixes, this.Project.MandatorySuffixes, this.Project.MandatoryAffixes);

            SyntaxTriviaList leadingTriviaList = node.CreateChildNodeIdentTrivia();
            SyntaxTriviaList trailingTriviaList = SyntaxFactory.ParseTrailingTrivia("\r\n", 0);

            PropertyKind propertyKind;
            try
            {
                propertyKind = (PropertyKind)Enum.Parse(typeof(PropertyKind), "Caption", true);
            }
            catch (Exception)
            {
                propertyKind = PropertyKind.Caption;
            }

            return SyntaxFactory.PropertyLiteral(propertyKind, value)
                .WithLeadingTrivia(leadingTriviaList)
                .WithTrailingTrivia(trailingTriviaList);
        }

    }
}
