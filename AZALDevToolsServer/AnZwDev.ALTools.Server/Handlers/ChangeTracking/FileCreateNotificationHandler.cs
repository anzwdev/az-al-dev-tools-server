using AnZwDev.ALTools.Server.Contracts;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.ChangeTracking
{
    public class FileCreateNotificationHandler : BaseALNotificationHandler<FilesNotificationRequest>
    {

        public FileCreateNotificationHandler(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "ws/fileCreate")
        {
        }

        public override async Task HandleNotification(FilesNotificationRequest parameters, NotificationContext context)
        {
            this.Server.Workspace.OnFilesCreate(parameters.files);

        }

    }
}