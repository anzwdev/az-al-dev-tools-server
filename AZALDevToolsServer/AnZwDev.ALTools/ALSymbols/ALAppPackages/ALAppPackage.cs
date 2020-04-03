using AnZwDev.ALTools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols.ALAppPackages
{
    public class ALAppPackage : ALAppBaseElement
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
        //public ALAppElementsCollection<ALAppEnum> Enums { get; set; }
        public ALAppElementsCollection<ALAppEnum> EnumTypes { get; set; }
        public ALAppElementsCollection<ALAppEnumExtension> EnumTypeExtensions { get; set; }
        public ALAppElementsCollection<ALAppInterface> Interfaces { get; set; }

        public ALAppPackage()
        {
        }

        protected override ALSymbolInformation CreateMainALSymbol()
        {
            return new ALSymbolInformation(ALSymbolKind.Package, StringHelper.Merge(this.Publisher, this.Name, this.Version));
        }

        protected override void AddChildALSymbols(ALSymbolInformation symbol)
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
            this.EnumTypeExtensions?.AddCollectionToALSymbol(symbol, ALSymbolKind.EnumExtensionTypeList);
            this.Interfaces?.AddCollectionToALSymbol(symbol, ALSymbolKind.InterfaceObjectList);

        }

    }
}
