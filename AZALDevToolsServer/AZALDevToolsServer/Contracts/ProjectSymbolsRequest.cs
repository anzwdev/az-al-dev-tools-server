using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZALDevToolsServer.Contracts
{
    public class ProjectSymbolsRequest
    {

        public bool includeDependencies { get; set; }
        public string projectPath { get; set; }
        public string packagesFolder { get; set; }

    }
}
