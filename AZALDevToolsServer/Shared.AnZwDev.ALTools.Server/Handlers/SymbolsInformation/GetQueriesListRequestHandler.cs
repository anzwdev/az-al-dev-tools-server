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
    public class GetQueriesListRequestHandler : BaseALRequestHandler<GetQueriesListRequest, GetQueriesListResponse>
    {

        public GetQueriesListRequestHandler(ALDevToolsServer server) : base(server, "al/getquerieslist")
        {
        }

        protected override async Task<GetQueriesListResponse> HandleMessage(GetQueriesListRequest parameters, RequestContext<GetQueriesListResponse> context)
        {
            GetQueriesListResponse response = new GetQueriesListResponse();

            ALProject project = this.Server.Workspace.FindProject(parameters.path, true);
            if (project != null)
            {
                QueryInformationProvider provider = new QueryInformationProvider();
                response.symbols = provider.GetQueries(project);
                response.symbols.Sort(new SymbolInformationComparer());
            }

            return response;
        }
    }
}
