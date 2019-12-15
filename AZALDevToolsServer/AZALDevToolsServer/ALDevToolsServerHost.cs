using AnZwDev.ALTools;
using AnZwDev.ALTools.ALProxy;
using AnZwDev.ALTools.ALSymbols;
using AnZwDev.VSCodeLangServer.Protocol.Server;
using AZALDevToolsServer.Handlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZALDevToolsServer
{
    public class ALDevToolsServerHost : LanguageServerHost
    {

        public ALDevToolsServer ALDevToolsServer { get; }

        public ALDevToolsServerHost(string extensionPath)
        {
            this.ALDevToolsServer = new ALDevToolsServer(extensionPath);
        }

        protected override void InitializeMessageHandlers()
        {
            base.InitializeMessageHandlers();
            //request handlers
            this.Dispatcher.RegisterRequestHandler(new ShutdownRequestHandler());

            //document symbols
            this.Dispatcher.RegisterRequestHandler(new DocumentSymbolsRequestHandler(this.ALDevToolsServer));

            //symbols libraries
            this.Dispatcher.RegisterRequestHandler(new AppPackageSymbolsRequestHandler(this.ALDevToolsServer));
            this.Dispatcher.RegisterRequestHandler(new ProjectSymbolsRequestHandler(this.ALDevToolsServer));
            this.Dispatcher.RegisterRequestHandler(new LibrarySymbolsDetailsRequestHandler(this.ALDevToolsServer));
            this.Dispatcher.RegisterRequestHandler(new CloseSymbolsLibraryNotificationHandler(this.ALDevToolsServer));

            //syntax tree analyzer
            this.Dispatcher.RegisterRequestHandler(new GetSyntaxTreeRequestHandler(this.ALDevToolsServer));
            this.Dispatcher.RegisterRequestHandler(new GetSyntaxTreeSymbolRequestHandler(this.ALDevToolsServer));
            this.Dispatcher.RegisterNotificationHandler(new CloseSyntaxTreeNotificationHandler(this.ALDevToolsServer));

            //code analyzers
            this.Dispatcher.RegisterRequestHandler(new GetCodeAnalyzersRulesRequestHandler(this.ALDevToolsServer));

            //standard notification handlers
            this.Dispatcher.RegisterNotificationHandler(new ExitNotificationHandler(this));
        }

    }
}
