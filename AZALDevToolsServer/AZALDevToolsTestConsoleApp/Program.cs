using AnZwDev.ALTools;
using AnZwDev.ALTools.ALLanguageInformation;
using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.ALSymbolReferences.Compiler;
using AnZwDev.ALTools.ALSymbolReferences.Serialization;
using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.ALSymbols.SymbolReaders;
using AnZwDev.ALTools.CodeAnalysis;
using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.ALTools.Server;
using AnZwDev.ALTools.Workspace;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using AnZwDev.ALTools.WorkspaceCommands;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZALDevToolsTestConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string extensionPath = "C:\\Projects\\MicrosoftALVersions\\LatestBC";

            ALDevToolsServerHost host = new ALDevToolsServerHost(extensionPath);
            host.Initialize();

            ALDevToolsServer alDevToolsServer = new ALDevToolsServer(extensionPath);

            //get list of errors and warnings
            //CompilerCodeAnalyzersLibrary lib = new CompilerCodeAnalyzersLibrary("Compiler");
            CodeAnalyzersLibrary lib = alDevToolsServer.CodeAnalyzersLibraries.GetCodeAnalyzersLibrary("Compiler");

            ImageInformationProvider provider = new ImageInformationProvider();
            List<ImageInformation> images = provider.GetActionImages();


            //string filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\small\\Pag50000.MySmallTableList.al";
            string filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\SmallBC18\\MyTableExt.al";
            ALSymbolInfoSyntaxTreeReader syntaxTreeReader = new ALSymbolInfoSyntaxTreeReader(true);
            ALSymbol symbols = syntaxTreeReader.ProcessSourceFile(filePath);

            ALFullSyntaxTree syntaxTree = new ALFullSyntaxTree();
            syntaxTree.Load("", filePath);

            //filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\SmallBC18\\Pag50104.MyPrefixMyPageCard.al";
            filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\SmallBC18\\fromattest2.al";
            string content = System.IO.File.ReadAllText(filePath);
            Dictionary<string, string> pm = new Dictionary<string, string>();
            pm.Add("sourceFilePath", filePath);
            string projectPath = "C:\\Projects\\Sandboxes\\al-test-projects\\SmallBC18";
            //WorkspaceCommandResult o = host.ALDevToolsServer.WorkspaceCommandsManager.RunCommand("removeWith", content, projectPath, null, pm);
            WorkspaceCommandResult o = host.ALDevToolsServer.WorkspaceCommandsManager.RunCommand("sortProperties", content, projectPath, null, pm);

            //test project
            string[] projects =
            {
                "C:\\Projects\\Sandboxes\\al-test-projects\\SmallBC18"
            };

            ALWorkspace workspace = new ALWorkspace();
            workspace.LoadProjects(projects);
            workspace.ResolveDependencies();
            ALProject project = workspace.Projects[0];

            TableInformationProvider tableInformationProvider = new TableInformationProvider();
            List<TableFieldInformaton> fields = tableInformationProvider.GetTableFields(project, "Purchase Line", false, false);
            List<TableFieldInformaton> fields2 = fields.Where(p => (p.Name.StartsWith("Description"))).ToList();

            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
