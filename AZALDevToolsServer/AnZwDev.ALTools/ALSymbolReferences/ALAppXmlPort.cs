using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols.ALAppPackages
{
    public class ALAppXmlPort : ALAppObject
    {

        public ALAppRequestPage RequestPage { get; set; }

        public ALAppXmlPort()
        {
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.XmlPortObject;
        }

    }
}
