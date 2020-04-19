using AnZwDev.ALTools;
using AnZwDev.ALTools.ALSymbols;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using AZALDevToolsServer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZALDevToolsServer.Handlers
{
    public class GetFullSyntaxTreeRequestHandler : BaseALRequestHandler<GetFullSyntaxTreeRequest, GetFullSyntaxTreeResponse>
    {
        
        public GetFullSyntaxTreeRequestHandler(ALDevToolsServer server) : base(server, "al/getfullsyntaxtree")
        {
        }

        protected override async Task<GetFullSyntaxTreeResponse> HandleMessage(GetFullSyntaxTreeRequest parameters, RequestContext<GetFullSyntaxTreeResponse> context)
        {
            ALFullSyntaxTree syntaxTree = new ALFullSyntaxTree(this.Server.ALExtensionProxy);
            syntaxTree.Load(parameters.source, parameters.path);

            GetFullSyntaxTreeResponse response = new GetFullSyntaxTreeResponse();
            response.root = syntaxTree.Root;
            
            return response;
        }
    }
}

