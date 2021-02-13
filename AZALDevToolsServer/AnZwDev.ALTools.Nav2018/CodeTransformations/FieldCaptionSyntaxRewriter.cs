using AnZwDev.ALTools.Nav2018.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Nav2018.CodeTransformations
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
            string value = node.GetNameStringValue();

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
