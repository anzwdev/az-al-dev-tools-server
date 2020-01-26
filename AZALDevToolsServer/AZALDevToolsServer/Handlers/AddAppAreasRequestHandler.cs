using AnZwDev.ALTools;
using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using AZALDevToolsServer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AZALDevToolsServer.Handlers
{
    public class AddAppAreasRequestHandler : BaseALRequestHandler<AddAppAreasRequest, AddAppAreasResponse>
    {

        public AddAppAreasRequestHandler(ALDevToolsServer server) : base(server, "al/addappareas")
        {
        }

        protected override async Task<AddAppAreasResponse> HandleMessage(AddAppAreasRequest parameters, RequestContext<AddAppAreasResponse> context)
        {
            AddAppAreasResponse response = new AddAppAreasResponse();
            try
            {
                AppAreaManager appAreaManager = new AppAreaManager(this.Server.ALExtensionProxy);
                int noOfFiles = 0;
                int noOfAppAreas = 0;
                if (String.IsNullOrWhiteSpace(parameters.appAreaName))
                    parameters.appAreaName = "All";

                if (!String.IsNullOrWhiteSpace(parameters.source))
                {
                    response.source = appAreaManager.AddMissingAppAreas(parameters.source, parameters.appAreaName, out noOfAppAreas);
                    if (noOfAppAreas > 0)
                        noOfFiles++;
                }
                else if (!String.IsNullOrWhiteSpace(parameters.path))
                {
                    ProcessDirectory(appAreaManager, parameters.path, parameters.appAreaName, ref noOfFiles, ref noOfAppAreas);
                }
                response.noOfFiles = noOfFiles;
                response.noOfAppAreas = noOfAppAreas;
            }
            catch (Exception e)
            {
                response.error = true;
                response.errorMessage = e.Message;
            }
            return response;
        }

        private bool ProcessFile(AppAreaManager appAreaManager, string filePath, string appAreaName, out int noOfAppAreas)
        {
            noOfAppAreas = 0;
            try
            {
                string source = System.IO.File.ReadAllText(filePath);
                string newSource = appAreaManager.AddMissingAppAreas(source, appAreaName, out noOfAppAreas);
                if ((newSource != source) && (noOfAppAreas > 0) && (!String.IsNullOrWhiteSpace(newSource)))
                    System.IO.File.WriteAllText(filePath, newSource);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private void ProcessDirectory(dynamic syntaxRewriter, string directoryPath, string appAreaName, ref int noOfFiles, ref int noOfAppAreas)
        {
            string[] filePathsList = System.IO.Directory.GetFiles(directoryPath, "*.al", System.IO.SearchOption.AllDirectories);
            for (int i=0; i<filePathsList.Length; i++)
            {
                int noOfFileAppAreas;
                if (ProcessFile(syntaxRewriter, filePathsList[i], appAreaName, out noOfFileAppAreas))
                {
                    if (noOfFileAppAreas > 0)
                    {
                        noOfAppAreas += noOfFileAppAreas;
                        noOfFiles++;
                    }
                }
            }
        }

    }
}
