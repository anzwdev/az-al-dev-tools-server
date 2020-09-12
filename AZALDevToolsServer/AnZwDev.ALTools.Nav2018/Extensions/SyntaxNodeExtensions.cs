/****************************************************************
 *                                                              *
 * Legacy version of the library maintained to support Nav 2018 *
 *                                                              *
 ****************************************************************/
using AnZwDev.ALTools.Nav2018.ALSymbols;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AnZwDev.ALTools.Nav2018.Extensions
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

        public static PropertySyntax GetProperty(this SyntaxNode node, string name)
        {
            PropertyListSyntax propertyList = node.TryGetValueOf<PropertyListSyntax>(name);
            if (propertyList != null)
            {
                foreach (PropertySyntax property in propertyList.Properties)
                {
                    if (property.Name.Identifier.ValueText == name)
                        return property;
                }
            }
            return null;
        }

        public static PropertyValueSyntax GetPropertyValue(this SyntaxNode node, string name)
        {
            PropertySyntax property = node.GetProperty(name);
            if (property != null)
                return property.Value;
            return null;
        }

        public static string GetNameStringValue(this SyntaxNode node)
        {
            string name = node.GetSyntaxNodeName();
            if (!String.IsNullOrWhiteSpace(name))
                name = ALSyntaxHelper.DecodeName(name);
            return name;
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
