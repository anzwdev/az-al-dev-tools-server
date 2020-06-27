using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using AnZwDev.ALTools.ALSymbols.ALAppPackages;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.ALSymbols.SymbolReaders;

namespace AnZwDev.ALTools.ALSymbols
{
    public class ALProjectSymbolsLibrary : ALSymbolsLibrary
    {

        public ALPackageSymbolsCache PackageSymbolsCache { get; }
        public List<ALSymbolsLibrary> Dependencies { get; set; }

        public bool IncludeDependencies { get; set; }
        public string Path { get; set; }
        public string PackagesPath { get; set; }
        public string FullName { get; set; }
        public string[] WorkspaceFolders { get; set; }

        public ALProjectSymbolsLibrary(ALPackageSymbolsCache cache, bool includeDependencies, string projectPath, string packagesFolder)
        {
            this.PackageSymbolsCache = cache;
            this.IncludeDependencies = includeDependencies;
            this.Path = projectPath;
            this.PackagesPath = System.IO.Path.Combine(this.Path, packagesFolder);
            this.Dependencies = new List<ALSymbolsLibrary>();
            this.WorkspaceFolders = null;
        }

        public override bool Load(bool forceReload)
        {
            this.Dependencies.Clear();

            //load project information
            ALProjectFile projectFile = ALProjectFile.Load(System.IO.Path.Combine(this.Path, "app.json"));
            this.FullName = StringExtensions.Merge(projectFile.publisher, projectFile.name, projectFile.version);

            this.Root = new ALSymbolInformation(ALSymbolKind.ProjectDefinition, this.FullName);

            if (this.IncludeDependencies)
                this.ProcessDependencies(projectFile, forceReload);

            //process project source code
            this.ProcessSourceCode();

            //return base.Load(forceReload);
            return true;
        }

        protected void ProcessDependencies(ALProjectFile projectFile, bool forceReload)
        {
            //load list of packages
            ALAppPackageFileInfosCollection packageFiles = new ALAppPackageFileInfosCollection();
            packageFiles.LoadFromFolder(this.PackagesPath);

            //collect list of projects from other folders
            WorkspaceProjectsCollection workspaceProjects = null;
            if ((this.WorkspaceFolders != null) && (this.WorkspaceFolders.Length > 1))
            {
                workspaceProjects = new WorkspaceProjectsCollection(this.WorkspaceFolders);
                workspaceProjects.Load();
            }

            bool baseAppLoaded = false;
            bool systemAppLoaded = false;

            //collect packages
            if (projectFile.dependencies != null)
            {
                for (int i = 0; i < projectFile.dependencies.Length; i++)
                {
                    bool workspaceProjectFound = false;
                    if (workspaceProjects != null)
                    {
                        WorkspaceProject depProject = workspaceProjects.FindByReference(projectFile.dependencies[i].appId,
                            projectFile.dependencies[i].publisher, projectFile.dependencies[i].name, projectFile.dependencies[i].version);
                        if (depProject != null)
                        {
                            workspaceProjectFound = true;
                            this.AddDepProject(depProject);
                        }
                    }

                    if (!workspaceProjectFound)
                        this.AddPackage(packageFiles, projectFile.dependencies[i].publisher,
                            projectFile.dependencies[i].name, projectFile.dependencies[i].version, forceReload);

                    if ((!String.IsNullOrWhiteSpace(projectFile.dependencies[i].publisher)) &&
                        (!String.IsNullOrWhiteSpace(projectFile.dependencies[i].name)) &&
                        (projectFile.dependencies[i].publisher.ToLower().Trim().Equals("microsoft")))
                    {
                        string appName = projectFile.dependencies[i].name.ToLower().Trim();
                        if (appName.Equals("system application"))
                            systemAppLoaded = true;
                        else if (appName.Equals("base application"))
                            baseAppLoaded = true;
                    }
                }
            }

            //collect system packages
            if (!String.IsNullOrWhiteSpace(projectFile.application))
            {
                this.AddPackage(packageFiles, "Microsoft", "Application", projectFile.application, forceReload);
                
                if (!baseAppLoaded)
                    this.AddPackage(packageFiles, "Microsoft", "Base Application", projectFile.application, forceReload);
                if (!systemAppLoaded)
                    this.AddPackage(packageFiles, "Microsoft", "System Application", projectFile.application, forceReload);
            }

            if (!String.IsNullOrWhiteSpace(projectFile.platform))
                this.AddPackage(packageFiles, "Microsoft", "System", projectFile.platform, forceReload);

            if (!String.IsNullOrWhiteSpace(projectFile.test))
                this.AddPackage(packageFiles, "Microsoft", "Test", projectFile.test, forceReload);

            //collect dependencies

            if (this.Dependencies.Count > 0)
            {
                ALSymbolInformation dependenciesList = new ALSymbolInformation(ALSymbolKind.Dependencies, "Dependencies");
                for (int i = 0; i < this.Dependencies.Count; i++)
                {
                    dependenciesList.AddChildSymbol(this.Dependencies[i].Root);
                }
                this.Root.AddChildSymbol(dependenciesList);
            }
        }

