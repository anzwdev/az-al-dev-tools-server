using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols.ALAppPackages
{
    public class ALAppTypeDefinition : ALAppElementWithName
    {

        public ALAppSubtypeDefinition Subtype { get; set; }

        public ALAppTypeDefinition()
        {
        }

        public override string GetSourceCode()
        {
            return this.Name;
        }

        public bool IsEmpty()
        {
            return ((this.Subtype == null) || (String.IsNullOrWhiteSpace(this.Subtype.Name)) || (this.Subtype.Name.ToLower() == "none"));
        }

    }
}
