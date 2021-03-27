using AnZwDev.ALTools.Server.Contracts.SymbolsInformation;
using AnZwDev.ALTools.Workspace;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.SymbolsInformation
{

    public class GetXmlPortTableElementDetailsRequestHandler : BaseALRequestHandler<GetXmlPortTableElementDetailsRequest, GetXmlPortTableElementDetailsResponse>
    {

        public GetXmlPortTableElementDetailsRequestHandler(ALDevToolsServer server) : base(server, "al/getxmlporttableelementdetails")
        {
        }

        protected override async Task<GetXmlPortTableElementDetailsResponse> HandleMessage(GetXmlPortTableElementDetailsRequest parameters, RequestContext<GetXmlPortTableElementDetailsResponse> context)
        {
            GetXmlPortTableElementDetailsResponse response = new GetXmlPortTableElementDetailsResponse();

            try
            {

                ALProject project = this.Server.Workspace.FindProject(parameters.path, true);
                if (project != null)
                {
                    XmlPortInformationProvider provider = new XmlPortInformationProvider();
                    response.symbol = provider.GetXmlPortTableElementDetails(project, parameters.xmlPortName, parameters.name, parameters.getXmlPortTableFields, parameters.getAvailableFields);

                    SymbolInformationComparer comparer = new SymbolInformationComparer();
                    if (response.symbol.AvailableTableFields != null)
                        response.symbol.AvailableTableFields.Sort(comparer);
                    if (response.symbol.XmlPortTableFields != null)
                        response.symbol.XmlPortTableFields.Sort(comparer);
                }
            }
            catch (Exception e)
            {
                response.symbol = new XmlPortTableElementInformation();
                response.symbol.Name = e.Message;
            }

            return response;

        }

    }

}
