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
    public class ALAppDotNetPackage : ALAppObject
    {

        public ALAppElementsCollection<ALAppDotNetAssemblyDeclaration> AssemblyDeclarations { get; set; }

        public ALAppDotNetPackage()
        {
        }

        protected override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.DotNetPackage;
        }

        protected override void AddChildALSymbols(ALSymbolInformation symbol)
        {
            this.AssemblyDeclarations?.AddToALSymbol(symbol);
            base.AddChildALSymbols(symbol);
        }

    }
}
