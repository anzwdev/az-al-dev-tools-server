using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.ALTools.ALSymbols;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppTableField : ALAppElementWithNameId
    {

        public ALAppTypeDefinition TypeDefinition { get; set; }
        public ALAppPropertiesCollection Properties { get; set; }

        public ALAppTableField()
        {
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.Field;
        }

        protected override ALSymbolInformation CreateMainALSymbol()
        {
            ALSymbolInformation symbol = base.CreateMainALSymbol();
            if (this.TypeDefinition != null)
                symbol.fullName = ALSyntaxHelper.EncodeName(this.Name) + ": " + this.TypeDefinition.GetSourceCode();
            this.UpdateSymbolSubtype(symbol);
            return symbol;
        }

        protected void UpdateSymbolSubtype(ALSymbolInformation symbol)
        {
            //detect subtype
            if (this.Properties != null)
            {
                ALAppProperty enabledState = this.Properties.Where(p => (p.Name == "Enabled")).FirstOrDefault();
                if ((enabledState != null) && (enabledState.Value == "0"))
                {
                    symbol.subtype = "Disabled";
                    symbol.fullName = symbol.fullName + " (Disabled)";
                    return;
                }

                ALAppProperty obsoleteState = this.Properties.Where(p => (p.Name == "ObsoleteState")).FirstOrDefault();
                if ((obsoleteState != null) && (!String.IsNullOrWhiteSpace(obsoleteState.Value)))
                {
                    string obsoleteReasonText = "";
                    ALAppProperty obsoleteReason = this.Properties.Where(p => (p.Name == "ObsoleteReason")).FirstOrDefault();
                    if ((obsoleteReason != null) && (!String.IsNullOrWhiteSpace(obsoleteReason.Value)))
                        obsoleteReasonText = ": " + obsoleteReason.Value.Trim();

                    if (obsoleteState.Value.Equals("Pending", StringComparison.CurrentCultureIgnoreCase))
                    {
                        symbol.subtype = "ObsoletePending";
                        symbol.fullName = symbol.fullName + " (Obsolete-Pending" + obsoleteReasonText + ")";
                    }
                    else if (obsoleteState.Value.Equals("Removed", StringComparison.CurrentCultureIgnoreCase))
                    {
                        symbol.subtype = "ObsoleteRemoved";
                        symbol.fullName = symbol.fullName + " (Obsolete-Removed" + obsoleteReasonText + ")";
                    }

                }

            }

        }

    }
}
