using AnZwDev.ALTools.Server.Contracts.SymbolsInformation;
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
    public class GetTableFieldsListRequestHandler : BaseALRequestHandler<GetTableFieldsListRequest, GetTableFieldsListResponse>
    {

        public GetTableFieldsListRequestHandler(ALDevToolsServer server, LanguageServerHost languageServerHost) : base(server, languageServerHost, "al/gettablefieldslist")
        {
        }

        protected override async Task<GetTableFieldsListResponse> HandleMessage(GetTableFieldsListRequest parameters, RequestContext<GetTableFieldsListResponse> context)
        {
            GetTableFieldsListResponse response = new GetTableFieldsListResponse();

            ALProject project = this.Server.Workspace.FindProject(parameters.path, true);
            if (project != null)
            {
                TableInformationProvider provider = new TableInformationProvider();
                response.symbols = provider.GetTableFields(project, parameters.table, parameters.includeDisabled, parameters.includeObsolete);
                response.symbols.Sort(new SymbolInformationComparer());
            }

            return response;
        }

    }
}
