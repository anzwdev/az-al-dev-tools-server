using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.ALTools.ALSymbols;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppPageControl : ALAppElementWithName
    {

        public string Expression { get; set; }
        public ALAppPageControlKind Kind { get; set; }
        public ALAppTypeDefinition TypeDefinition { get; set; }
        public ALAppElementsCollection<ALAppPageControl> Controls { get; set; }
        public ALAppPropertiesCollection Properties { get; set; }

        public ALAppPageControl()
        {
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            return this.Kind.ToALSymbolKind();
        }

        protected override ALSymbol CreateMainALSymbol()
        {
            ALSymbol symbol = base.CreateMainALSymbol();
            symbol.fullName = symbol.kind.ToName() + " " + ALSyntaxHelper.EncodeName(this.Name);
            return symbol;
        }

        protected override void AddChildALSymbols(ALSymbol symbol)
        {
            this.Controls?.AddToALSymbol(symbol);
            base.AddChildALSymbols(symbol);
        }

    }
}
