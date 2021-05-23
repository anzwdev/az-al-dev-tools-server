using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.VSCodeLangServer.Protocol.Server;

namespace AnZwDev.VSCodeLangServer.Protocol.MessageProtocol
{

    public abstract class NotificationHandler<TParams> : AbstractMessageHandler
    {

        public NotificationHandler(LanguageServerHost languageServerHost, string name) : base(languageServerHost, name)
        {
        }

        public override async Task HandleRawMessage(Message notificationMessage, MessageWriter messageWriter)
        {
            NotificationContext notificationContext = new NotificationContext(messageWriter);

            TParams typedParams = default(TParams);
            if (notificationMessage.Contents != null)
            {
                // TODO: Catch parse errors!
                typedParams = notificationMessage.Contents.ToObject<TParams>();
            }

            try
            {
                await HandleNotification(typedParams, notificationContext);
            }
            catch (Exception e)
            {
            }
        }

        public abstract Task HandleNotification(TParams parameters, NotificationContext context);

    }

}
