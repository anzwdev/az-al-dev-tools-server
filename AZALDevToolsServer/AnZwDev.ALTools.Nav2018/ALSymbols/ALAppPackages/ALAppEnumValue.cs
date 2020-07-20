/****************************************************************
 *                                                              *
 * Legacy version of the library maintained to support Nav 2018 *
 *                                                              *
 ****************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Nav2018.ALSymbols.ALAppPackages
{
    public class ALAppEnumValue : ALAppElementWithName
    {

        public int Ordinal { get; set; }

        public ALAppEnumValue()
        {
        }

        protected override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.EnumValue;
        }

        protected override ALSymbolInformation CreateMainALSymbol()
        {
            return new ALSymbolInformation(this.GetALSymbolKind(), this.Name, this.Ordinal);
        }

    }
}
