using AnZwDev.ALTools.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols.ALAppPackages
{
    public class ALAppDotNetTypeDeclaration : ALAppBaseElement
    {

        public string TypeName { get; set; }
        public string AliasName { get; set; }

        public ALAppDotNetTypeDeclaration()
        {
        }

        protected override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.DotNetTypeDeclaration;
        }

        protected override ALSymbolInformation CreateMainALSymbol()
        {
            ALSymbolInformation symbol = new ALSymbolInformation(this.GetALSymbolKind(), this.TypeName);
            if (!String.IsNullOrWhiteSpace(this.AliasName))
                symbol.fullName = ALSyntaxHelper.EncodeName(this.AliasName) + ": " + ALSyntaxHelper.EncodeName(this.TypeName.NotNull());
            return symbol;
        }

    }
}
