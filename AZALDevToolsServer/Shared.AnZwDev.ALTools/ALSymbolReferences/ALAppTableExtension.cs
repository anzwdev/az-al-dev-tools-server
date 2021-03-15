using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.ALTools.ALSymbols;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppTableExtension : ALAppTable
    {

        public string TargetObject { get; set; }

        public ALAppTableExtension()
        {
        }

        protected override ALSymbolInformation CreateMainALSymbol()
        {
            ALSymbolInformation symbol = base.CreateMainALSymbol();
            symbol.extends = this.TargetObject;
            return symbol;
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.TableExtensionObject;
        }

    }
}
