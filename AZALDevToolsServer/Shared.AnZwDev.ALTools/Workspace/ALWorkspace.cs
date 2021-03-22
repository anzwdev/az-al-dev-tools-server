﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using AnZwDev.ALTools.Core;
using AnZwDev.ALTools.ALSymbolReferences.Compiler;
using AnZwDev.ALTools.ALSymbolReferences.Serialization;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using AnZwDev.ALTools.ALSymbols;

namespace AnZwDev.ALTools.Workspace
{
    public class ALWorkspace
    {

        #region Public properties

        public List<ALProject> Projects { get; }
        public ALSymbolReferenceCompiler SymbolReferenceCompiler { get; }
        public AppPackagesSymbolReferencesCache SymbolReferencesCache { get; }

        #endregion

        public ALWorkspace()
        {
            this.Projects = new List<ALProject>();
            this.SymbolReferenceCompiler = new ALSymbolReferenceCompiler();
            this.SymbolReferencesCache = new AppPackagesSymbolReferencesCache();
        }

        #region Projects management

        public void LoadProjects(string[] workspaceFolders)
        {
            this.Projects.Clear();
            foreach (string path in workspaceFolders)
            {
                this.AddProject(path);
            }
            this.ResolveDependencies();
        }

        public void UpdateProjects(string[] addPaths, string[] removePaths)
        {
            if (removePaths != null)
            {
                for (int i = 0; i < removePaths.Length; i++)
                    this.RemoveProject(removePaths[i]);
            }
            if (addPaths != null)
            {
                for (int i = 0; i < addPaths.Length; i++)
                    this.AddProject(addPaths[i]);
            }
            this.ResolveDependencies();
        }

        protected void AddProject(string path)
        {
            ALProject project = new ALProject(this, path);
            this.Projects.Add(project);
            project.Load();
        }

        protected void RemoveProject(string path)
        {
            ALProject project = this.GetProjectByRootPath(path);
            if (project != null)
                this.Projects.Remove(project);
        }

        public ALProject GetProjectByRootPath(string path)
        {
            return this.Projects.Where(p => (p.RootPath == path)).FirstOrDefault();
        }

        public ALProject FindProject(string id, string name, string publisher, VersionNumber version)
        {
            return null;
        }

        public ALProject FindProject(string path)
        {
            return this.FindProject(path, false);
        }

        public ALProject FindProject(string path, bool selectFirstIfNotFound)
        {
            if (!String.IsNullOrWhiteSpace(path))
            {
                path = Path.GetFullPath(path);
                for (int i = 0; i < this.Projects.Count; i++)
                {
                    if (this.Projects[i].ContainsPath(path))
                        return this.Projects[i];
                }
            }
            if ((selectFirstIfNotFound) && (this.Projects.Count > 0))
                return this.Projects[0];
            return null;
        }

        #endregion

        #region Dependencies

        public void ResolveDependencies()
        {
            foreach (ALProject project in this.Projects)
            {
                project.ResolveDependencies();
            }
        }

        #endregion

        public void RebuildSymbolReferences()
        { 
            foreach (ALProject project in this.Projects)
            {
                project.RebuildSymbolReferences();
            }
        }

        #region Change tracking

        public void OnDocumentOpen(string path)
        {
            ALProject project = this.FindProject(path);
            if (project != null)
                project.OnDocumentOpen(path);
        }

        public void OnDocumentClose(string path)
        {
            ALProject project = this.FindProject(path);
            if (project != null)
                project.OnDocumentClose(path);
        }

        public void OnDocumentSave(string path)
        {
            ALProject project = this.FindProject(path);
            if (project != null)
                project.OnDocumentSave(path);
        }

        public ALSymbol OnDocumentChange(string path, string content, bool returnSymbols)
        {
            ALProject project = this.FindProject(path);
            if (project != null)
                return project.OnDocumentChange(path, content, returnSymbols);
            return null;
        }

        public void OnFilesCreate(string[] paths)
        {
            for (int i=0; i<paths.Length; i++)
            {
                ALProject project = this.FindProject(paths[i]);
                if (project != null)
                    project.OnFileCreate(paths[i]);
            }
        }

        public void OnFilesDelete(string[] paths)
        {
            for (int i = 0; i < paths.Length; i++)
            {
                ALProject project = this.FindProject(paths[i]);
                if (project != null)
                    project.OnFileDelete(paths[i]);
            }
        }

        public void OnFileRename(string oldPath, string newPath)
        {
            ALProject project = this.FindProject(oldPath);
            if (project != null)
                project.OnFileRename(oldPath, newPath);
        }


        public void OnFileSystemFileChange(string path)
        {
            ALProject project = this.FindProject(path);
            if (project != null)
                project.OnFileSystemFileChange(path);
        }

        public void OnFileSystemFileCreate(string path)
        {
            ALProject project = this.FindProject(path);
            if (project != null)
                project.OnFileSystemFileCreate(path);
        }

        public void OnFileSystemFileDelete(string path)
        {
            ALProject project = this.FindProject(path);
            if (project != null)
                project.OnFileSystemFileDelete(path);
        }

        #endregion


    }
}
