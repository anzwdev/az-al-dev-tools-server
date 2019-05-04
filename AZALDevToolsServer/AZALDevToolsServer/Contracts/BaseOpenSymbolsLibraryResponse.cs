using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZALDevToolsServer.Contracts
{
    public class BaseOpenSymbolsLibraryResponse : BaseSymbolsResponse
    {

        public int libraryId { get; set; }

    }
}
