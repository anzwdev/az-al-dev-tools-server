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
    public class AppPackageSymbolsRequestHandler : BaseALRequestHandler<AppPackageSymbolsRequest, AppPackageSymbolsResponse>
    {

        protected ALPackageSymbolsCache _appPackagesCache;

        public AppPackageSymbolsRequestHandler(ALDevToolsServer server) : base(server, "al/packagesymbols")
        {
            _appPackagesCache = new ALPackageSymbolsCache(server.ALExtensionProxy);
        }

        protected override async Task<AppPackageSymbolsResponse> HandleMessage(AppPackageSymbolsRequest parameters, RequestContext<AppPackageSymbolsResponse> context)
        {
            AppPackageSymbolsResponse response = new AppPackageSymbolsResponse();
            try
            {
                ALPackageSymbolsLibrary library = _appPackagesCache.GetSymbols(parameters.path, false);
                if (library != null)
                    response.root = library.Root;
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
