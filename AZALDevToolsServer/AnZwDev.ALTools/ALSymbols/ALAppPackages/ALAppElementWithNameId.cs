using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols.ALAppPackages
{
    public class ALAppElementWithNameId : ALAppElementWithName
    {

        public int Id { get; set; }

        public ALAppElementWithNameId()
        {
        }

        protected override ALSymbolInformation CreateMainALSymbol()
        {
            return new ALSymbolInformation(this.GetALSymbolKind(), this.Name, this.Id);
        }

    }
}
