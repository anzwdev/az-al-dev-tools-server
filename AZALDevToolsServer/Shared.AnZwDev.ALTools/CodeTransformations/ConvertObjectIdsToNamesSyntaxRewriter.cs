using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.ALSymbols.Internal;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class ConvertObjectIdsToNamesSyntaxRewriter : ALSyntaxRewriter
    {

        public ConvertObjectIdsToNamesSyntaxRewriter()
        {
        }

        public override SyntaxNode VisitAttributeArgumentList(AttributeArgumentListSyntax node)
        {
            return base.VisitAttributeArgumentList(node);
        }

        public override SyntaxNode VisitLiteralAttributeArgument(LiteralAttributeArgumentSyntax node)
        {
            return base.VisitLiteralAttributeArgument(node);
        }

        public override SyntaxNode VisitMemberAttribute(MemberAttributeSyntax node)
        {
            string name = node.Name?.Identifier.ValueText;
            if ((!String.IsNullOrWhiteSpace(name)) && (name.Equals("EventSubscriber", StringComparison.CurrentCultureIgnoreCase)) && (node.ArgumentList != null))
            {
                SeparatedSyntaxList<AttributeArgumentSyntax> arguments = node.ArgumentList.Arguments;
                if ((arguments != null) && (arguments.Count >= 2))
                {
                    OptionAccessAttributeArgumentSyntax objectTypeArgument = arguments[0] as OptionAccessAttributeArgumentSyntax;
                    LiteralAttributeArgumentSyntax objectNameOrIdArgument = arguments[1] as LiteralAttributeArgumentSyntax;
                    if ((objectTypeArgument != null) && (objectNameOrIdArgument != null))
                    {
                        string objectType = objectTypeArgument?.OptionAccess?.Name?.Identifier.ValueText;
                        string prevValue = objectNameOrIdArgument.ToString();
                        string newValue = prevValue;
                        if (Int32.TryParse(prevValue, out int intValue))
                        {
                            ALAppObject alAppObject = this.FindObjectById(objectType, intValue);
                            if (alAppObject != null)
                                newValue = alAppObject.Name;
                        }

                        if ((prevValue != newValue) && (!String.IsNullOrWhiteSpace(newValue)))
                        {
                            OptionAccessAttributeArgumentSyntax newObjectNameOrIdArgument = SyntaxFactory.OptionAccessAttributeArgument(
                                SyntaxFactory.OptionAccessExpression(
                                    SyntaxFactory.IdentifierName(SyntaxFactory.Identifier(this.ObjectTypeNameToEnumName(objectType))),
                                    SyntaxFactory.IdentifierName(SyntaxFactory.Identifier(newValue)))).WithTriviaFrom(objectNameOrIdArgument);
                            AttributeArgumentListSyntax argumentsList = node.ArgumentList.WithArguments(arguments.Replace(objectNameOrIdArgument, newObjectNameOrIdArgument));
                            node = node.WithArgumentList(argumentsList);
                        }

                    }
                }
            }

            return base.VisitMemberAttribute(node);
        }

        public override SyntaxNode VisitObjectNameOrId(ObjectNameOrIdSyntax node)
        {
            if (node.Identifier.Kind.ConvertToLocalType() == ConvertedSyntaxKind.ObjectId)
            {
                ObjectIdSyntax objectId = node.Identifier as ObjectIdSyntax;
                if (objectId != null)
                {
                    string idText = objectId.Value.ValueText;
                    string newName = idText;
                    if (Int32.TryParse(idText, out int idValue))
                    {
                        if (node.Parent.Kind.ConvertToLocalType() == ConvertedSyntaxKind.SubtypedDataType)
                        {
                            SubtypedDataTypeSyntax dataTypeNode = node.Parent as SubtypedDataTypeSyntax;
                            if ((dataTypeNode != null) && (dataTypeNode.TypeName != null) && (!dataTypeNode.ContainsDiagnostics))
                            {
                                ALAppObject alAppObject = this.FindObjectById(dataTypeNode.TypeName.ValueText, idValue);
                                if (alAppObject != null)
                                    newName = alAppObject.Name;
                            }
                        }
                    }

                    if ((newName != idText) && (!String.IsNullOrWhiteSpace(newName)))
                    {
                        SyntaxToken objectNameValue = SyntaxFactory.Identifier(newName).WithTriviaFrom(objectId.Value);
                        IdentifierNameSyntax objectName = SyntaxFactory.IdentifierName(objectNameValue).WithTriviaFrom(objectId);
                        ObjectNameOrIdSyntax newNode = SyntaxFactory.ObjectNameOrId(objectName).WithTriviaFrom(node);
                        return newNode;
                    }
                }
            }

            return base.VisitObjectNameOrId(node);
        }

        protected ALAppObject FindObjectById(string objectType, int objectId)
        {
            ALSymbolKind alSymbolKind = this.TypeNameToSymbolKind(objectType);
            if (alSymbolKind == ALSymbolKind.Undefined)
                return null;
            ALAppObject alAppObject = this.Project.Symbols.FindObjectById(alSymbolKind, objectId, false);
            if (alAppObject != null)
                return alAppObject;
            if (this.Project.Dependencies != null)
            {
                for (int i=0; i<this.Project.Dependencies.Count; i++)
                {
                    if (this.Project.Dependencies[i].Symbols != null)
                    {
                        alAppObject = this.Project.Dependencies[i].Symbols.FindObjectById(alSymbolKind, objectId, false);
                        if (alAppObject != null)
                            return alAppObject;
                    }
                }
            }
            return null;
        }

        protected ALSymbolKind TypeNameToSymbolKind(string typeName)
        {
            typeName = typeName.ToLower();
            switch (typeName)
            {
                case "table": return ALSymbolKind.TableObject;
                case "record": return ALSymbolKind.TableObject;
                case "page": return ALSymbolKind.PageObject;
                case "report": return ALSymbolKind.ReportObject;
                case "xmlport": return ALSymbolKind.XmlPortObject;
                case "query": return ALSymbolKind.QueryObject;
                case "codeunit": return ALSymbolKind.CodeunitObject;
                case "controladdin": return ALSymbolKind.ControlAddInObject;
                case "enum": return ALSymbolKind.EnumType;
                case "permissionset": return ALSymbolKind.PermissionSet;
                    //case "profile": return ALSymbolKind.ProfileObject;
                    //case "dotnet": return ALSymbolKind.DotNetPackage;
                    //case "interface": return ALSymbolKind.Interface;
            }
            return ALSymbolKind.Undefined;
        }

        protected string ObjectTypeNameToEnumName(string typeName)
        {
            typeName = typeName.ToLower();
            switch (typeName)
            {
                case "table": return "Database";
                case "record": return "Database";
                case "page": return "Page";
                case "report": return "Report";
                case "xmlport": return "XmlPort";
                case "query": return "Query";
                case "codeunit": return "Codeunit";
                case "enum": return "Enum";
            }
            return typeName;
        }

    }
}
