using Microsoft.Dynamics.Nav.CodeAnalysis;
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

    }
}
