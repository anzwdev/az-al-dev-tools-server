/****************************************************************
 *                                                              *
 * Legacy version of the library maintained to support Nav 2018 *
 *                                                              *
 ****************************************************************/
using AnZwDev.ALTools.Nav2018;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Nav2018.Handlers
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
