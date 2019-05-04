using AnZwDev.ALTools;
using AnZwDev.ALTools.ALSymbols;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using AZALDevToolsServer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZALDevToolsServer.Handlers
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
