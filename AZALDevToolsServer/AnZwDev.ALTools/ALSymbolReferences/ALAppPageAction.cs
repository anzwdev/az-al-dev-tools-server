using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols.ALAppPackages
{
    public class ALAppPageAction : ALAppElementWithName
    {

        public ALAppPageActionKind Kind { get; set; }
        public ALAppElementsCollection<ALAppPageAction> Actions { get; set; }
        public ALAppPropertiesCollection Properties { get; set; }

        public ALAppPageAction()
        {
        }

        public override ALSymbolKind GetALSymbolKind()
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
