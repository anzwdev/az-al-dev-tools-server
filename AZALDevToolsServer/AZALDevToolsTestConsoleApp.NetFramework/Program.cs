using AnZwDev.ALTools;
using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.ALSymbols.SymbolReaders;
using AnZwDev.ALTools.Server;
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
            //string extensionPath = "C:\\Users\\azwie\\.vscode\\extensions\\ms-dynamics-smb.al-5.0.280447";
            string extensionPath = "C:\\Users\\azwie\\Downloads\\VSCode-win32-x64-1.45.1\\data\\extensions\\microsoft.al-0.13.82793";

            ALDevToolsServerHost host = new ALDevToolsServerHost(extensionPath);
            host.Initialize();

            ALDevToolsServer alDevToolsServer = new ALDevToolsServer(extensionPath);

            string filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\small\\Pag50000.MySmallTableList.al";
            ALSyntaxTreeSymbolsReader syntaxTreeReader = new ALSyntaxTreeSymbolsReader();
            ALSyntaxTreeSymbol symbol = syntaxTreeReader.ProcessSourceFile(filePath);

            Console.WriteLine("Done");
        }
    }
}
