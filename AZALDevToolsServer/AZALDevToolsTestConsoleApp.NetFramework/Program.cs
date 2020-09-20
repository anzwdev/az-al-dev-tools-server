﻿using AnZwDev.ALTools;
using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.ALSymbols.SymbolReaders;
using AnZwDev.ALTools.CodeAnalysis;
using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.ALTools.Server;
using AnZwDev.ALTools.TypeInformation;
using AnZwDev.ALTools.WorkspaceCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZALDevToolsTestConsoleApp.NetFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            //string extensionPath = "C:\\Users\\azwie\\.vscode\\extensions\\ms-dynamics-smb.al-5.0.329509";
            //string extensionPath = "C:\\Users\\azwie\\Downloads\\VSCode-win32-x64-1.45.1\\data\\extensions\\microsoft.al-0.13.82793";
            string extensionPath = "C:\\Projects\\MicrosoftALVersions\\LatestBC";

            ALDevToolsServerHost host = new ALDevToolsServerHost(extensionPath);
            host.Initialize();

            ALDevToolsServer alDevToolsServer = new ALDevToolsServer(extensionPath);

            //string filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\small\\Pag50000.MySmallTableList.al";
            string filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\SmallBC16\\MyPage.al";
            ALSymbolInfoSyntaxTreeReader syntaxTreeReader = new ALSymbolInfoSyntaxTreeReader(true);
            ALSymbolInformation symbols = syntaxTreeReader.ProcessSourceFile(filePath);

            ALFullSyntaxTree syntaxTree = new ALFullSyntaxTree();
            syntaxTree.Load("", filePath);

            CodeAnalyzersLibrariesCollection caLibCol = new CodeAnalyzersLibrariesCollection(alDevToolsServer);
            CodeAnalyzersLibrary caLib = caLibCol.GetCodeAnalyzersLibrary("${CodeCop}");

            filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\SmallBC16\\AnZwDev_Small but great AZ AL Extension_1.0.0.0.app";
            ALSymbolInfoPackageReader packageReader = new ALSymbolInfoPackageReader();
            packageReader.ReadAppPackage(filePath);

            Console.WriteLine("Done");
        }
    }
}
