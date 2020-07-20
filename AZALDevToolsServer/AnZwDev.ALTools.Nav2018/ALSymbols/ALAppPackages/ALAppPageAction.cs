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
    public class ALAppPageAction : ALAppElementWithName
    {

        public ALAppPageActionKind Kind { get; set; }
        public ALAppElementsCollection<ALAppPageAction> Actions { get; set; }

        public ALAppPageAction()
        {
        }

        protected override ALSymbolKind GetALSymbolKind()
        {
            return this.Kind.ToALSymbolKind();
        }

        protected override ALSymbolInformation CreateMainALSymbol()
        {
            ALSymbolInformation symbol = base.CreateMainALSymbol();
            symbol.fullName = symbol.kind.ToName() + " " + ALSyntaxHelper.EncodeName(this.Name);
            return symbol;
        }

        protected override void AddChildALSymbols(ALSymbolInformation symbol)
        {
            this.Actions?.AddToALSymbol(symbol);
            base.AddChildALSymbols(symbol);
        }

    }
}
