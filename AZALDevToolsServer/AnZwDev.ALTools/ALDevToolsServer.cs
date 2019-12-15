using AnZwDev.ALTools.ALProxy;
using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools
{
    public class ALDevToolsServer
    {

        public ALExtensionProxy ALExtensionProxy { get; }
        public string ExtensionBinPath { get; }
        public ALPackageSymbolsCache AppPackagesCache { get; }
        public ALSymbolLibrariesCollection SymbolsLibraries { get; }
        public ALSyntaxTreesCollection SyntaxTrees { get; }
        public CodeAnalyzersLibrariesCollection CodeAnalyzersLibraries { get; set; }

        public ALDevToolsServer(string extensionPath)
        {
            this.ExtensionBinPath = Path.Combine(extensionPath, "bin");
            this.ALExtensionProxy = new ALExtensionProxy();
            this.ALExtensionProxy.Load(this.ExtensionBinPath);
            this.AppPackagesCache = new ALPackageSymbolsCache(this.ALExtensionProxy);
            this.SymbolsLibraries = new ALSymbolLibrariesCollection();
            this.SyntaxTrees = new ALSyntaxTreesCollection(this.ALExtensionProxy);
            this.CodeAnalyzersLibraries = new CodeAnalyzersLibrariesCollection(this);
        }


    }
}
