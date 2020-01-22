using AnZwDev.ALTools;
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
                Type appAreaProcessorType = this.Server.DynamicTypesCache.GetTypeFromResource(
                    "AnZwDev.ALTools.DynamicTypes.Resources.AppAreaSyntaxRewriter.txt",
                    "AnZwDev.ALTools.DynamicTypes.ResourcesAppAreaSyntaxRewriter");

                Type[] paramTypes = { typeof(bool) };
                Object[] paramValues = { false };
                ConstructorInfo constructorInfo = appAreaProcessorType.GetConstructor(paramTypes);
                dynamic syntaxRewriter = constructorInfo.Invoke(paramValues);

                if (!String.IsNullOrWhiteSpace(parameters.source))
                {
                    response.source = ProcessSourceCode(syntaxRewriter, parameters.source);
                }
                else if (!String.IsNullOrWhiteSpace(parameters.path))
                {
                    int noOfFiles = 0;
                    int noOfAppAreas = 0;
                    ProcessDirectory(syntaxRewriter, parameters.path, ref noOfFiles, ref noOfAppAreas);
                    response.noOfFiles = noOfFiles;
                    response.noOfAppAreas = noOfAppAreas;
                }
            }
            catch (Exception e)
            {
                response.error = true;
                response.errorMessage = e.Message;
            }
            return response;
        }

        private string ProcessSourceCode(dynamic syntaxRewriter, string source)
        {
            dynamic syntaxTree = this.Server.ALExtensionProxy.GetSyntaxTree(source);
            dynamic rootNode = syntaxTree.GetRoot();
            dynamic newRoot = syntaxRewriter.Visit(rootNode);
            if ((newRoot != rootNode) && (newRoot != null))
                return newRoot.ToFullString();
            return "";
        }

        private bool ProcessFile(dynamic syntaxRewriter, string filePath)
        {
            try
            {
                string source = System.IO.File.ReadAllText(filePath);
                string newSource = ProcessSourceCode(syntaxRewriter, source);
                if (newSource != source)
                    System.IO.File.WriteAllText(filePath, newSource);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private void ProcessDirectory(dynamic syntaxRewriter, string directoryPath, ref int noOfFiles, ref int noOfAppAreas)
        {
            string[] filePathsList = System.IO.Directory.GetFiles(directoryPath, "*.al", System.IO.SearchOption.AllDirectories);
            for (int i=0; i<filePathsList.Length; i++)
            {
                if (ProcessFile(syntaxRewriter, filePathsList[i]))
                    noOfFiles++;
            }
        }

    }
}
