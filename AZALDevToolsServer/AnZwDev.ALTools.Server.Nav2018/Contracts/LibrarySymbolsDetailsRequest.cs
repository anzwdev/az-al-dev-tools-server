/****************************************************************
 *                                                              *
 * Legacy version of the library maintained to support Nav 2018 *
 *                                                              *
 ****************************************************************/
using AnZwDev.ALTools.Nav2018.ALSymbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Nav2018.Contracts
{
    public class LibrarySymbolsDetailsRequest
    {

        public int libraryId { get; set; }
        public ALSymbolKind kind { get; set; }
        public int[][] paths { get; set; }

    }
}
