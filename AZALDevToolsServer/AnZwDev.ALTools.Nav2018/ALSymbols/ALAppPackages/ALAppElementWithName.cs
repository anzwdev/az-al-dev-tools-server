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
    public class ALAppElementWithName : ALAppBaseElement
    {

        public string Name { get; set; }

        public ALAppElementWithName()
        {
        }

        protected override ALSymbolInformation CreateMainALSymbol()
        {
            return new ALSymbolInformation(this.GetALSymbolKind(), this.Name);
        }

    }
}
