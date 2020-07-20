/****************************************************************
 *                                                              *
 * Legacy version of the library maintained to support Nav 2018 *
 *                                                              *
 ****************************************************************/
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Nav2018.Handlers
{

    public class ShutdownRequestHandler : RequestHandler<object, object>
    {

        public ShutdownRequestHandler() : base("shutdown")
        {
        }

        protected override async Task<object> HandleMessage(object parameters, RequestContext<object> context)
        {
            return new object();
        }
    }

}
