using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZALDevToolsServer.Contracts
{
    public class DocumentSymbolsRequest
    {

        public string source { get; set; }
        public string path { get; set; }

    }
}
