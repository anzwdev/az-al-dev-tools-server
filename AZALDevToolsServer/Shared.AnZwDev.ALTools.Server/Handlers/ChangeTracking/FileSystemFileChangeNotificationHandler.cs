using AnZwDev.ALTools.Server.Contracts.ChangeTracking;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.ChangeTracking
{
    public class FileSystemFileChangeNotificationHandler : BaseALNotificationHandler<FileSystemChangeNotificationRequest>
    {

        public FileSystemFileChangeNotificationHandler(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "ws/fsFileChange")
        {
        }

        public override async Task HandleNotification(FileSystemChangeNotificationRequest parameters, NotificationContext context)
        {
            this.Server.Workspace.OnFileSystemFileChange(parameters.path);

        }

    }
}
