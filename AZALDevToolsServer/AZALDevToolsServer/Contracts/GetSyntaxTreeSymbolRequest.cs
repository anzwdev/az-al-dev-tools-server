using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZALDevToolsServer.Contracts
{
    public class GetSyntaxTreeSymbolRequest
    {

        public string path { get; set; }
        public int[] symbolPath { get; set; }

    }
}
