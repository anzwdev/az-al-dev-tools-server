/****************************************************************
 *                                                              *
 * Legacy version of the library maintained to support Nav 2018 *
 *                                                              *
 ****************************************************************/
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using AnZwDev.VSCodeLangServer.Protocol.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Nav2018.Handlers
{
    public class ExitNotificationHandler : NotificationHandler<object>
    {

        public LanguageServerHost LanguageServer { get; }

        public ExitNotificationHandler(LanguageServerHost languageServer) : base("exit")
        {
            this.LanguageServer = languageServer;
        }

        public override async Task HandleNotification(object parameters, NotificationContext context)
        {
            this.LanguageServer.Stop();
        }

    }
}
