﻿/****************************************************************
 *                                                              *
 * Legacy version of the library maintained to support Nav 2018 *
 *                                                              *
 ****************************************************************/
using AnZwDev.ALTools.Nav2018;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using AnZwDev.ALTools.Server.Nav2018.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Nav2018.Handlers
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
