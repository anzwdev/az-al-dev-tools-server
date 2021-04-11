using AnZwDev.ALTools.Server.Contracts.ChangeTracking;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.ChangeTracking
{
    public class WorkspaceFoldersChangeNotificationHandler: BaseALNotificationHandler<WorkspaceFoldersChangeNotificationRequest>
    {

        public WorkspaceFoldersChangeNotificationHandler(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "ws/workspaceFoldersChange")
        {
        }

        public override async Task HandleNotification(WorkspaceFoldersChangeNotificationRequest parameters, NotificationContext context)
        {
            this.Server.Workspace.UpdateProjects(parameters.added, parameters.removed);
        }

    }
}
