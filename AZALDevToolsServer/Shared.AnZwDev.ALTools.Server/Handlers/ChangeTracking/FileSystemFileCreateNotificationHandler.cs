﻿using AnZwDev.ALTools.Server.Contracts.ChangeTracking;
using AnZwDev.VSCodeLangServer.Protocol.Server;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace AnZwDev.ALTools.Server.Handlers.ChangeTracking
{
    public class FileSystemFileCreateNotificationHandler : BaseALNotificationHandler<FileSystemChangeNotificationRequest>
    {

        public FileSystemFileCreateNotificationHandler(ALDevToolsServer alDevToolsServer, LanguageServerHost languageServerHost) : base(alDevToolsServer, languageServerHost, "ws/fsFileCreate")
        {
        }

        public override async Task HandleNotification(FileSystemChangeNotificationRequest parameters, NotificationContext context)
        {
            this.Server.Workspace.OnFileSystemFileCreate(parameters.path);

        }

    }
}
