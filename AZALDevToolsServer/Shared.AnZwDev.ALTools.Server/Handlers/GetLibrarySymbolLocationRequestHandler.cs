using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Server.Contracts;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers
{
    public class GetLibrarySymbolLocationRequestHandler : BaseALRequestHandler<GetLibrarySymbolLocationRequest, GetLibrarySymbolLocationResponse>
    {

        public GetLibrarySymbolLocationRequestHandler(ALDevToolsServer server) : base(server, "al/librarysymbollocation")
        {
        }

        protected override async Task<GetLibrarySymbolLocationResponse> HandleMessage(GetLibrarySymbolLocationRequest parameters, RequestContext<GetLibrarySymbolLocationResponse> context)
        {
            GetLibrarySymbolLocationResponse response = new GetLibrarySymbolLocationResponse();
            try
            {
                ALSymbolsLibrary library = this.Server.SymbolsLibraries.GetLibrary(parameters.libraryId);
                if (library != null)
                {
                    ALSymbol symbol = library.GetSymbolByPath(parameters.path);
                    if (symbol != null)
                        response.location = library.GetSymbolSourceLocation(symbol);
                }
            }
            catch (Exception e)
            {
            }
            return response;
        }


    }
}
