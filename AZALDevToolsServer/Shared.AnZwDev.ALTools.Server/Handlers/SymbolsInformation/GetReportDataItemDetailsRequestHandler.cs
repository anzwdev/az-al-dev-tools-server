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
    public class GetReportDataItemDetailsRequestHandler : BaseALRequestHandler<GetReportDataItemDetailsRequest, GetReportDataItemDetailsResponse>
    {

        public GetReportDataItemDetailsRequestHandler(ALDevToolsServer server) : base(server, "al/getreportdataitemdetails")
        {
        }

        protected override async Task<GetReportDataItemDetailsResponse> HandleMessage(GetReportDataItemDetailsRequest parameters, RequestContext<GetReportDataItemDetailsResponse> context)
        {
            GetReportDataItemDetailsResponse response = new GetReportDataItemDetailsResponse();

            try
            {

                ALProject project = this.Server.Workspace.FindProject(parameters.path, true);
                if (project != null)
                {
                    ReportInformationProvider provider = new ReportInformationProvider();
                    response.symbol = provider.GetReportDataItemInformationDetails(project, parameters.objectName, parameters.name, parameters.getExistingFields, parameters.getAvailableFields);
                    if (response.symbol != null)
                        response.symbol.Sort();
                }
            }
            catch (Exception e)
            {
                response.symbol = new ReportDataItemInformation();
                response.symbol.Name = e.Message;
            }

            return response;
        }

    }
}