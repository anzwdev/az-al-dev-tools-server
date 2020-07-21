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
    public class GetSyntaxTreeSymbolRequestHandler : BaseALRequestHandler<GetSyntaxTreeSymbolRequest, GetSyntaxTreeSymbolResponse>
    {

        public GetSyntaxTreeSymbolRequestHandler(ALDevToolsServer server) : base(server, "al/getsyntaxtreesymbol")
        {
        }

        protected override async Task<GetSyntaxTreeSymbolResponse> HandleMessage(GetSyntaxTreeSymbolRequest parameters, RequestContext<GetSyntaxTreeSymbolResponse> context)
        {
            ALSyntaxTree syntaxTree = this.Server.SyntaxTrees.FindOrCreate(parameters.path, false);
            ALSyntaxTreeSymbol symbol = syntaxTree.GetSyntaxTreeSymbolByPath(parameters.symbolPath);
            if (symbol != null)
                symbol = symbol.CreateSerializableCopy();

            GetSyntaxTreeSymbolResponse response = new GetSyntaxTreeSymbolResponse();
            response.symbol = symbol;

            return response;
        }
    }

}
