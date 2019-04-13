using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using AnZwDev.ALTools.ALProxy;

namespace AnZwDev.ALTools.ALSymbols.SymbolReaders
{
    /*
    public class ALSymbolInfoPackageReader
    {

        protected ALExtensionProxy ALExtensionProxy { get; }

        public ALSymbolInfoPackageReader(ALExtensionProxy alExtensionProxy)
        {
            this.ALExtensionProxy = alExtensionProxy;
        }

        public ALSymbolInformation ReadSymbols(string packagePath)
        {
            try
            {
                Stream packageStream = new FileStream(packagePath, FileMode.Open);
                ALSymbolInformation rootSymbol = this.ReadSymbols(packageStream);
                packageStream.Close();
                packageStream.Dispose();
                return rootSymbol;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public ALSymbolInformation ReadSymbols(Stream packageStream)
        {
            ALExtensionLibraryTypeProxy NAVAppPackageReader = new ALExtensionLibraryTypeProxy(
                this.ALExtensionProxy.CodeAnalysis,
                "Microsoft.Dynamics.Nav.CodeAnalysis.Packaging.NavAppPackageReader");

            ALExtensionLibraryTypeProxy SymbolReferenceJsonReader = new ALExtensionLibraryTypeProxy(
                this.ALExtensionProxy.CodeAnalysis,
                "Microsoft.Dynamics.Nav.CodeAnalysis.SymbolReference.SymbolReferenceJsonReader");

            //read module definition
            dynamic packageReader = NAVAppPackageReader.CallStaticMethod("Create", packageStream, true);
            Stream symbolReferenceStream = packageReader.ReadSymbolReferenceFile();
            dynamic moduleDefinition = SymbolReferenceJsonReader.CallStaticMethod("ReadModule", symbolReferenceStream, Type.Missing);
            symbolReferenceStream.Close();
            symbolReferenceStream.Dispose();
            //convert module definition to symbols information
            return ModuleDefinitionToSymbolInformation(moduleDefinition);

        }

        protected ALSymbolInformation ModuleDefinitionToSymbolInformation(dynamic moduleDefinition)
        {
            if (moduleDefinition == null)
                return null;

            ALSymbolInformation rootSymbol = new ALSymbolInformation(
                ALSymbolKind.Module,
                moduleDefinition.Publisher + " " + moduleDefinition.Name + " " + moduleDefinition.Version);

            rootSymbol.AddChildSymbol(this.ProcessTableList(moduleDefinition.Tables));
            rootSymbol.AddChildSymbol(this.ProcessPageList(moduleDefinition.Pages));
            rootSymbol.AddChildSymbol(this.ProcessReportList(moduleDefinition.Reports));
            rootSymbol.AddChildSymbol(this.ProcessXmlPortList(moduleDefinition.XmlPorts));
            rootSymbol.AddChildSymbol(this.ProcessQueryList(moduleDefinition.Queries));
            rootSymbol.AddChildSymbol(this.ProcessCodeunitList(moduleDefinition.Codeunits));
            rootSymbol.AddChildSymbol(this.ProcessControlAddInList(moduleDefinition.ControlAddIns));
            rootSymbol.AddChildSymbol(this.ProcessPageExtensionList(moduleDefinition.PageExtensions));
            rootSymbol.AddChildSymbol(this.ProcessTableExtensionList(moduleDefinition.TableExtensions));
            rootSymbol.AddChildSymbol(this.ProcessProfileList(moduleDefinition.Profiles));
            rootSymbol.AddChildSymbol(this.ProcessPageCustomizationList(moduleDefinition.PageCustomizations));
            rootSymbol.AddChildSymbol(this.ProcessDotNetPackageList(moduleDefinition.DotNetPackages));
            rootSymbol.AddChildSymbol(this.ProcessEnumList(moduleDefinition.EnumTypes));
            rootSymbol.AddChildSymbol(this.ProcessEnumExtensionList(moduleDefinition.EnumExtensionTypes));

            return rootSymbol;
        }

        #region Enum extensions processing

        protected ALSymbolInformation ProcessEnumExtensionList(dynamic[] types)
        {
            if ((types == null) || (types.Length == 0))
                return null;
            ALSymbolInformation container = new ALSymbolInformation(ALSymbolKind.EnumExtensionTypeList, "Enum Extensions");

            for (int i = 0; i < types.Length; i++)
            {
                container.AddChildSymbol(ProcessEnumTypeDefinition(types[i]));
            }

            return container;
        }

        protected ALSymbolInformation ProcessEnumExtensionTypeDefinition(dynamic type)
        {
            ALSymbolInformation container = new ALSymbolInformation(ALSymbolKind.EnumExtensionType, type.Name, type.Id);
            container.fullName = ALSyntaxHelper.EncodeName(type.Name) + ":" + type.TargetObject;
            ProcessEnumValues(container, type.Values);
            return container;
        }

        #endregion

        #region Enum processing

        protected ALSymbolInformation ProcessEnumList(dynamic[] types)
        {
            if ((types == null) || (types.Length == 0))
                return null;
            ALSymbolInformation container = new ALSymbolInformation(ALSymbolKind.EnumTypeList, "Enums");

            for (int i=0; i<types.Length; i++)
            {
                container.AddChildSymbol(ProcessEnumTypeDefinition(types[i]));
            }

            return container;
        }

        protected ALSymbolInformation ProcessEnumTypeDefinition(dynamic type)
        {
            ALSymbolInformation container = new ALSymbolInformation(ALSymbolKind.EnumType, type.Name, type.Id);
            ProcessEnumValues(container, type.Values);
            return container;
        }

        protected void ProcessEnumValues(ALSymbolInformation container, dynamic[] values)
        {
            if (values != null)
            {
                for (int i=0; i<values.Length;i++)
                {
                    container.AddChildSymbol(ProcessEnumValueDefinition(values[i]));
                }
            }
        }

        protected ALSymbolInformation ProcessEnumValueDefinition(dynamic value)
        {
            ALSymbolInformation container = new ALSymbolInformation(ALSymbolKind.EnumValue, value.Name, value.Id);
            return container;
        }

        #endregion

        #region DotNetPackage processing

        public ALSymbolInformation ProcessDotNetPackageList(dynamic[] dotNetPackages)
        {
            if ((dotNetPackages == null) || (dotNetPackages.Length == 0))
                return null;

            ALSymbolInformation container = new ALSymbolInformation(ALSymbolKind.DotNetPackageList, "DotNet Packages");
            for (int i=0; i<dotNetPackages.Length; i++)
            {
                container.AddChildSymbol(ProcessDotNetPackageDefinition(dotNetPackages[i]));
            }
            return container;
        }

        protected ALSymbolInformation ProcessDotNetPackageDefinition(dynamic dotNetPackage)
        {
            ALSymbolInformation container = new ALSymbolInformation(ALSymbolKind.DotNetPackage, ".NET Package");
            ProcessDotNetAssemblies(container, dotNetPackage.AssemblyDeclarations);                
            return container;
        }

        protected void ProcessDotNetAssemblies(ALSymbolInformation container, dynamic[] definitions)
        {
            if (definitions != null)
            {
                for (int i=0; i<definitions.Length; i++)
                {
                    container.AddChildSymbol(ProcessDotNetAssemblyDefinition(definitions[i]));
                }
            }
        }

        protected ALSymbolInformation ProcessDotNetAssemblyDefinition(dynamic definition)
        {
            ALSymbolInformation container = new ALSymbolInformation(ALSymbolKind.DotNetAssembly, definition.Name);
            ProcessDotNetTypeDeclarations(container, definition.TypeDeclarations);
            return container;
        }

        protected void ProcessDotNetTypeDeclarations(ALSymbolInformation container, dynamic[] types)
        {
            if (types != null)
            {
                for (int i=0; i<types.Length; i++)
                {
                    container.AddChildSymbol(ProcessDotNetTypeDeclarationDef(types[i]));
                }
            }
        }

        protected ALSymbolInformation ProcessDotNetTypeDeclarationDef(dynamic type)
        {
            ALSymbolInformation container = new ALSymbolInformation(ALSymbolKind.DotNetTypeDeclaration, type.AliasName);
            container.fullName = ALSyntaxHelper.EncodeName(type.AliasName) + ":" + type.TypeName;
            return container;
        }

        #endregion

        #region Profile processing

        protected ALSymbolInformation ProcessProfileList(dynamic[] profiles)
        {
            if ((profiles == null) || (profiles.Length == 0))
                return null;
            ALSymbolInformation container = new ALSymbolInformation(ALSymbolKind.ProfileObjectList, "Profiles");
            for (int i = 0; i < profiles.Length; i++)
            {
                container.AddChildSymbol(ProcessProfileDefinition(profiles[i]));
            }
            return container;
        }

        protected ALSymbolInformation ProcessProfileDefinition(dynamic profile)
        {
            ALSymbolInformation container = new ALSymbolInformation(ALSymbolKind.ProfileObject, profile.Name, profile.Id);
            return container;
        }


        #endregion

        #region ControlAddIns processing

        public ALSymbolInformation ProcessControlAddInList(dynamic[] controlAddIns)
        {
            if ((controlAddIns == null) || (controlAddIns.Length == 0))
                return null;

            ALSymbolInformation container = new ALSymbolInformation(ALSymbolKind.ControlAddInObjectList, "ControlAddIns");

            for (int i = 0; i < controlAddIns.Length; i++)
            {
                container.AddChildSymbol(ProcessControlAddInDefinition(controlAddIns[i]));
            }

            return container;
        }

        public ALSymbolInformation ProcessControlAddInDefinition(dynamic controlAddIn)
        {
            ALSymbolInformation container = new ALSymbolInformation(ALSymbolKind.ControlAddInObject, controlAddIn.Name, controlAddIn.Id);

            ProcessEvents(container, controlAddIn.Events);
            ProcessMethods(container, controlAddIn.Methods);
            return container;
        }

        protected void ProcessEvents(ALSymbolInformation container, dynamic[] events)
        {
            if (events != null)
            {
                for (int i = 0; i < events.Length; i++)
                {
                    container.AddChildSymbol(ProcessEventDefinition(events[i]));
                }
            }
        }

        protected ALSymbolInformation ProcessEventDefinition(dynamic method)
        {
            ALSymbolInformation container = new ALSymbolInformation(ALSymbolKind.EventDeclaration, method.Name);

            string fullName = method.Name + "(";
            if (method.Parameters != null)
            {
                for (int i = 0; i < method.Parameters.Length; i++)
                {
                    if (i > 0)
                        fullName = fullName + ",";
                    fullName = fullName + method.Parameters[i].Name + ":" + method.Parameters[i].Type;
                }
            }
            fullName = fullName + ")";
            return container;
        }

        #endregion

        #region Codeunits processing

        public ALSymbolInformation ProcessCodeunitList(dynamic[] codeunits)
        {
            if ((codeunits == null) || (codeunits.Length == 0))
                return null;

            ALSymbolInformation container = new ALSymbolInformation(ALSymbolKind.CodeunitObjectList, "Codeunits");

            for (int i = 0; i < codeunits.Length; i++)
            {
                container.AddChildSymbol(ProcessCodeunitDefinition(codeunits[i]));
            }

            return container;
        }

        public ALSymbolInformation ProcessCodeunitDefinition(dynamic codeunit)
        {
            ALSymbolInformation container = new ALSymbolInformation(ALSymbolKind.CodeunitObject, codeunit.Name, codeunit.Id);

            container.AddChildSymbol(ProcessVariables(codeunit.Variables));
            ProcessMethods(container, codeunit.Methods);
            return container;
        }

        #endregion

        #region Queries processing

        public ALSymbolInformation ProcessQueryList(dynamic[] queries)
        {
            if ((queries == null) || (queries.Length == 0))
                return null;

            ALSymbolInformation container = new ALSymbolInformation(ALSymbolKind.QueryObjectList, "Queries");

            for (int i = 0; i < queries.Length; i++)
            {
                container.AddChildSymbol(ProcessQueryDefinition(queries[i]));
            }

            return container;
        }

        public ALSymbolInformation ProcessQueryDefinition(dynamic query)
        {
            ALSymbolInformation container = new ALSymbolInformation(ALSymbolKind.QueryObject, query.Name, query.Id);

            ProcessQueryDataItems(container, query.Elements);
            container.AddChildSymbol(ProcessVariables(query.Variables));
            ProcessMethods(container, query.Methods);
            return container;
        }


        #endregion

        #region Query elements

        protected void ProcessQueryDataItems(ALSymbolInformation container, dynamic[] dataItems)
        {
            if (dataItems != null)
            {
                for (int i=0; i<dataItems.Length;i++)
                {
                    container.AddChildSymbol(ProcessQueryDataItemDefinition(dataItems[i]));
                }
            }
        }

        protected ALSymbolInformation ProcessQueryDataItemDefinition(dynamic dataItem)
        {
            ALSymbolInformation container = new ALSymbolInformation(ALSymbolKind.QueryDataItem, dataItem.Name);

            ProcessColumns(container, dataItem.Columns);
            ProcessQueryDataItems(container, dataItem.DataItems);

            return container;
        }

        protected void ProcessColumns(ALSymbolInformation container, dynamic[] columns)
        {
            if (columns != null)
            {
                for (int i=0; i<columns.Length; i++)
                {
                    container.AddChildSymbol(ProcessColumnDefinition(columns[i]));
                }
            }
        }

        protected ALSymbolInformation ProcessColumnDefinition(dynamic column)
        {
            ALSymbolInformation container = new ALSymbolInformation(ALSymbolKind.QueryColumn, column.Name);
            if (!String.IsNullOrWhiteSpace(column.SourceColumn))
                container.fullName = ALSyntaxHelper.EncodeName(column.Name) + ":" + column.SourceColumn;
            return container;
        }

        #endregion

        #region XmlPorts processing

        public ALSymbolInformation ProcessXmlPortList(dynamic[] xmlPorts)
        {
            if ((xmlPorts == null) || (xmlPorts.Length == 0))
                return null;

            ALSymbolInformation container = new ALSymbolInformation(ALSymbolKind.XmlPortObjectList, "XmlPorts");

            for (int i=0; i<xmlPorts.Length; i++)
            {
                container.AddChildSymbol(ProcessXmlPortDefinition(xmlPorts[i]));
            }

            return container;
        }

        public ALSymbolInformation ProcessXmlPortDefinition(dynamic xmlPort)
        {
            ALSymbolInformation container = new ALSymbolInformation(ALSymbolKind.XmlPortObject, xmlPort.Name, xmlPort.Id);
            container.AddChildSymbol(ProcessVariables(xmlPort.Variables));
            ProcessMethods(container, xmlPort.Methods);
            return container;
        }

        #endregion

        #region Report processing

        public ALSymbolInformation ProcessReportList(dynamic[] reports)
        {
            if ((reports == null) || (reports.Length == 0))
                return null;
            ALSymbolInformation container = new ALSymbolInformation(ALSymbolKind.ReportObjectList, "Reports");

            for (int i=0; i<reports.Length; i++)
            {
                container.AddChildSymbol(ProcessReport(reports[i]));
            }

            return container;
        }

        public ALSymbolInformation ProcessReport(dynamic report)
        {
            ALSymbolInformation container = new ALSymbolInformation(ALSymbolKind.ReportObject, report.Name, report.Id);

            ProcessReportDataItems(container, report.DataItems);
            container.AddChildSymbol(ProcessRequestPageDefinition(report.RequestPage));
            container.AddChildSymbol(ProcessVariables(report.Variables));
            ProcessMethods(container, report.Methods);

            return container;
        }

        #endregion

        #region Report elements

        public void ProcessReportDataItems(ALSymbolInformation container, dynamic[] dataItems)
        {
            if (dataItems != null)
            {
                for (int i=0; i<dataItems.Length; i++)
                {
                    container.AddChildSymbol(ProcessReportDataItemDefinition(dataItems[i]));
                }
            }
        }

        public ALSymbolInformation ProcessReportDataItemDefinition(dynamic dataItem)
        {
            ALSymbolInformation container = new ALSymbolInformation(ALSymbolKind.ReportDataItem, dataItem.Name);
            return container;
        }

        public ALSymbolInformation ProcessRequestPageDefinition(dynamic requestPage)
        {
            if (requestPage == null)
                return null;
            ALSymbolInformation container = new ALSymbolInformation(ALSymbolKind.RequestPage, "Request Page");

            ProcessControls(container, requestPage.Controls);
            ProcessActions(container, requestPage.Actions);

            return container;
        }

        #endregion

        #region Page processing

        protected ALSymbolInformation ProcessPageList(dynamic[] pages)
        {
            if ((pages == null) || (pages.Length == 0))
                return null;
            ALSymbolInformation container = new ALSymbolInformation(ALSymbolKind.PageObjectList, "Pages");
            for (int i = 0; i < pages.Length; i++)
            {
                container.AddChildSymbol(ProcessPageDefinition(pages[i]));
            }
            return container;
        }

        protected ALSymbolInformation ProcessPageDefinition(dynamic page)
        {
            ALSymbolInformation container = new ALSymbolInformation(ALSymbolKind.PageObject, page.Name, page.Id);

            ProcessControls(container, page.Controls);
            ProcessActions(container, page.Actions);
            container.AddChildSymbol(ProcessViews(page.Views));
            container.AddChildSymbol(ProcessVariables(page.Variables));
            ProcessMethods(container, page.Methods);

            return container;
        }

        #endregion

        #region Page elements

        public ALSymbolInformation ProcessViews(dynamic[] views)
        {
            if ((views == null) || (views.Length == 0))
                return null;

            ALSymbolInformation container = new ALSymbolInformation(ALSymbolKind.PageViewList, "views");

            for (int i=0; i<views.Length; i++)
            {
                container.AddChildSymbol(ProcessViewDefinition(views[i]));
            }

            return container;
        }

        public ALSymbolInformation ProcessViewDefinition(dynamic view)
        {
            ALSymbolInformation container = new ALSymbolInformation(ALSymbolKind.PageView, view.Name);
            ProcessControlChanges(container, view.ControlChanges);
            return container;
        }

        public void ProcessControls(ALSymbolInformation container, dynamic[] controls)
        {
            if (controls != null)
            {
                for (int i=0; i<controls.Length;i++)
                {
                    container.AddChildSymbol(ProcessControlDefinition(controls[i]));
                }
            }
        }

        public ALSymbolInformation ProcessControlDefinition(dynamic control)
        {
            ALSymbolInformation container = new ALSymbolInformation(ControlKindToSybolKind(control.Kind), control.Name);
            if (container.kind == ALSymbolKind.PageField)
            {
                container.fullName = ALSyntaxHelper.EncodeName(control.Name) + " : " + control.Type;
            }

            ProcessControls(container, control.Controls);
            ProcessActions(container, control.Actions);

            return container;
        }

        protected ALSymbolKind ControlKindToSybolKind(dynamic value)
        {
            ConvertedControlKind controlKind = ALEnumConverters.ControlKindConverter.Convert(value);
            switch (controlKind)
            {
                case ConvertedControlKind.Area: return ALSymbolKind.PageArea;
                case ConvertedControlKind.ChartPart: return ALSymbolKind.PageChartPart;
                case ConvertedControlKind.CueGroup: return ALSymbolKind.PageGroup;
                case ConvertedControlKind.Field: return ALSymbolKind.PageField;
                case ConvertedControlKind.Fixed: return ALSymbolKind.PageGroup;
                case ConvertedControlKind.Label: return ALSymbolKind.PageLabel;
                case ConvertedControlKind.Part: return ALSymbolKind.PagePart;
                case ConvertedControlKind.Repeater: return ALSymbolKind.PageGroup;
                case ConvertedControlKind.SystemPart:return ALSymbolKind.PageSystemPart;
                case ConvertedControlKind.UserControl: return ALSymbolKind.PageUserControl;
            }
            return ALSymbolKind.Undefined;
        }

        public void ProcessActions(ALSymbolInformation container, dynamic[] actions)
        {
            if (actions != null)
            {
                for (int i = 0; i < actions.Length; i++)
                {
                    container.AddChildSymbol(ProcessActionDefinition(actions[i]));
                }
            }
        }

        public ALSymbolInformation ProcessActionDefinition(dynamic action)
        {
            ALSymbolInformation container = new ALSymbolInformation(ActionKindToSybolKind(action.Kind), action.Name);

            ProcessActions(container, action.Actions);

            return container;
        }

        protected ALSymbolKind ActionKindToSybolKind(dynamic value)
        {
            ConvertedActionKind kind = ALEnumConverters.ActionKindConverter.Convert(value);
            switch (kind)
            {
                case ConvertedActionKind.Action: return ALSymbolKind.PageAction;
                case ConvertedActionKind.Area: return ALSymbolKind.PageActionArea;
                case ConvertedActionKind.Group: return ALSymbolKind.PageActionGroup;
                case ConvertedActionKind.Separator: return ALSymbolKind.PageActionSeparator;
            }
            return ALSymbolKind.Undefined;
        }

        #endregion

        #region Page customization processing

        protected ALSymbolInformation ProcessPageCustomizationList(dynamic[] pages)
        {
            if ((pages == null) || (pages.Length == 0))
                return null;
            ALSymbolInformation container = new ALSymbolInformation(ALSymbolKind.PageCustomizationObjectList, "Page Customizations");
            for (int i = 0; i < pages.Length; i++)
            {
                container.AddChildSymbol(ProcessPageCustomizationDefinition(pages[i]));
            }
            return container;
        }

        protected ALSymbolInformation ProcessPageCustomizationDefinition(dynamic page)
        {
            ALSymbolInformation container = new ALSymbolInformation(ALSymbolKind.PageCustomizationObject, page.Name, page.Id);

            ProcessControlChanges(container, page.ControlChanges);
            ProcessActionChanges(container, page.ActionChanges);
            ProcessViewChanges(container, page.ViewChanges);

            return container;
        }


        #endregion

        #region Page extension processing

        protected ALSymbolInformation ProcessPageExtensionList(dynamic[] pages)
        {
            if ((pages == null) || (pages.Length == 0))
                return null;
            ALSymbolInformation container = new ALSymbolInformation(ALSymbolKind.PageExtensionObjectList, "Page Extensions");
            for (int i = 0; i < pages.Length; i++)
            {
                container.AddChildSymbol(ProcessPageExtensionDefinition(pages[i]));
            }
            return container;
        }

        protected ALSymbolInformation ProcessPageExtensionDefinition(dynamic page)
        {
            ALSymbolInformation container = new ALSymbolInformation(ALSymbolKind.PageExtensionObject, page.Name, page.Id);

            ProcessControlChanges(container, page.ControlChanges);
            ProcessActionChanges(container, page.ActionChanges);
            ProcessViewChanges(container, page.ViewChanges);
            container.AddChildSymbol(ProcessVariables(page.Variables));
            ProcessMethods(container, page.Methods);

            return container;
        }

        #endregion

        #region Page extension elements

        protected void ProcessControlChanges(ALSymbolInformation container, dynamic[] controlChanges)
        {
            if (controlChanges != null)
            {
                for (int i=0; i<controlChanges.Length; i++)
                {
                    container.AddChildSymbol(ProcessControlChangeDefinition(controlChanges[i]));
                }
            }
        }

        protected ALSymbolInformation ProcessControlChangeDefinition(dynamic controlChange)
        {
            ALSymbolInformation container = new ALSymbolInformation(ControlChangeKindToALKind(controlChange.ChangeKind),
                controlChange.ChangeKind.ToString());
            ProcessControls(container, controlChange.Controls);
            return container;
        }

        protected ALSymbolKind ControlChangeKindToALKind(dynamic value)
        {
            ConvertedChangeKind kind = ALEnumConverters.ChangeKindConverter.Convert(value);
            switch (kind)
            {
                case ConvertedChangeKind.AddAfter:
                case ConvertedChangeKind.AddBefore:
                case ConvertedChangeKind.AddFirst:
                case ConvertedChangeKind.AddLast:
                case ConvertedChangeKind.Add: return ALSymbolKind.ControlAddChange;
                case ConvertedChangeKind.Modify: return ALSymbolKind.ControlModifyChange;
                case ConvertedChangeKind.MoveAfter:
                case ConvertedChangeKind.MoveBefore:
                case ConvertedChangeKind.MoveFirst:
                case ConvertedChangeKind.MoveLast: return ALSymbolKind.ControlMoveChange;
            }
            return ALSymbolKind.Undefined;
        }

        protected void ProcessActionChanges(ALSymbolInformation container, dynamic[] actionChanges)
        {
            if (actionChanges != null)
            {
                for (int i = 0; i < actionChanges.Length; i++)
                {
                    container.AddChildSymbol(ProcessActionChangeDefinition(actionChanges[i]));
                }
            }
        }

        protected ALSymbolInformation ProcessActionChangeDefinition(dynamic actionChange)
        {
            ALSymbolInformation container = new ALSymbolInformation(ActionChangeKindToALKind(actionChange.ChangeKind),
                actionChange.ChangeKind.ToString());
            ProcessActions(container, actionChange.Actions);
            return container;
        }

        protected ALSymbolKind ActionChangeKindToALKind(dynamic value)
        {
            ConvertedChangeKind kind = ALEnumConverters.ChangeKindConverter.Convert(value);
            switch (kind)
            {
                case ConvertedChangeKind.AddAfter:
                case ConvertedChangeKind.AddBefore:
                case ConvertedChangeKind.AddFirst:
                case ConvertedChangeKind.AddLast:
                case ConvertedChangeKind.Add: return ALSymbolKind.ActionAddChange;
                case ConvertedChangeKind.Modify: return ALSymbolKind.ActionModifyChange;
                case ConvertedChangeKind.MoveAfter:
                case ConvertedChangeKind.MoveBefore:
                case ConvertedChangeKind.MoveFirst:
                case ConvertedChangeKind.MoveLast: return ALSymbolKind.ActionMoveChange;
            }
            return ALSymbolKind.Undefined;
        }

        protected void ProcessViewChanges(ALSymbolInformation container, dynamic[] viewChanges)
        {
            if (viewChanges != null)
            {
                for (int i = 0; i < viewChanges.Length; i++)
                {
                    container.AddChildSymbol(ProcessViewChangeDefinition(viewChanges[i]));
                }
            }
        }

        protected ALSymbolInformation ProcessViewChangeDefinition(dynamic controlChange)
        {
            ALSymbolInformation container = new ALSymbolInformation(ViewChangeKindToALKind(controlChange.ChangeKind),
                controlChange.ChangeKind.ToString());
            container.AddChildSymbol(ProcessViews(controlChange.Views));
            return container;
        }

        protected ALSymbolKind ViewChangeKindToALKind(dynamic value)
        {
            ConvertedChangeKind kind = ALEnumConverters.ChangeKindConverter.Convert(value);
            switch (kind)
            {
                case ConvertedChangeKind.AddAfter:
                case ConvertedChangeKind.AddBefore:
                case ConvertedChangeKind.AddFirst:
                case ConvertedChangeKind.AddLast:
                case ConvertedChangeKind.Add: return ALSymbolKind.ViewAddChange;
                case ConvertedChangeKind.Modify: return ALSymbolKind.ViewModifyChange;
                case ConvertedChangeKind.MoveAfter:
                case ConvertedChangeKind.MoveBefore:
                case ConvertedChangeKind.MoveFirst:
                case ConvertedChangeKind.MoveLast: return ALSymbolKind.ViewMoveChange;
            }
            return ALSymbolKind.Undefined;
        }

        #endregion

        #region Table Extension processing

        //process subnodes
        protected ALSymbolInformation ProcessTableExtensionList(dynamic[] tables)
        {
            if ((tables == null) || (tables.Length == 0))
                return null;
            ALSymbolInformation container = new ALSymbolInformation(ALSymbolKind.TableExtensionObjectList, "Table Extensions");
            for (int i = 0; i < tables.Length; i++)
            {
                container.AddChildSymbol(ProcessTableDefinition(ALSymbolKind.TableExtensionObject, tables[i]));
            }
            return container;
        }

        #endregion

        #region Table processing

        //process subnodes
        protected ALSymbolInformation ProcessTableList(dynamic[] tables)
        {
            if ((tables == null) || (tables.Length == 0))
                return null;
            ALSymbolInformation container = new ALSymbolInformation(ALSymbolKind.TableObjectList, "Tables");
            for (int i=0; i<tables.Length; i++)
            {
                container.AddChildSymbol(ProcessTableDefinition(ALSymbolKind.TableObject, tables[i]));
            }
            return container;
        }

        protected ALSymbolInformation ProcessTableDefinition(ALSymbolKind kind, dynamic table)
        {
            ALSymbolInformation container = new ALSymbolInformation(kind, table.Name, table.Id);

            container.AddChildSymbol(ProcessFields(table.Fields));
            container.AddChildSymbol(ProcessFieldGroups(table.FieldGroups));
            container.AddChildSymbol(ProcessKeys(table.Keys));
            container.AddChildSymbol(ProcessVariables(table.Variables));
            ProcessMethods(container, table.Methods);

            return container;
        }

        #endregion

        #region Table elements

        protected ALSymbolInformation ProcessKeys(dynamic[] keys)
        {
            if ((keys == null) || (keys.Length == 0))
                return null;

            ALSymbolInformation container = new ALSymbolInformation(ALSymbolKind.KeyList, "Keys");
            for (int i = 0; i < keys.Length; i++)
            {
                container.AddChildSymbol(ProcessKeyDefinition(keys[i], (i==0)));
            }
            return container;
        }

        protected ALSymbolInformation ProcessKeyDefinition(dynamic key, bool isPrimayKey)
        {
            ALSymbolKind kind = isPrimayKey ? ALSymbolKind.PrimaryKey : ALSymbolKind.Key;
            ALSymbolInformation container = new ALSymbolInformation(kind, key.Name);
            container.fullName = ALSyntaxHelper.EncodeName(key.Name) + " : " + key.FieldNames;
            return container;
        }

        protected ALSymbolInformation ProcessFields(dynamic[] fields)
        {
            if ((fields == null) || (fields.Length == 0))
                return null;

            ALSymbolInformation container = new ALSymbolInformation(ALSymbolKind.FieldList, "Fields");
            for (int i=0; i<fields.Length; i++)
            {
                container.AddChildSymbol(ProcessFieldDefinition(fields[i]));
            }
            return container;
        }

        protected ALSymbolInformation ProcessFieldDefinition(dynamic field)
        {
            return new ALSymbolInformation(ALSymbolKind.Field, field.Name, field.Id);
        }

        protected ALSymbolInformation ProcessFieldGroups(dynamic[] fieldGroups)
        {
            if ((fieldGroups == null) || (fieldGroups.Length == 0))
                return null;

            ALSymbolInformation container = new ALSymbolInformation(ALSymbolKind.FieldGroupList, "Field Groups");
            for (int i = 0; i < fieldGroups.Length; i++)
            {
                container.AddChildSymbol(ProcessFieldGroupDefinition(fieldGroups[i]));
            }
            return container;
        }

        protected ALSymbolInformation ProcessFieldGroupDefinition(dynamic fieldGroup)
        {
            ALSymbolInformation container = new ALSymbolInformation(ALSymbolKind.FieldGroup, fieldGroup.Name);
            container.fullName = ALSyntaxHelper.EncodeName(fieldGroup.Name) + " : " + fieldGroup.FieldNames;
            return container;
        }

        #endregion

        #region Code elements

        protected ALSymbolInformation ProcessVariables(dynamic[] variables)
        {
            if ((variables == null) || (variables.Length == 0))
                return null;

            ALSymbolInformation container = new ALSymbolInformation(ALSymbolKind.VarSection, "vars");
            for (int i = 0; i < variables.Length; i++)
            {
                container.AddChildSymbol(ProcessVariableDefinition(variables[i]));
            }
            return container;
        }

        protected ALSymbolInformation ProcessVariableDefinition(dynamic variable)
        {
            ALSymbolInformation container = new ALSymbolInformation(ALSymbolKind.VariableDeclaration, variable.Name);
            container.fullName = ALSyntaxHelper.EncodeName(variable.Name) + " : " + variable.Type;
            return container;
        }

        protected void ProcessMethods(ALSymbolInformation container, dynamic[] methods)
        {
            if (methods != null)
            {
                for (int i = 0; i < methods.Length; i++)
                {
                    container.AddChildSymbol(ProcessMethodDefinition(methods[i]));
                }
            }
        }

        protected ALSymbolInformation ProcessMethodDefinition(dynamic method)
        {
            bool isLocal = false;// ((method.IsLocal.HasValue) && (method.IsLocal.Value));
            ALSymbolKind kind = isLocal ? ALSymbolKind.MethodDeclaration : ALSymbolKind.LocalMethodDeclaration;

            ALSymbolInformation container = new ALSymbolInformation(kind, method.Name);

            string fullName = method.Name + "(";
            if (method.Parameters != null)
            {
                for (int i=0; i<method.Parameters.Length; i++)
                {
                    if (i > 0)
                        fullName = fullName + ",";
                    fullName = fullName + method.Parameters[i].Name + ":" + method.Parameters[i].Type;
                }
            }
            fullName = fullName + ")";
            if (!String.IsNullOrWhiteSpace(method.ReturnType))
                fullName = fullName + ":" + method.ReturnType;

            return container;
        }


        #endregion

    }
    */
}
