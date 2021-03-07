using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols.ALAppPackages
{
    public class ALAppMethodParameter : ALAppVariable
    {

        public bool IsVar { get; set; }

        public ALAppMethodParameter()
        {
        }

        public override string GetSourceCode()
        {
            string sourceCode = ALSyntaxHelper.EncodeName(this.Name);
            if (this.IsVar)
                sourceCode = "var " + sourceCode;
            if (this.TypeDefinition != null)
                sourceCode = sourceCode + ": " + this.TypeDefinition.GetSourceCode();
            return sourceCode;
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.Parameter;
        }

    }
}
