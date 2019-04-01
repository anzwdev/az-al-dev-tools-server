using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZALDevToolsServer.Contracts
{
    public class DocumentSymbolsResponse
    {

        public ALSymbolInformation root { get; set; }

        public DocumentSymbolsResponse()
        {
        }

    }
}
