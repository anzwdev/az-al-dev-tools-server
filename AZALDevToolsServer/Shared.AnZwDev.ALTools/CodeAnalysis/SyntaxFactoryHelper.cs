using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeAnalysis
{
    public static class SyntaxFactoryHelper
    {

        public static IdentifierEqualsLiteralSyntax IdentifierEqualsLiteral(string identifier, string value)
        {
            return SyntaxFactory.IdentifierEqualsLiteral(identifier, SyntaxFactory.StringLiteralValue(SyntaxFactory.Literal(value)));
        }

        public static LabelSyntax Label(string labelText, string comment)
        {
            StringLiteralValueSyntax labelTextSyntax = SyntaxFactory.StringLiteralValue(SyntaxFactory.Literal(labelText));

            List<IdentifierEqualsLiteralSyntax> propertiesList = new List<IdentifierEqualsLiteralSyntax>();
            if (!String.IsNullOrWhiteSpace(comment))
                propertiesList.Add(SyntaxFactoryHelper.IdentifierEqualsLiteral("Comment", comment));

            if (propertiesList.Count > 0)
            {
                SeparatedSyntaxList<IdentifierEqualsLiteralSyntax> separatedSyntaxList = new SeparatedSyntaxList<IdentifierEqualsLiteralSyntax>();
                separatedSyntaxList = separatedSyntaxList.AddRange(propertiesList);
                CommaSeparatedIdentifierEqualsLiteralListSyntax propertiesListSyntax = SyntaxFactory.CommaSeparatedIdentifierEqualsLiteralList(separatedSyntaxList);
                return SyntaxFactory.Label(
                    labelTextSyntax, 
                    SyntaxFactory.Token(EnumExtensions.FromString("CommaToken", SyntaxKind.CommaToken)), 
                    propertiesListSyntax);
            }
            return SyntaxFactory.Label(labelTextSyntax);
        }

        public static PropertySyntax LabelProperty(PropertyKind propertyKind, string labelText, string comment)
        {
            LabelPropertyValueSyntax propertyValue = SyntaxFactory.LabelPropertyValue(SyntaxFactoryHelper.Label(labelText, comment));
            return SyntaxFactory.Property(propertyKind, propertyValue);
        }

        public static PropertySyntax CaptionProperty(string labelText, string comment)
        {
            PropertyKind propertyKind = EnumExtensions.FromString("Caption", PropertyKind.Caption);
            return SyntaxFactoryHelper.LabelProperty(propertyKind, labelText, comment);
        }

        public static PropertySyntax ToolTipProperty(string labelText, string comment)
        {
            PropertyKind propertyKind = EnumExtensions.FromString("ToolTip", PropertyKind.ToolTip);
            return SyntaxFactoryHelper.LabelProperty(propertyKind, labelText, comment);
        }

    }
}
