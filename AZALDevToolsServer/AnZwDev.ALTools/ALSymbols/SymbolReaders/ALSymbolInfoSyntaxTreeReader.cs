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
            }
            catch (Exception e)
            {
                sourceCode = "";
            }
            return ProcessSourceCode(sourceCode);
        }

        public ALSymbolInformation ProcessSourceCode(string source)
        {
            ALExtensionLibraryTypeProxy syntaxTree = new ALExtensionLibraryTypeProxy(
                this.ALExtensionProxy.CodeAnalysis,
                "Microsoft.Dynamics.Nav.CodeAnalysis.Syntax.SyntaxTree");

            //ParseObjectText(string text, string path = null, Encoding encoding = null, ParseOptions options = null, CancellationToken cancellationToken = default(CancellationToken));

            dynamic sourceTree = syntaxTree.CallStaticMethod("ParseObjectText", source, 
                Type.Missing, Type.Missing, Type.Missing, Type.Missing);

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
            ProcessNode(symbolInfo, node);

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

        protected void ProcessNode(ALSymbolInformation symbol, dynamic node)
        {
            ConvertedSyntaxKind kind = ALEnumConverters.SyntaxKindConverter.Convert(node.Kind);

            switch (kind)
            {
                case ConvertedSyntaxKind.XmlPortTableElement:
                    ProcessXmlPortTableElementNode(symbol, node);
                    break;
                case ConvertedSyntaxKind.ReportDataItem:
                    ProcessReportDataItemNode(symbol, node);
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
                    ProcessPageGroupNode(symbol, node);
                    break;
            }
        }

        protected void ProcessPageGroupNode(ALSymbolInformation symbol, dynamic syntax)
        {
            dynamic controlKeywordToken = syntax.ControlKeyword;
            if (controlKeywordToken != null)
            {
                ConvertedSyntaxKind controlKind = ALEnumConverters.SyntaxKindConverter.Convert(controlKeywordToken.Kind);
                if (controlKind == ConvertedSyntaxKind.PageRepeaterKeyword)
                    symbol.kind = ALSymbolKind.PageRepeater;
            }
        }


        protected void ProcessEnumValueNode(ALSymbolInformation symbol, dynamic syntax)
        {
            symbol.fullName = ALSyntaxHelper.EncodeName(symbol.name) + ": " + syntax.EnumValueToken.ToFullString();
        }

        protected void ProcessReportColumnNode(ALSymbolInformation symbol, dynamic syntax)
        {
            symbol.fullName = ALSyntaxHelper.EncodeName(symbol.name) + ": " + syntax.SourceExpression.ToFullString();
        }

        protected void ProcessXmlPortTableElementNode(ALSymbolInformation symbol, dynamic syntax)
        {
            symbol.fullName = symbol.kind.ToName() + " " + 
                ALSyntaxHelper.EncodeName(symbol.name) + 
                ": Record " + syntax.SourceTable.ToFullString();
        }

        protected void ProcessReportDataItemNode(ALSymbolInformation symbol, dynamic syntax)
        {
            symbol.fullName = ALSyntaxHelper.EncodeName(symbol.name) + ": Record " + syntax.DataItemTable.ToFullString();
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

        protected bool ProcessSyntaxNodeAttribute(dynamic syntaxTree, ALSymbolInformation parent, dynamic node)
        {
            ConvertedSyntaxKind kind = ALEnumConverters.SyntaxKindConverter.Convert(node.Kind);
            switch (kind)
            {
                case ConvertedSyntaxKind.SimpleTypeReference:
                case ConvertedSyntaxKind.RecordTypeReference:
                case ConvertedSyntaxKind.DotNetTypeReference:
                    parent.subtype = node.ToFullString();
                    return true;
                case ConvertedSyntaxKind.MemberAttribute:
                    string memberAttributeName = node.Name.ToString();
                    if ((parent.kind == ALSymbolKind.MethodDeclaration) || (parent.kind == ALSymbolKind.LocalMethodDeclaration))
                    {
                        ALSymbolKind newKind = this.MemberAttributeToMethodKind(memberAttributeName);
                        if (newKind != ALSymbolKind.Undefined)
                        {
                            parent.kind = newKind;
                            return true;
                        }
                    }
                    parent.subtype = memberAttributeName;
                    return true;
                case ConvertedSyntaxKind.ObjectId:
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

        protected ALSymbolKind MemberAttributeToMethodKind(string name)
        {
            //events
            if (name.Equals("IntegrationEvent"))
                return ALSymbolKind.IntegrationEventDeclaration;
            if (name.Equals("BusinessEvent"))
                return ALSymbolKind.BusinessEventDeclaration;
            if (name.Equals("EventSubscriber"))
                return ALSymbolKind.EventSubscriberDeclaration;
            //tests
            if (name.Equals("Test"))
                return ALSymbolKind.TestDeclaration;
            if (name.Equals("ConfirmHandler"))
                return ALSymbolKind.ConfirmHandlerDeclaration;
            if (name.Equals("FilterPageHandler"))
                return ALSymbolKind.FilterPageHandlerDeclaration;
            if (name.Equals("HyperlinkHandler"))
                return ALSymbolKind.HyperlinkHandlerDeclaration;
            if (name.Equals("MessageHandler"))
                return ALSymbolKind.MessageHandlerDeclaration;
            if (name.Equals("ModalPageHandler"))
                return ALSymbolKind.ModalPageHandlerDeclaration;
            if (name.Equals("PageHandler"))
                return ALSymbolKind.PageHandlerDeclaration;
            if (name.Equals("ReportHandler"))
                return ALSymbolKind.ReportHandlerDeclaration;
            if (name.Equals("RequestPageHandler"))
                return ALSymbolKind.RequestPageHandlerDeclaration;
            if (name.Equals("SendNotificationHandler"))
                return ALSymbolKind.SendNotificationHandlerDeclaration;
            if (name.Equals("SessionSettingsHandler"))
                return ALSymbolKind.SessionSettingsHandlerDeclaration;
            if (name.Equals("StrMenuHandler"))
                return ALSymbolKind.StrMenuHandlerDeclaration;

            return ALSymbolKind.Undefined;
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
