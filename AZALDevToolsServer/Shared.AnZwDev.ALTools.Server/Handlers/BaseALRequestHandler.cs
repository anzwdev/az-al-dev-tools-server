using AnZwDev.ALTools;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers
{
    public class BaseALRequestHandler<TParameters, TResult> : RequestHandler<TParameters, TResult>
    {

        public ALDevToolsServer Server { get; }

        public BaseALRequestHandler(ALDevToolsServer server, string name) : base(name)
        {
            this.Server = server;
        }

    }
}
