using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols.ALAppPackages
{
    public class ALAppEnum : ALAppObject
    {

        public ALAppElementsCollection<ALAppEnumValue> Values { get; set; }

        public ALAppEnum()
        {
        }

        protected override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.EnumType;
        }

        protected override void AddChildALSymbols(ALSymbolInformation symbol)
        {
            this.Values?.AddToALSymbol(symbol);
            base.AddChildALSymbols(symbol);
        }

    }
}
