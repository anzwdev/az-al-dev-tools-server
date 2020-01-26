using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZALDevToolsServer.Contracts
{
    public class AddAppAreasResponse
    {

        public string source { get; set; }
        public int noOfAppAreas { get; set; }
        public int noOfFiles { get; set; }
        public bool error { get; set; }
        public string errorMessage { get; set; }

    }
}
