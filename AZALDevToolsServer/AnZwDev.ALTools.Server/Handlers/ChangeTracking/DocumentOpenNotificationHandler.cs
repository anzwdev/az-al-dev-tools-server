﻿using AnZwDev.ALTools.Server.Contracts;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.ChangeTracking
{
    public class DocumentOpenNotificationHandler : BaseALNotificationHandler<DocumentChangeNotificationRequest>
    {

        public DocumentOpenNotificationHandler(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "ws/documentOpen")
        {
        }

        public override async Task HandleNotification(DocumentChangeNotificationRequest parameters, NotificationContext context)
        {
            this.Server.Workspace.OnDocumentOpen(parameters.path);
        }

    }
}
