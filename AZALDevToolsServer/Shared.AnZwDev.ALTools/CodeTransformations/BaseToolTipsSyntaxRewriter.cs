using AnZwDev.ALTools.CodeAnalysis;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class BaseToolTipsSyntaxRewriter : BasePageWithSourceSyntaxRewriter
    {

        public BaseToolTipsSyntaxRewriter()
        {
        }

        protected PageFieldSyntax SetPageFieldToolTip(PageFieldSyntax node, string newToolTip)
        {
            PropertySyntax propertySyntax = node.GetProperty("ToolTip");
            if (propertySyntax == null)
            {
                SyntaxTriviaList leadingTriviaList = node.CreateChildNodeIdentTrivia();
                SyntaxTriviaList trailingTriviaList = SyntaxFactory.ParseTrailingTrivia("\r\n", 0);
                return node.AddPropertyListProperties(
                    SyntaxFactoryHelper.ToolTipProperty(newToolTip, "", false)
                        .WithLeadingTrivia(leadingTriviaList)
                        .WithTrailingTrivia(trailingTriviaList));
            }
            else
            {
                bool validValue = true;
                if (propertySyntax.Value is LabelPropertyValueSyntax labelValue)
                    validValue = ((labelValue.Value?.LabelText?.Value.Value?.ToString()) != newToolTip);
                if (validValue)
                {
                    PropertySyntax newPropertySyntax = SyntaxFactoryHelper.ToolTipProperty(newToolTip, "", false)
                        .WithTriviaFrom(propertySyntax);
                    return node.ReplaceNode(propertySyntax, newPropertySyntax);
                }
            }
            return null;
        }

        protected PropertySyntax CreateToolTipProperty(string toolTipValue, SyntaxTriviaList leadingTriviaList, SyntaxTriviaList trailingTriviaList)
        {
            return SyntaxFactoryHelper.ToolTipProperty(toolTipValue, "", false)
                .WithLeadingTrivia(leadingTriviaList)
                .WithTrailingTrivia(trailingTriviaList);
        }


    }
}
