﻿using AnZwDev.ALTools.Server.Contracts.SymbolsInformation;
using AnZwDev.ALTools.Workspace;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using AnZwDev.VSCodeLangServer.Protocol.Server;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.SymbolsInformation
{
    public class GetTablesListRequestHandler : BaseALRequestHandler<GetTablesListRequest, GetTablesListResponse>
    {

        public GetTablesListRequestHandler(ALDevToolsServer server, LanguageServerHost languageServerHost) : base(server, languageServerHost, "al/gettableslist")
        {
        }

        protected override async Task<GetTablesListResponse> HandleMessage(GetTablesListRequest parameters, RequestContext<GetTablesListResponse> context)
        {
            GetTablesListResponse response = new GetTablesListResponse();

            ALProject project = this.Server.Workspace.FindProject(parameters.path, true);
            if (project != null)
            {
                TableInformationProvider provider = new TableInformationProvider();
                response.symbols = provider.GetTables(project);
                response.symbols.Sort(new SymbolInformationComparer());
            }

            return response;
        }


    }
}
