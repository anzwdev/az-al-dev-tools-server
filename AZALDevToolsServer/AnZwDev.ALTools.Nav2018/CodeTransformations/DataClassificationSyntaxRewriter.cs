/****************************************************************
 *                                                              *
 * Legacy version of the library maintained to support Nav 2018 *
 *                                                              *
 ****************************************************************/
using AnZwDev.ALTools.Nav2018.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Nav2018.CodeTransformations
{
    public class DataClassificationSyntaxRewriter : ALSyntaxRewriter
    {

        public string DataClassification { get; set; }

        public DataClassificationSyntaxRewriter()
        {
            this.DataClassification = null;
        }

        public override SyntaxNode VisitTable(TableSyntax node)
        {
            PropertySyntax propertySyntax = node.GetProperty("DataClassification");
            if (propertySyntax == null)
            {
                NoOfChanges++;
                node = node.AddPropertyListProperties(
                    this.CreateDataClassificationProperty(node));
            }
            else
            {
                string valueText = propertySyntax.Value.ToString();
                if ((String.IsNullOrWhiteSpace(valueText)) ||
                    (valueText.Equals("ToBeClassified", StringComparison.CurrentCultureIgnoreCase)))
                {
                    NoOfChanges++;
                    node = node.ReplaceNode(propertySyntax, this.CreateDataClassificationProperty(node));
                }
            }

            return base.VisitTable(node);
        }

        public override SyntaxNode VisitField(FieldSyntax node)
        {
            PropertySyntax propertySyntax = node.GetProperty("DataClassification");
            if (propertySyntax == null)
            {
                NoOfChanges++;
                return node.AddPropertyListProperties(
                    this.CreateDataClassificationProperty(node));
            } else
            {
                string valueText = propertySyntax.Value.ToString();
                if ((String.IsNullOrWhiteSpace(valueText)) ||
                    (valueText.Equals("ToBeClassified", StringComparison.CurrentCultureIgnoreCase)))
                {
                    NoOfChanges++;
                    return node.ReplaceNode(propertySyntax, this.CreateDataClassificationProperty(node));
                }
            }

            return base.VisitField(node);
        }

        protected PropertySyntax CreateDataClassificationProperty(SyntaxNode node)
        {
            SyntaxTriviaList leadingTriviaList = node.CreateChildNodeIdentTrivia();
            SyntaxTriviaList trailingTriviaList = SyntaxFactory.ParseTrailingTrivia("\r\n", 0);

            return SyntaxFactory.Property(
                SyntaxFactory.PropertyName(SyntaxFactory.Identifier("DataClassification"))
                    .WithTrailingTrivia(SyntaxFactory.ParseTrailingTrivia(" ")),
                SyntaxFactory.EnumPropertyValue(SyntaxFactory.IdentifierName(this.DataClassification))
                    .WithLeadingTrivia(SyntaxFactory.ParseLeadingTrivia(" ")))
                .WithLeadingTrivia(leadingTriviaList)                
                .WithTrailingTrivia(trailingTriviaList);
        }


    }
}
