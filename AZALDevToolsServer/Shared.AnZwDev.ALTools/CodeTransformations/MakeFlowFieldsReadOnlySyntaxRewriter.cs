using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class MakeFlowFieldsReadOnlySyntaxRewriter : ALSyntaxRewriter
    {

        public MakeFlowFieldsReadOnlySyntaxRewriter()
        {
        }

        public override SyntaxNode VisitField(FieldSyntax node)
        {
            PropertyValueSyntax fieldClass = node.GetPropertyValue("FieldClass");
            if ((fieldClass != null) && (fieldClass.ToString().Equals("FlowField", StringComparison.CurrentCultureIgnoreCase)))
            {
                PropertySyntax propertySyntax = node.GetProperty("Editable");
                if (propertySyntax == null)
                {
                    NoOfChanges++;
                    node = node.AddPropertyListProperties(this.CreateReadOnlyProperty(node));
                }
            }
            return base.VisitField(node);
        }

        protected PropertySyntax CreateReadOnlyProperty(SyntaxNode node)
        {
            SyntaxTriviaList leadingTriviaList = node.CreateChildNodeIdentTrivia();
            SyntaxTriviaList trailingTriviaList = SyntaxFactory.ParseTrailingTrivia("\r\n", 0);

            return SyntaxFactory.Property(
                SyntaxFactory.PropertyName(SyntaxFactory.Identifier("Editable"))
                    .WithTrailingTrivia(SyntaxFactory.ParseTrailingTrivia(" ")),
                SyntaxFactory.BooleanPropertyValue(SyntaxFactory.BooleanLiteralValue(
                    SyntaxFactory.Token(SyntaxKind.FalseKeyword)))
                    .WithLeadingTrivia(SyntaxFactory.ParseLeadingTrivia(" ")))
                .WithLeadingTrivia(leadingTriviaList)
                .WithTrailingTrivia(trailingTriviaList);
        }



    }
}
