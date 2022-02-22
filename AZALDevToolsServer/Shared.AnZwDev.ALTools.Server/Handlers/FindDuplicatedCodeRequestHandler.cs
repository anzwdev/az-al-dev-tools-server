using AnZwDev.ALTools.DuplicateCodeSearch;
using AnZwDev.ALTools.Server.Contracts;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using AnZwDev.VSCodeLangServer.Protocol.Server;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers
{
    public class FindDuplicatedCodeRequestHandler : BaseALRequestHandler<FindDuplicatedCodeRequest, FindDuplicatedCodeResponse>
    {

        public FindDuplicatedCodeRequestHandler(ALDevToolsServer server, LanguageServerHost languageServerHost) : base(server, languageServerHost, "al/findDuplicatedCode")
        {
        }

#pragma warning disable 1998
        protected override async Task<FindDuplicatedCodeResponse> HandleMessage(FindDuplicatedCodeRequest parameters, RequestContext<FindDuplicatedCodeResponse> context)
        {
            FindDuplicatedCodeResponse response = new FindDuplicatedCodeResponse();
            try
            {
                DCDuplicateCodeAnalyzer analyzer = new DCDuplicateCodeAnalyzer(parameters.minNoOfStatements);
                response.duplicates = analyzer.FindDuplicates(this.Server.Workspace);
            }
            catch (Exception e)
            {
                response.SetError(e);
                this.LogError(e);
            }
            return response;
        }
#pragma warning restore 1998
    }
}
