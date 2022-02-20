using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using AnZwDev.ALTools.ALSymbols.Internal;
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

        public static bool HasProperty(this SyntaxNode node, string propertyName)
        {
            return (node.GetProperty(propertyName) != null);
        }

        public static bool HasNonEmptyProperty(this SyntaxNode node, string propertyName, string emptyValue = null)
        {
            PropertySyntax propertySyntax = node.GetProperty(propertyName);
            return ((propertySyntax != null) &&
                (propertySyntax.Value != null) &&
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

        public static LabelInformation GetCaptionPropertyInformation(this SyntaxNode node)
        {
            return node.GetLabelPropertyInformation("Caption");
        }

        public static LabelInformation GetLabelPropertyInformation(this SyntaxNode node, string name)
        {
            PropertySyntax propertySyntax = node.GetProperty(name);
            if ((propertySyntax != null) && (propertySyntax.Value != null))
            {
                LabelPropertyValueSyntax labelPropertyValue = propertySyntax.Value as LabelPropertyValueSyntax;
                if ((labelPropertyValue != null) && (labelPropertyValue.Value != null))
                {
                    LabelSyntax labelSyntax = labelPropertyValue.Value;
                    LabelInformation labelInformation = new LabelInformation(name);

                    //get label text
                    if (labelSyntax.LabelText != null)
                        labelInformation.Value = ALSyntaxHelper.DecodeString(labelSyntax.LabelText.ToString());

                    //add property arguments
                    if ((labelSyntax.Properties != null) && (labelSyntax.Properties.Values != null))
                    {
                        foreach (IdentifierEqualsLiteralSyntax labelPropertySyntax in labelSyntax.Properties.Values)
                        {
                            if ((labelPropertySyntax.Identifier != null) && (labelPropertySyntax.Literal != null))
                            {
                                labelInformation.SetProperty(
                                    labelPropertySyntax.Identifier.ToString().Trim(),
                                    ALSyntaxHelper.DecodeStringOrName(labelPropertySyntax.Literal.ToString()));
                            }
                        }
                    }

                    return labelInformation;
                }
            }

            return null;
        }

        public static bool IsInsideCodeBlock(this SyntaxNode node)
        {
            while (node != null)
            {
                var kind = node.Kind.ConvertToLocalType();
                switch (kind)
                {
                    case ConvertedSyntaxKind.Block:
                    case ConvertedSyntaxKind.IfStatement:
                    case ConvertedSyntaxKind.ForStatement:
                    case ConvertedSyntaxKind.ForEachStatement:
                    case ConvertedSyntaxKind.RepeatStatement:
                    case ConvertedSyntaxKind.WhileStatement:
                    case ConvertedSyntaxKind.CaseStatement:
                        return true;
                    case ConvertedSyntaxKind.CodeunitObject:
                    case ConvertedSyntaxKind.ControlAddInObject:
                    case ConvertedSyntaxKind.PageObject:
                    case ConvertedSyntaxKind.TableObject:
                    case ConvertedSyntaxKind.ReportObject:
                    case ConvertedSyntaxKind.QueryObject:
                    case ConvertedSyntaxKind.XmlPortObject:
                    case ConvertedSyntaxKind.TableExtensionObject:
                    case ConvertedSyntaxKind.PageExtensionObject:
                    case ConvertedSyntaxKind.PermissionSet:
                    case ConvertedSyntaxKind.PermissionSetExtension:
                    case ConvertedSyntaxKind.EnumType:
                    case ConvertedSyntaxKind.EnumExtensionType:
                    case ConvertedSyntaxKind.ProfileExtensionObject:
                    case ConvertedSyntaxKind.ProfileObject:
                    case ConvertedSyntaxKind.PageCustomizationObject:
                    case ConvertedSyntaxKind.Interface:
                    case ConvertedSyntaxKind.CompilationUnit:
                        return false;
                }
                node = node.Parent;
            }
            return false;
        }

        #region Nav2018 helpers

#if NAV2018

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

#endif

        #endregion


    }
}
