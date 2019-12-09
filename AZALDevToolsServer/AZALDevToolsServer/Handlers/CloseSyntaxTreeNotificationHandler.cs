using AnZwDev.ALTools;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using AZALDevToolsServer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZALDevToolsServer.Handlers
{
    public class CloseSyntaxTreeNotificationHandler : BaseALNotificationHandler<CloseSyntaxTreeRequest>
    {

        public CloseSyntaxTreeNotificationHandler(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "al/closesyntaxtree")
        {
        }

        public override async Task HandleNotification(CloseSyntaxTreeRequest parameters, NotificationContext context)
        {
            this.Server.SyntaxTrees.Close(parameters.path);
        }

    }
}
