﻿/****************************************************************
 *                                                              *
 * Legacy version of the library maintained to support Nav 2018 *
 *                                                              *
 ****************************************************************/
using AnZwDev.ALTools.Nav2018.ALSymbols;
using AnZwDev.ALTools.Nav2018.CodeAnalysis;
using AnZwDev.ALTools.Nav2018.WorkspaceCommands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Nav2018
{
    public class ALDevToolsServer
    {

        public string ExtensionBinPath { get; }
        public ALPackageSymbolsCache AppPackagesCache { get; }
        public ALSymbolLibrariesCollection SymbolsLibraries { get; }
        public ALSyntaxTreesCollection SyntaxTrees { get; }
        public CodeAnalyzersLibrariesCollection CodeAnalyzersLibraries { get; set; }
        public WorkspaceCommandsManager WorkspaceCommandsManager { get; }
        public string PlatformSpecificFolder { get; }

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

            this.PlatformSpecificFolder = "";
        }


        private Dictionary<string, Assembly> _assembliesCache = new Dictionary<string, Assembly>();

        private System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string assemblyName = args.Name;

            int pos = assemblyName.IndexOf(",");
            if (pos >= 0)
                assemblyName = assemblyName.Substring(0, pos);

            //remove resources from assembly name
            //string resourcesName = ".resources";
            //if (assemblyName.EndsWith(resourcesName))
            //    assemblyName = assemblyName.Substring(0, assemblyName.Length - resourcesName.Length);

            if (_assembliesCache.ContainsKey(assemblyName))
                return _assembliesCache[assemblyName];

            Assembly assembly = null;

            string path;

            //load file from platform specific location, for windows we will take it from root .net framework folder
            if (String.IsNullOrWhiteSpace(this.PlatformSpecificFolder))
                path = System.IO.Path.Combine(this.ExtensionBinPath, assemblyName.Trim() + ".dll");
            else
                path = System.IO.Path.Combine(this.ExtensionBinPath, this.PlatformSpecificFolder, assemblyName.Trim() + ".dll");            
            if (File.Exists(path))           
                assembly = Assembly.LoadFrom(path);

            //try to load dll from analyzers folder, they are not platform specifica and are always in the bin\Analyzers folder
            if (assembly == null)
            {
                path = System.IO.Path.Combine(this.ExtensionBinPath, "Analyzers", assemblyName.Trim() + ".dll");
                if (File.Exists(path))
                    assembly = Assembly.LoadFrom(path);
            }

            if (assembly != null)
                _assembliesCache.Add(assemblyName, assembly);

            return assembly;
        }
    }
}