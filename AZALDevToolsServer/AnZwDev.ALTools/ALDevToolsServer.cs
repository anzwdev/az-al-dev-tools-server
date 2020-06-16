using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.CodeAnalysis;
using AnZwDev.ALTools.WorkspaceCommands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools
{
    public class ALDevToolsServer
    {

        public string ExtensionBinPath { get; }
        public ALPackageSymbolsCache AppPackagesCache { get; }
        public ALSymbolLibrariesCollection SymbolsLibraries { get; }
        public ALSyntaxTreesCollection SyntaxTrees { get; }
        public CodeAnalyzersLibrariesCollection CodeAnalyzersLibraries { get; set; }
        public WorkspaceCommandsManager WorkspaceCommandsManager { get; }

        public ALDevToolsServer(string extensionPath)
        {
            //initialize assembly loading
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            this.ExtensionBinPath = Path.Combine(extensionPath, "bin");
            this.AppPackagesCache = new ALPackageSymbolsCache();
            this.SymbolsLibraries = new ALSymbolLibrariesCollection();
            this.SyntaxTrees = new ALSyntaxTreesCollection();
            this.CodeAnalyzersLibraries = new CodeAnalyzersLibrariesCollection(this);
            this.WorkspaceCommandsManager = new WorkspaceCommandsManager();
        }

        private System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string assemblyName = args.Name;
            int pos = assemblyName.IndexOf(",");
            if (pos >= 0)
                assemblyName = assemblyName.Substring(0, pos);
            string path = System.IO.Path.Combine(this.ExtensionBinPath, assemblyName.Trim() + ".dll");
            return Assembly.LoadFrom(path);
        }
    }
}
