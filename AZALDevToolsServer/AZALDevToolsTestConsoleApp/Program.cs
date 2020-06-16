using AnZwDev.ALTools;
using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.ALSymbols.SymbolReaders;
using AnZwDev.ALTools.CodeTransformations;
using System;

namespace AZALDevToolsTestConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ALDevToolsServer alDevToolsServer = new ALDevToolsServer("C:\\Users\\azwie\\.vscode\\extensions\\ms-dynamics-smb.al-5.0.280447");

            string filePath = "";
            ALSyntaxTreeSymbolsReader syntaxTreeReader = new ALSyntaxTreeSymbolsReader();
            ALSyntaxTreeSymbol symbol = syntaxTreeReader.ProcessSourceFile(filePath);           
        }
    }
}
