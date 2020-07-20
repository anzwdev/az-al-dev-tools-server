/****************************************************************
 *                                                              *
 * Legacy version of the library maintained to support Nav 2018 *
 *                                                              *
 ****************************************************************/
using AnZwDev.ALTools.Nav2018;
using AnZwDev.ALTools.Nav2018.ALSymbols;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using AnZwDev.ALTools.Server.Nav2018.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Nav2018.Handlers
{
    public class LibrarySymbolsDetailsRequestHandler : BaseALRequestHandler<LibrarySymbolsDetailsRequest, LibrarySymbolsDetailsResponse>
    {

        public LibrarySymbolsDetailsRequestHandler(ALDevToolsServer server) : base(server, "al/librarysymbolsdetails")
        {
        }

        protected override async Task<LibrarySymbolsDetailsResponse> HandleMessage(LibrarySymbolsDetailsRequest parameters, RequestContext<LibrarySymbolsDetailsResponse> context)
        {
            LibrarySymbolsDetailsResponse response = new LibrarySymbolsDetailsResponse();
            try
            {
                ALSymbolsLibrary library = this.Server.SymbolsLibraries.GetLibrary(parameters.libraryId);
                if (library != null)
                {
                    response.symbols = library.GetSymbolsListByPath(parameters.paths, parameters.kind);
                }
            }
            catch (Exception e)
            {
            }
            return response;
        }

    }
}
