using AnZwDev.ALTools;
using AnZwDev.ALTools.ALSymbols;
using AnZwDev.VSCodeLangServer.Protocol.Server;
using AnZwDev.ALTools.Server.Handlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.ALTools.Server.Handlers.ChangeTracking;
using AnZwDev.ALTools.Server.Handlers.SymbolsInformation;

namespace AnZwDev.ALTools.Server
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
            this.Dispatcher.RegisterRequestHandler(new ShutdownRequestHandler(this));

            //document symbols
            this.Dispatcher.RegisterRequestHandler(new DocumentSymbolsRequestHandler(this.ALDevToolsServer, this));

            //symbols libraries
            this.Dispatcher.RegisterRequestHandler(new AppPackageSymbolsRequestHandler(this.ALDevToolsServer, this));
            this.Dispatcher.RegisterRequestHandler(new ProjectSymbolsRequestHandler(this.ALDevToolsServer, this));
            this.Dispatcher.RegisterRequestHandler(new LibrarySymbolsDetailsRequestHandler(this.ALDevToolsServer, this));
            this.Dispatcher.RegisterRequestHandler(new GetLibrarySymbolLocationRequestHandler(this.ALDevToolsServer, this));
            this.Dispatcher.RegisterRequestHandler(new CloseSymbolsLibraryNotificationHandler(this.ALDevToolsServer, this));
            
            this.Dispatcher.RegisterRequestHandler(new GetALAppContentRequestHandler(this.ALDevToolsServer, this));

            //syntax tree analyzer
            this.Dispatcher.RegisterRequestHandler(new GetSyntaxTreeRequestHandler(this.ALDevToolsServer, this));
            this.Dispatcher.RegisterRequestHandler(new GetSyntaxTreeSymbolRequestHandler(this.ALDevToolsServer, this));
            this.Dispatcher.RegisterNotificationHandler(new CloseSyntaxTreeNotificationHandler(this.ALDevToolsServer, this));

            this.Dispatcher.RegisterRequestHandler(new GetFullSyntaxTreeRequestHandler(this.ALDevToolsServer, this));

            //code analyzers
            this.Dispatcher.RegisterRequestHandler(new GetCodeAnalyzersRulesRequestHandler(this.ALDevToolsServer, this));

            //code transformations
            this.Dispatcher.RegisterRequestHandler(new WorkspaceCommandRequestHandler(this.ALDevToolsServer, this));

            //symbols information
            this.Dispatcher.RegisterRequestHandler(new GetTablesListRequestHandler(this.ALDevToolsServer, this));
            this.Dispatcher.RegisterRequestHandler(new GetTableFieldsListRequestHandler(this.ALDevToolsServer, this));
            this.Dispatcher.RegisterRequestHandler(new GetCodeunitsListRequestHandler(this.ALDevToolsServer, this));
            this.Dispatcher.RegisterRequestHandler(new GetInterfacesListRequestHandler(this.ALDevToolsServer, this));
            this.Dispatcher.RegisterRequestHandler(new GetEnumsListRequestHandler(this.ALDevToolsServer, this));
            this.Dispatcher.RegisterRequestHandler(new GetReportsListRequestHandler(this.ALDevToolsServer, this));
            this.Dispatcher.RegisterRequestHandler(new GetQueriesListRequestHandler(this.ALDevToolsServer, this));
            this.Dispatcher.RegisterRequestHandler(new GetXmlPortsListRequestHandler(this.ALDevToolsServer, this));
            this.Dispatcher.RegisterRequestHandler(new GetPageDetailsRequestHandler(this.ALDevToolsServer, this));
            this.Dispatcher.RegisterRequestHandler(new GetXmlPortTableElementDetailsRequestHandler(this.ALDevToolsServer, this));
            this.Dispatcher.RegisterRequestHandler(new GetReportDataItemDetailsRequestHandler(this.ALDevToolsServer, this));
            this.Dispatcher.RegisterRequestHandler(new GetQueryDataItemDetailsRequestHandler(this.ALDevToolsServer, this));

            //standard notification handlers
            this.Dispatcher.RegisterNotificationHandler(new ExitNotificationHandler(this));

            //document tracking notification handlers
            this.Dispatcher.RegisterNotificationHandler(new WorkspaceFoldersChangeNotificationHandler(this.ALDevToolsServer, this));
            this.Dispatcher.RegisterNotificationHandler(new DocumentOpenNotificationHandler(this.ALDevToolsServer, this));
            this.Dispatcher.RegisterRequestHandler(new DocumentContentChangeRequestHandler(this.ALDevToolsServer, this));
            this.Dispatcher.RegisterNotificationHandler(new DocumentCloseNotificationHandler(this.ALDevToolsServer, this));

            //this file change handlers are not used by vs code:
            //this.Dispatcher.RegisterNotificationHandler(new DocumentSaveNotificationHandler(this.ALDevToolsServer));
            //this.Dispatcher.RegisterNotificationHandler(new FileCreateNotificationHandler(this.ALDevToolsServer));
            //this.Dispatcher.RegisterNotificationHandler(new FileDeleteNotificationHandler(this.ALDevToolsServer));
            //this.Dispatcher.RegisterNotificationHandler(new FileRenameNotificationHandler(this.ALDevToolsServer));

            this.Dispatcher.RegisterNotificationHandler(new FileSystemFileCreateNotificationHandler(this.ALDevToolsServer, this));
            this.Dispatcher.RegisterNotificationHandler(new FileSystemFileDeleteNotificationHandler(this.ALDevToolsServer, this));
            this.Dispatcher.RegisterNotificationHandler(new FileSystemFileChangeNotificationHandler(this.ALDevToolsServer, this));

            //other message handlers
            this.Dispatcher.RegisterRequestHandler(new GetProjectSettingsRequestHandler(this.ALDevToolsServer, this));
        }

    }
}
