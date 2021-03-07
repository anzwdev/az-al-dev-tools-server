﻿using AnZwDev.ALTools.Server.Contracts;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.ChangeTracking
{
    public class FileDeleteNotificationHandler : BaseALNotificationHandler<FilesNotificationRequest>
    {

        public FileDeleteNotificationHandler(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "ws/fileDelete")
        {
        }

        public override async Task HandleNotification(FilesNotificationRequest parameters, NotificationContext context)
        {
            this.Server.Workspace.OnFilesDelete(parameters.files);
        }

    }
}