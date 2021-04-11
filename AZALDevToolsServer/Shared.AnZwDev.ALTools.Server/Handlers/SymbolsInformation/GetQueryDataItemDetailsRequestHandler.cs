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
    public class GetQueryDataItemDetailsRequestHandler : BaseALRequestHandler<GetQueryDataItemDetailsRequest, GetQueryDataItemDetailsResponse>
    {

        public GetQueryDataItemDetailsRequestHandler(ALDevToolsServer server) : base(server, "al/getquerydataitemdetails")
        {
        }

        protected override async Task<GetQueryDataItemDetailsResponse> HandleMessage(GetQueryDataItemDetailsRequest parameters, RequestContext<GetQueryDataItemDetailsResponse> context)
        {
            GetQueryDataItemDetailsResponse response = new GetQueryDataItemDetailsResponse();

            try
            {

                ALProject project = this.Server.Workspace.FindProject(parameters.path, true);
                if (project != null)
                {
                    QueryInformationProvider provider = new QueryInformationProvider();
                    response.symbol = provider.GetQueryDataItemInformationDetails(project, parameters.objectName, parameters.name, parameters.getExistingFields, parameters.getAvailableFields);
                    if (response.symbol != null)
                        response.symbol.Sort();
                }
            }
            catch (Exception e)
            {
                response.symbol = new QueryDataItemInformation();
                response.symbol.Name = e.Message;
            }

            return response;
        }

    }
}