        protected bool AddPackage(ALAppPackageFileInfosCollection packageFiles, string publisher, string name, string version, bool forceReload)
        {
            ALAppPackageFileInfo packageFileInfo = packageFiles.Find(publisher, name, version);
            if (packageFileInfo != null)
            {
                this.Dependencies.Add(this.PackageSymbolsCache.GetSymbols(packageFileInfo.FilePath, forceReload));
                return true;
            }
            return false;
        }

        protected void AddDepProject(WorkspaceProject depProject)
        {
            ALProjectSymbolsLibrary library = new ALProjectSymbolsLibrary(this.PackageSymbolsCache, false,
                depProject.ProjectPath, this.PackagesPath);
            library.Load(false);
            if (library.Root != null)
                library.Root.kind = ALSymbolKind.Package;
            this.Dependencies.Add(library);
        }

        protected void LoadFromCompiledPackage(ALProjectFile projectFile)
        {
            string projectPackageFileName = System.IO.Path.Combine(this.Path, projectFile.publisher.NotNull() + "_" +
                projectFile.name.NotNull() + "_" +
                projectFile.version.NotNull() + ".app");
            if (File.Exists(projectPackageFileName))
            {
                ALPackageSymbolsLibrary mainSymbolsLib = this.PackageSymbolsCache.GetSymbols(projectPackageFileName, false);
                this.Root.AddChildSymbol(mainSymbolsLib.Root);
            }
            else
            {
                this.Root.AddChildSymbol(new ALSymbolInformation(ALSymbolKind.Package, "Recompile solution - project file not found"));
            }
        }

        protected void ProcessSourceCode()
        {
            ALSymbolInformation parent;
            Dictionary<ALSymbolKind, ALSymbolInformation> groupSymbolsCollection = new Dictionary<ALSymbolKind, ALSymbolInformation>();
            if (this.IncludeDependencies)
            {
                parent = new ALSymbolInformation(ALSymbolKind.Package, this.FullName);
                this.Root.AddChildSymbol(parent);
            }
            else
                parent = this.Root;

            //process source files
            ALSymbolInfoSyntaxTreeReader syntaxTreeReader = new ALSymbolInfoSyntaxTreeReader(false);
            string[] files = System.IO.Directory.GetFiles(this.Path, "*.al", SearchOption.AllDirectories);
            for (int i=0; i<files.Length; i++)
            {
                ALSymbolInformation documentSymbols = syntaxTreeReader.ProcessSourceFile(files[i]);
                if (documentSymbols.childSymbols != null)
                {
                    for (int symbolIndex = 0; symbolIndex < documentSymbols.childSymbols.Count; symbolIndex++)
                    {
                        ALSymbolInformation symbol = documentSymbols.childSymbols[symbolIndex];
                        ALSymbolKind groupSymbolKind = symbol.kind.ToGroupSymbolKind();
                        ALSymbolInformation groupSymbol = null;
                        if (groupSymbolsCollection.ContainsKey(groupSymbolKind))
                            groupSymbol = groupSymbolsCollection[groupSymbolKind];
                        else
                        {
                            groupSymbol = new ALSymbolInformation(groupSymbolKind, groupSymbolKind.ToName());
                            groupSymbolsCollection.Add(groupSymbolKind, groupSymbol);
                        }
                        groupSymbol.AddChildSymbol(symbol);
                    }
                }
            }

            //add group symbols to parent element
            if (groupSymbolsCollection.Values.Count > 0)
            {
                List<ALSymbolInformation> groups = groupSymbolsCollection.Values.OrderBy(p => p.kind).ToList();
                for (int i=0; i<groups.Count;i++)
                {
                    parent.AddChildSymbol(groups[i]);
                }
            }
        }

    }
}
