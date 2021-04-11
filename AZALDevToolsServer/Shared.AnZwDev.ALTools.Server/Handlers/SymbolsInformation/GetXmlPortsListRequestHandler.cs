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
    public class GetXmlPortsListRequestHandler : BaseALRequestHandler<GetXmlPortsListRequest, GetXmlPortsListResponse>
    {

        public GetXmlPortsListRequestHandler(ALDevToolsServer server) : base(server, "al/getxmlportslist")
        {
        }

        protected override async Task<GetXmlPortsListResponse> HandleMessage(GetXmlPortsListRequest parameters, RequestContext<GetXmlPortsListResponse> context)
        {
            GetXmlPortsListResponse response = new GetXmlPortsListResponse();

            ALProject project = this.Server.Workspace.FindProject(parameters.path, true);
            if (project != null)
            {
                XmlPortInformationProvider provider = new XmlPortInformationProvider();
                response.symbols = provider.GetXmlPorts(project);
                response.symbols.Sort(new SymbolInformationComparer());
            }

            return response;
        }
    }

}