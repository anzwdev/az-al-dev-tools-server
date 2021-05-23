﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.ALTools;
using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.ALSymbols.SymbolReaders;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using AnZwDev.ALTools.Server.Contracts;
using AnZwDev.VSCodeLangServer.Utility;
using AnZwDev.VSCodeLangServer.Protocol.Server;

namespace AnZwDev.ALTools.Server.Handlers
{
    public class DocumentSymbolsRequestHandler : BaseALRequestHandler<DocumentSymbolsRequest, DocumentSymbolsResponse>
    {

        public DocumentSymbolsRequestHandler(ALDevToolsServer server, LanguageServerHost languageServerHost) : base(server, languageServerHost, "al/documentsymbols")
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
                response.root = new ALSymbol();
                response.root.fullName = e.Message;
                this.LogError(e);
            }

            return response;
        }

    }
}
