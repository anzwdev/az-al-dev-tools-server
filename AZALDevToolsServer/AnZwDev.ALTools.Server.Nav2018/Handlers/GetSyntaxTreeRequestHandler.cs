/****************************************************************
 *                                                              *
 * Legacy version of the library maintained to support Nav 2018 *
 *                                                              *
 ****************************************************************/
using AnZwDev.ALTools.Nav2018;
using AnZwDev.ALTools.Nav2018.ALSymbols;
using AnZwDev.ALTools.Nav2018.ALSymbols.SymbolReaders;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using AnZwDev.ALTools.Server.Nav2018.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Nav2018.Handlers
{
    public class GetSyntaxTreeRequestHandler : BaseALRequestHandler<GetSyntaxTreeRequest, GetSyntaxTreeResponse>
    {

        public GetSyntaxTreeRequestHandler(ALDevToolsServer server) : base(server, "al/getsyntaxtree")
        {
        }

        protected override async Task<GetSyntaxTreeResponse> HandleMessage(GetSyntaxTreeRequest parameters, RequestContext<GetSyntaxTreeResponse> context)
        {
            ALSyntaxTree syntaxTree = this.Server.SyntaxTrees.FindOrCreate(parameters.path, parameters.open);
            syntaxTree.Load(parameters.source);

            GetSyntaxTreeResponse response = new GetSyntaxTreeResponse();
            response.root = syntaxTree.RootSymbol;

            return response;
        }

    }
}
