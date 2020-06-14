using AnZwDev.ALTools;
using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.ALTools.WorkspaceCommands;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using AnZwDev.ALTools.Server.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers
{
    public class WorkspaceCommandRequestHandler : BaseALRequestHandler<WorkspaceCommandRequest, WorkspaceCommandResponse>
    {

        public WorkspaceCommandRequestHandler(ALDevToolsServer server) : base(server, "al/workspacecommand")
        {
        }

        protected override async Task<WorkspaceCommandResponse> HandleMessage(WorkspaceCommandRequest parameters, RequestContext<WorkspaceCommandResponse> context)
        {
            WorkspaceCommandResponse response = new WorkspaceCommandResponse();
            try
            {
                WorkspaceCommandResult commandResult = this.Server.WorkspaceCommandsManager.RunCommand(parameters.command, parameters.source, parameters.path, parameters.parameters);

                response.source = commandResult.Source;
                response.parameters = commandResult.Parameters;
            }
            catch (Exception e)
            {
                response.error = true;
                response.errorMessage = e.Message;
            }
            return response;
        }


    }
}
