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
    public class ALAppVariable : ALAppElementWithName
    {

        public ALAppTypeDefinition TypeDefinition { get; set; }

        public ALAppVariable()
        {
        }

        protected override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.VariableDeclaration;
        }

        protected override ALSymbolInformation CreateMainALSymbol()
        {
            ALSymbolInformation symbol = base.CreateMainALSymbol();
            if (this.TypeDefinition != null)
                symbol.fullName = ALSyntaxHelper.EncodeName(this.Name) + ": " + this.TypeDefinition.GetSourceCode();
            return symbol;
        }

    }
}
