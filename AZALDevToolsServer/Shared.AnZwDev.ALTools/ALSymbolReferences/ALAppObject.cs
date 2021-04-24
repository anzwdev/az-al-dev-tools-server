using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.ALTools.ALSymbols;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppObject : ALAppElementWithNameId
    {

        public string ReferenceSourceFileName { get; set; }
        public ALAppPropertiesCollection Properties { get; set; }
        public ALAppElementsCollection<ALAppVariable> Variables { get; set; }
        public ALAppElementsCollection<ALAppMethod> Methods { get; set; }


        public ALAppObject()
        {
        }

        protected override ALSymbol CreateMainALSymbol()
        {
            ALSymbol symbol = base.CreateMainALSymbol();
            symbol.fullName = symbol.kind.ToName() + " " + ALSyntaxHelper.EncodeName(this.Name);
            return symbol;
        }

        protected override void AddChildALSymbols(ALSymbol symbol)
        {
            this.Variables?.AddToALSymbol(symbol, ALSymbolKind.VarSection, "var");
            this.Methods?.AddToALSymbol(symbol);
            base.AddChildALSymbols(symbol);
        }

        public virtual void ReplaceIdReferences(ALAppObjectIdMap idMap)
        {
        }

    }
}
