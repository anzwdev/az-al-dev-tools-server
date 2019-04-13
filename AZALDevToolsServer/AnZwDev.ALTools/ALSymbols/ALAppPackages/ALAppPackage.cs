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

            this.Tables?.AddToALSymbol(symbol, ALSymbolKind.TableObjectList, "Tables");
            this.Pages?.AddToALSymbol(symbol, ALSymbolKind.PageObjectList, "Pages");
            this.Reports?.AddToALSymbol(symbol, ALSymbolKind.ReportObjectList, "Reports");
            this.XmlPorts?.AddToALSymbol(symbol, ALSymbolKind.XmlPortObjectList, "XmlPorts");
            this.Queries?.AddToALSymbol(symbol, ALSymbolKind.QueryObjectList, "Queries");
            this.Codeunits?.AddToALSymbol(symbol, ALSymbolKind.CodeunitObjectList, "Codeunits");
            this.ControlAddIns?.AddToALSymbol(symbol, ALSymbolKind.ControlAddInObjectList, "ControlAddIns");
            this.PageExtensions?.AddToALSymbol(symbol, ALSymbolKind.PageExtensionObjectList, "PageExtensions");
            this.TableExtensions?.AddToALSymbol(symbol, ALSymbolKind.TableExtensionObjectList, "TableExtensions");
            this.Pofiles?.AddToALSymbol(symbol, ALSymbolKind.ProfileObjectList, "Pofiles");
            this.PageCustomizations?.AddToALSymbol(symbol, ALSymbolKind.PageCustomizationObjectList, "PageCustomizations");
            this.DotNetPackages?.AddToALSymbol(symbol, ALSymbolKind.DotNetPackageList, "DotNetPackages");
            this.EnumTypes?.AddToALSymbol(symbol, ALSymbolKind.EnumTypeList, "Enums");
            this.EnumTypeExtensions?.AddToALSymbol(symbol, ALSymbolKind.EnumExtensionTypeList, "EnumExtensions");

        }

    }
}
