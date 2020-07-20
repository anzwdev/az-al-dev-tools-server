/****************************************************************
 *                                                              *
 * Legacy version of the library maintained to support Nav 2018 *
 *                                                              *
 ****************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.ALTools.Nav2018;
using AnZwDev.ALTools.Nav2018.ALSymbols;
using AnZwDev.ALTools.Nav2018.ALSymbols.SymbolReaders;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using AnZwDev.ALTools.Server.Nav2018.Contracts;

namespace AnZwDev.ALTools.Server.Nav2018.Handlers
{
    public class DocumentSymbolsRequestHandler : BaseALRequestHandler<DocumentSymbolsRequest, DocumentSymbolsResponse>
    {

        public DocumentSymbolsRequestHandler(ALDevToolsServer server) : base(server, "al/documentsymbols")
        {
        }

        protected override async Task<DocumentSymbolsResponse> HandleMessage(DocumentSymbolsRequest parameters, RequestContext<DocumentSymbolsResponse> context)
        {
            DocumentSymbolsResponse response = new DocumentSymbolsResponse();
            try
            {
                ALSymbolInfoSyntaxTreeReader symbolTreeBuilder = new ALSymbolInfoSyntaxTreeReader(
                    parameters.includeProperties);
                if (String.IsNullOrWhiteSpace(parameters.source))
                    response.root = symbolTreeBuilder.ProcessSourceFile(parameters.path);
                else
                    response.root = symbolTreeBuilder.ProcessSourceCode(parameters.source);
            }
            catch (Exception e)
            {
                response.root = new ALSymbolInformation();
                response.root.fullName = e.Message;
            }

            return response;
        }

    }
}
