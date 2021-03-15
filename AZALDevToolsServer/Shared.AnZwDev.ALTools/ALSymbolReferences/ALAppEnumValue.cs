using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppEnumValue : ALAppElementWithName
    {

        public int Ordinal { get; set; }

        public ALAppEnumValue()
        {
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.EnumValue;
        }

        protected override ALSymbolInformation CreateMainALSymbol()
        {
            return new ALSymbolInformation(this.GetALSymbolKind(), this.Name, this.Ordinal);
        }

    }
}
