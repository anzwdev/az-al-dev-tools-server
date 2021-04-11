using AnZwDev.ALTools.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.ALTools.ALSymbols;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppSymbolReference : ALAppBaseElement
    {

        public string AppId { get; set; }
        public string Name { get; set; }
        public string Publisher { get; set; }
        public string Version { get; set; }

        public ALAppElementsCollection<ALAppTable> Tables { get; set; }
        public ALAppElementsCollection<ALAppPage> Pages { get; set; }
        public ALAppElementsCollection<ALAppReport> Reports { get; set; }
        public ALAppElementsCollection<ALAppXmlPort> XmlPorts { get; set; }
        public ALAppElementsCollection<ALAppQuery> Queries { get; set; }
        public ALAppElementsCollection<ALAppCodeunit> Codeunits { get; set; }
        public ALAppElementsCollection<ALAppControlAddIn> ControlAddIns { get; set; }
        public ALAppElementsCollection<ALAppPageExtension> PageExtensions { get; set; }
        public ALAppElementsCollection<ALAppTableExtension> TableExtensions { get; set; }
        public ALAppElementsCollection<ALAppProfile> Pofiles { get; set; }
        public ALAppElementsCollection<ALAppPageCustomization> PageCustomizations { get; set; }
        public ALAppElementsCollection<ALAppDotNetPackage> DotNetPackages { get; set; }
        public ALAppElementsCollection<ALAppEnum> EnumTypes { get; set; }
        public ALAppElementsCollection<ALAppEnumExtension> EnumExtensionTypes { get; set; }
        public ALAppElementsCollection<ALAppInterface> Interfaces { get; set; }

        private ALSymbol _alSymbolCache = null;

        public ALAppSymbolReference()
        {
        }

        #region Objects processing

        public void AddObjects(List<ALAppObject> alObjectsList)
        {
            for (int i=0; i< alObjectsList.Count; i++)
            {
                this.AddObject(alObjectsList[i]);
            }
        }

        public void AddObject(ALAppObject alObject)
        {
            IALAppElementsCollection alObjectsCollection = this.GetObjectsCollection(alObject.GetALSymbolKind());
            if (alObjectsCollection != null)
            {
                alObjectsCollection.AddBaseElement(alObject);
                this.ClearALSymbolCache();
            }
        }

        public void RemoveObjects(List<ALAppObject> alObjectsList)
        {
            for (int i = 0; i < alObjectsList.Count; i++)
            {
                this.RemoveObject(alObjectsList[i]);
            }
        }

        public void RemoveObject(ALAppObject alObject)
        {
            IALAppElementsCollection alObjectsCollection = this.GetObjectsCollection(alObject.GetALSymbolKind());
            if (alObjectsCollection != null)
            {
                alObjectsCollection.RemoveBaseElement(alObject);
                this.ClearALSymbolCache();
            }
        }

        protected IALAppElementsCollection GetObjectsCollection(ALSymbolKind symbolKind)
        {
            switch (symbolKind)
            {
                case ALSymbolKind.TableObject:
                    if (this.Tables == null)
                        this.Tables = new ALAppElementsCollection<ALAppTable>();
                    return this.Tables;
                case ALSymbolKind.PageObject:
                    if (this.Pages == null)
                        this.Pages = new ALAppElementsCollection<ALAppPage>();
                    return this.Pages;
                case ALSymbolKind.ReportObject:
                    if (this.Reports == null)
                        this.Reports = new ALAppElementsCollection<ALAppReport>();
                    return this.Reports;
                case ALSymbolKind.XmlPortObject:
                    if (this.XmlPorts == null)
                        this.XmlPorts = new ALAppElementsCollection<ALAppXmlPort>();
                    return this.XmlPorts;
                case ALSymbolKind.QueryObject:
                    if (this.Queries == null)
                        this.Queries = new ALAppElementsCollection<ALAppQuery>();
                    return this.Queries;
                case ALSymbolKind.CodeunitObject:
                    if (this.Codeunits == null)
                        this.Codeunits = new ALAppElementsCollection<ALAppCodeunit>();
                    return this.Codeunits;
                case ALSymbolKind.ControlAddInObject:
                    if (this.ControlAddIns == null)
                        this.ControlAddIns = new ALAppElementsCollection<ALAppControlAddIn>();
                    return this.ControlAddIns;
                case ALSymbolKind.PageExtensionObject:
                    if (this.PageExtensions == null)
                        this.PageExtensions = new ALAppElementsCollection<ALAppPageExtension>();
                    return this.PageExtensions;
                case ALSymbolKind.TableExtensionObject:
                    if (this.TableExtensions == null)
                        this.TableExtensions = new ALAppElementsCollection<ALAppTableExtension>();
                    return this.TableExtensions;
                case ALSymbolKind.ProfileObject:
                    if (this.Pofiles == null)
                        this.Pofiles = new ALAppElementsCollection<ALAppProfile>();
                    return this.Pofiles;
                case ALSymbolKind.PageCustomizationObject:
                    if (this.PageCustomizations == null)
                        this.PageCustomizations = new ALAppElementsCollection<ALAppPageCustomization>();
                    return this.PageCustomizations;
                case ALSymbolKind.DotNetPackage:
                    if (this.DotNetPackages == null)
                        this.DotNetPackages = new ALAppElementsCollection<ALAppDotNetPackage>();
                    return this.DotNetPackages;
                case ALSymbolKind.EnumType:
                    if (this.EnumTypes == null)
                        this.EnumTypes = new ALAppElementsCollection<ALAppEnum>();
                    return this.EnumTypes;
                case ALSymbolKind.EnumExtensionType:
                    if (this.EnumExtensionTypes == null)
                        this.EnumExtensionTypes = new ALAppElementsCollection<ALAppEnumExtension>();
                    return this.EnumExtensionTypes;
                case ALSymbolKind.Interface:
                    if (this.Interfaces == null)
                        this.Interfaces = new ALAppElementsCollection<ALAppInterface>();
                    return this.Interfaces;
            }           
            return null;
        }

        #endregion

        #region ALSymbolInformation conversion

        protected void ClearALSymbolCache()
        {
            _alSymbolCache = null;
        }

        public ALSymbolsLibrary ToALSymbolsLibrary()
        {
            return new ALSymbolsLibrary(this.ToALSymbol());
        }

        public override ALSymbol ToALSymbol()
        {
            if (_alSymbolCache == null)
                _alSymbolCache = base.ToALSymbol();
            return _alSymbolCache;
        }

        protected override ALSymbol CreateMainALSymbol()
        {
            return new ALSymbol(ALSymbolKind.Package, StringExtensions.Merge(this.Publisher, this.Name, this.Version));
        }

        protected override void AddChildALSymbols(ALSymbol symbol)
        {
            base.AddChildALSymbols(symbol);

            this.Tables?.AddCollectionToALSymbol(symbol, ALSymbolKind.TableObjectList);
            this.Pages?.AddCollectionToALSymbol(symbol, ALSymbolKind.PageObjectList);
            this.Reports?.AddCollectionToALSymbol(symbol, ALSymbolKind.ReportObjectList);
            this.XmlPorts?.AddCollectionToALSymbol(symbol, ALSymbolKind.XmlPortObjectList);
            this.Queries?.AddCollectionToALSymbol(symbol, ALSymbolKind.QueryObjectList);
            this.Codeunits?.AddCollectionToALSymbol(symbol, ALSymbolKind.CodeunitObjectList);
            this.ControlAddIns?.AddCollectionToALSymbol(symbol, ALSymbolKind.ControlAddInObjectList);
            this.PageExtensions?.AddCollectionToALSymbol(symbol, ALSymbolKind.PageExtensionObjectList);
            this.TableExtensions?.AddCollectionToALSymbol(symbol, ALSymbolKind.TableExtensionObjectList);
            this.Pofiles?.AddCollectionToALSymbol(symbol, ALSymbolKind.ProfileObjectList);
            this.PageCustomizations?.AddCollectionToALSymbol(symbol, ALSymbolKind.PageCustomizationObjectList);
            this.DotNetPackages?.AddCollectionToALSymbol(symbol, ALSymbolKind.DotNetPackageList);
            this.EnumTypes?.AddCollectionToALSymbol(symbol, ALSymbolKind.EnumTypeList);
            this.EnumExtensionTypes?.AddCollectionToALSymbol(symbol, ALSymbolKind.EnumExtensionTypeList);
            this.Interfaces?.AddCollectionToALSymbol(symbol, ALSymbolKind.InterfaceObjectList);
        }

        #endregion

    }
}
