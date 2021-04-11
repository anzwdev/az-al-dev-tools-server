using AnZwDev.ALTools.Server.Contracts.ChangeTracking;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace AnZwDev.ALTools.Server.Handlers.ChangeTracking
{
    public class FileSystemFileDeleteNotificationHandler : BaseALNotificationHandler<FileSystemChangeNotificationRequest>
    {

        public FileSystemFileDeleteNotificationHandler(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "ws/fsFileDelete")
        {
        }

        public override async Task HandleNotification(FileSystemChangeNotificationRequest parameters, NotificationContext context)
        {
            this.Server.Workspace.OnFileSystemFileDelete(parameters.path);

        }

    }
}
