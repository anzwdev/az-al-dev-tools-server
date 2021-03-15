using AnZwDev.ALTools;
using AnZwDev.ALTools.ALSymbols;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using AnZwDev.ALTools.Server.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.ALTools.Workspace;

namespace AnZwDev.ALTools.Server.Handlers
{
    public class ProjectSymbolsRequestHandler : BaseALRequestHandler<ProjectSymbolsRequest, ProjectSymbolsResponse>
    {

        public ProjectSymbolsRequestHandler(ALDevToolsServer server) : base(server, "al/projectsymbols")
        {
        }

        protected override async Task<ProjectSymbolsResponse> HandleMessage(ProjectSymbolsRequest parameters, RequestContext<ProjectSymbolsResponse> context)
        {
            ProjectSymbolsResponse response = new ProjectSymbolsResponse();
            try
            {
                //find project
                ALProject project = this.Server.Workspace.FindProject(parameters.projectPath);
                if (project != null)
                {
                    ALSymbolsLibrary library = project.CreateSymbolsLibrary(parameters.includeDependencies);
                    response.libraryId = this.Server.SymbolsLibraries.AddLibrary(library);
                    response.root = library.GetObjectsTree();
                }
            }
            catch (Exception e)
            {
                response.root = new ALSymbolInformation();
                response.root.fullName = e.Message;
            }

            return response;
        }

    }
}
