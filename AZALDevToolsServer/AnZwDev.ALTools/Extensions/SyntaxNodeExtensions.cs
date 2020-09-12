using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AnZwDev.ALTools.Extensions
{
    public static class SyntaxNodeExtensions
    {

        public static string GetSyntaxNodeName(this SyntaxNode node)
        {
            return node.GetType().TryGetPropertyValueAsString(node, "Name");
        }

        public static T TryGetValueOf<T>(this SyntaxNode node, string propertyName)
        {
            return node.GetType().TryGetPropertyValue<T>(node, propertyName);
        }

        public static bool HasProperty(this SyntaxNode node, string propertyName, string emptyValue = null)
        {
            PropertySyntax propertySyntax = node.GetProperty(propertyName);
            return ((propertySyntax != null) &&
                (!String.IsNullOrWhiteSpace(propertySyntax.Value.ToString())) &&
                (
                    (emptyValue == null) ||
                    (!emptyValue.Equals(propertySyntax.Value.ToString(), StringComparison.CurrentCultureIgnoreCase))));
        }

        public static SyntaxTriviaList CreateChildNodeIdentTrivia(this SyntaxNode node)
        {
            //calculate indent
            int indentLength = 4;
            string indent;
            SyntaxTriviaList leadingTrivia = node.GetLeadingTrivia();
            if (leadingTrivia != null)
            {
                indent = leadingTrivia.ToString();
                int newLinePos = indent.LastIndexOf("/n");
                if (newLinePos >= 0)
                    indent = indent.Substring(newLinePos + 1);
                indentLength += indent.Length;
            }
            indent = "".PadLeft(indentLength);

            return SyntaxFactory.ParseLeadingTrivia(indent, 0);
        }

    }
}
