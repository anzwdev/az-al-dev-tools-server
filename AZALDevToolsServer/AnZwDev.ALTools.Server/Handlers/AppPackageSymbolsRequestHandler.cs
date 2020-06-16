using AnZwDev.ALTools;
using AnZwDev.ALTools.ALSymbols;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using AnZwDev.ALTools.Server.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers
{
    public class AppPackageSymbolsRequestHandler : BaseALRequestHandler<AppPackageSymbolsRequest, AppPackageSymbolsResponse>
    {

        public AppPackageSymbolsRequestHandler(ALDevToolsServer server) : base(server, "al/packagesymbols")
        {
        }

        protected override async Task<AppPackageSymbolsResponse> HandleMessage(AppPackageSymbolsRequest parameters, RequestContext<AppPackageSymbolsResponse> context)
        {
            AppPackageSymbolsResponse response = new AppPackageSymbolsResponse();
            try
            {
                ALPackageSymbolsLibrary library = this.Server.AppPackagesCache.GetSymbols(parameters.path, false);
                if (library != null)
                {
                    response.libraryId = this.Server.SymbolsLibraries.AddLibrary(library);
                    response.root = library.GetObjectsTree(); // .Root;
                }
            }
            catch (Exception e)
            {
                response.root = new ALSymbolInformation();
                response.root.fullName = e.Message;
            }
            return response;
        }

    }
}
