using AnZwDev.ALTools.Server.Contracts;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.ChangeTracking
{
    public class FileRenameNotificationHandler : BaseALNotificationHandler<FilesRenameNotificationRequest>
    {

        public FileRenameNotificationHandler(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "ws/fileRename")
        {
        }

        public override async Task HandleNotification(FilesRenameNotificationRequest parameters, NotificationContext context)
        {
            if (parameters.files != null)
            {
                for (int i = 0; i < parameters.files.Length; i++)
                {
                    this.Server.Workspace.OnFileRename(parameters.files[i].oldPath, parameters.files[i].newPath);
                }
            }
        }

    }
}