﻿using AnZwDev.ALTools.Server.Contracts.SymbolsInformation;
using AnZwDev.ALTools.Workspace;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace AnZwDev.ALTools.Server.Handlers.SymbolsInformation
{
    public class GetReportsListRequestHandler : BaseALRequestHandler<GetReportsListRequest, GetReportsListResponse>
    {

        public GetReportsListRequestHandler(ALDevToolsServer server) : base(server, "al/getreportslist")
        {
        }

        protected override async Task<GetReportsListResponse> HandleMessage(GetReportsListRequest parameters, RequestContext<GetReportsListResponse> context)
        {
            GetReportsListResponse response = new GetReportsListResponse();

            ALProject project = this.Server.Workspace.FindProject(parameters.path, true);
            if (project != null)
            {
                ReportInformationProvider provider = new ReportInformationProvider();
                response.symbols = provider.GetReports(project);
                response.symbols.Sort(new SymbolInformationComparer());
            }

            return response;
        }
    }
}