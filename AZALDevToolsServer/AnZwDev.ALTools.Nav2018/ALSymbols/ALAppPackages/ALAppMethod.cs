﻿/****************************************************************
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
    public class ALAppMethod : ALAppElementWithName
    {

        public bool IsLocal { get; set; }
        public ALAppElementsCollection<ALAppMethodParameter> Parameters { get; set; }
        public ALAppElementsCollection<ALAppAttribute> Attributes { get; set; }
        public ALAppTypeDefinition ReturnTypeDefinition { get; set; }

        public ALAppMethod()
        {
        }

        protected override ALSymbolKind GetALSymbolKind()
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

        protected override ALSymbolInformation CreateMainALSymbol()
        {
            ALSymbolInformation symbol = base.CreateMainALSymbol();

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

        protected override void AddChildALSymbols(ALSymbolInformation symbol)
        {
            this.Parameters?.AddToALSymbol(symbol, ALSymbolKind.ParameterList, "parameters");
            base.AddChildALSymbols(symbol);
        }

    }
}
