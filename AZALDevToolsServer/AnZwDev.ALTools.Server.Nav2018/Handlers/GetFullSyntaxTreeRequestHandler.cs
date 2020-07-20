/****************************************************************
 *                                                              *
 * Legacy version of the library maintained to support Nav 2018 *
 *                                                              *
 ****************************************************************/
using AnZwDev.ALTools.Nav2018;
using AnZwDev.ALTools.Nav2018.ALSymbols;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using AnZwDev.ALTools.Server.Nav2018.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Nav2018.Handlers
{
    public class GetFullSyntaxTreeRequestHandler : BaseALRequestHandler<GetFullSyntaxTreeRequest, GetFullSyntaxTreeResponse>
    {
        
        public GetFullSyntaxTreeRequestHandler(ALDevToolsServer server) : base(server, "al/getfullsyntaxtree")
        {
        }

        protected override async Task<GetFullSyntaxTreeResponse> HandleMessage(GetFullSyntaxTreeRequest parameters, RequestContext<GetFullSyntaxTreeResponse> context)
        {
            ALFullSyntaxTree syntaxTree = new ALFullSyntaxTree();
            syntaxTree.Load(parameters.source, parameters.path);

            GetFullSyntaxTreeResponse response = new GetFullSyntaxTreeResponse();
            response.root = syntaxTree.Root;
            
            return response;
        }
    }
}

