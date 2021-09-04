﻿using AnZwDev.ALTools.Server.Contracts.ChangeTracking;
using AnZwDev.VSCodeLangServer.Protocol.Server;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.ChangeTracking
{
    public class WorkspaceFoldersChangeNotificationHandler: BaseALNotificationHandler<WorkspaceFoldersChangeNotificationRequest>
    {

        public WorkspaceFoldersChangeNotificationHandler(ALDevToolsServer alDevToolsServer, LanguageServerHost languageServerHost) : base(alDevToolsServer, languageServerHost, "ws/workspaceFoldersChange")
        {
        }

#pragma warning disable 1998
        public override async Task HandleNotification(WorkspaceFoldersChangeNotificationRequest parameters, NotificationContext context)
        {
            this.Server.Workspace.UpdateProjects(parameters.added, parameters.removed);
        }
#pragma warning restore 1998

    }
}
