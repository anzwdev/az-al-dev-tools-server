/****************************************************************
 *                                                              *
 * Legacy version of the library maintained to support Nav 2018 *
 *                                                              *
 ****************************************************************/
using AnZwDev.ALTools.Nav2018;
using AnZwDev.ALTools.Nav2018.CodeAnalysis;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using AnZwDev.ALTools.Server.Nav2018.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Nav2018.Handlers
{
    public class GetCodeAnalyzersRulesRequestHandler : BaseALRequestHandler<GetCodeAnalyzersRulesRequest, GetCodeAnalyzersRulesResponse>
    {

        public GetCodeAnalyzersRulesRequestHandler(ALDevToolsServer server) : base(server, "al/getcodeanalyzersrules")
        {
        }

        protected override async Task<GetCodeAnalyzersRulesResponse> HandleMessage(GetCodeAnalyzersRulesRequest parameters, RequestContext<GetCodeAnalyzersRulesResponse> context)
        {
            CodeAnalyzersLibrary library = this.Server.CodeAnalyzersLibraries.GetCodeAnalyzersLibrary(parameters.name);

            GetCodeAnalyzersRulesResponse response = new GetCodeAnalyzersRulesResponse();
            response.name = parameters.name;
            if (library != null)
                response.rules = library.Rules;

            return response;
        }


    }
}
