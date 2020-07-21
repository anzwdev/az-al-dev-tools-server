/****************************************************************
 *                                                              *
 * Legacy version of the library maintained to support Nav 2018 *
 *                                                              *
 ****************************************************************/
using AnZwDev.ALTools.Nav2018;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using AnZwDev.ALTools.Server.Nav2018.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Nav2018.Handlers
{
    public class CloseSymbolsLibraryNotificationHandler : BaseALNotificationHandler<CloseSymbolsLibraryRequest>
    {

        public CloseSymbolsLibraryNotificationHandler(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "al/closesymbolslibrary")
        {
        }

        public override async Task HandleNotification(CloseSymbolsLibraryRequest parameters, NotificationContext context)
        {
            this.Server.SymbolsLibraries.RemoveLibrary(parameters.libraryId);
        }
    }
}
