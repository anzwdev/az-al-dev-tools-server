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
    public class ALDevToolsServer : LanguageServerHost
    {

        public ALExtensionProxy ALExtensionProxy { get; set; }
        public string ExtensionBinPath { get; set; }

        public ALDevToolsServer(string extensionPath)
        {
            this.ExtensionBinPath = Path.Combine(extensionPath, "bin");
            this.ALExtensionProxy = new ALExtensionProxy();
            this.ALExtensionProxy.Load(this.ExtensionBinPath);
        }

        protected override void InitializeMessageHandlers()
        {
            base.InitializeMessageHandlers();
            //request handlers
            this.Dispatcher.RegisterRequestHandler(new ShutdownRequestHandler());
            this.Dispatcher.RegisterRequestHandler(new DocumentSymbolsRequestHandler(this));
            this.Dispatcher.RegisterRequestHandler(new AppPackageSymbolsRequestHandler(this));
            //notification handlers
            this.Dispatcher.RegisterNotificationHandler(new ExitNotificationHandler(this));
        }

    }
}
