/****************************************************************
 *                                                              *
 * Legacy version of the library maintained to support Nav 2018 *
 *                                                              *
 ****************************************************************/
using AnZwDev.ALTools.Nav2018;
using AnZwDev.ALTools.Nav2018.CodeTransformations;
using AnZwDev.ALTools.Nav2018.WorkspaceCommands;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using AnZwDev.ALTools.Server.Nav2018.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Nav2018.Handlers
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
