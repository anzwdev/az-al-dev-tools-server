﻿using AnZwDev.ALTools.ALSymbols.Internal;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols.SymbolReaders
{
    public class ALSymbolInfoSyntaxTreeReader
    {

        public bool IncludeProperties { get; set; }
       
        public ALSymbolInfoSyntaxTreeReader(bool includeProperties)
        {
            this.IncludeProperties = includeProperties;
        }

        #region Main processing

        public ALSymbolInformation ProcessSourceFile(string path)
        {
            string sourceCode;
            try
            {
                System.IO.StreamReader reader = new System.IO.StreamReader(path);
                sourceCode = reader.ReadToEnd();
                reader.Close();
                reader.Dispose();

                return ProcessSourceCode(sourceCode);
            }
            catch (Exception e)
            {
                return new ALSymbolInformation(ALSymbolKind.Undefined, "LangServer Error: " + e.Message);
            }
        }

        public ALSymbolInformation ProcessSourceCode(string source)
        {
            SyntaxTree sourceTree = SyntaxTree.ParseObjectText(source);
            return ProcessSyntaxTree(sourceTree);
        }

        public ALSymbolInformation ProcessSyntaxTree(SyntaxTree syntaxTree)
        {
            SyntaxNode node = syntaxTree.GetRoot();
            ALSymbolInformation root = new ALSymbolInformation();
            ProcessChildSyntaxNode(syntaxTree, root, node);

            root.UpdateFields();

            if ((root.childSymbols != null) && (root.childSymbols.Count == 1))
                return root.childSymbols[0];

            return root;
        }

        protected ALSymbolInformation CreateSymbolInfo(SyntaxTree syntaxTree, SyntaxNode node)
        {
            //Detect symbol type
            ALSymbolKind alSymbolKind = SyntaxKindToALSymbolKind(node);
            if (alSymbolKind == ALSymbolKind.Undefined)
                return null;

            //create symbol info
            Type nodeType = node.GetType();
            ALSymbolInformation symbolInfo = new ALSymbolInformation();
            symbolInfo.name = ALSyntaxHelper.DecodeName(nodeType.TryGetPropertyValueAsString(node, "Name"));
            symbolInfo.kind = alSymbolKind;

            if (node.ContainsDiagnostics)
                symbolInfo.containsDiagnostics = true;

            var lineSpan = syntaxTree.GetLineSpan(node.FullSpan);
            symbolInfo.range = new Range(lineSpan.StartLinePosition.Line, lineSpan.StartLinePosition.Character,
                lineSpan.EndLinePosition.Line, lineSpan.EndLinePosition.Character);

            lineSpan = syntaxTree.GetLineSpan(node.Span);
            symbolInfo.selectionRange = new Range(lineSpan.StartLinePosition.Line, lineSpan.StartLinePosition.Character,
                lineSpan.StartLinePosition.Line, lineSpan.StartLinePosition.Character);

            //additional information
            ProcessNode(syntaxTree, symbolInfo, node);

            //process child nodes
            IEnumerable<SyntaxNode> list = node.ChildNodes();
            if (list != null)
            {
                foreach (SyntaxNode childNode in list)
                {
                    ProcessChildSyntaxNode(syntaxTree, symbolInfo, childNode);
                }
            }

            return symbolInfo;
        }

        #endregion

        #region Processing special syntax properties

        protected void ProcessNode(SyntaxTree syntaxTree, ALSymbolInformation symbol, SyntaxNode node)
        {
            switch (node.Kind.ConvertToLocalType())
            {
                case ConvertedSyntaxKind.XmlPortTableElement:
                    ProcessXmlPortTableElementNode(syntaxTree, symbol, (XmlPortTableElementSyntax)node);
                    break;
                case ConvertedSyntaxKind.ReportDataItem:
                    ProcessReportDataItemNode(syntaxTree, symbol, (ReportDataItemSyntax)node );
                    break;
                case ConvertedSyntaxKind.ReportColumn:
                    ProcessReportColumnNode(symbol, (ReportColumnSyntax)node);
                    break;
                case ConvertedSyntaxKind.Key:
                    ProcessKeyNode(symbol, (KeySyntax)node);
                    break;
                case ConvertedSyntaxKind.EventDeclaration:
                    ProcessEventDeclarationNode(symbol, (EventDeclarationSyntax)node);
                    break;
                case ConvertedSyntaxKind.TriggerDeclaration:
                case ConvertedSyntaxKind.EventTriggerDeclaration:
                case ConvertedSyntaxKind.MethodDeclaration:
                    ProcessMethodOrTriggerDeclarationNode(symbol, (MethodOrTriggerDeclarationSyntax)node);
                    break;
                case ConvertedSyntaxKind.Field:
                    ProcessFieldNode(symbol, (FieldSyntax)node);
                    break;
                case ConvertedSyntaxKind.VariableDeclaration:
                    ProcessVariableDeclarationNode(symbol, (VariableDeclarationSyntax)node);
                    break;
                case ConvertedSyntaxKind.Parameter:
                    ProcessParameterNode(symbol, (ParameterSyntax)node);
                    break;
                case ConvertedSyntaxKind.EnumValue:
                    ProcessEnumValueNode(symbol, (EnumValueSyntax)node);
                    break;
                case ConvertedSyntaxKind.PageGroup:
                    ProcessPageGroupNode(syntaxTree, symbol, (PageGroupSyntax)node);
                    break;
                case ConvertedSyntaxKind.PageArea:
                    ProcessPageAreaNode(syntaxTree, symbol, (PageAreaSyntax)node);
                    break;
                case ConvertedSyntaxKind.PagePart:
                    ProcessPagePartNode(symbol, (PagePartSyntax)node);
                    break;
                case ConvertedSyntaxKind.PageSystemPart:
                    ProcessPageSystemPartNode(symbol, (PageSystemPartSyntax)node);
                    break;
                case ConvertedSyntaxKind.PageChartPart:
                    ProcessPageChartPartNode(symbol, (PageChartPartSyntax)node);
                    break;
                case ConvertedSyntaxKind.PageField:
                    ProcessPageFieldNode(symbol, (PageFieldSyntax)node);
                    break;
                case ConvertedSyntaxKind.PageExtensionObject:
                    ProcessPageExtensionObjectNode(symbol, (PageExtensionSyntax)node);
                    break;
                case ConvertedSyntaxKind.ControlAddChange:
                    ProcessControlAddChangeNode(syntaxTree, symbol, (ControlAddChangeSyntax)node);
                    break;
                case ConvertedSyntaxKind.PageCustomizationObject:
                    ProcessPageCustomizationObjectNode(symbol, (PageCustomizationSyntax)node);
                    break;
                case ConvertedSyntaxKind.QueryDataItem:
                    ProcessQueryDataItemNode(syntaxTree, symbol, (QueryDataItemSyntax)node);
                    break;
                case ConvertedSyntaxKind.QueryColumn:
                    ProcessQueryColumnNode(symbol, (QueryColumnSyntax)node);
                    break;
                case ConvertedSyntaxKind.VarSection:
                case ConvertedSyntaxKind.GlobalVarSection:
                    ProcessVarSection(syntaxTree, symbol, (VarSectionBaseSyntax)node);
                    break;
            }
        }

        protected void ProcessVarSection(SyntaxTree syntaxTree, ALSymbolInformation symbol, VarSectionBaseSyntax syntax)
        {
            ProcessNodeContentRangeFromChildren(syntaxTree, symbol, syntax);
        }

        protected void ProcessControlAddChangeNode(SyntaxTree syntaxTree, ALSymbolInformation symbol, ControlAddChangeSyntax syntax)
        {
            this.ProcessNodeContentRange(syntaxTree, symbol, syntax, syntax.OpenBraceToken, syntax.CloseBraceToken);
        }

        protected void ProcessQueryColumnNode(ALSymbolInformation symbol, QueryColumnSyntax syntax)
        {
            if (syntax.RelatedField != null)
                symbol.source = ALSyntaxHelper.DecodeName(syntax.RelatedField.ToString());
        }

        protected void ProcessQueryDataItemNode(SyntaxTree syntaxTree, ALSymbolInformation symbol, QueryDataItemSyntax syntax)
        {
            if (syntax.DataItemTable != null)
                symbol.source = ALSyntaxHelper.DecodeName(syntax.DataItemTable.ToString());
            this.ProcessNodeContentRange(syntaxTree, symbol, syntax, syntax.OpenBraceToken, syntax.CloseBraceToken);
        }

        protected void ProcessPageCustomizationObjectNode(ALSymbolInformation symbol, PageCustomizationSyntax syntax)
        {
            if (syntax.BaseObject != null)
                symbol.extends = ALSyntaxHelper.DecodeName(syntax.BaseObject.ToString());
        }

        protected void ProcessPageExtensionObjectNode(ALSymbolInformation symbol, PageExtensionSyntax syntax)
        {
            if (syntax.BaseObject != null)
                symbol.extends = ALSyntaxHelper.DecodeName(syntax.BaseObject.ToString());
        }

        protected void ProcessPageFieldNode(ALSymbolInformation symbol, PageFieldSyntax syntax)
        {
            if (syntax.Expression != null)
                symbol.source = ALSyntaxHelper.DecodeName(syntax.Expression.ToString());
        }

        protected void ProcessPagePartNode(ALSymbolInformation symbol, PagePartSyntax syntax)
        {
            string name = symbol.kind.ToName() + " " + ALSyntaxHelper.EncodeName(symbol.name);
            if (syntax.PartName != null)
                symbol.fullName = name + ": " + syntax.PartName.ToFullString();
            symbol.fullName = name;
        }

        protected void ProcessPageSystemPartNode(ALSymbolInformation symbol, PageSystemPartSyntax syntax)
        {
            string name = symbol.kind.ToName() + " " + ALSyntaxHelper.EncodeName(symbol.name);
            if (syntax.SystemPartType != null)
                symbol.fullName = name + ": " + syntax.SystemPartType.ToFullString();
            symbol.fullName = name;
        }

        protected void ProcessPageChartPartNode(ALSymbolInformation symbol, PageChartPartSyntax syntax)
        {
            string name = symbol.kind.ToName() + " " + ALSyntaxHelper.EncodeName(symbol.name);
            if (syntax.ChartPartType != null)
                symbol.fullName = name + ": " + syntax.ChartPartType.ToFullString();
            symbol.fullName = name;
        }

        protected void ProcessPageAreaNode(SyntaxTree syntaxTree, ALSymbolInformation symbol, PageAreaSyntax syntax)
        {
            this.ProcessNodeContentRange(syntaxTree, symbol, syntax, syntax.OpenBraceToken, syntax.CloseBraceToken);
        }

        protected void ProcessPageGroupNode(SyntaxTree syntaxTree, ALSymbolInformation symbol, PageGroupSyntax syntax)
        {
            SyntaxToken controlKeywordToken = syntax.ControlKeyword;
            if ((controlKeywordToken != null) && (controlKeywordToken.Kind.ConvertToLocalType() == ConvertedSyntaxKind.PageRepeaterKeyword))
                symbol.kind = ALSymbolKind.PageRepeater;
            this.ProcessNodeContentRange(syntaxTree, symbol, syntax, syntax.OpenBraceToken, syntax.CloseBraceToken);
        }

        protected void ProcessEnumValueNode(ALSymbolInformation symbol, EnumValueSyntax syntax)
        {
            symbol.fullName = ALSyntaxHelper.EncodeName(symbol.name) + ": " + syntax.EnumValueToken.ToFullString();
        }

        protected void ProcessReportColumnNode(ALSymbolInformation symbol, ReportColumnSyntax syntax)
        {
            symbol.fullName = ALSyntaxHelper.EncodeName(symbol.name) + ": " + syntax.SourceExpression.ToFullString();
            if (syntax.SourceExpression != null)
                symbol.source = ALSyntaxHelper.DecodeName(syntax.SourceExpression.ToString());
        }

        protected void ProcessXmlPortTableElementNode(SyntaxTree syntaxTree, ALSymbolInformation symbol, XmlPortTableElementSyntax syntax)
        {
            symbol.fullName = symbol.kind.ToName() + " " + 
                ALSyntaxHelper.EncodeName(symbol.name) + 
                ": Record " + syntax.SourceTable.ToFullString();
            symbol.source = ALSyntaxHelper.DecodeName(syntax.SourceTable.ToFullString());
            this.ProcessNodeContentRange(syntaxTree, symbol, syntax, syntax.OpenBraceToken, syntax.CloseBraceToken);
        }

        protected void ProcessReportDataItemNode(SyntaxTree syntaxTree, ALSymbolInformation symbol, ReportDataItemSyntax syntax)
        {
            symbol.fullName = ALSyntaxHelper.EncodeName(symbol.name) + ": Record " + syntax.DataItemTable.ToFullString();
            if (syntax.DataItemTable != null)
                symbol.source = ALSyntaxHelper.DecodeName(syntax.DataItemTable.ToString());
            this.ProcessNodeContentRange(syntaxTree, symbol, syntax, syntax.OpenBraceToken, syntax.CloseBraceToken);
        }

        protected void ProcessFieldNode(ALSymbolInformation symbol, FieldSyntax syntax)
        {
            Type syntaxType = syntax.GetType();
            if (syntax.No != null)
            {
                string idText = syntax.No.ToString();
                int id = 0;
                if (Int32.TryParse(idText, out id))
                    symbol.id = id;
            }

            symbol.fullName = ALSyntaxHelper.EncodeName(symbol.name) + ": " + syntax.Type.ToFullString();
        }

        protected void ProcessParameterNode(ALSymbolInformation symbol, ParameterSyntax syntax)
        {
            symbol.fullName = ALSyntaxHelper.EncodeName(symbol.name) + ": " + syntax.Type.ToFullString();
        }

        protected void ProcessVariableDeclarationNode(ALSymbolInformation symbol, VariableDeclarationSyntax syntax)
        {
            symbol.fullName = ALSyntaxHelper.EncodeName(symbol.name) + ": " + syntax.Type.ToFullString();
        }

        protected void ProcessKeyNode(ALSymbolInformation symbol, KeySyntax syntax)
        {
            symbol.fullName = ALSyntaxHelper.EncodeName(symbol.name) + ": " + syntax.Fields.ToFullString();
        }

        protected void ProcessMethodOrTriggerDeclarationNode(ALSymbolInformation symbol, MethodOrTriggerDeclarationSyntax syntax)
        {
            string namePart = "(";
            if ((syntax.ParameterList != null))// && (syntax.ParameterList.Parameters != null))
                namePart = namePart + syntax.ParameterList.Parameters.ToFullString();
            namePart = namePart + ")";

            if ((syntax.ReturnValue != null) && (syntax.ReturnValue.Kind.ConvertToLocalType() != ConvertedSyntaxKind.None))
                namePart = namePart + " " + syntax.ReturnValue.ToFullString();
                
            symbol.fullName = ALSyntaxHelper.EncodeName(symbol.name) + namePart;
        }

        protected void ProcessEventDeclarationNode(ALSymbolInformation symbol, EventDeclarationSyntax syntax)
        {
            string namePart = "(";
            if ((syntax.ParameterList != null)) // && (syntax.ParameterList.Parameters != null))
                namePart = namePart + syntax.ParameterList.Parameters.ToFullString();
            namePart = namePart + ")";

            symbol.fullName = ALSyntaxHelper.EncodeName(symbol.name) + namePart;
        }

        protected void ProcessNodeContentRange(SyntaxTree syntaxTree, ALSymbolInformation symbol, SyntaxNode node,
            SyntaxToken contentStartToken, SyntaxToken contentEndToken)
        {
            if ((contentStartToken != null) && (contentEndToken != null))
            {
                var startSpan = syntaxTree.GetLineSpan(contentStartToken.Span);
                var endSpan = syntaxTree.GetLineSpan(contentEndToken.Span);
                symbol.contentRange = new Range(startSpan.EndLinePosition.Line, startSpan.EndLinePosition.Character,
                    endSpan.StartLinePosition.Line, endSpan.StartLinePosition.Character);
            }
        }

        protected void ProcessNodeContentRangeFromChildren(SyntaxTree syntaxTree, ALSymbolInformation symbol, SyntaxNode syntax)
        {
            IEnumerable<SyntaxNode> list = syntax.ChildNodes();
            if (list != null)
            {
                Range totalRange = null;
                foreach (SyntaxNode childNode in list)
                {
                    var lineSpan = syntaxTree.GetLineSpan(childNode.FullSpan);
                    Range nodeRange = new Range(lineSpan.StartLinePosition.Line, lineSpan.StartLinePosition.Character,
                        lineSpan.EndLinePosition.Line, lineSpan.EndLinePosition.Character);
                    if (totalRange == null)
                        totalRange = nodeRange;
                    else
                    {
                        if (totalRange.start.IsGreater(nodeRange.start))
                            totalRange.start.Set(nodeRange.start);
                        if (totalRange.end.IsLower(nodeRange.end))
                            totalRange.end.Set(nodeRange.end);
                    }
                }
                symbol.contentRange = totalRange;
            }
        }

        #endregion

        #region processing child nodes

        protected void ProcessChildSyntaxNode(SyntaxTree syntaxTree, ALSymbolInformation parent, SyntaxNode node)
        {
            //check if node is an attribute of parent symbol
            if (!ProcessSyntaxNodeAttribute(syntaxTree, parent, node))
            {
                ALSymbolInformation symbolInfo = CreateSymbolInfo(syntaxTree, node);
                if (IsValidChildSymbolInformation(symbolInfo))
                {
                    if (((parent.childSymbols == null) || (parent.childSymbols.Count == 0)) &&
                        (symbolInfo.kind == ALSymbolKind.Key))
                        symbolInfo.kind = ALSymbolKind.PrimaryKey;

                    parent.AddChildSymbol(symbolInfo);
                }
            }
        }

        protected void ProcessSyntaxNodePropertyList(SyntaxTree syntaxTree, ALSymbolInformation parent, SyntaxNode node)
        {
            IEnumerable<SyntaxNode> list = node.ChildNodes();
            if (list != null)
            {
                foreach (SyntaxNode childNode in list)
                {
                    if (childNode.Kind.ConvertToLocalType() == ConvertedSyntaxKind.Property)
                    {
                        Type nodeType = childNode.GetType();
                        string name = nodeType.TryGetPropertyValueAsString(childNode, "Name").NotNull();
                        string value = nodeType.TryGetPropertyValueAsString(childNode, "Value").NotNull();
                        this.ProcessSyntaxNodeProperty(syntaxTree, parent, name, value);
                    }
                }
            }
        }

        protected void ProcessVariableListDeclarationNode(SyntaxTree syntaxTree, ALSymbolInformation parent, VariableListDeclarationSyntax node)
        {
            string typeName = node.Type.ToFullString();
            string elementDataType = typeName;
            if (node.Type.DataType != null)
                elementDataType = node.Type.DataType.ToString();

            foreach (VariableDeclarationNameSyntax nameNode in node.VariableNames)
            {
                ALSymbolInformation variableSymbol = CreateSymbolInfo(syntaxTree, nameNode); //new ALSymbolInformation(ALSymbolKind.VariableDeclaration, variableName);
                variableSymbol.fullName = ALSyntaxHelper.EncodeName(variableSymbol.name) +
                    ": " + typeName;
                variableSymbol.subtype = typeName;
                variableSymbol.elementsubtype = elementDataType;

                parent.AddChildSymbol(variableSymbol);
            }
        }

        protected void ProcessSyntaxNodeProperty(SyntaxTree syntaxTree, ALSymbolInformation parent, string name, string value)
        {
            if ((name != null) && (value != null))
            {
                name = name.ToLower();
                switch (parent.kind)
                {
                    case ALSymbolKind.PageObject:
                        if (name == "sourcetable")
                            parent.source = ALSyntaxHelper.DecodeName(value);
                        break;
                    case ALSymbolKind.Field:
                        if ((name == "enabled") && (value != null) && (
                            (value.Equals("false", StringComparison.CurrentCultureIgnoreCase)) ||
                            (value == "0")))
                        {
                            if ((parent.subtype != null) && (parent.subtype.StartsWith("Obsolete")))
                            {
                                int obsoletePos = parent.fullName.LastIndexOf("(Obsolete");
                                if (obsoletePos > 0)
                                    parent.fullName = parent.fullName.Substring(0, obsoletePos - 1);
                            }
                            parent.subtype = "Disabled";
                            parent.fullName = parent.fullName + " (Disabled)";
                        }
                        else if ((name == "obsoletestate") && (value != null))
                        {
                            if (String.IsNullOrEmpty(parent.subtype))
                            {
                                if (value.Equals("Pending", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    parent.subtype = "ObsoletePending";
                                    parent.fullName = parent.fullName + " (Obsolete-Pending)";
                                }
                                else if (value.Equals("Removed", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    parent.subtype = "ObsoleteRemoved";
                                    parent.fullName = parent.fullName + " (Obsolete-Removed)";
                                }
                            }
                        }
                        break;
                }
            }
        }

        protected bool ProcessSyntaxNodeAttribute(SyntaxTree syntaxTree, ALSymbolInformation parent, SyntaxNode node)
        {
            switch (node.Kind.ConvertToLocalType())
            {
                case ConvertedSyntaxKind.PropertyList:
                    this.ProcessSyntaxNodePropertyList(syntaxTree, parent, node);
                    return !this.IncludeProperties;
                case ConvertedSyntaxKind.SimpleTypeReference:
                case ConvertedSyntaxKind.RecordTypeReference:
                case ConvertedSyntaxKind.DotNetTypeReference:
                    parent.subtype = node.ToFullString();
                    parent.elementsubtype = node.GetType().TryGetPropertyValueAsString(node, "DataType");
                    if (String.IsNullOrWhiteSpace(parent.elementsubtype))
                        parent.elementsubtype = parent.subtype;
                    return true;
                case ConvertedSyntaxKind.MemberAttribute:
                    string memberAttributeName = node.GetSyntaxNodeName().NotNull();
                    if ((parent.kind == ALSymbolKind.MethodDeclaration) || (parent.kind == ALSymbolKind.LocalMethodDeclaration))
                    {
                        ALSymbolKind newKind = ALSyntaxHelper.MemberAttributeToMethodKind(memberAttributeName);
                        if (newKind != ALSymbolKind.Undefined)
                        {
                            parent.kind = newKind;
                            return true;
                        }
                    }
                    parent.subtype = memberAttributeName;
                    return true;
                case ConvertedSyntaxKind.ObjectId:
                    ObjectIdSyntax objectIdSyntax = (ObjectIdSyntax)node;
                    if ((objectIdSyntax.Value != null) && (objectIdSyntax.Value.Value != null))
                        parent.id = (int)objectIdSyntax.Value.Value;
                    return true;
                case ConvertedSyntaxKind.IdentifierName:
                    var lineSpan = syntaxTree.GetLineSpan(node.Span);
                    parent.selectionRange = new Range(lineSpan.StartLinePosition.Line, lineSpan.StartLinePosition.Character,
                        lineSpan.EndLinePosition.Line, lineSpan.EndLinePosition.Character);
                    return true;
                case ConvertedSyntaxKind.VariableListDeclaration:
                    this.ProcessVariableListDeclarationNode(syntaxTree, parent, (VariableListDeclarationSyntax)node);
                    return true;

            }
            return false;
        }

        protected bool IsValidChildSymbolInformation(ALSymbolInformation symbolInformation)
        {
            if (symbolInformation == null)
                return false;

            switch (symbolInformation.kind)
            {
                case ALSymbolKind.ParameterList: return ((symbolInformation.childSymbols != null) && (symbolInformation.childSymbols.Count > 0));
                default: return true;
            }
        }

        #endregion

        #region Syntax node kind to al kind conversion

        protected ALSymbolKind SyntaxKindToALSymbolKind(SyntaxNode node)
        {
            switch (node.Kind.ConvertToLocalType())
            {
                //file root
                case ConvertedSyntaxKind.CompilationUnit: return ALSymbolKind.CompilationUnit;
                //object types
                case ConvertedSyntaxKind.CodeunitObject: return ALSymbolKind.CodeunitObject;
                case ConvertedSyntaxKind.TableObject: return ALSymbolKind.TableObject;
                case ConvertedSyntaxKind.TableExtensionObject: return ALSymbolKind.TableExtensionObject;
                case ConvertedSyntaxKind.PageObject: return ALSymbolKind.PageObject;
                case ConvertedSyntaxKind.PageExtensionObject: return ALSymbolKind.PageExtensionObject;
                case ConvertedSyntaxKind.ReportObject: return ALSymbolKind.ReportObject;
                case ConvertedSyntaxKind.XmlPortObject: return ALSymbolKind.XmlPortObject;
                case ConvertedSyntaxKind.QueryObject: return ALSymbolKind.QueryObject;
                case ConvertedSyntaxKind.ControlAddInObject: return ALSymbolKind.ControlAddInObject;
                case ConvertedSyntaxKind.ProfileObject: return ALSymbolKind.ProfileObject;
                case ConvertedSyntaxKind.PageCustomizationObject: return ALSymbolKind.PageCustomizationObject;
                case ConvertedSyntaxKind.DotNetPackage: return ALSymbolKind.DotNetPackage;
                case ConvertedSyntaxKind.Interface: return ALSymbolKind.Interface;

                //code elements
                case ConvertedSyntaxKind.MethodDeclaration:
                    MethodDeclarationSyntax methodSyntax = (MethodDeclarationSyntax)node;
                    if (methodSyntax.AccessModifier != null)
                    {
                        switch (methodSyntax.AccessModifier.Kind.ConvertToLocalType())
                        {
                            case ConvertedSyntaxKind.ProtectedKeyword:
                                return ALSymbolKind.LocalMethodDeclaration;
                            case ConvertedSyntaxKind.LocalKeyword:
                                return ALSymbolKind.LocalMethodDeclaration;
                            case ConvertedSyntaxKind.InternalKeyword:
                                return ALSymbolKind.LocalMethodDeclaration;
                            default:
                                return ALSymbolKind.MethodDeclaration;
                        }
                    }
                    else
                        return ALSymbolKind.MethodDeclaration;

                case ConvertedSyntaxKind.ParameterList: return ALSymbolKind.ParameterList;
                case ConvertedSyntaxKind.Parameter: return ALSymbolKind.Parameter;
                case ConvertedSyntaxKind.VarSection: return ALSymbolKind.VarSection;
                case ConvertedSyntaxKind.GlobalVarSection: return ALSymbolKind.GlobalVarSection;
                case ConvertedSyntaxKind.VariableDeclaration: return ALSymbolKind.VariableDeclaration;
                case ConvertedSyntaxKind.VariableDeclarationName: return ALSymbolKind.VariableDeclarationName;
                case ConvertedSyntaxKind.TriggerDeclaration: return ALSymbolKind.TriggerDeclaration;

                //table and table extensions
                case ConvertedSyntaxKind.FieldList: return ALSymbolKind.FieldList;
                case ConvertedSyntaxKind.Field: return ALSymbolKind.Field;
                case ConvertedSyntaxKind.DotNetAssembly: return ALSymbolKind.DotNetAssembly;
                case ConvertedSyntaxKind.DotNetTypeDeclaration: return ALSymbolKind.DotNetTypeDeclaration;
                case ConvertedSyntaxKind.FieldExtensionList: return ALSymbolKind.FieldExtensionList;
                case ConvertedSyntaxKind.FieldModification: return ALSymbolKind.FieldModification;
                case ConvertedSyntaxKind.KeyList: return ALSymbolKind.KeyList;
                case ConvertedSyntaxKind.Key: return ALSymbolKind.Key;
                case ConvertedSyntaxKind.FieldGroupList: return ALSymbolKind.FieldGroupList;
                case ConvertedSyntaxKind.FieldGroup: return ALSymbolKind.FieldGroup;

                //page, page extenstions and page customizations
                case ConvertedSyntaxKind.PageLayout: return ALSymbolKind.PageLayout;
                case ConvertedSyntaxKind.PageActionList: return ALSymbolKind.PageActionList;
                case ConvertedSyntaxKind.GroupActionList: return ALSymbolKind.GroupActionList;
                case ConvertedSyntaxKind.PageArea: return ALSymbolKind.PageArea;
                case ConvertedSyntaxKind.PageGroup: return ALSymbolKind.PageGroup;
                case ConvertedSyntaxKind.PageField: return ALSymbolKind.PageField;
                case ConvertedSyntaxKind.PageLabel: return ALSymbolKind.PageLabel;
                case ConvertedSyntaxKind.PagePart: return ALSymbolKind.PagePart;
                case ConvertedSyntaxKind.PageSystemPart: return ALSymbolKind.PageSystemPart;
                case ConvertedSyntaxKind.PageChartPart: return ALSymbolKind.PageChartPart;
                case ConvertedSyntaxKind.PageUserControl: return ALSymbolKind.PageUserControl;
                case ConvertedSyntaxKind.PageAction: return ALSymbolKind.PageAction;
                case ConvertedSyntaxKind.PageActionGroup: return ALSymbolKind.PageActionGroup;
                case ConvertedSyntaxKind.PageActionArea: return ALSymbolKind.PageActionArea;
                case ConvertedSyntaxKind.PageActionSeparator: return ALSymbolKind.PageActionSeparator;
                case ConvertedSyntaxKind.PageExtensionActionList: return ALSymbolKind.PageExtensionActionList;
                case ConvertedSyntaxKind.ActionAddChange: return ALSymbolKind.ActionAddChange;
                case ConvertedSyntaxKind.ActionMoveChange: return ALSymbolKind.ActionMoveChange;
                case ConvertedSyntaxKind.ActionModifyChange: return ALSymbolKind.ActionModifyChange;
                case ConvertedSyntaxKind.PageExtensionLayout: return ALSymbolKind.PageExtensionLayout;
                case ConvertedSyntaxKind.ControlAddChange: return ALSymbolKind.ControlAddChange;
                case ConvertedSyntaxKind.ControlMoveChange: return ALSymbolKind.ControlMoveChange;
                case ConvertedSyntaxKind.ControlModifyChange: return ALSymbolKind.ControlModifyChange;
                case ConvertedSyntaxKind.PageExtensionViewList: return ALSymbolKind.PageExtensionViewList;
                case ConvertedSyntaxKind.ViewAddChange: return ALSymbolKind.ViewAddChange;
                case ConvertedSyntaxKind.ViewMoveChange: return ALSymbolKind.ViewMoveChange;
                case ConvertedSyntaxKind.ViewModifyChange: return ALSymbolKind.ViewModifyChange;
                case ConvertedSyntaxKind.PageViewList: return ALSymbolKind.PageViewList;
                case ConvertedSyntaxKind.PageView: return ALSymbolKind.PageView;

                //xmlports
                case ConvertedSyntaxKind.XmlPortSchema: return ALSymbolKind.XmlPortSchema;
                case ConvertedSyntaxKind.XmlPortTableElement: return ALSymbolKind.XmlPortTableElement;
                case ConvertedSyntaxKind.XmlPortFieldElement: return ALSymbolKind.XmlPortFieldElement;
                case ConvertedSyntaxKind.XmlPortTextElement: return ALSymbolKind.XmlPortTextElement;
                case ConvertedSyntaxKind.XmlPortFieldAttribute: return ALSymbolKind.XmlPortFieldAttribute;
                case ConvertedSyntaxKind.XmlPortTextAttribute: return ALSymbolKind.XmlPortTextAttribute;
                case ConvertedSyntaxKind.RequestPage: return ALSymbolKind.RequestPage;

                //reports
                case ConvertedSyntaxKind.ReportDataSetSection: return ALSymbolKind.ReportDataSetSection;
                case ConvertedSyntaxKind.ReportLabelsSection: return ALSymbolKind.ReportLabelsSection;
                case ConvertedSyntaxKind.ReportDataItem: return ALSymbolKind.ReportDataItem;
                case ConvertedSyntaxKind.ReportColumn: return ALSymbolKind.ReportColumn;
                case ConvertedSyntaxKind.ReportLabel: return ALSymbolKind.ReportLabel;
                case ConvertedSyntaxKind.ReportLabelMultilanguage: return ALSymbolKind.ReportLabelMultilanguage;

                //dotnet packages



                //control add-ins
                case ConvertedSyntaxKind.EventTriggerDeclaration: return ALSymbolKind.EventTriggerDeclaration;
                case ConvertedSyntaxKind.EventDeclaration: return ALSymbolKind.EventDeclaration;

                //queries
                case ConvertedSyntaxKind.QueryElements: return ALSymbolKind.QueryElements;
                case ConvertedSyntaxKind.QueryDataItem: return ALSymbolKind.QueryDataItem;
                case ConvertedSyntaxKind.QueryColumn: return ALSymbolKind.QueryColumn;
                case ConvertedSyntaxKind.QueryFilter: return ALSymbolKind.QueryFilter;


                //codeunits


                //enums and enum extensions
                case ConvertedSyntaxKind.EnumType: return ALSymbolKind.EnumType;
                case ConvertedSyntaxKind.EnumValue: return ALSymbolKind.EnumValue;
                case ConvertedSyntaxKind.EnumExtensionType: return ALSymbolKind.EnumExtensionType;

                //properties
                case ConvertedSyntaxKind.PropertyList: return ALSymbolKind.PropertyList;
                case ConvertedSyntaxKind.Property: return ALSymbolKind.Property;

            }
            return ALSymbolKind.Undefined;
        }

        #endregion

    }
}
