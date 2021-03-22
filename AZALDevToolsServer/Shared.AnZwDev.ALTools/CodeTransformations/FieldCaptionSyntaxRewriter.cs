using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class FieldCaptionSyntaxRewriter : ALSyntaxRewriter
    {

        public FieldCaptionSyntaxRewriter()
        {
        }

        public override SyntaxNode VisitField(FieldSyntax node)
        {
            PropertySyntax propertySyntax = node.GetProperty("Caption");
            if (propertySyntax == null)
            {
                NoOfChanges++;
                return node.AddPropertyListProperties(
                    this.CreateCaptionProperty(node));
            }
            else
            {
                string valueText = propertySyntax.Value.ToString();
                if (String.IsNullOrWhiteSpace(valueText))
                {
                    NoOfChanges++;
                    return node.ReplaceNode(propertySyntax, this.CreateCaptionProperty(node));
                }
            }
            return base.VisitField(node);
        }

        protected PropertySyntax CreateCaptionProperty(SyntaxNode node)
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
