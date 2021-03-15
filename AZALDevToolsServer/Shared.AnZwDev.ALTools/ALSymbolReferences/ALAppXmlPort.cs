using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.ALTools.ALSymbols;

namespace AnZwDev.ALTools.ALSymbolReferences
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
