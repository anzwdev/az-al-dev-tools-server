using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.VSCodeLangServer.Protocol.MessageProtocol
{

    public abstract class NotificationHandler<TParams> : AbstractMessageHandler
    {

        public NotificationHandler(string name) : base(name)
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

            await HandleNotification(typedParams, notificationContext);
        }

        public abstract Task HandleNotification(TParams parameters, NotificationContext context);

    }

}
