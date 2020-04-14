using AnZwDev.ALTools;
using AnZwDev.ALTools.ALProxy;
using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.ALSymbols.ALAppPackages;
using AnZwDev.ALTools.ALSymbols.SymbolReaders;
using AnZwDev.ALTools.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZALDevToolsTestConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.

            ALDevToolsServer server = new ALDevToolsServer("C:\\Users\\azwie\\.vscode\\extensions\\ms-dynamics-smb.al-5.0.254558");

            ALExtensionProxy alExtensionProxy = server.ALExtensionProxy;
            //alExtensionProxy.Load();

            ALPackageSymbolsLibrary lib = new ALPackageSymbolsLibrary(alExtensionProxy,
                "C:\\Projects\\Sandboxes\\al-test-projects\\SmallBC16\\.alpackages\\Microsoft_Base Application_16.0.11240.12076.app");
            lib.Load(false);

            //ALPackageSymbolsLibrary lib = new ALPackageSymbolsLibrary(alExtensionProxy,
            //    "C:\\Projects\\Sandboxes\\al-test-projects\\small\\.alpackages\\Microsoft_System_14.0.29487.0.app");
            //lib.Load(false);


            CodeAnalyzersLibrary library = server.CodeAnalyzersLibraries.GetCodeAnalyzersLibrary("${AppSourceCop}");


            //ALPackageSymbolsLibrary lib = new ALPackageSymbolsLibrary(alExtensionProxy,
            //    "C:\\Projects\\Sandboxes\\samplealprojects\\big\\.alpackages\\Microsoft_Application_11.0.20901.0.app");
            //lib.Load(false);

            ALSymbolInfoSyntaxTreeReader s = new ALSymbolInfoSyntaxTreeReader(alExtensionProxy, false);
            //ALSymbolInformation m = s.ProcessSourceFile("C:\\Projects\\Sandboxes\\ALProject5\\New Page.al");
            //ALSymbolInformation m = s.ProcessSourceFile(
            //    "C:\\Projects\\Sandboxes\\samplealprojects\\big\\ftest\\CardPageTest02.al");

            ALSymbolInformation m = s.ProcessSourceFile(
                "C:\\Projects\\Sandboxes\\al-test-projects\\SmallBC16\\Cod50000.MyFileMgt.al");



            /*
            DateTime t1 = DateTime.Now;
            ALSymbolInfoPackageReader appPackageReader = new ALSymbolInfoPackageReader(alExtensionProxy);
            ALAppPackage package = appPackageReader.ReadAppPackage("C:\\Projects\\Sandboxes\\samplealprojects\\big\\.alpackages\\Microsoft_Application_14.0.29581.0.app");
            DateTime t2 = DateTime.Now;
            TimeSpan t = t2 - t1;
            Console.WriteLine(t.TotalMilliseconds.ToString() + "ms");
            Console.WriteLine("Package " + package.Name);
            if (package.Tables != null)
                Console.WriteLine("Tables: " + package.Tables.Count.ToString());

            ALSymbolInformation symbol = package.ToALSymbol();              
            */

            ALPackageSymbolsCache packagesCache = new ALPackageSymbolsCache(alExtensionProxy);
            ALProjectSymbolsLibrary projectSymbols = new ALProjectSymbolsLibrary(packagesCache, 
                alExtensionProxy,
                false,
                "C:\\Projects\\Sandboxes\\AL Previews\\SampleProject",
                ".alpackages");
            projectSymbols.Load(false);

            Console.WriteLine("Hello World!");
            Console.ReadKey();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }
    }
}
