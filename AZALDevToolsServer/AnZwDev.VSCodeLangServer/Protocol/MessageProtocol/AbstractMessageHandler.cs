using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.VSCodeLangServer.Protocol.MessageProtocol
{
    public abstract class AbstractMessageHandler
    {

        public string Name { get; }

        public AbstractMessageHandler(string name)
        {
            this.Name = name;
        }

        public abstract Task HandleRawMessage(Message message, MessageWriter messageWriter);

    }
}
