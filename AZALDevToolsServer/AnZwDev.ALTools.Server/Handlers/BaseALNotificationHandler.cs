using AnZwDev.ALTools;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers
{
    public abstract class BaseALNotificationHandler<T> : NotificationHandler<T>
    {

        public ALDevToolsServer Server { get; }

        public BaseALNotificationHandler(ALDevToolsServer alDevToolsServer, string name) : base(name)
        {
            this.Server = alDevToolsServer;
        }

    }
}
