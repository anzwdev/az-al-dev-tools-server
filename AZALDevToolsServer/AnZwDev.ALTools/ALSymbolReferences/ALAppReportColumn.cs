using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbols.ALAppPackages
{
    public class ALAppReportColumn : ALAppElementWithName
    {

        public string SourceExpression { get; set; }

        public ALAppReportColumn()
        {
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.ReportColumn;
        }

        protected override ALSymbolInformation CreateMainALSymbol()
        {
            ALSymbolInformation symbol = base.CreateMainALSymbol();
            if (!String.IsNullOrWhiteSpace(this.SourceExpression))
                symbol.fullName = ALSyntaxHelper.EncodeName(this.Name) + ": " + this.SourceExpression;
            return symbol;
        }


    }
}
