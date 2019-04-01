using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol.Channel;
using AnZwDev.VSCodeLangServer.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.VSCodeLangServer.Protocol.Server
{
    public class LanguageServerHost
    {

        public MessageDispatcher Dispatcher { get; protected set; }
        public ChannelBase Channel { get; protected set; }
        public ProtocolEndpoint Endpoint { get; protected set; }
        public ILogger Logger { get; protected set; }

        public LanguageServerHost()
        {
        }

        public virtual void Initialize()
        {
            //initialize logger
            this.Logger = CreateLogger();
            //initialize protocol dispatcher and endpoint
            this.Dispatcher = new MessageDispatcher(this.Logger);
            this.Channel = new StdioServerChannel(this.Logger);
            this.Endpoint = new ProtocolEndpoint(this.Channel, this.Dispatcher, this.Logger);
            //initialize message handlers
            InitializeMessageHandlers();
        }

        protected virtual ILogger CreateLogger()
        {
            string logFilePath = System.IO.Path.Combine(this.GetType().Assembly.Location, "log.txt");
            Logging.Builder builder = Logging.CreateLogger();
            builder.AddLogFile(logFilePath, LogLevel.Diagnostic);
            return builder.Build();
        }

        protected virtual void InitializeMessageHandlers()
        {
        }

        public void Run()
        {
            this.Channel.Start(MessageProtocolType.LanguageServer);
            this.Endpoint.Start();
            this.Endpoint.WaitForExit();
        }

        public void Stop()
        {
            this.Endpoint.Stop();
        }

    }
}
