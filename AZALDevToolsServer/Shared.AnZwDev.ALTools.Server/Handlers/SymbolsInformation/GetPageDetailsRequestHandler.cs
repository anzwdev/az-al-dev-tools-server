using AnZwDev.ALTools.Server.Contracts.SymbolsInformation;
using AnZwDev.ALTools.Workspace;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.SymbolsInformation
{
    public class GetPageDetailsRequestHandler : BaseALRequestHandler<GetPageDetailsRequest, GetPageDetailsResponse>
    {

        public GetPageDetailsRequestHandler(ALDevToolsServer server) : base(server, "al/getpagedetails")
        {
        }

        protected override async Task<GetPageDetailsResponse> HandleMessage(GetPageDetailsRequest parameters, RequestContext<GetPageDetailsResponse> context)
        {
                GetPageDetailsResponse response = new GetPageDetailsResponse();

            try
            {

                ALProject project = this.Server.Workspace.FindProject(parameters.path, true);
                if (project != null)
                {
                    PageInformationProvider provider = new PageInformationProvider();
                    response.symbol = provider.GetPageDetails(project, parameters.name, parameters.getExistingFields, parameters.getAvailableFields);
                    if (response.symbol != null)
                        response.symbol.Sort();
                }
            }
            catch (Exception e)
            {
                response.symbol = new PageInformation();
                response.symbol.Name = e.Message;
            }

                return response;

        }

    }
}
