using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols.ALAppPackages
{
    public class ALAppDotNetAssemblyDeclaration : ALAppElementWithName
    {

        public ALAppElementsCollection<ALAppDotNetTypeDeclaration> TypeDeclarations { get; set; }

        public ALAppDotNetAssemblyDeclaration()
        {
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.DotNetAssembly;
        }

        protected override void AddChildALSymbols(ALSymbolInformation symbol)
        {
            this.TypeDeclarations?.AddToALSymbol(symbol);
            base.AddChildALSymbols(symbol);
        }

    }
}
