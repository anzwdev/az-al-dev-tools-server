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
using AnZwDev.ALTools.Workspace;

namespace AnZwDev.ALTools.Server.Handlers
{
    public class GetProjectSettingsRequestHandler : BaseALRequestHandler<GetProjectSettingsRequest, GetProjectSettingsResponse>
    {

        public GetProjectSettingsRequestHandler(ALDevToolsServer server) : base(server, "al/getprojectsettings")
        {
        }

        protected override async Task<GetProjectSettingsResponse> HandleMessage(GetProjectSettingsRequest parameters, RequestContext<GetProjectSettingsResponse> context)
        {
            GetProjectSettingsResponse response = new GetProjectSettingsResponse();
            try
            {
                ALProject project = this.Server.Workspace.FindProject(parameters.path, true);
                if (project != null)
                {
                    response.mandatoryPrefixes = project.MandatoryPrefixes;
                    response.mandatorySuffixes = project.MandatorySuffixes;
                    response.mandatoryAffixes = project.MandatoryAffixes;
                }
            }
            catch (Exception e)
            {
            }

            return response;
        }

    }
}
