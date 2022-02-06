﻿using AnZwDev.ALTools;
using AnZwDev.ALTools.ALLanguageInformation;
using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.ALSymbolReferences.Compiler;
using AnZwDev.ALTools.ALSymbolReferences.Serialization;
using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.ALSymbols.SymbolReaders;
using AnZwDev.ALTools.CodeAnalysis;
using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.ALTools.Core;
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

            ALDevToolsServerHost host = new(extensionPath);
            host.Initialize();

            ALDevToolsServer alDevToolsServer = new(extensionPath);

            //get list of errors and warnings
            //CompilerCodeAnalyzersLibrary lib = new CompilerCodeAnalyzersLibrary("Compiler");
            CodeAnalyzersLibrary lib = alDevToolsServer.CodeAnalyzersLibraries.GetCodeAnalyzersLibrary("Compiler");

            ImageInformationProvider provider = new();
            List<ImageInformation> images = provider.GetActionImages();


            //string filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\small\\Pag50000.MySmallTableList.al";
            string filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\SmallBC18\\MyTableExt.al";
            ALSymbolInfoSyntaxTreeReader syntaxTreeReader = new(true);
            ALSymbol symbols = syntaxTreeReader.ProcessSourceFile(filePath);

            ALFullSyntaxTree syntaxTree = new();
            syntaxTree.Load("", filePath);


            filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\IssueTests\\Attendee.Table.al";
            filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\IssueTests\\Attendee_Original.Table.al";
            ALSymbolInfoSyntaxTreeReader s = new(true);
            symbols = s.ProcessSourceFile(filePath);


            //test project
            string[] projects =
            {
                //"C:\\Projects\\Sandboxes\\al-test-projects\\SmallBC18"
                "C:\\Projects\\Sandboxes\\al-test-projects\\BC184TestProject"
            };
            host.ALDevToolsServer.Workspace.LoadProjects(projects);
            host.ALDevToolsServer.Workspace.ResolveDependencies();

            //filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\SmallBC18\\Pag50104.MyPrefixMyPageCard.al";
            //filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\SmallBC18\\permissionset-Ext50101.MyPermSetExt03.al";
            //filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\BC184TestProject\\Pag50101.asUFDGHQEUGF.al";
            //filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\BC184TestProject\\net.al";
            //filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\BC184TestProject\\ObjectIdTestPage.al";
            //filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\BC184TestProject\\FunctionsTest.al";
            //filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\BC184TestProject\\Pag50103.MyTestPage2.al";
            //filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\BC184TestProject\\Rep50100.MyReport.al";
            //filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\BC184TestProject\\MyTestCU.al";
            filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\BC184TestProject\\Cod50100.MyCodeunit.al";
            
            string content = FileUtils.SafeReadAllText(filePath);
            Dictionary<string, string> pm = new();
            pm.Add("sourceFilePath", filePath);
            pm.Add("skipFormatting", "true");

            pm.Add("removeGlobalVariables", "true");
            pm.Add("removeLocalVariables", "true");
            pm.Add("removeLocalMethodParameters", "true");

            pm.Add("dependencyName0", "Microsoft - System");
            pm.Add("dependencyName1", "Microsoft - Application");
            pm.Add("dependencyName2", "Microsoft - System Application");
            pm.Add("dependencyName3", "Microsoft - Base Application");


            //WorkspaceCommandResult o = host.ALDevToolsServer.WorkspaceCommandsManager.RunCommand("removeWith", content, projectPath, null, pm);
            //WorkspaceCommandResult o = host.ALDevToolsServer.WorkspaceCommandsManager.RunCommand("addAllObjectsPermissions", content, projectPath, null, pm);
            //WorkspaceCommandResult o = host.ALDevToolsServer.WorkspaceCommandsManager.RunCommand("fixIdentifiersCase", content, projects[0], null, pm);
            //WorkspaceCommandResult o = host.ALDevToolsServer.WorkspaceCommandsManager.RunCommand("removeUnusedVariables", content, projects[0], filePath, null, pm);
            //WorkspaceCommandResult o = host.ALDevToolsServer.WorkspaceCommandsManager.RunCommand("convertObjectIdsToNames", content, projects[0], null, pm);
            //WorkspaceCommandResult o = host.ALDevToolsServer.WorkspaceCommandsManager.RunCommand("addParentheses", content, projects[0], filePath, null, pm);
            //WorkspaceCommandResult o = host.ALDevToolsServer.WorkspaceCommandsManager.RunCommand("sortVariables", content, projects[0], filePath, null, pm);
            WorkspaceCommandResult o = host.ALDevToolsServer.WorkspaceCommandsManager.RunCommand("removeBeginEnd", content, projects[0], filePath, null, pm);
            //WorkspaceCommandResult o = host.ALDevToolsServer.WorkspaceCommandsManager.RunCommand("addToolTips", content, projects[0], filePath, null, pm);
            //WorkspaceCommandResult o = host.ALDevToolsServer.WorkspaceCommandsManager.RunCommand("refreshToolTips", content, projects[0], filePath, null, pm);
            //WorkspaceCommandResult o = host.ALDevToolsServer.WorkspaceCommandsManager.RunCommand("sortProperties", content, projects[0], filePath, null, pm);

            ALProject project = host.ALDevToolsServer.Workspace.Projects[0];

            
            PageInformationProvider pageInformationProvider = new PageInformationProvider();
            List<string> toolTipsList = pageInformationProvider.GetPageFieldAvailableToolTips(project, "Page", "MyTestPage", "", "Rec.\"No.\"");

            PageInformation pageInformation = pageInformationProvider.GetPageDetails(project, "wqefewf", true, true, true, null);

            ObjectIdInformationProvider objectIdInformationProvider = new();
            long id = objectIdInformationProvider.GetNextObjectId(project, "Page");

            TableInformationProvider tableInformationProvider = new();
            List<TableFieldInformaton> fields = tableInformationProvider.GetTableFields(project, "Purchase Line", false, false, true, true, true, false, null);
            List<TableFieldInformaton> fields2 = fields.Where(p => (p.Name.StartsWith("Description"))).ToList();

            ReportInformationProvider reportInformationProvider = new();
            ReportInformation reportInformation = reportInformationProvider.GetFullReportInformation(project, "Sales Order");


            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
