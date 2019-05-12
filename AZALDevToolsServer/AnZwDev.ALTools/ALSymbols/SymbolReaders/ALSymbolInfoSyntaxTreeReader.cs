using AnZwDev.ALTools.ALProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols.SymbolReaders
{
    public class ALSymbolInfoSyntaxTreeReader
    {

        protected ALExtensionProxy ALExtensionProxy { get; }

        public ALSymbolInfoSyntaxTreeReader(ALExtensionProxy alExtensionProxy)
        {
            this.ALExtensionProxy = alExtensionProxy;
        }

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
            dynamic sourceTree = this.ALExtensionProxy.GetSyntaxTree(source);
            return ProcessSyntaxTree(sourceTree);
        }

        public ALSymbolInformation ProcessSyntaxTree(dynamic syntaxTree)
        {
            //this.SyntaxTree = syntaxTree;

            dynamic node = syntaxTree.GetRoot();
            ALSymbolInformation root = new ALSymbolInformation();
            ProcessChildSyntaxNode(syntaxTree, root, node);

            root.UpdateFields();

            if ((root.childSymbols != null) && (root.childSymbols.Count == 1))
                return root.childSymbols[0];

            return root;
        }

        protected ALSymbolInformation CreateSymbolInfo(dynamic syntaxTree, dynamic node)
        {
            //Detect symbol type
            ALSymbolKind alSymbolKind = SyntaxKindToALSymbolKind(node);
            if (alSymbolKind == ALSymbolKind.Undefined)
                return null;

            //create symbol info
            ALSymbolInformation symbolInfo = new ALSymbolInformation();
            if ((node.GetType().GetProperty("Name") != null) && (node.Name != null))
                symbolInfo.name = ALSyntaxHelper.DecodeName(node.Name.ToString());
            symbolInfo.kind = alSymbolKind;

            dynamic lineSpan = syntaxTree.GetLineSpan(node.FullSpan);
            symbolInfo.range = new Range(lineSpan.StartLinePosition.Line, lineSpan.StartLinePosition.Character,
                lineSpan.EndLinePosition.Line, lineSpan.EndLinePosition.Character);

            lineSpan = syntaxTree.GetLineSpan(node.Span);
            symbolInfo.selectionRange = new Range(lineSpan.StartLinePosition.Line, lineSpan.StartLinePosition.Character,
                lineSpan.StartLinePosition.Line, lineSpan.StartLinePosition.Character);

            //additional information
            ProcessNode(syntaxTree, symbolInfo, node);

            //process child nodes
            IEnumerable<dynamic> list = node.ChildNodes();
            if (list != null)
            {
                foreach (dynamic childNode in list)
                {
                    ProcessChildSyntaxNode(syntaxTree, symbolInfo, childNode);
                }
            }

            return symbolInfo;
        }

        #region Processing special syntax properties

        protected void ProcessNode(dynamic syntaxTree, ALSymbolInformation symbol, dynamic node)
        {
            ConvertedSyntaxKind kind = ALEnumConverters.SyntaxKindConverter.Convert(node.Kind);

            switch (kind)
            {
                case ConvertedSyntaxKind.XmlPortTableElement:
                    ProcessXmlPortTableElementNode(symbol, node);
                    break;
                case ConvertedSyntaxKind.ReportDataItem:
                    ProcessReportDataItemNode(syntaxTree, symbol, node);
                    break;
                case ConvertedSyntaxKind.ReportColumn:
                    ProcessReportColumnNode(symbol, node);
                    break;
                case ConvertedSyntaxKind.Key:
                    ProcessKeyNode(symbol, node);
                    break;
                case ConvertedSyntaxKind.EventDeclaration:
                    ProcessEventDeclarationNode(symbol, node);
                    break;
                case ConvertedSyntaxKind.TriggerDeclaration:
                case ConvertedSyntaxKind.EventTriggerDeclaration:
                case ConvertedSyntaxKind.MethodDeclaration:
                    ProcessMethodOrTriggerDeclarationNode(symbol, node);
                    break;
                case ConvertedSyntaxKind.Field:
                    ProcessFieldNode(symbol, node);
                    break;
                case ConvertedSyntaxKind.VariableDeclaration:
                    ProcessVariableDeclarationNode(symbol, node);
                    break;
                case ConvertedSyntaxKind.Parameter:
                    ProcessParameterNode(symbol, node);
                    break;
                case ConvertedSyntaxKind.EnumValue:
                    ProcessEnumValueNode(symbol, node);
                    break;
                case ConvertedSyntaxKind.PageGroup:
                    ProcessPageGroupNode(syntaxTree, symbol, node);
                    break;
                case ConvertedSyntaxKind.PagePart:
                    ProcessPagePartNode(symbol, node);
                    break;
                case ConvertedSyntaxKind.PageSystemPart:
                    ProcessPageSystemPartNode(symbol, node);
                    break;
                case ConvertedSyntaxKind.PageChartPart:
                    ProcessPageChartPartNode(symbol, node);
                    break;
                case ConvertedSyntaxKind.PageField:
                    ProcessPageFieldNode(symbol, node);
                    break;
                case ConvertedSyntaxKind.PageExtensionObject:
                    ProcessPageExtensionObjectNode(symbol, node);
                    break;
                case ConvertedSyntaxKind.ControlAddChange:
                    ProcessControlAddChangeNode(syntaxTree, symbol, node);
                    break;
                case ConvertedSyntaxKind.PageCustomizationObject:
                    ProcessPageCustomizationObjectNode(symbol, node);
                    break;
                case ConvertedSyntaxKind.QueryDataItem:
                    ProcessQueryDataItemNode(syntaxTree, symbol, node);
                    break;
                case ConvertedSyntaxKind.QueryColumn:
                    ProcessQueryColumnNode(symbol, node);
                    break;
            }
        }

        protected void ProcessControlAddChangeNode(dynamic syntaxTree, ALSymbolInformation symbol, dynamic syntax)
        {
            this.ProcessNodeContentRange(syntaxTree, symbol, syntax);
        }

        protected void ProcessQueryColumnNode(ALSymbolInformation symbol, dynamic syntax)
        {
            if (syntax.RelatedField != null)
                symbol.source = ALSyntaxHelper.DecodeName(syntax.RelatedField.ToString());
        }

        protected void ProcessQueryDataItemNode(dynamic syntaxTree, ALSymbolInformation symbol, dynamic syntax)
        {
            if (syntax.DataItemTable != null)
                symbol.source = ALSyntaxHelper.DecodeName(syntax.DataItemTable.ToString());
            this.ProcessNodeContentRange(syntaxTree, symbol, syntax);
        }

        protected void ProcessPageCustomizationObjectNode(ALSymbolInformation symbol, dynamic syntax)
        {
            if (syntax.BaseObject != null)
                symbol.extends = ALSyntaxHelper.DecodeName(syntax.BaseObject.ToString());
        }

        protected void ProcessPageExtensionObjectNode(ALSymbolInformation symbol, dynamic syntax)
        {
            if (syntax.BaseObject != null)
                symbol.extends = ALSyntaxHelper.DecodeName(syntax.BaseObject.ToString());
        }

        protected void ProcessPageFieldNode(ALSymbolInformation symbol, dynamic syntax)
        {
            if (syntax.Expression != null)
                symbol.source = ALSyntaxHelper.DecodeName(syntax.Expression.ToString());
        }

        protected void ProcessPagePartNode(ALSymbolInformation symbol, dynamic syntax)
        {
            string name = symbol.kind.ToName() + " " + ALSyntaxHelper.EncodeName(symbol.name);
            if (syntax.PartName != null)
                symbol.fullName = name + ": " + syntax.PartName.ToFullString();
            symbol.fullName = name;
        }

        protected void ProcessPageSystemPartNode(ALSymbolInformation symbol, dynamic syntax)
        {
            string name = symbol.kind.ToName() + " " + ALSyntaxHelper.EncodeName(symbol.name);
            if (syntax.SystemPartType != null)
                symbol.fullName = name + ": " + syntax.SystemPartType.ToFullString();
            symbol.fullName = name;
        }

        protected void ProcessPageChartPartNode(ALSymbolInformation symbol, dynamic syntax)
        {
            string name = symbol.kind.ToName() + " " + ALSyntaxHelper.EncodeName(symbol.name);
            if (syntax.ChartPartType != null)
                symbol.fullName = name + ": " + syntax.ChartPartType.ToFullString();
            symbol.fullName = name;
        }

        protected void ProcessPageGroupNode(dynamic syntaxTree, ALSymbolInformation symbol, dynamic syntax)
        {
            dynamic controlKeywordToken = syntax.ControlKeyword;
            if (controlKeywordToken != null)
            {
                ConvertedSyntaxKind controlKind = ALEnumConverters.SyntaxKindConverter.Convert(controlKeywordToken.Kind);
                if (controlKind == ConvertedSyntaxKind.PageRepeaterKeyword)
                    symbol.kind = ALSymbolKind.PageRepeater;
            }
            this.ProcessNodeContentRange(syntaxTree, symbol, syntax);
        }


        protected void ProcessEnumValueNode(ALSymbolInformation symbol, dynamic syntax)
        {
            symbol.fullName = ALSyntaxHelper.EncodeName(symbol.name) + ": " + syntax.EnumValueToken.ToFullString();
        }

        protected void ProcessReportColumnNode(ALSymbolInformation symbol, dynamic syntax)
        {
            symbol.fullName = ALSyntaxHelper.EncodeName(symbol.name) + ": " + syntax.SourceExpression.ToFullString();
            if (syntax.SourceExpression != null)
                symbol.source = ALSyntaxHelper.DecodeName(syntax.SourceExpression.ToString());
        }

        protected void ProcessXmlPortTableElementNode(ALSymbolInformation symbol, dynamic syntax)
        {
            symbol.fullName = symbol.kind.ToName() + " " + 
                ALSyntaxHelper.EncodeName(symbol.name) + 
                ": Record " + syntax.SourceTable.ToFullString();
        }

        protected void ProcessReportDataItemNode(dynamic syntaxTree, ALSymbolInformation symbol, dynamic syntax)
        {
            symbol.fullName = ALSyntaxHelper.EncodeName(symbol.name) + ": Record " + syntax.DataItemTable.ToFullString();
            if (syntax.DataItemTable != null)
                symbol.source = ALSyntaxHelper.DecodeName(syntax.DataItemTable.ToString());
            this.ProcessNodeContentRange(syntaxTree, symbol, syntax);
        }

        protected void ProcessFieldNode(ALSymbolInformation symbol, dynamic syntax)
        {
            symbol.fullName = ALSyntaxHelper.EncodeName(symbol.name) + ": " + syntax.Type.ToFullString();
        }

        protected void ProcessParameterNode(ALSymbolInformation symbol, dynamic syntax)
        {
            symbol.fullName = ALSyntaxHelper.EncodeName(symbol.name) + ": " + syntax.Type.ToFullString();
        }

        protected void ProcessVariableDeclarationNode(ALSymbolInformation symbol, dynamic syntax)
        {
            symbol.fullName = ALSyntaxHelper.EncodeName(symbol.name) + ": " + syntax.Type.ToFullString();
        }

        protected void ProcessKeyNode(ALSymbolInformation symbol, dynamic syntax)
        {
            symbol.fullName = ALSyntaxHelper.EncodeName(symbol.name) + ": " + syntax.Fields.ToFullString();
        }

        protected void ProcessMethodOrTriggerDeclarationNode(ALSymbolInformation symbol, dynamic syntax)
        {
            string namePart = "(";
            if ((syntax.ParameterList != null))// && (syntax.ParameterList.Parameters != null))
                namePart = namePart + syntax.ParameterList.Parameters.ToFullString();
            namePart = namePart + ")";

            if ((syntax.ReturnValue != null) && 
                (ALEnumConverters.SyntaxKindConverter.Convert(syntax.ReturnValue.Kind) != ConvertedSyntaxKind.None))
                namePart = namePart + " " + syntax.ReturnValue.ToFullString();
                
            symbol.fullName = ALSyntaxHelper.EncodeName(symbol.name) + namePart;
        }

        protected void ProcessEventDeclarationNode(ALSymbolInformation symbol, dynamic syntax)
        {
            string namePart = "(";
            if ((syntax.ParameterList != null)) // && (syntax.ParameterList.Parameters != null))
                namePart = namePart + syntax.ParameterList.Parameters.ToFullString();
            namePart = namePart + ")";

            symbol.fullName = ALSyntaxHelper.EncodeName(symbol.name) + namePart;
        }

        protected void ProcessNodeContentRange(dynamic syntaxTree, ALSymbolInformation symbol, dynamic syntax)
        {
            dynamic contentStartToken = syntax.OpenBraceToken;
            dynamic contentEndToken = syntax.CloseBraceToken;
            if ((contentStartToken != null) && (contentEndToken != null))
            {
                dynamic startSpan = syntaxTree.GetLineSpan(contentStartToken.Span);
                dynamic endSpan = syntaxTree.GetLineSpan(contentEndToken.Span);
                symbol.contentRange = new Range(startSpan.EndLinePosition.Line, startSpan.EndLinePosition.Character,
                    endSpan.StartLinePosition.Line, endSpan.StartLinePosition.Character);
            }
        }

        #endregion

        #region processing child nodes

        protected void ProcessChildSyntaxNode(dynamic syntaxTree, ALSymbolInformation parent, dynamic node)
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

        protected void ProcessSyntaxNodePropertyList(dynamic syntaxTree, ALSymbolInformation parent, dynamic node)
        {
            IEnumerable<dynamic> list = node.ChildNodes();
            if (list != null)
            {
                foreach (dynamic childNode in list)
                {
                    ConvertedSyntaxKind kind = ALEnumConverters.SyntaxKindConverter.Convert(childNode.Kind);
                    if (kind == ConvertedSyntaxKind.Property)
                    {
                        string name = childNode.Name.ToString();
                        string value = childNode.Value.ToString();
                        this.ProcessSyntaxNodeProperty(syntaxTree, parent, name, value);
                    }
                }
            }
        }

        protected void ProcessSyntaxNodeProperty(dynamic syntaxTree, ALSymbolInformation parent, string name, string value)
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
                }
            }
        }

        protected bool ProcessSyntaxNodeAttribute(dynamic syntaxTree, ALSymbolInformation parent, dynamic node)
        {
            ConvertedSyntaxKind kind = ALEnumConverters.SyntaxKindConverter.Convert(node.Kind);
            switch (kind)
            {
                case ConvertedSyntaxKind.PropertyList:
                    this.ProcessSyntaxNodePropertyList(syntaxTree, parent, node);
                    return true;
                case ConvertedSyntaxKind.SimpleTypeReference:
                case ConvertedSyntaxKind.RecordTypeReference:
                case ConvertedSyntaxKind.DotNetTypeReference:
                    parent.subtype = node.ToFullString();
                    return true;
                case ConvertedSyntaxKind.MemberAttribute:
                    string memberAttributeName = node.Name.ToString();
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
                    if ((node.Value != null) && (node.Value.Value != null))
                        parent.id = node.Value.Value;
                    return true;
                case ConvertedSyntaxKind.IdentifierName:
                    dynamic lineSpan = syntaxTree.GetLineSpan(node.Span);
                    parent.selectionRange = new Range(lineSpan.StartLinePosition.Line, lineSpan.StartLinePosition.Character,
                        lineSpan.EndLinePosition.Line, lineSpan.EndLinePosition.Character);
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

        protected ALSymbolKind SyntaxKindToALSymbolKind(dynamic node)
        {
            ConvertedSyntaxKind kind = ALEnumConverters.SyntaxKindConverter.Convert(node.Kind);

            switch (kind)
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

                //code elements
                case ConvertedSyntaxKind.MethodDeclaration:
                    if ((node.LocalKeyword != null) &&
                        (ALEnumConverters.SyntaxKindConverter.Convert(node.LocalKeyword.Kind) != ConvertedSyntaxKind.None))
                        return ALSymbolKind.LocalMethodDeclaration;
                    else
                        return ALSymbolKind.MethodDeclaration;

                case ConvertedSyntaxKind.ParameterList: return ALSymbolKind.ParameterList;
                case ConvertedSyntaxKind.Parameter: return ALSymbolKind.Parameter;
                case ConvertedSyntaxKind.VarSection: return ALSymbolKind.VarSection;
                case ConvertedSyntaxKind.VariableDeclaration: return ALSymbolKind.VariableDeclaration;
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
            }
            return ALSymbolKind.Undefined;
        }

        #endregion

    }
}
