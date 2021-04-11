using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.ALTools.ALSymbols;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppMethod : ALAppElementWithName
    {

        public bool IsLocal { get; set; }
        public string AccessModifier { get; set; }
        public int MethodKind { get; set; }
        public ALAppElementsCollection<ALAppMethodParameter> Parameters { get; set; }
        public ALAppElementsCollection<ALAppAttribute> Attributes { get; set; }
        public ALAppTypeDefinition ReturnTypeDefinition { get; set; }

        public ALAppMethod()
        {
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            if (this.Attributes != null)
            {
                for (int i=0; i<this.Attributes.Count; i++)
                {
                    ALSymbolKind kind = ALSyntaxHelper.MemberAttributeToMethodKind(this.Attributes[i].Name);
                    if (kind != ALSymbolKind.Undefined)
                        return kind;
                }
            }
            if (this.IsLocal)
                return ALSymbolKind.LocalMethodDeclaration;
            return ALSymbolKind.MethodDeclaration;
        }

        protected override ALSymbol CreateMainALSymbol()
        {
            ALSymbol symbol = base.CreateMainALSymbol();

            //build full name
            string fullName = ALSyntaxHelper.EncodeName(this.Name) + "(";
            if (this.Parameters != null)
            {
                for (int i=0; i<this.Parameters.Count; i++)
                {
                    if (i > 0)
                        fullName = fullName + ", ";
                    fullName = fullName + this.Parameters[i].GetSourceCode();
                }
            }
            fullName = fullName + ")";
            if ((this.ReturnTypeDefinition != null) && (!this.ReturnTypeDefinition.IsEmpty()))
                fullName = fullName + ": " + this.ReturnTypeDefinition.GetSourceCode();

            symbol.fullName = fullName;
            return symbol;
        }

        protected override void AddChildALSymbols(ALSymbol symbol)
        {
            this.Parameters?.AddToALSymbol(symbol, ALSymbolKind.ParameterList, "parameters");
            base.AddChildALSymbols(symbol);
        }

    }
}
